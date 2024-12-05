using UnityEngine;

namespace Gameplay.Projectiles
{
    [CreateAssetMenu(menuName = "Data/Gameplay/Projectile", fileName = "Projectile")]
    public class ProjectileData : ScriptableObject
    {
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public float LifeTime { get; private set; }
    }
}