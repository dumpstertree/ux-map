using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

	public float Influence {
		get; set;
	}
	public Vector3 Position {
		get => transform.position;
	}

	private void OnEnable () {

		Targeting.Register( this );
	}
	private void OnDisable () {

		Targeting.Deregister( this );
	}
}
