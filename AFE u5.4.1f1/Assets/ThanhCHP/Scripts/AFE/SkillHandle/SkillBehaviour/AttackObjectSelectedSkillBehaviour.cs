using System;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public class AttackObjectSelectedSkillBehaviour : SkillBehaviour
    {
        private const float RadiusAttack = 7.0f;

        [SerializeField] private FollowObjectBlowUp followObjectBlowUp;

        private FollowObjectBlowUp FollowObjectBlowUp => followObjectBlowUp
            ? followObjectBlowUp
            : followObjectBlowUp = gameObject.GetOrAddComponent<FollowObjectBlowUp>();

        public override void ActiveSkill(IInputMessage inputMessage)
        {
            if (inputMessage.ObjectReceive != null)
            {
                var receivers =
                    FollowObjectBlowUp.GetObjectsBlowUpArea(inputMessage.ObjectReceive.GetTransform.position,
                        RadiusAttack);

                ActiveSkillSubject.OnNext(receivers);
                Observable.Timer(TimeSpan.FromMilliseconds(300))
                    .Subscribe(_ => receivers.ToList().ForEach(x => x.TakeDamage(CreateDamageMessage())));
            }
        }
    }
}