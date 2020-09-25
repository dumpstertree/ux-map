using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load : MonoBehaviour {

	private void Start () {

		App.Instance.Load( App.Instance.Server.Maps[0] );
	}
}
