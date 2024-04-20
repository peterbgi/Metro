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
    /// Interaction logic for TerkepView.xaml
    /// </summary>
    public partial class TerkepView : UserControl
    {
        private TerkepViewModel viewModel;
        public TerkepView()
        {
            InitializeComponent();
            viewModel = App.Current.Services.GetService<TerkepViewModel>();
            DataContext = viewModel;
            DrawRailLines();
            DrawStations();
        }

        private void DrawRailLines()
        {
            SolidColorBrush sarga = new(Colors.Yellow);
            SolidColorBrush piros = new(Colors.Red);
            SolidColorBrush sotetkek = new(Colors.DarkBlue);
            SolidColorBrush zold = new(Colors.Green);
            foreach (var metroVonal in viewModel.MetroVonalak)
            {
                // Azért 1-ről indul,
                // mert a Dictionary-ben így lett hozzáadva legelső elem
                for (int i = 1; i < metroVonal.Allomasok.Count; i++)
                {
                    var start = metroVonal.Allomasok[i];
                    var veg = metroVonal.Allomasok[i + 1];

                    Line vonal = new()
                    {
                        X1 = start.X, Y1 = start.Y,
                        X2 = veg.X, Y2 = veg.Y,
                        StrokeThickness = 4
                    };

                    switch (metroVonal.VonalNev)
                    {
                        case "M1":
                            vonal.Stroke = sarga;
                            break;
                        case "M2":
                            vonal.Stroke = piros;
                            break;
                        case "M3":
                            vonal.Stroke = sotetkek;
                            break;
                        case "M4":
                            vonal.Stroke = zold;
                            break;
                    }

                    cnvTerkep.Children.Add(vonal);
                }
            }
        }

        private void DrawStations()
        {
            SolidColorBrush fekete = new(Colors.Black);
            SolidColorBrush szurke = new(Colors.LightGray);
            szurke.Opacity = 0.75;
            foreach (var allomas in viewModel.Allomasok)
            {
                Ellipse kor = new()
                {
                    Width = 10,
                    Height = 10,
                    Fill = fekete,
                    //Stroke = fekete,
                    //StrokeThickness = 2
                    ToolTip = allomas.AllomasNev
                };
                // A kör közepétől 5-t kell kivonni, mert 10 a szélessége
                Canvas.SetLeft(kor, allomas.X - 5);
                Canvas.SetTop(kor, allomas.Y - 5);
                cnvTerkep.Children.Add(kor);

                TextBlock szoveg = new()
                {
                    Text = allomas.AllomasNev,
                    MaxWidth = 100, 
                    TextWrapping = TextWrapping.Wrap,
                    FontSize = 8,
                    Background = szurke
                };
                Canvas.SetLeft(szoveg, allomas.X + 5);
                Canvas.SetTop(szoveg, allomas.Y + 2);
                Panel.SetZIndex(szoveg,3);
                cnvTerkep.Children.Add(szoveg);
            }
        }

        private void cnvTerkep_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point pont = e.GetPosition(cnvTerkep);
            foreach (var allomas in viewModel.Allomasok)
            {
                if (Math.Abs(allomas.X - pont.X) < 5 && Math.Abs(allomas.Y - pont.Y) < 5)
                {
                    viewModel.SendMessage(allomas.AllomasNev);
                    return;
                }
            }
        }
    }
}