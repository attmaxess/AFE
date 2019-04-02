using System;
using Photon.Pun;
using UniRx;
using UnityEngine;

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
            photonView.RPC("BlowUpRpc", RpcTarget.All, timeUp);
        }

        [PunRPC]
        private void BlowUpRpc(float timeUp)
        {
            IsCrowdControl.Value = true;
            DoBlowUp(timeUp, () => DoBlowDown(Constant.KnockDown, DoOnBlowDownComplete));
            SendMessageBlowUp();
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
    }
}