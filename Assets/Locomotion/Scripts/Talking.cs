using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talking : MonoBehaviour {

	private float _talkStartTime = 0f;
	private float _talkTime = 0f;
	private bool _talking;

	[SerializeField] private GameObject _visual;

	private void Update () {

		if ( Time.time > _talkStartTime + _talkTime ) {
			_talkStartTime = _talkStartTime + _talkTime + Random.Range( 1, 5f );
			_talkTime = Random.Range( 0.5f, 2f );
		}

		_talking = Time.time > _talkStartTime;

		if ( _talking ) {
			_visual.transform.localScale = Vector3.Lerp( _visual.transform.localScale, Vector3.one * .2f, 0.2f );
			GetComponentInChildren<Target>().Influence = 10f;
		} else {
			_visual.transform.localScale = Vector3.Lerp( _visual.transform.localScale, Vector3.zero, 0.2f );
			GetComponentInChildren<Target>().Influence = 0f;
		}
	}
}
