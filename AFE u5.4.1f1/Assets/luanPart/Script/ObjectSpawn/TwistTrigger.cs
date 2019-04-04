using UnityEngine;
using Com.Beetsoft.AFE;

public class TwistTrigger : TriggerObject
{
    [SerializeField] private float timeUp = 0.7f;

    private float TimeUp => timeUp;

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.GetComponent<IReceiveDamageable>() != null && other.transform.GetInstanceID() != idObjecctIgnore)
        {
            other.GetComponent<IReceiveDamageable>().GetDamageReceive(dam, 0);
            var blowUp = other.GetComponent<BlowUpObject>();
            blowUp?.BlowUp(TimeUp);
        }
    }
}
