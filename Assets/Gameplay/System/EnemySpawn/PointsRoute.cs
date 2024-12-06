using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.System.EnemySpawn
{
    [Serializable]
    public class PointsRoute
    {
        [SerializeField] private List<Transform> _points;

        public IEnumerator<Transform> Enumerator => _points.GetEnumerator();
    }
}