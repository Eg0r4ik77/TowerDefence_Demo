using Gameplay.Projectiles;
using UnityEngine;

namespace Gameplay.Towers
{
    [CreateAssetMenu(menuName = "Data/Gameplay/Tower/ProjectileTower/Base", fileName = "ProjectileTower")]
    public class ProjectileTowerData : TowerData
    {
        [field: SerializeField] public Projectile ProjectilePrefab { get; private set; }
        [field: SerializeField] public int MaxProjectilesCountInPool { get; private set; }
    }
}