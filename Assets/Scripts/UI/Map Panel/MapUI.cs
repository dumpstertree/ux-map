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

		ClearSearchCells();
		gameObject.SetActive( false );
		_search.text = "";
	}

	[Header( "Interaction" )]
	[SerializeField] private Button _minimizeButton = null;
	[SerializeField] private RectTransform _mapContent = null;
	[SerializeField] private RectTransform _searchContent = null;

	[Header( "Interaction" )]
	[SerializeField] private Dropdown _areaDropdown = null;
	[SerializeField] private InputField _search = null;

	[Header( "Prefabs" )]
	[SerializeField] private UIMarker _pointOfInterestMarkerPrefab = null;
	[SerializeField] private PointOfInterestCell _placeOfInterestCellPrefab = null;
	[SerializeField] private SearchCell _searchCellPrefab = null;
	[SerializeField] private GameObject _playerMarkerPrefab = null;

	[SerializeField] private Image _mapVisual = null;



	private Map _map;
	private List<UIMarker> _markerInstances = new List<UIMarker>();
	private List<SearchCell> _searchCellInstances = new List<SearchCell>();
	private List<PointOfInterestCell> _placesOfInterestCells = new List<PointOfInterestCell>();

	private GameObject _playerInstance;


	// mono
	private void Awake () {

		_instance = this;

		_minimizeButton.onClick.AddListener( Dismiss );
		_areaDropdown.onValueChanged.AddListener( HandleDropdownChanged );
		_search.onValueChanged.AddListener( HandleInputFieldChanged );
		_search.onValidateInput += delegate ( string s, int i, char c ) { return char.ToLower( c ); };

		var options = new List<Dropdown.OptionData>();
		foreach ( Map m in App.Instance.Server.Maps ) {
			options.Add( new Dropdown.OptionData( m._sceneName ) );
		}

		_areaDropdown.options = options;
		_playerMarkerPrefab = Instantiate( _playerMarkerPrefab );
		_playerMarkerPrefab.transform.SetParent( _mapContent );
	}
	private void LateUpdate () {

		if ( _map == App.Instance.Map && Player.Instance != null ) {
			_playerMarkerPrefab.gameObject.SetActive( true );
			var normalizedPoint = App.Instance.Map.GetNormalizedPosition( Player.Instance.transform.position );
			_playerMarkerPrefab.transform.localPosition = new Vector2( normalizedPoint.x * _mapContent.rect.width - ( _mapContent.rect.width / 2f ), normalizedPoint.z * _mapContent.rect.height - ( _mapContent.rect.height / 2f ) );
		} else {
			_playerMarkerPrefab.gameObject.SetActive( false );
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

		ClearPlacesOfInterestCells();
		ClearSearchCells();

		if ( string.IsNullOrEmpty( text ) ) {
			BuildPlacesOfInterestCells();
		} else {
			BuildSearchCells( App.Instance.Search( text ) );
		}
	}
	private void HandleDropdownChanged ( int val ) {

		ChangeMap( App.Instance.Server.Maps[val] );
	}

	private void ChangeMap ( Map map ) {

		_map = map;
		_mapVisual.sprite = _map._map;

		ClearMarkers();
		BuildMarkers();

		ClearPlacesOfInterestCells();
		ClearSearchCells();

		if ( string.IsNullOrEmpty( _search.text ) ) {
			BuildPlacesOfInterestCells();
		} else {
			BuildSearchCells( App.Instance.Search( _search.text ) );
		}
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
	private void ClearPlacesOfInterestCells () {

		foreach ( PointOfInterestCell c in _placesOfInterestCells ) {
			Destroy( c.gameObject );
		}

		_placesOfInterestCells.Clear();
	}


	// builders
	private void BuildSearchCells ( List<(Map, Place)> results ) {

		foreach ( (Map, Place) r in results ) {

			var inst = Instantiate( _searchCellPrefab );
			inst.transform.SetParent( _searchContent, false );

			inst.Set( r.Item1, r.Item2 );

			_searchCellInstances.Add( inst );
		}
	}
	private void BuildMarkers () {

		foreach ( Place p in _map._pointsOfInterest ) {

			var inst = Instantiate( _pointOfInterestMarkerPrefab );
			var normalizedPoint = _map.GetNormalizedPosition( p.Position );

			inst.transform.SetParent( _mapContent.transform );
			inst.Set( _map, p );

			_markerInstances.Add( inst );

			inst.transform.localPosition = new Vector2( normalizedPoint.x * _mapContent.rect.width - ( _mapContent.rect.width / 2f ), normalizedPoint.z * _mapContent.rect.height - ( _mapContent.rect.height / 2f ) );
		}
	}
	private void BuildPlacesOfInterestCells () {
		foreach ( Place p in _map._pointsOfInterest ) {

			var inst = Instantiate( _placeOfInterestCellPrefab );
			inst.transform.SetParent( _searchContent, false );

			inst.Set( _map, p );
			_placesOfInterestCells.Add( inst );
		}
	}

}
