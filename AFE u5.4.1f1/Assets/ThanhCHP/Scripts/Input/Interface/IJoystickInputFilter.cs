using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public interface IJoystickInputFilter
    {
        void Run(IRunMessage message);
        void Idle(IRunMessage message);
        void BasicAttack(ISkillMessage message);
        void Spell1(ISkillMessage message);
        void Spell2(ISkillMessage message);
        void Spell3(ISkillMessage message);
        void Spell4(ISkillMessage message);
        void Recall(ISkillMessage message);
        void DefaultSpellA(ISkillMessage message);
        void DefaultSpellB(ISkillMessage message);
    }
}