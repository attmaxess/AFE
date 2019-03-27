using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public class BladeAttack : SkillBehaviour
    {
        [SerializeField] private LayerMask layerMaskTarget;

        private LayerMask LayerMaskTarget => layerMaskTarget;

        public override void ActiveSkill(IInputMessage inputMessage)
        {
            var receiver = gameObject.GetAllReceiverDamageNearestByRayCastAll(inputMessage.Direction
                , SkillConfig.Range.Value, LayerMaskTarget);
            ActiveSkillSubject.OnNext(receiver);
        }
    }
}