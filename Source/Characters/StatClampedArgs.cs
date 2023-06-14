namespace Turnable.Characters
{
    internal struct StatClampedArgs
    {
        internal int TriedValue { get; }

        internal StatClampedArgs(int triedValue) : this() => TriedValue = triedValue;
    }
}
