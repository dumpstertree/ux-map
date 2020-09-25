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

	private void Awake () {

		_instance = this;

		_maximizeButton.onClick.AddListener( Maximize );
	}

	private void Maximize () {

		MapUI.Instance.Present();
	}
}
