using HarmonyLib;

namespace com.github.zehsteam.KidnapperFoxSettings.Patches;

[HarmonyPatch(typeof(RoundManager))]
internal class RoundManagerPatch
{
    [HarmonyPatch("Start")]
    [HarmonyPostfix]
    static void StartPatch()
    {
        SetBushWolfMaxSpawnCount();
    }

    private static void SetBushWolfMaxSpawnCount()
    {
        EnemyType bushWolfEnemyType = RoundManager.Instance.WeedEnemies.Find(_ => _.enemyType.enemyName == "Bush Wolf").enemyType;

        if (bushWolfEnemyType == null)
        {
            Plugin.logger.LogError("Error: Failed to set Kidnapper Fox max spawn count. Could not find BushWolf EnemyType in RoundManager WeedEnemies.");
            return;
        }

        bushWolfEnemyType.MaxCount = Plugin.ConfigManager.MaxSpawnCount.Value;
    }
}
