# Fuel & Energy Management

Fuel levels, consumption rates, and energy systems (ERS, DRS, battery) for race strategy.

## Overview

The `fuelData` object provides critical information for race strategy, including fuel consumption, remaining laps, and energy system status. Essential for endurance racing and strategy planning.

## Data Structure

```json
{
  "fuelData": {
    "Fuel": 45.2,
    "FuelCapacity": 120.0,
    "FuelPerLap": 2.8,
    "FuelRemaining": 45.2,
    "FuelEstimatedLaps": 16.1,
    "FuelToEnd": 8.4,
    "ERS_Data": {
      "ERSLevel": 0.85,
      "ERSMode": "Medium"
    },
    "DRS_Data": {
      "DRSAvailable": true,
      "DRSEnabled": false
    },
    "BatteryData": {
      "BatteryCharge": 0.92,
      "BatteryDrain": 0.03
    }
  }
}
```

## Fuel Properties

### Fuel / FuelRemaining
**Type:** `double` (nullable)
**Unit:** liters
**Range:** 0.0 - FuelCapacity

Current fuel level in the tank.

**Note:** Some simulators provide both properties with the same value.

---

### FuelCapacity
**Type:** `double` (nullable)
**Unit:** liters

Maximum fuel tank capacity.

**Usage:**
```javascript
const fuelPercent = (data.fuelData.Fuel / data.fuelData.FuelCapacity) * 100;
console.log(`Fuel: ${fuelPercent.toFixed(0)}%`);
```

---

### FuelPerLap
**Type:** `double` (nullable)
**Unit:** liters per lap

Average fuel consumption per lap based on recent laps.

**Usage:**
```javascript
// Calculate if refuel is needed
const lapsRemaining = data.positionData.TotalLaps - data.positionData.CompletedLaps;
const fuelNeeded = lapsRemaining * data.fuelData.FuelPerLap;

if (fuelNeeded > data.fuelData.FuelRemaining) {
  console.log("⚠️  REFUEL REQUIRED");
}
```

---

### FuelEstimatedLaps
**Type:** `double` (nullable)
**Unit:** laps

Estimated number of laps remaining with current fuel level.

**Formula:** `FuelRemaining / FuelPerLap`

**Usage:**
```javascript
if (data.fuelData.FuelEstimatedLaps < 3) {
  showWarning("LOW FUEL - " + data.fuelData.FuelEstimatedLaps.toFixed(1) + " laps left");
}
```

---

### FuelToEnd
**Type:** `double` (nullable)
**Unit:** liters

Amount of fuel needed to finish the race (based on remaining laps and consumption rate).

**Usage:**
```javascript
const fuelDeficit = data.fuelData.FuelToEnd - data.fuelData.FuelRemaining;

if (fuelDeficit > 0) {
  console.log(`Need to refuel ${fuelDeficit.toFixed(1)}L`);
} else {
  console.log(`No refuel needed (${Math.abs(fuelDeficit).toFixed(1)}L margin)`);
}
```

## Energy Systems

### ERS_Data (Energy Recovery System)
**Type:** `object` (nullable)

Hybrid energy recovery system data (F1, LMP1, hybrid vehicles).

**Typical Properties:**
- `ERSLevel`: Current charge level (0.0-1.0)
- `ERSMode`: Deployment mode (None, Low, Medium, High, Overtake)
- `ERSDeployment`: Amount currently being deployed

**Simulators:** Primarily F1 games, iRacing (LMP1), some rFactor 2 mods

---

### DRS_Data (Drag Reduction System)
**Type:** `object` (nullable)

Drag Reduction System status (F1, some GT cars).

**Typical Properties:**
- `DRSAvailable`: Boolean - can DRS be activated?
- `DRSEnabled`: Boolean - is DRS currently active?
- `DRSAllowed`: Boolean - is DRS allowed in this zone?

**Simulators:** F1 games, iRacing (select series)

---

### BatteryData
**Type:** `object` (nullable)

Electric vehicle battery information (Formula E, hybrid vehicles).

**Typical Properties:**
- `BatteryCharge`: Current charge level (0.0-1.0)
- `BatteryDrain`: Current drain rate
- `BatteryRechargeRate`: Current recharge rate

**Simulators:** rFactor 2 (Formula E), specific hybrid mods

## Use Cases

### Fuel Strategy Calculator
```javascript
class FuelStrategy {
  constructor() {
    this.fuelHistory = [];
  }

  update(data) {
    const fuelData = data.fuelData;
    const posData = data.positionData;

    if (!fuelData || !posData) return;

    const lapsRemaining = posData.TotalLaps - posData.CompletedLaps;
    const fuelNeeded = fuelData.FuelPerLap * lapsRemaining;
    const fuelMargin = fuelData.FuelRemaining - fuelNeeded;

    return {
      lapsRemaining,
      fuelPerLap: fuelData.FuelPerLap,
      fuelRemaining: fuelData.FuelRemaining,
      fuelNeeded,
      fuelMargin,
      mustRefuel: fuelMargin < 0,
      refuelAmount: Math.max(0, -fuelMargin + (fuelData.FuelPerLap * 2)), // +2 lap safety
      canFinish: fuelData.FuelEstimatedLaps >= lapsRemaining
    };
  }

  shouldPitForFuel(strategy) {
    // Consider pitting if less than 3 laps of fuel margin
    return strategy.fuelMargin < (strategy.fuelPerLap * 3);
  }
}
```

### Fuel Warning System
```javascript
function checkFuelWarnings(data) {
  const fuel = data.fuelData;
  const pos = data.positionData;

  const warnings = [];

  // Critical fuel level
  if (fuel.FuelEstimatedLaps < 2) {
    warnings.push({
      level: 'critical',
      message: `CRITICAL: Only ${fuel.FuelEstimatedLaps.toFixed(1)} laps of fuel!`
    });
  }
  // Low fuel warning
  else if (fuel.FuelEstimatedLaps < 5) {
    warnings.push({
      level: 'warning',
      message: `LOW FUEL: ${fuel.FuelEstimatedLaps.toFixed(1)} laps remaining`
    });
  }

  // Not enough to finish
  const lapsToGo = pos.TotalLaps - pos.CompletedLaps;
  if (fuel.FuelEstimatedLaps < lapsToGo) {
    warnings.push({
      level: 'critical',
      message: `Cannot finish! Short by ${(lapsToGo - fuel.FuelEstimatedLaps).toFixed(1)} laps`
    });
  }

  return warnings;
}
```

### Pit Stop Calculator
```javascript
function calculatePitStop(data) {
  const fuel = data.fuelData;
  const pos = data.positionData;

  const lapsRemaining = pos.TotalLaps - pos.CompletedLaps;
  const fuelForRace = lapsRemaining * fuel.FuelPerLap;
  const fuelDeficit = fuelForRace - fuel.FuelRemaining;

  // Add 2 lap safety margin
  const refuelAmount = fuelDeficit + (fuel.FuelPerLap * 2);

  // Cap at tank capacity
  const maxRefuel = fuel.FuelCapacity - fuel.FuelRemaining;
  const actualRefuel = Math.min(refuelAmount, maxRefuel);

  // Estimate pit stop time (rough estimate: 2L per second)
  const refuelTime = actualRefuel / 2.0;
  const totalPitTime = refuelTime + 5; // +5s for pit lane, tire change, etc.

  return {
    needed: fuelDeficit > 0,
    refuelAmount: actualRefuel,
    estimatedTime: totalPitTime,
    lapsUntilEmpty: fuel.FuelEstimatedLaps,
    recommendation: fuel.FuelEstimatedLaps < lapsRemaining ? 'PIT NOW' : 'NO PIT NEEDED'
  };
}
```

### ERS Management Display
```javascript
function displayERSInfo(ersData) {
  if (!ersData) return null;

  const chargePercent = (ersData.ERSLevel * 100).toFixed(0);

  let status = 'Available';
  if (ersData.ERSLevel < 0.2) status = 'Low';
  if (ersData.ERSLevel > 0.9) status = 'Full';

  return {
    charge: `${chargePercent}%`,
    mode: ersData.ERSMode,
    status: status,
    color: ersData.ERSLevel < 0.2 ? 'red' :
           ersData.ERSLevel < 0.5 ? 'yellow' : 'green'
  };
}
```

## Simulator Compatibility

| Property | iRacing | ACC | AC | rFactor 2 | F1 Games |
|----------|---------|-----|----|-----------| ---------|
| Fuel | ✓ | ✓ | ✓ | ✓ | ✓ |
| FuelCapacity | ✓ | ✓ | ✓ | ✓ | ✓ |
| FuelPerLap | ✓ | ✓ | ~ | ✓ | ✓ |
| FuelRemaining | ✓ | ✓ | ✓ | ✓ | ✓ |
| FuelEstimatedLaps | ✓ | ✓ | ~ | ✓ | ✓ |
| FuelToEnd | ✓ | ~ | ~ | ✓ | ~ |
| ERS_Data | ✓* | ✗ | ✗ | ✓* | ✓ |
| DRS_Data | ✓* | ✗ | ✗ | ✗ | ✓ |
| BatteryData | ✗ | ✗ | ✗ | ✓* | ✗ |

**Legend:**
- ✓ = Fully supported
- ✓* = Supported in specific cars/series only
- ~ = Partially supported or calculated by SimHub
- ✗ = Not available

## Related Documentation

- [Position & Timing](POSITION-TIMING.md) - Lap and race information
- [Safety & Race Control](SAFETY-CONTROL.md) - Pit stop information
- [Data Categories Overview](DATA-CATEGORIES.md) - All available data
