using OrbitalSharp.Plotting;
using ScottPlot;

namespace OrbitalSharp.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var plotter = new ScottOrbitPlotter();

            var orbit1 = KeplerianElements.WithPeriod(90 * 60, body: Body.Earth);
            plotter.Plot(orbit1, title: "Orbit 1");
            plotter.Export("orbit1");

            var molniya1 = KeplerianElements.WithPeriod(
                Constants.EarthSiderealDay / 2,
                e: 0.741,
                i: Utils.DegreesToRadians(63.4),
                argPe: Utils.DegreesToRadians(270),
                body: Body.Earth);
            plotter.Plot(molniya1, title: "Molniya 1");
            plotter.Export("molniya1");

            var orbit2 = KeplerianElements.WithAltitude(300 * 1000, body: Body.Earth);
            plotter.Plot(orbit2, title: "Orbit 2");
            plotter.Export("orbit2");

            var molniya2 = KeplerianElements.WithAltitude(
                508 * 1000,
                e: 0.741,
                i: Utils.DegreesToRadians(63.4),
                argPe: Utils.DegreesToRadians(270),
                body: Body.Earth);
            plotter.Plot(molniya2, title: "Molniya 2");
            plotter.Export("molniya2");

            var orbit3 = KeplerianElements.WithApsideAltitudes(
                1000 * 1000,
                400 * 1000,
                body: Body.Earth);
            plotter.Plot(orbit3, title: "Orbit 3");
            plotter.Export("orbit3");

            var molniya3 = KeplerianElements.WithApsideAltitudes(
                39873 * 1000,
                508 * 1000,
                i: Utils.DegreesToRadians(63.4),
                argPe: Utils.DegreesToRadians(270),
                body: Body.Earth);
            plotter.Plot(molniya3, title: "Molniya 3");
            plotter.Export("molniya3");

            var orbit4 = KeplerianElements.WithApsideRadii(7000 * 1000, 8400 * 1000, body: Body.Earth);
            plotter.Plot(orbit4, title: "Orbit 4");
            plotter.Export("orbit4");
        }
    }
}
