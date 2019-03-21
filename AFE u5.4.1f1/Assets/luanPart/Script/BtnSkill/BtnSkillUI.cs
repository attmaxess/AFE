using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using ControlFreak2;
using UniRx;
using UniRx.Triggers;
using TMPro;

public struct MessagePlayerData
{
    public int hp;
    public int isAttack;

    public MessagePlayerData(int hp, int isAttack)
    {
        this.hp = hp;
        this.isAttack = isAttack;
    }
}

public class BtnSkillUI : MonoBehaviour
{
    public Dictionary<BtnState, BtnSkillBase> btnSkillBase = new Dictionary<BtnState, BtnSkillBase>();
    BtnSkillBase curState;

    public bool canUseSkill;
    public bool isCountTime;
    public TextMeshProUGUI number;

    private void Start()
    {

        //var a = MessageBroker.Default.Receive<MessagePlayerData>().Subscribe(_ => { });
        //MessageBroker.Default.Publish<MessagePlayerData>(new MessagePlayerData(10, 100));
        var btns = GetComponents<BtnSkillBase>();
        foreach (var item in btns)
        {
            item.btnSkillUI = this;
            btnSkillBase.Add(item.state, item);
        }
        foreach (var item in btnSkillBase)
        {
            if (item.Key == BtnState.Enable)
            {
                curState = item.Value;
                curState.StartState();
            }
        }
    }

    private void Update()
    {
        SwitchState(curState.Update());
    }

    void SwitchState(BtnState nextState)
    {
        if (nextState != BtnState.None && nextState != curState.state)
        {
            curState.StopState();
            curState = btnSkillBase[nextState];
            curState.StartState();
        }
    }
}