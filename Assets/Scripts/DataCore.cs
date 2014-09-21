using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataCore : MonoBehaviour
{
	// DataCore
	//-------------------------------------------------------	
	public static DataCore instance;

	public GameObject enemyPrefab;
	public GameObject playerPrefab;
	public GameObject playerObject;
	public static Player player;
	
	public Transform spawnPoint;
	public Transform[] enemySpawnPoints;
	public List<GameObject> enemies = new List<GameObject>();
	
	// View determines what is seen by the player and what actions they may take
	public enum VIEW { Physical, Metaphysical };
	
	// Default to physical view
	public static VIEW _view = VIEW.Physical;
	
	void Awake()
	{
		instance = this;

		PathCore._Reset();
		
		//playerObject = GameObject.FindGameObjectWithTag("Player");
		CreatePlayer();
	}
	
	public static T GetCachedComponent<T>( GameObject gameObject, ref T cachedComponent ) where T : MonoBehaviour
	{
		if( cachedComponent == null )
		{
			cachedComponent = gameObject.GetComponent<T>();
		}
		
		return cachedComponent;
	}
	
	public void CreatePlayer()
	{		
		GameObject playerObject = Instantiate (playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		
		playerObject.transform.position = spawnPoint.transform.position;
		playerObject.transform.rotation = spawnPoint.transform.rotation;

		player = playerObject.GetComponent<Player>();
	}

	public static void RespawnPlayer()
	{
		player.health = player.maxHealth;
		player.transform.position = instance.spawnPoint.transform.position;
		player.transform.rotation = instance.spawnPoint.transform.rotation;
	}

	public static void GameOver()
	{
		Application.LoadLevel(2);
	}
}
