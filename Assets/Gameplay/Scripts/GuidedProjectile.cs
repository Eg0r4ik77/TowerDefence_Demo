namespace Gameplay.Scripts
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
			var translation = transform.forward * speed;
			
			if (_target != null)
			{
				translation = _target.Position - transform.position;
				transform.LookAt(_target.Position);
			}
		
			if (translation.magnitude > speed) {
				translation = translation.normalized * speed;
			}
		
			transform.Translate (translation);
		}
	}
}
