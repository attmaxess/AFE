/*  This file is part of the "Tanks Multiplayer" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using UnityEngine;
using UnityEngine.Networking;

namespace TanksMP
{
    /// <summary>
    /// Projectile script for player shots with collision/hit logic.
    /// </summary>
	public class Bullet : NetworkBehaviour
    {
        /// <summary>
        /// Projectile travel speed in units.
        /// </summary>
        public float speed = 10;
        
        /// <summary>
        /// Damage to cause on a player that gets hit.
        /// </summary>
        public int damage = 3;
        
        /// <summary>
        /// Delay until despawned automatically when nothing gets hit.
        /// </summary>
        public float despawnDelay = 1f;
        
        /// <summary>
        /// Bounce count of walls and other environment obstactles.
        /// </summary>
		public int bounce = 0;

        /// <summary>
        /// Clip to play when a player gets hit.
        /// </summary>
        public AudioClip hitClip;
        
        /// <summary>
        /// Clip to play when this projectile gets despawned.
        /// </summary>
        public AudioClip explosionClip;
        
        /// <summary>
        /// Object to spawn when a player gets hit.
        /// </summary>
        public GameObject hitFX;
        
        /// <summary>
        /// Object to spawn when this projectile gets despawned.
        /// </summary>
        public GameObject explosionFX;

        //reference to rigidbody component
        private Rigidbody myRigidbody;
        //reference to collider component
        private SphereCollider sphereCol;
        //caching maximum count of bounces for restore
        private int maxBounce;
        
        /// <summary>
        /// Player gameobject that spawned this projectile.
        /// </summary>
        [HideInInspector]
		[SyncVar]
        public GameObject owner;


        //get component references
        void Awake()
        {
            myRigidbody = GetComponent<Rigidbody>();
            sphereCol = GetComponent<SphereCollider>();
            maxBounce = bounce;
        }


        //set initial travelling velocity
        void OnSpawn()
        {
            myRigidbody.velocity = speed * transform.forward;
        }
        
        
        /// <summary>
        /// On Host, add automatic despawn coroutine
        /// </summary>
        public override void OnStartServer()
        {
            PoolManager.Despawn(gameObject, despawnDelay);
        }
        
        
        //check what was hit on collisions
        void OnTriggerEnter(Collider col)
        {
            //cache corresponding gameobject that was hit
            GameObject obj = col.gameObject;
            //try to get a player component out of the collided gameobject
            Player player = obj.GetComponent<Player>();
            
            //we actually hit a player
            //do further checks
            if(player != null)
            {
                //ignore ourselves & disable friendly fire (same team index)
                if (player.gameObject == owner) return;
                else if (player.teamIndex == owner.GetComponent<Player>().teamIndex) return;

                //create clips and particles on hit
                if (hitFX) PoolManager.Spawn(hitFX, transform.position, Quaternion.identity);
                if (hitClip) AudioManager.Play3D(hitClip, transform.position);

                //on the player that was hit, set the killing player to the owner of this bullet
                //maybe this owner really killed the player, but that check is done in the Player script
                player.killedBy = owner;
            }
		    else if(bounce > 0)
            {
                //a player was not hit but something else, and we still have some bounces left
                //create a ray that points in the direction this bullet is currently flying to
                Ray ray = new Ray(transform.position - transform.forward, transform.forward);
			    RaycastHit hit;

                //perform spherecast in the flying direction, on the default layer
			    if(Physics.SphereCast(ray, sphereCol.radius, out hit, speed, 1 << 0))
			    {
                    //something was hit in the direction this projectile is flying to
                    //get new reflected (bounced off) direction of the colliding object
                    Vector3 dir = Vector3.Reflect(ray.direction, hit.normal);
                    //rotate bullet to face the new direction
                    transform.rotation = Quaternion.LookRotation(dir);
                    //reassign velocity with the new direction in mind
                    OnSpawn();

                    //play clip at the collided position
                    if (hitClip) AudioManager.Play3D(hitClip, transform.position);
                    //substract bouncing count by one
                    bounce--;
                    //exit execution until next collision
                    return;
                }
            }
           
            //the previous code is not synced to clients at all, because all that clients need is the
            //initial position and direction of the bullet to calculate the exact same behavior on their end.
            //at this point, continue with the critical game aspects only on the server
            if(!isServer) return;

            //apply bullet damage to the collided player
            if(player) player.TakeDamage(this);
            //despawn gameobject
            PoolManager.Despawn(gameObject);
        }


        //set despawn effects and reset variables
        void OnDespawn()
        {
            //create clips and particles on despawn
            if(explosionFX) PoolManager.Spawn(explosionFX, transform.position, transform.rotation);
            if(explosionClip) AudioManager.Play3D(explosionClip, transform.position);

            //reset modified variables to the initial state
            myRigidbody.velocity = Vector3.zero;
            myRigidbody.angularVelocity = Vector3.zero;
            bounce = maxBounce;
            
            //skip for non-hosts
            if(!isServer) return;
            //server despawned this instance, despawn it for the network too
            NetworkServer.UnSpawn(gameObject);
        }
    }
}
