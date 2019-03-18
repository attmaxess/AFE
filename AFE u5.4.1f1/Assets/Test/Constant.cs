using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public static class Constant
    {
        public static class AnimationPram
        {
            public static readonly int Attack = Animator.StringToHash("Attack");
            public static readonly int AttackInt = Animator.StringToHash("Attack Int");

            public static readonly int Q = Animator.StringToHash("Q");
            public static readonly int QInt = Animator.StringToHash("Q Int");

            public static readonly int W = Animator.StringToHash("W");
            public static readonly int E = Animator.StringToHash("E");
            public static readonly int R = Animator.StringToHash("R");

            public static readonly int Idle = Animator.StringToHash("Idle");
            public static readonly int IdleBool = Animator.StringToHash("Idle Bool");
            public static readonly int IdleInt = Animator.StringToHash("Idle Int");
            public static readonly int IdleInInt = Animator.StringToHash("Idle In Int");
            public static readonly int IdleOutInt = Animator.StringToHash("Idle Out Int");

            public static readonly int Run = Animator.StringToHash("Run");
            public static readonly int RunOutInt = Animator.StringToHash("Run Out Int");

            public static readonly int Death = Animator.StringToHash("Death");
        }

        public static class SMB
        {
            public static string Attack { get; } = "Attack";

            public static string Idle { get; } = "Idle";
            public static string IdleIn { get; } = "Yasuo_Idle_In";
            public static string SheathIdle { get; } = "Yasuo_Sheath_Idle";

            public static string Run { get; } = "Run";
            public static string RunIn { get; } = "Yasuo_Run_In";
            public static string SheathRun { get; } = "Yasuo_Sheath_Run";
            
            public static string RunFast { get; } = "RunFast";
            public static string Spell1 { get; } = "Q";
            public static string Spell2 { get; } = "W";
            public static string Spell3 { get; } = "E";
            public static string Spell4 { get; } = "R";
            public static string Recall { get; } = "Recall";
            public static string Emote { get; } = "Emote";
            public static string Death { get; } = "Death";
        }

        public static class Yasuo
        {
            public static int AttackClipAmount { get; } = 4;
            public static int QClipAmount { get; } = 4;

            public static float OffsetTimeSpell3AndSpell1 { get; } = 1.2f;
        }
    }
}