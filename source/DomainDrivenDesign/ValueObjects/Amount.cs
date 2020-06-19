using System.Collections.Generic;

namespace DomainDrivenDesign
{
    public sealed class Amount : ValueObject
    {
        public Amount(decimal value)
        {
            Value = value;
        }

        public decimal Value { get; private set; }

        public override string ToString() => Value.ToString();

        protected override IEnumerable<object> GetEquals()
        {
            yield return Value;
        }
    }
}
