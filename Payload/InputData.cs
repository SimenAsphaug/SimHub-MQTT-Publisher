using GameReaderCommon;
using Newtonsoft.Json;

namespace SimHub.MQTTPublisher.Payload
{
    public class InputData
    {
        public InputData(GameData data, SimHubMQTTPublisherPluginSettings settings)
        {
            // Steering
            if (settings.Include_SteeringInput)
            {
                SteeringAngle = GetSafeDoubleProperty(data, "SteeringAngle");
                SteeringInput = GetSafeDoubleProperty(data, "SteeringInput");
                SteeringWheelAngle = GetSafeDoubleProperty(data, "SteeringWheelAngle");
            }

            // Pedal Inputs (Raw values 0-1)
            if (settings.Include_PedalInputs)
            {
                ThrottleRaw = GetSafeDoubleProperty(data, "ThrottleRaw");
                BrakeRaw = GetSafeDoubleProperty(data, "BrakeRaw");
                ClutchRaw = GetSafeDoubleProperty(data, "ClutchRaw");
                Handbrake = GetSafeDoubleProperty(data, "Handbrake");
                PitLimiter = GetSafeBoolProperty(data, "PitLimiter");
            }

            // Driver Assists
            if (settings.Include_DriverAssists)
            {
                TractionControl = GetSafeIntProperty(data, "TractionControl");
                TractionControlLevel = GetSafeIntProperty(data, "TractionControlLevel");
                ABS = GetSafeIntProperty(data, "ABS");
                ABSLevel = GetSafeIntProperty(data, "ABSLevel");
                StabilityControl = GetSafeIntProperty(data, "StabilityControl");
                AutoClutch = GetSafeBoolProperty(data, "AutoClutch");
                AutoGear = GetSafeBoolProperty(data, "AutoGear");
            }

            // Electronic Systems
            if (settings.Include_ElectronicSystems)
            {
                ElectronicStabilityProgram = GetSafeBoolProperty(data, "ElectronicStabilityProgram");
                BrakeBias = GetSafeDoubleProperty(data, "BrakeBias");
                TractionControlCut = GetSafeDoubleProperty(data, "TractionControlCut");
            }

            // Input Device Info
            if (settings.Include_InputDeviceInfo)
            {
                IsKeyboard = GetSafeBoolProperty(data, "IsKeyboard");
                IsGamepad = GetSafeBoolProperty(data, "IsGamepad");
                IsWheel = GetSafeBoolProperty(data, "IsWheel");
            }
        }

        // Steering
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? SteeringAngle { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? SteeringInput { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? SteeringWheelAngle { get; set; }

        // Pedal Inputs (Raw values 0-1)
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? ThrottleRaw { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? BrakeRaw { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? ClutchRaw { get; set; }

        // Additional Controls
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? Handbrake { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? PitLimiter { get; set; }

        // Driver Assists
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? TractionControl { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? TractionControlLevel { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? ABS { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? ABSLevel { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? StabilityControl { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? AutoClutch { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? AutoGear { get; set; }

        // Electronic Systems
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? ElectronicStabilityProgram { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? BrakeBias { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? TractionControlCut { get; set; }

        // Input Device Info
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsKeyboard { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsGamepad { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsWheel { get; set; }

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