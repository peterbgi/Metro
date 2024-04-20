using Metro.Repositories;

namespace Metro.Tests.Repositories
{
    [TestClass]
    [DeploymentItem(@"Data/metro.xlsx")]
    public class MetroRepositoryTests
    {
        [TestMethod("#1 Fájl létezik")]
        public void FileExists()
        {
            string myfile = "metro.xlsx";
            bool exists = File.Exists(myfile);
            Assert.IsTrue(exists);
        }

        [TestMethod("#3 Vonalak száma")]
        public void VonalTest()
        {
            var repo = new MetroRepository("metro.xlsx");
            int elvart = 4;
            int eredmeny = repo.MetroVonalak.Count;
            Assert.AreEqual(elvart, eredmeny);
        }

        [TestMethod("#2 Állomás terek száma")]
        public void AllomasTest()
        {
            var repo = new MetroRepository("metro.xlsx");
            var terMegallo = repo.Allomasok.FindAll(x => x.AllomasNev.Contains("tér")).Count;
            bool allitas = terMegallo < 5;
            Assert.IsFalse(allitas);
        }
    }
}
