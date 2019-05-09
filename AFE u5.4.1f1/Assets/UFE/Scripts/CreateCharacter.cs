using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCharacter : MonoBehaviour
{
    public static ControlsScript p1ControlsScript = null;
    public static ControlsScript p2ControlsScript = null;
    public static GlobalInfo config;

    [Header("UFEDoCreate")]
    public static bool sdoneUFEDoCreate = true;

    [ContextMenu("UFEDoCreate")]
    public void UFEDoCreate()
    {
        StartCoroutine(_UFEDoCreate());
    }

    public static IEnumerator _UFEDoCreate()
    {
        sdoneUFEDoCreate = false;

        config = UFE.config;

        UFE.gameMode = GameMode.VersusMode;        
        UFE.SetCPU(1, false);
        UFE.SetCPU(2, false);

        if (UFE.config.gameGUI.battleGUI == null)
        {
            Debug.LogError("Battle GUI not found! Make sure you have set the prefab correctly in the Global Editor");
            UFE.battleGUI = new GameObject("BattleGUI").AddComponent<UFEScreen>();
        }
        else
        {
            UFE.battleGUI = (UFEScreen)GameObject.Instantiate(UFE.config.gameGUI.battleGUI);
        }
        UFE.battleGUI.transform.SetParent(UFE.canvas != null ? UFE.canvas.transform : null, false);
        UFE.battleGUI.OnShow();
        UFE.canvasGroup.alpha = 0;

        GameObject gameEngine = new GameObject("Game");
        UFE.gameEngine = gameEngine;
        UFE.cameraScript = gameEngine.AddComponent<CameraScript>();

        GameObject stageInstance = null;

        UFE.SetStage("AFERoom");

        if (UFE.config.stagePrefabStorage == StorageMode.Legacy)
        {
            if (UFE.config.selectedStage.prefab != null)
            {
                stageInstance = (GameObject)Instantiate(config.selectedStage.prefab);
                stageInstance.transform.parent = gameEngine.transform;
            }
            else
            {
                Debug.LogError("Stage prefab not found! Make sure you have set the prefab correctly in the Global Editor.");
            }
        }
        else
        {
#if !UFE_BASIC
            GameObject prefab = Resources.Load<GameObject>(config.selectedStage.stageResourcePath);

            if (prefab != null)
            {
                stageInstance = (GameObject)GameObject.Instantiate(prefab);
                stageInstance.transform.parent = gameEngine.transform;
            }
            else
            {
                Debug.LogError("Stage prefab not found! Make sure the prefab is correctly located under the Resources folder and the path is written correctly.");
            }
#endif
        }

        CharacterInfo[] charInfos = UFE.GetVersusModeSelectableCharacters();
        CharacterInfo p1Info = charInfos[5];
        CharacterInfo p2Info = charInfos[8];

        UFE.SetPlayer1(p1Info);
        UFE.SetPlayer2(p2Info);

        GameObject p1 = new GameObject("Player1");
        p1.transform.parent = gameEngine.transform;
        p1ControlsScript = p1.AddComponent<ControlsScript>();
        p1.AddComponent<PhysicsScript>();
        p1ControlsScript.myInfo = (CharacterInfo)Instantiate(p1Info);
        UFE.config.player1Character = p1ControlsScript.myInfo;
        UFE.p1ControlsScript = p1ControlsScript;
        p1ControlsScript.myInfo.playerNum = 1;

        GameObject p2 = new GameObject("Player2");
        p2.transform.parent = gameEngine.transform;
        p2ControlsScript = p2.AddComponent<ControlsScript>();
        p2.AddComponent<PhysicsScript>();
        p2ControlsScript.myInfo = (CharacterInfo)Instantiate(p2Info);
        UFE.config.player2Character = p2ControlsScript.myInfo;
        UFE.p2ControlsScript = p2ControlsScript;
        p2ControlsScript.myInfo.playerNum = 2;

        p1ControlsScript.opControlsScript = p2ControlsScript;
        p2ControlsScript.opControlsScript = p1ControlsScript;

        UFE.cameraScript.player1 = p1.transform;
        UFE.cameraScript.player2 = p2.transform;

        if (UFE.config.player1Character.name == UFE.config.player2Character.name)
        {
            if (UFE.config.player2Character.alternativeCostumes.Length > 0)
            {
                UFE.config.player2Character.isAlt = true;
                UFE.config.player2Character.selectedCostume = 0;
                p2ControlsScript.myInfo.characterPrefab = UFE.config.player2Character.alternativeCostumes[0].prefab;
            }
        }

        if (UFE.config.aiOptions.engine == AIEngine.FuzzyAI)
        {
            UFE.SetFuzzyAI(1, UFE.config.player1Character);
            UFE.SetFuzzyAI(2, UFE.config.player2Character);
        }
        else
        {
            UFE.SetRandomAI(1);
            UFE.SetRandomAI(2);
        }

        UFE.config.player1Character.currentLifePoints = (float)UFE.config.player1Character.lifePoints;
        UFE.config.player2Character.currentLifePoints = (float)UFE.config.player2Character.lifePoints;
        UFE.config.player1Character.currentGaugePoints = 0;
        UFE.config.player2Character.currentGaugePoints = 0;        

        UFE.config.currentRound = 1;
        UFE.config.lockInputs = true;
        UFE.SetTimer(config.roundOptions.timer);
        UFE.PauseTimer();        

        UFE.PauseGame(false);

        sdoneUFEDoCreate = true;

        yield break;
    }
}
