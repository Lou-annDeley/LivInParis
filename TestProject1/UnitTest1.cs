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
            Noeud<string> noeud2 = metro.AjouterSommet(station2);
            metro.AjouterLien(noeud, noeud2,2);
            List<Noeud<string>> plus_petit_cheminS2 = metro.Dijkstra2(noeud, noeud2);
            string chemin = "";
            foreach (Noeud<string> c in plus_petit_cheminS2)
            {
                chemin += c.Valeur+" ";
            }
            Assert.AreEqual("Chatelet Liège ", chemin);

        }

        [TestMethod]
        public void TestDijkstrapluslong()
        {
            Graphe<string> metro = new Graphe<string>();

            Noeud<string> A = metro.AjouterSommet("Chatelet");
            Noeud<string> B = metro.AjouterSommet("République");
            Noeud<string> C = metro.AjouterSommet("Bastille");
            Noeud<string> D = metro.AjouterSommet("Gare de Lyon");
            Noeud<string> E = metro.AjouterSommet("Liège");

            metro.AjouterLien(A, B, 2);
            metro.AjouterLien(A, C, 5);
            metro.AjouterLien(B, D, 3);
            metro.AjouterLien(C, D, 1);
            metro.AjouterLien(D, E, 4);

            
            List<Noeud<string>> plus_petit_chemin = metro.Dijkstra2(A, E);

            string chemin = "";
            foreach (Noeud<string> c in plus_petit_chemin)
            {
                chemin += c.Valeur + " ";
            }
            Assert.AreEqual("Chatelet République Gare de Lyon Liège ", chemin);
        }

        [TestMethod]
        public void TestDijkstramemedepartarrivee()
        {
            Graphe<string> metro = new Graphe<string>();

            Noeud<string> A = metro.AjouterSommet("Chatelet");

            List<Noeud<string>> plus_petit_chemin = metro.Dijkstra2(A, A);

            string chemin = "";
            foreach (Noeud<string> c in plus_petit_chemin)
            {
                chemin += c.Valeur + " ";
            }

            Assert.AreEqual("Chatelet ", chemin);
        }

        [TestMethod]
        public void Testbellman()
        {
            Graphe<string> metro = new Graphe<string>();
            string station = "Chatelet";
            string station2 = "Liège";
            Noeud<string> noeud = metro.AjouterSommet(station);
            Noeud<string> noeud2 = metro.AjouterSommet(station2);
            metro.AjouterLien(noeud, noeud2, 2);
            List<Noeud<string>> plus_petit_cheminS2 = metro.BellmanFord2(noeud, noeud2);
            string chemin = "";
            foreach (Noeud<string> c in plus_petit_cheminS2)
            {
                chemin += c.Valeur + " ";
            }
            Assert.AreEqual("Chatelet Liège ", chemin);

        }

        [TestMethod]
        public void Testbellman_pluslong()
        {
            Graphe<string> metro = new Graphe<string>();

            Noeud<string> A = metro.AjouterSommet("Chatelet");
            Noeud<string> B = metro.AjouterSommet("République");
            Noeud<string> C = metro.AjouterSommet("Bastille");
            Noeud<string> D = metro.AjouterSommet("Gare de Lyon");
            Noeud<string> E = metro.AjouterSommet("Liège");

            metro.AjouterLien(A, B, 2);
            metro.AjouterLien(A, C, 5);
            metro.AjouterLien(B, D, 3);
            metro.AjouterLien(C, D, 1);
            metro.AjouterLien(D, E, 4);


            List<Noeud<string>> plus_petit_chemin = metro.BellmanFord2(A, E);

            string chemin = "";
            foreach (Noeud<string> c in plus_petit_chemin)
            {
                chemin += c.Valeur + " ";
            }
            Assert.AreEqual("Chatelet République Gare de Lyon Liège ", chemin);
        }

        [TestMethod]
        public void TestBellman_memedepartarrivee()
        {
            Graphe<string> metro = new Graphe<string>();

            Noeud<string> A = metro.AjouterSommet("Chatelet");

            List<Noeud<string>> plus_petit_chemin = metro.Dijkstra2(A, A);

            string chemin = "";
            foreach (Noeud<string> c in plus_petit_chemin)
            {
                chemin += c.Valeur + " ";
            }

            Assert.AreEqual("Chatelet ", chemin);
        }
    }
}