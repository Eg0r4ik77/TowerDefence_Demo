using UnityEngine;

namespace Gameplay.Projectiles.CannonProjectile
{
	public class CannonProjectile : Projectile
	{
		private float _shootAngle;
		private Vector3 _velocity;

		private float _distance;
		
		public void SetAngle(float angle)
		{
			_shootAngle = angle;

			_distance = 2f;
			_velocity = (transform.forward * Mathf.Sin(_shootAngle * Mathf.Deg2Rad) +
			             transform.up * Mathf.Cos(_shootAngle * Mathf.Deg2Rad)) * speed;
		}
		
		protected override void Translate()
		{
			if (_distance > 0)
			{
				var translation = transform.forward * (speed * Time.deltaTime);
				transform.position += translation;
				_distance -= translation.magnitude;
				return;
			}
			
			_velocity.y += Physics.gravity.y * Time.deltaTime;
			transform.position += _velocity * Time.deltaTime;
		}
	}
}
