using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turnable.Tiled
{
    internal record MapFilePath
    {
        internal string Value { get; init; }
        internal ImmutableArray<string> SupportedExtensions = new[] { ".json", ".JSON", ".tmj", ".TMJ" }.ToImmutableArray();

        internal MapFilePath(string value)
        {
            if (!IsValid(value)) throw new ArgumentException($"{Path.GetExtension(value)} is not a supported file extension for a Tiled Map");

            Value = value;
        }

        private bool IsValid(string value) => SupportedExtensions.Contains(Path.GetExtension(value));
    }
}
