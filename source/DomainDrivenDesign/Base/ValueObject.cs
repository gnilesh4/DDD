using System.Collections.Generic;
using System.Linq;

namespace DomainDrivenDesign
{
    public abstract class ValueObject
    {
        public static bool operator !=(ValueObject a, ValueObject b)
        {
            return !(a == b);
        }

        public static bool operator ==(ValueObject a, ValueObject b)
        {
            if (a is null && b is null)
            {
                return true;
            }

            if (a is null || b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            return GetEquals().SequenceEqual((obj as ValueObject).GetEquals());
        }

        public override int GetHashCode()
        {
            return GetEquals().Aggregate(0, (index, obj) => { return index * 1000 + obj.GetHashCode(); });
        }

        protected abstract IEnumerable<object> GetEquals();
    }
}
