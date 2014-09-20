using System;
using UnityEngine;
using System.Collections;

public class AICore : MonoBehaviour {
	
	public static System.Random _RAND = null;
	
	// System Routines : Miscellaneous Initialization and Runtime Functions ////
	
	void Start () {
	}
	
	void Update () {
	}
	
	// Probability : Miscellaneous Probability and Statistics Functions ////////
	
	bool _AreFloatsEqual(float x, float y, float threshhold) {
		return System.Math.Abs(x - y) < threshhold;
	}
	
	public static int _RandomInteger(int min, int max) {
		if(_RAND == null) {
			_RAND = new System.Random((int)DateTime.Now.Ticks);
		}
		int n = _RAND.Next(min, max);
		return n;
	}
	
	public static float _RandomFloat(float min, float max) {
		if(_RAND == null) {
			_RAND = new System.Random((int)DateTime.Now.Ticks);
		}
		float z = _RAND.Next(0, 1000) / 1000.0f;
		return _Defuzzify(z, min, max);
	}
	
	public static bool _RandomProbability(float pr) {
		return ((1.0f - pr) < _RandomFloat(0.0f, 1.0f));	
	}
	
	public static int _RandomDistribution(float[] dist) {
		// Pick a random spot in the distribution
		float r = _RandomFloat(0.0f, 1.0f);
		float q = 0.0f;
		// Normalize the contents of the array
		float sum = 0.0f;
		foreach(float val in dist) {
			sum += val;	
		}
		for(int n = 0; n < dist.Length; n++) {
			// Normalize this item if the distribution sum is > 1.0
			if(sum > 1.0f) dist[n] /= sum;
			// (If the dist sum is greater than 1, an item will be definitely chosen)
			// (If the dist sum is less than 1, the possibility for no item (-1) is returned)
			
			// Pick an item according to its probability
			q += dist[n];
			if(r < q) return n;
		}
		// The caller must check for -1 if they passed in a non-100% distribution
		return -1;
	}
	
	// Fuzzy Logic : Fuzzification Functions ///////////////////////////////////
	
	public static float _IsItMax(float x, float min, float max) {
		return (x - min) / (max - min);	
	}
	
	public static float _IsItMin(float x, float min, float max) {
		return 1.0f - _IsItMax(x, min, max);	
	}
	
	// Fuzzy Logic : Contemplation Functions //////////////////////////////////
	
	public static float _And(float x, float y) {
		return x * y;	
	}
	
	public static float _And(params float[] _collection) {
		float z = 1.0f;
		foreach(float q in _collection) {
			z = z * q;	
		}
		return z;
	}
	
	public static float _Or(float x, float y) {
		float z = x + y;
		if(z > 1.0f) z = 1.0f;
		return z;
	}
	
	public static float _Or(params float[] _collection) {
		float z = 0.0f;
		foreach(float q in _collection) {
			z = z + q;
			if(z > 1.0f) z = 1.0f;
		}
		return z;
	}
	
	public static float _Not(float x) {
		return 1 - x;	
	}
	
	public static float _Yes(float x) {
		return x;	
	}
	
	// Fuzzy Logic : Defuzzification Functions //////////////////////////////
	
	public static float _Defuzzify(float z, float min, float max) {
		return min + z * (max - min);	
	}
	
	// Behavior Matrices ////////////////////////////////////////////////////
	
	public static float _BehaviorMatrix3(float[] B, float[] C) {
		float z = _Or(
			B[0] * _And(_Not(C[2]), _Not(C[1]), _Not(C[0])),      
			B[1] * _And(_Not(C[2]), _Not(C[1]), _Yes(C[0])),      
			B[2] * _And(_Not(C[2]), _Yes(C[1]), _Not(C[0])),      
			B[3] * _And(_Not(C[2]), _Yes(C[1]), _Yes(C[0])),      
			B[4] * _And(_Yes(C[2]), _Not(C[1]), _Not(C[0])),      
			B[5] * _And(_Yes(C[2]), _Not(C[1]), _Yes(C[0])),      
			B[6] * _And(_Yes(C[2]), _Yes(C[1]), _Not(C[0])),      
			B[7] * _And(_Yes(C[2]), _Yes(C[1]), _Yes(C[0]))   
			);
		return z;
	}
	
	// Major Utility Functions ////////////////////////////////////////////
	
	// _GetTargetDistance : Returns the distance to the target from the source
	public static float _GetTargetDistance(GameObject source, GameObject target) {
		if(source == null) return 0.0f;
		if(target == null) return 0.0f;
		
		// Determine vector toward target
		Vector3 vector = target.transform.position - source.transform.position;
		
		// Return the distance to the target
		return vector.magnitude;
	}
	
	public static void _GetOrientationAwareness3D(
		Component source, Component target,
		out float zSameForward, out bool bRollRight,
		out float zSameRight, out bool bPitchUp,
		out float zSameUp, out bool bYawRight
		) {
		zSameForward = 0.0f;
		zSameRight = 0.0f;
		zSameUp = 0.0f;
		bRollRight = false;
		bPitchUp = false;
		bYawRight = false;
		//if(targetPosition == null) return;
		if(source == null) return;
		
		// Determine our orientation vectors
		Vector3 s_fwd = source.transform.TransformDirection(Vector3.forward);
		Vector3 s_rgt = source.transform.TransformDirection(Vector3.right);
		Vector3 s_up = source.transform.TransformDirection(Vector3.up);
		
		// Determine target orientation vectors
		Vector3 t_fwd = target.transform.TransformDirection(Vector3.forward);
		Vector3 t_rgt = target.transform.TransformDirection(Vector3.right);
		Vector3 t_up = target.transform.TransformDirection(Vector3.up);
		
		// Need to normalize the vectors
		s_fwd.Normalize();
		s_rgt.Normalize();
		s_up.Normalize();
		t_fwd.Normalize();
		t_rgt.Normalize();
		t_up.Normalize();
		
		// Answer the question: Am I looking in same forward direction?
		float dot1 = Vector3.Dot(s_fwd, t_fwd);
		zSameForward = _IsItMax(dot1, -1.0f, 1.0f);
		
		// Answer the question: Am I oriented in the same rightward direction?
		float dot2 = Vector3.Dot(s_rgt, t_rgt);
		zSameRight = _IsItMax(dot2, -1.0f, 1.0f);
		
		// Answer the question: Am I oriented in the same upward direction?
		float dot3 = Vector3.Dot(s_up, t_up);
		zSameUp = _IsItMax(dot3, -1.0f, 1.0f);
		
		// Answer the question: Is it best to yaw rightward?
		float dot4 = Vector3.Dot(s_fwd, t_rgt);
		bYawRight = dot4 < 0.0f;
	}
	
	// _GetSpatialAwareness3D : Answers the fuzzy questions "Is target in front?" and
	// all other directions (back, right, left). Also returns distance to target.
	public static void _GetSpatialAwareness3D(
		Component source, Component target, out float fTargetDistance,
		out float zIsTargetBehindMe, out float zIsTargetInFrontOfMe,
		out float zIsTargetToMyLeft, out float zIsTargetToMyRight,
		out float zIsTargetAboveMe, out float zIsTargetBelowMe
		) {
		_GetSpatialAwareness3D(source, target.transform.position, out fTargetDistance,
		                       out zIsTargetBehindMe, out zIsTargetInFrontOfMe,
		                       out zIsTargetToMyLeft, out zIsTargetToMyRight,
		                       out zIsTargetAboveMe, out zIsTargetBelowMe
		                       );
	}
	
	// _GetSpatialAwareness3D : Full version supports vector as the target
	public static void _GetSpatialAwareness3D(
		Component source, Vector3 targetPosition, out float fTargetDistance,
		out float zIsTargetBehindMe, out float zIsTargetInFrontOfMe,
		out float zIsTargetToMyLeft, out float zIsTargetToMyRight,
		out float zIsTargetAboveMe, out float zIsTargetBelowMe
		) {
		fTargetDistance = 0.0f;
		zIsTargetBehindMe = 0.0f;
		zIsTargetInFrontOfMe = 0.0f;
		zIsTargetToMyLeft = 0.0f;
		zIsTargetToMyRight = 0.0f;
		zIsTargetAboveMe = 0.0f;
		zIsTargetBelowMe = 0.0f;
		//if(targetPosition == null) return;
		if(source == null) return;
		
		// Determine our orientation vectors
		Vector3 fwd = source.transform.TransformDirection(Vector3.forward);
		Vector3 rgt = source.transform.TransformDirection(Vector3.right);
		Vector3 up = source.transform.TransformDirection(Vector3.up);
		
		// Determine vector toward target
		Vector3 vector = targetPosition - source.transform.position;
		fTargetDistance = vector.magnitude;
		
		// Need to normalize the vectors
		vector.Normalize();
		fwd.Normalize();
		rgt.Normalize();
		up.Normalize();
		
		// Answer the question: Is the target in front or behind me?
		float dot1 = Vector3.Dot(fwd, vector);
		zIsTargetBehindMe = _IsItMin(dot1, -1.0f, 1.0f);
		zIsTargetInFrontOfMe = _Not(zIsTargetBehindMe);
		
		// Answer the question: Is the target to my left or right?
		float dot2 = Vector3.Dot(rgt, vector);
		zIsTargetToMyLeft = _IsItMin(dot2, -1.0f, 1.0f);
		zIsTargetToMyRight = _Not(zIsTargetToMyLeft);
		
		// Answer the question: Is the target above or below me?
		float dot3 = Vector3.Dot (up, vector);
		zIsTargetBelowMe = _IsItMin(dot3, -1.0f, 1.0f);
		zIsTargetAboveMe = _Not(zIsTargetBelowMe);
	}
	
	// _GetSpatialAwareness2D : Answers the fuzzy questions "Is target in front?" and
	// all other directions (back, right, left). Also returns distance to target.
	public static void _GetSpatialAwareness2D(Component source, Component target, out float fTargetDistance, out float zIsTargetBehindMe, out float zIsTargetInFrontOfMe, out float zIsTargetToMyLeft, out float zIsTargetToMyRight) {
		_GetSpatialAwareness2D(source, target.transform.position, out fTargetDistance, out zIsTargetBehindMe, out zIsTargetInFrontOfMe, out zIsTargetToMyLeft, out zIsTargetToMyRight);
	}
	
	// _GetSpatialAwareness2D : Full version supports vector as the target
	public static void _GetSpatialAwareness2D(Component source, Vector3 targetPosition, out float fTargetDistance, out float zIsTargetBehindMe, out float zIsTargetInFrontOfMe, out float zIsTargetToMyLeft, out float zIsTargetToMyRight) {
		fTargetDistance = 0.0f;
		zIsTargetBehindMe = 0.0f;
		zIsTargetInFrontOfMe = 0.0f;
		zIsTargetToMyLeft = 0.0f;
		zIsTargetToMyRight = 0.0f;
		//if(targetPosition == null) return;
		if(source == null) return;
		
		// Determine our orientation vectors
		Vector3 fwd = source.transform.TransformDirection(Vector3.forward);
		Vector3 rgt = source.transform.TransformDirection(Vector3.right);
		
		// Determine vector toward target
		Vector3 vector = targetPosition - source.transform.position;
		fTargetDistance = vector.magnitude;
		
		// Need to normalize the vectors
		vector.Normalize();
		fwd.Normalize();
		rgt.Normalize();
		
		// Answer the question: Is the target in front or behind me?
		float dot1 = Vector3.Dot(fwd, vector);
		zIsTargetBehindMe = _IsItMin(dot1, -1.0f, 1.0f);
		zIsTargetInFrontOfMe = _Not(zIsTargetBehindMe);
		
		// Answer the question: Is the target to my left or right?
		float dot2 = Vector3.Dot(rgt, vector);
		zIsTargetToMyLeft = _IsItMin(dot2, -1.0f, 1.0f);
		zIsTargetToMyRight = _Not(zIsTargetToMyLeft);
	}
}
