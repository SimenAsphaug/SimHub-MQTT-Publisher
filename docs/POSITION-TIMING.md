# Position & Timing Data

Race position, lap times, sector times, gaps to other drivers, and lap information.

## Overview

The `positionData` object contains competitive racing information including your position, timing data, and gaps to other competitors. Essential for racing and strategy presets.

## Data Structure

```json
{
  "positionData": {
    "Position": 3,
    "PositionInClass": 2,
    "Gap": 1.245,
    "GapToLeader": 5.678,
    "GapToAhead": 1.245,
    "GapToBehind": 2.134,
    "LastLapTime": 94.532,
    "BestLapTime": 93.821,
    "PersonalBestLapTime": 93.654,
    "SessionBestLapTime": 93.201,
    "DeltaToSessionBest": 0.620,
    "DeltaToPersonalBest": 0.167,
    "DeltaToOptimal": -0.054,
    "Sector1Time": 28.345,
    "Sector2Time": 35.678,
    "Sector3Time": 30.509,
    "Sector1BestTime": 28.123,
    "Sector2BestTime": 35.456,
    "Sector3BestTime": 30.422,
    "CurrentSector": 2,
    "CurrentLap": 12,
    "TotalLaps": 25,
    "CompletedLaps": 11
  }
}
```

## Position Information

### Position
**Type:** `integer` (nullable)
**Range:** 1 - number of cars in session

Overall race position among all cars.

### PositionInClass
**Type:** `integer` (nullable)
**Range:** 1 - number of cars in class

Position within your vehicle class (multi-class racing).

**Example:** P5 overall but P2 in GT3 class.

## Gap Information

All gaps are measured in **seconds**.

### Gap
**Type:** `double` (nullable)
**Unit:** seconds

Time gap to the car in the position ahead. Same as `GapToAhead` in most sims.

### GapToLeader
**Type:** `double` (nullable)
**Unit:** seconds

Time gap to the race leader (P1).

**Usage:**
```javascript
if (data.positionData.Position === 1) {
  console.log("You are the leader!");
} else {
  console.log(`${data.positionData.GapToLeader.toFixed(3)}s behind leader`);
}
```

### GapToAhead
**Type:** `double` (nullable)
**Unit:** seconds

Time gap to the car directly ahead.

### GapToBehind
**Type:** `double` (nullable)
**Unit:** seconds

Time gap to the car directly behind (positive = they're behind you).

**Usage:**
```javascript
// Pressure indicator
if (data.positionData.GapToBehind < 1.0) {
  showWarning("CAR BEHIND - " + data.positionData.GapToBehind.toFixed(1) + "s");
}
```

## Lap Times

All lap times are measured in **seconds**.

### LastLapTime
**Type:** `double` (nullable)
**Unit:** seconds

The time of your most recently completed lap.

### BestLapTime
**Type:** `double` (nullable)
**Unit:** seconds

Your fastest lap time in the current session.

### PersonalBestLapTime
**Type:** `double` (nullable)
**Unit:** seconds

Your all-time personal best lap time for this track/car combination.

**Note:** This may persist across sessions depending on the simulator.

### SessionBestLapTime
**Type:** `double` (nullable)
**Unit:** seconds

The fastest lap time achieved by any driver in the current session.

**Usage:**
```javascript
function formatLapTime(seconds) {
  const mins = Math.floor(seconds / 60);
  const secs = (seconds % 60).toFixed(3);
  return `${mins}:${secs.padStart(6, '0')}`;
}

console.log("Session Best:", formatLapTime(data.positionData.SessionBestLapTime));
console.log("Your Best:", formatLapTime(data.positionData.BestLapTime));
```

## Delta Times

All deltas are measured in **seconds**.
- **Positive value** = you're slower
- **Negative value** = you're faster

### DeltaToSessionBest
**Type:** `double` (nullable)
**Unit:** seconds

Real-time delta to the session best lap.

### DeltaToPersonalBest
**Type:** `double` (nullable)
**Unit:** seconds

Real-time delta to your personal best lap.

### DeltaToOptimal
**Type:** `double` (nullable)
**Unit:** seconds

Real-time delta to your theoretical optimal lap (best sectors combined).

**Usage:**
```javascript
function getDeltaColor(delta) {
  if (delta < -0.1) return 'green';  // Faster
  if (delta > 0.1) return 'red';     // Slower
  return 'white';                     // Even
}

const deltaColor = getDeltaColor(data.positionData.DeltaToPersonalBest);
const deltaText = (delta > 0 ? '+' : '') + delta.toFixed(3);
```

## Sector Times

Tracks are typically divided into 3 sectors. All times in **seconds**.

### Current Sector Times

- `Sector1Time` - Time for sector 1 in current lap
- `Sector2Time` - Time for sector 2 in current lap
- `Sector3Time` - Time for sector 3 in current lap

### Best Sector Times

- `Sector1BestTime` - Your best sector 1 time
- `Sector2BestTime` - Your best sector 2 time
- `Sector3BestTime` - Your best sector 3 time

### CurrentSector
**Type:** `integer` (nullable)
**Values:** 1, 2, or 3

The sector you're currently in.

**Usage:**
```javascript
function showSectorComparison(data) {
  const currentSector = data.positionData.CurrentSector;

  // When completing sector 1
  if (currentSector === 2 && data.positionData.Sector1Time) {
    const delta = data.positionData.Sector1Time - data.positionData.Sector1BestTime;
    console.log(`S1: ${formatDelta(delta)}`);
  }
}
```

## Lap Information

### CurrentLap
**Type:** `integer` (nullable)
**Range:** 1 - TotalLaps (or unlimited in practice)

The lap you're currently on.

### TotalLaps
**Type:** `integer` (nullable)

Total number of laps in the race. May be null in practice or time-limited sessions.

### CompletedLaps
**Type:** `integer` (nullable)

Number of laps you've completed.

**Usage:**
```javascript
const remaining = data.positionData.TotalLaps - data.positionData.CompletedLaps;
console.log(`${remaining} laps remaining`);

const progress = (data.positionData.CompletedLaps / data.positionData.TotalLaps) * 100;
console.log(`Race progress: ${progress.toFixed(1)}%`);
```

## Use Cases

### Race Progress Widget
```javascript
function createRaceProgressWidget(data) {
  const pos = data.positionData;

  return {
    position: `P${pos.Position}`,
    gap: pos.Position === 1
      ? 'Leader'
      : `+${pos.GapToLeader.toFixed(1)}s`,
    lapsRemaining: pos.TotalLaps - pos.CompletedLaps,
    percentComplete: (pos.CompletedLaps / pos.TotalLaps * 100).toFixed(1)
  };
}
```

### Lap Time Tracker
```javascript
class LapTimeTracker {
  constructor() {
    this.previousLap = 0;
    this.lapTimes = [];
  }

  update(data) {
    const currentLap = data.positionData?.CurrentLap;

    if (currentLap && currentLap > this.previousLap) {
      const lapTime = data.positionData.LastLapTime;

      this.lapTimes.push({
        lap: this.previousLap,
        time: lapTime,
        delta: lapTime - data.positionData.BestLapTime
      });

      this.onLapComplete(this.lapTimes[this.lapTimes.length - 1]);
      this.previousLap = currentLap;
    }
  }

  onLapComplete(lapData) {
    console.log(`Lap ${lapData.lap}: ${formatLapTime(lapData.time)} (${formatDelta(lapData.delta)})`);
  }

  getAverageLapTime() {
    if (this.lapTimes.length === 0) return 0;
    const sum = this.lapTimes.reduce((acc, lap) => acc + lap.time, 0);
    return sum / this.lapTimes.length;
  }
}
```

### Position Battle Alert
```javascript
function checkPositionBattle(data) {
  const gapAhead = data.positionData?.GapToAhead || Infinity;
  const gapBehind = data.positionData?.GapToBehind || Infinity;

  if (gapAhead < 2.0) {
    return {
      alert: true,
      message: `Catching P${data.positionData.Position - 1}`,
      urgency: 'medium'
    };
  }

  if (gapBehind < 1.0) {
    return {
      alert: true,
      message: `P${data.positionData.Position + 1} closing in!`,
      urgency: 'high'
    };
  }

  return { alert: false };
}
```

### Consistency Analysis
```javascript
function analyzeConsistency(lapTimes) {
  if (lapTimes.length < 3) return null;

  const times = lapTimes.map(l => l.time);
  const avg = times.reduce((a, b) => a + b) / times.length;

  const variance = times.reduce((acc, time) => {
    return acc + Math.pow(time - avg, 2);
  }, 0) / times.length;

  const stdDev = Math.sqrt(variance);

  return {
    average: avg,
    consistency: stdDev < 0.5 ? 'Excellent' :
                 stdDev < 1.0 ? 'Good' :
                 stdDev < 2.0 ? 'Fair' : 'Inconsistent',
    stdDeviation: stdDev
  };
}
```

## Simulator Compatibility

| Property | iRacing | ACC | AC | rFactor 2 | F1 Games |
|----------|---------|-----|----|-----------| ---------|
| Position | ✓ | ✓ | ✓ | ✓ | ✓ |
| PositionInClass | ✓ | ✓ | ~ | ✓ | ~ |
| Gaps | ✓ | ✓ | ✓ | ✓ | ✓ |
| Lap Times | ✓ | ✓ | ✓ | ✓ | ✓ |
| Delta Times | ✓ | ✓ | ~ | ✓ | ✓ |
| Sector Times | ✓ | ✓ | ✓ | ✓ | ✓ |
| Lap Info | ✓ | ✓ | ✓ | ✓ | ✓ |

## Related Documentation

- [Car State Data](CAR-STATE.md) - Basic vehicle state
- [Safety & Race Control](SAFETY-CONTROL.md) - Race status and safety car
- [Fuel & Energy](FUEL-ENERGY.md) - Fuel strategy
- [Data Categories Overview](DATA-CATEGORIES.md) - All available data
