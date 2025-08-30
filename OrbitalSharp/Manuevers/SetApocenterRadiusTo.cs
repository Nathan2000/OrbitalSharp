using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbitalSharp.Manuevers
{
    public class SetApocenterRadiusTo : ImpulseOperation
    {
        private double _apocenterRadius;

        public SetApocenterRadiusTo(double radius)
        {
            _apocenterRadius = radius;
        }

        public override double GetVelocityDelta(KeplerianElements orbit)
        {
            throw new NotImplementedException();
            // Get velocity at pericenter
/*            orbit.p

            (double a, double e) = Utils.ElementsForApsides(_apocenterRadius, pericenterRadius);
*/            
        }
    }
}
