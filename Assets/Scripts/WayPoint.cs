using UnityEngine;
using System.Collections;

public class WayPoint : MonoBehaviour 
{	
	// _Connections : Waypoint connections (configurable by user)
	public WayPoint[] _Connections = null;
	
	void Start () 
	{
		this.renderer.enabled = false;
		PathCore._RegisterWayPoint(this);
	}
}
