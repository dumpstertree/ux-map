using UnityEngine;

[System.Serializable]
public class Map {

	public Vector3 GetNormalizedPosition ( Vector3 position ) {

		var x = ( position.x + ( _width / 2f ) ) / _width;
		var z = ( position.z + ( _width / 2f ) ) / _height;

		return new Vector3( x, 0, z );
	}

	[SerializeField] public string _sceneName = "";
	[SerializeField] public Place[] _pointsOfInterest = null;

	[SerializeField] public Sprite _map = null;
	[SerializeField] public float _width = 0f;
	[SerializeField] public float _height = 0f;
}

[System.Serializable]
public class Server {

	[SerializeField] public Map[] Maps;
}

[System.Serializable]
public class Place {

	[SerializeField] public string Name;
	[SerializeField] public Vector3 Position;
	[SerializeField] public Sprite Sprite;
}