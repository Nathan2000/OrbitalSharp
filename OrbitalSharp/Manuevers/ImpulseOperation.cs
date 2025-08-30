using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbitalSharp.Manuevers
{
    public abstract class ImpulseOperation : Operation
    {
        public abstract double GetVelocityDelta(KeplerianElements orbit);
    }
}
