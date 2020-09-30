using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public static Player Instance {
		get => _instance;
	}
	private static Player _instance;

	public void Teleport ( Vector3 position ) {

		_charController.enabled = false;
		transform.position = position;
		_charController.enabled = true;
	}
	private void Awake () {

		_instance = this;
		UnityEngine.SceneManagement.SceneManager.sceneLoaded += ( a, b ) => OnSceneLoaded();
		UnityEngine.SceneManagement.SceneManager.sceneUnloaded += ( a ) => OnSceneUnloaded();
	}
	private void OnSceneUnloaded () {

		if ( _navMesh != null ) {
			Destroy( _navMesh );
		}
	}
	private void OnSceneLoaded () { }

	[SerializeField] private float _walkSpeed = 5f;
	[SerializeField] private float _runSpeed = 10f;
	[SerializeField] private Transform _camera;
	[SerializeField] private Animator _anim;
	[SerializeField] private CharacterController _charController = null;
	[SerializeField] private LayerMask _clickMask = default;

	private UnityEngine.AI.NavMeshAgent _navMesh;
	private UnityEngine.AI.NavMeshPath _path;
	private Vector3 _lastInput = Vector2.zero;
	private Vector3? _clickPosition;

	private void Update () {

		if ( _navMesh == null ) {

			var p = transform.position;
			transform.position = Vector3.zero;
			_navMesh = gameObject.AddComponent<UnityEngine.AI.NavMeshAgent>();
			// _navMesh.updatePosition = false;
			// _navMesh.updateRotation = false;
			_navMesh.speed = 0f;
			transform.position = p;
		}
		// _lookIK.LookAtObject = Targeting.GetBestTarget( transform.position, _camera.forward )?.transform;

		if ( _navMesh == null ) {
			return;
		}

		var input = GetMovementDirection();
		var running = Input.GetKey( KeyCode.LeftShift ) || Input.GetKey( KeyCode.RightShift );

		_anim.SetBool( "Walking", input != Vector3.zero );
		_anim.SetBool( "Running", running );

		Move( Vector3.Lerp( _lastInput, input, 0.1f ), running );

		_lastInput = input;
	}

	private void Move ( Vector3 dir, bool running ) {

		_charController.Move( dir * Time.deltaTime * ( running ? _runSpeed : _walkSpeed ) );

		if ( dir != Vector3.zero ) {
			transform.rotation = Quaternion.Slerp( transform.rotation, Quaternion.LookRotation( dir.normalized, Vector3.up ), 0.3f );
		}

	}
	private Vector3 GetMovementDirection () {

		if ( Input.GetMouseButton( 0 ) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() ) {

			RaycastHit targetHit;
			Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );

			if ( Physics.Raycast( ray, out targetHit, Mathf.Infinity ) ) {

				if ( _clickMask == ( _clickMask | ( 1 << targetHit.transform.gameObject.layer ) ) ) {
					_clickPosition = targetHit.point;
				}
			}
		}

		if ( _clickPosition.HasValue ) {
			var success = _navMesh.SetDestination( new Vector3( _clickPosition.Value.x, transform.position.y, _clickPosition.Value.z ) );
			if ( !success ) {
				_clickPosition = null;
			}
		}

		if ( _clickPosition.HasValue && _navMesh.remainingDistance < 0.5f ) {
			_clickPosition = null;
		}

		var horizontal = 0f;
		var vertical = 0f;

		if ( Input.GetKey( KeyCode.D ) || Input.GetKey( KeyCode.RightArrow ) ) {
			horizontal += 1f;
		}
		if ( Input.GetKey( KeyCode.A ) || Input.GetKey( KeyCode.LeftArrow ) ) {
			horizontal -= 1f;
		}
		if ( Input.GetKey( KeyCode.W ) || Input.GetKey( KeyCode.UpArrow ) ) {
			vertical += 1f;
		}
		if ( Input.GetKey( KeyCode.S ) || Input.GetKey( KeyCode.DownArrow ) ) {
			vertical -= 1f;
		}

		var input = new Vector2( horizontal, vertical ).normalized;
		if ( input != Vector2.zero ) {
			_path = null;

			var cameraForward = _camera.transform.forward;
			cameraForward.y = 0;
			cameraForward.Normalize();

			var cameraRotation = Quaternion.LookRotation( cameraForward, Vector3.up );
			var worldSpaceMovement = cameraRotation * new Vector3( input.x, 0, input.y );

			return worldSpaceMovement;
		}

		if ( _clickPosition.HasValue ) {

			UnityEngine.AI.NavMeshHit hit;
			_navMesh.SamplePathPosition( _navMesh.areaMask, 10f, out hit );

			return ( hit.position - transform.position ).normalized;
		}

		return Vector2.zero;
	}

	private void OnDrawGizmos () {

		if ( _path == null ) {
			return;
		}

		Gizmos.color = Color.red;
		Vector3? last = (Vector3?)null;
		foreach ( Vector3 p in _path.corners ) {

			if ( last.HasValue ) {
				Gizmos.DrawLine( last.Value, p );
			}

			last = p;
		}
	}
}
