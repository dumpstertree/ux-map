using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacesContent : MonoBehaviour {

	[SerializeField] private PointOfInterestCell _pointOfInterestCellPrefab = null;
	[SerializeField] private Transform _content = null;

	private List<PointOfInterestCell> _instances = new List<PointOfInterestCell>();

	public void Set ( Map map ) {

		Clear();

		foreach ( Place p in map._pointsOfInterest ) {

			var inst = Instantiate( _pointOfInterestCellPrefab );
			inst.transform.SetParent( _content, false );

			inst.Set( map, p );
			_instances.Add( inst );
		}
	}
	private void Clear () {

		foreach ( PointOfInterestCell c in _instances ) {
			Destroy( c.gameObject );
		}

		_instances.Clear();
	}
}
