using System;

namespace Com.Beetsoft.AFE
{
    public interface IReceiveDamageable
    {
        void TakeDamage(IDamageMessage message);
    }

    public interface IReceiveDamageObserver
    {
        IObservable<IDamageMessage> OnTakeDamageAsObservable();
    }
}