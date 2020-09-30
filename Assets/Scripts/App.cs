using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class App : MonoBehaviour {


	public static App Instance {
		get; private set;
	}

	public Server Server {
		get => _server;
		set => _server = value;
	}
	public Map Map {
		get => _map;
		set => _map = value;
	}

	public List<(Map, Place)> Search ( string param ) {

		var results = new List<(Map, Place)>();

		if ( string.IsNullOrEmpty( param ) ) {
			return results;
		}

		foreach ( Map m in Server.Maps ) {
			foreach ( Place p in m._pointsOfInterest ) {

				if ( p.Name.Contains( param ) ) {
					results.Add( (m, p) );
				}
			}
		}

		return results;
	}
	public void Load ( Map map ) {

		if ( map != Map ) {

			Map = map;
			SceneManager.LoadScene( map._sceneName );
			MapUI.Instance.Set( Map );
		}

		if ( Player.Instance != null ) {
			Player.Instance.transform.position = Vector3.zero;
		}

		MapUI.Instance?.Dismiss();
	}
	public void Load ( Map map, Place teleportLocation ) {

		if ( map != Map ) {

			Map = map;
			SceneManager.LoadScene( map._sceneName );
			MapUI.Instance.Set( Map );
		}

		if ( Player.Instance != null ) {
			Player.Instance.Teleport( teleportLocation.Position );
		}

		MapUI.Instance.Dismiss();
	}


	[SerializeField] private Server _server = null;
	[SerializeField] private Map _map = null;
	[SerializeField] private WSMarker _markerPrefab = null;

	private List<WSMarker> _markers = new List<WSMarker>();


	private void Awake () {

		if ( Instance == null ) {
			Instance = this;
			DontDestroyOnLoad( gameObject );
		} else {
			Destroy( gameObject );
		}
	}
	private void Start () {
		Load( _server.Maps[0] );
	}

	private void OnLevelWasLoaded () {

		foreach ( Place p in Map._pointsOfInterest ) {

			var inst = Instantiate( _markerPrefab );
			inst.Set( p );

			inst.transform.position = p.Position + Vector3.up * 2.5f;
			_markers.Add( inst );
		}
	}
}
