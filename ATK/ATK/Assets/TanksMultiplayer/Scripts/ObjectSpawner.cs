/*  This file is part of the "Tanks Multiplayer" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace TanksMP
{
    /// <summary>
    /// Manages network-synced spawning of prefabs, in this case powerups.
    /// With the respawn time synced on all clients it supports host migration too.
    /// </summary>
	public class ObjectSpawner : NetworkBehaviour
	{
        /// <summary>
        /// Prefab to sync the instantiation for over the network.
        /// </summary>
		public GameObject prefab;
        
        /// <summary>
        /// Checkbox whether the object should be respawned after being despawned.
        /// </summary>
        public bool respawn;
        
        /// <summary>
        /// Delay until respawning the object again after it got despawned.
        /// </summary>
		public int respawnTime;
        
        /// <summary>
        /// Reference to the spawned prefab gameobject instance in the scene.
        /// </summary>
        [HideInInspector]
        public GameObject obj;
        
        //time value when the next respawn should happen measured in game time
        private float nextSpawn;
        

        /// <summary>
        /// Server only: check if the object is active in the scene already, then the
        /// host or migrated host does not have to do anything. If the object is not
        /// active, the host has to continue with the SpawnCoroutine where it left off
        /// </summary>
		public override void OnStartServer()
		{         
            if(obj != null && obj.activeInHierarchy)
                return;
                           
            StartCoroutine(SpawnRoutine());
		}


        //calculates the remaining time until the next respawn,
        //waits for the delay to have passed and then instantiates the object
		IEnumerator SpawnRoutine()
		{    
            float delay = Mathf.Clamp(nextSpawn - Time.time, 0, respawnTime);
			yield return new WaitForSeconds(delay);
			Instantiate();
		}
		
        
        /// <summary>
        /// Server only: spawns the prefab over the network, making use of object pooling.
        /// Keeps the parent-child tracking by setting the network id to this managing gameobject.
        /// </summary>
		[Server]
		public void Instantiate()
		{
			obj = PoolManager.Spawn(prefab, transform.position, transform.rotation);
            obj.GetComponent<Powerup>().parentId = GetComponent<NetworkIdentity>().netId;
			NetworkServer.Spawn(obj, prefab.GetComponent<NetworkIdentity>().assetId);
		}
        
        
        /// <summary>
        /// Server only: despawns the instantiated object across the network using object pooling.
        /// If respawn is enabled, trigger the spawn coroutine to wait until the next spawn time.
        /// </summary>
		[Server]
		public void Destroy()
		{
			PoolManager.Despawn(obj);

			NetworkServer.UnSpawn(obj);
            
			obj = null;
			if(respawn)
            {
                StartCoroutine(SpawnRoutine());
            }
		}
        
        
        /// <summary>
        /// Sets the next spawn time in 'respawnTime' seconds from now on, measured in game time.
        /// This is done on all clients too, so that everyone can continue where the old host has
        /// left off to not disrupt the timed spawn intervals of powerups.
        /// </summary>
        public void SetRespawn()
        {
            nextSpawn = Time.time + respawnTime;
        }
	}
}
