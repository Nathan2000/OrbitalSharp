using OrbitalSharp.Wpf.Classes;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace OrbitalSharp.Wpf.ViewModel
{
    public class NewOrbitViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public string Title { get; set; } = "New Orbit";

        public List<DataField> Fields { get; private set; } = [];

        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AddDouble(string key, string label, double defaultValue, string description)
        {
            Fields.Add(DataField.CreateDoubleField(key, defaultValue, label, description));
        }

        public void AddBody(string key, string label, Body defaultValue, string description)
        {
            Fields.Add(DataField.CreateBodyField(key, defaultValue, label, description));
        }

        public double GetDouble(string key)
        {
            var field = Fields.SingleOrDefault(f => f.Key == key)
                ?? throw new KeyNotFoundException($"No field with key {key} was found.");
            return field.GetValue<double>();
        }

        public Body? GetBody(string key)
        {
            var field = Fields.SingleOrDefault(f => f.Key == key)
                ?? throw new KeyNotFoundException($"No field with key {key} was found.");
            return field.GetValue<Body>();
        }
    }
}
