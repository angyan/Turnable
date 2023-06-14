using Turnable.Game;

namespace Turnable.AI.Planning;

internal interface IAction
{
    string Name { get; }
    void Do(State state);
    void Undo(State state);

    //int Cost { get; }
    //bool IsPreconditionMet(State state);
    //State ApplyEffects(State state);
}
