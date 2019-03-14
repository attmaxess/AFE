using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateCharacter
{
    Idle,
    Run,
    Attack,
    Skill
}

public interface IBaseCharacterState<T>
{
    void Transtion();
    void StartState();
    T Update();
    void EndState();
}


public class BaseCharaterState : IBaseCharacterState<StateCharacter>
{
    StateCharacter nextState;
    public StateCharacter stateType;
    ThirdPersonControllerNET player;
    public ThirdPersonControllerNET Player
    {
        set { player = value; }
        get { return player; }
    }
    public virtual void StartState()
    {

    }

    public virtual StateCharacter Update()
    {
        return nextState;
    }

    public virtual void EndState()
    {

    }

    public virtual void Transtion()
    {
    }
}

public class IdleState : BaseCharaterState
{
    public StateCharacter character;

    public override void StartState()
    {
        base.StartState();
    }

    public override void Transtion()
    {
        base.Transtion();
    }

    public override StateCharacter Update()
    {
        Transtion();
        return base.Update();
    }
}


