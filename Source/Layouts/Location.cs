using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tests")]

namespace Turnable.Layouts;

public readonly record struct Location(Coordinate X, Coordinate Y);
