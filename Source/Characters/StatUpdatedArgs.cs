namespace Turnable.Characters
{
    internal struct StatUpdatedArgs
    {
        internal int NewValue { get; }

        internal StatUpdatedArgs(int newValue) : this() => NewValue = newValue;
    }
}
