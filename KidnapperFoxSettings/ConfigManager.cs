using BepInEx.Configuration;
using System.Collections.Generic;
using System.Reflection;

namespace com.github.zehsteam.KidnapperFoxSettings;

internal class ConfigManager
{
    // Kidnapper Fox Settings
    public ConfigEntry<int> MaxSpawnCount;
    public ConfigEntry<int> Health;
    public ConfigEntry<bool> DropAllItemsWhenGrabbed;
    public ConfigEntry<bool> InstaKillPlayer;
    public ConfigEntry<int> Damage;
    public ConfigEntry<float> DamageInterval;
    
    public ConfigManager()
    {
        BindConfigs();
        ClearUnusedEntries();
    }

    private void BindConfigs()
    {
        ConfigFile configFile = Plugin.Instance.Config;

        // Kidnapper Fox Settings
        MaxSpawnCount = configFile.Bind("Kidnapper Fox Settings", "MaxSpawnCount", 1, "The max amount of Kidnapper Foxes that can spawn on a moon.");
        Health = configFile.Bind("Kidnapper Fox Settings", "Health", 7, "The amount of health (shovel hits) the Kidnapper Fox has.");
        DropAllItemsWhenGrabbed = configFile.Bind("Kidnapper Fox Settings", "DropAllItemsWhenGrabbed", true, "If enabled, you will drop all your items when the Kidnapper Fox grabs you with their tongue.");
        InstaKillPlayer = configFile.Bind("Kidnapper Fox Settings", "InstaKillPlayer", true, "If enabled, the Kidnapper Fox will insta-kill you when you are taken to their nest.");
        Damage = configFile.Bind("Kidnapper Fox Settings", "Damage", 50, "The amount of the damage the Kidnapper Fox will deal to players per bite. This setting requires InstaKillPlayer to be false.");
        DamageInterval = configFile.Bind("Kidnapper Fox Settings", "DamageInterval", 1f, "The seconds between each bite from the Kidnapper Fox when you are in their nest. This setting requires InstaKillPlayer to be false.");
    }

    private void ClearUnusedEntries()
    {
        ConfigFile configFile = Plugin.Instance.Config;

        // Normally, old unused config entries don't get removed, so we do it with this piece of code. Credit to Kittenji.
        PropertyInfo orphanedEntriesProp = configFile.GetType().GetProperty("OrphanedEntries", BindingFlags.NonPublic | BindingFlags.Instance);
        var orphanedEntries = (Dictionary<ConfigDefinition, string>)orphanedEntriesProp.GetValue(configFile, null);
        orphanedEntries.Clear(); // Clear orphaned entries (Unbinded/Abandoned entries)
        configFile.Save(); // Save the config file to save these changes
    }
}
