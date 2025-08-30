using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbitalSharp.Manuevers
{
    public class PropagateAnomalyTo : TimeOperation
    {
        private double _meanAnomaly;

        public PropagateAnomalyTo(double meanAnomaly)
        {
            _meanAnomaly = meanAnomaly;
        }

        public override double GetTimeDelta(KeplerianElements orbit)
        {
            throw new NotImplementedException();
        }
    }
}
