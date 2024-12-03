using UnityEngine;

namespace Gameplay.Scripts
{
	public class CannonProjectile : Projectile
	{
		protected override void Translate()
		{
			var translation = transform.forward * (speed * Time.deltaTime);
			transform.Translate(translation);
		}
	}
}
