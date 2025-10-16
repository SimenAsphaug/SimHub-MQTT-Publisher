using GameReaderCommon;
using Newtonsoft.Json;

namespace SimHub.MQTTPublisher.Payload
{
    public class DamageData
    {
        public DamageData(GameData data, SimHubMQTTPublisherPluginSettings settings)
        {
            // Car Damage
            if (settings.Include_CarDamage)
            {
                Engine = GetSafeDoubleProperty(data, "CarDamagesEngine");
                Transmission = GetSafeDoubleProperty(data, "CarDamagesTransmission");
                Aerodynamics = GetSafeDoubleProperty(data, "CarDamagesAero");
                Suspension = GetSafeDoubleProperty(data, "CarDamagesSuspension");
                Brakes = GetSafeDoubleProperty(data, "CarDamagesBrakes");
                Clutch = GetSafeDoubleProperty(data, "CarDamagesClutch");
            }

            // Engine Temperatures
            if (settings.Include_EngineTemperatures)
            {
                WaterTemperature = GetSafeDoubleProperty(data, "WaterTemperature");
                OilTemperature = GetSafeDoubleProperty(data, "OilTemperature");
                OilPressure = GetSafeDoubleProperty(data, "OilPressure");
                EngineTemperature = GetSafeDoubleProperty(data, "EngineTemperature");
            }

            // Brake Temperatures
            if (settings.Include_BrakeTemperatures)
            {
                BrakeTemperatureFL = GetSafeDoubleProperty(data, "BrakeTemperatureFL");
                BrakeTemperatureFR = GetSafeDoubleProperty(data, "BrakeTemperatureFR");
                BrakeTemperatureRL = GetSafeDoubleProperty(data, "BrakeTemperatureRL");
                BrakeTemperatureRR = GetSafeDoubleProperty(data, "BrakeTemperatureRR");
            }

            // Additional Mechanical Data
            if (settings.Include_TurboData)
            {
                TurboBoost = GetSafeDoubleProperty(data, "TurboBoost");
                Manifold = GetSafeDoubleProperty(data, "Manifold");
                ExhaustTemperature = GetSafeDoubleProperty(data, "ExhaustTemperature");
            }

            // Wear Indicators
            if (settings.Include_WearIndicators)
            {
                EngineWear = GetSafeDoubleProperty(data, "EngineWear");
                GearboxWear = GetSafeDoubleProperty(data, "GearboxWear");
                SuspensionWear = GetSafeDoubleProperty(data, "SuspensionWear");
            }
        }

        // Car Damage (typically 0-1 range where 1 is fully damaged)
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? Engine { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? Transmission { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? Aerodynamics { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? Suspension { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? Brakes { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? Clutch { get; set; }

        // Engine Temperatures and Pressures
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? WaterTemperature { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? OilTemperature { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? OilPressure { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? EngineTemperature { get; set; }

        // Brake Temperatures
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? BrakeTemperatureFL { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? BrakeTemperatureFR { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? BrakeTemperatureRL { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? BrakeTemperatureRR { get; set; }

        // Additional Mechanical Data
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? TurboBoost { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? Manifold { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? ExhaustTemperature { get; set; }

        // Wear Indicators
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? EngineWear { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? GearboxWear { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? SuspensionWear { get; set; }

        private double? GetSafeDoubleProperty(GameData data, string propertyName)
        {
            try
            {
                var property = data.NewData.GetType().GetProperty(propertyName);
                var value = property?.GetValue(data.NewData);
                if (value == null) return null;
                if (double.TryParse(value.ToString(), out double result))
                    return result;
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}