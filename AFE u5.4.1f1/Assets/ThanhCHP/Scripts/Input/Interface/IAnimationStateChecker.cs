using UniRx;

namespace Com.Beetsoft.AFE
{
    public interface IAnimationStateChecker
    {
        IReactiveProperty<bool> IsRun { get; }
        IReactiveProperty<bool> IsBasicAttack { get; }
        IReactiveProperty<bool> IsInStateSpell1 { get; }
        IReactiveProperty<bool> IsInStateSpell2 { get; }
        IReactiveProperty<bool> IsInStateSpell3 { get; }
        IReactiveProperty<bool> IsInStateSpell4 { get; }
    }
}