using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathCore : MonoBehaviour {

	static public List<Component> _WayPoints = new List<Component>();

	static public void _Reset() {
		_WayPoints = new List<Component>();
	}

	static public void _RegisterWayPoint(Component point) {
		_WayPoints.Add(point);	
	}
	
	static public Component _GetClosestWayPoint(Vector3 position) {
		Component closestWayPoint = null;
		float closestDistance = 9999999.0f;
		foreach(Component wayPoint in _WayPoints) {
			Vector3 vector = wayPoint.transform.position - position;
			float distance = vector.magnitude;
			if(distance < closestDistance) {
				closestDistance = distance;
				closestWayPoint = wayPoint;
			}
		}
		// Return the closest waypoint
		return closestWayPoint;
	}
	
	// _GetWayPointByCollider : Returns the WayPoint who has the indicated collider
	static public Component _GetWayPointByCollider(Collider obj) {
		if(obj == null) return null;
		// Search in waypoint collection for a matching game object
		foreach(Component wayPoint in _WayPoints) {
			if(wayPoint == null) continue;
			if(wayPoint.gameObject == obj.gameObject) return wayPoint;
			if(obj.transform.parent == null) continue;
			if(wayPoint.gameObject == obj.transform.parent.gameObject) return wayPoint;
		}

		return null;
	}
	
	////////////////////////////////////////////////////////////////////
	
	private const int _INFINITY = 9999999;
	
	public class PathFinder {
		private class NODE {
			public Component _WayPoint;			// The corresponding WayPoint positioned in the editor
			public List<NODE> _connections;		// Adjacencies of this node to other nodes
			public int _PathLength;				// Length of path found by Dijkstra's Algorithm
			public List<NODE> _Path;			// Path found by Dijkstra's Algorithm
			public bool _IsSealed;				// Indicates the node is sealed in Dijkstra's Algorithm
			public bool _IsTraveled; 			// Indicates a robot has traveled to this node 
		}
		
		private List<NODE> _WorkingSet;
		private List<NODE> _Nodes;
		
		private NODE _StartNode = null;
		private NODE _EndNode = null;
		
		// _OnInitialize : Called after all waypoints are registered
		public void _OnInitialize() {
			_WorkingSet = new List<NODE>();
			_Nodes = new List<NODE>();
			// Create a node for each waypoint and initialize it
			foreach(Component wp in _WayPoints) {
				NODE node = new NODE();
				node._WayPoint = wp;
				node._IsSealed = false;
				node._connections = new List<NODE>();
				node._PathLength = _INFINITY;
				node._Path = new List<NODE>();
				// Add the node to the collection of nodes
				_Nodes.Add(node);
			}
			// Discover connections between nodes
			_UpdateConnections();
		}
				
		private void _UpdateConnections() {
			// Check each node against each other node for visibility
			foreach(NODE node1 in _Nodes) {
				// [2] Construct graph manually
				node1._connections.Clear();
				WayPoint wayPoint = node1._WayPoint as WayPoint;
				foreach(WayPoint wp in wayPoint._Connections) {
						node1._connections.Add(
							_GetNodeFromWayPoint(wp)
						);
				}
			}
		}
		
		private NODE _GetNodeFromWayPoint(Component wayPoint) {
			foreach(NODE node in _Nodes) {
				if(node._WayPoint == wayPoint) {
					return node;
				}
			}
			return null;
		}
		
		private void _AppendWorkingSet(NODE node) {
			if(_WorkingSet.Contains(node)) return;
			_WorkingSet.Add(node);
		}
		
		public void _FindPath(Vector3 ptStart, Vector3 ptEnd) {
			Component wayPoint1 = PathCore._GetClosestWayPoint(ptStart);
			Component wayPoint2 = PathCore._GetClosestWayPoint(ptEnd);
			NODE node1 = _GetNodeFromWayPoint(wayPoint1);
			NODE node2 = _GetNodeFromWayPoint(wayPoint2);
			//node1._WayPoint.renderer.enabled = true;
			//node2._WayPoint.renderer.enabled = true;
			_FindPath(node1, node2);
		}
		
		private void _FindPath(NODE nodeStart, NODE nodeEnd) {
			_StartNode = nodeStart;
			_EndNode = nodeEnd;
			// Initialize Dijkstra's Algorithm
			foreach(NODE node in _Nodes) {
				node._Path.Clear();
				node._PathLength = _INFINITY;
				node._IsSealed = false;
				node._IsTraveled = false;				
			}
			nodeStart._PathLength = 0;
			nodeStart._Path.Add(nodeStart);
			_WorkingSet.Clear();
			
			// Verify validity of algorithm
			if(nodeStart._connections.Count <= 0) return;
			
			// FAMOUS: Dijkstra's Algorithm
			int max_counter = 0;
			_AppendWorkingSet(nodeStart);
			while(nodeEnd._IsSealed == false) {
				if(max_counter++ > 1000) break;
				for(int nNode1 = 0; nNode1 < _WorkingSet.Count; nNode1++) {
					NODE node1 = _WorkingSet[nNode1];
					if(node1._IsSealed) continue;
					for(int nNode2 = 0; nNode2 < node1._connections.Count; nNode2++) {
						NODE node2 = node1._connections[nNode2];
						if(node2._IsSealed) continue;
						_AppendWorkingSet(node2);
						int edgeLength = 1; // Assume edge length is 1
						if((node1._PathLength + edgeLength) < node2._PathLength) {
							node2._Path.Clear();
							node2._Path.AddRange(node1._Path);	
							node2._Path.Add(node2);
							node2._PathLength = node1._PathLength + edgeLength;
						}
					}
					// Extra stuff to give algorithm early out
					if(node1._connections.Count > 0) {
						bool bSealed = true;
						foreach(NODE node2 in node1._connections) {
							if(node2 == node1) continue;
							//if(node2._IsSealed == false) continue;
							if(node2._PathLength == _INFINITY) bSealed = false;
						}
						node1._IsSealed = bSealed;
					}
					if(nodeEnd._PathLength < _INFINITY) {
						if(node1._PathLength > nodeEnd._PathLength) {
							node1._IsSealed = true;	
						}
					}
				}
			}
			// Set all the waypoints in the path to illuminate
			foreach(NODE node in _EndNode._Path) {
				//node._WayPoint.renderer.enabled = true;	
			}
		}
		
		public Component _GetNextUntraveledWayPointOnPath(Vector3 position) {
			foreach(NODE node in _EndNode._Path) {
				if(node._IsTraveled) continue;
				return node._WayPoint;
			}
			return null;
		}
		
		public void _MarkWayPointAsTraveled(Component wayPoint) {
			foreach(NODE node in _EndNode._Path) {
				if(node._WayPoint == wayPoint) {
					node._IsTraveled = true;
					//wayPoint.renderer.enabled = false;
				}
			}
		}
	}
}
