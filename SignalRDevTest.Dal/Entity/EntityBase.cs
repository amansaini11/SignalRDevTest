using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SignalRDevTest.Dal.Entity
{
    [Serializable]
    [DataContract(IsReference = true)]
    public abstract class EntityBase
    {
    }
}
