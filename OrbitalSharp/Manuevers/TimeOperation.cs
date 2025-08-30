using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbitalSharp.Manuevers
{
    public abstract class TimeOperation : Operation
    {
        public abstract double GetTimeDelta(KeplerianElements orbit);
    }
}
