namespace Metro.Models
{
    public class Vonal
    {
        public string VonalNev { get; set; }
        public Dictionary<int, Allomas> Allomasok;
        public Vonal(string nev)
        {
            VonalNev = nev;
            Allomasok = new Dictionary<int, Allomas>();
        }
    }
}
