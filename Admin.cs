
using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities;
namespace Iks_ASConvert;

public class Admin
{
    public SteamID Steamid;
    public string Flags;

    public Admin(string steamid, string flags)
    {
        Steamid = new SteamID(UInt64.Parse(steamid));
        Flags = flags;
    }
}
