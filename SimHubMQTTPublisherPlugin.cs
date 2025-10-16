using GameReaderCommon;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Newtonsoft.Json;
using SimHub.MQTTPublisher.Settings;
using SimHub.Plugins;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SimHub.MQTTPublisher
{
    [PluginDescription("MQTT Publisher")]
    [PluginAuthor("Asphaug")]
    [PluginName("MQTT Publisher Enhanced")]
    public class SimHubMQTTPublisherPlugin : IPlugin, IDataPlugin, IWPFSettingsV2
    {
        public SimHubMQTTPublisherPluginSettings Settings;

        public SimHubMQTTPublisherPluginUserSettings UserSettings { get; private set; }

        private MqttFactory mqttFactory;
        private IMqttClient mqttClient;

        /// <summary>
        /// Instance of the current plugin manager
        /// </summary>
        public PluginManager PluginManager { get; set; }

        /// <summary>
        /// Gets the left menu icon. Icon must be 24x24 and compatible with black and white display.
        /// </summary>
        public ImageSource PictureIcon => this.ToIcon(Properties.Resources.sdkmenuicon);

        /// <summary>
        /// Gets a short plugin title to show in left menu. Return null if you want to use the title as defined in PluginName attribute.
        /// </summary>
        public string LeftMenuTitle => "MQTT Publisher";

        /// <summary>
        /// Called one time per game data update, contains all normalized game data,
        /// raw data are intentionnally "hidden" under a generic object type (A plugin SHOULD NOT USE IT)
        ///
        /// This method is on the critical path, it must execute as fast as possible and avoid throwing any error
        ///
        /// </summary>
        /// <param name="pluginManager"></param>
        /// <param name="data">Current game data, including current and previous data frame.</param>
        public void DataUpdate(PluginManager pluginManager, ref GameData data)
        {
            if (data.GameRunning)
            {
                var payload = JsonConvert.SerializeObject(
                    new Payload.PayloadRoot(data, UserSettings, Settings),
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

                // Resolve topic placeholders like {gameName}
                var resolvedTopic = ResolveTopic(Settings.Topic, data);

                var applicationMessage = new MqttApplicationMessageBuilder()
               .WithTopic(resolvedTopic)
               .WithPayload(payload)
               .Build();

                Task.Run(async () => await mqttClient.PublishAsync(applicationMessage, CancellationToken.None)).Wait();
            }
        }

        /// <summary>
        /// Called at plugin manager stop, close/dispose anything needed here !
        /// Plugins are rebuilt at game change
        /// </summary>
        /// <param name="pluginManager"></param>
        public void End(PluginManager pluginManager)
        {
            // Save settings
            this.SaveCommonSettings("GeneralSettings", Settings);
            this.SaveCommonSettings("UserSettings", UserSettings);
            mqttClient.Dispose();
        }

        /// <summary>
        /// Returns the settings control, return null if no settings control is required
        /// </summary>
        /// <param name="pluginManager"></param>
        /// <returns></returns>
        public System.Windows.Controls.Control GetWPFSettingsControl(PluginManager pluginManager)
        {
            return new SimHubMQTTPublisherPluginUI(this);
        }

        /// <summary>
        /// Called once after plugins startup
        /// Plugins are rebuilt at game change
        /// </summary>
        /// <param name="pluginManager"></param>
        public void Init(PluginManager pluginManager)
        {
            SimHub.Logging.Current.Info("Starting plugin");

            // Load settings
            Settings = this.ReadCommonSettings<SimHubMQTTPublisherPluginSettings>("GeneralSettings", () => new SimHubMQTTPublisherPluginSettings());

            UserSettings = this.ReadCommonSettings<SimHubMQTTPublisherPluginUserSettings>("UserSettings", () => new SimHubMQTTPublisherPluginUserSettings());

            this.mqttFactory = new MqttFactory();

            CreateMQTTClient();
        }

        internal void CreateMQTTClient()
        {
            var newmqttClient = mqttFactory.CreateMqttClient();

            var mqttClientOptions = new MqttClientOptionsBuilder()
               .WithTcpServer(Settings.Server)
               .WithCredentials(Settings.Login, Settings.Password)
               .Build();

            newmqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

            var oldMqttClient = this.mqttClient;

            mqttClient = newmqttClient;

            if (oldMqttClient != null)
            {
                oldMqttClient.Dispose();
            }
        }

        /// <summary>
        /// Resolves topic placeholders like {gameName}, {sessionType}, {trackName}, {carName} with actual values
        /// </summary>
        /// <param name="topic">Topic template with placeholders</param>
        /// <param name="data">Game data containing runtime values</param>
        /// <returns>Resolved topic string</returns>
        private string ResolveTopic(string topic, GameData data)
        {
            if (string.IsNullOrEmpty(topic))
                return topic;

            // Replace {gameName} with actual game name
            if (topic.Contains("{gameName}"))
            {
                var gameName = data.GameName ?? "Unknown";
                gameName = SanitizeTopicSegment(gameName);
                topic = topic.Replace("{gameName}", gameName);
            }

            // Replace {sessionType} with current session type (Practice, Qualifying, Race, etc.)
            if (topic.Contains("{sessionType}"))
            {
                var sessionType = GetPropertyValue(data, "SessionTypeName") ?? "Unknown";
                sessionType = SanitizeTopicSegment(sessionType);
                topic = topic.Replace("{sessionType}", sessionType);
            }

            // Replace {trackName} with current track/circuit name
            if (topic.Contains("{trackName}"))
            {
                var trackName = GetPropertyValue(data, "TrackName") ?? GetPropertyValue(data, "TrackDisplayName") ?? "Unknown";
                trackName = SanitizeTopicSegment(trackName);
                topic = topic.Replace("{trackName}", trackName);
            }

            // Replace {carName} with current vehicle name
            if (topic.Contains("{carName}"))
            {
                var carName = GetPropertyValue(data, "CarName") ?? GetPropertyValue(data, "CarModel") ?? "Unknown";
                carName = SanitizeTopicSegment(carName);
                topic = topic.Replace("{carName}", carName);
            }

            return topic;
        }

        /// <summary>
        /// Safely retrieves a property value from GameData using reflection
        /// </summary>
        /// <param name="data">Game data object</param>
        /// <param name="propertyName">Property name to retrieve</param>
        /// <returns>Property value as string, or null if not found</returns>
        private string GetPropertyValue(GameData data, string propertyName)
        {
            try
            {
                var property = data.NewData.GetType().GetProperty(propertyName);
                var value = property?.GetValue(data.NewData);
                return value?.ToString();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Sanitizes a string for use in MQTT topics (removes invalid characters)
        /// </summary>
        /// <param name="input">String to sanitize</param>
        /// <returns>Sanitized string safe for MQTT topics</returns>
        private string SanitizeTopicSegment(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "Unknown";

            // Replace spaces with underscores
            input = input.Replace(" ", "_");

            // Remove any characters that aren't alphanumeric, underscore, or hyphen
            input = System.Text.RegularExpressions.Regex.Replace(input, @"[^a-zA-Z0-9_\-]", "");

            // If result is empty, return Unknown
            return string.IsNullOrEmpty(input) ? "Unknown" : input;
        }
    }
}