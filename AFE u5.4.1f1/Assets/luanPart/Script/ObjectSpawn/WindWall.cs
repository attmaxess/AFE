using UnityEngine;
using Com.Beetsoft.AFE;

public class WindWall : TriggerObject
{
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.GetComponent<IDamageOnPlayer>() != null)
        {
            Debug.Log("IDamageOnPlayer");
            other.GetComponent<IReceiveDamageable>().GetDamageReceive(10, 0);
        }
    }
}
