using Gameplay.Scripts;

public class CannonProjectile : Projectile {

	private Pool<CannonProjectile> _pool;

	protected override void Translate()
	{
		var translation = transform.forward * speed;
		transform.Translate (translation);
	}
}
