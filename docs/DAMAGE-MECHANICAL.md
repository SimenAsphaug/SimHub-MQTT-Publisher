# Damage & Mechanical Systems

Vehicle damage, mechanical wear, engine temperatures, and component health monitoring.

## Overview

The `damageData` object provides information about car damage, mechanical component wear, and system temperatures. Essential for monitoring car health during races and avoiding mechanical failures.

## Data Structure

```json
{
  "damageData": {
    "CarDamage": {
      "Front": 0.05,
      "Rear": 0.02,
      "Left": 0.0,
      "Right": 0.03
    },
    "EngineTemperatures": {
      "WaterTemp": 92.5,
      "OilTemp": 105.8
    },
    "BrakeTemperatures": [425.5, 432.1, 315.8, 320.2],
    "TurboData": {
      "Boost": 1.8,
      "Temperature": 850.0
    },
    "WearIndicators": {
      "Engine": 0.15,
      "Gearbox": 0.08,
      "Suspension": 0.12
    }
  }
}
```

## Damage Properties

### CarDamage
**Type:** `object` (nullable)

Damage levels for different sections of the car.

**Typical Properties:**
- `Front`: Front damage (0.0 = no damage, 1.0 = completely destroyed)
- `Rear`: Rear damage
- `Left`: Left side damage
- `Right`: Right side damage

**Simulator Variations:**
Some simulators provide more granular damage (Front Left, Front Right, etc.) or component-specific damage (Front Wing, Sidepods, Floor).

**Usage:**
```javascript
function assessDamage(carDamage) {
  if (!carDamage) return { severity: 'None', advice: 'No damage data available' };

  const damages = {
    Front: carDamage.Front || 0,
    Rear: carDamage.Rear || 0,
    Left: carDamage.Left || 0,
    Right: carDamage.Right || 0
  };

  const maxDamage = Math.max(...Object.values(damages));
  const avgDamage = Object.values(damages).reduce((a, b) => a + b, 0) / 4;

  let severity, advice;
  if (maxDamage > 0.7) {
    severity = 'Critical';
    advice = 'Major damage - consider pitting for repairs';
  } else if (maxDamage > 0.4) {
    severity = 'Heavy';
    advice = 'Significant damage affecting performance';
  } else if (maxDamage > 0.15) {
    severity = 'Moderate';
    advice = 'Minor damage - manageable but monitor closely';
  } else if (maxDamage > 0) {
    severity = 'Light';
    advice = 'Superficial damage - continue racing';
  } else {
    severity = 'None';
    advice = 'Car in perfect condition';
  }

  return {
    severity,
    advice,
    maxDamage: (maxDamage * 100).toFixed(1) + '%',
    avgDamage: (avgDamage * 100).toFixed(1) + '%',
    areas: damages
  };
}

const damageReport = assessDamage(data.damageData.CarDamage);
console.log(`Damage: ${damageReport.severity} (${damageReport.maxDamage} max)`);
console.log(damageReport.advice);
```

---

## Engine Temperature Properties

### EngineTemperatures
**Type:** `object` (nullable)

Engine cooling system temperatures.

**Typical Properties:**
- `WaterTemp`: Coolant/water temperature (°C)
- `OilTemp`: Engine oil temperature (°C)

**Optimal Ranges:**
- **Water/Coolant**: 80-105°C (optimal), >110°C (overheat warning), >120°C (critical)
- **Oil**: 90-120°C (optimal), >130°C (overheat warning), >140°C (critical)

**Usage:**
```javascript
function monitorEngineTemps(engineTemps) {
  if (!engineTemps) return { status: 'No data', warnings: [] };

  const warnings = [];
  let status = 'Normal';

  // Check water temperature
  const water = engineTemps.WaterTemp || 0;
  if (water > 120) {
    warnings.push('🔴 CRITICAL: Water temp extremely high!');
    status = 'Critical';
  } else if (water > 110) {
    warnings.push('⚠️ WARNING: Water temp high');
    status = 'Warning';
  } else if (water > 105) {
    warnings.push('⚡ Water temp elevated');
    status = 'Elevated';
  }

  // Check oil temperature
  const oil = engineTemps.OilTemp || 0;
  if (oil > 140) {
    warnings.push('🔴 CRITICAL: Oil temp extremely high!');
    status = 'Critical';
  } else if (oil > 130) {
    warnings.push('⚠️ WARNING: Oil temp high');
    if (status === 'Normal') status = 'Warning';
  } else if (oil > 120) {
    warnings.push('⚡ Oil temp elevated');
    if (status === 'Normal') status = 'Elevated';
  }

  return {
    status,
    warnings,
    water: water.toFixed(1) + '°C',
    oil: oil.toFixed(1) + '°C',
    advice: status === 'Critical' ? 'SLOW DOWN or PIT IMMEDIATELY' :
            status === 'Warning' ? 'Reduce pace, monitor closely' :
            status === 'Elevated' ? 'Watch temperatures' : 'Temps normal'
  };
}

const tempStatus = monitorEngineTemps(data.damageData.EngineTemperatures);
console.log(`Engine: ${tempStatus.status}`);
console.log(`Water: ${tempStatus.water}, Oil: ${tempStatus.oil}`);
if (tempStatus.warnings.length > 0) {
  tempStatus.warnings.forEach(w => console.log(w));
}
```

---

## Turbocharger Properties

### TurboData
**Type:** `object` (nullable)

Turbocharger boost pressure and temperature.

**Typical Properties:**
- `Boost`: Boost pressure (bar or PSI)
- `Temperature`: Turbo temperature (°C)

**Typical Values:**
- **Boost**: 0.5-2.5 bar (varies greatly by car)
- **Temperature**: 600-1000°C (can go higher in racing applications)

**Usage:**
```javascript
function analyzeTurbo(turboData) {
  if (!turboData) return null;

  const boost = turboData.Boost || 0;
  const temp = turboData.Temperature || 0;

  let boostStatus = 'Normal';
  if (boost < 0.5) boostStatus = 'Low boost';
  else if (boost > 2.5) boostStatus = 'High boost';

  let tempStatus = 'Normal';
  if (temp > 1000) tempStatus = 'Very hot';
  else if (temp > 850) tempStatus = 'Hot';

  return {
    boost: boost.toFixed(2) + ' bar',
    temp: temp.toFixed(0) + '°C',
    boostStatus,
    tempStatus,
    warning: tempStatus === 'Very hot' ? 'Turbo running very hot!' : null
  };
}
```

---

## Component Wear Properties

### WearIndicators
**Type:** `object` (nullable)

Component wear levels (0.0 = new, 1.0 = completely worn/failed).

**Typical Properties:**
- `Engine`: Engine wear
- `Gearbox`: Transmission wear
- `Suspension`: Suspension wear
- `Brakes`: Brake component wear

**Usage:**
```javascript
function checkComponentWear(wearIndicators) {
  if (!wearIndicators) return { status: 'No data', components: [] };

  const components = [];

  for (const [component, wear] of Object.entries(wearIndicators)) {
    let status, color;

    if (wear > 0.9) {
      status = 'Critical';
      color = 'red';
    } else if (wear > 0.7) {
      status = 'High';
      color = 'orange';
    } else if (wear > 0.5) {
      status = 'Moderate';
      color = 'yellow';
    } else if (wear > 0.3) {
      status = 'Light';
      color = 'green';
    } else {
      status = 'Good';
      color = 'green';
    }

    components.push({
      name: component,
      wear: (wear * 100).toFixed(0) + '%',
      status,
      color
    });
  }

  const criticalComponents = components.filter(c => c.status === 'Critical');

  return {
    components,
    criticalCount: criticalComponents.length,
    warning: criticalComponents.length > 0
      ? `${criticalComponents.map(c => c.name).join(', ')} critically worn!`
      : null
  };
}

const wearReport = checkComponentWear(data.damageData.WearIndicators);
if (wearReport.warning) {
  console.log(`⚠️ ${wearReport.warning}`);
}
wearReport.components.forEach(c => {
  console.log(`${c.name}: ${c.wear} (${c.status})`);
});
```

---

## Use Cases

### Complete Health Monitor Dashboard

```javascript
class CarHealthMonitor {
  update(damageData) {
    return {
      overall: this.getOverallHealth(damageData),
      damage: this.analyzeDamage(damageData.CarDamage),
      engine: this.analyzeEngine(damageData.EngineTemperatures),
      turbo: this.analyzeTurbo(damageData.TurboData),
      wear: this.analyzeWear(damageData.WearIndicators),
      warnings: this.getWarnings(damageData)
    };
  }

  getOverallHealth(data) {
    const issues = [];

    // Check damage
    if (data.CarDamage) {
      const maxDamage = Math.max(...Object.values(data.CarDamage));
      if (maxDamage > 0.4) issues.push('damage');
    }

    // Check engine temps
    if (data.EngineTemperatures) {
      if (data.EngineTemperatures.WaterTemp > 110 ||
          data.EngineTemperatures.OilTemp > 130) {
        issues.push('overheating');
      }
    }

    // Check wear
    if (data.WearIndicators) {
      const maxWear = Math.max(...Object.values(data.WearIndicators));
      if (maxWear > 0.7) issues.push('wear');
    }

    if (issues.length === 0) return { status: 'Excellent', color: 'green' };
    if (issues.length === 1) return { status: 'Good', color: 'yellow' };
    if (issues.length === 2) return { status: 'Moderate', color: 'orange' };
    return { status: 'Critical', color: 'red' };
  }

  analyzeDamage(carDamage) {
    if (!carDamage) return null;

    const damages = Object.entries(carDamage).map(([area, value]) => ({
      area,
      value: (value * 100).toFixed(0) + '%',
      severity: value > 0.4 ? 'High' : value > 0.15 ? 'Moderate' : 'Light'
    }));

    return damages.filter(d => parseFloat(d.value) > 0);
  }

  analyzeEngine(temps) {
    if (!temps) return null;

    return {
      water: {
        temp: temps.WaterTemp?.toFixed(1) + '°C',
        status: temps.WaterTemp > 110 ? 'High' : 'Normal'
      },
      oil: {
        temp: temps.OilTemp?.toFixed(1) + '°C',
        status: temps.OilTemp > 130 ? 'High' : 'Normal'
      }
    };
  }

  analyzeTurbo(turbo) {
    if (!turbo) return null;

    return {
      boost: turbo.Boost?.toFixed(2) + ' bar',
      temp: turbo.Temperature?.toFixed(0) + '°C',
      status: turbo.Temperature > 950 ? 'Hot' : 'Normal'
    };
  }

  analyzeWear(wear) {
    if (!wear) return null;

    return Object.entries(wear).map(([component, value]) => ({
      component,
      wear: (value * 100).toFixed(0) + '%',
      status: value > 0.7 ? 'High' : value > 0.4 ? 'Moderate' : 'Low'
    }));
  }

  getWarnings(data) {
    const warnings = [];

    // Damage warnings
    if (data.CarDamage) {
      const maxDamage = Math.max(...Object.values(data.CarDamage));
      if (maxDamage > 0.5) {
        warnings.push({ level: 'critical', message: 'Heavy damage detected' });
      }
    }

    // Temperature warnings
    if (data.EngineTemperatures) {
      if (data.EngineTemperatures.WaterTemp > 115) {
        warnings.push({ level: 'critical', message: 'Engine overheating!' });
      }
      if (data.EngineTemperatures.OilTemp > 135) {
        warnings.push({ level: 'critical', message: 'Oil temp critical!' });
      }
    }

    // Wear warnings
    if (data.WearIndicators) {
      Object.entries(data.WearIndicators).forEach(([component, wear]) => {
        if (wear > 0.85) {
          warnings.push({
            level: 'critical',
            message: `${component} critically worn (${(wear * 100).toFixed(0)}%)`
          });
        }
      });
    }

    return warnings;
  }
}
```

### Pit Stop Advisor

```javascript
class PitStopAdvisor {
  shouldPit(damageData, fuelData) {
    const reasons = [];

    // Check damage
    if (damageData.CarDamage) {
      const maxDamage = Math.max(...Object.values(damageData.CarDamage));
      if (maxDamage > 0.5) {
        reasons.push({
          priority: 'high',
          reason: 'Heavy damage requires repairs',
          action: 'Repair damage'
        });
      }
    }

    // Check engine temps
    if (damageData.EngineTemperatures) {
      if (damageData.EngineTemperatures.WaterTemp > 115) {
        reasons.push({
          priority: 'critical',
          reason: 'Engine overheating',
          action: 'Cool down, check for damage'
        });
      }
    }

    // Check component wear
    if (damageData.WearIndicators) {
      Object.entries(damageData.WearIndicators).forEach(([component, wear]) => {
        if (wear > 0.9) {
          reasons.push({
            priority: 'critical',
            reason: `${component} critically worn`,
            action: `Replace ${component.toLowerCase()}`
          });
        }
      });
    }

    return {
      shouldPit: reasons.length > 0,
      reasons,
      urgency: reasons.some(r => r.priority === 'critical') ? 'Immediate' : 'Soon'
    };
  }
}
```

### Temperature Monitoring System

```javascript
class TemperatureMonitor {
  constructor() {
    this.history = [];
    this.maxHistory = 30; // 30 data points
  }

  update(engineTemps) {
    if (!engineTemps) return null;

    this.history.push({
      timestamp: Date.now(),
      water: engineTemps.WaterTemp,
      oil: engineTemps.OilTemp
    });

    if (this.history.length > this.maxHistory) {
      this.history.shift();
    }

    return {
      current: this.getCurrentStatus(engineTemps),
      trend: this.calculateTrend(),
      prediction: this.predictOverheat()
    };
  }

  getCurrentStatus(temps) {
    return {
      water: {
        value: temps.WaterTemp?.toFixed(1) + '°C',
        status: this.getTempStatus(temps.WaterTemp, 105, 115)
      },
      oil: {
        value: temps.OilTemp?.toFixed(1) + '°C',
        status: this.getTempStatus(temps.OilTemp, 120, 135)
      }
    };
  }

  getTempStatus(temp, warning, critical) {
    if (temp > critical) return 'Critical';
    if (temp > warning) return 'Warning';
    if (temp > warning - 10) return 'Elevated';
    return 'Normal';
  }

  calculateTrend() {
    if (this.history.length < 5) return 'Insufficient data';

    const recent = this.history.slice(-5);
    const waterTrend = recent[4].water - recent[0].water;
    const oilTrend = recent[4].oil - recent[0].oil;

    return {
      water: waterTrend > 2 ? 'Rising' : waterTrend < -2 ? 'Falling' : 'Stable',
      oil: oilTrend > 2 ? 'Rising' : oilTrend < -2 ? 'Falling' : 'Stable'
    };
  }

  predictOverheat() {
    if (this.history.length < 10) return null;

    const recent = this.history.slice(-10);
    const waterRate = (recent[9].water - recent[0].water) / 9;
    const oilRate = (recent[9].oil - recent[0].oil) / 9;

    const waterMinutesToOverheat = waterRate > 0 ? (120 - recent[9].water) / waterRate : null;
    const oilMinutesToOverheat = oilRate > 0 ? (140 - recent[9].oil) / oilRate : null;

    const warnings = [];
    if (waterMinutesToOverheat && waterMinutesToOverheat < 5) {
      warnings.push(`Water temp will reach critical in ~${waterMinutesToOverheat.toFixed(1)} updates`);
    }
    if (oilMinutesToOverheat && oilMinutesToOverheat < 5) {
      warnings.push(`Oil temp will reach critical in ~${oilMinutesToOverheat.toFixed(1)} updates`);
    }

    return warnings.length > 0 ? warnings : null;
  }
}
```

## Simulator Compatibility

| Property | iRacing | ACC | AC | rFactor 2 | F1 Games |
|----------|---------|-----|----|-----------| ---------|
| CarDamage | ✓ | ✓ | ~ | ✓ | ✓ |
| EngineTemperatures | ✓ | ✓ | ✓ | ✓ | ✓ |
| BrakeTemperatures | ✓ | ✓ | ✓ | ✓ | ✓ |
| TurboData | ✓* | ~ | ~ | ✓* | ✓* |
| WearIndicators | ✓ | ✓ | ~ | ✓ | ✓ |

**Legend:**
- ✓ = Fully supported
- ✓* = Supported in specific cars only
- ~ = Partially supported or basic implementation
- ✗ = Not available

**Notes:**
- Damage modeling varies significantly between simulators
- Some simulators provide visual-only damage vs. performance-affecting damage
- Turbo data only available for turbocharged cars
- Component wear may be simulated or cosmetic depending on simulator
- F1 games provide more detailed component wear for career mode

## Related Documentation

- [Tire Data](TIRE-DATA.md) - Brake temperatures also tracked in tire data
- [Car State](CAR-STATE.md) - Basic vehicle telemetry
- [Data Categories Overview](DATA-CATEGORIES.md) - All available data
