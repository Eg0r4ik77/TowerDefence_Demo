using System;
using R3;
using TMPro;
using UnityEngine;

namespace Gameplay.Enemies
{
    public class EnemyCanvas : MonoBehaviour
    {
        [SerializeField] private Enemy _enemy;
        [SerializeField] private TMP_Text _healthTextMesh;

        private IDisposable _healthDisposable;

        private void Update()
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        }

        private void OnEnable()
        {
            ViewHealth(_enemy.Health);

            _healthDisposable = _enemy.Damaged.Subscribe(_ => ViewHealth(_enemy.Health));
        }

        private void OnDisable()
        {
            _healthDisposable.Dispose();
        }

        private void ViewHealth(float health)
        {
            _healthTextMesh.text = $"HP: {health}";
        }
    }
}