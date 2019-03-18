using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateCharacter
{
    None,
    Idle,
    Run,
    Attack,
    Skill,
    Death,
    Hit
}

public interface IBaseCharacterState<T>
{
    void Transtion();
    void StartState();
    T Update();
    void EndState();
}


public abstract class BaseCharaterState : IBaseCharacterState<StateCharacter>
{
    public StateCharacter nextState;
    public StateCharacter stateType;
    ThirdPersonControllerNET player;
    public ThirdPersonControllerNET Player
    {
        set { player = value; }
        get { return player; }
    }
    public abstract void StartState();

    public virtual StateCharacter Update()
    {
        Transtion();
        return nextState;
    }

    public abstract void EndState();

    public abstract void Transtion();

}

public class IdleState : BaseCharaterState
{
    public StateCharacter character;

    public override void EndState()
    {
        Player.idleRpc = false;
    }

    public override void StartState()
    {
        Player.PlayAnim("Idle");
    }

    public override void Transtion()
    {
        if (Player.AttackRpc)
        {
            nextState = StateCharacter.Attack;
        }

        if (Player.skill_1Rpc)
        {
            nextState = StateCharacter.Skill;
        }

        if (Player.skill_2Rpc)
        {
            nextState = StateCharacter.Skill;
        }

        if (Player.skill_3Rpc)
        {
            nextState = StateCharacter.Skill;
        }

        if (Player.skill_4Rpc)
        {
            nextState = StateCharacter.Skill;
        }

        if (Player.runRpc)
        {
            nextState = StateCharacter.Run;
        }
    }

    public override StateCharacter Update()
    {
        return nextState;
    }
}

public class HitState : BaseCharaterState
{
    public override void EndState()
    {
        Player.hitRpc = false;
    }

    public override void StartState()
    {
        Player.PlayAnim("Hit");
    }

    public override void Transtion()
    {
        if (Player.AttackRpc)
        {
            nextState = StateCharacter.Attack;
        }

        if (Player.skill_1Rpc)
        {
            nextState = StateCharacter.Skill;
        }

        if (Player.skill_2Rpc)
        {
            nextState = StateCharacter.Skill;
        }

        if (Player.skill_3Rpc)
        {
            nextState = StateCharacter.Skill;
        }

        if (Player.skill_4Rpc)
        {
            nextState = StateCharacter.Skill;
        }

        if (Player.runRpc)
        {
            nextState = StateCharacter.Run;
        }
    }
}

public class SkillState : BaseCharaterState
{
    public override void EndState()
    {

    }

    public override void StartState()
    {
        Player.PlayAnim("Skill");
    }

    public override void Transtion()
    {
        if (Player.runRpc)
        {
            nextState = StateCharacter.Run;
        }

        if (Player.idleRpc)
        {
            nextState = StateCharacter.Run;
        }
        if (Player.AttackRpc)
        {
            nextState = StateCharacter.Attack;
        }
    }
}


