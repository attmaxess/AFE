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
    T UpdateState();
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
        Debug.Log("StartState - " + nextState);
        nextState = state;
    }

    public virtual void StopState()
    {
    }

    public virtual BtnState UpdateState()
    {
        Debug.Log("Update - " + nextState + " - "+ gameObject.name + " - "+ this);
        return nextState;
    }

    public virtual void Input()
    {
    }
}
