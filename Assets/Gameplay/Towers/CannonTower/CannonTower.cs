using Gameplay.Projectiles.CannonProjectile;
using Gameplay.Targets;
using UnityEngine;

namespace Gameplay.Towers.CannonTower
{
	public class CannonTower : ProjectileTower<CannonProjectile>
	{
		[SerializeField] private float _rotationSpeed;
		[SerializeField] private float _minimumAngleDifference;
		[SerializeField] private float _cannonLength;
		
		private Vector3? _predictedPosition;

		protected override void Initialize()
		{
			base.Initialize();

			var cannonTowerData = data as CannonTowerData;

			if (cannonTowerData == null)
				return;

			_rotationSpeed = cannonTowerData.RotationSpeed;
			_minimumAngleDifference = cannonTowerData.MinimumAngleDifference;
			_cannonLength = cannonTowerData.CannonLength;
		}

		protected override bool ReadyToShoot(ITarget target)
		{
			if (!base.ReadyToShoot(target))
			{
				RotateTo(target.Position);
				return false;
			}

			_predictedPosition = CalculatePredictedShootPosition(target);
			
			var adjustedPredictedPosition = _predictedPosition.Value;
			adjustedPredictedPosition.y = shootPoint.position.y;

			var angleBetweenCannonAndTarget =
				Vector3.Angle(adjustedPredictedPosition - shootPoint.position, shootPoint.forward);
			
			RotateTo(_predictedPosition.Value);
			if (Mathf.Abs(angleBetweenCannonAndTarget) < _minimumAngleDifference)
				return true;
			
			return false;
		}

		private Vector3 CalculatePredictedShootPosition(ITarget target)
		{
			var g = Physics.gravity.y;
			
			var projectileSpeed = projectilePrefab.Speed;
			var targetSpeed = target.Speed;

			var deltaY = shootPoint.position.y - target.Position.y;
			var deltaX = shootPoint.position.x - target.Position.x;

			var shootingAngle = Vector3.Angle(-shootPoint.up, target.Position - shootPoint.position);

			var sin = Mathf.Sin(shootingAngle * Mathf.Deg2Rad);
			var cos = Mathf.Cos(shootingAngle * Mathf.Deg2Rad);

			var flightTime = deltaY / (projectileSpeed * cos + g * deltaX / (2 * (targetSpeed - projectileSpeed * sin)));
			
			var projectileDepartureTime = _cannonLength / (projectileSpeed / Time.fixedDeltaTime);
			flightTime += projectileDepartureTime;
			
			var predictedPosition = target.Position + Vector3.left * (target.Speed * flightTime);

			return predictedPosition;
		}

		private void RotateTo(Vector3 position)
		{
			var targetPosition = position;
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
			projectile.SetAngle(shootingAngle, _cannonLength);
		}
		
		private void OnDrawGizmos()
		{
			if (_target == null)
				return;
			var pred = CalculatePredictedShootPosition(_target);

			var adjustedPredictedPosition = pred;
			adjustedPredictedPosition.y = shootPoint.position.y;
			
			Gizmos.DrawLine(shootPoint.position, adjustedPredictedPosition);
			Gizmos.DrawLine(shootPoint.position, shootPoint.position + shootPoint.forward);
		}
	}
}
