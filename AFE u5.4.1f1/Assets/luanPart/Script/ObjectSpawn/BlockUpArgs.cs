namespace Com.Beetsoft.AFE
{
    public class BlockUpArgs
    {
        public BlockUpArgs(IReceiveDamageable objectHasBeenBlowUp)
        {
            ObjectHasBeenBlowUp = objectHasBeenBlowUp;
        }

        public IReceiveDamageable ObjectHasBeenBlowUp { get; }
    }
}