using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCharacter : MonoBehaviour
{
    public static ControlsScript p1ControlsScript = null;
    public static ControlsScript p2ControlsScript = null;

    [ContextMenu("UFEDoCreate")]
    public void UFEDoCreate()
    {
        _UFEDoCreate(this.transform);
    }

    public static void _UFEDoCreate(Transform parent)
    {
        CharacterInfo[] charInfos = UFE.GetVersusModeSelectableCharacters();
        CharacterInfo p1Info = charInfos[1];
        CharacterInfo p2Info = charInfos[3];

        GameObject p1 = new GameObject("Player1");
        p1.transform.parent = parent;
        p1ControlsScript = p1.AddComponent<ControlsScript>();
        p1.AddComponent<PhysicsScript>();
        p1ControlsScript.myInfo = (CharacterInfo)Instantiate(p1Info);
        UFE.config.player1Character = p1ControlsScript.myInfo;
        p1ControlsScript.myInfo.playerNum = 1;

        GameObject p2 = new GameObject("Player2");
        p2.transform.parent = parent;
        p2ControlsScript = p2.AddComponent<ControlsScript>();
        p2.AddComponent<PhysicsScript>();
        p2ControlsScript.myInfo = (CharacterInfo)Instantiate(p2Info);
        UFE.config.player2Character = p2ControlsScript.myInfo;
        p2ControlsScript.myInfo.playerNum = 2;

        if (UFE.config.player1Character.name == UFE.config.player2Character.name)
        {
            if (UFE.config.player2Character.alternativeCostumes.Length > 0)
            {
                UFE.config.player2Character.isAlt = true;
                UFE.config.player2Character.selectedCostume = 0;
                p2ControlsScript.myInfo.characterPrefab = UFE.config.player2Character.alternativeCostumes[0].prefab;
            }
        }

        UFE.PauseGame(false);
    }
}
