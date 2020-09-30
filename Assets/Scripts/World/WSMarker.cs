using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSMarker : MonoBehaviour {

	public void Set ( Place place ) {

		_sprite.sprite = place.Sprite;
		_text.text = place.Name;
	}

	private void LateUpdate () {

		transform.forward = Vector3.ProjectOnPlane( -( transform.position - Camera.main.transform.position ), Vector3.up );
	}

	[SerializeField] private SpriteRenderer _sprite = null;
	[SerializeField] private TMPro.TMP_Text _text = null;
}
