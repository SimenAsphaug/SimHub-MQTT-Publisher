# Integration Examples

Practical code examples for integrating SimHub MQTT telemetry into your applications.

## Table of Contents

- [JavaScript/Node.js](#javascriptnodejs)
- [Python](#python)
- [Web Dashboard (HTML/JavaScript)](#web-dashboard-htmljavascript)
- [Node-RED](#node-red)
- [Home Assistant](#home-assistant)
- [Discord Bot](#discord-bot)
- [InfluxDB Data Logging](#influxdb-data-logging)

---

## JavaScript/Node.js

### Basic MQTT Client

```javascript
const mqtt = require('mqtt');

// Connect to MQTT broker
const client = mqtt.connect('mqtt://your-broker-address:1883', {
  username: 'your_username',
  password: 'your_password'
});

client.on('connect', () => {
  console.log('Connected to MQTT broker');

  // Subscribe to telemetry topic
  client.subscribe('simhub/telemetry', (err) => {
    if (!err) {
      console.log('Subscribed to simhub/telemetry');
    }
  });
});

client.on('message', (topic, message) => {
  try {
    const data = JSON.parse(message.toString());

    console.log('=== Telemetry Update ===');
    console.log(`Speed: ${data.carState?.SpeedKmh?.toFixed(1)} km/h`);
    console.log(`RPM: ${data.carState?.Rpms?.toFixed(0)}`);
    console.log(`Gear: ${data.carState?.Gear}`);
    console.log(`Position: ${data.positionData?.Position}`);
    console.log(`Lap: ${data.positionData?.CurrentLap}/${data.positionData?.TotalLaps}`);

    // Check for flags
    if (data.flagState?.Flags) {
      checkFlags(data.flagState.Flags);
    }
  } catch (error) {
    console.error('Error parsing telemetry:', error);
  }
});

function checkFlags(flags) {
  if (flags & 16) console.log('🔴 RED FLAG');
  if (flags & 8) console.log('🟡 YELLOW FLAG');
  if (flags & 4) console.log('🟢 GREEN FLAG');
  if (flags & 1) console.log('🏁 CHECKERED FLAG');
}

client.on('error', (error) => {
  console.error('MQTT Error:', error);
});
```

### Advanced Data Processing

```javascript
const mqtt = require('mqtt');

class TelemetryMonitor {
  constructor(brokerUrl, options) {
    this.client = mqtt.connect(brokerUrl, options);
    this.previousData = null;
    this.lapStartTime = null;
    this.bestLapTime = Infinity;

    this.setupHandlers();
  }

  setupHandlers() {
    this.client.on('connect', () => {
      console.log('✓ Connected to MQTT broker');
      this.client.subscribe('simhub/telemetry');
    });

    this.client.on('message', (topic, message) => {
      const data = JSON.parse(message.toString());
      this.processData(data);
    });
  }

  processData(data) {
    // Detect new lap
    if (this.previousData && this.hasCompletedLap(data)) {
      const lapTime = data.positionData.LastLapTime;
      this.onLapCompleted(lapTime, data);
    }

    // Monitor fuel consumption
    if (data.fuelData) {
      this.monitorFuel(data.fuelData);
    }

    // Check tire temperatures
    if (data.tireData?.TireTemperatures) {
      this.checkTireTemp(data.tireData.TireTemperatures);
    }

    this.previousData = data;
  }

  hasCompletedLap(data) {
    return (
      data.positionData?.CurrentLap >
      this.previousData?.positionData?.CurrentLap
    );
  }

  onLapCompleted(lapTime, data) {
    console.log(`\n🏁 Lap ${data.positionData.CurrentLap - 1} completed`);
    console.log(`   Time: ${this.formatLapTime(lapTime)}`);

    if (lapTime < this.bestLapTime) {
      this.bestLapTime = lapTime;
      console.log(`   ✨ NEW BEST LAP! ✨`);
    }

    // Calculate delta to best
    const delta = lapTime - this.bestLapTime;
    if (delta > 0) {
      console.log(`   Delta to best: +${delta.toFixed(3)}s`);
    }

    // Fuel info
    if (data.fuelData) {
      console.log(`   Fuel remaining: ${data.fuelData.FuelRemaining?.toFixed(2)}L`);
      console.log(`   Laps remaining: ${data.fuelData.FuelEstimatedLaps?.toFixed(1)}`);
    }
  }

  monitorFuel(fuelData) {
    if (fuelData.FuelEstimatedLaps < 3) {
      console.warn(`⚠️  LOW FUEL: ${fuelData.FuelEstimatedLaps.toFixed(1)} laps remaining`);
    }
  }

  checkTireTemp(temps) {
    const avgTemp = temps.reduce((a, b) => a + b, 0) / temps.length;

    if (avgTemp > 105) {
      console.warn(`🔥 HIGH TIRE TEMPS: ${avgTemp.toFixed(1)}°C average`);
    } else if (avgTemp < 70) {
      console.warn(`❄️  LOW TIRE TEMPS: ${avgTemp.toFixed(1)}°C average`);
    }
  }

  formatLapTime(seconds) {
    const mins = Math.floor(seconds / 60);
    const secs = (seconds % 60).toFixed(3);
    return `${mins}:${secs.padStart(6, '0')}`;
  }
}

// Usage
const monitor = new TelemetryMonitor('mqtt://your-broker:1883', {
  username: 'your_username',
  password: 'your_password'
});
```

---

## Python

### Basic MQTT Client

```python
import json
import paho.mqtt.client as mqtt

def on_connect(client, userdata, flags, rc):
    if rc == 0:
        print("Connected to MQTT broker")
        client.subscribe("simhub/telemetry")
    else:
        print(f"Connection failed with code {rc}")

def on_message(client, userdata, msg):
    try:
        data = json.loads(msg.payload.decode())

        # Extract telemetry
        car_state = data.get('carState', {})
        position_data = data.get('positionData', {})
        flag_state = data.get('flagState', {})

        print("=== Telemetry Update ===")
        print(f"Speed: {car_state.get('SpeedKmh', 0):.1f} km/h")
        print(f"RPM: {car_state.get('Rpms', 0):.0f}")
        print(f"Gear: {car_state.get('Gear', 'N')}")
        print(f"Position: {position_data.get('Position', '-')}")
        print(f"Lap: {position_data.get('CurrentLap', 0)}/{position_data.get('TotalLaps', 0)}")

        # Check flags
        flags = flag_state.get('Flags', 0)
        if flags & 16:
            print("🔴 RED FLAG")
        elif flags & 8:
            print("🟡 YELLOW FLAG")
        elif flags & 4:
            print("🟢 GREEN FLAG")
        if flags & 1:
            print("🏁 CHECKERED FLAG")

    except json.JSONDecodeError as e:
        print(f"Error parsing JSON: {e}")
    except Exception as e:
        print(f"Error processing message: {e}")

# Create MQTT client
client = mqtt.Client()
client.username_pw_set("your_username", "your_password")

# Attach callbacks
client.on_connect = on_connect
client.on_message = on_message

# Connect to broker
client.connect("your-broker-address", 1883, 60)

# Start loop
print("Starting telemetry monitor...")
client.loop_forever()
```

### Data Logging to CSV

```python
import json
import csv
from datetime import datetime
import paho.mqtt.client as mqtt

class TelemetryLogger:
    def __init__(self, filename="telemetry_log.csv"):
        self.filename = filename
        self.csv_file = None
        self.csv_writer = None
        self.initialize_csv()

    def initialize_csv(self):
        self.csv_file = open(self.filename, 'w', newline='')
        self.csv_writer = csv.writer(self.csv_file)

        # Write header
        self.csv_writer.writerow([
            'Timestamp', 'Speed_kmh', 'RPM', 'Gear', 'Throttle', 'Brake',
            'Position', 'CurrentLap', 'LapTime_ms', 'TireTemp_FL', 'TireTemp_FR',
            'TireTemp_RL', 'TireTemp_RR', 'Fuel_L', 'Flags'
        ])

    def log_telemetry(self, data):
        timestamp = datetime.now().isoformat()
        car_state = data.get('carState', {})
        position_data = data.get('positionData', {})
        tire_data = data.get('tireData', {})
        fuel_data = data.get('fuelData', {})
        flag_state = data.get('flagState', {})

        tire_temps = tire_data.get('TireTemperatures', [0, 0, 0, 0])

        row = [
            timestamp,
            car_state.get('SpeedKmh', 0),
            car_state.get('Rpms', 0),
            car_state.get('Gear', 'N'),
            car_state.get('Throttle', 0),
            car_state.get('Brake', 0),
            position_data.get('Position', 0),
            position_data.get('CurrentLap', 0),
            car_state.get('CurrentLapTime', 0),
            tire_temps[0] if len(tire_temps) > 0 else 0,
            tire_temps[1] if len(tire_temps) > 1 else 0,
            tire_temps[2] if len(tire_temps) > 2 else 0,
            tire_temps[3] if len(tire_temps) > 3 else 0,
            fuel_data.get('Fuel', 0),
            flag_state.get('Flags', 0)
        ]

        self.csv_writer.writerow(row)
        self.csv_file.flush()

    def close(self):
        if self.csv_file:
            self.csv_file.close()

# MQTT callbacks
logger = TelemetryLogger()

def on_connect(client, userdata, flags, rc):
    print(f"Connected with result code {rc}")
    client.subscribe("simhub/telemetry")

def on_message(client, userdata, msg):
    try:
        data = json.loads(msg.payload.decode())
        logger.log_telemetry(data)
        print(f"Logged telemetry at {datetime.now()}")
    except Exception as e:
        print(f"Error: {e}")

# Setup MQTT client
client = mqtt.Client()
client.username_pw_set("your_username", "your_password")
client.on_connect = on_connect
client.on_message = on_message

try:
    client.connect("your-broker-address", 1883, 60)
    print("Starting telemetry logger...")
    client.loop_forever()
except KeyboardInterrupt:
    print("\nStopping logger...")
    logger.close()
    client.disconnect()
```

---

## Web Dashboard (HTML/JavaScript)

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>SimHub Telemetry Dashboard</title>
    <script src="https://unpkg.com/mqtt/dist/mqtt.min.js"></script>
    <style>
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: #1a1a1a;
            color: #ffffff;
            padding: 20px;
            margin: 0;
        }
        .dashboard {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
            gap: 20px;
            max-width: 1400px;
            margin: 0 auto;
        }
        .card {
            background: #2a2a2a;
            border-radius: 10px;
            padding: 20px;
            box-shadow: 0 4px 6px rgba(0,0,0,0.3);
        }
        .card h2 {
            margin-top: 0;
            color: #4CAF50;
            font-size: 18px;
            border-bottom: 2px solid #4CAF50;
            padding-bottom: 10px;
        }
        .value {
            font-size: 48px;
            font-weight: bold;
            margin: 20px 0;
            text-align: center;
        }
        .label {
            font-size: 14px;
            color: #888;
            text-transform: uppercase;
        }
        .flag-indicator {
            padding: 15px;
            border-radius: 5px;
            text-align: center;
            font-weight: bold;
            font-size: 20px;
            margin-top: 10px;
        }
        .flag-green { background: #4CAF50; color: white; }
        .flag-yellow { background: #FFC107; color: black; }
        .flag-red { background: #F44336; color: white; }
        .flag-blue { background: #2196F3; color: white; }
        .tire-grid {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 10px;
            margin-top: 15px;
        }
        .tire {
            background: #3a3a3a;
            padding: 15px;
            border-radius: 5px;
            text-align: center;
        }
        .tire-temp {
            font-size: 24px;
            font-weight: bold;
        }
        .connection-status {
            position: fixed;
            top: 10px;
            right: 10px;
            padding: 10px 20px;
            border-radius: 5px;
            font-weight: bold;
        }
        .connected { background: #4CAF50; }
        .disconnected { background: #F44336; }
    </style>
</head>
<body>
    <div id="status" class="connection-status disconnected">Disconnected</div>

    <div class="dashboard">
        <div class="card">
            <h2>Speed</h2>
            <div class="value" id="speed">0</div>
            <div class="label">KM/H</div>
        </div>

        <div class="card">
            <h2>RPM</h2>
            <div class="value" id="rpm">0</div>
            <div class="label">REVOLUTIONS</div>
        </div>

        <div class="card">
            <h2>Gear</h2>
            <div class="value" id="gear">N</div>
            <div class="label">CURRENT GEAR</div>
        </div>

        <div class="card">
            <h2>Position</h2>
            <div class="value" id="position">-</div>
            <div class="label">RACE POSITION</div>
        </div>

        <div class="card">
            <h2>Lap</h2>
            <div class="value" id="lap">0 / 0</div>
            <div class="label">CURRENT / TOTAL</div>
        </div>

        <div class="card">
            <h2>Lap Time</h2>
            <div class="value" id="laptime">0:00.000</div>
            <div class="label">CURRENT LAP</div>
        </div>

        <div class="card">
            <h2>Flags</h2>
            <div id="flags" class="flag-indicator" style="background: #3a3a3a;">
                NO FLAG
            </div>
        </div>

        <div class="card">
            <h2>Tire Temperatures</h2>
            <div class="tire-grid">
                <div class="tire">
                    <div class="label">FL</div>
                    <div class="tire-temp" id="tire-fl">--</div>
                </div>
                <div class="tire">
                    <div class="label">FR</div>
                    <div class="tire-temp" id="tire-fr">--</div>
                </div>
                <div class="tire">
                    <div class="label">RL</div>
                    <div class="tire-temp" id="tire-rl">--</div>
                </div>
                <div class="tire">
                    <div class="label">RR</div>
                    <div class="tire-temp" id="tire-rr">--</div>
                </div>
            </div>
        </div>

        <div class="card">
            <h2>Fuel</h2>
            <div class="value" id="fuel">0.0</div>
            <div class="label">LITERS REMAINING</div>
            <div style="margin-top: 15px; text-align: center;">
                <div class="label">Laps Remaining</div>
                <div style="font-size: 24px; font-weight: bold;" id="fuel-laps">0.0</div>
            </div>
        </div>
    </div>

    <script>
        // MQTT Connection
        const client = mqtt.connect('ws://your-broker-address:9001', {
            username: 'your_username',
            password: 'your_password'
        });

        const statusEl = document.getElementById('status');

        client.on('connect', () => {
            statusEl.textContent = 'Connected';
            statusEl.className = 'connection-status connected';
            client.subscribe('simhub/telemetry');
        });

        client.on('disconnect', () => {
            statusEl.textContent = 'Disconnected';
            statusEl.className = 'connection-status disconnected';
        });

        client.on('message', (topic, message) => {
            const data = JSON.parse(message.toString());
            updateDashboard(data);
        });

        function updateDashboard(data) {
            // Car State
            if (data.carState) {
                document.getElementById('speed').textContent =
                    (data.carState.SpeedKmh || 0).toFixed(0);
                document.getElementById('rpm').textContent =
                    (data.carState.Rpms || 0).toFixed(0);
                document.getElementById('gear').textContent =
                    data.carState.Gear || 'N';

                // Lap time
                const lapTimeMs = data.carState.CurrentLapTime || 0;
                document.getElementById('laptime').textContent =
                    formatLapTime(lapTimeMs / 1000);
            }

            // Position Data
            if (data.positionData) {
                document.getElementById('position').textContent =
                    data.positionData.Position || '-';
                document.getElementById('lap').textContent =
                    `${data.positionData.CurrentLap || 0} / ${data.positionData.TotalLaps || 0}`;
            }

            // Flags
            if (data.flagState) {
                updateFlags(data.flagState.Flags || 0);
            }

            // Tires
            if (data.tireData && data.tireData.TireTemperatures) {
                const temps = data.tireData.TireTemperatures;
                document.getElementById('tire-fl').textContent = temps[0]?.toFixed(1) + '°C' || '--';
                document.getElementById('tire-fr').textContent = temps[1]?.toFixed(1) + '°C' || '--';
                document.getElementById('tire-rl').textContent = temps[2]?.toFixed(1) + '°C' || '--';
                document.getElementById('tire-rr').textContent = temps[3]?.toFixed(1) + '°C' || '--';
            }

            // Fuel
            if (data.fuelData) {
                document.getElementById('fuel').textContent =
                    (data.fuelData.FuelRemaining || 0).toFixed(1);
                document.getElementById('fuel-laps').textContent =
                    (data.fuelData.FuelEstimatedLaps || 0).toFixed(1);
            }
        }

        function updateFlags(flags) {
            const flagEl = document.getElementById('flags');

            if (flags & 16) {
                flagEl.textContent = '🔴 RED FLAG';
                flagEl.className = 'flag-indicator flag-red';
            } else if (flags & 8) {
                flagEl.textContent = '🟡 YELLOW FLAG';
                flagEl.className = 'flag-indicator flag-yellow';
            } else if (flags & 32) {
                flagEl.textContent = '🔵 BLUE FLAG';
                flagEl.className = 'flag-indicator flag-blue';
            } else if (flags & 4) {
                flagEl.textContent = '🟢 GREEN FLAG';
                flagEl.className = 'flag-indicator flag-green';
            } else if (flags & 1) {
                flagEl.textContent = '🏁 CHECKERED';
                flagEl.className = 'flag-indicator';
                flagEl.style.background = 'repeating-linear-gradient(45deg, #000, #000 10px, #fff 10px, #fff 20px)';
            } else {
                flagEl.textContent = 'NO FLAG';
                flagEl.className = 'flag-indicator';
                flagEl.style.background = '#3a3a3a';
            }
        }

        function formatLapTime(seconds) {
            const mins = Math.floor(seconds / 60);
            const secs = (seconds % 60).toFixed(3);
            return `${mins}:${secs.padStart(6, '0')}`;
        }
    </script>
</body>
</html>
```

**Note:** This example requires WebSocket support on your MQTT broker. Most brokers support WebSockets on port 9001.

---

## Node-RED

Node-RED flow for processing SimHub telemetry:

```json
[
    {
        "id": "mqtt-in",
        "type": "mqtt in",
        "name": "SimHub Telemetry",
        "topic": "simhub/telemetry",
        "broker": "your-mqtt-broker",
        "outputs": 1,
        "x": 120,
        "y": 100
    },
    {
        "id": "json-parse",
        "type": "json",
        "name": "Parse JSON",
        "property": "payload",
        "x": 310,
        "y": 100
    },
    {
        "id": "extract-speed",
        "type": "function",
        "name": "Extract Speed",
        "func": "msg.payload = {\n    speed: msg.payload.carState?.SpeedKmh || 0,\n    timestamp: msg.payload.time\n};\nreturn msg;",
        "x": 510,
        "y": 60
    },
    {
        "id": "check-flags",
        "type": "function",
        "name": "Check for Yellow Flag",
        "func": "const flags = msg.payload.flagState?.Flags || 0;\nif (flags & 8) {\n    msg.payload = {\n        alert: 'YELLOW FLAG',\n        time: new Date().toISOString()\n    };\n    return msg;\n}\nreturn null;",
        "x": 510,
        "y": 140
    },
    {
        "id": "debug-speed",
        "type": "debug",
        "name": "Speed Output",
        "x": 710,
        "y": 60
    },
    {
        "id": "debug-flag",
        "type": "debug",
        "name": "Flag Alert",
        "x": 710,
        "y": 140
    }
]
```

---

## Home Assistant

MQTT sensor configuration for Home Assistant:

```yaml
# configuration.yaml

mqtt:
  sensor:
    - name: "SimHub Speed"
      state_topic: "simhub/telemetry"
      value_template: "{{ value_json.carState.SpeedKmh | round(1) }}"
      unit_of_measurement: "km/h"
      icon: "mdi:speedometer"

    - name: "SimHub RPM"
      state_topic: "simhub/telemetry"
      value_template: "{{ value_json.carState.Rpms | round(0) }}"
      unit_of_measurement: "RPM"
      icon: "mdi:engine"

    - name: "SimHub Gear"
      state_topic: "simhub/telemetry"
      value_template: "{{ value_json.carState.Gear }}"
      icon: "mdi:car-shift-pattern"

    - name: "SimHub Position"
      state_topic: "simhub/telemetry"
      value_template: "{{ value_json.positionData.Position }}"
      icon: "mdi:podium"

    - name: "SimHub Fuel"
      state_topic: "simhub/telemetry"
      value_template: "{{ value_json.fuelData.FuelRemaining | round(1) }}"
      unit_of_measurement: "L"
      icon: "mdi:gas-station"

  binary_sensor:
    - name: "SimHub Yellow Flag"
      state_topic: "simhub/telemetry"
      value_template: "{{ (value_json.flagState.Flags | int & 8) != 0 }}"
      payload_on: true
      payload_off: false
      device_class: "safety"

    - name: "SimHub Red Flag"
      state_topic: "simhub/telemetry"
      value_template: "{{ (value_json.flagState.Flags | int & 16) != 0 }}"
      payload_on: true
      payload_off: false
      device_class: "safety"
```

Automation example - turn lights yellow during caution:

```yaml
# automations.yaml

- alias: "Racing Yellow Flag Lights"
  trigger:
    - platform: state
      entity_id: binary_sensor.simhub_yellow_flag
      to: "on"
  action:
    - service: light.turn_on
      target:
        entity_id: light.room_lights
      data:
        rgb_color: [255, 255, 0]
        brightness: 255

- alias: "Racing Green Flag Lights"
  trigger:
    - platform: state
      entity_id: binary_sensor.simhub_yellow_flag
      to: "off"
  action:
    - service: light.turn_on
      target:
        entity_id: light.room_lights
      data:
        rgb_color: [0, 255, 0]
        brightness: 128
```

---

## Discord Bot

Simple Discord bot that posts lap times:

```javascript
const { Client, GatewayIntentBits } = require('discord.js');
const mqtt = require('mqtt');

const discordClient = new Client({
  intents: [GatewayIntentBits.Guilds, GatewayIntentBits.GuildMessages]
});

const mqttClient = mqtt.connect('mqtt://your-broker:1883', {
  username: 'your_username',
  password: 'your_password'
});

let channelId = 'YOUR_CHANNEL_ID';
let previousLap = 0;

discordClient.once('ready', () => {
  console.log(`Discord bot logged in as ${discordClient.user.tag}`);
});

mqttClient.on('connect', () => {
  console.log('Connected to MQTT broker');
  mqttClient.subscribe('simhub/telemetry');
});

mqttClient.on('message', async (topic, message) => {
  try {
    const data = JSON.parse(message.toString());
    const currentLap = data.positionData?.CurrentLap;

    // Check if lap was completed
    if (currentLap && currentLap > previousLap && previousLap > 0) {
      const lastLapTime = data.positionData?.LastLapTime;
      const position = data.positionData?.Position;

      if (lastLapTime) {
        const channel = await discordClient.channels.fetch(channelId);
        const lapTimeFormatted = formatLapTime(lastLapTime);

        await channel.send(
          `🏁 Lap ${previousLap} completed!\n` +
          `⏱️ Time: ${lapTimeFormatted}\n` +
          `📍 Position: P${position || '?'}`
        );
      }
    }

    previousLap = currentLap || 0;
  } catch (error) {
    console.error('Error:', error);
  }
});

function formatLapTime(seconds) {
  const mins = Math.floor(seconds / 60);
  const secs = (seconds % 60).toFixed(3);
  return `${mins}:${secs.padStart(6, '0')}`;
}

discordClient.login('YOUR_DISCORD_BOT_TOKEN');
```

---

## InfluxDB Data Logging

Store telemetry in InfluxDB for analysis:

```javascript
const mqtt = require('mqtt');
const { InfluxDB, Point } = require('@influxdata/influxdb-client');

// InfluxDB setup
const influxUrl = 'http://localhost:8086';
const token = 'your-influxdb-token';
const org = 'your-org';
const bucket = 'simhub';

const influxClient = new InfluxDB({ url: influxUrl, token: token });
const writeApi = influxClient.getWriteApi(org, bucket);

// MQTT setup
const mqttClient = mqtt.connect('mqtt://your-broker:1883', {
  username: 'your_username',
  password: 'your_password'
});

mqttClient.on('connect', () => {
  console.log('Connected to MQTT broker');
  mqttClient.subscribe('simhub/telemetry');
});

mqttClient.on('message', (topic, message) => {
  try {
    const data = JSON.parse(message.toString());

    // Car state point
    if (data.carState) {
      const point = new Point('car_state')
        .floatField('speed_kmh', data.carState.SpeedKmh || 0)
        .floatField('rpm', data.carState.Rpms || 0)
        .stringField('gear', data.carState.Gear || 'N')
        .floatField('throttle', data.carState.Throttle || 0)
        .floatField('brake', data.carState.Brake || 0);

      writeApi.writePoint(point);
    }

    // Position data point
    if (data.positionData) {
      const point = new Point('position_data')
        .intField('position', data.positionData.Position || 0)
        .intField('current_lap', data.positionData.CurrentLap || 0)
        .floatField('last_lap_time', data.positionData.LastLapTime || 0)
        .floatField('gap_to_leader', data.positionData.GapToLeader || 0);

      writeApi.writePoint(point);
    }

    // Tire data point
    if (data.tireData && data.tireData.TireTemperatures) {
      const temps = data.tireData.TireTemperatures;
      const point = new Point('tire_data')
        .floatField('temp_fl', temps[0] || 0)
        .floatField('temp_fr', temps[1] || 0)
        .floatField('temp_rl', temps[2] || 0)
        .floatField('temp_rr', temps[3] || 0);

      writeApi.writePoint(point);
    }

    // Flush data
    writeApi.flush();

  } catch (error) {
    console.error('Error processing telemetry:', error);
  }
});

// Handle shutdown
process.on('SIGINT', async () => {
  console.log('\nClosing connections...');
  await writeApi.close();
  mqttClient.end();
  process.exit(0);
});
```

Query example in Flux:

```flux
from(bucket: "simhub")
  |> range(start: -1h)
  |> filter(fn: (r) => r._measurement == "car_state")
  |> filter(fn: (r) => r._field == "speed_kmh")
  |> aggregateWindow(every: 1s, fn: mean)
```

---

## Additional Resources

- [MQTT.js Documentation](https://github.com/mqttjs/MQTT.js)
- [Paho MQTT Python Documentation](https://www.eclipse.org/paho/index.php?page=clients/python/index.php)
- [Node-RED MQTT Nodes](https://nodered.org/docs/user-guide/nodes#mqtt)
- [Home Assistant MQTT Integration](https://www.home-assistant.io/integrations/mqtt/)
- [InfluxDB Client Libraries](https://docs.influxdata.com/influxdb/cloud/api-guide/client-libraries/)
