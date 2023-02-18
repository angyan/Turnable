using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turnable.Tiled
{
    internal record MapJsonString
    {
        internal string Value { get; init; }

        internal MapJsonString(string value)
        {
            if (!IsValid(value)) throw new ArgumentException($"{(value == null ? "null" : "\"null\"")} is not a valid value for constructing a JsonString");

            Value = value;
        }

        private bool IsValid(string value) => value != null && value != "null";
    }
}
