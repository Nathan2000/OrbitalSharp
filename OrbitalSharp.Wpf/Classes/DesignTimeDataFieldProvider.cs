using ScottPlot.ArrowShapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbitalSharp.Wpf.Classes
{
    public static class DesignTimeDataFieldProvider
    {
        public static List<DataField> GetSampleDataFields()
        {
            return [
                DataField.CreateDoubleField("double", 5400.0, "Double Field"),
                DataField.CreateBodyField("body", Body.Earth, "Body Dropdown")
            ];
        }
    }
}
