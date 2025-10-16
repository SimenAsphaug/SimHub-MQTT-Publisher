# SimHub MQTT Publisher Enhanced

A powerful and flexible SimHub plugin that publishes granular racing telemetry data to an MQTT broker in real-time. Perfect for building custom dashboards, data analysis tools, streaming overlays, and IoT integrations.

**Forked from and enhanced:** [SHWotever/SimHub-MQTT-Publisher](https://github.com/SHWotever/SimHub-MQTT-Publisher)

## Features

- **80+ Granular Telemetry Properties** - Select exactly what data you need
- **Real-time MQTT Publishing** - Low-latency data streaming to any MQTT broker
- **Quick Preset Configurations** - Basic, Racing, Strategy, and Analysis presets
- **Export/Import Settings** - Share configurations between installations
- **Connection Testing** - Built-in MQTT broker connectivity verification
- **Password Protection** - Secure credential management with show/hide toggle
- **Debug Mode** - Complete raw telemetry data for discovery and troubleshooting
- **Multi-Game Support** - Works with any simulator supported by SimHub

## Table of Contents

- [Installation](#installation)
- [Configuration](#configuration)
- [Usage](#usage)
- [Data Structure](#data-structure)
- [Use Cases](#use-cases)
- [Documentation](#documentation)
- [Troubleshooting](#troubleshooting)
- [Contributing](#contributing)
- [License](#license)

## Installation

### Requirements

- **SimHub** (version 8.0 or later recommended)
- **MQTT Broker** (Mosquitto, EMQX, HiveMQ, AWS IoT Core, Azure IoT Hub, etc.)
- **.NET Framework 4.8** (usually included with SimHub)

### Installation Steps

1. **Download the latest release** from the [Releases page](https://github.com/SimenAsphaug/SimHub-MQTT-Publisher/releases)

2. **Extract the release archive** - You'll find three DLL files:
   - `SimHub.MQTTPublisher.dll`
   - `MQTTnet.dll`
   - `Newtonsoft.Json.dll`

3. **Copy all three DLL files** to your SimHub installation directory (typically `C:\Program Files (x86)\SimHub\`)

4. **Restart SimHub** - The plugin will be automatically detected and loaded

5. **Verify installation**:
   - Open SimHub
   - Navigate to Settings → Plugins
   - Look for "MQTT Publisher Enhanced" in the plugin list

## Configuration

### Basic Setup

1. **Open SimHub** and navigate to the MQTT Publisher Enhanced plugin settings

2. **Configure MQTT Connection**:
   ```
   MQTT Server: mqtt://your-broker-address:1883
   Login: your_username (if authentication enabled)
   Password: your_password (if authentication enabled)
   Topic: simhub/telemetry (or your preferred topic)
   ```

3. **Test Connection**:
   - Click the "Test Connection" button
   - Wait for the status message (green = success, red = failure)
   - Troubleshoot any connection issues before proceeding

4. **Select Telemetry Data**:
   - Use **Quick Presets** for common configurations:
     - **Basic**: Essential telemetry (speed, RPM, gear, throttle, brake)
     - **Racing**: Racing-focused data (position, gaps, lap times, flags)
     - **Strategy**: Strategy management (fuel, tire wear, pit stops)
     - **Analysis**: Everything enabled for comprehensive analysis

   - Or **manually select** individual properties from the expandable categories

5. **Apply Settings** - Click the "Apply Settings" button to save and activate

### Advanced Configuration

#### Export/Import Settings

**Export your configuration:**
1. Click "Export Settings" button
2. Choose a location and filename (e.g., `my-racing-config.json`)
3. Settings are saved as JSON (password excluded for security)

**Import a configuration:**
1. Click "Import Settings" button
2. Select a previously exported JSON file
3. Review the imported settings
4. Click "Apply Settings" to activate

#### Debug Mode

Enable debug mode to receive ALL raw telemetry data:

1. Expand the "Advanced Debugging" section
2. Check "Enable Debug Mode (Send All Raw Telemetry)"
3. Apply settings

**Warning:** Debug mode significantly increases data volume. Use only for:
- Discovering available properties for your simulator
- Troubleshooting data issues
- Custom integration development

## Usage

### Starting Data Stream

Once configured, the plugin automatically publishes telemetry data when:
1. SimHub is running
2. A supported racing simulator is running
3. You're in an active session (practice, qualifying, race)

Data is published to your configured MQTT topic in JSON format.

### Monitoring the Data

Use any MQTT client to subscribe and view the data:

**Using mosquitto_sub (command line):**
```bash
mosquitto_sub -h your-broker-address -t simhub/telemetry -u username -P password
```

**Using MQTT Explorer (GUI):**
1. Connect to your broker
2. Subscribe to your topic (e.g., `simhub/telemetry`)
3. View real-time JSON payloads

**Using Node-RED:**
1. Add an MQTT Input node
2. Configure broker connection
3. Set topic to match your configuration
4. Process the JSON data with function nodes

## Data Structure

The plugin publishes data as JSON with the following top-level structure:

```json
{
  "time": 1704067200000,
  "userId": "unique-user-id",
  "carState": { ... },
  "flagState": { ... },
  "positionData": { ... },
  "tireData": { ... },
  "fuelData": { ... },
  "weatherData": { ... },
  "damageData": { ... },
  "inputData": { ... },
  "safetyData": { ... },
  "trackInformation": { ... },
  "vehicleInformation": { ... },
  "sessionInfo": { ... }
}
```

**Note:** Only enabled data categories appear in the payload. Null values are automatically excluded to minimize bandwidth.

### Example Output (Basic Preset)

```json
{
  "time": 1704067200000,
  "userId": "abc-123-def-456",
  "carState": {
    "SpeedKmh": 245.7,
    "Rpms": 8450,
    "Gear": "5",
    "Throttle": 0.95,
    "Brake": 0.0,
    "CurrentLapTime": 94536.2
  },
  "flagState": {
    "Flags": 4,
    "GameName": "Assetto Corsa Competizione"
  }
}
```

For detailed documentation of all data fields, see the [Documentation](#documentation) section.

## Use Cases

### Real-time Dashboard

Build custom web dashboards using:
- **MQTT.js** (JavaScript MQTT client)
- **WebSockets** (with MQTT broker WebSocket support)
- **Chart.js / D3.js** for data visualization
- **React / Vue / Angular** for UI framework

### Data Analysis

Collect telemetry for post-race analysis:
- Subscribe to MQTT topic with Python, Node.js, or any language
- Store data in database (InfluxDB, MongoDB, PostgreSQL)
- Analyze lap times, tire degradation, fuel consumption
- Generate reports and insights

### Streaming Overlays

Enhance your live streams:
- OBS Browser Source with MQTT WebSocket connection
- Display real-time telemetry on stream
- Custom graphics and animations based on race events
- Flag status indicators and position updates

### IoT Integration

Connect physical devices:
- LED strips for flag status (green = racing, yellow = caution)
- Vibration motors for tire wear warnings
- Physical gauges for RPM, speed, fuel
- Smart home integration (change room lighting based on race status)

### Discord/Telegram Bots

Create race notifications:
- Subscribe to MQTT with bot
- Send alerts for race start, finish, incidents
- Post lap time updates to channels
- Team race coordination

## Documentation

Comprehensive documentation is available in the `/docs` folder:

- **[Data Categories Overview](docs/DATA-CATEGORIES.md)** - Complete list of all available data
- **[Car State Data](docs/CAR-STATE.md)** - Speed, RPM, gear, pedal inputs, engine status
- **[Flag System](docs/FLAG-DATA.md)** - Understanding flag states and bitwise values
- **[Position & Timing](docs/POSITION-TIMING.md)** - Race position, lap times, sector times, gaps
- **[Tire Data](docs/TIRE-DATA.md)** - Temperature, pressure, wear, grip, compound
- **[Fuel & Energy](docs/FUEL-ENERGY.md)** - Fuel levels, consumption, ERS, DRS, battery
- **[Weather & Track Conditions](docs/WEATHER-CONDITIONS.md)** - Temperature, rain, wind, grip
- **[Damage & Mechanical](docs/DAMAGE-MECHANICAL.md)** - Car damage, temperatures, wear indicators
- **[Driver Input](docs/DRIVER-INPUT.md)** - Steering, pedals, assists, electronic systems
- **[Safety & Race Control](docs/SAFETY-CONTROL.md)** - Safety car, pit status, penalties, formation laps
- **[Integration Examples](docs/INTEGRATION-EXAMPLES.md)** - Code samples for common use cases

## Troubleshooting

### Connection Issues

**"Connection timeout" error:**
- Verify broker address and port (default MQTT port is 1883)
- Check firewall rules allow outbound connections on port 1883
- Ensure MQTT broker is running and accessible
- Try connecting with mosquitto_sub to verify broker availability

**"Connection failed" error:**
- Check username and password if authentication is enabled
- Verify broker supports MQTT 3.1.1 protocol
- Check broker logs for connection rejection reasons
- Ensure SSL/TLS settings match broker configuration (this plugin uses non-SSL by default)

### No Data Received

**Subscribed to topic but receiving no messages:**
- Confirm a racing simulator is running and in an active session
- Verify at least one telemetry option is enabled
- Check topic name matches exactly (MQTT is case-sensitive)
- Ensure "Apply Settings" was clicked after configuration
- Look for errors in SimHub logs

**Data is missing expected fields:**
- Not all simulators provide all telemetry properties
- Enable Debug Mode temporarily to see all available properties for your sim
- Some properties are only available during specific conditions (e.g., DRS in F1 games)

### Performance Issues

**SimHub or game stuttering:**
- Reduce the number of enabled telemetry properties
- Check MQTT broker can handle the message rate
- Consider using MQTT QoS level 0 for better performance
- Verify network connection is stable

**High bandwidth usage:**
- Disable unnecessary telemetry categories
- Avoid Debug Mode unless needed
- Consider implementing message throttling in your client application

### Data Format Issues

**Parsing JSON fails:**
- Ensure your client expects null values may be omitted
- Check for partial messages (unlikely but possible with network issues)
- Validate JSON structure with online JSON validator
- Some fields may return unexpected types depending on simulator

## Contributing

Contributions are welcome! Please feel free to:

- Report bugs via GitHub Issues
- Suggest features or enhancements
- Submit pull requests with improvements
- Share your use cases and integrations
- Improve documentation

## Version History

**v1.0.0** (Current)
- Complete rewrite with 80+ granular telemetry properties
- Quick preset configurations (Basic, Racing, Strategy, Analysis)
- Export/Import settings functionality
- Connection testing
- Password visibility toggle
- Debug mode for raw telemetry discovery
- Comprehensive documentation
- Multi-simulator support improvements

## License

MIT License

Original project by SHWotever - Enhanced by Simen Asphaug

## Acknowledgments

- **SHWotever** for creating the original SimHub MQTT Publisher
- **SimHub** team for the excellent racing telemetry platform
- **MQTT.NET** for the robust MQTT client library
- All contributors and users of this enhanced version

## Support

For issues, questions, or suggestions:
- Open an issue on GitHub
- Check existing documentation in the `/docs` folder
- Review closed issues for similar problems

---

**Happy Racing!** 🏁
