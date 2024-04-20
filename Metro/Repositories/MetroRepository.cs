using ClosedXML.Excel;
using Metro.Models;

namespace Metro.Repositories
{
    public class MetroRepository
    {
        private readonly string _filePath;
        public List<Vonal> MetroVonalak { get; }
        public List<Allomas> Allomasok { get; }

        public MetroRepository(string filePath = "Data/metro.xlsx")
        {
            _filePath = filePath;
            MetroVonalak = new();
            Allomasok = new();
            LoadData();
        }

        private void LoadData()
        {
            using (var workBook = new XLWorkbook(_filePath))
            {
                // Beolvassa legelső munkalapot
                var munkalap = workBook.Worksheet(1);
                // A KITÖLTÖTT sorok száma
                int sorokSzama = munkalap.RowsUsed().Count();
                for (int sor = 2; sor <= sorokSzama; sor++)
                {
                    string nev = munkalap.Cell(sor, 1).GetValue<string>();
                    string x = munkalap.Cell(sor, 2).GetValue<string>();
                    string y = munkalap.Cell(sor, 3).GetValue<string>();
                    Allomasok.Add(new Allomas(nev, x, y));
                }
                // Vonalak beolvasása
                munkalap = workBook.Worksheet(2);
                sorokSzama = munkalap.RowsUsed().Count();
                int oszlopokSzama = munkalap.ColumnsUsed().Count();
                for (int sor = 2; sor <= sorokSzama; sor++)
                {
                    string nev = munkalap.Cell(sor, 1).GetValue<string>();
                    var vonal = new Vonal(nev);
                    int megalloSzam = 1;
                    for (int oszlop = 2; oszlop <= oszlopokSzama; oszlop++)
                    {
                        string allomasNev = munkalap.Cell(sor, oszlop).GetValue<string>();
                        // Állomás kikeresése név alapján
                        var allomas = Allomasok.SingleOrDefault(x => x.AllomasNev == allomasNev);
                        if (allomas != null)
                        {
                            vonal.Allomasok.Add(megalloSzam, allomas);
                            megalloSzam++;
                        }
                    }
                    MetroVonalak.Add(vonal);
                }
            }
        }

        public bool VonalonLetezik(Vonal vonal, string allomasNev)
        {
            return vonal.Allomasok.Any(x => x.Value.AllomasNev == allomasNev);
        }
    }
}
