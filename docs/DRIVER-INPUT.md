# Driver Input & Controls

Raw driver inputs including throttle, brake, steering, and electronic system settings.

## Overview

The `inputData` object provides information about driver control inputs and electronic system configurations. Useful for analyzing driving technique, creating input overlays, and monitoring electronic aids.

## Data Structure

```json
{
  "inputData": {
    "Throttle": 0.85,
    "Brake": 0.0,
    "Clutch": 0.0,
    "Steering": -0.23,
    "ElectronicSystems": {
      "BrakeBias": 52.5,
      "TractionControl": 3,
      "ABS": 2,
      "DifferentialSetting": 8
    },
    "InputDeviceInfo": {
      "ControllerType": "Wheel",
      "ForceFeedback": 0.65
    }
  }
}
```

## Primary Input Properties

### Throttle
**Type:** `double` (nullable)
**Unit:** Normalized (0.0 = no input, 1.0 = full throttle)

Current throttle pedal position.

**Usage:**
```javascript
function analyzeThrottle(throttle) {
  const percent = (throttle * 100).toFixed(0);

  let status;
  if (throttle > 0.9) status = 'Full throttle';
  else if (throttle > 0.5) status = 'Partial throttle';
  else if (throttle > 0.1) status = 'Light throttle';
  else status = 'Off throttle';

  return {
    value: percent + '%',
    status,
    raw: throttle
  };
}

const throttleStatus = analyzeThrottle(data.inputData.Throttle);
console.log(`Throttle: ${throttleStatus.value} (${throttleStatus.status})`);
```

---

### Brake
**Type:** `double` (nullable)
**Unit:** Normalized (0.0 = no input, 1.0 = full brake)

Current brake pedal position.

**Usage:**
```javascript
function analyzeBrake(brake) {
  const percent = (brake * 100).toFixed(0);

  let status;
  if (brake > 0.9) status = 'Heavy braking';
  else if (brake > 0.5) status = 'Moderate braking';
  else if (brake > 0.1) status = 'Light braking';
  else status = 'No braking';

  return {
    value: percent + '%',
    status,
    raw: brake
  };
}

// Trail braking detection
function detectTrailBraking(throttle, brake) {
  return throttle > 0.1 && brake > 0.1 ? 'Trail braking detected' : null;
}
```

---

### Clutch
**Type:** `double` (nullable)
**Unit:** Normalized (0.0 = released, 1.0 = fully depressed)

Current clutch pedal position.

**Usage:**
```javascript
function analyzeClutch(clutch, gear) {
  const percent = (clutch * 100).toFixed(0);

  let status;
  if (clutch > 0.8) status = 'Fully engaged';
  else if (clutch > 0.3) status = 'Slipping';
  else if (clutch > 0.05) status = 'Partial';
  else status = 'Released';

  // Detect potential clutch issues
  const warnings = [];
  if (clutch > 0.3 && gear > 1) {
    warnings.push('Clutch slipping in gear - may indicate improper usage');
  }

  return {
    value: percent + '%',
    status,
    warnings
  };
}
```

---

### Steering
**Type:** `double` (nullable)
**Unit:** Normalized (-1.0 = full left, 0.0 = center, 1.0 = full right)

Current steering wheel angle.

**Usage:**
```javascript
function analyzeSteering(steering) {
  const angle = steering * 100; // Approximate percentage
  const absAngle = Math.abs(angle);

  let direction;
  if (steering > 0.05) direction = 'Right';
  else if (steering < -0.05) direction = 'Left';
  else direction = 'Straight';

  let magnitude;
  if (absAngle > 80) magnitude = 'Sharp';
  else if (absAngle > 40) magnitude = 'Moderate';
  else if (absAngle > 10) magnitude = 'Gentle';
  else magnitude = 'Minimal';

  return {
    value: steering.toFixed(3),
    angle: angle.toFixed(1) + '%',
    direction,
    magnitude,
    display: `${direction} ${magnitude !== 'Minimal' ? magnitude : ''}`
  };
}

const steeringStatus = analyzeSteering(data.inputData.Steering);
console.log(`Steering: ${steeringStatus.display} (${steeringStatus.angle})`);
```

---

## Electronic Systems

### ElectronicSystems
**Type:** `object` (nullable)

Electronic system settings and configurations.

**Typical Properties:**

#### BrakeBias
**Type:** `double`
**Unit:** Percentage (0-100, where 50 = balanced, >50 = front bias, <50 = rear bias)

Brake balance between front and rear axles.

**Usage:**
```javascript
function analyzeBrakeBias(bias) {
  if (bias > 55) return { position: 'Front-biased', advice: 'More stable braking, may understeer' };
  if (bias < 45) return { position: 'Rear-biased', advice: 'More rotation, risk of oversteer' };
  return { position: 'Balanced', advice: 'Neutral braking balance' };
}

const biasAnalysis = analyzeBrakeBias(data.inputData.ElectronicSystems.BrakeBias);
console.log(`Brake Bias: ${data.inputData.ElectronicSystems.BrakeBias}% (${biasAnalysis.position})`);
```

#### TractionControl
**Type:** `int` (nullable)
**Unit:** Setting level (typically 0-12, 0 = off)

Traction control intervention level.

**Usage:**
```javascript
function describeTractionControl(level) {
  if (level === 0) return 'OFF - No intervention';
  if (level <= 3) return 'Low - Minimal intervention, for experienced drivers';
  if (level <= 7) return 'Medium - Balanced intervention';
  return 'High - Aggressive intervention, easier to drive';
}

console.log(`TC: ${data.inputData.ElectronicSystems.TractionControl} - ${describeTractionControl(data.inputData.ElectronicSystems.TractionControl)}`);
```

#### ABS
**Type:** `int` (nullable)
**Unit:** Setting level (typically 0-5, 0 = off)

Anti-lock braking system intervention level.

**Usage:**
```javascript
function describeABS(level) {
  if (level === 0) return 'OFF - Manual braking control';
  if (level <= 2) return 'Low - Minimal assistance';
  return 'High - Strong anti-lock intervention';
}
```

#### DifferentialSetting
**Type:** `int` (nullable)
**Unit:** Setting level (car-specific range)

Differential lock/preload setting.

**Typical Impact:**
- Lower settings: More open diff, better turn-in, risk of inside wheel spin
- Higher settings: Locked diff, more traction on exit, may push wide

---

## Input Device Information

### InputDeviceInfo
**Type:** `object` (nullable)

Information about the input device being used.

**Typical Properties:**

#### ControllerType
**Type:** `string`

Type of controller being used.

**Common Values:**
- `"Wheel"` - Steering wheel and pedals
- `"Gamepad"` - Xbox/PlayStation controller
- `"Keyboard"` - Keyboard input

#### ForceFeedback
**Type:** `double` (nullable)
**Unit:** Normalized (0.0-1.0)

Current force feedback intensity (for wheel users).

**Usage:**
```javascript
function analyzeFFB(ffb) {
  if (ffb > 0.9) return 'Very strong forces - near wheel limit';
  if (ffb > 0.7) return 'Strong forces - good feedback';
  if (ffb > 0.3) return 'Moderate forces';
  return 'Light forces';
}
```

---

## Use Cases

### Input Overlay Display

```javascript
class InputOverlay {
  update(inputData) {
    return {
      throttle: this.formatBar(inputData.Throttle, '#00ff00'),
      brake: this.formatBar(inputData.Brake, '#ff0000'),
      clutch: this.formatBar(inputData.Clutch, '#0000ff'),
      steering: this.formatSteering(inputData.Steering),
      electronics: this.formatElectronics(inputData.ElectronicSystems)
    };
  }

  formatBar(value, color) {
    const percent = (value * 100).toFixed(0);
    const barWidth = Math.round(value * 20); // 20 chars wide
    const bar = '█'.repeat(barWidth) + '░'.repeat(20 - barWidth);

    return {
      value: percent + '%',
      bar,
      color
    };
  }

  formatSteering(steering) {
    const center = 20; // Center position
    const position = Math.round(center + (steering * center));
    const indicator = ' '.repeat(position) + '│';

    return {
      value: steering.toFixed(2),
      visual: '├' + '─'.repeat(39) + '┤',
      indicator
    };
  }

  formatElectronics(systems) {
    if (!systems) return null;

    return {
      brakeBias: `BB: ${systems.BrakeBias?.toFixed(1) || '--'}%`,
      tc: `TC: ${systems.TractionControl ?? '--'}`,
      abs: `ABS: ${systems.ABS ?? '--'}`,
      diff: `DIFF: ${systems.DifferentialSetting ?? '--'}`
    };
  }
}
```

### Driving Technique Analyzer

```javascript
class DrivingAnalyzer {
  constructor() {
    this.samples = [];
  }

  analyzeLap(inputData, speedData) {
    this.samples.push({
      throttle: inputData.Throttle,
      brake: inputData.Brake,
      steering: inputData.Steering,
      speed: speedData.SpeedKmh,
      timestamp: Date.now()
    });
  }

  generateReport() {
    if (this.samples.length < 10) return 'Insufficient data';

    const analysis = {
      throttleSmooth: this.analyzeThrottleSmoothness(),
      brakingPoints: this.analyzeBrakingPoints(),
      corneringStyle: this.analyzeCorneringStyle(),
      trailBraking: this.detectTrailBrakingUsage()
    };

    return analysis;
  }

  analyzeThrottleSmoothness() {
    let changes = 0;
    for (let i = 1; i < this.samples.length; i++) {
      const diff = Math.abs(this.samples[i].throttle - this.samples[i - 1].throttle);
      if (diff > 0.3) changes++; // Sudden changes
    }

    const smoothness = 1 - (changes / this.samples.length);
    return {
      score: (smoothness * 100).toFixed(0) + '%',
      advice: smoothness > 0.8 ? 'Smooth throttle application' :
              smoothness > 0.6 ? 'Moderately smooth' :
              'Jerky throttle inputs - practice smoother application'
    };
  }

  analyzeBrakingPoints() {
    const brakingEvents = [];
    let inBraking = false;

    for (let i = 0; i < this.samples.length; i++) {
      if (this.samples[i].brake > 0.3 && !inBraking) {
        brakingEvents.push({
          speed: this.samples[i].speed,
          pressure: this.samples[i].brake
        });
        inBraking = true;
      } else if (this.samples[i].brake < 0.1) {
        inBraking = false;
      }
    }

    return {
      count: brakingEvents.length,
      avgSpeed: (brakingEvents.reduce((sum, e) => sum + e.speed, 0) / brakingEvents.length).toFixed(1),
      avgPressure: (brakingEvents.reduce((sum, e) => sum + e.pressure, 0) / brakingEvents.length).toFixed(2)
    };
  }

  analyzeCorneringStyle() {
    let trailBrakingSamples = 0;
    let coastingSamples = 0;

    for (const sample of this.samples) {
      if (Math.abs(sample.steering) > 0.2) { // In corner
        if (sample.throttle > 0.1 && sample.brake > 0.1) {
          trailBrakingSamples++;
        } else if (sample.throttle < 0.1 && sample.brake < 0.1) {
          coastingSamples++;
        }
      }
    }

    return {
      trailBraking: (trailBrakingSamples / this.samples.length * 100).toFixed(1) + '%',
      coasting: (coastingSamples / this.samples.length * 100).toFixed(1) + '%',
      style: trailBrakingSamples > coastingSamples ? 'Aggressive (trail braking)' : 'Smooth (coasting entry)'
    };
  }

  detectTrailBrakingUsage() {
    let trailBrakingCount = 0;

    for (const sample of this.samples) {
      if (sample.throttle > 0.1 && sample.brake > 0.1) {
        trailBrakingCount++;
      }
    }

    return {
      detected: trailBrakingCount > 0,
      frequency: (trailBrakingCount / this.samples.length * 100).toFixed(1) + '%'
    };
  }
}
```

### Electronic Aid Optimizer

```javascript
class AidOptimizer {
  recommendSettings(drivingData, lapTimes) {
    const recommendations = [];

    // Analyze traction control usage
    if (this.detectWheelSpin(drivingData)) {
      recommendations.push({
        system: 'Traction Control',
        current: drivingData.ElectronicSystems.TractionControl,
        suggestion: Math.min(drivingData.ElectronicSystems.TractionControl + 1, 12),
        reason: 'Frequent wheel spin detected on corner exit'
      });
    }

    // Analyze brake lock-ups
    if (this.detectLockUps(drivingData)) {
      recommendations.push({
        system: 'ABS',
        current: drivingData.ElectronicSystems.ABS,
        suggestion: Math.min(drivingData.ElectronicSystems.ABS + 1, 5),
        reason: 'Brake lock-ups detected'
      });
    }

    // Analyze brake bias
    const biasRecommendation = this.optimizeBrakeBias(drivingData);
    if (biasRecommendation) {
      recommendations.push(biasRecommendation);
    }

    return recommendations;
  }

  detectWheelSpin(data) {
    // Simplified: would need wheel speed data in real implementation
    return data.Throttle > 0.8 && data.Steering > 0.2;
  }

  detectLockUps(data) {
    // Simplified: would need wheel speed data
    return data.Brake > 0.9;
  }

  optimizeBrakeBias(data) {
    const bias = data.ElectronicSystems.BrakeBias;

    // Simplified optimization logic
    if (bias > 60) {
      return {
        system: 'Brake Bias',
        current: bias,
        suggestion: bias - 2,
        reason: 'Very front-biased - may cause understeer'
      };
    }

    return null;
  }
}
```

### Input Consistency Monitor

```javascript
class ConsistencyMonitor {
  constructor() {
    this.corneringSamples = [];
  }

  recordCorner(inputData, cornerEntry, cornerExit) {
    this.corneringSamples.push({
      entry: {
        brake: cornerEntry.brake,
        speed: cornerEntry.speed
      },
      apex: {
        throttle: inputData.Throttle,
        steering: inputData.Steering
      },
      exit: {
        throttle: cornerExit.throttle
      }
    });
  }

  analyzeConsistency() {
    if (this.corneringSamples.length < 3) return 'Need more data';

    const entryBraking = this.corneringSamples.map(s => s.entry.brake);
    const exitThrottle = this.corneringSamples.map(s => s.exit.throttle);

    return {
      entryConsistency: this.calculateVariance(entryBraking),
      exitConsistency: this.calculateVariance(exitThrottle),
      overall: this.getConsistencyRating()
    };
  }

  calculateVariance(values) {
    const avg = values.reduce((a, b) => a + b, 0) / values.length;
    const variance = values.reduce((sum, val) => sum + Math.pow(val - avg, 2), 0) / values.length;
    return Math.sqrt(variance);
  }

  getConsistencyRating() {
    // Simplified rating
    const variance = this.calculateVariance(
      this.corneringSamples.map(s => s.apex.throttle)
    );

    if (variance < 0.05) return 'Excellent - Very consistent';
    if (variance < 0.1) return 'Good - Consistent';
    if (variance < 0.2) return 'Moderate - Some variation';
    return 'Poor - Inconsistent inputs';
  }
}
```

## Simulator Compatibility

| Property | iRacing | ACC | AC | rFactor 2 | F1 Games |
|----------|---------|-----|----|-----------| ---------|
| Throttle | ✓ | ✓ | ✓ | ✓ | ✓ |
| Brake | ✓ | ✓ | ✓ | ✓ | ✓ |
| Clutch | ✓ | ✓ | ✓ | ✓ | ✓ |
| Steering | ✓ | ✓ | ✓ | ✓ | ✓ |
| BrakeBias | ✓ | ✓ | ~ | ✓ | ✓ |
| TractionControl | ✓ | ✓ | ✓ | ✓ | ✓ |
| ABS | ✓ | ✓ | ✓ | ✓ | ✓ |
| DifferentialSetting | ✓ | ~ | ~ | ✓ | ~ |
| InputDeviceInfo | ~ | ~ | ~ | ~ | ~ |

**Legend:**
- ✓ = Fully supported
- ~ = Partially supported or limited
- ✗ = Not available

**Notes:**
- All simulators provide basic input data (throttle, brake, steering, clutch)
- Electronic system settings availability depends on car/series
- Differential settings vary by car type
- Force feedback data is simulator-specific
- Some games may normalize inputs differently

## Related Documentation

- [Car State](CAR-STATE.md) - Vehicle response to driver inputs
- [Tire Data](TIRE-DATA.md) - Tire response to inputs
- [Data Categories Overview](DATA-CATEGORIES.md) - All available data
