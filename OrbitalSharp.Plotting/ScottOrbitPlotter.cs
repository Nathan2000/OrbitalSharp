using ScottPlot;
using ScottPlot.Plottables;

namespace OrbitalSharp.Plotting
{
    public class ScottOrbitPlotter : IOrbitPlotter<Plot>
    {
        private const double Kilo = 1000.0;

        private Plot _plot;
        private Marker? _spaceship;

        private static readonly Color OrbitColor = Colors.DarkGray;

        public ScottOrbitPlotter() : this(new Plot())
        {
        }

        public ScottOrbitPlotter(Plot plot)
        {
            _plot = plot;
        }

        public Plot Plot(KeplerianElements orbit, string? title = null, bool firstTime = true)
        {
            if (orbit == null)
                return _plot;

            if (firstTime)
            {
                _plot.Clear();
                _plot.Title(title);
                _plot.XLabel("X (km)");
                _plot.YLabel("Y (km)");
                _plot.ShowLegend();

                PlotBody(orbit.body);
                PlotOrbit(orbit, label: "Initial orbit");
                PlotMarkers(orbit);
                _spaceship = PlotSpacecraft(orbit);
                _plot.Axes.AutoScale();
                _plot.Axes.SquareUnits(true);
            }
            else
            {
                if (_spaceship is not null)
                {
                    UpdateSpacecraft(orbit, _spaceship);
                }
            }
            return _plot;
        }

        public void Export(string? title = null)
        {
            string filename = string.Format("{0}.{1}", title ?? "plot", "png");
            _plot.SavePng(filename, 600, 600);
        }

        private void PlotBody(Body body)
        {
            if (body.AtmosphereThickness > 0)
            {
                var atmosphere = _plot.Add.Circle(Coordinates.Zero, body.MeanRadius / Kilo + body.AtmosphereThickness);
                var atmoColor = Color.FromHtml(body.AtmosphereColor ?? "#8da4ff");
                var nearAtmoColor = atmoColor.WithAlpha(1f);
                var farAtmoColor = atmoColor.WithAlpha(0f);
                atmosphere.FillColor = nearAtmoColor;
                float startPoint = (float)(body.MeanRadius / Kilo / (body.MeanRadius / Kilo + body.AtmosphereThickness));
                atmosphere.FillHatch = new Gradient(GradientType.Radial)
                {
                    ColorPositions = [startPoint, 1f],
                    Colors = [nearAtmoColor, farAtmoColor],
                };
                atmosphere.LineWidth = 0;
            }

            var planet = _plot.Add.Circle(Coordinates.Zero, body.MeanRadius / Kilo);
            planet.FillColor = Color.FromHtml(body.PlotColor ?? "#ebebeb");
            planet.LineWidth = 0;
        }

        private void PlotOrbit(KeplerianElements orbit, string label = "")
        {
            double a = orbit.a / Kilo;
            double b = orbit.a * Math.Sqrt(1 - Math.Pow(orbit.e, 2)) / Kilo;
            double cx = -orbit.a * orbit.e * Math.Cos(orbit.argPe) / Kilo;
            double cy = -orbit.a * orbit.e * Math.Sin(orbit.argPe) / Kilo;
            var ellipse = _plot.Add.Ellipse(cx, cy, a, b, Angle.FromRadians(orbit.argPe));
            ellipse.LegendText = label;
            ellipse.LinePattern = LinePattern.Dashed;
            ellipse.LineWidth = 2;
            ellipse.LineColor = OrbitColor;
        }

        private Marker PlotSpacecraft(KeplerianElements orbit)
        {
            double r = orbit.a * (1 - Math.Pow(orbit.e, 2)) / (1 + orbit.e * Math.Cos(orbit.f));
            double x = r * Math.Cos(orbit.f) / Kilo;
            double y = r * Math.Sin(orbit.f) / Kilo;
            double xRot = x * Math.Cos(orbit.argPe) - y * Math.Sin(orbit.argPe);
            double yRot = x * Math.Sin(orbit.argPe) + y * Math.Cos(orbit.argPe);
            return _plot.Add.Marker(xRot, yRot, MarkerShape.FilledCircle, 5, Colors.Red);
        }

        private void UpdateSpacecraft(KeplerianElements orbit, Marker spacecraft)
        {
            double r = orbit.a * (1 - Math.Pow(orbit.e, 2)) / (1 + orbit.e * Math.Cos(orbit.f));
            double x = r * Math.Cos(orbit.f) / Kilo;
            double y = r * Math.Sin(orbit.f) / Kilo;
            double xRot = x * Math.Cos(orbit.argPe) - y * Math.Sin(orbit.argPe);
            double yRot = x * Math.Sin(orbit.argPe) + y * Math.Cos(orbit.argPe);
            spacecraft.X = xRot;
            spacecraft.Y = yRot;
        }

        private void PlotMarkers(KeplerianElements orbit)
        {
            double rPer = orbit.a * (1 - orbit.e);
            double xPer = rPer * Math.Cos(orbit.argPe) / Kilo;
            double yPer = rPer * Math.Sin(orbit.argPe) / Kilo;

            _plot.Add.Marker(xPer, yPer, MarkerShape.FilledTriangleUp, 10, OrbitColor);
            _plot.Add.Text("P", xPer, yPer);

            double rApo = orbit.a * (1 + orbit.e);
            double xApo = rApo * Math.Cos(orbit.argPe + Math.PI) / Kilo;
            double yApo = rApo * Math.Sin(orbit.argPe + Math.PI) / Kilo;

            _plot.Add.Marker(xApo, yApo, MarkerShape.OpenTriangleDown, 10, OrbitColor);
            _plot.Add.Text("A", xApo, yApo);

            // TODO: Add markers for ascending and descending nodes
        }
    }
}
