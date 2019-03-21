using UniRx;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public interface ISkillConfig
    {
        ChampionModel ChampionModel { get; }
        Sprite IconCurrent { get; }
        FloatReactiveProperty Cost { get; }
        float CostPerLevel { get; }
        FloatReactiveProperty Range { get; }
        float RangePerLevel { get; }
        FloatReactiveProperty PhysicDamage { get; }
        float PhysicDamagePerLevel { get; }
        float PhysicDamageBonus { get; }
        FloatReactiveProperty MagicDamage { get; }
        float MagicDamagePerLevel { get; }
        float MagicDamageBonus { get; }
        FloatReactiveProperty Cooldown { get; }
        float CooldownPerLevel { get; }
    }
}