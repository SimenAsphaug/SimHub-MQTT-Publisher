using GameReaderCommon;
using Newtonsoft.Json;

namespace SimHub.MQTTPublisher.Payload
{
    public class TrackInformation
    {
        public TrackInformation(GameData data, SimHubMQTTPublisherPluginSettings settings)
        {
            if (settings.Include_TrackName)
            {
                this.TrackId = data.NewData.TrackId;
                this.TrackCode = data.NewData.TrackCode;
            }

            if (settings.Include_TrackConfiguration)
                this.TrackConfig = data.NewData.TrackConfig;

            if (settings.Include_TrackLength)
                this.TrackLength = data.NewData.TrackLength;
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TrackId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TrackConfig { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TrackCode { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? TrackLength { get; set; }
    }
}