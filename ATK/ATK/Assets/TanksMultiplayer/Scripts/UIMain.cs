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
    /// UI script for all elements, settings and user interactions in the menu scene.
    /// </summary>
    public class UIMain : MonoBehaviour
    {
        /// <summary>
        /// Window object for loading screen between connecting and scene switch.
        /// </summary>
        public GameObject loadingWindow;

        /// <summary>
        /// Window object for displaying errors with the connection or timeouts.
        /// </summary>
        public GameObject connectionErrorWindow;
        
        /// <summary>
        /// Window object for displaying errors with the billing actions.
        /// </summary>
        public GameObject billingErrorWindow;
        
        /// <summary>
		/// UI button for opening the Everyplay hub, hooked into inspector event.
        /// This is automatically hidden on non-supported devices.
		/// </summary>
		public Button everyplayButton;

        /// <summary>
		/// Settings: input field for the player name.
		/// </summary>
		public InputField nameField;

		/// <summary>
		/// Settings: checkbox for playing background music.
		/// </summary>
		public Toggle musicToggle;

		/// <summary>
		/// Settings: slider for adjusting game sound volume.
		/// </summary>
		public Slider volumeSlider;
			
        /// <summary>
        /// Settings: checkbox for recording gameplay videos.
        /// This is automatically hidden on non-supported devices.
        /// </summary>
        public Toggle recordToggle;

		//how many times the shop has been opened
		//private int shopOpened = 0;

		//how many times the settings have been opened
		//private int settingsOpened = 0;
        

        //initialize player selection in Settings window
        //if this is the first time launching the game, set initial values
        void Start()
        {      
            //set initial values for all settings               
            if (!PlayerPrefs.HasKey(PrefsKeys.playerName))
                PlayerPrefs.SetString(PrefsKeys.playerName, "User" + System.String.Format("{0:0000}", Random.Range(1, 9999)));

            if (!PlayerPrefs.HasKey(PrefsKeys.playMusic))
                PlayerPrefs.SetString(PrefsKeys.playMusic, "true");

            if (!PlayerPrefs.HasKey(PrefsKeys.appVolume))
                PlayerPrefs.SetFloat(PrefsKeys.appVolume, 1f);

            if (!PlayerPrefs.HasKey(PrefsKeys.recordGame))
                PlayerPrefs.SetString(PrefsKeys.recordGame, "true");

            if (!PlayerPrefs.HasKey(PrefsKeys.activeTank))
                PlayerPrefs.SetString(PrefsKeys.activeTank, Encryptor.Encrypt("0"));

            PlayerPrefs.Save();
            
            //read the selections and set them in the corresponding UI elements
            nameField.text = PlayerPrefs.GetString(PrefsKeys.playerName);
            musicToggle.isOn = bool.Parse(PlayerPrefs.GetString(PrefsKeys.playMusic));
            volumeSlider.value = PlayerPrefs.GetFloat(PrefsKeys.appVolume);
            recordToggle.isOn = bool.Parse(PlayerPrefs.GetString(PrefsKeys.recordGame));

            //call the onValueChanged callbacks once with their saved values
            OnMusicChanged(musicToggle.isOn);
            OnVolumeChanged(volumeSlider.value);
            
            //listen to network connection and IAP billing errors
            NetworkManagerCustom.connectionFailedEvent += OnConnectionError;
            UnityIAPManager.purchaseFailedEvent += OnBillingError;
            
            //add Everyplay callback in case it changes later on,
            //but also call it immediately right when its initialization finishes
            UnityEveryplayManager.recordingSupportedEvent += OnEveryplaySupport;
            OnEveryplaySupport(UnityEveryplayManager.IsSupported());
        }


        /// <summary>
        /// Tries to enter the game scene. Sets the loading screen active while connecting to the
        /// Matchmaker and starts the timeout coroutine at the same time.
        /// </summary>
        public void Play()
        {
            //UnityAnalyticsManager.MainSceneClosed(shopOpened, settingsOpened, musicToggle.isOn,
            //                      Encryptor.Decrypt(PlayerPrefs.GetString(PrefsKeys.activeTank)));

            loadingWindow.SetActive(true);
            NetworkManagerCustom.StartMatch();
            StartCoroutine(HandleTimeout());
        }
        
        
        //coroutine that waits 10 seconds before cancelling joining a match
        IEnumerator HandleTimeout()
        {
            yield return new WaitForSeconds(10);
            
            //timeout has passed, we would like to stop joining a game now
            //networkSceneName has not been set yet for whatever reason, so this forces a scene reload
            //to work around that we set the networkSceneName to the current scene name manually
            NetworkManagerCustom.networkSceneName = SceneManager.GetActiveScene().name;
            NetworkManagerCustom.singleton.StopHost();
            
            //display connection issue window
            OnConnectionError();
        }


        //activates the connection error window to be visible
        void OnConnectionError()
        {
            StopCoroutine(HandleTimeout());
            loadingWindow.SetActive(false);
            connectionErrorWindow.SetActive(true);
        }
        
        
        //activates the billing error window to be visible
        void OnBillingError(string error)
        {
            //get text label to display billing failed reason
            Text errorLabel = billingErrorWindow.GetComponentInChildren<Text>();
            if(errorLabel)
                errorLabel.text = "Purchase failed.\n" + error;
            
            billingErrorWindow.SetActive(true);
        }


        /// <summary>
        /// Increase counter when opening the shop.
        /// Used for Unity Analytics purposes.
        /// </summary>
        public void OpenShop()
        {
            //shopOpened++;
        }


        /// <summary>
        /// Increase counter when opening settings.
        /// Used for Unity Analytics purposes.
        /// </summary>
        public void OpenSettings()
        {
            //settingsOpened++;
        }


        /// <summary>
        /// Modify music AudioSource based on player selection.
        /// Called by Toggle onValueChanged event.
        /// </summary>
        public void OnMusicChanged(bool value)
        {
			AudioManager.GetInstance().musicSource.enabled = musicToggle.isOn;
            AudioManager.PlayMusic(0);
        }


        /// <summary>
        /// Modify global game volume based on player selection.
        /// Called by Slider onValueChanged event.
        /// </summary>
        public void OnVolumeChanged(float value)
        {
            volumeSlider.value = value;
            AudioListener.volume = value;
        }
			

        /// <summary>
        /// Saves all player selections chosen in the Settings window on the device.
        /// </summary>
        public void CloseSettings()
        {
            PlayerPrefs.SetString(PrefsKeys.playerName, nameField.text);
            PlayerPrefs.SetString(PrefsKeys.playMusic, musicToggle.isOn.ToString());
            PlayerPrefs.SetFloat(PrefsKeys.appVolume, volumeSlider.value);
            PlayerPrefs.SetString(PrefsKeys.recordGame, recordToggle.isOn.ToString());
            PlayerPrefs.Save();
        }

			
		/// <summary>
        /// Sets the visibility of Everyplay related UI elements.
        /// Called by Everplay when initialization finishes.
        /// </summary>
		public void OnEveryplaySupport(bool supported)
		{
			everyplayButton.gameObject.SetActive(supported);
			recordToggle.gameObject.SetActive(supported);
		}
			
			
		/// <summary>
        /// Displays the Everyplay community hub using our manager.
        /// </summary>
		public void ShowEveryplay()
		{
			UnityEveryplayManager.ShowMenu();
		}

			
        /// <summary>
        /// Opens a browser window to the App Store entry for this app.
        /// </summary>
        public void RateApp()
        {
            //UnityAnalyticsManager.RateStart();
            
			#if UNITY_ANDROID
				Application.OpenURL("http://play.google.com/store/apps/details?id=" + Application.bundleIdentifier);
			#elif UNITY_IPHONE
				Application.OpenURL("http://itunes.com/apps/" + Application.identifier);
			#endif
        }
    }
}
