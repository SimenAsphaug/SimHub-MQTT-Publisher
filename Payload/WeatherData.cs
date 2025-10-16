using GameReaderCommon;
using Newtonsoft.Json;

namespace SimHub.MQTTPublisher.Payload
{
    public class WeatherData
    {
        public WeatherData(GameData data, SimHubMQTTPublisherPluginSettings settings)
        {
            // Temperature
            if (settings.Include_AirTemperature)
                AirTemperature = GetSafeDoubleProperty(data, "AirTemperature");

            if (settings.Include_TrackTemperature)
            {
                TrackTemperature = GetSafeDoubleProperty(data, "TrackTemperature");
                RoadTemperature = GetSafeDoubleProperty(data, "RoadTemperature");
            }

            // Weather Conditions
            if (settings.Include_WeatherType)
            {
                WeatherType = GetSafeStringProperty(data, "WeatherType");
                IsWetTrack = GetSafeBoolProperty(data, "IsWetTrack");
            }

            if (settings.Include_RainLevel)
                RainLevel = GetSafeDoubleProperty(data, "RainLevel");

            if (settings.Include_Humidity)
                Humidity = GetSafeDoubleProperty(data, "Humidity");

            // Wind
            if (settings.Include_WindData)
            {
                WindSpeed = GetSafeDoubleProperty(data, "WindSpeed");
                WindDirection = GetSafeDoubleProperty(data, "WindDirection");
            }

            // Track Conditions
            if (settings.Include_TrackGrip)
            {
                TrackGrip = GetSafeDoubleProperty(data, "TrackGrip");
                TrackWetness = GetSafeDoubleProperty(data, "TrackWetness");
            }

            // Time and Lighting
            if (settings.Include_TimeOfDay)
            {
                TimeOfDay = GetSafeStringProperty(data, "TimeOfDay");
                DayTime = GetSafeDoubleProperty(data, "DayTime");
                IsNight = GetSafeBoolProperty(data, "IsNight");
                SunAngle = GetSafeDoubleProperty(data, "SunAngle");
            }
        }

        // Temperature
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? AirTemperature { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? TrackTemperature { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? RoadTemperature { get; set; }

        // Weather Conditions
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WeatherType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? RainLevel { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? Humidity { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsWetTrack { get; set; }

        // Wind
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? WindSpeed { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? WindDirection { get; set; }

        // Track Conditions
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? TrackGrip { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? TrackWetness { get; set; }

        // Time and Lighting
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TimeOfDay { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? DayTime { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsNight { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? SunAngle { get; set; }

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