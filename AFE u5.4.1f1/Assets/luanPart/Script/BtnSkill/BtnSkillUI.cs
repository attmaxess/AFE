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
    public SkillHandler skillHandler;

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


    }

    void FinDSkillReader()
    {
        if (skillHandler != null) return;
        TestYasuo[] _listTestYasuo = FindObjectsOfType<TestYasuo>();
        if (_listTestYasuo.Length <= 0) return;

        var yasuoMine = _listTestYasuo.Single(_ => _.photonView.IsMine);
        if (skillType == SkillType.Skill_1)
        {
            var _l = yasuoMine.GetComponents<SkillHandler>();
            foreach (var item in _l)
            {
                if (item.GetComponent<ISkillSpell_1>() != null)
                    skillHandler = item;
            }
        }
        if (skillType == SkillType.Skill_2)
        {
            var _l = yasuoMine.GetComponents<SkillHandler>();
            foreach (var item in _l)
            {
                if (item.GetComponent<ISkillSpell_2>() != null)
                    skillHandler = item;
            }
        }
        if (skillType == SkillType.Skill_3)
        {
            var _l = yasuoMine.GetComponents<SkillHandler>();
            foreach (var item in _l)
            {
                if (item.GetComponent<ISkillSpell_3>() != null)
                    skillHandler = item;
            }
        }
        if (skillType == SkillType.Skill_4)
        {
            var _l = yasuoMine.GetComponents<SkillHandler>();
            foreach (var item in _l)
            {
                if (item.GetComponent<ISkillSpell_4>() != null)
                    skillHandler = item;
            }
        }

        skillHandler.OnReceiveSkillMessageOutputAsObservable()
           .Subscribe(_skillHandle =>
           {
               if (_skillHandle == null)
               {
                   Debug.Log("_skillHandle == null");
                   return;
               }
               else
               {
                   Debug.Log(_skillHandle.Cooldown);
                   isCountTime = true;
                   canUseSkill = false;
                   countTime = _skillHandle.Cooldown;
                   if (_skillHandle.Icon != null)
                       touchJoystickSprite.SetSprite(_skillHandle.Icon);
               }
           });
    }

    private void Update()
    {
        SwitchState(curState.UpdateState());
        FinDSkillReader();

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