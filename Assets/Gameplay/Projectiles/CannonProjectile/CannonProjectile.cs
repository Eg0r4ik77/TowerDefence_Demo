using UnityEngine;

namespace Gameplay.Projectiles.CannonProjectile
{
	public class CannonProjectile : Projectile
	{
		private float _shootAngle;
		private float _cannonDistanceLeft;
		private Vector3 _velocity;

		public void SetTranslationParameters(float angle, float cannonLength)
		{
			_shootAngle = angle;
			_cannonDistanceLeft = cannonLength;
			
			_velocity = (transform.forward * Mathf.Sin(_shootAngle * Mathf.Deg2Rad) -
			             transform.up * Mathf.Cos(_shootAngle * Mathf.Deg2Rad)) * speed;
		}
		
		protected override void Translate()
		{
			if (_cannonDistanceLeft > 0)
			{
				var translation = TranslateLinearly();
				_cannonDistanceLeft -= translation.magnitude;
				return;
			}

			TranslateParabolically();
		}

		private Vector3 TranslateLinearly()
		{
			var translation = transform.forward * (speed * Time.deltaTime);
			transform.position += translation;

			return translation;
		}

		private void TranslateParabolically()
		{
			_velocity.y += Physics.gravity.y * Time.deltaTime;
			transform.position += _velocity * Time.deltaTime;
		}
	}
}
