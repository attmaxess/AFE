using System.Linq;
using Photon.Pun;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public class TestYasuo : MonoBehaviourPun
    {
        [SerializeField] private ChampionModel championModel;
        [SerializeField] private JoystickInputFilter joystickInputFilter;
        [SerializeField] private AnimatorHandler animatorHandler;
        [SerializeField] private HealthBarPlayer barPlayer;

        private ChampionModel ChampionModel => championModel;

        private JoystickInputFilter JoystickInputFilter => joystickInputFilter;

        private AnimatorHandler AnimatorHandler => animatorHandler;

        private ChampionModel ChampionCopy;


        private void Awake()
        {
            var monoPuns = GetComponents<MonoBehaviourPun>();

            // edit
            ChampionCopy = Instantiate(ChampionModel);

            var initChampionConfigList = monoPuns.OfType<IInitialize<IChampionConfig>>().ToList();
            initChampionConfigList.ForEach(x => x.Initialize(ChampionCopy));

            var initJoystick = monoPuns.OfType<IInitialize<IJoystickInputFilterObserver>>().ToList();
            initJoystick.ForEach(x => x.Initialize(JoystickInputFilter));

            var intiAnimationChecker = monoPuns.OfType<IInitialize<IAnimationStateChecker>>().ToList();
            intiAnimationChecker.ForEach(x => x.Initialize(AnimatorHandler));

        }

        private void Start()
        {
            CreateHealthBar(ChampionCopy, photonView.IsMine);
            if (!photonView.IsMine) return;
        }

        void CreateHealthBar(IChampionConfig championConfig, bool isMine)
        {
            var _barPlayer = Instantiate(barPlayer.transform, CanvasJoystickManager.Singleton.barPlayer, false);
            _barPlayer.GetComponent<HealthBarPlayer>().SetInit(championConfig, isMine);
        }
    }
}

