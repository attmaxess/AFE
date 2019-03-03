/*  This file is part of the "Tanks Multiplayer" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using UnityEngine;
using UnityEngine.Networking;

namespace TanksMP
{
    /// <summary>
    /// Base class for all derived Powerup scripts (health, shields, etc.)
    /// Extend this to create highly customized Powerups with specific functionality.
    /// </summary>
	public class Powerup : NetworkBehaviour
	{	
        /// <summary>
        /// Network reference to the object that spawned this powerup in the scene.
        /// Used for access to the ObjectSpawner parent in OnDespawn().
        /// </summary>
		[HideInInspector]
		[SyncVar]
        public NetworkInstanceId parentId;

        /// <summary>
        /// Clip to play when this powerup is consumed by a player.
        /// </summary>
        public AudioClip useClip;
        
        //Reference to the local object (script) that spawned this powerup.
		#pragma warning disable 0649
        private ObjectSpawner spawner;
		#pragma warning restore 0649
        
        
        /// <summary>
        /// Get component references once the network is initialized.
        /// </summary>
        public override void OnStartClient()
        {
            GameObject parentObj = ClientScene.FindLocalObject(parentId);
            spawner = parentObj.GetComponent<ObjectSpawner>();
            spawner.obj = gameObject;
        }

        
        //Server only: check for players colliding with the powerup
        //possible collision are defined in the Physics Matrix
		void OnTriggerEnter(Collider col)
		{
            if (!isServer) return;

    		GameObject obj = col.gameObject;
			Player player = obj.GetComponent<Player>();

            //no need to check whether player is null
            //try to apply powerup to player. Destroy after use
            if (Apply(player))
			    spawner.Destroy();
		}


        /// <summary>
        /// Tries to apply the powerup to a colliding player. Returns 'true' if consumed.
        /// Override this method in your own powerup script to implement custom powerups.
        /// </summary>
		public virtual bool Apply(Player p)
		{
            //do something to the player
            //return result
            return false;
		}
        
        
        //if consumed, play audio clip. Now with the powerup despawned,
        //set the next spawn time on the managing ObjectSpawner script
        void OnDespawn()
        {
            if (useClip) AudioManager.Play3D(useClip, transform.position);
            spawner.SetRespawn();
        }
	}
}
