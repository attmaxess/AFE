using UnityEngine;
using Com.Beetsoft.AFE;

[RequireComponent(typeof(Rigidbody))]
public class TriggerObject : MonoBehaviour, ITriggerObject
{

    public float dam = 10;
    void Awake()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter " + other.transform.name);

        if (other.GetComponent<IReceiveDamageable>() != null)
        {
            /*   GetComponent<ObjectElementSkillBehaviour>().ReturnPool();
               other.GetComponent<IReceiveDamageable>().GetDamageReceive(damage.Value, 0);   */
        }
    }
}
