using Gameplay.Projectiles.CannonProjectile;
using Gameplay.Targets;
using UnityEngine;

namespace Gameplay.Towers.CannonTower
{
	public class CannonTower : ProjectileTower<CannonProjectile>
	{
		[SerializeField] private float _rotationSpeed;
		[SerializeField] private float _minimumAngleDifference;
		
		private Vector3? _predictedPosition;

		protected override void Initialize()
		{
			base.Initialize();

			var cannonTowerData = data as CannonTowerData;

			if (cannonTowerData == null)
				return;

			_rotationSpeed = cannonTowerData.RotationSpeed;
			_minimumAngleDifference = cannonTowerData.MinimumAngleDifference;
		}

		protected override bool ReadyToShoot(ITarget target)
		{
			RotateToTarget(target);
			
			if (!base.ReadyToShoot(target))
				return false;

			_predictedPosition = CalculatePredictedShootPosition(target);
			
			var adjustedPredictedPosition = _predictedPosition.Value;
			adjustedPredictedPosition.y = shootPoint.position.y;

			var angleBetweenCannonAndTarget =
				Vector3.Angle(adjustedPredictedPosition - shootPoint.position, shootPoint.forward);
			
			return angleBetweenCannonAndTarget > 0 && angleBetweenCannonAndTarget < _minimumAngleDifference;
		}

		private Vector3 CalculatePredictedShootPosition(ITarget target)
		{
			var g = Physics.gravity.y;
			
			var projectileSpeed = projectilePrefab.Speed;
			var targetSpeed = target.Speed;

			var deltaY = shootPoint.position.y - target.Position.y;
			var deltaX = shootPoint.position.x - target.Position.x;

			var shootingAngle = Vector3.Angle(shootPoint.up, target.Position - shootPoint.position);

			var sin = Mathf.Sin(shootingAngle * Mathf.Deg2Rad);
			var cos = Mathf.Cos(shootingAngle * Mathf.Deg2Rad);

			var flightTime = deltaY / (projectileSpeed * cos + g * deltaX / (2 * (targetSpeed - projectileSpeed * sin)));
			var predictedPosition = target.Position + Vector3.right * (target.Speed * flightTime);

			return predictedPosition;
		}

		private void RotateToTarget(ITarget target)
		{
			var targetPosition = target.Position;
			var direction = (targetPosition - transform.position).normalized;
			
			direction.y = 0;

			var rotation = Quaternion.LookRotation(direction);
			var degreesDelta = _rotationSpeed * Time.deltaTime;

			transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, degreesDelta);
		}
		
		protected override void InitializeProjectile(CannonProjectile projectile, ITarget target)
		{
			projectile.transform.position = shootPoint.position;
			projectile.transform.rotation = shootPoint.rotation;

			var shootingAngle = Vector3.Angle(shootPoint.up, target.Position - shootPoint.position);
			projectile.SetAngle(shootingAngle);
		}
		
		private void OnDrawGizmos()
		{
			if (_predictedPosition == null)
				return;

			var adjustedPredictedPosition = _predictedPosition.Value;
			adjustedPredictedPosition.y = shootPoint.position.y;
			
			Gizmos.DrawLine(shootPoint.position, adjustedPredictedPosition);
			Gizmos.DrawLine(shootPoint.position, shootPoint.position + shootPoint.forward);
		}
	}
}
