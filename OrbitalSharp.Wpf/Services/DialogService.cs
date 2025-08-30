using OrbitalSharp.Wpf.Controls;
using OrbitalSharp.Wpf.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OrbitalSharp.Wpf.Services
{
    public class DialogService(Window owner) : IDialogService
    {
        public bool ShowDialog<TViewModel>(TViewModel viewModel)
        {
            var window = ResolveWindowFor(viewModel);
            window.Owner = owner;
            window.DataContext = viewModel;
            return window.ShowDialog() == true;
        }

        private Window ResolveWindowFor<TViewModel>(TViewModel viewModel)
        {
            if (viewModel is NewOrbitViewModel newOrbitVm)
            {
                return new NewOrbitWindow();
            }

            throw new NotImplementedException();
        }
    }
}
