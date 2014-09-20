using UnityEngine;
using System.Collections;

public class EnemyBase : MonoBehaviour
{
	
	// easy access Enemy script component
	Enemy enemy;
	public Enemy Enemy
	{
		get
		{
			return DataCore.GetCachedComponent<Enemy>( gameObject, ref enemy );
		}
	}
	
	// easy to acess test shooting component
	EnemySight sight;
	public EnemySight EnemySight
	{
		get
		{
			return DataCore.GetCachedComponent<EnemySight>( gameObject, ref sight );
		}
	}

}