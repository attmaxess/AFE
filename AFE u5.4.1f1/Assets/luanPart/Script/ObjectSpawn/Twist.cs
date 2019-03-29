using UnityEngine;
using Com.Beetsoft.AFE;

public class Twist : TriggerObject
{
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.GetComponent<WindWall>())
        {
            gameObject.GetComponent<IReceiveDamageable>().GetDamageReceive(dam,0);
            gameObject.GetComponent<ObjectElementSkillBehaviour>().ReturnPool();
        }
    }
}