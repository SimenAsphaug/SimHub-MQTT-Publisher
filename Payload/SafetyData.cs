using GameReaderCommon;
using Newtonsoft.Json;

namespace SimHub.MQTTPublisher.Payload
{
    public class SafetyData
    {
        public SafetyData(GameData data, SimHubMQTTPublisherPluginSettings settings)
        {
            // Safety Car Information
            if (settings.Include_SafetyCarInfo)
            {
                SafetyCar = GetSafeBoolProperty(data, "SafetyCar");
                SafetyCarActive = GetSafeBoolProperty(data, "SafetyCarActive");
                VirtualSafetyCar = GetSafeBoolProperty(data, "VirtualSafetyCar");
                SafetyCarTime = GetSafeDoubleProperty(data, "SafetyCarTime");
            }

            // Flag Sectors
            if (settings.Include_FlagSectors)
            {
                YellowFlagSector1 = GetSafeBoolProperty(data, "YellowFlagSector1");
                YellowFlagSector2 = GetSafeBoolProperty(data, "YellowFlagSector2");
                YellowFlagSector3 = GetSafeBoolProperty(data, "YellowFlagSector3");
            }

            // Pit Information
            if (settings.Include_PitInformation)
            {
                IsInPitLane = GetSafeBoolProperty(data, "IsInPitLane");
                IsInPit = GetSafeBoolProperty(data, "IsInPit");
                PitSpeedLimit = GetSafeDoubleProperty(data, "PitSpeedLimit");
                PitLimiterOn = GetSafeBoolProperty(data, "PitLimiterOn");
                PitWindowStart = GetSafeIntProperty(data, "PitWindowStart");
                PitWindowEnd = GetSafeIntProperty(data, "PitWindowEnd");
                MandatoryPitDone = GetSafeBoolProperty(data, "MandatoryPitDone");
            }

            // Race Control
            if (settings.Include_RaceControl)
            {
                RaceStarted = GetSafeBoolProperty(data, "RaceStarted");
                RaceFinished = GetSafeBoolProperty(data, "RaceFinished");
                SessionPaused = GetSafeBoolProperty(data, "SessionPaused");
                IsReplay = GetSafeBoolProperty(data, "IsReplay");
                IsSpectator = GetSafeBoolProperty(data, "IsSpectator");
                RedFlagActive = GetSafeBoolProperty(data, "RedFlagActive");
                SessionStopped = GetSafeBoolProperty(data, "SessionStopped");
            }

            // Penalties
            if (settings.Include_Penalties)
            {
                HasPenalty = GetSafeBoolProperty(data, "HasPenalty");
                PenaltyTime = GetSafeDoubleProperty(data, "PenaltyTime");
                PenaltyCount = GetSafeIntProperty(data, "PenaltyCount");
            }

            // Formation/Rolling Start
            if (settings.Include_FormationLap)
            {
                FormationLap = GetSafeBoolProperty(data, "FormationLap");
                WarmupLap = GetSafeBoolProperty(data, "WarmupLap");
            }
        }

        // Safety Car Information
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? SafetyCar { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? SafetyCarActive { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? VirtualSafetyCar { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? SafetyCarTime { get; set; }

        // Flag Sectors
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? YellowFlagSector1 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? YellowFlagSector2 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? YellowFlagSector3 { get; set; }

        // Pit Information
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsInPitLane { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsInPit { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? PitSpeedLimit { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? PitLimiterOn { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? PitWindowStart { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? PitWindowEnd { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? MandatoryPitDone { get; set; }

        // Race Control
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? RaceStarted { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? RaceFinished { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? SessionPaused { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsReplay { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsSpectator { get; set; }

        // Penalties
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasPenalty { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? PenaltyTime { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? PenaltyCount { get; set; }

        // Formation/Rolling Start
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? FormationLap { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? WarmupLap { get; set; }

        // Emergency
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? RedFlagActive { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? SessionStopped { get; set; }

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