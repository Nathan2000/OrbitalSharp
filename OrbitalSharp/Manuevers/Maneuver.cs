using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbitalSharp.Manuevers
{
    public class Maneuver
    {
        public List<Operation> Operations { get; set; }

        public Maneuver(Operation operation) : this([operation]) { }

        public Maneuver(List<Operation> operations)
        {
            Operations = operations;
        }

        public static Maneuver SetApocenterRadiusTo(double apocenterRadius)
        {
            var operations = new List<Operation>
            {
                new PropagateAnomalyTo(0),
                new SetApocenterRadiusTo(apocenterRadius)
            };
            return new Maneuver(operations);
        }

        //public static Maneuver SetPericenterRadiusTo(double pericenterRadius)
        //{
        //    var operations = new List<Operation>
        //    {
        //        new PropagateAnomalyTo(Math.PI),
        //        new SetPericenterRadiusTo(pericenterRadius)
        //    };
        //    return new Maneuver(operations);
        //}

        //public static Maneuver SetApocenterAltitudeTo(double apocenterAltitude)
        //{
        //    var operations = new List<Operation>
        //    {
        //        new PropagateAnomalyTo(0),
        //        new SetApocenterAltitudeTo(apocenterAltitude)
        //    };
        //    return new Maneuver(operations);
        //}

        //public static Maneuver SetPericenterAltitudeTo(double pericenterAltitude)
        //{
        //    var operations = new List<Operation>
        //    {
        //        new PropagateAnomalyTo(Math.PI),
        //        new SetPericenterAltitudeTo(pericenterAltitude)
        //    };
        //    return new Maneuver(operations);
        //}

        //public static Maneuver ChangeApocenterBy(double delta)
        //{
        //    var operations = new List<Operation>
        //    {
        //        new PropagateAnomalyTo(0),
        //        new ChangeApocenterBy(delta)
        //    };
        //    return new Maneuver(operations);
        //}

        //public static Maneuver ChangePericenterBy(double delta)
        //{
        //    var operations = new List<Operation>
        //    {
        //        new PropagateAnomalyTo(0),
        //        new ChangePericenterBy(delta)
        //    };
        //    return new Maneuver(operations);
        //}

        //public static Maneuver HohmannTransferToRadius(double radius)
        //{
        //    var operations = new List<Operation>
        //    {
        //        new SetPericenterHere(),
        //        new SetApocenterRadiusTo(radius),
        //        new PropagateAnomalyTo(Math.PI),
        //        new Circularize()
        //    };
        //    return new Maneuver(operations);
        //}

        //public static Maneuver HohmannTransferToAltitude(double altitude)
        //{
        //    var operations = new List<Operation>
        //    {
        //        new SetPericenterHere(),
        //        new SetApocenterAltitudeTo(altitude),
        //        new PropagateAnomalyTo(Math.PI),
        //        new Circularize()
        //    };
        //    return new Maneuver(operations);
        //}
    }
}
