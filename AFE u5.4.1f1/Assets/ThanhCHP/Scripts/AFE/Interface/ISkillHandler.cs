using System;
using UniRx;

namespace Com.Beetsoft.AFE
{
    public interface ISkillHandler
    {
    }

    public static class SkillHandlerExtension
    {
        public static IDamageMessage CreateDamageMessage(this ISkillHandler skillHandler, ISkillMessage message)
        {
            return new DamageMessage(message.PhysicDamage, message.MagicDamage);
        }

        public static IObservable<ISkillMessage> ApplySkillAfterTimer(this ISkillHandler skillHandler,
            ISkillMessage message, int milliseconds)
        {
            return Observable.Timer(TimeSpan.FromMilliseconds(milliseconds)).Select(_ => message);
        }

//        public static IObservable<ISkillMessage> RequestApplySkill<T>(this ISkillHandler skillHandler,
//            IObservable<ISkillMessage> applySkillAfterTimer, ISkillMessage message, IObservable<T> requestCancer)
//        {
//            
//        }
    }
}