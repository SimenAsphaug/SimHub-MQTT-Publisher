# Weather & Track Conditions

Environmental conditions affecting the race, including temperature, weather type, rain, wind, and track grip.

## Overview

The `weatherData` object provides critical environmental information that impacts car setup, tire choice, and race strategy. Weather conditions can dramatically affect grip levels, tire temperatures, and fuel consumption.

## Data Structure

```json
{
  "weatherData": {
    "AirTemperature": 24.5,
    "TrackTemperature": 32.8,
    "WeatherType": "Overcast",
    "RainLevel": 0.0,
    "Humidity": 62.0,
    "WindData": {
      "Speed": 8.5,
      "Direction": 185
    },
    "TrackGrip": 0.92,
    "TimeOfDay": "14:35"
  }
}
```

## Temperature Properties

### AirTemperature
**Type:** `double` (nullable)
**Unit:** Celsius (°C)

Ambient air temperature affecting aerodynamics and engine cooling.

**Impact:**
- Higher air temp = less air density = less downforce and engine power
- Lower air temp = better engine cooling but slower tire warm-up

**Usage:**
```javascript
function assessAirTemp(temp) {
  if (temp < 10) return { condition: 'Cold', impact: 'Slow tire warm-up, risk of graining' };
  if (temp < 20) return { condition: 'Cool', impact: 'Good engine cooling, moderate tire wear' };
  if (temp < 30) return { condition: 'Optimal', impact: 'Balanced conditions' };
  return { condition: 'Hot', impact: 'Overheating risk, high tire degradation' };
}

const airCondition = assessAirTemp(data.weatherData.AirTemperature);
console.log(`Air: ${data.weatherData.AirTemperature}°C - ${airCondition.impact}`);
```

---

### TrackTemperature
**Type:** `double` (nullable)
**Unit:** Celsius (°C)

Track surface temperature, crucial for tire performance.

**Impact:**
- Directly affects tire grip and wear rates
- Influences tire compound choice
- Changes throughout the day in dynamic weather

**Typical Ranges:**
- Cold: < 20°C (hard to get heat into tires)
- Moderate: 20-35°C (optimal for most compounds)
- Hot: 35-50°C (high wear, blistering risk)
- Extreme: > 50°C (severe degradation)

**Usage:**
```javascript
function recommendTireCompound(trackTemp) {
  if (trackTemp < 20) return 'Soft (helps with warm-up)';
  if (trackTemp < 35) return 'Medium (balanced)';
  return 'Hard (durability in hot conditions)';
}

const compound = recommendTireCompound(data.weatherData.TrackTemperature);
console.log(`Track: ${data.weatherData.TrackTemperature}°C → ${compound}`);
```

---

## Weather Condition Properties

### WeatherType
**Type:** `string` (nullable)

Current weather condition description.

**Common Values:**
- `"Clear"` - Sunny, dry conditions
- `"Partly Cloudy"` - Mixed sun and clouds
- `"Overcast"` - Fully clouded
- `"Light Rain"` - Drizzle
- `"Rain"` - Moderate rainfall
- `"Heavy Rain"` - Intense rainfall
- `"Thunderstorm"` - Storm conditions

**Usage:**
```javascript
function getTireChoice(weatherType) {
  const wetConditions = ['Light Rain', 'Rain', 'Heavy Rain', 'Thunderstorm'];

  if (wetConditions.includes(weatherType)) {
    return weatherType === 'Light Rain' ? 'Intermediates' : 'Full Wets';
  }

  return 'Slicks';
}

console.log(`Weather: ${data.weatherData.WeatherType}`);
console.log(`Recommended: ${getTireChoice(data.weatherData.WeatherType)}`);
```

---

### RainLevel
**Type:** `double` (nullable)
**Unit:** Normalized (0.0 = dry, 1.0 = heavy rain)

Rainfall intensity level.

**Ranges:**
- 0.0: Dry
- 0.0-0.2: Drizzle/light rain
- 0.2-0.5: Moderate rain
- 0.5-0.8: Heavy rain
- 0.8-1.0: Extreme rainfall

**Usage:**
```javascript
function analyzeRainConditions(rainLevel) {
  if (rainLevel === 0) return { status: 'Dry', tires: 'Slicks', visibility: 'Clear' };
  if (rainLevel < 0.2) return { status: 'Drizzle', tires: 'Slicks or Inters', visibility: 'Good' };
  if (rainLevel < 0.5) return { status: 'Moderate', tires: 'Intermediates', visibility: 'Moderate' };
  if (rainLevel < 0.8) return { status: 'Heavy', tires: 'Full Wets', visibility: 'Poor' };
  return { status: 'Extreme', tires: 'Full Wets', visibility: 'Very Poor' };
}

const rain = analyzeRainConditions(data.weatherData.RainLevel);
console.log(`Rain: ${(data.weatherData.RainLevel * 100).toFixed(0)}% - ${rain.status}`);
console.log(`Recommended tires: ${rain.tires}`);
```

---

### Humidity
**Type:** `double` (nullable)
**Unit:** Percentage (%)

Relative humidity level.

**Impact:**
- High humidity (>70%) can affect engine performance slightly
- Influences driver comfort and concentration
- Can indicate rain likelihood when combined with weather type

**Usage:**
```javascript
function assessHumidity(humidity) {
  if (humidity < 30) return 'Dry air - watch for static track conditions';
  if (humidity < 60) return 'Moderate - stable conditions';
  if (humidity < 80) return 'High - potential rain incoming';
  return 'Very high - rain likely or in progress';
}

console.log(`Humidity: ${data.weatherData.Humidity}%`);
console.log(assessHumidity(data.weatherData.Humidity));
```

---

## Wind Properties

### WindData
**Type:** `object` (nullable)

Wind speed and direction information.

**Typical Properties:**
- `Speed`: Wind speed (m/s or km/h depending on simulator)
- `Direction`: Wind direction in degrees (0-360, where 0/360 = North)

**Impact:**
- Headwind: Better braking, worse acceleration
- Tailwind: Worse braking, better acceleration
- Crosswind: Affects car balance and stability

**Usage:**
```javascript
function analyzeWindImpact(windData, trackDirection) {
  if (!windData || !windData.Speed) return 'No wind data';

  const speed = windData.Speed;
  const direction = windData.Direction;

  // Calculate relative wind angle to track
  const relativeAngle = Math.abs(direction - trackDirection) % 360;

  let windType;
  if (relativeAngle < 45 || relativeAngle > 315) windType = 'Headwind';
  else if (relativeAngle > 135 && relativeAngle < 225) windType = 'Tailwind';
  else windType = 'Crosswind';

  let intensity;
  if (speed < 5) intensity = 'Light';
  else if (speed < 15) intensity = 'Moderate';
  else intensity = 'Strong';

  return {
    type: windType,
    intensity,
    speed: speed.toFixed(1) + ' m/s',
    direction: direction + '°',
    advice: getWindAdvice(windType, intensity)
  };
}

function getWindAdvice(type, intensity) {
  if (intensity === 'Light') return 'Minimal impact';

  if (type === 'Headwind') return 'Later braking points, earlier acceleration';
  if (type === 'Tailwind') return 'Earlier braking, can risk later apex speed';
  return 'Expect car instability in fast corners';
}
```

---

## Track Grip Properties

### TrackGrip
**Type:** `double` (nullable)
**Unit:** Normalized (0.0 = no grip, 1.0 = maximum grip)

Overall track grip level.

**Factors:**
- Track temperature
- Weather conditions
- Track evolution (rubber buildup over session)
- Track surface type

**Usage:**
```javascript
function assessTrackGrip(grip) {
  if (grip > 0.95) return { level: 'Excellent', advice: 'Push hard, high grip available' };
  if (grip > 0.85) return { level: 'Good', advice: 'Normal racing conditions' };
  if (grip > 0.70) return { level: 'Moderate', advice: 'Reduced grip - smooth inputs' };
  if (grip > 0.50) return { level: 'Low', advice: 'Slippery - very smooth inputs required' };
  return { level: 'Critical', advice: 'Extremely low grip - drive carefully' };
}

const gripStatus = assessTrackGrip(data.weatherData.TrackGrip);
console.log(`Track Grip: ${(data.weatherData.TrackGrip * 100).toFixed(0)}% - ${gripStatus.level}`);
console.log(gripStatus.advice);
```

---

### TimeOfDay
**Type:** `string` (nullable)
**Format:** "HH:MM" (24-hour time)

Current time of day in the session (for dynamic time simulations).

**Impact:**
- Affects track temperature
- Changes lighting conditions
- Influences strategy in endurance races

**Usage:**
```javascript
function analyzeTimeOfDay(timeOfDay, sessionDuration) {
  const [hours, minutes] = timeOfDay.split(':').map(Number);
  const totalMinutes = hours * 60 + minutes;

  let period;
  if (totalMinutes < 6 * 60) period = 'Night';
  else if (totalMinutes < 10 * 60) period = 'Dawn';
  else if (totalMinutes < 17 * 60) period = 'Day';
  else if (totalMinutes < 20 * 60) period = 'Dusk';
  else period = 'Night';

  return {
    time: timeOfDay,
    period,
    trackTempTrend: period === 'Day' ? 'Rising' : 'Falling',
    visibility: period === 'Night' ? 'Headlights required' : 'Natural light'
  };
}

const timeAnalysis = analyzeTimeOfDay(data.weatherData.TimeOfDay);
console.log(`Time: ${timeAnalysis.time} (${timeAnalysis.period})`);
console.log(`Track temp trend: ${timeAnalysis.trackTempTrend}`);
```

---

## Use Cases

### Weather Dashboard

```javascript
class WeatherDashboard {
  update(weatherData) {
    return {
      summary: this.getSummary(weatherData),
      temperatures: this.getTemperatures(weatherData),
      conditions: this.getConditions(weatherData),
      recommendations: this.getRecommendations(weatherData)
    };
  }

  getSummary(w) {
    return `${w.WeatherType || 'Unknown'} • Track ${w.TrackTemperature?.toFixed(1) || '--'}°C • Air ${w.AirTemperature?.toFixed(1) || '--'}°C`;
  }

  getTemperatures(w) {
    return {
      air: `${w.AirTemperature?.toFixed(1) || '--'}°C`,
      track: `${w.TrackTemperature?.toFixed(1) || '--'}°C`,
      humidity: `${w.Humidity?.toFixed(0) || '--'}%`
    };
  }

  getConditions(w) {
    return {
      weather: w.WeatherType || 'Unknown',
      rain: w.RainLevel ? `${(w.RainLevel * 100).toFixed(0)}%` : 'Dry',
      wind: w.WindData ? `${w.WindData.Speed?.toFixed(1) || '--'} m/s @ ${w.WindData.Direction || '--'}°` : 'N/A',
      grip: w.TrackGrip ? `${(w.TrackGrip * 100).toFixed(0)}%` : 'N/A',
      time: w.TimeOfDay || 'N/A'
    };
  }

  getRecommendations(w) {
    const recommendations = [];

    // Track temperature recommendations
    if (w.TrackTemperature > 40) {
      recommendations.push('⚠️ Very hot track - expect high tire wear');
    } else if (w.TrackTemperature < 15) {
      recommendations.push('❄️ Cold track - difficulty warming tires');
    }

    // Weather recommendations
    if (w.RainLevel > 0.3) {
      recommendations.push('🌧️ Wet conditions - consider intermediate or wet tires');
    } else if (w.RainLevel > 0) {
      recommendations.push('☁️ Light rain - monitor conditions closely');
    }

    // Grip recommendations
    if (w.TrackGrip < 0.7) {
      recommendations.push('⚠️ Low grip - smooth inputs required');
    }

    return recommendations;
  }
}
```

### Dynamic Setup Advisor

```javascript
class SetupAdvisor {
  recommendSetup(weatherData) {
    const trackTemp = weatherData.TrackTemperature || 25;
    const rainLevel = weatherData.RainLevel || 0;
    const grip = weatherData.TrackGrip || 0.9;

    return {
      tires: this.recommendTires(rainLevel, trackTemp),
      downforce: this.recommendDownforce(trackTemp, rainLevel),
      suspension: this.recommendSuspension(grip, trackTemp),
      strategy: this.recommendStrategy(weatherData)
    };
  }

  recommendTires(rain, temp) {
    if (rain > 0.5) return 'Full Wet tires';
    if (rain > 0.1) return 'Intermediate tires';

    if (temp > 35) return 'Hard compound (durability in heat)';
    if (temp > 25) return 'Medium compound (balanced)';
    return 'Soft compound (generate temperature)';
  }

  recommendDownforce(temp, rain) {
    if (rain > 0.2) return 'High (stability in wet)';
    if (temp > 30) return 'Medium-High (manage tire temps)';
    return 'Track-dependent';
  }

  recommendSuspension(grip, temp) {
    if (grip < 0.7) return 'Softer (maximize contact)';
    return 'Balanced';
  }

  recommendStrategy(w) {
    const strategies = [];

    if (w.RainLevel > 0 && w.RainLevel < 0.3) {
      strategies.push('Variable weather - be ready for tire changes');
    }

    if (w.TimeOfDay) {
      const hour = parseInt(w.TimeOfDay.split(':')[0]);
      if (hour >= 16 && hour <= 19) {
        strategies.push('Track temp will drop - tires may lose performance');
      }
    }

    return strategies;
  }
}
```

### Weather Change Alert System

```javascript
class WeatherMonitor {
  constructor() {
    this.history = [];
    this.maxHistory = 20; // Track last 20 data points
  }

  update(weatherData) {
    this.history.push({
      timestamp: Date.now(),
      data: { ...weatherData }
    });

    if (this.history.length > this.maxHistory) {
      this.history.shift();
    }

    return this.detectChanges();
  }

  detectChanges() {
    if (this.history.length < 2) return { alerts: [] };

    const current = this.history[this.history.length - 1].data;
    const previous = this.history[this.history.length - 2].data;

    const alerts = [];

    // Rain change detection
    const rainChange = (current.RainLevel || 0) - (previous.RainLevel || 0);
    if (rainChange > 0.1) alerts.push('🌧️ Rain increasing!');
    if (rainChange < -0.1) alerts.push('☀️ Rain decreasing!');

    // Temperature trends
    const trackTempChange = (current.TrackTemperature || 0) - (previous.TrackTemperature || 0);
    if (Math.abs(trackTempChange) > 2) {
      alerts.push(`🌡️ Track temp ${trackTempChange > 0 ? 'rising' : 'falling'} rapidly`);
    }

    // Grip changes
    const gripChange = (current.TrackGrip || 0) - (previous.TrackGrip || 0);
    if (gripChange < -0.05) alerts.push('⚠️ Grip decreasing!');
    if (gripChange > 0.05) alerts.push('✅ Grip improving!');

    return {
      alerts,
      trends: this.calculateTrends()
    };
  }

  calculateTrends() {
    if (this.history.length < 5) return null;

    const recent = this.history.slice(-5);

    const trackTempTrend = recent[4].data.TrackTemperature - recent[0].data.TrackTemperature;
    const rainTrend = recent[4].data.RainLevel - recent[0].data.RainLevel;

    return {
      trackTemp: trackTempTrend > 1 ? 'Rising' : trackTempTrend < -1 ? 'Falling' : 'Stable',
      rain: rainTrend > 0.05 ? 'Increasing' : rainTrend < -0.05 ? 'Decreasing' : 'Stable'
    };
  }
}
```

## Simulator Compatibility

| Property | iRacing | ACC | AC | rFactor 2 | F1 Games |
|----------|---------|-----|----|-----------| ---------|
| AirTemperature | ✓ | ✓ | ✓ | ✓ | ✓ |
| TrackTemperature | ✓ | ✓ | ✓ | ✓ | ✓ |
| WeatherType | ✓ | ✓ | ~ | ✓ | ✓ |
| RainLevel | ✓ | ✓ | ~ | ✓ | ✓ |
| Humidity | ✓ | ~ | ✗ | ✓ | ✓ |
| WindData | ✓ | ✓ | ~ | ✓ | ✓ |
| TrackGrip | ✓ | ✓ | ~ | ✓ | ~ |
| TimeOfDay | ✓ | ✓ | ~ | ✓ | ✓ |

**Legend:**
- ✓ = Fully supported
- ~ = Partially supported or simulator-dependent
- ✗ = Not available

**Notes:**
- Dynamic weather systems vary significantly between simulators
- Some simulators have static weather, others feature full dynamic transitions
- Wind data may be simplified or estimated in some titles
- Track grip evolution depends on simulator's rubber simulation

## Related Documentation

- [Tire Data](TIRE-DATA.md) - Tire temperatures and performance affected by weather
- [Fuel & Energy](FUEL-ENERGY.md) - Fuel consumption affected by temperature
- [Data Categories Overview](DATA-CATEGORIES.md) - All available data
