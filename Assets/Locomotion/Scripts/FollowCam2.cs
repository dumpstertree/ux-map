using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam2 : MonoBehaviour {

	private enum States {
		Resting,
		Following
	}

	private States _state;
	private float _lastDist;

	[SerializeField] private Transform _target = null;

	[Header( "Resting" )]
	[SerializeField] private float _restingDistance = 5f;
	[SerializeField] private float _restingHeight = 2.5f;
	[SerializeField] private float _restingMinDistance = 2f;
	[SerializeField] private float _restingMaxDistance = 10f;

	[Header( "Following" )]
	[SerializeField] private float _followingDistance = 5f;
	[SerializeField] private float _followingHeight = 2.5f;

	// mono
	private void Awake () { }
	private void Update () {

		switch ( _state ) {

			case States.Resting:
				DoResting();
				TryExitResting();
				break;

			case States.Following:
				DoFollowing();
				TryExitFollowing();
				break;
		}

		var horizontal = Input.GetAxis( "Horizontal" );
		var vertical = Input.GetAxis( "Vertical" );
		var hasInput = horizontal > 0.99f || vertical > 0.99f;
		var pointOfInterest = Targeting.GetBestTarget( _target.position, _target.forward );

		var tar = pointOfInterest != null && !hasInput ? pointOfInterest.transform : _target;
		var targetRotation = Quaternion.LookRotation( new Vector3( tar.position.x - transform.position.x, 0, tar.position.z - transform.position.z ), Vector3.up );

		transform.rotation = Quaternion.LookRotation( new Vector3( _target.position.x - transform.position.x, 0, _target.position.z - transform.position.z ), Vector3.up );
		transform.GetChild( 0 ).rotation = Quaternion.Slerp(
			transform.GetChild( 0 ).rotation,
			targetRotation,
			0.1f
		);
	}
	private void OnDrawGizmos () {

		Gizmos.color = _state == States.Resting ? Color.green : Color.red;
		Gizmos.DrawWireSphere( transform.position, _restingMinDistance );
		Gizmos.DrawWireSphere( transform.position, _restingMaxDistance );
	}

	// states
	private void DoResting () {

		var targetPoint = GetRestingPoint( _target, _restingDistance, _restingHeight );



		// move
		// transform.position = Vector3.Lerp(
		// 	transform.position,
		// 	targetPoint,
		// 	0.2f
		// );

		// rotate

	}
	private void DoFollowing () {

		var horizontal = Input.GetAxis( "Horizontal" );
		var vertical = Input.GetAxis( "Vertical" );
		var changeInDist = Mathf.Abs( _lastDist - Vector3.Distance( transform.position, _target.position ) );
		var targetPoint = GetFollowingPoint( _target, _followingDistance, _followingHeight, horizontal, vertical, changeInDist );
		var targetRotation = Quaternion.LookRotation( new Vector3( _target.position.x - transform.position.x, 0, _target.position.z - transform.position.z ), Vector3.up );

		// move
		transform.position = Vector3.Lerp(
			transform.position,
			targetPoint,
			0.2f
		);

		// // rotate
		// transform.rotation = Quaternion.Slerp(
		// 	transform.rotation,
		// 	targetRotation,
		// 	0.2f
		// );

		_lastDist = Vector3.Distance( transform.position, _target.position );
	}
	private void TryExitResting () {

		var distToTarget = Vector3.Distance( transform.position, _target.position );
		var isTooClose = distToTarget < _restingMinDistance;
		var isTooFar = distToTarget > _restingMaxDistance;

		if ( isTooClose || isTooFar ) {
			_state = States.Following;
		}
	}
	private void TryExitFollowing () {

		var horizontal = Input.GetAxis( "Horizontal" );
		var vertical = Input.GetAxis( "Vertical" );
		var changeInDist = _lastDist - Vector3.Distance( transform.position, _target.position );
		var targetPoint = GetFollowingPoint( _target, _followingDistance, _followingHeight, horizontal, vertical, changeInDist );
		var distanceToTarget = Vector3.Distance( transform.position, targetPoint );
		var isCompleted = distanceToTarget < 0.01f;

		if ( isCompleted ) {
			_state = States.Resting;
		}
	}

	// get points
	private Vector3 GetRestingPoint ( Transform target, float followDistance, float followHeight ) {

		var dirToTargetDir = -new Vector3( target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z ).normalized;

		// generate position
		var pos = target.position;
		pos += dirToTargetDir * followDistance;
		pos += Vector3.up * followHeight;

		return pos;
	}
	private Vector3 GetFollowingPoint ( Transform target, float followDistance, float followHeight, float horizontal, float vertical, float distanceChange ) {

		// input
		var hasVertical = Mathf.Abs( vertical ) > 0.99f;
		var hasHorizontal = Mathf.Abs( horizontal ) > 0.99f;

		// target values
		var dirToTargetDir = -new Vector3( target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z ).normalized;
		var objectBackDir = -target.forward;

		// interpolate
		var hasInput = hasVertical || hasHorizontal;
		var distanceChanged = distanceChange > 0.05f;
		var interpolateMod = distanceChanged || !hasInput ? 1f : 0f;
		var direction = Vector3.Slerp( dirToTargetDir, objectBackDir, 0.1f * interpolateMod );

		// generate position
		var pos = target.position;
		pos += direction * followDistance;
		pos += Vector3.up * followHeight;

		return pos;
	}
}
