using System.Collections.Generic;

namespace DomainDrivenDesign
{
    public sealed class Quantity : ValueObject
    {
        public Quantity(decimal value)
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
