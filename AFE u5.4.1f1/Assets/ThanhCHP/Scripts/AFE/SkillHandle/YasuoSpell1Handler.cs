using UnityEngine;
using  UniRx;

namespace Com.Beetsoft.AFE
{
    public class YasuoSpell1Handler : SkillHandler
    {
        [SerializeField] private SkillReader skillReader;

        private SkillReader SkillReader => skillReader;

        private void Start()
        {
            this.JoystickInputFilterObserver
                .OnSpell1AsObservable()
                .Subscribe(message => { SkillReader.GetSkillBehaviourCurrent().ActiveSkill(message); });
        }
    }
}