using UnityEngine;

namespace Gameplay.Enemies
{
    [CreateAssetMenu(menuName = "Data/Gameplay/Enemies/Base", fileName = "Enemy")]
    public class EnemyData : ScriptableObject
    {
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public int MaxHealth { get; private set; }
        [field: SerializeField] public float ReachDistance { get; private set; }
    }
}