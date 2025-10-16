# Flag Data Reference

This document explains how to interpret flag data from the SimHub MQTT Publisher plugin.

## Overview

Flags are a critical component of racing, indicating track conditions and race status. The plugin provides flag information through the `flagState` object in the JSON payload.

## Data Structure

```json
{
  "flagState": {
    "Flags": 4,
    "FlagName": "Green",
    "GameName": "Assetto Corsa Competizione"
  }
}
```

## Properties

### `Flags` (integer)

Combined flag state as a bitwise integer. This allows multiple flags to be active simultaneously.

**Type:** `integer` (nullable)
**Example:** `4` (Green flag), `8` (Yellow flag), `12` (Green + Yellow)

### `FlagName` (string)

Human-readable name of the primary flag currently displayed.

**Type:** `string` (nullable)
**Example:** `"Green"`, `"Yellow"`, `"Checkered"`

**Note:** Not all simulators provide this property. When unavailable, rely on the `Flags` integer value.

### `GameName` (string)

Name of the racing simulator currently running.

**Type:** `string`
**Example:** `"Assetto Corsa Competizione"`, `"iRacing"`, `"rFactor 2"`

## Flag Values (iRacing Standard)

The plugin uses iRacing's flag bit values as the standard. Flags are represented as hexadecimal bitmasks:

| Flag | Hex Value | Decimal | Description |
|------|-----------|---------|-------------|
| Checkered | `0x00000001` | 1 | Race finished |
| White | `0x00000002` | 2 | Final lap |
| Green | `0x00000004` | 4 | Racing / Track clear |
| Yellow | `0x00000008` | 8 | Caution / Hazard on track |
| Red | `0x00000010` | 16 | Session stopped |
| Blue | `0x00000020` | 32 | Faster car approaching (move over) |
| Debris | `0x00000040` | 64 | Debris on track |
| Crossed | `0x00000080` | 128 | Halfway point |
| Yellow Waving | `0x00000100` | 256 | Waving yellow flag |
| One Lap to Green | `0x00000200` | 512 | One lap until restart |
| Green Held | `0x00000400` | 1024 | Green flag ready |
| Ten to Go | `0x00000800` | 2048 | 10 laps remaining |
| Five to Go | `0x00001000` | 4096 | 5 laps remaining |
| Random Waving | `0x00002000` | 8192 | Random waving flag |
| Full Course Caution | `0x00004000` | 16384 | Full course yellow |
| Full Course Caution Waving | `0x00008000` | 32768 | Waving full course yellow |
| Black | `0x00010000` | 65536 | Disqualification / Penalty |
| Disqualify | `0x00020000` | 131072 | Disqualification |

## Bitwise Operations

Since multiple flags can be active simultaneously, use bitwise AND operations to check for specific flags.

### JavaScript Example

```javascript
const flagData = {
  Flags: 12  // Binary: 1100 (Yellow + Green)
};

// Check for specific flag
function hasFlag(flags, flagValue) {
  return (flags & flagValue) !== 0;
}

// Check individual flags
const hasGreen = hasFlag(flagData.Flags, 4);    // true
const hasYellow = hasFlag(flagData.Flags, 8);   // true
const hasRed = hasFlag(flagData.Flags, 16);     // false
const hasBlue = hasFlag(flagData.Flags, 32);    // false

console.log(`Green: ${hasGreen}`);   // Green: true
console.log(`Yellow: ${hasYellow}`); // Yellow: true
console.log(`Red: ${hasRed}`);       // Red: false
```

### Python Example

```python
flag_data = {
    "Flags": 12  # Binary: 1100 (Yellow + Green)
}

# Check for specific flag
def has_flag(flags, flag_value):
    return (flags & flag_value) != 0

# Check individual flags
has_green = has_flag(flag_data["Flags"], 4)    # True
has_yellow = has_flag(flag_data["Flags"], 8)   # True
has_red = has_flag(flag_data["Flags"], 16)     # False
has_blue = has_flag(flag_data["Flags"], 32)    # False

print(f"Green: {has_green}")   # Green: True
print(f"Yellow: {has_yellow}") # Yellow: True
print(f"Red: {has_red}")       # Red: False
```

### C# Example

```csharp
var flagData = new { Flags = 12 }; // Binary: 1100 (Yellow + Green)

// Check for specific flag
bool HasFlag(int flags, int flagValue)
{
    return (flags & flagValue) != 0;
}

// Check individual flags
bool hasGreen = HasFlag(flagData.Flags, 4);    // true
bool hasYellow = HasFlag(flagData.Flags, 8);   // true
bool hasRed = HasFlag(flagData.Flags, 16);     // false
bool hasBlue = HasFlag(flagData.Flags, 32);    // false

Console.WriteLine($"Green: {hasGreen}");   // Green: true
Console.WriteLine($"Yellow: {hasYellow}"); // Yellow: true
Console.WriteLine($"Red: {hasRed}");       // Red: false
```

## Common Flag Combinations

### Racing Conditions

| Decimal Value | Flags Active | Meaning |
|---------------|--------------|---------|
| 4 | Green | Normal racing |
| 8 | Yellow | Caution period |
| 12 | Green + Yellow | Local yellow (some simulators) |
| 16 | Red | Session stopped |
| 32 | Blue | Faster car approaching |
| 36 | Green + Blue | Being lapped during racing |

### Race Start/End

| Decimal Value | Flags Active | Meaning |
|---------------|--------------|---------|
| 4 | Green | Race start / Restart |
| 512 | One Lap to Green | Final pace lap |
| 1024 | Green Held | About to go green |
| 1 | Checkered | Race finished |
| 2 | White | Final lap |

## Simulator-Specific Notes

### iRacing
- Provides comprehensive `SessionFlags` value with all flag combinations
- Most reliable source for flag data
- Supports all flag types listed above

### Assetto Corsa / ACC
- Provides individual flag properties (Flag_Green, Flag_Yellow, etc.)
- Plugin combines these into a single integer using iRacing bit values
- Some advanced flags may not be available

### rFactor 2
- Provides individual flag properties
- Plugin combines these into a single integer
- Flag availability depends on mod/track

### F1 Series
- Provides flag data through SimHub's normalized properties
- Plugin converts to standard bit values
- VSC (Virtual Safety Car) may appear as yellow flag

## Use Cases

### LED Flag Indicator

```javascript
// Control RGB LED strip based on flag status
function updateFlagLED(flags) {
  if (flags & 16) {
    // Red flag - solid red
    setLED(255, 0, 0);
  } else if (flags & 8) {
    // Yellow flag - solid yellow
    setLED(255, 255, 0);
  } else if (flags & 4) {
    // Green flag - solid green
    setLED(0, 255, 0);
  } else if (flags & 1) {
    // Checkered - animate black and white
    animateCheckered();
  } else {
    // No flag - off
    setLED(0, 0, 0);
  }
}
```

### Dashboard Warning

```javascript
// Show warning banner on dashboard
function getFlagWarning(flags) {
  if (flags & 16) {
    return { text: "RED FLAG - SESSION STOPPED", color: "red", urgent: true };
  }
  if (flags & 65536) {
    return { text: "BLACK FLAG - PIT IMMEDIATELY", color: "black", urgent: true };
  }
  if (flags & 8) {
    return { text: "YELLOW FLAG - SLOW DOWN", color: "yellow", urgent: false };
  }
  if (flags & 32) {
    return { text: "BLUE FLAG - FASTER CAR BEHIND", color: "blue", urgent: false };
  }
  return null;
}
```

### Data Logging

```python
# Log flag changes for post-race analysis
def log_flag_change(prev_flags, current_flags):
    if prev_flags != current_flags:
        flag_names = []
        if current_flags & 1: flag_names.append("Checkered")
        if current_flags & 2: flag_names.append("White")
        if current_flags & 4: flag_names.append("Green")
        if current_flags & 8: flag_names.append("Yellow")
        if current_flags & 16: flag_names.append("Red")
        if current_flags & 32: flag_names.append("Blue")
        if current_flags & 65536: flag_names.append("Black")

        timestamp = time.time()
        flags_str = ", ".join(flag_names) if flag_names else "None"

        print(f"[{timestamp}] Flag change: {flags_str} (value: {current_flags})")
        log_to_database(timestamp, current_flags, flags_str)
```

## Debug Mode

To see raw flag data from your simulator, enable Debug Mode in the plugin settings. The `debugData` object will include:

```json
{
  "debugData": {
    "Flag_Green": 1,
    "Flag_Yellow": 0,
    "Flag_Red": 0,
    "Flag_Blue": 0,
    "Flag_White": 0,
    "Flag_Black": 0,
    "Flag_Checkered": 0,
    "Flag_Orange": 0,
    "SessionFlags": 4
  }
}
```

This helps identify which flag properties are available for your specific simulator.

## Troubleshooting

**No flag data received:**
- Not all simulators provide flag information
- Enable Debug Mode to check what's available
- Some games only provide flags during online races

**Flag value is always 0:**
- May indicate no active flags (normal during practice)
- Check if simulator provides flag data in current session type
- Try online race or official session

**Multiple flags active unexpectedly:**
- This is normal - simulators can show multiple flags
- Example: Blue flag (32) during green (4) = 36
- Use bitwise operations to check specific flags independently

**FlagName doesn't match Flags value:**
- FlagName shows primary/most important flag only
- Flags integer shows all active flags
- Always prefer Flags integer for programmatic checks

## Related Documentation

- [Safety & Race Control](SAFETY-CONTROL.md) - Safety car and race control events
- [Position & Timing](POSITION-TIMING.md) - Race position and timing data
- [Data Categories Overview](DATA-CATEGORIES.md) - All available data categories
