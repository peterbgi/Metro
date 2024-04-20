using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Metro.Messages;
using Metro.Models;
using Metro.Repositories;

namespace Metro.ViewModels
{
    public class TerkepViewModel : ObservableObject
    {
        private readonly MetroRepository _repository;

        public List<Allomas> Allomasok { get; }
        public List<Vonal> MetroVonalak { get; }

        private double _zoomX;
        public double ZoomX
        {
            get { return _zoomX; }
            set { SetProperty(ref _zoomX, value); }
        }

        private double _zoomY;
        public double ZoomY
        {
            get { return _zoomY; }
            set { SetProperty(ref _zoomY, value); }
        }

        public IRelayCommand<string> ZoomCommand { get; }


        public TerkepViewModel(MetroRepository repository)
        {
            _repository = repository;
            Allomasok = _repository.Allomasok;
            MetroVonalak = _repository.MetroVonalak;
            ZoomX = 1; ZoomY = 1;
            ZoomCommand = new RelayCommand<string>(Zoom);
            induloAllomas = true;
        }

        private void Zoom(string zoom)
        {
            zoom = zoom.Replace('.', ',');
            double.TryParse(zoom, out double num);
            // Alaphelyzetbe állítás
            if (num == 1)
            {
                ZoomX = 1;
                ZoomY = 1;
            }
            else if(ZoomX + num > 0 && ZoomY + num > 0)
            {
                ZoomX += num;
                ZoomY += num;
            }
        }
        // Alapértlemezetten TRUE
        private bool induloAllomas;
        public void SendMessage(string allomasNev)
        {
            var uzenet = new AllomasMessage(induloAllomas, allomasNev);
            // Üzenet küldés
            WeakReferenceMessenger.Default.Send(new AllomasValtozasMessage(uzenet));
            induloAllomas = !induloAllomas;
        }
    }
}
