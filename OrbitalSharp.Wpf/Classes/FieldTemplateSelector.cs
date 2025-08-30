using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace OrbitalSharp.Wpf.Classes
{
    public class FieldTemplateSelector : DataTemplateSelector
    {
        public required DataTemplate DoubleTemplate { get; set; }
        public required DataTemplate BodyTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is not DataField field)
            {
                return base.SelectTemplate(item, container);
            }

            if (field.ValueType.IsAssignableTo(typeof(double)))
            {
                return DoubleTemplate;
            }
            if (field.ValueType.IsAssignableTo(typeof(Body)))
            {
                return BodyTemplate;
            }
            throw new NotImplementedException();
        }
    }
}
