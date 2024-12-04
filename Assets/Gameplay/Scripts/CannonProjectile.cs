using UnityEngine;

namespace Gameplay.Scripts
{
	public class CannonProjectile : Projectile
	{
		private float _shootAngle;
		private Vector3 _velocity;

		public void SetAngle(float angle)
		{
			_shootAngle = angle;
			_velocity = (transform.forward * Mathf.Sin(_shootAngle) + transform.up * Mathf.Cos(_shootAngle)) * Speed;
		}
		
		protected override void Translate()
		{
			_velocity.y += Physics.gravity.y * Time.deltaTime;

			// Перемещаем проектайл
			// transform.position += _velocity * Time.deltaTime;
			//
			// var x = transform.forward * (_currentSpeed * Time.deltaTime * Mathf.Sin(_shootAngle));
			// var y = (-_currentSpeed * transform.up + Physics.gravity) * Time.deltaTime;
			//
			// var translation = x + y;
			//transform.Translate(_velocity * Time.deltaTime, Space.World);

			transform.position += _velocity * Time.deltaTime;
		}
	}
}
