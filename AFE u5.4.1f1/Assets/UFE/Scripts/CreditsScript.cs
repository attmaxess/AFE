using UnityEngine;
using System.Collections;


public class CreditsScript : MonoBehaviour {

	public GUIStyle backButtonStyle;
	private Rect backButtonRect;

	void Start () {
		Rect newPixelInset = GetComponent<GUITexture>().pixelInset;
		newPixelInset.width *= ((float)Screen.width/1280);
		newPixelInset.height *= ((float)Screen.height/720);
		GetComponent<GUITexture>().pixelInset = newPixelInset;

		backButtonRect = new Rect(0, 0, backButtonStyle.normal.background.width, backButtonStyle.normal.background.height);
		backButtonRect = SetResolution(backButtonRect);

	}

	void OnGUI(){
		if (GUI.Button(backButtonRect, "", backButtonStyle)) UFE.StartIntro(2);
	}

	Rect SetResolution(Rect rect){
		rect.width *= ((float)Screen.width/1280);
		rect.height *= ((float)Screen.height/720);
		rect.x = ((float)Screen.width/2) - (rect.width/2);
		rect.y = Screen.height - (100 * ((float)Screen.height/720));
		return rect;
	}
}
