# Car State Data

Basic vehicle state information including speed, RPM, gear, pedal inputs, and engine status.

## Overview

The `carState` object contains real-time information about your vehicle's current state. This is the most fundamental telemetry data and is included in the "Basic" preset.

## Data Structure

```json
{
  "carState": {
    "SpeedKmh": 287.3,
    "Rpms": 11245,
    "Gear": "7",
    "Throttle": 1.0,
    "Brake": 0.0,
    "Clutch": 0.0,
    "CarCoordinates": [1234.5, 567.8, 90.1],
    "CurrentLapTime": 45823.5,
    "CarModel": "Mercedes-AMG GT3",
    "CarClass": "GT3",
    "EngineIgnitionOn": true,
    "EngineStarted": true
  }
}
```

## Properties

### SpeedKmh
**Type:** `double` (nullable)
**Unit:** kilometers per hour
**Range:** 0.0 - 500+ (depending on vehicle)

Current vehicle speed in kilometers per hour.

**Usage:**
```javascript
const speedKmh = data.carState.SpeedKmh;
const speedMph = speedKmh * 0.621371; // Convert to mph
```

---

### Rpms
**Type:** `double` (nullable)
**Unit:** revolutions per minute
**Range:** 0 - MaxRpm (typically 6000-18000)

Current engine RPM. Use with `MaxRpm` property to calculate RPM percentage for shift indicators.

**Usage:**
```javascript
const rpmPercentage = (data.carState.Rpms / data.vehicleInformation.MaxRpm) * 100;

if (rpmPercentage > 95) {
  console.log("SHIFT UP!");
}
```

---

### Gear
**Type:** `string` (nullable)
**Values:** `"R"`, `"N"`, `"1"`, `"2"`, `"3"`, `"4"`, `"5"`, `"6"`, `"7"`, `"8"`

Current gear. Values vary by vehicle:
- `"R"` = Reverse
- `"N"` = Neutral
- `"1"` - `"8"` = Forward gears (number depends on vehicle)

**Usage:**
```javascript
function getGearDisplay(gear) {
  if (gear === "R") return "⮌ REV";
  if (gear === "N") return "⊗ NEU";
  return gear;
}
```

---

### Throttle
**Type:** `double` (nullable)
**Unit:** percentage (0.0 - 1.0)
**Range:** 0.0 (released) to 1.0 (full throttle)

Throttle pedal position.

**Note:** This is pedal position, not engine load. Full throttle (1.0) in high gear at low RPM won't produce maximum power.

**Usage:**
```javascript
const throttlePercent = data.carState.Throttle * 100;
console.log(`Throttle: ${throttlePercent.toFixed(0)}%`);
```

---

### Brake
**Type:** `double` (nullable)
**Unit:** percentage (0.0 - 1.0)
**Range:** 0.0 (released) to 1.0 (full brake)

Brake pedal position.

**Usage:**
```javascript
// Brake indicator
if (data.carState.Brake > 0.1) {
  showBrakeLights();
}

// Trail braking detection
if (data.carState.Brake > 0 && data.carState.Throttle > 0) {
  console.log("Trail braking");
}
```

---

### Clutch
**Type:** `double` (nullable)
**Unit:** percentage (0.0 - 1.0)
**Range:** 0.0 (released) to 1.0 (fully pressed)

Clutch pedal position.

**Note:** Many modern race cars and most games have sequential gearboxes without clutch requirement.

---

### CarCoordinates
**Type:** `array<double>` (nullable)
**Format:** `[X, Y, Z]`
**Unit:** meters

3D world position of the vehicle. Useful for:
- Track position mapping
- Distance calculations between cars
- Replay systems
- Relative positioning

**Usage:**
```javascript
function calculateDistance(pos1, pos2) {
  const dx = pos2[0] - pos1[0];
  const dy = pos2[1] - pos1[1];
  const dz = pos2[2] - pos1[2];
  return Math.sqrt(dx*dx + dy*dy + dz*dz);
}
```

---

### CurrentLapTime
**Type:** `double` (nullable)
**Unit:** milliseconds
**Range:** 0 - lap duration

Time elapsed in the current lap in milliseconds.

**Usage:**
```javascript
function formatLapTime(ms) {
  const totalSeconds = ms / 1000;
  const minutes = Math.floor(totalSeconds / 60);
  const seconds = (totalSeconds % 60).toFixed(3);
  return `${minutes}:${seconds.padStart(6, '0')}`;
}

console.log(formatLapTime(data.carState.CurrentLapTime));
// Output: "1:34.567"
```

---

### CarModel
**Type:** `string` (nullable)

Name of the vehicle model.

**Examples:**
- `"Mercedes-AMG GT3"`
- `"Porsche 911 GT3 R"`
- `"Ferrari 488 GT3 Evo"`
- `"Dallara F3"`

**Note:** Naming varies by simulator. Some use full names, others use abbreviations.

---

### CarClass
**Type:** `string` (nullable)

Racing class designation.

**Examples:**
- `"GT3"`
- `"GTE"`
- `"LMP1"`
- `"F1"`
- `"Formula 3"`
- `"Touring Car"`

---

### EngineIgnitionOn
**Type:** `boolean` (nullable)
**Values:** `true` or `false`

Indicates whether the engine ignition is on. Different from `EngineStarted`:
- `EngineIgnitionOn = true` means key is in "on" position
- `EngineStarted = true` means engine is actually running

**Usage:**
```javascript
if (data.carState.EngineIgnitionOn && !data.carState.EngineStarted) {
  console.log("Starter motor active or engine stalled");
}
```

---

### EngineStarted
**Type:** `boolean` (nullable)
**Values:** `true` or `false`

Indicates whether the engine is running.

**Usage:**
```javascript
if (!data.carState.EngineStarted && data.carState.SpeedKmh > 0) {
  console.log("Rolling with engine off - coasting or being towed");
}
```

---

## Use Cases

### Speed Display
```javascript
function createSpeedometer(data) {
  const speed = data.carState?.SpeedKmh || 0;
  const maxSpeed = 400; // km/h
  const percentage = (speed / maxSpeed) * 100;

  updateSpeedDisplay(speed.toFixed(0));
  updateSpeedNeedle(percentage);
}
```

### RPM Shift Light
```javascript
function updateShiftLight(data) {
  const rpm = data.carState?.Rpms || 0;
  const maxRpm = data.vehicleInformation?.MaxRpm || 8000;
  const rpmPercent = (rpm / maxRpm) * 100;

  if (rpmPercent > 98) {
    setShiftLight('red', 'flash');
  } else if (rpmPercent > 95) {
    setShiftLight('red', 'solid');
  } else if (rpmPercent > 90) {
    setShiftLight('yellow', 'solid');
  } else {
    setShiftLight('off');
  }
}
```

### Telemetry Recording
```javascript
class TelemetryRecorder {
  constructor() {
    this.samples = [];
    this.recordingInterval = null;
  }

  startRecording() {
    this.recordingInterval = setInterval(() => {
      if (window.currentTelemetry?.carState) {
        this.samples.push({
          timestamp: Date.now(),
          speed: window.currentTelemetry.carState.SpeedKmh,
          rpm: window.currentTelemetry.carState.Rpms,
          gear: window.currentTelemetry.carState.Gear,
          throttle: window.currentTelemetry.carState.Throttle,
          brake: window.currentTelemetry.carState.Brake,
          position: [...window.currentTelemetry.carState.CarCoordinates]
        });
      }
    }, 100); // 10 Hz sampling
  }

  stopRecording() {
    clearInterval(this.recordingInterval);
    return this.samples;
  }
}
```

### Driving Style Analysis
```javascript
function analyzeDrivingStyle(carState) {
  const analysis = {
    isCoasting: carState.Throttle === 0 && carState.Brake === 0,
    isAccelerating: carState.Throttle > 0.5,
    isBraking: carState.Brake > 0.1,
    isTrailBraking: carState.Brake > 0 && carState.Throttle > 0,
    isWheelSpin: carState.Throttle > 0.8 && carState.Rpms > 10000 && carState.SpeedKmh < 100
  };

  return analysis;
}
```

## Simulator Compatibility

| Property | iRacing | ACC | AC | rFactor 2 | F1 Games |
|----------|---------|-----|----|-----------| ---------|
| SpeedKmh | ✓ | ✓ | ✓ | ✓ | ✓ |
| Rpms | ✓ | ✓ | ✓ | ✓ | ✓ |
| Gear | ✓ | ✓ | ✓ | ✓ | ✓ |
| Throttle | ✓ | ✓ | ✓ | ✓ | ✓ |
| Brake | ✓ | ✓ | ✓ | ✓ | ✓ |
| Clutch | ✓ | ✓ | ✓ | ✓ | ✓ |
| CarCoordinates | ✓ | ✓ | ✓ | ✓ | ✓ |
| CurrentLapTime | ✓ | ✓ | ✓ | ✓ | ✓ |
| CarModel | ✓ | ✓ | ✓ | ✓ | ✓ |
| CarClass | ✓ | ✓ | ✓ | ✓ | ✓ |
| EngineIgnitionOn | ✓ | ✓ | ~ | ✓ | ~ |
| EngineStarted | ✓ | ✓ | ~ | ✓ | ~ |

**Legend:** ✓ = Fully supported, ~ = Partially supported or always returns same value

## Related Documentation

- [Position & Timing Data](POSITION-TIMING.md) - Lap times and race position
- [Vehicle Information](VEHICLE-INFORMATION.md) - Static vehicle properties like MaxRpm
- [Driver Input Data](DRIVER-INPUT.md) - More detailed input information
- [Data Categories Overview](DATA-CATEGORIES.md) - All available data
