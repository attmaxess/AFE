using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace Com.Beetsoft.AFE
{
    public class ChampionModel : ScriptableObject, IChampionConfig
    {
        [SerializeField] private FloatReactiveProperty health;
        [SerializeField] private float healthPerLevel;
        [SerializeField] private FloatReactiveProperty healthRegen;
        [SerializeField] private float healthRegenPerLevel;
        [SerializeField] private FloatReactiveProperty mana;
        [SerializeField] private float manaPerLevel;
        [SerializeField] private FloatReactiveProperty manaRegen;
        [SerializeField] private float manaRegenPerLevel;
        [SerializeField] private FloatReactiveProperty range;
        [SerializeField] private float rangePerLevel;
        [SerializeField] private FloatReactiveProperty attackDamage;
        [SerializeField] private float attackDamagePerLevel;
        [SerializeField] private FloatReactiveProperty attackSpeed;
        [SerializeField] private float attackSpeedPerLevel;
        [SerializeField] private FloatReactiveProperty abilityPower;
        [SerializeField] private FloatReactiveProperty armor;
        [SerializeField] private float armorPerLevel;
        [SerializeField] private FloatReactiveProperty magicResist;
        [SerializeField] private float magicResistPerLevel;
        [SerializeField] private FloatReactiveProperty critical;
        [SerializeField] private FloatReactiveProperty moveSpeed;

        FloatReactiveProperty IChampionConfig.Health => health;

        float IChampionConfig.HealthPerLevel => healthPerLevel;

        FloatReactiveProperty IChampionConfig.HealthRegen => healthRegen;

        float IChampionConfig.HealthRegenPerLevel => healthRegenPerLevel;

        FloatReactiveProperty IChampionConfig.Mana => mana;

        float IChampionConfig.ManaPerLevel => manaPerLevel;

        FloatReactiveProperty IChampionConfig.ManaRegen => manaRegen;

        float IChampionConfig.ManaRegenPerLevel => manaRegenPerLevel;

        FloatReactiveProperty IChampionConfig.Range => range;

        float IChampionConfig.RangePerLevel => rangePerLevel;

        FloatReactiveProperty IChampionConfig.AttackDamage => attackDamage;

        float IChampionConfig.AttackDamagePerLevel => attackDamagePerLevel;

        FloatReactiveProperty IChampionConfig.AttackSpeed => attackSpeed;

        float IChampionConfig.AttackSpeedPerLevel => attackSpeedPerLevel;

        FloatReactiveProperty IChampionConfig.AbilityPower => abilityPower;

        FloatReactiveProperty IChampionConfig.Armor => armor;

        float IChampionConfig.ArmorPerLevel => armorPerLevel;

        FloatReactiveProperty IChampionConfig.MagicResist => magicResist;

        float IChampionConfig.MagicResistPerLevel => magicResistPerLevel;

        FloatReactiveProperty IChampionConfig.Critical => critical;

        FloatReactiveProperty IChampionConfig.MoveSpeed => moveSpeed;
    }
}