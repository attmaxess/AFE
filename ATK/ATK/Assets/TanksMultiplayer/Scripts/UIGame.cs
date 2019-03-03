/*  This file is part of the "Tanks Multiplayer" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TanksMP
{
    /// <summary>
    /// UI script for all elements, team events and user interactions in the game scene.
    /// </summary>
    public class UIGame : MonoBehaviour
    {
        /// <summary>
        /// Joystick components controlling player movement and actions on mobile devices.
        /// </summary>
        public UIJoystick[] controls;
        
        /// <summary>
        /// UI sliders displaying team fill for each team using absolute values.
        /// </summary>
        public Slider[] teamSize;
        
        /// <summary>
        /// UI texts displaying kill scores for each team.
        /// </summary>
        public Text[] teamScore;
        
        /// <summary>
        /// UI text for indicating player death and who killed this player.
        /// </summary>
        public Text deathText;
        
        /// <summary>
        /// UI text displaying the time in seconds left until player respawn.
        /// </summary>
        public Text spawnDelayText;

		/// <summary>
		/// Reference to the sprite used by Everyplay to display a video thumbnail.
        /// This is only used on devices supporting recording and stays empty otherwise.
		/// </summary>
		public Image thumbnailImage;

        /// <summary>
        /// UI text for indicating game end and which team has won the round.
        /// </summary>
        public Text gameOverText;

        /// <summary>
        /// UI window gameobject activated on game end, offering sharing and restart buttons.
        /// </summary>
        public GameObject gameOverMenu;


        //initialize variables
        IEnumerator Start()
        {
            //on non-mobile devices hide joystick controls, except in editor
            #if !UNITY_EDITOR && (UNITY_STANDALONE || UNITY_WEBGL)
                ToggleControls(false);
            #endif
            
            //play background music
            AudioManager.PlayMusic(1);
            //don't continue Everyplay initialization on non-supported devices
            if(!UnityEveryplayManager.IsRecordingSupported())
                yield break;
                
            //set thumbnail used by Everyplay to our image reference, start recording and
            //immediately take a snapshot to be displayed as thumbnail after a short delay
            //this is because Unity Everyplay seems to need some time to initialize properly
			UnityEveryplayManager.InitializeThumbnail(thumbnailImage);
            UnityEveryplayManager.StartRecord();
			yield return new WaitForSeconds(0.5f);
			UnityEveryplayManager.TakeThumbnail();
        }
        

        /// <summary>
        /// Method called by the SyncList operation over the Network when its content changes.
        /// This is an implementation for changes to the team fill, updating the slider values.
        /// Parameters: type of operation, index of team which received updates.
        /// </summary>
        public void OnTeamSizeChanged(UnityEngine.Networking.SyncListInt.Operation op, int index)
        {
            teamSize[index].value = GameManager.GetInstance().size[index];
        }
        

        /// <summary>
        /// Method called by the SyncList operation over the Network when its content changes.
        /// This is an implementation for changes to the team score, updating the text values.
        /// Parameters: type of operation, index of team which received updates.
        /// </summary>
        public void OnTeamScoreChanged(UnityEngine.Networking.SyncListInt.Operation op, int index)
        {
            teamScore[index].text = GameManager.GetInstance().score[index].ToString();
            teamScore[index].GetComponent<Animator>().Play("Animation");
        }

        
        /// <summary>
        /// Enables or disables visibility of joystick controls.
        /// </summary>
        public void ToggleControls(bool state)
        {
            for(int i = 0; i < controls.Length; i++)
                controls[i].gameObject.SetActive(state);
        }

        
        /// <summary>
        /// Sets death text showing who killed the player in its team color.
        /// Parameters: killer's name, killer's team
        /// </summary>
        public void SetDeathText(string playerName, Team team)
        {
            //hide joystick controls while displaying death text
            #if UNITY_EDITOR || (!UNITY_STANDALONE && !UNITY_WEBGL)
                ToggleControls(false);
            #endif
            
            //show killer name and colorize the name converting its team color to an HTML RGB hex value for UI markup
            deathText.text = "KILLED BY\n<color=#" + ColorUtility.ToHtmlStringRGB(team.material.color) + ">" + playerName + "</color>";
        }
        
        
        /// <summary>
        /// Set respawn delay value displayed to the absolute time value received.
        /// The remaining time value is calculated in a coroutine by GameManager.
        /// </summary>
        public void SetSpawnDelay(float time)
        {                
            spawnDelayText.text = Mathf.Ceil(time) + "";
        }
        
        
        /// <summary>
        /// Hides any UI components related to player death after respawn.
        /// </summary>
        public void DisableDeath()
        {
            //show joystick controls after disabling death text
            #if UNITY_EDITOR || (!UNITY_STANDALONE && !UNITY_WEBGL)
                ToggleControls(true);
            #endif
            
            //clear text component values
            deathText.text = string.Empty;
            spawnDelayText.text = string.Empty;
        }


        /// <summary>
        /// Set game end text and display winning team in its team color.
        /// </summary>
        public void SetGameOverText(Team team)
        {
            //hide joystick controls while displaying game end text
            #if UNITY_EDITOR || (!UNITY_STANDALONE && !UNITY_WEBGL)
                ToggleControls(false);
            #endif
            
            //show winning team and colorize it by converting the team color to an HTML RGB hex value for UI markup
            gameOverText.text = "TEAM <color=#" + ColorUtility.ToHtmlStringRGB(team.material.color) + ">" + team.name + "</color> WINS!";
        }


        /// <summary>
        /// Displays the game's end screen. Called by GameManager after few seconds delay.
        /// Stops Everplay recording and tries to display a video ad, if not shown already.
        /// </summary>
        public void ShowGameOver()
        {
            UnityEveryplayManager.StopRecord();
                        
            //hide text but enable game over window
            gameOverText.gameObject.SetActive(false);
            gameOverMenu.SetActive(true);
            
            //check whether an ad was shown during the game
            //if no ad was shown during the whole round, we request one here
            #if UNITY_ADS
            if(!UnityAdsManager.didShowAd())
                UnityAdsManager.ShowAd(true);
            #endif
        }


        /// <summary>
        /// Returns to the starting scene and immediately requests another game session.
        /// In the starting scene we have the loading screen and disconnect handling set up already,
        /// so this saves us additional work of doing the same logic twice in the game scene. The
        /// restart request is implemented in another gameobject that lives throughout scene changes.
        /// </summary>
        public void Restart()
        {
            GameObject gObj = new GameObject("RestartNow");
            gObj.AddComponent<UIRestartButton>();
            DontDestroyOnLoad(gObj);
            
            Quit();
        }


        /// <summary>
        /// Shares the recorded gameplay video using Everyplay's sharing dialog.
        /// </summary>
        public void Share()
        {
            UnityEveryplayManager.Share();
        }


        /// <summary>
        /// Stops receiving further network updates which leads to loading the starting scene.
        /// </summary>
        public void Quit()
        {	
			SceneManager.LoadScene(0);
        }
    }
}
