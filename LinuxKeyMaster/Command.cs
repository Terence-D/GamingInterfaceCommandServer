namespace GIC.Linux.KeyMaster
{
    public class Command
    {
        public const int KEY_DOWN = 0;
        public const int KEY_UP = 1;

        public string Key { get; set; }
        public string[] Modifier { get; set; }
        public int ActivatorType { get; set; }
    }
}
