using UnityEngine;
using Com.Beetsoft.AFE;

public class WindWallTrigger : TriggerObject
{
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        var damBullet = other.GetComponent<IDamageBullet>();
        damBullet?.ReturnPool();
        if (other.GetComponent<IDamageBullet>() != null)
        {
            other.GetComponent<IReceiveDamageable>().GetDamageReceive(10, 0);
        }
    }
}
