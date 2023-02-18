using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tests")]

namespace Turnable.Layouts;

internal readonly record struct Location(int X, int Y);
