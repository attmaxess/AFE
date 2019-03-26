using UniRx;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    [CreateAssetMenu(fileName = "SkillModel", menuName = "AFE/Skill", order = 2)]
    public class SkillModel : ScriptableObject, ISkillConfig
    {
        [SerializeField] private ChampionModel championModel;
        [SerializeField] private Sprite iconCurrent;
        [SerializeField] private FloatReactiveProperty cost;
        [SerializeField] private float costPerLevel;
        [SerializeField] private FloatReactiveProperty range;
        [SerializeField] private float rangePerLevel;
        [SerializeField] private FloatReactiveProperty physicDamage;
        [SerializeField] private float physicDamagePerLevel;
        [SerializeField] private float physicDamageBonus;
        [SerializeField] private FloatReactiveProperty magicDamage;
        [SerializeField] private float magicDamagePerLevel;
        [SerializeField] private float magicDamageBonus;
        [SerializeField] private FloatReactiveProperty cooldown;
        [SerializeField] private float cooldownPerLevel;

        ChampionModel ISkillConfig.ChampionModel => championModel;

        Sprite ISkillConfig.IconCurrent => iconCurrent;

        FloatReactiveProperty ISkillConfig.Cost => cost;

        float ISkillConfig.CostPerLevel => costPerLevel;

        FloatReactiveProperty ISkillConfig.Range => range;

        float ISkillConfig.RangePerLevel => rangePerLevel;

        FloatReactiveProperty ISkillConfig.PhysicDamage => physicDamage;

        float ISkillConfig.PhysicDamagePerLevel => physicDamagePerLevel;

        float ISkillConfig.PhysicDamageBonus => physicDamageBonus;

        FloatReactiveProperty ISkillConfig.MagicDamage => magicDamage;

        float ISkillConfig.MagicDamagePerLevel => magicDamagePerLevel;

        float ISkillConfig.MagicDamageBonus => magicDamageBonus;

        FloatReactiveProperty ISkillConfig.Cooldown => cooldown;

        float ISkillConfig.CooldownPerLevel => cooldownPerLevel;
    }

}