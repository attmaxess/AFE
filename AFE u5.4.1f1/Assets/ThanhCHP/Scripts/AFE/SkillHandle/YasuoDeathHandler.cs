using AFE.Extensions;
using UniRx;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public class YasuoDeathHandler : SkillHandler
    {
        protected override void Start()
        {
            base.Start();

            Debug.Log("yasuo death handler");

            ChampionConfig.Health.Subscribe(health =>
            {
                if (health <= 0)
                {
                    Debug.Log("DEATH");
                    Animator.SetTriggerWithBool(Constant.AnimationPram.Death);
                }

            });
        }
    }
}

