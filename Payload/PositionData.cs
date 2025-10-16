using GameReaderCommon;
using Newtonsoft.Json;

namespace SimHub.MQTTPublisher.Payload
{
    public class PositionData
    {
        public PositionData(GameData data, SimHubMQTTPublisherPluginSettings settings)
        {
            // Position Information
            if (settings.Include_Position)
                Position = GetSafeIntProperty(data, "Position");
            if (settings.Include_PositionInClass)
                PositionInClass = GetSafeIntProperty(data, "PositionInClass");

            // Gap Information
            if (settings.Include_Gap)
                Gap = GetSafeDoubleProperty(data, "Gap");
            if (settings.Include_GapToLeader)
                GapToLeader = GetSafeDoubleProperty(data, "GapToLeader");
            if (settings.Include_GapToAhead)
                GapToAhead = GetSafeDoubleProperty(data, "GapToAhead");
            if (settings.Include_GapToBehind)
                GapToBehind = GetSafeDoubleProperty(data, "GapToBehind");

            // Lap Times
            if (settings.Include_LastLapTime)
                LastLapTime = GetSafeDoubleProperty(data, "LastLapTime");
            if (settings.Include_BestLapTime)
                BestLapTime = GetSafeDoubleProperty(data, "BestLapTime");
            if (settings.Include_PersonalBestLapTime)
                PersonalBestLapTime = GetSafeDoubleProperty(data, "PersonalBestLapTime");
            if (settings.Include_SessionBestLapTime)
                SessionBestLapTime = GetSafeDoubleProperty(data, "SessionBestLapTime");

            // Delta Times
            if (settings.Include_DeltaToSessionBest)
                DeltaToSessionBest = GetSafeDoubleProperty(data, "DeltaToSessionBest");
            if (settings.Include_DeltaToPersonalBest)
                DeltaToPersonalBest = GetSafeDoubleProperty(data, "DeltaToPersonalBest");
            if (settings.Include_DeltaToOptimal)
                DeltaToOptimal = GetSafeDoubleProperty(data, "DeltaToOptimal");

            // Sector Times
            if (settings.Include_Sector1Time)
                Sector1Time = GetSafeDoubleProperty(data, "Sector1Time");
            if (settings.Include_Sector2Time)
                Sector2Time = GetSafeDoubleProperty(data, "Sector2Time");
            if (settings.Include_Sector3Time)
                Sector3Time = GetSafeDoubleProperty(data, "Sector3Time");
            if (settings.Include_Sector1BestTime)
                Sector1BestTime = GetSafeDoubleProperty(data, "Sector1BestTime");
            if (settings.Include_Sector2BestTime)
                Sector2BestTime = GetSafeDoubleProperty(data, "Sector2BestTime");
            if (settings.Include_Sector3BestTime)
                Sector3BestTime = GetSafeDoubleProperty(data, "Sector3BestTime");
            if (settings.Include_CurrentSector)
                CurrentSector = GetSafeIntProperty(data, "CurrentSector");

            // Lap Information
            if (settings.Include_CurrentLap)
                CurrentLap = GetSafeIntProperty(data, "CurrentLap");
            if (settings.Include_TotalLaps)
                TotalLaps = GetSafeIntProperty(data, "TotalLaps");
            if (settings.Include_CompletedLaps)
                CompletedLaps = GetSafeIntProperty(data, "CompletedLaps");
        }

        // Position Information
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Position { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? PositionInClass { get; set; }

        // Gap Information
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? Gap { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? GapToLeader { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? GapToAhead { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? GapToBehind { get; set; }

        // Lap Times
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? LastLapTime { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? BestLapTime { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? PersonalBestLapTime { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? SessionBestLapTime { get; set; }

        // Delta Times
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? DeltaToSessionBest { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? DeltaToPersonalBest { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? DeltaToOptimal { get; set; }

        // Sector Times
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? Sector1Time { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? Sector2Time { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? Sector3Time { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? Sector1BestTime { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? Sector2BestTime { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? Sector3BestTime { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? CurrentSector { get; set; }

        // Lap Information
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? CurrentLap { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? TotalLaps { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? CompletedLaps { get; set; }

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
    }
}