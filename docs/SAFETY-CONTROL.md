# Safety & Race Control

Safety car status, pit information, race control events, and penalties.

## Overview

The `safetyData` object provides information about race control conditions, safety car deployment, pit lane status, and penalties. Critical for race strategy and awareness of session conditions.

## Data Structure

```json
{
  "safetyData": {
    "SafetyCarInfo": {
      "SafetyCarActive": false,
      "VSCActive": false,
      "FullCourseYellow": false
    },
    "FlagSectors": {
      "Sector1": "Green",
      "Sector2": "Yellow",
      "Sector3": "Green"
    },
    "PitInformation": {
      "PitLaneOpen": true,
      "PitLimiterActive": false,
      "InPitLane": false,
      "PitWindowOpen": true
    },
    "RaceControl": {
      "SessionActive": true,
      "SessionPaused": false,
      "RaceStarted": true
    },
    "Penalties": {
      "CurrentPenalty": null,
      "PendingPenalties": [],
      "ServedPenalties": 0
    },
    "FormationLap": false
  }
}
```

## Safety Car Properties

### SafetyCarInfo
**Type:** `object` (nullable)

Safety car and virtual safety car status information.

**Typical Properties:**

#### SafetyCarActive
**Type:** `boolean`

Indicates if the physical safety car is deployed on track.

**Impact:**
- No overtaking allowed (except to unlap yourself)
- Reduced speed (match safety car pace)
- Strategic pit stop opportunity
- Race timing and gaps frozen

**Usage:**
```javascript
function handleSafetyCar(safetyCarActive, currentPosition) {
  if (safetyCarActive) {
    return {
      alert: '🚗 SAFETY CAR DEPLOYED',
      strategy: currentPosition < 10
        ? 'Consider pitting - cheap pit stop window'
        : 'Stay out unless fuel/tire critical',
      rules: ['No overtaking', 'Match SC pace', 'Maintain gaps']
    };
  }
  return null;
}
```

#### VSCActive
**Type:** `boolean`

Virtual Safety Car active - drivers must maintain delta time.

**Impact:**
- Must stay within delta time (shown in sim)
- Smaller time loss for pit stops than full SC
- Can still be strategic pit window
- More consistent than full SC

**Usage:**
```javascript
function handleVSC(vscActive, fuelNeeded, tireWear) {
  if (vscActive) {
    const shouldPit = fuelNeeded || tireWear > 0.7;

    return {
      alert: '⚠️ VSC ACTIVE',
      advice: shouldPit
        ? 'Good time to pit - reduced time loss'
        : 'Maintain delta time',
      pitAdvantage: 'Approximately 50% of normal pit loss'
    };
  }
  return null;
}
```

#### FullCourseYellow
**Type:** `boolean`

Full course yellow flag (all sectors yellow).

**Impact:**
- Slower pace required
- No overtaking
- Prepare for safety car or VSC deployment

---

## Flag System

### FlagSectors
**Type:** `object` (nullable)

Flag status for each track sector.

**Typical Properties:**
- `Sector1`: Flag status in sector 1
- `Sector2`: Flag status in sector 2
- `Sector3`: Flag status in sector 3

**Common Flag Values:**
- `"Green"` - Normal racing
- `"Yellow"` - Caution, incident ahead, no overtaking
- `"Double Yellow"` - Incident on track, prepare to stop
- `"Blue"` - Being lapped, let faster car pass
- `"White"` - Slow vehicle on track
- `"Red"` - Session stopped

**Usage:**
```javascript
function analyzeFlagSituation(flagSectors, currentSector) {
  const flags = {
    1: flagSectors.Sector1,
    2: flagSectors.Sector2,
    3: flagSectors.Sector3
  };

  const warnings = [];
  const nextSector = (currentSector % 3) + 1;

  // Check upcoming sector
  if (flags[nextSector] === 'Yellow' || flags[nextSector] === 'Double Yellow') {
    warnings.push({
      severity: 'warning',
      message: `${flags[nextSector]} flag in upcoming Sector ${nextSector}`,
      advice: 'Prepare to slow down, no overtaking'
    });
  }

  if (flags[nextSector] === 'Blue') {
    warnings.push({
      severity: 'info',
      message: 'Blue flag in upcoming sector',
      advice: 'Prepare to let faster car pass'
    });
  }

  // Check for red flag
  if (Object.values(flags).includes('Red')) {
    warnings.push({
      severity: 'critical',
      message: 'RED FLAG - Session stopped',
      advice: 'Return to pits immediately'
    });
  }

  return {
    current: flags[currentSector],
    upcoming: flags[nextSector],
    warnings,
    raceable: !Object.values(flags).includes('Red')
  };
}
```

---

## Pit Lane Properties

### PitInformation
**Type:** `object` (nullable)

Pit lane and pit stop related information.

**Typical Properties:**

#### PitLaneOpen
**Type:** `boolean`

Indicates if pit lane is open for entry.

**Usage:**
```javascript
function checkPitAvailability(pitLaneOpen, fuelLow) {
  if (!pitLaneOpen && fuelLow) {
    return {
      alert: '⚠️ CRITICAL: Pit lane closed but fuel low!',
      action: 'Conserve fuel, wait for pit lane to open'
    };
  }

  if (pitLaneOpen) {
    return { status: 'Pit lane open', action: 'Can pit if needed' };
  }

  return { status: 'Pit lane closed', action: 'Cannot pit' };
}
```

#### PitLimiterActive
**Type:** `boolean`

Pit speed limiter currently engaged.

**Usage:**
```javascript
function checkPitLimiter(inPitLane, limiterActive) {
  if (inPitLane && !limiterActive) {
    return {
      alert: '🚨 WARNING: In pit lane without limiter!',
      consequence: 'Speed penalty likely',
      action: 'Engage pit limiter immediately'
    };
  }

  if (!inPitLane && limiterActive) {
    return {
      alert: '⚠️ Limiter active on track',
      consequence: 'Losing time',
      action: 'Disengage pit limiter'
    };
  }

  return null;
}
```

#### InPitLane
**Type:** `boolean`

Currently in pit lane.

#### PitWindowOpen
**Type:** `boolean`

Pit window is open (for races with mandatory pit stops).

**Usage:**
```javascript
function managePitWindow(pitWindowOpen, hasPitted, lapsRemaining) {
  if (!pitWindowOpen && !hasPitted) {
    return {
      alert: '⚠️ Pit window not yet open',
      advice: 'Wait for pit window to open before pitting'
    };
  }

  if (pitWindowOpen && !hasPitted && lapsRemaining < 5) {
    return {
      alert: '🚨 URGENT: Must pit soon!',
      advice: 'Pit window open, race almost over',
      urgency: 'high'
    };
  }

  return { status: 'Normal' };
}
```

---

## Race Control Properties

### RaceControl
**Type:** `object` (nullable)

Race session control and status.

**Typical Properties:**

#### SessionActive
**Type:** `boolean`

Session is currently active (not ended).

#### SessionPaused
**Type:** `boolean`

Session is paused (red flag, technical issue).

**Usage:**
```javascript
function checkSessionStatus(raceControl) {
  if (!raceControl.SessionActive) {
    return {
      status: 'Session ended',
      action: 'Return to pits'
    };
  }

  if (raceControl.SessionPaused) {
    return {
      status: 'Session paused (Red flag)',
      action: 'Maintain position, await restart',
      note: 'May be able to work on car depending on series rules'
    };
  }

  return { status: 'Session active', action: 'Continue racing' };
}
```

#### RaceStarted
**Type:** `boolean`

Race has started (lights out, green flag dropped).

---

## Penalty Properties

### Penalties
**Type:** `object` (nullable)

Penalty information and status.

**Typical Properties:**

#### CurrentPenalty
**Type:** `string` or `object` (nullable)

Currently active penalty (e.g., "5 second time penalty", "Drive-through").

**Common Penalty Types:**
- Time penalties (5s, 10s, 30s, etc.)
- Drive-through
- Stop-and-go
- Grid position penalty (for next race)
- Disqualification

#### PendingPenalties
**Type:** `array`

Penalties not yet served.

#### ServedPenalties
**Type:** `int`

Number of penalties already served.

**Usage:**
```javascript
function managePenalties(penalties, inPitLane) {
  const warnings = [];

  if (penalties.CurrentPenalty) {
    const penaltyType = typeof penalties.CurrentPenalty === 'string'
      ? penalties.CurrentPenalty
      : penalties.CurrentPenalty.Type;

    warnings.push({
      level: 'critical',
      message: `Active penalty: ${penaltyType}`,
      action: getPenaltyAction(penaltyType, inPitLane)
    });
  }

  if (penalties.PendingPenalties && penalties.PendingPenalties.length > 0) {
    warnings.push({
      level: 'warning',
      message: `${penalties.PendingPenalties.length} pending penalty(ies)`,
      action: 'Plan pit stop to serve penalties'
    });
  }

  return warnings;
}

function getPenaltyAction(penaltyType, inPitLane) {
  if (penaltyType.includes('Drive-through')) {
    return inPitLane
      ? 'Drive through pit lane without stopping'
      : 'Must pit to serve drive-through';
  }

  if (penaltyType.includes('Stop-and-go') || penaltyType.includes('Stop and go')) {
    return inPitLane
      ? 'Stop in pit box for required time'
      : 'Must pit for stop-and-go';
  }

  if (penaltyType.match(/\d+\s*second/i)) {
    return 'Time penalty will be added to race time or next pit stop';
  }

  return 'Check race control for penalty details';
}
```

---

## Formation Lap Property

### FormationLap
**Type:** `boolean`

Currently on formation/warmup lap (before race start).

**Usage:**
```javascript
function handleFormationLap(formationLap) {
  if (formationLap) {
    return {
      status: 'Formation lap',
      actions: [
        'Warm up tires (weaving)',
        'Warm up brakes',
        'Check systems',
        'Maintain grid position'
      ],
      warning: 'Do not overtake or gain unfair advantage'
    };
  }
  return null;
}
```

---

## Use Cases

### Race Strategy Manager

```javascript
class RaceStrategyManager {
  update(safetyData, fuelData, tireData, position) {
    return {
      safetyCar: this.analyzeSafetyCarStrategy(safetyData, fuelData, tireData, position),
      pitStrategy: this.analyzePitStrategy(safetyData, fuelData, tireData),
      penalties: this.managePenalties(safetyData.Penalties),
      warnings: this.getStrategicWarnings(safetyData)
    };
  }

  analyzeSafetyCarStrategy(safety, fuel, tires, position) {
    if (!safety.SafetyCarInfo) return null;

    const { SafetyCarActive, VSCActive } = safety.SafetyCarInfo;

    if (SafetyCarActive || VSCActive) {
      const needsService = fuel.FuelEstimatedLaps < 10 ||
                           Math.max(tires.TyreWearFrontLeft, tires.TyreWearFrontRight) > 0.6;

      return {
        opportunity: true,
        type: SafetyCarActive ? 'Full Safety Car' : 'VSC',
        recommendation: needsService
          ? 'PIT NOW - cheap pit stop window'
          : position < 5
            ? 'STAY OUT - protect track position'
            : 'Consider pitting for fresh tires',
        timeLoss: SafetyCarActive ? 'Minimal' : 'Reduced (~50%)'
      };
    }

    return { opportunity: false };
  }

  analyzePitStrategy(safety, fuel, tires) {
    if (!safety.PitInformation) return null;

    const canPit = safety.PitInformation.PitLaneOpen;
    const mustPit = safety.PitInformation.PitWindowOpen &&
                    !this.hasPitted; // Would need to track this

    const needsFuel = fuel.FuelEstimatedLaps < 5;
    const needsTires = Math.max(
      tires.TyreWearFrontLeft,
      tires.TyreWearFrontRight,
      tires.TyreWearRearLeft,
      tires.TyreWearRearRight
    ) > 0.8;

    if (!canPit && (needsFuel || needsTires)) {
      return {
        status: 'Critical',
        issue: 'Pit lane closed but service needed',
        action: 'Conserve and wait for pit lane to open'
      };
    }

    if (mustPit) {
      return {
        status: 'Mandatory',
        action: 'Must pit to comply with regulations',
        urgency: 'High'
      };
    }

    return { status: 'Optional' };
  }

  managePenalties(penalties) {
    if (!penalties) return null;

    if (penalties.CurrentPenalty ||
        (penalties.PendingPenalties && penalties.PendingPenalties.length > 0)) {
      return {
        hasPenalties: true,
        current: penalties.CurrentPenalty,
        pending: penalties.PendingPenalties?.length || 0,
        advice: 'Serve penalties during next pit stop to minimize time loss'
      };
    }

    return { hasPenalties: false };
  }

  getStrategicWarnings(safety) {
    const warnings = [];

    // Flag warnings
    if (safety.FlagSectors) {
      const hasYellow = Object.values(safety.FlagSectors).some(f =>
        f === 'Yellow' || f === 'Double Yellow'
      );
      if (hasYellow) {
        warnings.push('Yellow flags on track - incident ahead');
      }
    }

    // Formation lap
    if (safety.FormationLap) {
      warnings.push('Formation lap - warm up tires and brakes');
    }

    // Session paused
    if (safety.RaceControl?.SessionPaused) {
      warnings.push('RED FLAG - Session paused');
    }

    return warnings;
  }
}
```

### Pit Lane Monitor

```javascript
class PitLaneMonitor {
  constructor() {
    this.pitEntryTime = null;
    this.limiterWarned = false;
  }

  update(safetyData, speed) {
    const pit = safetyData.PitInformation;
    if (!pit) return null;

    const warnings = [];

    // Track pit entry
    if (pit.InPitLane && !this.pitEntryTime) {
      this.pitEntryTime = Date.now();
    } else if (!pit.InPitLane) {
      this.pitEntryTime = null;
      this.limiterWarned = false;
    }

    // Check limiter
    if (pit.InPitLane && !pit.PitLimiterActive && !this.limiterWarned) {
      warnings.push({
        level: 'critical',
        message: '🚨 PIT LIMITER NOT ACTIVE!',
        consequence: 'Speeding penalty imminent'
      });
      this.limiterWarned = true;
    }

    // Check speed (if available)
    if (pit.InPitLane && speed && speed > 80) { // Typical pit limit ~60-80 km/h
      warnings.push({
        level: 'critical',
        message: `SPEEDING IN PITS: ${speed.toFixed(0)} km/h`
      });
    }

    // Limiter active on track
    if (!pit.InPitLane && pit.PitLimiterActive) {
      warnings.push({
        level: 'warning',
        message: 'Pit limiter active on track - losing time!'
      });
    }

    return {
      inPits: pit.InPitLane,
      limiterActive: pit.PitLimiterActive,
      pitTime: this.pitEntryTime ? (Date.now() - this.pitEntryTime) / 1000 : 0,
      warnings
    };
  }
}
```

### Safety Car Opportunity Detector

```javascript
class SafetyCarDetector {
  constructor() {
    this.scDeployedTime = null;
    this.lastSCState = false;
  }

  update(safetyData, position, fuelNeeded, tireWear) {
    const sc = safetyData.SafetyCarInfo;
    if (!sc) return null;

    const scActive = sc.SafetyCarActive || sc.VSCActive;

    // Detect SC deployment
    if (scActive && !this.lastSCState) {
      this.scDeployedTime = Date.now();
      this.lastSCState = true;

      return this.evaluatePitOpportunity(sc, position, fuelNeeded, tireWear);
    }

    // SC withdrawn
    if (!scActive && this.lastSCState) {
      this.lastSCState = false;
      this.scDeployedTime = null;

      return {
        event: 'Safety car withdrawn',
        advice: 'Prepare for restart, warm tires',
        action: 'Race pace resume'
      };
    }

    return null;
  }

  evaluatePitOpportunity(sc, position, fuelNeeded, tireWear) {
    const scType = sc.SafetyCarActive ? 'Full SC' : 'VSC';
    const needsService = fuelNeeded > 10 || tireWear > 0.5;

    let recommendation;
    if (needsService) {
      recommendation = {
        action: 'PIT NOW',
        reason: 'Service needed + cheap pit window',
        priority: 'High'
      };
    } else if (position <= 3) {
      recommendation = {
        action: 'STAY OUT',
        reason: 'Protect podium position',
        priority: 'Medium'
      };
    } else if (position > 10) {
      recommendation = {
        action: 'CONSIDER PITTING',
        reason: 'Low risk position for fresh tire gamble',
        priority: 'Medium'
      };
    } else {
      recommendation = {
        action: 'EVALUATE',
        reason: 'Mid-field position - depends on strategy',
        priority: 'Low'
      };
    }

    return {
      event: `${scType} deployed`,
      opportunity: true,
      recommendation,
      timeLoss: scType === 'Full SC' ? '~5-10s' : '~15-20s'
    };
  }
}
```

## Simulator Compatibility

| Property | iRacing | ACC | AC | rFactor 2 | F1 Games |
|----------|---------|-----|----|-----------| ---------|
| SafetyCarInfo | ✓ | ✓ | ~ | ✓ | ✓ |
| FlagSectors | ✓ | ~ | ~ | ✓ | ✓ |
| PitInformation | ✓ | ✓ | ✓ | ✓ | ✓ |
| RaceControl | ✓ | ✓ | ~ | ✓ | ✓ |
| Penalties | ✓ | ✓ | ~ | ✓ | ✓ |
| FormationLap | ✓ | ✓ | ~ | ✓ | ✓ |

**Legend:**
- ✓ = Fully supported
- ~ = Partially supported or basic implementation
- ✗ = Not available

**Notes:**
- Safety car implementation varies by simulator and series
- VSC not available in all simulators or series
- Flag system detail varies (some sims have basic flags only)
- Penalty systems are series/game specific
- F1 games have most detailed penalty and safety car systems

## Related Documentation

- [Position & Timing](POSITION-TIMING.md) - Race position and timing affected by safety cars
- [Fuel & Energy](FUEL-ENERGY.md) - Fuel strategy during safety car periods
- [Flag Data](FLAG-DATA.md) - Detailed flag system information
- [Data Categories Overview](DATA-CATEGORIES.md) - All available data
