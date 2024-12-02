using UnityEngine;
using Gameplay.Scripts;

public class GuidedProjectile : Projectile {
	public GameObject m_target;

	private Pool<GuidedProjectile> _pool;

	protected override void Translate()
	{
		if (m_target == null) {
			Destroy (gameObject);
			return;
		}

		var translation = m_target.transform.position - transform.position;
		if (translation.magnitude > speed) {
			translation = translation.normalized * speed;
		}
		transform.Translate (translation);
	}
}
