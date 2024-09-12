using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;

namespace com.github.zehsteam.KidnapperFoxSettings.Patches;

[HarmonyPatch(typeof(BushWolfEnemy))]
internal class BushWolfEnemyPatch
{
    [HarmonyPatch(nameof(BushWolfEnemy.Start))]
    [HarmonyPostfix]
    private static void StartPatch(ref BushWolfEnemy __instance)
    {
        __instance.enemyHP = Plugin.ConfigManager.Health.Value;
    }

    [HarmonyPatch(nameof(BushWolfEnemy.OnCollideWithPlayer))]
    [HarmonyPrefix]
    private static bool OnCollideWithPlayerPatch(ref BushWolfEnemy __instance, Collider other, ref bool ___foundSpawningPoint, ref bool ___inKillAnimation, ref Vector3 ___currentHidingSpot, ref float ___timeSinceTakingDamage, ref PlayerControllerB ___lastHitByPlayer, ref bool ___dragging, ref bool ___startedShootingTongue)
    {
        if (Plugin.ConfigManager.InstaKillPlayer.Value)
        {
            return true;
        }

        if (___foundSpawningPoint && !___inKillAnimation && !__instance.isEnemyDead && __instance.MeetsStandardPlayerCollisionConditions(other, ___inKillAnimation) != null)
        {
            float num = Vector3.Distance(__instance.transform.position, ___currentHidingSpot);

            bool flag = false;

            if (___timeSinceTakingDamage < 2.5f && ___lastHitByPlayer != null && num < 16f)
            {
                flag = true;
            }

            else if (num < 7f && ___dragging && !___startedShootingTongue && __instance.targetPlayer == GameNetworkManager.Instance.localPlayerController)
            {
                flag = true;
            }

            if (flag)
            {
                int damage = Plugin.ConfigManager.Damage.Value;
                PlayerControllerB localPlayerScript = GameNetworkManager.Instance.localPlayerController;

                if (localPlayerScript.health <= damage)
                {
                    localPlayerScript.KillPlayer(Vector3.up * 15f, spawnBody: true, CauseOfDeath.Mauling, 8);
                    __instance.DoKillPlayerAnimationServerRpc((int)__instance.targetPlayer.playerClientId);
                }
                else
                {
                    localPlayerScript.DamagePlayer(damage, hasDamageSFX: true, callRPC: true, causeOfDeath: CauseOfDeath.Mauling, deathAnimation: 8);
                    __instance.SetEnemyStunned(true, Plugin.ConfigManager.DamageInterval.Value, localPlayerScript);
                }
            }
        }

        return false;
    }

    public static bool IsLocalPlayerBeingDragged()
    {
        foreach (var enemyScript in Object.FindObjectsByType<BushWolfEnemy>(FindObjectsSortMode.None))
        {
            if (enemyScript.targetPlayer == GameNetworkManager.Instance.localPlayerController)
            {
                return true;
            }
        }

        return false;
    }
}
