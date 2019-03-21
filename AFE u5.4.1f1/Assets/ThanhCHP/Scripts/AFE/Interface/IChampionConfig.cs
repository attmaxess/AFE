using UniRx;

namespace Com.Beetsoft.AFE
{
    public interface IChampionConfig
    {
        FloatReactiveProperty Health { get; }
        float HealthPerLevel { get; }
        FloatReactiveProperty HealthRegen { get; }
        float HealthRegenPerLevel { get; }
        FloatReactiveProperty Mana { get; }
        float ManaPerLevel { get; }
        FloatReactiveProperty ManaRegen { get; }
        float ManaRegenPerLevel { get; }
        FloatReactiveProperty Range { get; }
        float RangePerLevel { get; }
        FloatReactiveProperty AttackDamage { get; }
        float AttackDamagePerLevel { get; }
        FloatReactiveProperty AttackSpeed { get; }
        float AttackSpeedPerLevel { get; }
        FloatReactiveProperty AbilityPower { get; }
        FloatReactiveProperty Armor { get; }
        float ArmorPerLevel { get; }
        FloatReactiveProperty MagicResist { get; }
        float MagicResistPerLevel { get; }
        FloatReactiveProperty Critical { get; }
        FloatReactiveProperty MoveSpeed { get; }
        FloatReactiveProperty CooldownSkillBonus { get; }
    }
}