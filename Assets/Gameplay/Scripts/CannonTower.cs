using UnityEngine;

namespace Gameplay.Scripts
{
	public class CannonTower : ProjectileTower<CannonProjectile>
	{
		[SerializeField] private float _rotationSpeed;
		[SerializeField] private Transform _cannon;
		[SerializeField] private float _minimumAngleDifference;
		
		protected override bool ReadyToShoot(ITarget target)
		{
			if (!base.ReadyToShoot(target))
				return false;

			var vector = GetPredictedShootPosition(target) - _cannon.position;
			vector.y = _cannon.forward.y;
			
			var angleBetweenCannonAndTarget = Vector3.Angle(vector, _cannon.forward);
			if (angleBetweenCannonAndTarget < _minimumAngleDifference)
				return true;
			
			RotateToTarget(target);
			return false;
		}

		private Vector3 GetPredictedShootPosition(ITarget target)
		{
			var g = Physics.gravity.y;
			
			var vp = _projectilePrefab.Speed;
			var vt = target.Speed;

			var deltaY = shootPoint.position.y - target.Position.y;
			var deltaX = shootPoint.position.x - target.Position.x;

			var angle = Vector3.Angle(shootPoint.up, target.Position - shootPoint.position);
			var angleInRadians = angle * Mathf.Deg2Rad;
			
			var sin = Mathf.Sin(angleInRadians);
			var cos = Mathf.Cos(angleInRadians);

			var t = deltaY / (vp * cos + g * deltaX / (2 * (vt - vp * sin)));
			var predictedPosition = target.Position + Vector3.right * (target.Speed * t); // forward!!!

			_predicted = predictedPosition;
			return predictedPosition;
		}

		// перенести в базовый класс?
		private void RotateToTarget(ITarget target)
		{
			var targetPosition = target.Position;
			var direction = (targetPosition - transform.position).normalized;
			direction.y = 0;

			Quaternion rotation = Quaternion.LookRotation(direction);
			float degreesDelta = _rotationSpeed * Time.deltaTime;

			transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, degreesDelta);
		}
		
		protected override void InitializeProjectile(CannonProjectile projectile, ITarget target)
		{
			projectile.transform.position = shootPoint.position;
			projectile.transform.rotation = shootPoint.rotation;
			projectile.SetAngle(Vector3.Angle(shootPoint.up, target.Position - shootPoint.position) * Mathf.Deg2Rad);
		}

		private Vector3? _predicted;

		private void OnDrawGizmos()
		{
			if (_predicted == null)
				return;
			
			Gizmos.DrawLine(shootPoint.position, _predicted.Value);
		}
	}
}
