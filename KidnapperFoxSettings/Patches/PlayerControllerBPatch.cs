using GameNetcodeStuff;
using HarmonyLib;

namespace com.github.zehsteam.KidnapperFoxSettings.Patches;

[HarmonyPatch(typeof(PlayerControllerB))]
internal class PlayerControllerBPatch
{
    [HarmonyPatch(nameof(PlayerControllerB.DropAllHeldItemsAndSync))]
    [HarmonyPrefix]
    private static bool DropAllHeldItemsAndSyncPatch()
    {
        if (BushWolfEnemyPatch.IsLocalPlayerBeingDragged())
        {
            return Plugin.ConfigManager.DropAllItemsWhenGrabbed.Value;
        }

        return true;
    }
}
