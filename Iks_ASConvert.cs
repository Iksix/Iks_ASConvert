using System.Data;
using System.Numerics;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Config;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;
using MySqlConnector;


namespace Iks_ASConvert;

public class Iks_ASConvert : BasePlugin, IPluginConfig<PluginConfig>
{
    public override string ModuleName => "Iks_ASConvert";

    public override string ModuleVersion => "0.0.1";
    public override string ModuleAuthor => "iks";

    private string _dbConnectionString = "";

    public PluginConfig Config { get; set; }

    public void OnConfigParsed(PluginConfig config)
    {
        _dbConnectionString = "Server=" + config.host + ";Database=" + config.database
                              + ";port=" + config.port + ";User Id=" + config.user + ";password=" + config.pass;

        Task.Run(async () =>
        {
            await SetFlagsToAdmins();
        });
        Config = config;
    }

    public async Task SetFlagsToAdmins()
    {
        List<Admin> admins = new List<Admin>();
        string sql =
            "SELECT * FROM as_admins";
        try
        {
            using (var connection = new MySqlConnection(_dbConnectionString))
            {
                connection.Open();
                var comm = new MySqlCommand(sql, connection);
                var reader = await comm.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    admins.Add(new Admin(reader.GetString("steamid"), reader.GetString("flags"), reader.GetInt32("immunity")));
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($" [Iks_AsConverter] Db error: {ex}");
        }

        Server.NextFrame(() =>
        {
            SetFlags(admins);
        });
    }


    public void SetFlags(List<Admin> admins)
    {
        foreach (var admin in admins)
        {
            AdminManager.ClearPlayerPermissions(admin.Steamid);
            AdminManager.SetPlayerImmunity(admin.Steamid, 0);

            foreach (var flags in Config.ConvertFlags)
            {
                if (admin.Flags.Contains(flags.Key))
                {
                    AdminManager.AddPlayerPermissions(admin.Steamid, flags.Value.ToArray());
                    AdminManager.SetPlayerImmunity(admin.Steamid, admin.Immunity < 0 ? 0 : (uint)admin.Immunity);
                    Console.WriteLine($"[Iks_AsConverter] Admin {admin.Steamid.ToString()} converted to {string.Join(", ", flags.Value)}");
                }
            }
        }
    }
    [CommandHelper(whoCanExecute: CommandUsage.SERVER_ONLY)]
    [ConsoleCommand("css_as_convert")]
    public void OnConvertCommand(CCSPlayerController? controller, CommandInfo info)
    {
        Task.Run(async () =>
        {
            await SetFlagsToAdmins();
        });
    }
}
