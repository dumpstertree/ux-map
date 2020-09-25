using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMarker : MonoBehaviour {

	public void Set ( Map map, Place pointOfInterest ) {

		_map = map;
		_pointOfInterest = pointOfInterest;
		_icon.sprite = pointOfInterest.Sprite;
		_title.text = pointOfInterest.Name;
	}

	[SerializeField] private Button _button = null;
	[SerializeField] private Image _icon = null;
	[SerializeField] private Text _title = null;

	private Map _map;
	private Place _pointOfInterest;

	private void Awake () {

		_button.onClick.AddListener( Teleport );
	}
	private void Teleport () {

		App.Instance.Load( _map, _pointOfInterest );
		MapUI.Instance.Dismiss();
	}
}
