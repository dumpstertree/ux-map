using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour {

	void LateUpdate () {

		var player = Player.Instance.transform;
		transform.position = Vector3.Lerp( transform.position, player.position + ( -player.forward * 10 ) + ( Vector3.up * 2 ), 0.2f );
		transform.LookAt( player );
	}
}
