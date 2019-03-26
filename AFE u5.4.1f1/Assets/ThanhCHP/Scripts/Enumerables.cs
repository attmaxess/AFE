namespace Com.Beetsoft.AFE.Enumerables
{
    public static class AnimationState
    {
        public enum BasicAttack
        {
            Hit1 = 1,
            Hit2,
            Hit3,
            Hit4
        }

        public enum Spell1
        {
            Spell1A = 1,
            Spell1B,
            Spell1C,
            Spell1_Dash,
        }

        public enum RunOut
        {
            None = 0,
            RunOut,
            RunOutLoop,
        }

        public enum IdleIn
        {
            None = 0,
            IdleIn,
        }

        public enum IdleOut
        {
            None,
            IdleOut,
        }
    }
}