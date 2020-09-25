using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviour {

	public static MapUI Instance {
		get => _instance;
	}

	private static MapUI _instance;


	public void Present () {

		gameObject.SetActive( true );
	}
	public void Dismiss () {

		gameObject.SetActive( false );
		_search.text = "";
	}

	[Header( "Points" )]
	[SerializeField] private UIMarker _pointOfInterestMarkerPrefab = null;

	[Header( "Interaction" )]
	[SerializeField] private Button _minimizeButton = null;
	[SerializeField] private RectTransform _content = null;
	[SerializeField] private Transform _searchContent;

	[SerializeField] private Dropdown _areaDropdown = null;
	[SerializeField] private InputField _search = null;

	[SerializeField] private Image _mapVisual = null;
	[SerializeField] private PlacesContent _places = null;
	[SerializeField] private SearchCell _searchCell = null;

	[SerializeField] private GameObject _playerMarker = null;


	private Map _map;
	private List<UIMarker> _markerInstances = new List<UIMarker>();
	private List<SearchCell> _searchCellInstances = new List<SearchCell>();
	private GameObject _playerInstance;


	// mono
	private void Awake () {

		_instance = this;

		_minimizeButton.onClick.AddListener( Dismiss );
		_areaDropdown.onValueChanged.AddListener( HandleDropdownChanged );
		_search.onValueChanged.AddListener( HandleInputFieldChanged );

		var options = new List<Dropdown.OptionData>();
		foreach ( Map m in App.Instance.Server.Maps ) {
			options.Add( new Dropdown.OptionData( m._sceneName ) );
		}

		_areaDropdown.options = options;
		_playerMarker = Instantiate( _playerMarker );
		_playerMarker.transform.SetParent( _content );
	}
	private void LateUpdate () {

		if ( _map == App.Instance.Map ) {
			_playerMarker.gameObject.SetActive( true );
			var normalizedPoint = App.Instance.Map.GetNormalizedPosition( Player.Instance.transform.position );
			_playerMarker.transform.localPosition = new Vector2( normalizedPoint.x * _content.rect.width - ( _content.rect.width / 2f ), normalizedPoint.z * _content.rect.height - ( _content.rect.height / 2f ) );
		} else {
			_playerMarker.gameObject.SetActive( false );
		}
	}
	private void Start () {

		ChangeMap( App.Instance.Map );
	}
	public void Set ( Map map ) {

		ChangeMap( map );
	}

	// handle events
	private void HandleInputFieldChanged ( string text ) {

		ClearSearchCells();
		BuildSearchCells( App.Instance.Search( text ) );
	}
	private void HandleDropdownChanged ( int val ) {

		ChangeMap( App.Instance.Server.Maps[val] );
	}

	// 
	private void ChangeMap ( Map map ) {

		_map = map;
		_mapVisual.sprite = _map._map;

		ClearMarkers();
		BuildMarkers();

		_places.Set( _map );
	}

	// clear
	private void ClearSearchCells () {

		foreach ( SearchCell inst in _searchCellInstances ) {
			Destroy( inst.gameObject );
		}

		_searchCellInstances.Clear();
	}
	private void ClearMarkers () {

		foreach ( UIMarker m in _markerInstances ) {
			Destroy( m.gameObject );
		}

		_markerInstances.Clear();
	}

	// builders
	private void BuildSearchCells ( List<(Map, Place)> results ) {

		foreach ( (Map, Place) r in results ) {

			var inst = Instantiate( _searchCell );
			inst.transform.SetParent( _searchContent, false );

			inst.Set( r.Item1, r.Item2 );

			_searchCellInstances.Add( inst );
		}
	}
	private void BuildMarkers () {

		foreach ( Place p in _map._pointsOfInterest ) {

			var inst = Instantiate( _pointOfInterestMarkerPrefab );
			var normalizedPoint = _map.GetNormalizedPosition( p.Position );

			inst.transform.SetParent( _content.transform );
			inst.Set( _map, p );

			_markerInstances.Add( inst );

			inst.transform.localPosition = new Vector2( normalizedPoint.x * _content.rect.width - ( _content.rect.width / 2f ), normalizedPoint.z * _content.rect.height - ( _content.rect.height / 2f ) );
		}
	}
}
