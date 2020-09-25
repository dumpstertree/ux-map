using UnityEngine;
using UnityEngine.UI;

public class SearchCell : MonoBehaviour {

	[SerializeField] private Text _text = null;
	[SerializeField] private Button _gotoButton = null;

	public void Set ( Map map, Place place ) {

		_map = map;
		_place = place;

		_text.text = $"{place.Name} in {map._sceneName}";
	}


	private Map _map;
	private Place _place;

	private void Awake () {

		_gotoButton.onClick.AddListener( HandleGotoButtonClicked );
	}
	private void HandleGotoButtonClicked () {

		App.Instance.Load( _map, _place );
	}
}
