using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public interface IJoystickInputFilter
    {
        void Run(IRunMessage message);
        void Idle(IRunMessage message);
        void BasicAttack(IInputMessage message);
        void Spell1(IInputMessage message);
        void Spell2(IInputMessage message);
        void Spell3(IInputMessage message);
        void Spell4(IInputMessage message);
        void Recall(IInputMessage message);
        void DefaultSpellA(IInputMessage message);
        void DefaultSpellB(IInputMessage message);
    }
}