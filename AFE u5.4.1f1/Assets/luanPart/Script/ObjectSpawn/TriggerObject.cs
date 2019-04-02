using UnityEngine;
using Com.Beetsoft.AFE;

[RequireComponent(typeof(Rigidbody))]
public abstract class TriggerObject : MonoBehaviour, ITriggerObject
{
    [System.NonSerialized]
    public int idObjecctIgnore;
    public void SetIdIgnore(int id)
    {
        idObjecctIgnore = id;
    }

    void Awake()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }

    public float dam = 10;

    protected virtual void OnTriggerEnter(Collider other)
    {
        Debug.Log(idObjecctIgnore + " - " + transform.GetInstanceID());

        if (other.GetComponent<IReceiveDamageable>() != null)
        {
            /*   GetComponent<ObjectElementSkillBehaviour>().ReturnPool();
               other.GetComponent<IReceiveDamageable>().GetDamageReceive(damage.Value, 0);   */
        }
    }
}
