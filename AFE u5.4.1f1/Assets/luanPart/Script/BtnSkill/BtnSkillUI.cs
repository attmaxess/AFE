using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using ControlFreak2;
using UniRx;
using UniRx.Triggers;
using TMPro;
using UnityEngine.UI;
using Com.Beetsoft.AFE;
using System.Linq;

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
    public BtnState btnState;
    public bool canUseSkill;
    public bool isCountTime;
    public TextMeshProUGUI number;
    public TouchJoystickSpriteAnimator touchJoystickSprite;
    public enum SkillType
    {
        Skill_1,
        Skill_2,
        Skill_3,
        Skill_4
    }

    public SkillType skillType;
    private SkillHandler skillHandler;

    [HideInInspector]
    public float countTime;

    private void Start()
    {
        canUseSkill = true;
        isCountTime = false;
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
                btnState = BtnState.Enable;
                curState = item.Value;
                curState.StartState();
            }
        }

        /*   skillData.CountTime.Where(_count => _count > 0).Subscribe(_ =>
           {
               countTime = _;
           });
           skillData.Disable.Subscribe(_ =>
           {
               canUseSkill = !_;
           });
           skillData.SpriteCurrent.Subscribe(_ =>
           {
               touchJoystickSprite.SetSprite(_);
           });   */


        FinDSkillReader();
        skillHandler.SkillMessageOutputCurrent().ObserveEveryValueChanged(_ =>
        {
            Debug.Log("Have Change");
            countTime = _.Cooldown;
            touchJoystickSprite.SetSprite(_.Icon);
            return true;
        });
    }

    void FinDSkillReader()
    {
        if (skillHandler != null) return;
        TestYasuo[] _listTestYasuo = FindObjectsOfType<TestYasuo>();

        var yasuoMine = _listTestYasuo.Where(_ => _.photonView.IsMine).Single();

        if (skillType == SkillType.Skill_1)
        {
            skillHandler = yasuoMine.GetComponents<SkillHandler>().Where(_ => _.GetComponent<ISkillSpell_1>() != null).Single();
        }
        if (skillType == SkillType.Skill_2)
        {
            skillHandler = yasuoMine.GetComponents<SkillHandler>().Where(_ => _.GetComponent<ISkillSpell_2>() != null).Single();
        }
        if (skillType == SkillType.Skill_3)
        {
            skillHandler = yasuoMine.GetComponents<SkillHandler>().Where(_ => _.GetComponent<ISkillSpell_3>() != null).Single();
        }
        if (skillType == SkillType.Skill_4)
        {
            skillHandler = yasuoMine.GetComponents<SkillHandler>().Where(_ => _.GetComponent<ISkillSpell_4>() != null).Single();
        }

    }

    private void Update()
    {
        SwitchState(curState.UpdateState());
    }

    void SwitchState(BtnState nextState)
    {
        if (nextState != BtnState.None && nextState != curState.state)
        {
            btnState = nextState;
            curState.StopState();
            curState = btnSkillBase[nextState];
            curState.StartState();
        }
    }

    public void ReceiveMessage(Sprite sprite, float countTime, bool canUseSkill)
    {
        this.countTime = countTime > 0 ? countTime : 0;
        this.canUseSkill = canUseSkill;
        touchJoystickSprite.SetSprite(sprite);
    }
}