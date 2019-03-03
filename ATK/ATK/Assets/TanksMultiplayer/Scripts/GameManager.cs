/*  This file is part of the "Tanks Multiplayer" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Networking;

namespace TanksMP
{
    /// <summary>
    /// Manages game workflow and provides high-level access to networked logic during a game.
    /// It manages functions such as team fill, scores and ending a game, but also video ad results.
    /// </summary>
	public class GameManager : NetworkBehaviour
    {   
        //reference to this script instance
        private static GameManager instance;
        
        /// <summary>
        /// The local player instance spawned for this client.
        /// </summary>
        [HideInInspector]
        public Player localPlayer;
        
        /// <summary>
        /// Reference to the UI script displaying game stats.
        /// </summary>
        public UIGame ui;
        
        /// <summary>
        /// Definition of playing teams with additional properties.
        /// </summary>
        public Team[] teams;
        
        /// <summary>
        /// Networked list storing team fill for each team.
        /// E.g. if size[0] = 2, there are two players in team 0.
        /// </summary>
        public SyncListInt size = new SyncListInt();
        
        /// <summary>
        /// Networked list storing team scores for each team.
        /// E.g. if score[0] = 2, team 0 scored 2 points.
        /// </summary>
        public SyncListInt score = new SyncListInt();

        /// <summary>
        /// The maximum amount of kills to reach before ending the game.
        /// </summary>
        public int maxScore = 30;

        /// <summary>
        /// The delay in seconds before respawning a player after it got killed.
        /// </summary>
        public int respawnTime = 5;
        
        
        //initialize variables
        void Awake()
        {
            instance = this;

            //if Unity Ads is enabled, hook up its result callback
            #if UNITY_ADS
                UnityAdsManager.adResultEvent += HandleAdResult;
            #endif
        }
        

        /// <summary>
        /// Returns a reference to this script instance.
        /// </summary>
        public static GameManager GetInstance()
        {
            return instance;
        }


        /// <summary>
        /// Global check whether this client is the match master or not.
        /// </summary>
		public static bool isMaster()
		{

			return GetInstance().isServer;
		}
        

        /// <summary>
        /// Server only: initialize SyncList length with team size once.
        /// Also verifies the team fill for each team in case of host migration.
        /// </summary>
        public override void OnStartServer()
        {
            //should execute only on the initial master
            if(size.Count != teams.Length)
            {
                for(int i = 0; i < teams.Length; i++)
                {
                    size.Add(0);
                    score.Add(0);
                }
            }
                       
            //update team fill in UI
            StartCoroutine(OnHostMigration());
        }

        
        //called for the first host and new host after migration, if any
        IEnumerator OnHostMigration()
        {
            //wait for the next frame because UNET needs time to initialize
            //not doing this could prevent a successful host migration execution
            yield return new WaitForEndOfFrame();
            
            //workaround for old host not decreasing its team size on game quit for itself
            //there is no way to get the old server player object in the migration manager,
            //so we'll have to compare the current active players to detect the team mismatch.
            //maybe some previous clients did not make the switch to the new host either and
            //accidentally disconnected too, which makes this good practice to update the UI
            int[] tempSize = new int[teams.Length];
            foreach(NetworkConnection conn in NetworkServer.connections)
            {
                //loop over each connection and get the controlling player object for it
                foreach(NetworkInstanceId netId in conn.clientOwnedObjects)
                {
                    GameObject obj = NetworkServer.FindLocalObject(netId);
                    if(obj == null) continue;
                    
                    Player netPlayer = obj.GetComponent<Player>();
                    if(netPlayer == null) continue;
                    
                    //with the Player found get its team index and increase its team fill
                    tempSize[netPlayer.teamIndex]++;
                }
            }
            
            //loop over team fill and update SyncList and UI with new values, where necessary
            for(int i = 0; i < tempSize.Length; i++)
            {                
                if(size[i] != tempSize[i])
                {
                    size[i] = tempSize[i];
                    ui.OnTeamSizeChanged(SyncListInt.Operation.OP_DIRTY, i);
                }
            }
        }


        /// <summary>
        /// On establishing a successful connection to the master and initializing client variables,
        /// update the game UI, i.e. team fill and scores, by looping over the received SyncLists.
        /// </summary>
        public override void OnStartClient()
        {        
            //double check whether the game joined is still running, otherwise return to the menu scene
            //this could happen when a connection has been made right before the game ended and stopped
            if(IsGameOver())
            {
                ui.Quit();
                return;
            }
                
            //these callbacks are not handled reliable by UNET, but we subscribe nonetheless
            //maybe some display updates are called twice then which isn't too bad
            size.Callback = ui.OnTeamSizeChanged;
            score.Callback = ui.OnTeamScoreChanged;
            //call the hooks manually for the first time, for each team
            for(int i = 0; i < teams.Length; i++) ui.OnTeamSizeChanged(SyncListInt.Operation.OP_DIRTY, i);
            for(int i = 0; i < teams.Length; i++) ui.OnTeamScoreChanged(SyncListInt.Operation.OP_DIRTY, i);
        }

        
        /// <summary>
        /// Returns the next team index a player should be assigned to.
        /// </summary>
        public int GetTeamFill()
        {
            //init variables
            int teamNo = 0;

            int min = size[0];
            //loop over teams to find the lowest fill
            for(int i = 0; i < teams.Length; i++)
            {
                //if fill is lower than the previous value
                //store new fill and team for next iteration
                if(size[i] < min)
                {
                    min = size[i];
                    teamNo = i;
                }
            }
            
            //return index of lowest team
            return teamNo;
        }
        
        
        /// <summary>
        /// Returns a random spawn position within the team's spawn area.
        /// </summary>
        public Vector3 GetSpawnPosition(int teamIndex)
        {
            //init variables
            Vector3 pos = teams[teamIndex].spawn.position;
            BoxCollider col = teams[teamIndex].spawn.GetComponent<BoxCollider>();

            if(col != null)
            {
                //find a position within the box collider range, first set fixed y position
                //the counter determines how often we are calculating a new position if out of range
                pos.y = col.transform.position.y;
                int counter = 10;
                
                //try to get random position within collider bounds
                //if it's not within bounds, do another iteration
                do
                {
                    pos.x = UnityEngine.Random.Range(col.bounds.min.x, col.bounds.max.x);
                    pos.z = UnityEngine.Random.Range(col.bounds.min.z, col.bounds.max.z);
                    counter--;
                }
                while(!col.bounds.Contains(pos) && counter > 0);
            }
            
            return pos;
        }


        //implements what to do when an ad view completes
        #if UNITY_ADS
        void HandleAdResult(ShowResult result)
        {
            switch (result)
            {
                //in case the player successfully watched an ad,
                //it sends a request for it be respawned
                case ShowResult.Finished:
                case ShowResult.Skipped:
                    localPlayer.CmdRespawn();
                    break;
                
                //in case the ad can't be shown, just handle it
                //like we wouldn't have tried showing a video ad
                //with the regular death countdown (force ad skip)
                case ShowResult.Failed:
                    DisplayDeath(true);
                    break;
            }
        }
        #endif


        /// <summary>
        /// Returns whether a team reached the maximum game score.
        /// </summary>
        public bool IsGameOver()
        {
            //init variables
            bool isOver = false;
            
            //loop over teams to find the highest score
            for(int i = 0; i < teams.Length; i++)
            {
                //score is greater or equal to max score,
                //which means the game is finished
                if(score[i] >= maxScore)
                {
                    isOver = true;
                    break;
                }
            }
            
            //return the result
            return isOver;
        }
        
        
        /// <summary>
        /// Only for this player: sets the death text stating the killer on death.
        /// If Unity Ads is enabled, tries to show an ad during the respawn delay.
        /// By using the 'skipAd' parameter is it possible to force skipping ads.
        /// </summary>
        public void DisplayDeath(bool skipAd = false)
        {   
            //get the player component that killed us
            Player other = localPlayer.killedBy.GetComponent<Player>();

            //calculate if we should show a video ad
            #if UNITY_ADS
            if (!skipAd && UnityAdsManager.ShowAd())
                return;
            #endif

            //when no ad is being shown, set the death text
            //and start waiting for the respawn delay immediately
            ui.SetDeathText(other.myName, teams[other.teamIndex]);
            StartCoroutine(SpawnRoutine());
        }
        
        
        //coroutine spawning the player after a respawn delay
        IEnumerator SpawnRoutine()
        {
            //calculate point in time for respawn
            float targetTime = Time.time + respawnTime;
           
            //wait for the respawn to be over,
            //while waiting update the respawn countdown
            while(targetTime - Time.time > 0)
            {
                ui.SetSpawnDelay(targetTime - Time.time);
                yield return null;
            }
            
            //respawn now: send request to the server
            ui.DisableDeath();
            localPlayer.CmdRespawn();
        }
        
        
        /// <summary>
        /// Only for this player: sets game over text stating the winning team.
        /// Disables player movement so no updates are sent through the network.
        /// </summary>
        public void DisplayGameOver(int teamIndex)
        {           
            localPlayer.enabled = false;
            localPlayer.camFollow.HideMask(true);
            ui.SetGameOverText(teams[teamIndex]);
            
            //starts coroutine for displaying the game over window
            StartCoroutine(DisplayGameOver());
        }
        
        
        //displays game over window after short delay
        IEnumerator DisplayGameOver()
        {
            //give the user a chance to read which team won the game
            //before enabling the game over screen
            yield return new WaitForSeconds(3);
            
            //show game over window and disconnect from network
            ui.ShowGameOver();
            NetworkManager.singleton.StopHost();
        }
        
        
        //clean up callbacks on scene switches
        void OnDestroy()
        {
            #if UNITY_ADS
                UnityAdsManager.adResultEvent -= HandleAdResult;
            #endif
        }
    }

    
    /// <summary>
    /// Defines properties of a team.
    /// </summary>
     [System.Serializable]
    public class Team
    {
        /// <summary>
        /// The name of the team shown on game over.
        /// </summary>
        public string name;
             
        /// <summary>
        /// The color of a team for UI and player prefabs.
        /// </summary>   
        public Material material;
            
        /// <summary>
        /// The spawn point of a team in the scene. In case it has a BoxCollider
        /// component attached, a point within the collider bounds will be used.
        /// </summary>
        public Transform spawn;
    }
}
