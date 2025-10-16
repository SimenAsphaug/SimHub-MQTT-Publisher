using GameReaderCommon;
using Newtonsoft.Json;

namespace SimHub.MQTTPublisher.Payload
{
    public class TireData
    {
        public TireData(GameData data, SimHubMQTTPublisherPluginSettings settings)
        {
            // Tire Temperatures
            if (settings.Include_TireTemperatures)
            {
                TemperatureFL = GetSafeDoubleProperty(data, "TyreTemperatureFL");
                TemperatureFR = GetSafeDoubleProperty(data, "TyreTemperatureFR");
                TemperatureRL = GetSafeDoubleProperty(data, "TyreTemperatureRL");
                TemperatureRR = GetSafeDoubleProperty(data, "TyreTemperatureRR");
            }

            // Tire Pressures
            if (settings.Include_TirePressures)
            {
                PressureFL = GetSafeDoubleProperty(data, "TyrePressureFL");
                PressureFR = GetSafeDoubleProperty(data, "TyrePressureFR");
                PressureRL = GetSafeDoubleProperty(data, "TyrePressureRL");
                PressureRR = GetSafeDoubleProperty(data, "TyrePressureRR");
            }

            // Tire Wear
            if (settings.Include_TireWear)
            {
                WearFL = GetSafeDoubleProperty(data, "TyreWearFL");
                WearFR = GetSafeDoubleProperty(data, "TyreWearFR");
                WearRL = GetSafeDoubleProperty(data, "TyreWearRL");
                WearRR = GetSafeDoubleProperty(data, "TyreWearRR");
            }

            // Tire Grip
            if (settings.Include_TireGrip)
            {
                GripFL = GetSafeDoubleProperty(data, "TyreGripFL");
                GripFR = GetSafeDoubleProperty(data, "TyreGripFR");
                GripRL = GetSafeDoubleProperty(data, "TyreGripRL");
                GripRR = GetSafeDoubleProperty(data, "TyreGripRR");
            }

            // Tire Compound
            if (settings.Include_TireCompound)
            {
                Compound = GetSafeStringProperty(data, "TyreCompound");
                CompoundShort = GetSafeStringProperty(data, "TyreCompoundShort");
            }

            // Tire Dirt
            if (settings.Include_TireDirt)
            {
                DirtFL = GetSafeDoubleProperty(data, "TyreDirtFrontLeft");
                DirtFR = GetSafeDoubleProperty(data, "TyreDirtFrontRight");
                DirtRL = GetSafeDoubleProperty(data, "TyreDirtRearLeft");
                DirtRR = GetSafeDoubleProperty(data, "TyreDirtRearRight");
            }
        }

        // Tire Temperatures
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? TemperatureFL { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? TemperatureFR { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? TemperatureRL { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? TemperatureRR { get; set; }

        // Tire Pressures
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? PressureFL { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? PressureFR { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? PressureRL { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? PressureRR { get; set; }

        // Tire Wear
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? WearFL { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? WearFR { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? WearRL { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? WearRR { get; set; }

        // Tire Grip
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? GripFL { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? GripFR { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? GripRL { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? GripRR { get; set; }

        // Tire Compound
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Compound { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CompoundShort { get; set; }

        // Tire Dirt
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? DirtFL { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? DirtFR { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? DirtRL { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? DirtRR { get; set; }

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

        private string GetSafeStringProperty(GameData data, string propertyName)
        {
            try
            {
                var property = data.NewData.GetType().GetProperty(propertyName);
                return property?.GetValue(data.NewData)?.ToString();
            }
            catch
            {
                return null;
            }
        }
    }
}