using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public interface IJoystickInputFilter
    {
        void Run(Vector3 dir);
        void Idle(Vector3 position);
        void BasicAttack(GameObject target);
        void Spell1(Vector3 dir);
        void Spell2(Vector3 dir);
        void Spell3(Vector3 dir);
        void Spell4(Vector3 dir);
        void Recall(Vector3 dir);
        void DefaultSpellA(Vector3 dir);
        void DefaultSpellB(Vector3 dir);
    }
}