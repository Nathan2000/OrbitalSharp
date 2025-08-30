using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OrbitalSharp.Wpf.Classes
{
    public class DataField : INotifyPropertyChanged
    {
        public string Key { get; set; }
        public Type ValueType { get; set; }

        private object? _value;
        public object? Value
        {
            get => _value;
            set
            {
                if (value is string str && ValueType != typeof(string))
                {
                    object parsed = Convert.ChangeType(str, ValueType);
                    _value = parsed;
                    OnPropertyChanged();
                    return;
                }

                if (value != null && !ValueType.IsInstanceOfType(value))
                    throw new ArgumentException($"Value must be of type {ValueType.Name}.");
                _value = value;
                OnPropertyChanged();
            }
        }

        private string _label;
        public string Label
        {
            get => _label;
            set
            {
                _label = value;
                OnPropertyChanged();
            }
        }

        private string? _description;

        public string? Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public DataField()
        {
            Key = string.Empty;
            ValueType = typeof(string);
            _label = string.Empty;
        }

        public DataField(string key, Type valueType, object? defaultValue = null, string? label = null, string? description = null)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            ValueType = valueType;
            Value = defaultValue ?? (valueType.IsValueType ? Activator.CreateInstance(valueType) : null);

            _label = label ?? key;
            _description = description;
        }

        public T? GetValue<T>()
        {
            if (!typeof(T).IsAssignableFrom(ValueType))
                throw new InvalidCastException($"Expected type {typeof(T).Name} but field is of type {ValueType.Name}.");
            return (T?)Value;
        }

        public void SetValue<T>(T value)
        {
            if (!typeof(T).IsAssignableFrom(ValueType))
                throw new InvalidCastException($"Expected type {ValueType.Name} but provided type is {typeof(T).Name}.");

            Value = value;
        }

        protected void OnPropertyChanged([CallerMemberName]string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static DataField CreateStringField(string key, string defaultValue = "", string? label = null, string? description = null)
            => new(key, typeof(string), defaultValue, label, description);

        public static DataField CreateIntField(string key, int defaultValue = 0, string? label = null, string? description = null)
            => new(key, typeof(int), defaultValue, label, description);

        public static DataField CreateDoubleField(string key, double defaultValue = 0.0, string? label = null, string? description = null)
            => new(key, typeof(double), defaultValue, label, description);

        public static DataField CreateBoolField(string key, bool defaultValue = false, string? label = null, string? description = null)
            => new(key, typeof(bool), defaultValue, label, description);

        public static DataField CreateDateTimeField(string key, DateTime? defaultValue = null, string? label = null, string? description = null)
            => new(key, typeof(DateTime), defaultValue ?? DateTime.Now, label, description);

        public static DataField CreateBodyField(string key, Body? defaultValue = null, string? label = null, string? description = null)
            => new(key, typeof(Body), defaultValue ?? Body.Earth, label, description);

        // TODO: Add other types of fields, like sliders (for 0-1 values and angles)
        //public static DataField CreateDoubleSliderField(string key, double defaultValue = 0.0, string? label = null, string? description = null)
        //    => new(key, typeof(double), defaultValue, label, description);
    }
}
