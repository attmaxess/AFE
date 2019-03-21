using Photon.Pun;

namespace Com.Beetsoft.AFE
{
    public abstract class SkillHandler : MonoBehaviourPun,
        ISkillHandler,
        IInitialize<IJoystickInputFilterObserver>,
        IInitialize<IAnimationStateChecker>
    {
        protected IJoystickInputFilterObserver JoystickInputFilterObserver { get; private set; }
        
        protected IAnimationStateChecker AnimationStateChecker { get; private set; }

        void IInitialize<IJoystickInputFilterObserver>.Initialize(IJoystickInputFilterObserver init)
        {
            JoystickInputFilterObserver = init;
        }

        void IInitialize<IAnimationStateChecker>.Initialize(IAnimationStateChecker init)
        {
            AnimationStateChecker = init;
        }
    }
}