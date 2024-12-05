using UnityEngine;

namespace Gameplay.Towers.CannonTower
{
    [CreateAssetMenu(menuName = "Data/Gameplay/Tower/ProjectileTower/CannonTower", fileName = "CannonTower")]
    public class CannonTowerData : ProjectileTowerData
    {
        [field: SerializeField] public float RotationSpeed { get; private set; }
        [field: SerializeField] public float MinimumAngleDifference { get; private set; }
    }
}