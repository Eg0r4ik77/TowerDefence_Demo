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
		private Vector3 ProjectileDeparturePosition => shootPoint.position + shootPoint.forward * _cannonLength;

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
			_predictedPosition = CalculatePredictedShootPosition(target);
			
			var adjustedPredictedPosition = _predictedPosition.Value;
			adjustedPredictedPosition.y = ProjectileDeparturePosition.y;

			var angleBetweenCannonAndTarget =
				Vector3.Angle(adjustedPredictedPosition - ProjectileDeparturePosition, shootPoint.forward);

			RotateTo(_predictedPosition.Value);

			return angleBetweenCannonAndTarget < _minimumAngleDifference && base.ReadyToShoot(target);
		}

		private Vector3 CalculatePredictedShootPosition(ITarget target)
		{
			var g = Physics.gravity.y;
			
			var projectileSpeed = projectilePrefab.Speed;
			var targetSpeed = target.Speed;

			var deltaY = ProjectileDeparturePosition.y - target.Position.y;
			var deltaX = ProjectileDeparturePosition.x - target.Position.x;

			var shootingAngle = Vector3.Angle(-shootPoint.up, target.Position - ProjectileDeparturePosition);

			var sin = Mathf.Sin(shootingAngle * Mathf.Deg2Rad);
			var cos = Mathf.Cos(shootingAngle * Mathf.Deg2Rad);

			var flightTime = deltaY / (projectileSpeed * cos + g * deltaX / (2 * (targetSpeed - projectileSpeed * sin)));
			
			var projectileDepartureTime = _cannonLength / projectileSpeed;
			flightTime += projectileDepartureTime;
			
			var predictedPosition = target.Position + target.Forward * (target.Speed * flightTime);

			return predictedPosition;
		}

		private void RotateTo(Vector3 position)
		{
			var direction = (position - transform.position).normalized;
			
			direction.y = 0;

			var rotation = Quaternion.LookRotation(direction);
			var degreesDelta = _rotationSpeed * Time.deltaTime;

			transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, degreesDelta);
		}
		
		protected override void InitializeProjectile(CannonProjectile projectile, ITarget target)
		{
			projectile.transform.position = shootPoint.position;
			projectile.transform.rotation = shootPoint.rotation;

			var shootingAngle = Vector3.Angle(-shootPoint.up, _predictedPosition.Value - shootPoint.position);
			projectile.SetTranslationParameters(shootingAngle, _cannonLength);
		}
		
		private void OnDrawGizmos()
		{
			if (shootTarget == null)
				return;
			
			var predictedPosition = CalculatePredictedShootPosition(shootTarget);

			var adjustedPredictedPosition = predictedPosition;
			adjustedPredictedPosition.y = shootPoint.position.y;
			
			Gizmos.DrawLine(shootPoint.position, adjustedPredictedPosition);
			Gizmos.DrawRay(shootPoint.position, shootPoint.forward * 10);
		}
	}
}
