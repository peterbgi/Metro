using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Metro.Messages;
using Metro.Models;
using Metro.Repositories;
using System.Collections.ObjectModel;

namespace Metro.ViewModels
{
    public class UtvonalViewModel : ObservableObject
    {
        private readonly MetroRepository _repository;

        // A ComboBoxban kijelölt elem
        private string? _indulas;
        public string? Indulas
        {
            get { return _indulas; }
            set { SetProperty(ref _indulas, value); }
        }
        // A ComboBoxban kijelölt elem
        private string? _erkezes;
        public string? Erkezes
        {
            get { return _erkezes; }
            set { SetProperty(ref _erkezes, value); }
        }

        // Ezt töltjük a ComboBoxokba
        public List<Allomas> Allomasok { get; }
        // Útvonalterv listája
        public ObservableCollection<string> UtvonalTerv { get; set; }
        // Gombok
        public IRelayCommand TervezesCommand { get; }
        public IRelayCommand KiuritesCommand { get; }

        public UtvonalViewModel(MetroRepository repository)
        {
            _repository = repository;
            Allomasok = _repository.Allomasok;
            UtvonalTerv = new ObservableCollection<string>();
            KiuritesCommand = new RelayCommand(Kiurites);
            TervezesCommand = new RelayCommand(Tervezes);
            RegisterMessages();
        }

        private void RegisterMessages()
        {
            WeakReferenceMessenger.Default.Register<AllomasValtozasMessage>(this, (r, m) => 
            {
                if (m.Value.Indulo)
                {
                    Indulas = m.Value.AllomasNev;
                }
                else
                {
                    Erkezes = m.Value.AllomasNev;
                }
            });
        }

        private void Kiurites()
        {
            UtvonalTerv.Clear();
            Indulas = null;
            Erkezes = null;
        }

        private void Tervezes()
        {
            UtvonalTerv.Clear();
            UtvonalTerv.Add($"Indulás innen: {Indulas}");
            UtvonalTerv.Add($"Érkezés ide: {Erkezes}");
            UtvonalTerv.Add($"------------------------------");
            Vonal? induloVonal = null, vegVonal = null;
            foreach (var vonal in _repository.MetroVonalak)
            {
                bool indulasLetezik = _repository.VonalonLetezik(vonal, Indulas);
                bool erkezesLetezik = _repository.VonalonLetezik(vonal, Erkezes);
                if (indulasLetezik)
                {
                    induloVonal = vonal;
                }
                if (erkezesLetezik)
                {
                    vegVonal = vonal;
                }
                // Ha meg van mindkét vonal
                if (induloVonal != null && vegVonal != null)
                {
                    break;
                }
            }
            if (induloVonal != null && vegVonal != null)
            {
                // Ha mindkét állomás ugyanazon a vonalon van
                if (induloVonal == vegVonal)
                {
                    UtvonalTerv.Add($"Induljon el átszállás nélkül az {induloVonal.VonalNev} vonalon.");
                    return;
                }
            }
            // Ha a két állomás két külön vonalon van
            foreach (var allomas in induloVonal.Allomasok)
            {
                string allomasNev = allomas.Value.AllomasNev;
                bool vanAtszallas = _repository.VonalonLetezik(vegVonal, allomasNev);
                // Ha van közvetlen átszállás
                if (vanAtszallas)
                {
                    string indulovonal = induloVonal.VonalNev;
                    string vegvonal = vegVonal.VonalNev;
                    UtvonalTerv.Add($"Induljon el az {indulovonal} vonalon");
                    UtvonalTerv.Add($"szálljon át a(z) {allomasNev} állomáson");
                    UtvonalTerv.Add($"az {vegvonal} vonalra");
                    return;
                }
                // Ha nincs közvetlen átszállás
                else
                {
                    List<Vonal> kulsoVonalak = new();
                    kulsoVonalak.AddRange(_repository.MetroVonalak);
                    kulsoVonalak.Remove(induloVonal);
                    kulsoVonalak.Remove(vegVonal);
                    foreach (var vonal in kulsoVonalak)
                    {
                        foreach (var kozosAllomas in vonal.Allomasok)
                        {
                            string jelenlegiAllomas = kozosAllomas.Value.AllomasNev;
                            bool induloVonalKozos = _repository.VonalonLetezik(vonal, allomasNev);
                            bool vegVonalKozos = _repository.VonalonLetezik(vegVonal, jelenlegiAllomas);
                            // Ha csak egyszer kell átszállni
                            if (induloVonalKozos && vegVonalKozos)
                            {
                                UtvonalTerv.Add($"Induljon el az {induloVonal.VonalNev} vonalon");
                                UtvonalTerv.Add($"szálljon át a(z) {allomasNev} állomáson");
                                UtvonalTerv.Add($"az {vonal.VonalNev} vonalra");
                                UtvonalTerv.Add($"szálljon át a(z) {jelenlegiAllomas} állomáson");
                                UtvonalTerv.Add($"az {vegVonal.VonalNev} vonalra");
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
}
