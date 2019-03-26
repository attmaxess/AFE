using UniRx;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public interface ISkillConfig
    {
        ChampionModel ChampionModel { get; }
        Sprite IconCurrent { get; }
        /// <summary>
        /// Năng lượng tốn mỗi lần dùng skill
        /// </summary>
        FloatReactiveProperty Cost { get; }
        /// <summary>
        /// Năng lượng tốn tăng theo level skill
        /// </summary>
        float CostPerLevel { get; }
        /// <summary>
        /// Tầm sử dụng, tầm bay, tầm ảnh hưởng,...
        /// </summary>
        FloatReactiveProperty Range { get; }
        float RangePerLevel { get; }
        /// <summary>
        /// sát thương vật lý skill gây ra
        /// </summary>
        FloatReactiveProperty PhysicDamage { get; }
        /// <summary>
        /// sát thương vật lý cộng thêm mỗi level
        /// </summary>
        float PhysicDamagePerLevel { get; }
        /// <summary>
        /// sát thương vật lý cộng thêm với sát thương của character
        /// </summary>
        float PhysicDamageBonus { get; }
        /// <summary>
        /// sát thương phép thuật skill gây ra
        /// </summary>
        FloatReactiveProperty MagicDamage { get; }
        /// <summary>
        /// sát thương phép thuật mỗi level
        /// </summary>
        float MagicDamagePerLevel { get; }
        /// <summary>
        /// sát thương phép thuật cộng thêm với sát thương của character
        /// </summary>
        float MagicDamageBonus { get; }
        /// <summary>
        /// thời gian hồi chiêu
        /// </summary>
        FloatReactiveProperty Cooldown { get; }
        /// <summary>
        /// thời gian hồi chiêu giảm mỗi level
        /// </summary>
        float CooldownPerLevel { get; }
    }
}