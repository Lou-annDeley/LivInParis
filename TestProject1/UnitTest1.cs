using DocumentFormat.OpenXml.Bibliography;
using LivInParis___DELPIERRE_DROUIN_DELEY;
using NuGet.Frameworks;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Testdjikstra()
        {
            Graphe<string> metro = new Graphe<string>();
            string station = "Chatelet";
            string station2 = "Liège";
            Noeud<string> noeud = metro.AjouterSommet(station);
            List<Noeud<string>> plus_petit_cheminS2 = metro.Dijkstra2(metro.Sommets[0], metro.Sommets[1]);
            Assert.Equals(noeud, plus_petit_cheminS2);

        }
    }
}