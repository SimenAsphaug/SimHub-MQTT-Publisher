using GameReaderCommon;
using Newtonsoft.Json;

namespace SimHub.MQTTPublisher.Payload
{
    public class VehicleInformation
    {
        public VehicleInformation(GameData data, SimHubMQTTPublisherPluginSettings settings)
        {
            if (settings.Include_VehicleModel)
            {
                this.CarModel = data.NewData.CarModel;
                this.CarId = data.NewData.CarId;
            }

            if (settings.Include_VehicleClass)
                this.CarClass = data.NewData.CarClass;

            if (settings.Include_MaxRpm)
                this.MaxRpm = data.NewData.MaxRpm;
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CarModel { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CarClass { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CarId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? MaxRpm { get; set; }
    }
}