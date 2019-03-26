using System;
using UniRx;

namespace Com.Beetsoft.AFE
{
    public interface ISkillHandler
    {
        ISkillOutputMessage SkillMessageOutputCurrent();
        IObservable<ISkillOutputMessage> OnReceiveSkillMessageOutputAsObservable();
    }

    public static class SkillHandlerExtension
    {
        public static IDamageMessage CreateDamageMessage(this ISkillHandler skillHandler, IInputMessage message)
        {
            return new DamageMessage(message.PhysicDamage, message.MagicDamage);
        }

        public static IObservable<IInputMessage> ApplySkillAfterTimer(this ISkillHandler skillHandler,
            IInputMessage message, int milliseconds)
        {
            return Observable.Timer(TimeSpan.FromMilliseconds(milliseconds)).Select(_ => message);
        }

//        public static ISkillOutputMessage CreateSkillOutputMessage(this ISkillHandler skillHandler)
//        {
//            
//        }

//        public static IObservable<ISkillMessage> RequestApplySkill<T>(this ISkillHandler skillHandler,
//            IObservable<ISkillMessage> applySkillAfterTimer, ISkillMessage message, IObservable<T> requestCancer)
//        {
//            
//        }
    }
}