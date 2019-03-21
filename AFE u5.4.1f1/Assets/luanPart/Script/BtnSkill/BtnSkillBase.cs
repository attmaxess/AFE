using UnityEngine;

public enum BtnState
{
    None,
    CountTime,
    Disable,
    Enable
}

public interface IBtnSkillBase<T>
{
    void Input();
    void StartState();
    T Update();
    void StopState();
}


public abstract class BtnSkillBase : MonoBehaviour, IBtnSkillBase<BtnState>
{
    BtnSkillUI _btnSkillUI;
    public BtnSkillUI btnSkillUI { get { return _btnSkillUI; } set { _btnSkillUI = value; } }

    public BtnState state;
    
    protected BtnState nextState;

    public virtual void StartState()
    {
        nextState = state;
    }

    public virtual void StopState()
    {
    }

    public virtual BtnState Update()
    {
        return nextState;
    }

    public virtual void Input()
    {
    }
}
