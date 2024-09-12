using System.Linq;
using UnityEngine;

namespace com.github.zehsteam.KidnapperFoxSettings;

internal class Utils
{
    public static EnemyType GetEnemyTypeFromResources(string enemyName)
    {
        try
        {
            return Resources.FindObjectsOfTypeAll<EnemyType>().Single((EnemyType x) => x.enemyName == enemyName);
        }
        catch
        {
            Plugin.logger.LogError($"Failed to get \"{enemyName}\" EnemyType from Resources.");
        }

        return null;
    }
}
