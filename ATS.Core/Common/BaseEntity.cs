using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Core
{
    public abstract class BaseEntity
    {

    }

    public abstract class Entity<T> : BaseEntity, IEntity<T>
    {
        public virtual T ID { get; set; }
    }



    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class IgnoreAudit : Attribute
    {
        
    }
}
