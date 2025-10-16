# Tire Data

Documentation for tire temperatures, pressures, wear, grip, and compound information.

## Coming Soon

This documentation page is under construction. In the meantime, refer to [Data Categories Overview](DATA-CATEGORIES.md) for a summary of available tire data properties.

## Quick Reference

The `tireData` object includes:
- **TireTemperatures**: Surface temps for all 4 tires `[FL, FR, RL, RR]` in °C
- **TirePressures**: Tire pressures `[FL, FR, RL, RR]` in PSI/kPa
- **TireWear**: Wear percentage `[FL, FR, RL, RR]` (0-100%)
- **TireGrip**: Grip levels `[FL, FR, RL, RR]` (0.0-1.0)
- **TireCompound**: Compound name (Soft, Medium, Hard, Wet, etc.)
- **TireDirt**: Dirt accumulation `[FL, FR, RL, RR]` (0.0-1.0)

## Example
```json
{
  "tireData": {
    "TireTemperatures": [85.3, 87.1, 82.5, 84.8],
    "TirePressures": [27.8, 27.9, 27.5, 27.6],
    "TireWear": [12.3, 15.7, 8.2, 10.1],
    "TireCompound": "Soft"
  }
}
```
