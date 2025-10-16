using GameReaderCommon;
using Newtonsoft.Json;

namespace SimHub.MQTTPublisher.Payload
{
    public class FuelData
    {
        public FuelData(GameData data, SimHubMQTTPublisherPluginSettings settings)
        {
            // Fuel Information
            if (settings.Include_Fuel)
            {
                Fuel = GetSafeDoubleProperty(data, "Fuel");
                FuelLevel = GetSafeDoubleProperty(data, "FuelLevel");
            }

            if (settings.Include_FuelCapacity)
                FuelCapacity = GetSafeDoubleProperty(data, "MaxFuel");  // SimHub/iRacing use "MaxFuel"

            if (settings.Include_FuelPerLap)
                FuelPerLap = GetSafeDoubleProperty(data, "FuelPerLap");

            if (settings.Include_FuelRemaining)
                FuelRemaining = GetSafeDoubleProperty(data, "Fuel");  // "Fuel" is the remaining fuel

            if (settings.Include_FuelEstimatedLaps)
                FuelEstimatedLaps = GetSafeDoubleProperty(data, "EstimatedFuelRemaingLaps");  // Note: SimHub has a typo "Remaing"

            if (settings.Include_FuelToEnd)
                FuelToEnd = GetSafeDoubleProperty(data, "FuelToEnd");

            // ERS (Energy Recovery System)
            if (settings.Include_ERS_Data)
            {
                ERS_DeployedThisLap = GetSafeDoubleProperty(data, "ERS_DeployedThisLap");
                ERS_EnergyStore = GetSafeDoubleProperty(data, "ERS_EnergyStore");
                ERS_MaxEnergyPerLap = GetSafeDoubleProperty(data, "ERS_MaxEnergyPerLap");
                ERSStored = GetSafeDoubleProperty(data, "ERSStored");
            }

            // DRS (Drag Reduction System)
            if (settings.Include_DRS_Data)
            {
                DRS_Available = GetSafeBoolProperty(data, "DRS_Available");
                DRS_Enabled = GetSafeBoolProperty(data, "DRS_Enabled");
                DRS_Count = GetSafeIntProperty(data, "DRS_Count");
            }

            // Battery (for electric vehicles)
            if (settings.Include_BatteryData)
            {
                BatteryLevel = GetSafeDoubleProperty(data, "BatteryLevel");
                BatteryTemperature = GetSafeDoubleProperty(data, "BatteryTemperature");
            }
        }

        // Fuel Information
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? Fuel { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? FuelCapacity { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? FuelPerLap { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? FuelRemaining { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? FuelEstimatedLaps { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? FuelToEnd { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? FuelLevel { get; set; }

        // ERS (Energy Recovery System)
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? ERS_DeployedThisLap { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? ERS_EnergyStore { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? ERS_MaxEnergyPerLap { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? ERSStored { get; set; }

        // DRS (Drag Reduction System)
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? DRS_Available { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? DRS_Enabled { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? DRS_Count { get; set; }

        // Battery (for electric vehicles)
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? BatteryLevel { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? BatteryTemperature { get; set; }

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

        private int? GetSafeIntProperty(GameData data, string propertyName)
        {
            try
            {
                var property = data.NewData.GetType().GetProperty(propertyName);
                var value = property?.GetValue(data.NewData);
                if (value == null) return null;
                if (int.TryParse(value.ToString(), out int result))
                    return result;
                return null;
            }
            catch
            {
                return null;
            }
        }

        private bool? GetSafeBoolProperty(GameData data, string propertyName)
        {
            try
            {
                var property = data.NewData.GetType().GetProperty(propertyName);
                var value = property?.GetValue(data.NewData);
                if (value == null) return null;

                if (value is bool boolValue)
                    return boolValue;
                if (value is int intValue)
                    return intValue == 1;
                if (bool.TryParse(value.ToString(), out bool parsedBool))
                    return parsedBool;

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}