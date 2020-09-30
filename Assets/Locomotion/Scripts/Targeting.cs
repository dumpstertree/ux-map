using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Targeting : MonoBehaviour {


	public static void Register ( Target target ) {

		_targets.Add( target );
	}
	public static void Deregister ( Target target ) {

		_targets.Remove( target );
	}
	private static List<Target> _targets = new List<Target>();

	private const float DOT_CUTOFF = 0.0f;
	private const float DISTANCE_CUTOFF = 5f;

	public static Target GetBestTarget ( Vector3 position, Vector3 forward ) {

		var tars = new List<Target>( _targets );

		// remove out of distance
		for ( int i = tars.Count - 1; i >= 0; i-- ) {

			var curTarget = tars[i];
			var curDist = Vector3.Distance( curTarget.Position, position );

			if ( curDist > DISTANCE_CUTOFF ) {
				tars.RemoveAt( i );
			}
		}

		// remove out of dot
		for ( int i = tars.Count - 1; i >= 0; i-- ) {

			var curTarget = tars[i];
			var curDot = Vector3.Dot( ( curTarget.Position - position ).normalized, forward );

			if ( curDot < DOT_CUTOFF ) {
				tars.RemoveAt( i );
			}
		}

		tars.Sort( ( x, y ) => y.Influence.CompareTo( x.Influence ) );

		// idk return the first
		if ( tars.Count > 0 ) {
			return tars[0];
		}

		return null;
	}
}
