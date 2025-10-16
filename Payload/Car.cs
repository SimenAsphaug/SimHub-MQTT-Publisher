using GameReaderCommon;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace SimHub.MQTTPublisher.Payload
{
    public class Car
    {
        public Car(GameData data, SimHubMQTTPublisherPluginSettings settings)
        {
            if (settings.Include_SpeedKmh)
                SpeedKmh = data.NewData.SpeedKmh;

            if (settings.Include_Rpms)
                Rpms = data.NewData.Rpms;

            if (settings.Include_Clutch)
                Clutch = data.NewData.Clutch;

            if (settings.Include_Throttle)
                Throttle = data.NewData.Throttle;

            if (settings.Include_Brake)
                Brake = data.NewData.Brake;

            if (settings.Include_Gear)
                Gear = data.NewData.Gear;

            if (settings.Include_CarCoordinates)
                CarCoordinates = data.NewData.CarCoordinates.ToList();

            if (settings.Include_CurrentLapTime)
                CurrentLapTime = data.NewData.CurrentLapTime.TotalMilliseconds;

            if (settings.Include_CarModel)
                CarModel = data.NewData.CarModel;

            if (settings.Include_CarClass)
                CarClass = data.NewData.CarClass;

            if (settings.Include_EngineIgnitionOn)
                EngineIgnitionOn = data.NewData.EngineIgnitionOn == 1;

            if (settings.Include_EngineStarted)
                EngineStarted = data.NewData.EngineStarted == 1;
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? SpeedKmh { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? Rpms { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? Brake { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? Throttle { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? Clutch { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Gear { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<double> CarCoordinates { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? CurrentLapTime { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CarModel { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CarClass { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? EngineIgnitionOn { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? EngineStarted { get; set; }
    }
}