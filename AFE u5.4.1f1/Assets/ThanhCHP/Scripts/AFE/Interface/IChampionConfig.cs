using UniRx;

namespace Com.Beetsoft.AFE
{
    public interface IChampionConfig
    {
        /// <summary>
        /// Máu
        /// </summary>
        FloatReactiveProperty Health { get; }
        /// <summary>
        /// Máu cộng thêm mỗi level
        /// </summary>
        float HealthPerLevel { get; }
        /// <summary>
        /// máu hồi mỗi giây
        /// </summary>
        FloatReactiveProperty HealthRegen { get; }
        /// <summary>
        /// máu hồi mỗi giây cộng thêm mỗi khi lên level
        /// </summary>
        float HealthRegenPerLevel { get; }
        /// <summary>
        /// Năng lương
        /// </summary>
        FloatReactiveProperty Mana { get; }
        /// <summary>
        /// Năng lương cộng thêm mỗi level
        /// </summary>
        float ManaPerLevel { get; }
        /// <summary>
        /// Năng lương hồi mỗi giây
        /// </summary>
        FloatReactiveProperty ManaRegen { get; }
        /// <summary>
        /// Năng lương hồi mỗi giây cộng thêm mỗi khi lên level
        /// </summary>
        float ManaRegenPerLevel { get; }
        /// <summary>
        /// Tầm đánh
        /// </summary>
        FloatReactiveProperty Range { get; }
        /// <summary>
        /// Tầm đánh cộng thêm mỗi level
        /// </summary>
        float RangePerLevel { get; }
        /// <summary>
        /// sát thương gây ra mỗi đòn đánh, hoặc dùng cộng vào sát thương của skill
        /// </summary>
        FloatReactiveProperty AttackDamage { get; }
        /// <summary>
        /// sát thương được cộng thêm khi lên level
        /// </summary>
        float AttackDamagePerLevel { get; }
        /// <summary>
        /// tốc độ ra đòn mỗi giây
        /// </summary>
        FloatReactiveProperty AttackSpeed { get; }
        /// <summary>
        /// tốc độ đánh cộng thêm khi lên level
        /// </summary>
        float AttackSpeedPerLevel { get; }
        /// <summary>
        /// sức mạnh phép thuật
        /// </summary>
        FloatReactiveProperty AbilityPower { get; }
        /// <summary>
        /// giáp
        /// </summary>
        FloatReactiveProperty Armor { get; }
        /// <summary>
        /// giáp cộng thêm mỗi level
        /// </summary>
        float ArmorPerLevel { get; }
        /// <summary>
        /// kháng phép
        /// </summary>
        FloatReactiveProperty MagicResist { get; }
        /// <summary>
        /// kháng phép công thêm mỗi level
        /// </summary>
        float MagicResistPerLevel { get; }
        /// <summary>
        /// tỉ lệ chí mạng
        /// </summary>
        FloatReactiveProperty Critical { get; }
        /// <summary>
        /// tốc độ di chuyển
        /// </summary>
        FloatReactiveProperty MoveSpeed { get; }
        /// <summary>
        /// giảm thời gian hồi chiêu skill
        /// </summary>
        FloatReactiveProperty CooldownSkillBonus { get; }
    }
}