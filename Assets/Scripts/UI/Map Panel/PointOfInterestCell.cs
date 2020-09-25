using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointOfInterestCell : MonoBehaviour {

	public void Set ( Map map, Place pointOfInterest ) {

		_map = map;
		_pointOfInterest = pointOfInterest;

		_text.text = _pointOfInterest.Name;
	}

	[SerializeField] private Text _text = null;
	[SerializeField] private Button _goToButtonn = null;

	private Map _map;
	private Place _pointOfInterest;

	private void Awake () {

		_goToButtonn.onClick.AddListener( GoTo );
	}
	private void GoTo () {

		App.Instance.Load( _map, _pointOfInterest );
	}
}
