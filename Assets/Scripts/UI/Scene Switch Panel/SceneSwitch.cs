using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour {

	[SerializeField] private string[] _sceneNames = new string[0];
	[SerializeField] private Button _togglePrefab = null;
	[SerializeField] private Transform _content;

	private static SceneSwitch _inst = null;


	private void Awake () {

		UnityEngine.Screen.fullScreen = true;

		if ( _inst == null ) {
			_inst = this;
		} else {
			Destroy( gameObject );
		}


		DontDestroyOnLoad( this );

		for ( int i = 0; i < _sceneNames.Length; i++ ) {

			var sceneName = _sceneNames[i];
			var inst = Instantiate( _togglePrefab );

			inst.GetComponentInChildren<Text>().text = ( i + 1 ).ToString();
			inst.transform.SetParent( _content, false );
			inst.onClick.AddListener( () => Load( sceneName ) );
		}
	}

	private void Load ( string path ) {

		SceneManager.LoadScene( path, LoadSceneMode.Single );
	}
}
