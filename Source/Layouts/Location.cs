namespace Turnable.Layouts;

public record Location(Coordinate X, Coordinate Y)
{
    internal int DistanceTo(Location end, bool allowDiagonal) =>
        allowDiagonal ? Math.Max(Math.Abs(X - end.X), Math.Abs(Y - end.Y)) : Math.Abs(X - end.X) + Math.Abs(Y - end.Y);
};
