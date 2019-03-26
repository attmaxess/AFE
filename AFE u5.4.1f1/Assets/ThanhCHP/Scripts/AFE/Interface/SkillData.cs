using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Beetsoft.AFE
{

    // edit other Skill Data
    [System.Serializable]
    public class ImageReactiveProperty : ReactiveProperty<Sprite>
    {
        public ImageReactiveProperty()
        {
        }

        public ImageReactiveProperty(Sprite initialValue) : base(initialValue)
        {
        }
    }
    [System.Serializable]
    public class TypeSkillReactiveProperty : ReactiveProperty<TypeSkill>
    {
        public TypeSkillReactiveProperty()
        {
        }

        public TypeSkillReactiveProperty(TypeSkill initialValue) : base(initialValue)
        {
        }
    }
    public enum TypeSkill
    {
        BasicAttack,
        Skill_1,
        Skill_2,
        Skill_3,
        Skill_4
    }

    public interface ISkillData
    {
        BoolReactiveProperty Disable { get; }
        FloatReactiveProperty CountTime { get; }
        ImageReactiveProperty SpriteCurrent { get; }
        ChampionModel ChampionModel { get; }
        FloatReactiveProperty Range { get; }
        TypeSkillReactiveProperty TypeSkillReactiveProperty { get; }
    }

    [CreateAssetMenu(fileName = "SkillData", menuName = "AFE/SkillData", order = 3)]
    public class SkillData : ScriptableObject, ISkillData
    {
        public BoolReactiveProperty Disable => disable;

        public FloatReactiveProperty CountTime => countTime;

        public ImageReactiveProperty SpriteCurrent => spriteCurrent;

        public ChampionModel ChampionModel => championModel;

        public FloatReactiveProperty Range => range;

        public TypeSkillReactiveProperty TypeSkillReactiveProperty => typeSkillReactiveProperty;

        [SerializeField]
        BoolReactiveProperty disable;
        [SerializeField]
        FloatReactiveProperty countTime;
        [SerializeField]
        ImageReactiveProperty spriteCurrent;
        [SerializeField]
        ChampionModel championModel;
        [SerializeField]
        FloatReactiveProperty range;
        [SerializeField]
        TypeSkillReactiveProperty typeSkillReactiveProperty;
    }

}