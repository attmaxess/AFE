using UnityEngine;
using Com.Beetsoft.AFE;

public class TwistTrigger : TriggerObject
{

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.GetComponent<IReceiveDamageable>() != null && other.transform.GetInstanceID() != idObjecctIgnore)
        {
            other.GetComponent<IReceiveDamageable>().GetDamageReceive(dam, 0);
            var _blowUp = other.GetComponent<BlowUpObject>();
            if (_blowUp == null)
            {
                _blowUp = other.gameObject.AddComponent<BlowUpObject>();
            }
            _blowUp.BlowUp();
            gameObject.GetComponent<ObjectElementSkillBehaviour>().ReturnPool();
        }
    }
}
