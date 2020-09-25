using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	[SerializeField] private float _movementSpeed = 5f;

	public static Player Instance {
		get => _instance;
	}
	private static Player _instance;

	public void Teleport ( Vector3 position ) {

		transform.position = position;
	}
	private void Awake () {

		_instance = this;
	}

	private void Update () {

		var h = 0f;
		var v = 0f;

		if ( Input.GetKey( KeyCode.UpArrow ) ) {
			v += 1f;
		}
		if ( Input.GetKey( KeyCode.DownArrow ) ) {
			v -= 1f;
		}
		if ( Input.GetKey( KeyCode.LeftArrow ) ) {
			h -= 1f;
		}
		if ( Input.GetKey( KeyCode.RightArrow ) ) {
			h += 1f;
		}

		var input = new Vector2( h, v );

		if ( input.magnitude < .1f ) {
			return;
		}

		transform.rotation = Quaternion.Slerp( transform.rotation, Quaternion.LookRotation( GetSteeringVector( FindObjectOfType<Camera>().transform.forward, input ), Vector3.up ), 0.2f );
		transform.position += GetSteeringVector( FindObjectOfType<Camera>().transform.forward, input ) * input.magnitude * Time.deltaTime * _movementSpeed;
	}
	public static Vector3 GetSteeringVector ( Vector3 cameraForward, Vector2 stick ) {

		// convert input to XZ plane
		var inputXZ = new Vector3( stick.x, 0, stick.y );

		// get the world-centric movement vector
		var camXZForward = Vector3.ProjectOnPlane( cameraForward, Vector3.up ).normalized;
		var camXZRotation = Quaternion.LookRotation( camXZForward, Vector3.up );
		return camXZRotation * inputXZ;
	}
}
