
using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Core;
namespace Iks_ASConvert;

public class PluginConfig : BasePluginConfig
{
    [JsonPropertyName("host")] public string host { get; set; } = "host";
    [JsonPropertyName("database")] public string database { get; set; } = "database";
    [JsonPropertyName("user")] public string user { get; set; } = "user";
    [JsonPropertyName("pass")] public string pass { get; set; } = "pass";
    [JsonPropertyName("port")] public string port { get; set; } = "3306";

    [JsonPropertyName("ConvertFlags")]
    public Dictionary<string, List<string>> ConvertFlags { get; set; } = new Dictionary<string, List<string>>()
    {
        {"z", new List<string>() {
            "@css/root",
            "@css/ban",
            "@css/mute",
            "@css/gag"
        }},
        {"b", new List<string>() {
            "@css/ban"
        }},
        {"m", new List<string>() {
            "@css/mute"
        }}
    };
}
