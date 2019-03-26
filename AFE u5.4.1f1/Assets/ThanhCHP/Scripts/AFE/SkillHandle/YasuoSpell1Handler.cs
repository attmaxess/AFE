using UnityEngine;
using UniRx;

namespace Com.Beetsoft.AFE
{
    public interface ISkillSpell_1
    {

    }

    public class YasuoSpell1Handler : SkillHandler,ISkillSpell_1
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