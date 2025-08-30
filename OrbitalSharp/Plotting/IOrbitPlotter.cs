namespace OrbitalSharp.Plotting
{
    public interface IOrbitPlotter<T>
    {
        T Plot(KeplerianElements elements, string title = "", bool firstTime = true);
        void Export(string title = "");
    }
}
