# Tire Data & Telemetry

Tire temperatures, pressures, wear, and grip information for optimal performance management.

## Overview

The `tireData` object provides detailed telemetry for all four tires, essential for setup optimization, stint management, and avoiding performance degradation. Critical for maintaining competitive pace throughout a race.

## Data Structure

```json
{
  "tireData": {
    "TyreTemperatureFrontLeft": 85.2,
    "TyreTemperatureFrontRight": 87.1,
    "TyreTemperatureRearLeft": 92.4,
    "TyreTemperatureRearRight": 93.8,
    "TyrePressureFrontLeft": 27.5,
    "TyrePressureFrontRight": 27.6,
    "TyrePressureRearLeft": 26.8,
    "TyrePressureRearRight": 26.9,
    "TyreWearFrontLeft": 0.15,
    "TyreWearFrontRight": 0.18,
    "TyreWearRearLeft": 0.22,
    "TyreWearRearRight": 0.25,
    "BrakeTemperatureFrontLeft": 425.5,
    "BrakeTemperatureFrontRight": 432.1,
    "BrakeTemperatureRearLeft": 315.8,
    "BrakeTemperatureRearRight": 320.2
  }
}
```

## Temperature Properties

### TyreTemperature[Position]
**Type:** `double` (nullable)
**Unit:** Celsius (°C)
**Positions:** FrontLeft, FrontRight, RearLeft, RearRight

Current tire surface temperature.

**Optimal Ranges (varies by tire compound):**
- Slick tires: 80-100°C
- Street tires: 60-90°C
- Wet tires: 50-80°C

**Usage:**
```javascript
function getTireStatus(temp, optimalMin = 80, optimalMax = 100) {
  if (temp < optimalMin - 10) return { status: 'COLD', color: 'blue' };
  if (temp < optimalMin) return { status: 'Warming', color: 'cyan' };
  if (temp <= optimalMax) return { status: 'Optimal', color: 'green' };
  if (temp <= optimalMax + 10) return { status: 'Hot', color: 'yellow' };
  return { status: 'OVERHEAT', color: 'red' };
}

const flStatus = getTireStatus(data.tireData.TyreTemperatureFrontLeft);
console.log(`FL Tire: ${flStatus.status}`);
```

---

### BrakeTemperature[Position]
**Type:** `double` (nullable)
**Unit:** Celsius (°C)
**Positions:** FrontLeft, FrontRight, RearLeft, RearRight

Current brake disc temperature.

**Typical Ranges:**
- Cold: < 200°C (reduced braking performance)
- Optimal: 200-500°C (depends on brake material)
- Hot: 500-700°C (fade risk)
- Critical: > 700°C (severe fade, potential damage)

**Usage:**
```javascript
function checkBrakeTemps(data) {
  const temps = [
    { name: 'FL', temp: data.tireData.BrakeTemperatureFrontLeft },
    { name: 'FR', temp: data.tireData.BrakeTemperatureFrontRight },
    { name: 'RL', temp: data.tireData.BrakeTemperatureRearLeft },
    { name: 'RR', temp: data.tireData.BrakeTemperatureRearRight }
  ];

  const warnings = temps
    .filter(b => b.temp > 600)
    .map(b => `${b.name}: ${b.temp.toFixed(0)}°C - BRAKE FADE RISK`);

  return warnings;
}
```

---

## Pressure Properties

### TyrePressure[Position]
**Type:** `double` (nullable)
**Unit:** PSI (pounds per square inch)
**Positions:** FrontLeft, FrontRight, RearLeft, RearRight

Current tire pressure (hot pressure during running).

**Note:** Pressure increases with temperature. Typical increase: 2-4 PSI from cold to hot.

**Usage:**
```javascript
// Monitor pressure balance
function checkPressureBalance(data) {
  const fl = data.tireData.TyrePressureFrontLeft;
  const fr = data.tireData.TyrePressureFrontRight;
  const rl = data.tireData.TyrePressureRearLeft;
  const rr = data.tireData.TyrePressureRearRight;

  const frontDiff = Math.abs(fl - fr);
  const rearDiff = Math.abs(rl - rr);

  const warnings = [];
  if (frontDiff > 1.0) {
    warnings.push(`Front pressure imbalance: ${frontDiff.toFixed(1)} PSI`);
  }
  if (rearDiff > 1.0) {
    warnings.push(`Rear pressure imbalance: ${rearDiff.toFixed(1)} PSI`);
  }

  return {
    balanced: warnings.length === 0,
    warnings,
    pressures: { fl, fr, rl, rr }
  };
}
```

---

## Wear Properties

### TyreWear[Position]
**Type:** `double` (nullable)
**Unit:** Normalized (0.0 = new, 1.0 = completely worn)
**Positions:** FrontLeft, FrontRight, RearLeft, RearRight

Tire wear level. Higher values indicate more wear.

**Usage:**
```javascript
function analyzeTireWear(data) {
  const wear = {
    fl: data.tireData.TyreWearFrontLeft,
    fr: data.tireData.TyreWearFrontRight,
    rl: data.tireData.TyreWearRearLeft,
    rr: data.tireData.TyreWearRearRight
  };

  const avgWear = (wear.fl + wear.fr + wear.rl + wear.rr) / 4;
  const maxWear = Math.max(wear.fl, wear.fr, wear.rl, wear.rr);

  const wearPercent = (avgWear * 100).toFixed(1);

  let status = 'Good';
  if (avgWear > 0.8) status = 'Critical';
  else if (avgWear > 0.6) status = 'High';
  else if (avgWear > 0.4) status = 'Moderate';

  return {
    average: wearPercent,
    status,
    mostWorn: Object.keys(wear).reduce((a, b) => wear[a] > wear[b] ? a : b),
    estimatedLapsRemaining: calculateRemainingLaps(avgWear, data)
  };
}
```

---

## Use Cases

### Tire Temperature Monitoring Dashboard

```javascript
class TireMonitor {
  constructor(optimalTemp = { min: 80, max: 100 }) {
    this.optimalTemp = optimalTemp;
  }

  analyze(data) {
    const tires = {
      fl: data.tireData.TyreTemperatureFrontLeft,
      fr: data.tireData.TyreTemperatureFrontRight,
      rl: data.tireData.TyreTemperatureRearLeft,
      rr: data.tireData.TyreTemperatureRearRight
    };

    const analysis = {};
    for (const [pos, temp] of Object.entries(tires)) {
      analysis[pos] = {
        temp: temp.toFixed(1),
        status: this.getStatus(temp),
        color: this.getColor(temp),
        deviation: (temp - (this.optimalTemp.min + this.optimalTemp.max) / 2).toFixed(1)
      };
    }

    return analysis;
  }

  getStatus(temp) {
    const { min, max } = this.optimalTemp;
    if (temp < min - 10) return 'COLD';
    if (temp < min) return 'Warming';
    if (temp <= max) return 'Optimal';
    if (temp <= max + 10) return 'Hot';
    return 'OVERHEAT';
  }

  getColor(temp) {
    const { min, max } = this.optimalTemp;
    if (temp < min - 10) return '#0066ff'; // Blue
    if (temp < min) return '#00ccff';      // Cyan
    if (temp <= max) return '#00ff00';     // Green
    if (temp <= max + 10) return '#ffcc00'; // Yellow
    return '#ff0000';                      // Red
  }
}
```

### Tire Wear Prediction

```javascript
class TireWearPredictor {
  constructor() {
    this.wearHistory = [];
    this.maxHistorySize = 10; // Track last 10 laps
  }

  recordLap(data) {
    const avgWear = (
      data.tireData.TyreWearFrontLeft +
      data.tireData.TyreWearFrontRight +
      data.tireData.TyreWearRearLeft +
      data.tireData.TyreWearRearRight
    ) / 4;

    this.wearHistory.push({
      lap: data.positionData.CompletedLaps,
      wear: avgWear
    });

    if (this.wearHistory.length > this.maxHistorySize) {
      this.wearHistory.shift();
    }
  }

  predictRemainingStint(currentLap, raceEndLap, wearLimit = 0.95) {
    if (this.wearHistory.length < 2) return null;

    // Calculate wear rate per lap
    const recent = this.wearHistory.slice(-5);
    const wearRate = (recent[recent.length - 1].wear - recent[0].wear) / (recent.length - 1);

    const currentWear = this.wearHistory[this.wearHistory.length - 1].wear;
    const wearRemaining = wearLimit - currentWear;
    const lapsOnCurrentTires = Math.floor(wearRemaining / wearRate);

    const lapsToRaceEnd = raceEndLap - currentLap;

    return {
      wearRate: (wearRate * 100).toFixed(2) + '% per lap',
      currentWear: (currentWear * 100).toFixed(1) + '%',
      estimatedTireLaps: lapsOnCurrentTires,
      canFinishRace: lapsOnCurrentTires >= lapsToRaceEnd,
      recommendation: lapsOnCurrentTires >= lapsToRaceEnd
        ? 'Current tires OK to finish'
        : `Pit required in ~${lapsOnCurrentTires} laps`
    };
  }
}
```

### Pressure and Temperature Correlation

```javascript
function analyzePressureTempCorrelation(data) {
  const tires = ['FrontLeft', 'FrontRight', 'RearLeft', 'RearRight'];

  return tires.map(pos => {
    const temp = data.tireData[`TyreTemperature${pos}`];
    const pressure = data.tireData[`TyrePressure${pos}`];
    const wear = data.tireData[`TyreWear${pos}`];

    // Estimate cold pressure (rough approximation)
    const estimatedColdPressure = pressure - (temp - 20) * 0.03;

    return {
      position: pos,
      temperature: temp.toFixed(1) + '°C',
      pressure: pressure.toFixed(1) + ' PSI',
      estimatedColdPressure: estimatedColdPressure.toFixed(1) + ' PSI',
      wear: (wear * 100).toFixed(1) + '%',
      status: temp > 100 && pressure > 30 ? 'High temp & pressure' : 'Normal'
    };
  });
}
```

### Brake Temperature Warning System

```javascript
class BrakeMonitor {
  constructor() {
    this.fadeThreshold = 600; // °C
    this.criticalThreshold = 700; // °C
  }

  check(data) {
    const brakes = {
      FL: data.tireData.BrakeTemperatureFrontLeft,
      FR: data.tireData.BrakeTemperatureFrontRight,
      RL: data.tireData.BrakeTemperatureRearLeft,
      RR: data.tireData.BrakeTemperatureRearRight
    };

    const warnings = [];
    const criticals = [];

    for (const [pos, temp] of Object.entries(brakes)) {
      if (temp > this.criticalThreshold) {
        criticals.push({ position: pos, temp, message: `${pos}: CRITICAL ${temp.toFixed(0)}°C` });
      } else if (temp > this.fadeThreshold) {
        warnings.push({ position: pos, temp, message: `${pos}: Fade risk ${temp.toFixed(0)}°C` });
      }
    }

    return {
      critical: criticals.length > 0,
      warnings: warnings.length > 0,
      messages: [...criticals, ...warnings],
      maxTemp: Math.max(...Object.values(brakes)),
      avgFrontTemp: (brakes.FL + brakes.FR) / 2,
      avgRearTemp: (brakes.RL + brakes.RR) / 2
    };
  }

  suggestBrakeBalance(avgFront, avgRear) {
    const diff = avgFront - avgRear;

    if (Math.abs(diff) < 50) return 'Brake balance OK';
    if (diff > 100) return 'Consider moving brake bias rearward';
    if (diff < -100) return 'Consider moving brake bias forward';

    return 'Minor imbalance, monitor';
  }
}
```

### Complete Tire Dashboard

```javascript
class TireDashboard {
  update(data) {
    const tireData = data.tireData;

    return {
      temperatures: this.formatTemps(tireData),
      pressures: this.formatPressures(tireData),
      wear: this.formatWear(tireData),
      brakes: this.formatBrakes(tireData),
      warnings: this.checkWarnings(tireData)
    };
  }

  formatTemps(tireData) {
    return {
      fl: `${tireData.TyreTemperatureFrontLeft?.toFixed(1) || '--'}°C`,
      fr: `${tireData.TyreTemperatureFrontRight?.toFixed(1) || '--'}°C`,
      rl: `${tireData.TyreTemperatureRearLeft?.toFixed(1) || '--'}°C`,
      rr: `${tireData.TyreTemperatureRearRight?.toFixed(1) || '--'}°C`
    };
  }

  formatPressures(tireData) {
    return {
      fl: `${tireData.TyrePressureFrontLeft?.toFixed(1) || '--'} PSI`,
      fr: `${tireData.TyrePressureFrontRight?.toFixed(1) || '--'} PSI`,
      rl: `${tireData.TyrePressureRearLeft?.toFixed(1) || '--'} PSI`,
      rr: `${tireData.TyrePressureRearRight?.toFixed(1) || '--'} PSI`
    };
  }

  formatWear(tireData) {
    return {
      fl: `${((tireData.TyreWearFrontLeft || 0) * 100).toFixed(0)}%`,
      fr: `${((tireData.TyreWearFrontRight || 0) * 100).toFixed(0)}%`,
      rl: `${((tireData.TyreWearRearLeft || 0) * 100).toFixed(0)}%`,
      rr: `${((tireData.TyreWearRearRight || 0) * 100).toFixed(0)}%`
    };
  }

  formatBrakes(tireData) {
    return {
      fl: `${tireData.BrakeTemperatureFrontLeft?.toFixed(0) || '--'}°C`,
      fr: `${tireData.BrakeTemperatureFrontRight?.toFixed(0) || '--'}°C`,
      rl: `${tireData.BrakeTemperatureRearLeft?.toFixed(0) || '--'}°C`,
      rr: `${tireData.BrakeTemperatureRearRight?.toFixed(0) || '--'}°C`
    };
  }

  checkWarnings(tireData) {
    const warnings = [];

    // Temperature warnings
    const temps = [
      tireData.TyreTemperatureFrontLeft,
      tireData.TyreTemperatureFrontRight,
      tireData.TyreTemperatureRearLeft,
      tireData.TyreTemperatureRearRight
    ];
    if (Math.max(...temps) > 110) warnings.push('⚠️ Tire overheat');
    if (Math.min(...temps) < 60) warnings.push('❄️ Tires cold');

    // Wear warnings
    const wearLevels = [
      tireData.TyreWearFrontLeft,
      tireData.TyreWearFrontRight,
      tireData.TyreWearRearLeft,
      tireData.TyreWearRearRight
    ];
    if (Math.max(...wearLevels) > 0.8) warnings.push('🔴 High tire wear');

    // Brake warnings
    const brakeTemps = [
      tireData.BrakeTemperatureFrontLeft,
      tireData.BrakeTemperatureFrontRight
    ];
    if (Math.max(...brakeTemps) > 600) warnings.push('🔥 Brake fade risk');

    return warnings;
  }
}
```

## Simulator Compatibility

| Property | iRacing | ACC | AC | rFactor 2 | F1 Games |
|----------|---------|-----|----|-----------| ---------|
| TyreTemperature[Pos] | ✓ | ✓ | ✓ | ✓ | ✓ |
| TyrePressure[Pos] | ✓ | ✓ | ✓ | ✓ | ✓ |
| TyreWear[Pos] | ✓ | ✓ | ✓ | ✓ | ✓ |
| BrakeTemperature[Pos] | ✓ | ✓ | ✓ | ✓ | ✓ |

**Legend:**
- ✓ = Fully supported
- ~ = Partially supported or calculated
- ✗ = Not available

**Notes:**
- Some simulators provide tire core temperature vs. surface temperature
- Pressure units may vary (PSI, bar, kPa) - this plugin normalizes to PSI
- Wear calculation methods differ between simulators
- Brake temperature may represent disc or pad temperature depending on simulator

## Related Documentation

- [Fuel & Energy](FUEL-ENERGY.md) - Fuel and energy management
- [Car State](CAR-STATE.md) - Basic vehicle telemetry
- [Data Categories Overview](DATA-CATEGORIES.md) - All available data
