using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapUI : MonoBehaviour {

	public static MiniMapUI Instance {
		get => _instance;
	}

	private static MiniMapUI _instance;


	[SerializeField] private Button _maximizeButton = null;
	[SerializeField] private Image _content = null;
	[SerializeField] private UIMarker _pointOfInterestMarkerPrefab = null;

	private List<UIMarker> _markerInstances = new List<UIMarker>();

	private void Awake () {

		_instance = this;

		_maximizeButton.onClick.AddListener( Maximize );
	}

	private void Maximize () {

		MapUI.Instance.Present();
	}

	private void LateUpdate () {

		if ( Player.Instance != null ) {

			var normalizedPoint = App.Instance.Map.GetNormalizedPosition( Player.Instance.transform.position );
			_content.transform.localPosition = new Vector2( ( ( ( 1f - normalizedPoint.x ) * _content.rectTransform.rect.width ) - ( _content.rectTransform.rect.width / 2f ) ),
															 ( ( 1f - normalizedPoint.z ) * _content.rectTransform.rect.height ) - ( _content.rectTransform.rect.height / 2f ) );

		}
	}
	private void ClearMarkers () {

		foreach ( UIMarker m in _markerInstances ) {
			Destroy( m.gameObject );
		}

		_markerInstances.Clear();
	}
	private void BuildMarkers () {

		foreach ( Place p in App.Instance.Map._pointsOfInterest ) {

			var inst = Instantiate( _pointOfInterestMarkerPrefab );
			var normalizedPoint = App.Instance.Map.GetNormalizedPosition( p.Position );

			inst.transform.SetParent( _content.transform );
			inst.Set( App.Instance.Map, p );

			_markerInstances.Add( inst );

			inst.transform.localPosition = new Vector2( normalizedPoint.x * _content.rectTransform.rect.width - ( _content.rectTransform.rect.width / 2f ), normalizedPoint.z * _content.rectTransform.rect.height - ( _content.rectTransform.rect.height / 2f ) );
		}
	}


	private void OnLevelWasLoaded () {

		_content.sprite = App.Instance.Map._map;

		ClearMarkers();
		BuildMarkers();
	}
}
