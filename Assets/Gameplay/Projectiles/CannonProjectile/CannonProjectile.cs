using UnityEngine;

namespace Gameplay.Projectiles.CannonProjectile
{
	public class CannonProjectile : Projectile
	{
		private float _shootAngle;
		private Vector3 _velocity;

		public void SetAngle(float angle)
		{
			_shootAngle = angle;
			
			_velocity = (transform.forward * Mathf.Sin(_shootAngle *  Mathf.Deg2Rad) + transform.up * Mathf.Cos(_shootAngle * Mathf.Deg2Rad)) * Speed;
		}
		
		protected override void Translate()
		{
			_velocity.y += Physics.gravity.y * Time.deltaTime;
			transform.position += _velocity * Time.deltaTime;
		}
	}
}
