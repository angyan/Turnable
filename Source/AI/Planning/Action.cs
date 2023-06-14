using Turnable.Game;

namespace Turnable.AI.Planning;

internal record Action(string Name) : IAction
{
    public void Do(State state)
    {
        throw new NotImplementedException();
    }

    public void Undo(State state)
    {
        throw new NotImplementedException();
    }
}
