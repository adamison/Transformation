using UnityEngine;
using System.Collections;

public class MPActionBehavior : MonoBehaviour 
{
	// Attach to any object that requires new behavior while in MP view
	
	private bool _actionComplete = false;
	
	// Test data
	private Material _originalMaterial;
	public Material _mpMaterial;

	void Start ()
	{
		_originalMaterial = renderer.material;
	}

	void Update ()
	{
		if(DataCore._view == DataCore.VIEW.Metaphysical)
		{
			MetaphysicalAction();
		}
		
		if(DataCore._view == DataCore.VIEW.Physical)
		{
			PhysicalAction();
		}
	}
	
	void MetaphysicalAction ()
	{
		// for now let's just swap the wall material
		renderer.material = _mpMaterial;
	}
	
	void PhysicalAction ()
	{
		renderer.material = _originalMaterial;
	}
}
