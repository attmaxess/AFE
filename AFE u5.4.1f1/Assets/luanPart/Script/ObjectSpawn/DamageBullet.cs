using Com.Beetsoft.AFE;
using UnityEngine;


public interface IDamageBullet
{
    void ReturnPool();
}

public class DamageBullet : MonoBehaviour, IDamageBullet
{
    ObjectElementSkillBehaviour objectElementSkillBehaviour;
    void Awake()
    {
        objectElementSkillBehaviour = GetComponent<ObjectElementSkillBehaviour>();
    }
    public void ReturnPool()
    {
        objectElementSkillBehaviour?.ReturnPool();
    }
}
