using Gameplay.Targets.Monster;
using UnityEngine;

namespace Gameplay.System.EnemySpawn
{
    [CreateAssetMenu(menuName = "Data/Gameplay/System/EnemySpawner", fileName = "EnemySpawner")]
    public class EnemySpawnerData : ScriptableObject
    {
        [field: SerializeField] public float Interval { get; private set; }
        [field: SerializeField] public Monster MonsterPrefab { get; private set; }
        [field: SerializeField] public int MaxMonsterCountInPool { get; private set; }
    }
}