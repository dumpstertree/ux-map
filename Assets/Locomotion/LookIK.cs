using UnityEngine;

[RequireComponent( typeof( Animator ) )]
public class LookIK : MonoBehaviour {

	public Transform LookAtObject {
		set => _lookObj = value;
		get => _lookObj;
	}
	public float Weight {
		set => _weight = value;
		get => _weight;
	}

	[SerializeField] private float _lerpSpeed = 0.2f;
	[SerializeField] private Transform _lookObj = null;
	[Range( 0, 1 ), SerializeField] private float _weight = 1.0f;

	private float _lastWeight;
	private Vector3 _lookAtPosition;
	private Animator _animator;

	private void Awake () {

		_animator = GetComponent<Animator>();
	}
	private void OnAnimatorIK () {

		if ( _animator ) {

			if ( _lookObj != null ) {
				_weight = 1f;
				_animator.SetLookAtPosition( _lookObj.position );
			} else {
				_weight = 0f;
			}


			_lastWeight = Mathf.Lerp( _lastWeight, _weight, _lerpSpeed );
			_animator.SetLookAtWeight( _lastWeight );
		}
	}
}