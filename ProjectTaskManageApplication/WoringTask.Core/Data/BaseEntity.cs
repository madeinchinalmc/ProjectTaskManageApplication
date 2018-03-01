using System;
using System.Collections.Generic;
using System.Text;

namespace WoringTask.Core.Data
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as BaseEntity);
        }

        public virtual bool Equals(BaseEntity other)
        {
            if (other == null || !(other is BaseEntity))
            {
                return false;
            }
            return (this == (BaseEntity)other);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(BaseEntity x, BaseEntity y)
        {
            if ((object)x == null && (object)y == null)
            {
                return true;
            }

            if ((object)x == null || (object)y == null)
            {
                return false;
            }

            return x.Id == y.Id;
        }

        public static bool operator !=(BaseEntity x, BaseEntity y)
        {
            return !(x == y);
        }
    }
}
