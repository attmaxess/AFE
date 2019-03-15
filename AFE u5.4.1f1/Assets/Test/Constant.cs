using UnityEngine;

public static class Constant
{
    public static class Animation
    {
        public static int Attack { get; } = Animator.StringToHash("Attack");
        public static int AttackToIdle { get; } = Animator.StringToHash("Attack To Idle");
        public static string AttackInt { get; } = "Attack Int";

        public static string Q { get; } = "Q";
        public static string W { get; } = "W";
        public static string E { get; } = "E";
        public static string R { get; } = "R";

        public static string Idle { get; } = "Idle";
        public static string QToIdle { get; } = "Q To Idle";

        public static string Run { get; } = "Run";
        public static string EToRun { get; } = "E To Run";
        public static string QToRun { get; } = "Q To Run";

        public static string Death { get; } = "Death";
    }

    public static class Yasuo
    {
        public static int AttackClipAmount { get; } = 4;
    }
}