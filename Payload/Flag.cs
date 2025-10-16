using GameReaderCommon;
using Newtonsoft.Json;

namespace SimHub.MQTTPublisher.Payload
{
    /// <summary>
    /// Flag information for racing simulators
    ///
    /// iRacing Flag Values (SessionFlags):
    /// 0x00000001 (1) = Checkered flag
    /// 0x00000002 (2) = White flag (1 lap to go)
    /// 0x00000004 (4) = Green flag (start/restart)
    /// 0x00000008 (8) = Yellow flag (caution)
    /// 0x00000010 (16) = Red flag (session stopped)
    /// 0x00000020 (32) = Blue flag (faster car approaching)
    /// 0x00000040 (64) = Debris flag
    /// 0x00000080 (128) = Crossed flag
    /// 0x00000100 (256) = Yellow waving
    /// 0x00000200 (512) = One lap to green
    /// 0x00000400 (1024) = Green held
    /// 0x00000800 (2048) = Ten to go
    /// 0x00001000 (4096) = Five to go
    /// 0x00002000 (8192) = Random waving
    /// 0x00004000 (16384) = Full course caution
    /// 0x00008000 (32768) = Full course caution waving
    /// 0x00010000 (65536) = Black flag
    /// 0x00020000 (131072) = Disqualify flag
    ///
    /// Note: Flags can be combined (bitwise OR). Use bitwise AND to check individual flags.
    /// Example: (Flags & 4) != 0 checks for green flag
    /// </summary>
    public class Flag
    {
        public Flag(GameData data, SimHubMQTTPublisherPluginSettings settings)
        {
            if (settings.Include_Flags)
            {
                // Try iRacing SessionFlags first (most comprehensive)
                Flags = GetSafeIntProperty(data, "SessionFlags");

                // If SessionFlags not available, build from individual flag properties
                if (!Flags.HasValue)
                {
                    Flags = BuildFlagsFromIndividualProperties(data);
                }

                // If still null or 0, default to 0 to show the property exists
                if (!Flags.HasValue)
                {
                    Flags = 0;
                }
            }

            // Additional metadata
            if (settings.Include_FlagName)
                FlagName = GetSafeStringProperty(data, "Flag_Name");

            // Note: GameName has been moved to root level in PayloadRoot

            // Debug flags - only create object if enabled
            if (settings.Include_DebugFlags)
            {
                DebugFlags = new
                {
                    Flag_Green = GetSafeIntProperty(data, "Flag_Green"),
                    Flag_Yellow = GetSafeIntProperty(data, "Flag_Yellow"),
                    Flag_Red = GetSafeIntProperty(data, "Flag_Red"),
                    Flag_Blue = GetSafeIntProperty(data, "Flag_Blue"),
                    Flag_White = GetSafeIntProperty(data, "Flag_White"),
                    Flag_Black = GetSafeIntProperty(data, "Flag_Black"),
                    Flag_Checkered = GetSafeIntProperty(data, "Flag_Checkered"),
                    Flag_Orange = GetSafeIntProperty(data, "Flag_Orange"),
                    SessionFlags = GetSafeIntProperty(data, "SessionFlags")
                };
            }
            // If Include_DebugFlags is false, DebugFlags remains null and won't be serialized
        }

        /// <summary>
        /// Combined flag state as integer. Use bitwise operations to check individual flags.
        /// See class documentation for flag values per game.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Flags { get; set; }

        /// <summary>
        /// Human-readable flag name (if available)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FlagName { get; set; }

        /// <summary>
        /// Debug information showing individual flag states (temporary)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object DebugFlags { get; set; }

        private int? BuildFlagsFromIndividualProperties(GameData data)
        {
            int flags = 0;

            // Build flags using SimHub's normalized flag properties
            // Using iRacing bit values as the standard
            if (GetSafeBoolProperty(data, "Flag_Checkered") == true) flags |= 0x1;      // 1
            if (GetSafeBoolProperty(data, "Flag_White") == true) flags |= 0x2;          // 2
            if (GetSafeBoolProperty(data, "Flag_Green") == true) flags |= 0x4;          // 4
            if (GetSafeBoolProperty(data, "Flag_Yellow") == true) flags |= 0x8;         // 8
            if (GetSafeBoolProperty(data, "Flag_Red") == true) flags |= 0x10;           // 16
            if (GetSafeBoolProperty(data, "Flag_Blue") == true) flags |= 0x20;          // 32
            if (GetSafeBoolProperty(data, "Flag_Orange") == true) flags |= 0x40;        // 64 (debris equivalent)
            if (GetSafeBoolProperty(data, "Flag_Black") == true) flags |= 0x10000;      // 65536

            return flags == 0 ? (int?)null : (int?)flags;
        }

        private object GetSafeProperty(GameData data, string propertyName)
        {
            try
            {
                var property = data.NewData.GetType().GetProperty(propertyName);
                return property?.GetValue(data.NewData);
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

        private int? GetSafeIntProperty(GameData data, string propertyName)
        {
            try
            {
                var property = data.NewData.GetType().GetProperty(propertyName);
                var value = property?.GetValue(data.NewData);
                if (value == null) return null;

                if (value is int intValue)
                    return intValue;
                if (int.TryParse(value.ToString(), out int parsedInt))
                    return parsedInt;

                return null;
            }
            catch
            {
                return null;
            }
        }

    }
}