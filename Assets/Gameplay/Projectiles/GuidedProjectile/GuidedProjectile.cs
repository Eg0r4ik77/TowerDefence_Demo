using Gameplay.Enemies;
using UnityEngine;

namespace Gameplay.Projectiles.GuidedProjectile
{
	public class GuidedProjectile : Projectile
	{
		private ITarget _target;

		public void SetTarget(ITarget target)
		{
			_target = target;
		}
	
		protected override void Translate()
		{
			var direction = transform.forward;
			
			if (_target != null)
			{
				direction = (_target.Position - transform.position).normalized;
				transform.LookAt(_target.Position);
			}

			var translation = direction * (speed * Time.deltaTime);
		
			transform.Translate (translation, Space.World);
		}
	}
}
