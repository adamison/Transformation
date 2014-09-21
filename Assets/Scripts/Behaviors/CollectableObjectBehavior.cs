using UnityEngine;
using System.Collections;

public class CollectableObjectBehavior : MonoBehaviour 
{
	
	public Component _EffectPrefab = null;
	public float _EffectHeightOffset = 0.0f;
	public Color _EffectColorModifier = Color.white;
	
	void OnTriggerEnter(Collider other) 
	{
		if(other.tag == "Player") 
		{
			Destroy(this.gameObject);
			
			if(_EffectPrefab != null)
			{				
				Vector3 pos = this.transform.position;
				pos.y += _EffectHeightOffset;
				Quaternion rot = this.transform.rotation;
				
				Component obj = Instantiate(_EffectPrefab, pos, rot) as Component;
				
				if(obj != null)
				{
					obj.particleSystem.startColor = _EffectColorModifier;
				}
				
			}
		}
	}

}
