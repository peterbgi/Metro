using Metro.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Metro.Views
{
    /// <summary>
    /// Interaction logic for UtvonalView.xaml
    /// </summary>
    public partial class UtvonalView : UserControl
    {
        public UtvonalView()
        {
            DataContext = App.Current.Services.GetService<UtvonalViewModel>();
            InitializeComponent();
        }
    }
}
