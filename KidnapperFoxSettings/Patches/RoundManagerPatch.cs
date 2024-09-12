using HarmonyLib;

namespace com.github.zehsteam.KidnapperFoxSettings.Patches;

[HarmonyPatch(typeof(RoundManager))]
internal class RoundManagerPatch
{
    [HarmonyPatch(nameof(RoundManager.Start))]
    [HarmonyPostfix]
    private static void StartPatch()
    {
        SetBushWolfMaxSpawnCount();
    }

    private static void SetBushWolfMaxSpawnCount()
    {
        EnemyType enemyType = Utils.GetEnemyTypeFromResources("Bush Wolf");

        if (enemyType == null)
        {
            Plugin.logger.LogError("Failed to set Kidnapper Fox max spawn count. Could not find EnemyType.");
            return;
        }

        enemyType.MaxCount = Plugin.ConfigManager.MaxSpawnCount.Value;
    }
}
