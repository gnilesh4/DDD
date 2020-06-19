using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DomainDrivenDesign
{
    public sealed class Email : ValueObject
    {
        public Email(string value)
        {
            Validate(value);
            Value = value;
        }

        public string Value { get; private set; }

        public override string ToString() => Value;

        protected override IEnumerable<object> GetEquals()
        {
            yield return Value;
        }

        private void Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(nameof(value));
            }

            const string regex = @"^([a-z0-9_\.\-]{3,})@([\da-z\.\-]{3,})\.([a-z\.]{2,6})$";

            if (!new Regex(regex).IsMatch(value))
            {
                throw new ArgumentException(nameof(value));
            }
        }
    }
}
