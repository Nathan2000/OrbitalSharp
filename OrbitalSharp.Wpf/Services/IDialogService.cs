using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbitalSharp.Wpf.Services
{
    public interface IDialogService
    {
        bool ShowDialog<TViewModel>(TViewModel viewModel);
    }
}
