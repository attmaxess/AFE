using UnityEngine;
using System.Collections;

public class CharacterSelectionScript : MonoBehaviour {

	
	public GUISkin customSkin;
	public GUIStyle returnButtonStyle;
	public AudioClip selectSound;
	public AudioClip moveCursorSound;

	public Rect charactersGrid;
	public int gridColumns = 4;
	public int gridRows = 2;
	
	public ButtonPress selectButton = ButtonPress.Button1;
	public ButtonPress deselectButton = ButtonPress.Button4;

	public GUIStyle controllerStyle;
	public GUIStyle p1KeyboardStyle;
	public GUIStyle p2KeyboardStyle;
	public Texture2D p1Hud;
	public Texture2D p2Hud;
	public Texture2D p1P2Hud;
	public Texture2D p1Overlay;
	public Texture2D p2Overlay;
	
	private bool p1KeyboardSelected = true;
	private bool p1ControllerSelected = true;
	private bool p2KeyboardSelected = true;
	private bool p2ControllerSelected = true;

	private string p1JoystickName;
	private string p2JoystickName;
	
	private string p1HorizontalAxis = "P1KeyboardHorizontal";
	private string p1VerticalAxis = "P1KeyboardVertical";
	private string p2HorizontalAxis = "P2KeyboardHorizontal";
	private string p2VerticalAxis = "P2KeyboardVertical";

	private bool p1AxisHeld;
	private bool p2AxisHeld;

	private int p1HoverIndex = 0;
	private int p2HoverIndex = 0;
	
	private Rect returnButtonRect;

	private bool startingStageSelect;

	void Start () {
		GUITexture[] guiTextures = GetComponentsInChildren<GUITexture>();
		foreach(GUITexture guiTexChild in guiTextures){
			guiTexChild.pixelInset = SetResolution(guiTexChild.pixelInset);
		}

		if (UFE.config.characters.Length >= 4) p2HoverIndex = 3;

		UFE.config.player1Character = null;
		UFE.config.player2Character = null;

		returnButtonRect = new Rect(10, 10, returnButtonStyle.normal.background.width, returnButtonStyle.normal.background.height);
		returnButtonRect = SetResolution(returnButtonRect);
	}

	int CharacterMenuSelection(int selectedIndex, string horizontalAxis, string verticalAxis){
		if (Input.GetAxisRaw(horizontalAxis) > 0){
			if (UFE.config.soundfx) Camera.main.GetComponent<AudioSource>().PlayOneShot(moveCursorSound);
			if (selectedIndex == UFE.config.characters.Length - 1){
				selectedIndex = 0;
			}else{
				selectedIndex += 1;
			}
		}else if (Input.GetAxisRaw(horizontalAxis) < 0){
			if (UFE.config.soundfx) Camera.main.GetComponent<AudioSource>().PlayOneShot(moveCursorSound);
			if (selectedIndex == 0){
				selectedIndex = UFE.config.characters.Length - 1;
			}else{
				selectedIndex -= 1;
			}
		}

		if (Input.GetAxisRaw(verticalAxis) < 0){
			if (UFE.config.soundfx) Camera.main.GetComponent<AudioSource>().PlayOneShot(moveCursorSound);
			if (selectedIndex <= 3){
				selectedIndex += 4;
			}
		}else if (Input.GetAxisRaw(verticalAxis) > 0){
			if (UFE.config.soundfx) Camera.main.GetComponent<AudioSource>().PlayOneShot(moveCursorSound);
			if (selectedIndex > 3){
				selectedIndex -= 4;
			}
		}

		return selectedIndex;
	}
	
	void StartStageSelect(){
		UFE.StartStageSelect(0);
	}

	void Update(){
		if (Input.GetAxisRaw(p1HorizontalAxis) == 0 && Input.GetAxisRaw(p1VerticalAxis) == 0) p1AxisHeld = false;
		if (Input.GetAxisRaw(p2HorizontalAxis) == 0 && Input.GetAxisRaw(p2VerticalAxis) == 0) p2AxisHeld = false;

		// Select Character
		if (!p1AxisHeld && UFE.config.player1Character == null){
			p1HoverIndex = CharacterMenuSelection(p1HoverIndex, p1HorizontalAxis, p1VerticalAxis);

			if (Input.GetButtonDown(UFE.GetInputReference(selectButton, UFE.config.player1_Inputs))){
				if (UFE.config.soundfx) Camera.main.GetComponent<AudioSource>().PlayOneShot(selectSound);
				UFE.config.player1Character = UFE.config.characters[p1HoverIndex];
			}
		}

		if (!p2AxisHeld && UFE.config.player2Character == null){
			p2HoverIndex = CharacterMenuSelection(p2HoverIndex, p2HorizontalAxis, p2VerticalAxis);

			//Debug.Log(selectButton.ToString() +" = "+ Input.GetButtonDown(UFE.GetInputReference(selectButton, UFE.config.player2_Inputs)));
			if (Input.GetButtonDown(UFE.GetInputReference(selectButton, UFE.config.player2_Inputs))){
				if (UFE.config.soundfx) Camera.main.GetComponent<AudioSource>().PlayOneShot(selectSound);
				UFE.config.player2Character = UFE.config.characters[p2HoverIndex];
			}
		}



		// Both selected
		if (UFE.config.player1Character != null && UFE.config.player2Character != null){
			startingStageSelect = true;
			Invoke ("StartStageSelect", .8f);
		}


		// Deselect Character
		if (!startingStageSelect && Input.GetButtonDown(UFE.GetInputReference(deselectButton, UFE.config.player1_Inputs))) 
			UFE.config.player1Character = null;
		
		if (!startingStageSelect && Input.GetButtonDown(UFE.GetInputReference(deselectButton, UFE.config.player2_Inputs))) 
			UFE.config.player2Character = null;


		if (Input.GetAxisRaw(p1HorizontalAxis) != 0 || Input.GetAxisRaw(p1VerticalAxis) != 0) p1AxisHeld = true;
		if (Input.GetAxisRaw(p2HorizontalAxis) != 0 || Input.GetAxisRaw(p2VerticalAxis) != 0) p2AxisHeld = true;
	}

	void OnGUI(){
		GUI.skin = customSkin;
		p1JoystickName = "";
		p2JoystickName = "";
		string[] joysticks = Input.GetJoystickNames();
		foreach(string joystickName in joysticks){
			if (joystickName != p2JoystickName) p1JoystickName = joystickName;
			if (joystickName != p1JoystickName) p2JoystickName = joystickName;
		}

		p1KeyboardSelected = GUI.Toggle(SetResolution(new Rect(412, 176, 202, 59)), p1KeyboardSelected,  "", p1KeyboardStyle);
		if (p1KeyboardSelected) {
			p1ControllerSelected = false;
			foreach (InputReferences inputRef in UFE.config.player1_Inputs){
				if (inputRef.inputType == InputType.HorizontalAxis){
					inputRef.inputButtonName = "P1KeyboardHorizontal";
					p1HorizontalAxis = inputRef.inputButtonName;
				}else if (inputRef.inputType == InputType.VerticalAxis){
					inputRef.inputButtonName = "P1KeyboardVertical";
					p1VerticalAxis = inputRef.inputButtonName;
				}
			}
		}
		if (!p1KeyboardSelected && !p1ControllerSelected) p1KeyboardSelected = true;

		if (p1JoystickName == "") GUI.enabled = false;
		p1ControllerSelected = GUI.Toggle(SetResolution(new Rect(421, 324, 177, 116)), p1ControllerSelected,  "", controllerStyle);
		if (p1ControllerSelected && p1KeyboardSelected) {
			p1KeyboardSelected = false;
			foreach (InputReferences inputRef in UFE.config.player1_Inputs){
				if (inputRef.inputType == InputType.HorizontalAxis){
					inputRef.inputButtonName = "P1JoystickHorizontal";
					p1HorizontalAxis = inputRef.inputButtonName;
				}else if (inputRef.inputType == InputType.VerticalAxis){
					inputRef.inputButtonName = "P1JoystickVertical";
					p1VerticalAxis = inputRef.inputButtonName;
				}
			}
		}
		GUI.enabled = true;

		p2KeyboardSelected = GUI.Toggle(SetResolution(new Rect(677, 176, 202, 59)), p2KeyboardSelected,  "", p2KeyboardStyle);
		if (p2KeyboardSelected && p2ControllerSelected) {
			p2ControllerSelected = false;
			foreach (InputReferences inputRef in UFE.config.player2_Inputs){
				if (inputRef.inputType == InputType.HorizontalAxis){
					inputRef.inputButtonName = "P2KeyboardHorizontal";
					p2HorizontalAxis = inputRef.inputButtonName;
				}else if (inputRef.inputType == InputType.VerticalAxis){
					inputRef.inputButtonName = "P2KeyboardVertical";
					p2VerticalAxis = inputRef.inputButtonName;
				}
			}
		}
		if (!p2KeyboardSelected && !p2ControllerSelected) p2KeyboardSelected = true;

		if (p2JoystickName == "") GUI.enabled = false;
		p2ControllerSelected = GUI.Toggle(SetResolution(new Rect(691, 324, 177, 116)), p2ControllerSelected,  "", controllerStyle);
		if (p2ControllerSelected && p2KeyboardSelected) {
			p2KeyboardSelected = false;
			foreach (InputReferences inputRef in UFE.config.player1_Inputs){
				if (inputRef.inputType == InputType.HorizontalAxis){
					inputRef.inputButtonName = "P2JoystickHorizontal";
					p2HorizontalAxis = inputRef.inputButtonName;
				}else if (inputRef.inputType == InputType.VerticalAxis){
					inputRef.inputButtonName = "P2JoystickVertical";
					p2VerticalAxis = inputRef.inputButtonName;
				}
			}
		}
		GUI.enabled = true;


		Texture2D p1SelectedBig = null;
		Texture2D p2SelectedBig = null;
		
		//GUI.BeginGroup(SetResolution(new Rect(457, 465, 378, 220)));{
		GUI.BeginGroup(SetResolution(charactersGrid));{
			int xCount = 0;
			int yCount = 0;
			int currentIndex = 0;
			foreach(CharacterInfo character in UFE.config.characters){
				float xPos = 96 * xCount;
				float yPos = 114 * yCount;

				xCount ++;
				if (xCount == gridColumns){
					yCount ++;
					xCount = 0;
				}
				GUI.DrawTexture(SetResolution(new Rect(xPos, yPos, 86, 105)), character.profilePictureSmall);
				//GUI.DrawTextureWithTexCoords(SetResolution(new Rect(xPos, yPos, 86, 105)), character.profilePictureSmall, new Rect(0, 0, 1, 1));
				//if (GUI.Button(SetResolution(new Rect(xPos, yPos, 86, 105)), character.profilePictureSmall)) {
					// .
				//}

				//if (UFE.config.player1Character != null)
				
				//GUI.color = new Color(1,1,1,(Mathf.Sin(Time.time * 20) + 1)/ 2);

				if (p1HoverIndex == currentIndex && p1HoverIndex == p2HoverIndex){
					bool selTemp = (UFE.config.player1Character != null && UFE.config.player1Character != null)? true : false;
					DrawHud(new Rect(xPos, yPos, 86, 105), p1P2Hud, selTemp);
					//GUI.DrawTexture(SetResolution(new Rect(xPos, yPos, 86, 105)), p1P2Hud);
					p1SelectedBig = character.profilePictureBig;
					p2SelectedBig = character.profilePictureBig;
				}else{
					if (p1HoverIndex == currentIndex){
						DrawHud(new Rect(xPos, yPos, 86, 105), p1Hud, (UFE.config.player1Character != null));
						//GUI.DrawTexture(SetResolution(new Rect(xPos, yPos, 86, 105)), p1Hud);
						p1SelectedBig = character.profilePictureBig;
					}
					
					if (p2HoverIndex == currentIndex){
						DrawHud(new Rect(xPos, yPos, 86, 105), p2Hud, (UFE.config.player2Character != null));
						//GUI.DrawTexture(SetResolution(new Rect(xPos, yPos, 86, 105)), p2Hud);
						p2SelectedBig = character.profilePictureBig;
					}
				}

				//GUI.color = Color.white;

				currentIndex ++;

			}
		}GUI.EndGroup();
		
		
		if (p1SelectedBig != null) {
			GUI.DrawTexture(SetResolution(new Rect(79, 112, 311, 496)), p1SelectedBig);
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			GUI.skin.label.fontSize = 30 * (Screen.height/720);
			GUI.Label(SetResolution(new Rect(178, 597, 250, 50)), UFE.config.characters[p1HoverIndex].characterName);
		}
		if (p2SelectedBig != null) {
			GUI.DrawTextureWithTexCoords(SetResolution(new Rect(902, 113, 311, 496)), p2SelectedBig, new Rect(0, 0, -1, 1));
			GUI.skin.label.alignment = TextAnchor.UpperRight;
			GUI.skin.label.fontSize = 30 * (Screen.height/720);
			GUI.Label(SetResolution(new Rect(860, 597, 250, 50)), UFE.config.characters[p2HoverIndex].characterName);
		}
		GUI.skin.label.alignment = TextAnchor.UpperLeft;

		GUI.DrawTexture(SetResolution(new Rect(61, 554, p1Overlay.width, p1Overlay.height)), p1Overlay);
		GUI.DrawTexture(SetResolution(new Rect(1130, 554, p2Overlay.width, p2Overlay.height)), p2Overlay);

		if (GUI.Button(returnButtonRect, "", returnButtonStyle)) UFE.StartIntro(2);
	}

	void DrawHud(Rect rect, Texture2D hud, bool selected){
		if (!selected) GUI.color = new Color(1,1,1,(Mathf.Sin(Time.time * 15) + 1)/ 2);
		GUI.DrawTexture(SetResolution(rect), hud);
		GUI.color = Color.white;
	}

	Rect SetResolution(Rect rect){
		rect.x *= ((float)Screen.width/1280);
		rect.y *= ((float)Screen.height/720);
		rect.width *= ((float)Screen.width/1280);
		rect.height *= ((float)Screen.height/720);
		return rect;
	}
}
