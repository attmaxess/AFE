namespace Com.Beetsoft.AFE
{
    public class BlockDownArgs
    {
        public BlockDownArgs(IReceiveDamageable objectHasBeenBlowDown)
        {
            ObjectHasBeenBlowDown = objectHasBeenBlowDown;
        }

        public IReceiveDamageable ObjectHasBeenBlowDown { get; }
    }
}