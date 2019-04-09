using System;
using Photon.Pun;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Com.Beetsoft.AFE
{
    public interface IKnockUpable
    {
        void BlowUp(float timeUp);
    }

    public interface ICrowdControl
    {
        ReactiveProperty<bool> IsCrowdControl { get; }
    }

    public class BlowUpObject : MonoBehaviourPun,
        ICrowdControl,
        IKnockUpable
    {
        private IDisposable dispose;
        [SerializeField] private ObservableTween.EaseType easeType = ObservableTween.EaseType.OutCubic;
        [SerializeField] private float maxYAxis = 4.0f;
        [SerializeField] private float minYAxis;

        private float MinYAxis => minYAxis;

        private float MaxYAxis => maxYAxis;

        private ObservableTween.EaseType EaseType => easeType;

        private IDisposable TweenDisposable { get; set; }

        public ReactiveProperty<bool> IsCrowdControl { get; } = new ReactiveProperty<bool>(false);

        private void Start()
        {
            minYAxis = transform.position.y;
        }
        public void BlowUp(float timeUp)
        {
            photonView.RPC("BlowUpRpc", RpcTarget.All, timeUp, GetRandomRotate());
        }

        [PunRPC]
        private void BlowUpRpc(float timeUp, Vector3 forceRotate)
        {
            IsCrowdControl.Value = true;
            DoBlowUp(timeUp, () => DoBlowDown(Constant.KnockDown, DoOnBlowDownComplete));
            SendMessageBlowUp();
            Rotate(forceRotate);
        }

        private void DoBlowUp(float time, Action onComplete = null)
        {
            TweenDisposable?.Dispose();
            var position = transform.position;
            TweenDisposable = Blow(position.y, MaxYAxis, time, EaseType, onComplete);
        }

        private void DoBlowDown(float time, Action onComplete = null)
        {
            TweenDisposable?.Dispose();
            var position = transform.position;
            TweenDisposable = Blow(position.y, MinYAxis, time, ObservableTween.EaseType.InCubic, onComplete);
        }


        private IDisposable Blow(float yStart, float yEnd, float time, ObservableTween.EaseType easeType,
            Action onComplete = null)
        {
            return ObservableTween.Tween(yStart, yEnd, time, easeType, onCompleteTween: onComplete)
                .Subscribe(posY =>
                {
                    var temp = transform.position;
                    temp.y = posY;
                    transform.position = temp;
                });
        }

        private void SendMessageBlowUp()
        {
            AsyncMessageBroker.Default.PublishAsync(new BlockUpArgs(GetComponent<IReceiveDamageable>()))
                .Subscribe(_ => { Debug.Log("Send message blow up success"); });
        }

        private void SendMessageBlowDown()
        {
            AsyncMessageBroker.Default.PublishAsync(new BlockDownArgs(GetComponent<IReceiveDamageable>()))
                .Subscribe(_ => { Debug.Log("Send message blow down success"); });
        }

        private void DoOnBlowDownComplete()
        {
            IsCrowdControl.Value = false;
            SendMessageBlowDown();
        }

        private void Rotate(Vector3 forward)
        {
            var championTransform = GetComponent<IChampionTransform>();
            if (championTransform != null)
            {
                championTransform.Forward = forward;
            }
        }

        private static Vector3 GetRandomRotate() => Quaternion.Euler(0, Random.Range(0, 360), 0) * Vector3.forward;
    }
}