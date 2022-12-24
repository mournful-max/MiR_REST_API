using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MiR_REST_API.ResponseModels
{
    public sealed class Status
    {
        [JsonPropertyName("joystick_low_speed_mode_enabled")]
        public bool? JoystickLowSpeedModeEnabled;
        [JsonPropertyName("mission_queue_url")]
        public string MissionQueueUrl;
        [JsonPropertyName("mode_id")]
        public int? ModeId;
        [JsonPropertyName("moved")]
        public double? Moved;
        [JsonPropertyName("mission_queue_id")]
        public int? MissionQueueId;
        [JsonPropertyName("robot_name")]
        public string RobotName;
        [JsonPropertyName("joystick_web_session_id")]
        public string JoystickWebSessionId;
        [JsonPropertyName("uptime")]
        public int? Uptime;
        [JsonPropertyName("errors")]
        public List<ErrorSchema> Errors;
        [JsonPropertyName("unloaded_map_changes")]
        public bool? UnloadedMapChanges;
        [JsonPropertyName("distance_to_next_target")]
        public double? DistanceToNextTarget;
        [JsonPropertyName("serial_number")]
        public string SerialNumber;
        [JsonPropertyName("mode_key_state")]
        public string ModeKeyState;
        [JsonPropertyName("battery_percentage")]
        public double? BatteryPercentage;
        [JsonPropertyName("map_id")]
        public string MapId;
        [JsonPropertyName("safety_system_muted")]
        public bool? SafetySystemMuted;
        [JsonPropertyName("mission_text")]
        public string MissionText;
        [JsonPropertyName("state_text")]
        public string StateText;
        [JsonPropertyName("velocity")]
        public VelocitySchema Velocity;
        [JsonPropertyName("footprint")]
        public string Footprint;
        [JsonPropertyName("user_prompt")]
        public UserPromptSchema UserPrompt;
        [JsonPropertyName("allowed_methods")]
        public List<string> AllowedMethods;
        [JsonPropertyName("robot_model")]
        public string RobotModel;
        [JsonPropertyName("mode_text")]
        public string ModeText;
        [JsonPropertyName("session_id")]
        public string SessionId;
        [JsonPropertyName("state_id")]
        public int? StateId;
        [JsonPropertyName("battery_time_remaining")]
        public int? BatteryTimeRemaining;
        [JsonPropertyName("position")]
        public PositionSchema Position;


        public sealed class ErrorSchema
        {
            [JsonPropertyName("code")]
            public int? Code;
            [JsonPropertyName("description")]
            public string Description;
            [JsonPropertyName("module")]
            public string Module;
        }
        public sealed class VelocitySchema
        {
            [JsonPropertyName("angular")]
            public double? Angular;
            [JsonPropertyName("linear")]
            public double? Linear;
        }
        public sealed class UserPromptSchema
        {
            [JsonPropertyName("guid")]
            public string Guid;
            [JsonPropertyName("options")]
            public string[] Options;
            [JsonPropertyName("question")]
            public string Question;
            [JsonPropertyName("timeout")]
            public double? Timeout;
            [JsonPropertyName("user_group")]
            public string UserGroup;
        }
        public sealed class PositionSchema
        {
            [JsonPropertyName("y")]
            public double? Y;
            [JsonPropertyName("x")]
            public double? X;
            [JsonPropertyName("orientation")]
            public double? Orientation;
        }
    }
}
