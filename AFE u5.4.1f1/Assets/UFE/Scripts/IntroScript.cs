using UnityEngine;
using System.Collections;


public class IntroScript : MonoBehaviour {

	public GUIStyle startButtonStyle;
	public GUIStyle optionsButtonStyle;
	public GUIStyle creditsButtonStyle;
	public AudioClip selectSound;

	public GUISkin customSkin;

	private Rect startButtonRect;
	private Rect optionsButtonRect;
	private Rect creditsButtonRect;

	private bool optionsIsOpen;
	private bool startingCharacterSelect;

	void Start () {
		UFE.SetLanguage("English");

		Rect newPixelInset = GetComponent<GUITexture>().pixelInset;
		newPixelInset.width *= ((float)Screen.width/1280);
		newPixelInset.height *= ((float)Screen.height/720);
		GetComponent<GUITexture>().pixelInset = newPixelInset;

		startButtonRect = new Rect(0, 0, startButtonStyle.normal.background.width, startButtonStyle.normal.background.height);
		optionsButtonRect = new Rect(0, 0, optionsButtonStyle.normal.background.width, optionsButtonStyle.normal.background.height);
		creditsButtonRect = new Rect(0, 0, creditsButtonStyle.normal.background.width, creditsButtonStyle.normal.background.height);

		startButtonRect = SetResolution(startButtonRect, 260);
		optionsButtonRect = SetResolution(optionsButtonRect, 180);
		creditsButtonRect = SetResolution(creditsButtonRect, 100);

		/*startButtonRect.width *= ((float)Screen.width/1280);
		startButtonRect.height *= ((float)Screen.height/720);
		startButtonRect.x = ((float)Screen.width/2) - (startButtonRect.width/2);
		startButtonRect.y = Screen.height - (260 * ((float)Screen.height/720));

		optionsButtonRect.width *= ((float)Screen.width/1280);
		optionsButtonRect.height *= ((float)Screen.height/720);
		optionsButtonRect.x = ((float)Screen.width/2) - (optionsButtonRect.width/2);
		optionsButtonRect.y = Screen.height - (180 * ((float)Screen.height/720));

		creditsButtonRect.width *= ((float)Screen.width/1280);
		creditsButtonRect.height *= ((float)Screen.height/720);
		creditsButtonRect.x = ((float)Screen.width/2) - (creditsButtonRect.width/2);
		creditsButtonRect.y = Screen.height - (100 * ((float)Screen.height/720));*/
	}

	// Automatically adjust the GUI resolution to 16:9
	Rect SetResolution(Rect rect, float yPos){
		rect.width *= ((float)Screen.width/1280);
		rect.height *= ((float)Screen.height/720);
		rect.x = ((float)Screen.width/2) - (rect.width/2);
		rect.y = Screen.height - (yPos * ((float)Screen.height/720));
		return rect;
	}

	// Order UFE to close the Intro prefab and start CharacterSelect prefab
	void StartCharacterSelect(){
		UFE.StartCharacterSelect(2);
	}

	void OnGUI(){
		// Small GUI code to make the 3 option menu
		GUI.skin = customSkin;

		if (optionsIsOpen) GUI.enabled = false;
		
		if (startingCharacterSelect) GUI.color = new Color(1,1,1,(Mathf.PingPong(Time.time * 15, 1))/ 2);
		if (GUI.Button(startButtonRect, "", startButtonStyle) && !startingCharacterSelect) {
			if (UFE.config.soundfx) Camera.main.GetComponent<AudioSource>().PlayOneShot(selectSound);
			Invoke("StartCharacterSelect",.5f);
			startingCharacterSelect = true;
		}
		GUI.color = Color.white;

		if (GUI.Button(optionsButtonRect, "", optionsButtonStyle) && !startingCharacterSelect) {
			if (UFE.config.soundfx) Camera.main.GetComponent<AudioSource>().PlayOneShot(selectSound);
			optionsIsOpen = true;
		}

		if (GUI.Button(creditsButtonRect, "", creditsButtonStyle) && !startingCharacterSelect) {
			if (UFE.config.soundfx) Camera.main.GetComponent<AudioSource>().PlayOneShot(selectSound);
			UFE.StartCreditsScreen(2);
		}
		GUI.enabled = true;

		if (optionsIsOpen){
			GUI.BeginGroup(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 130, 400, 220));{
				GUI.Box(new Rect(0, 0, 400, 220), "Options");
				GUI.BeginGroup(new Rect(15, 0, 380, 220));{
					GUILayoutUtility.GetRect(1,45);

					GUILayout.BeginHorizontal();{
						GUILayout.Label("Music", GUILayout.Width(240));
						if (UFE.GetMusic()){
							if (GUILayout.Button("On", GUILayout.Width(120))) UFE.SetMusic(false);
						}else{
							if (GUILayout.Button("Off", GUILayout.Width(120))) UFE.SetMusic(true);
						}
					}GUILayout.EndHorizontal();
					
					if (UFE.GetMusic()){
						GUILayout.BeginHorizontal();{
							GUILayout.Label("Music Volume", GUILayout.Width(240));
							UFE.SetVolume(GUILayout.HorizontalSlider(UFE.GetVolume(), 0, 1, GUILayout.Width(120)));
						}GUILayout.EndHorizontal();
					}else{
						GUILayoutUtility.GetRect(1,34);
					}

					GUILayout.BeginHorizontal();{
						GUILayout.Label("Sound FX", GUILayout.Width(240));
						if (UFE.GetSoundFX()){
							if (GUILayout.Button("On", GUILayout.Width(120))) UFE.SetSoundFX(false);
						}else{
							if (GUILayout.Button("Off", GUILayout.Width(120))) UFE.SetSoundFX(true);
						}
					}GUILayout.EndHorizontal();

					GUILayoutUtility.GetRect(1,40);
					GUILayout.BeginHorizontal();{
						GUILayout.FlexibleSpace();
						if (GUILayout.Button("Close")) optionsIsOpen = false;
						GUILayout.FlexibleSpace();
					}GUILayout.EndHorizontal();
					
				}GUI.EndGroup();

			}GUI.EndGroup();
		}

	}
}
