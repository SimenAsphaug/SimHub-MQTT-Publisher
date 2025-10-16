# Data Categories Overview

Complete reference of all 80+ telemetry properties available in the SimHub MQTT Publisher Enhanced plugin.

## Table of Contents

- [Payload Structure](#payload-structure)
- [Car State](#car-state)
- [Flag Information](#flag-information)
- [Position & Timing Data](#position--timing-data)
- [Tire Data](#tire-data)
- [Fuel & Energy Management](#fuel--energy-management)
- [Weather & Track Conditions](#weather--track-conditions)
- [Damage & Mechanical](#damage--mechanical)
- [Driver Input](#driver-input)
- [Safety & Race Control](#safety--race-control)
- [Track Information](#track-information)
- [Vehicle Information](#vehicle-information)
- [Session Information](#session-information)
- [Debug Mode](#debug-mode)

## Payload Structure

Every MQTT message contains a JSON payload with the following top-level properties:

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
  "sessionInfo": { ... },
  "debugData": { ... }
}
```

### Root Properties

| Property | Type | Description |
|----------|------|-------------|
| `time` | `integer` | Unix timestamp in milliseconds when data was captured |
| `userId` | `string` | Unique identifier for the SimHub installation |

**Note:** Only categories with enabled properties will appear in the payload. Empty categories are omitted entirely.

---

## Car State

Basic vehicle state information - speed, RPM, gear, and pedal inputs.

**Payload Key:** `carState`

| Property | Type | Unit | Description |
|----------|------|------|-------------|
| `SpeedKmh` | `double` | km/h | Current vehicle speed |
| `Rpms` | `double` | RPM | Engine revolutions per minute |
| `Gear` | `string` | - | Current gear (R, N, 1-8) |
| `Throttle` | `double` | 0.0-1.0 | Throttle pedal position (0 = released, 1 = full) |
| `Brake` | `double` | 0.0-1.0 | Brake pedal position (0 = released, 1 = full) |
| `Clutch` | `double` | 0.0-1.0 | Clutch pedal position (0 = released, 1 = pressed) |
| `CarCoordinates` | `array<double>` | meters | 3D position [X, Y, Z] in world space |
| `CurrentLapTime` | `double` | ms | Time elapsed in current lap |
| `CarModel` | `string` | - | Name of the car/vehicle model |
| `CarClass` | `string` | - | Vehicle class designation |
| `EngineIgnitionOn` | `boolean` | - | Engine ignition status |
| `EngineStarted` | `boolean` | - | Engine running status |

**Example:**
```json
{
  "carState": {
    "SpeedKmh": 287.3,
    "Rpms": 11245,
    "Gear": "7",
    "Throttle": 1.0,
    "Brake": 0.0,
    "Clutch": 0.0,
    "CurrentLapTime": 45823.5,
    "CarModel": "Mercedes-AMG GT3",
    "CarClass": "GT3",
    "EngineIgnitionOn": true,
    "EngineStarted": true
  }
}
```

**See also:** [Car State Documentation](CAR-STATE.md)

---

## Flag Information

Race flags and track status indicators.

**Payload Key:** `flagState`

| Property | Type | Description |
|----------|------|-------------|
| `Flags` | `integer` | Bitwise flag state (see flag documentation) |
| `FlagName` | `string` | Human-readable flag name |
| `GameName` | `string` | Racing simulator name |
| `DebugFlags` | `object` | Individual flag states (debug mode) |

**Example:**
```json
{
  "flagState": {
    "Flags": 4,
    "FlagName": "Green",
    "GameName": "Assetto Corsa Competizione"
  }
}
```

**See also:** [Flag Data Documentation](FLAG-DATA.md)

---

## Position & Timing Data

Race position, lap times, sector times, and gaps to other drivers.

**Payload Key:** `positionData`

### Position Information
| Property | Type | Description |
|----------|------|-------------|
| `Position` | `integer` | Overall race position |
| `PositionInClass` | `integer` | Position within vehicle class |

### Gap Information
| Property | Type | Unit | Description |
|----------|------|------|-------------|
| `Gap` | `double` | seconds | Gap to position ahead |
| `GapToLeader` | `double` | seconds | Gap to race leader |
| `GapToAhead` | `double` | seconds | Gap to car directly ahead |
| `GapToBehind` | `double` | seconds | Gap to car directly behind |

### Lap Times
| Property | Type | Unit | Description |
|----------|------|------|-------------|
| `LastLapTime` | `double` | seconds | Previous lap time |
| `BestLapTime` | `double` | seconds | Driver's best lap time |
| `PersonalBestLapTime` | `double` | seconds | All-time personal best for track/car |
| `SessionBestLapTime` | `double` | seconds | Fastest lap in current session |

### Delta Times
| Property | Type | Unit | Description |
|----------|------|------|-------------|
| `DeltaToSessionBest` | `double` | seconds | Delta to session best lap |
| `DeltaToPersonalBest` | `double` | seconds | Delta to personal best lap |
| `DeltaToOptimal` | `double` | seconds | Delta to theoretical optimal lap |

### Sector Times
| Property | Type | Unit | Description |
|----------|------|------|-------------|
| `Sector1Time` | `double` | seconds | Current lap sector 1 time |
| `Sector2Time` | `double` | seconds | Current lap sector 2 time |
| `Sector3Time` | `double` | seconds | Current lap sector 3 time |
| `Sector1BestTime` | `double` | seconds | Best sector 1 time |
| `Sector2BestTime` | `double` | seconds | Best sector 2 time |
| `Sector3BestTime` | `double` | seconds | Best sector 3 time |
| `CurrentSector` | `integer` | - | Current sector number (1-3) |

### Lap Information
| Property | Type | Description |
|----------|------|-------------|
| `CurrentLap` | `integer` | Current lap number |
| `TotalLaps` | `integer` | Total laps in race |
| `CompletedLaps` | `integer` | Number of completed laps |

**See also:** [Position & Timing Documentation](POSITION-TIMING.md)

---

## Tire Data

Tire temperatures, pressures, wear, and grip levels.

**Payload Key:** `tireData`

All tire properties provide data for all four tires in the format: `[FrontLeft, FrontRight, RearLeft, RearRight]`

| Property | Type | Unit | Description |
|----------|------|------|-------------|
| `TireTemperatures` | `array<double>` | °C | Tire surface temperatures [FL, FR, RL, RR] |
| `TirePressures` | `array<double>` | PSI/kPa | Tire pressures [FL, FR, RL, RR] |
| `TireWear` | `array<double>` | % | Tire wear percentage [FL, FR, RL, RR] |
| `TireGrip` | `array<double>` | 0.0-1.0 | Tire grip levels [FL, FR, RL, RR] |
| `TireCompound` | `string` | - | Tire compound name (Soft, Medium, Hard, etc.) |
| `TireDirt` | `array<double>` | 0.0-1.0 | Tire dirt accumulation [FL, FR, RL, RR] |

**Example:**
```json
{
  "tireData": {
    "TireTemperatures": [85.3, 87.1, 82.5, 84.8],
    "TirePressures": [27.8, 27.9, 27.5, 27.6],
    "TireWear": [12.3, 15.7, 8.2, 10.1],
    "TireCompound": "Soft",
    "TireGrip": [0.98, 0.96, 0.99, 0.97]
  }
}
```

**See also:** [Tire Data Documentation](TIRE-DATA.md)

---

## Fuel & Energy Management

Fuel levels, consumption, and energy systems (ERS, DRS, battery).

**Payload Key:** `fuelData`

### Fuel Information
| Property | Type | Unit | Description |
|----------|------|------|-------------|
| `Fuel` | `double` | liters | Current fuel level |
| `FuelCapacity` | `double` | liters | Maximum fuel capacity |
| `FuelPerLap` | `double` | liters | Average fuel consumption per lap |
| `FuelRemaining` | `double` | liters | Fuel remaining in tank |
| `FuelEstimatedLaps` | `double` | laps | Estimated laps remaining on current fuel |
| `FuelToEnd` | `double` | liters | Fuel needed to finish race |

### Energy Systems
| Property | Type | Description |
|----------|------|-------------|
| `ERS_Data` | `object` | Energy Recovery System data (F1, hybrid vehicles) |
| `DRS_Data` | `object` | Drag Reduction System status and availability |
| `BatteryData` | `object` | Battery charge and usage (electric/hybrid vehicles) |

**See also:** [Fuel & Energy Documentation](FUEL-ENERGY.md)

---

## Weather & Track Conditions

Environmental conditions affecting the race.

**Payload Key:** `weatherData`

| Property | Type | Unit | Description |
|----------|------|------|-------------|
| `AirTemperature` | `double` | °C | Ambient air temperature |
| `TrackTemperature` | `double` | °C | Track surface temperature |
| `WeatherType` | `string` | - | Weather condition (Clear, Overcast, Rain, etc.) |
| `RainLevel` | `double` | 0.0-1.0 | Rainfall intensity |
| `Humidity` | `double` | % | Relative humidity |
| `WindData` | `object` | - | Wind speed and direction |
| `TrackGrip` | `double` | 0.0-1.0 | Overall track grip level |
| `TimeOfDay` | `string` | - | Current time of day in session |

**See also:** [Weather & Conditions Documentation](WEATHER-CONDITIONS.md)

---

## Damage & Mechanical

Vehicle damage and mechanical wear indicators.

**Payload Key:** `damageData`

| Property | Type | Description |
|----------|------|-------------|
| `CarDamage` | `object` | Damage to various car components |
| `EngineTemperatures` | `object` | Water and oil temperatures |
| `BrakeTemperatures` | `array<double>` | Brake disc temperatures [FL, FR, RL, RR] |
| `TurboData` | `object` | Turbocharger boost and temperature |
| `WearIndicators` | `object` | Component wear levels |

**See also:** [Damage & Mechanical Documentation](DAMAGE-MECHANICAL.md)

---

## Driver Input

Raw driver inputs and electronic system settings.

**Payload Key:** `inputData`

| Property | Type | Description |
|----------|------|-------------|
| `SteeringInput` | `object` | Steering wheel angle and rate |
| `PedalInputs` | `object` | Raw pedal position values |
| `DriverAssists` | `object` | TC, ABS, stability control settings |
| `ElectronicSystems` | `object` | Brake bias, differential, etc. |
| `InputDeviceInfo` | `object` | Controller type and force feedback data |

**See also:** [Driver Input Documentation](DRIVER-INPUT.md)

---

## Safety & Race Control

Safety car, pit information, and race control events.

**Payload Key:** `safetyData`

| Property | Type | Description |
|----------|------|-------------|
| `SafetyCarInfo` | `object` | Safety car and VSC status |
| `FlagSectors` | `object` | Flag status by track sector |
| `PitInformation` | `object` | Pit lane status, limiter, pit window |
| `RaceControl` | `object` | Session start/end, pause status |
| `Penalties` | `object` | Current penalties and warnings |
| `FormationLap` | `boolean` | Formation/warmup lap indicator |

**See also:** [Safety & Race Control Documentation](SAFETY-CONTROL.md)

---

## Track Information

Static track details for the current session.

**Payload Key:** `trackInformation`

| Property | Type | Unit | Description |
|----------|------|------|-------------|
| `TrackName` | `string` | - | Name of the racing circuit |
| `TrackLength` | `double` | meters | Full lap distance |
| `TrackConfiguration` | `string` | - | Track layout variant |

**Example:**
```json
{
  "trackInformation": {
    "TrackName": "Spa-Francorchamps",
    "TrackLength": 7004,
    "TrackConfiguration": "Grand Prix"
  }
}
```

---

## Vehicle Information

Static vehicle specifications.

**Payload Key:** `vehicleInformation`

| Property | Type | Unit | Description |
|----------|------|------|-------------|
| `VehicleModel` | `string` | - | Vehicle model name |
| `VehicleClass` | `string` | - | Racing class designation |
| `MaxRpm` | `double` | RPM | Maximum engine RPM |

---

## Session Information

Current racing session details.

**Payload Key:** `sessionInfo`

| Property | Type | Description |
|----------|------|-------------|
| `SessionType` | `string` | Practice, Qualifying, Race, etc. |
| `SessionTimeLeft` | `double` | Time remaining in session (seconds) |
| `SessionLaps` | `integer` | Total laps in session |

---

## Debug Mode

When debug mode is enabled, the `debugData` object contains ALL raw telemetry properties from SimHub.

**Payload Key:** `debugData`

```json
{
  "debugData": {
    "_GameName": "Assetto Corsa Competizione",
    "_GameRunning": true,
    "_PropertyCount": 247,
    "SpeedKmh": 245.7,
    "SpeedMph": 152.6,
    "Rpms": 8450,
    // ... 200+ more properties
  }
}
```

**Use debug mode to:**
- Discover available properties for your simulator
- Troubleshoot data issues
- Find properties not yet exposed in standard categories
- Build custom integrations

**Warning:** Debug mode significantly increases payload size and MQTT bandwidth usage.

---

## Property Availability

Not all properties are available in all racing simulators. The plugin handles this gracefully:

- **Null values are omitted** from the JSON payload
- **Missing properties** don't appear in the payload
- **Empty categories** (all properties null) are omitted entirely

This minimizes bandwidth usage and simplifies data parsing.

### Simulator Compatibility

| Feature Category | iRacing | ACC | AC | rFactor 2 | F1 Games | Other |
|------------------|---------|-----|----|-----------| ---------|-------|
| Car State | ✓ Full | ✓ Full | ✓ Full | ✓ Full | ✓ Full | ✓ Most |
| Flags | ✓ Full | ✓ Basic | ✓ Basic | ✓ Basic | ✓ Basic | ✓ Varies |
| Position/Timing | ✓ Full | ✓ Full | ✓ Full | ✓ Full | ✓ Full | ✓ Most |
| Tires | ✓ Full | ✓ Full | ✓ Full | ✓ Full | ✓ Full | ✓ Most |
| Fuel | ✓ Full | ✓ Full | ✓ Full | ✓ Full | ✓ Full | ✓ Most |
| Weather | ✓ Full | ✓ Full | ✓ Basic | ✓ Full | ✓ Full | ✓ Varies |
| Damage | ✓ Full | ✓ Full | ✓ Basic | ✓ Full | ✓ Full | ✓ Varies |
| Input Data | ✓ Full | ✓ Full | ✓ Full | ✓ Full | ✓ Full | ✓ Most |
| Safety/Race | ✓ Full | ✓ Full | ✓ Basic | ✓ Full | ✓ Full | ✓ Varies |

**Legend:**
- ✓ Full: All properties available
- ✓ Basic: Core properties available, some advanced features missing
- ✓ Most: Majority of properties available
- ✓ Varies: Depends on game version and mode

---

## Related Documentation

- [Flag Data](FLAG-DATA.md)
- [Position & Timing](POSITION-TIMING.md)
- [Tire Data](TIRE-DATA.md)
- [Fuel & Energy](FUEL-ENERGY.md)
- [Integration Examples](INTEGRATION-EXAMPLES.md)
