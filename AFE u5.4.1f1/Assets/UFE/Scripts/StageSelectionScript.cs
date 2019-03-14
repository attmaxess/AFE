using UnityEngine;
using System.Collections;

public class StageSelectionScript : MonoBehaviour {
	
	
	public GUISkin customSkin;
	public GUIStyle returnButtonStyle;
	public AudioClip selectSound;
	public AudioClip moveCursorSound;

	public ButtonPress selectButton = ButtonPress.Button1;
	public ButtonPress deselectButton = ButtonPress.Button4;
	
	public Texture2D p1Overlay;
	public Texture2D p2Overlay;

	private Rect returnButtonRect;
	private string horizontalAxis;
	private bool axisHeld;
	private int hoverIndex;

	private bool startingGame;
	
	void Start () {
		GUITexture[] guiTextures = GetComponentsInChildren<GUITexture>();
		foreach(GUITexture guiTexChild in guiTextures){
			guiTexChild.pixelInset = SetResolution(guiTexChild.pixelInset);
		}

		UFE.config.selectedStage = null;
		horizontalAxis = UFE.GetInputReference(InputType.HorizontalAxis, UFE.config.player1_Inputs);
		
		returnButtonRect = new Rect(10, 10, returnButtonStyle.normal.background.width, returnButtonStyle.normal.background.height);
		returnButtonRect = SetResolution(returnButtonRect);
		
	}
	
	int StageSelect(int selectedIndex){
		if (Input.GetAxisRaw(horizontalAxis) > 0){
			if (UFE.config.soundfx) Camera.main.GetComponent<AudioSource>().PlayOneShot(moveCursorSound);
			if (selectedIndex == UFE.config.stages.Length - 1){
				selectedIndex = 0;
			}else{
				selectedIndex += 1;
			}
		}else if (Input.GetAxisRaw(horizontalAxis) < 0){
			if (UFE.config.soundfx) Camera.main.GetComponent<AudioSource>().PlayOneShot(moveCursorSound);
			if (selectedIndex == 0){
				selectedIndex = UFE.config.stages.Length - 1;
			}else{
				selectedIndex -= 1;
			}
		}
		
		return selectedIndex;
	}
	
	void Update(){
		if (Input.GetAxisRaw(horizontalAxis) == 0) axisHeld = false;
		
		// Select Stage
		if (!axisHeld && UFE.config.selectedStage == null){
			hoverIndex = StageSelect(hoverIndex);
			
			if (Input.GetButtonDown(UFE.GetInputReference(selectButton, UFE.config.player1_Inputs)) ||
			    Input.GetKeyDown(KeyCode.Space) ||
			    Input.GetKeyDown(KeyCode.Return)){
				UFE.config.selectedStage = UFE.config.stages[hoverIndex];
				if (UFE.config.soundfx) Camera.main.GetComponent<AudioSource>().PlayOneShot(selectSound);
				Invoke("StartGame",1.2f);
				startingGame = true;
			}
		}
		
		if (Input.GetAxisRaw(horizontalAxis) != 0) axisHeld = true;
	}


	void StartGame(){
		UFE.StartGame(3);
	}

	void OnGUI(){
		GUI.skin = customSkin;

		if (startingGame) GUI.color = new Color(1,1,1,(Mathf.PingPong(Time.time * 15, 1))/ 2);
		GUI.DrawTexture(SetResolution(new Rect(472, 279, 336, 182)), UFE.config.stages[hoverIndex].screenshot);
		
		GUI.color = Color.white;
		
		if (UFE.config.player1Character.profilePictureBig != null) {
			GUI.DrawTexture(SetResolution(new Rect(79, 112, 311, 496)), UFE.config.player1Character.profilePictureBig);
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			GUI.skin.label.fontSize = 30 * (Screen.height/720);
			GUI.Label(SetResolution(new Rect(178, 597, 250, 50)), UFE.config.player1Character.characterName);
		}
		if (UFE.config.player2Character.profilePictureBig != null) {
			GUI.DrawTextureWithTexCoords(SetResolution(new Rect(902, 113, 311, 496)), UFE.config.player2Character.profilePictureBig, new Rect(0, 0, -1, 1));
			GUI.skin.label.alignment = TextAnchor.UpperRight;
			GUI.skin.label.fontSize = 30 * (Screen.height/720);
			GUI.Label(SetResolution(new Rect(860, 597, 250, 50)), UFE.config.player2Character.characterName);
		}
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		
		GUI.DrawTexture(SetResolution(new Rect(61, 554, p1Overlay.width, p1Overlay.height)), p1Overlay);
		GUI.DrawTexture(SetResolution(new Rect(1130, 554, p2Overlay.width, p2Overlay.height)), p2Overlay);
		
		if (GUI.Button(returnButtonRect, "", returnButtonStyle)) UFE.StartCharacterSelect(2);
	}

	Rect SetResolution(Rect rect){
		rect.x *= ((float)Screen.width/1280);
		rect.y *= ((float)Screen.height/720);
		rect.width *= ((float)Screen.width/1280);
		rect.height *= ((float)Screen.height/720);
		return rect;
	}
}
