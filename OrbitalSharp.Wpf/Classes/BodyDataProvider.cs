using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbitalSharp.Wpf.Classes
{
    public static class BodyDataProvider
    {
        public static List<Body> GetAllBodies() => [Body.Mercury, Body.Venus, Body.Earth, Body.Mars, Body.Jupiter, Body.Saturn, Body.Uranus, Body.Neptune];
    }
}
