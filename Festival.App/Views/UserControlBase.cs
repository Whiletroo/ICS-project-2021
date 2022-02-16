using Festival.App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Festival.App.Views
{
    public abstract class UserControlBase : UserControl
    {
        protected UserControlBase()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is IViewModel viewModel)
            {
                viewModel.Load();
            }
        }
    }
}
