using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {

	[SerializeField] private Transform _trackObject;
	[SerializeField] private float _followDistance = 7f;
	[SerializeField] private float _followHeight = 2f;
	// [SerializeField] private Transform _lookingAtObject;

	private bool _tracking;

	private void LateUpdate () {

		// change states
		if ( _tracking ) {

			var distanceToFollowPoint = Vector3.Distance( transform.position, GetFollowPoint() );
			var isCompleted = distanceToFollowPoint < 0.01f;

			// disable
			if ( isCompleted ) {
				_tracking = false;
			}

		} else {

			var distanceToTarget = Vector3.Distance( transform.position, _trackObject.position );
			var isTooClose = distanceToTarget < 2f;
			var isTooFar = distanceToTarget > 10f;

			// enable
			if ( isTooClose || isTooFar ) {
				_tracking = true;
			}
		}


		// apply changes
		// if ( _tracking ) {


		transform.position = Vector3.Lerp( Vector3.Lerp( transform.position, GetFollowPoint(), 0.1f ), GetFollowPoint(), 0.1f );
		// }

		// always be looking
		var targetRot = Quaternion.LookRotation( new Vector3( _trackObject.position.x - transform.position.x, 0, _trackObject.position.z - transform.position.z ) );
		transform.rotation = Quaternion.Slerp( transform.rotation, targetRot, 0.5f );


		// var xOffset = ( Input.mousePosition.x / Screen.width - 0.5f ) / 0.5f;
		// var yOffset = -( Input.mousePosition.y / Screen.height - 0.5f ) / 0.5f;

		// var forward = ( _trackObject.position - transform.position );
		// forward.y = 0;
		// forward.Normalize();

		// var right = Vector3.Cross( Vector3.up, forward );

		// var horizontalRot = Quaternion.AngleAxis( xOffset * 30f, Vector3.up );
		// var verticalRot = Quaternion.AngleAxis( yOffset * 30f, right );

		var camTarget = Targeting.GetBestTarget( _trackObject.position, transform.forward );
		var playerTarget = Targeting.GetBestTarget( _trackObject.position, _trackObject.forward );
		var target = camTarget == playerTarget ? camTarget : null;

		if ( target != null && Input.GetAxis( "Horizontal" ) < 0.99f && Input.GetAxis( "Vertical" ) < 0.99f ) {

			transform.GetChild( 0 ).rotation = Quaternion.Slerp(
				transform.GetChild( 0 ).rotation,
				Quaternion.LookRotation( new Vector3( target.Position.x - transform.position.x, 0, target.Position.z - transform.position.z ), Vector3.up ),
				Time.deltaTime
				);
		} else {

			transform.GetChild( 0 ).rotation = Quaternion.Slerp(
				transform.GetChild( 0 ).rotation,
				transform.rotation,
				Time.deltaTime
				);
		}
	}

	private float _lastDist = 0f;
	private Vector3 GetFollowPoint () {

		var hasInput = Mathf.Abs( Input.GetAxis( "Horizontal" ) ) > 0.99f || Mathf.Abs( Input.GetAxis( "Vertical" ) ) > 0.99f;

		var dirToTargetDir = -new Vector3( _trackObject.transform.position.x - transform.position.x, 0, _trackObject.transform.position.z - transform.position.z ).normalized;
		var objectBackDir = -_trackObject.forward;

		var interpolateMod = Mathf.Abs( Vector3.Distance( transform.position, _trackObject.position ) - _lastDist ) > 0.05f || !hasInput ? 1f : 0f;
		var curDir = Vector3.Slerp( dirToTargetDir, objectBackDir, 0.1f * interpolateMod );

		_lastDist = Vector3.Distance( transform.position, _trackObject.position );

		return _trackObject.position + ( curDir * _followDistance ) + ( Vector3.up * _followHeight );
	}
	private Vector3 GetRestingPoint () {

		return Vector3.zero;
	}
}
