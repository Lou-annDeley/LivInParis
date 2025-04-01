using System;
using System.Collections.Generic;
using System.IO;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using SkiaSharp;

namespace LivInParis___DELPIERRE_DROUIN_DELEY
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = "MetroParis.xlsx"; // Nom du fichier Excel

            Graphe<string> metro = new Graphe<string>();
            Dictionary<int, Noeud<string>> noeuds = new Dictionary<int, Noeud<string>>();

            // Utilisation de ClosedXML pour lire le fichier Excel
            using (var workbook = new XLWorkbook(filePath))
            {
                var sheetNoeuds = workbook.Worksheet("Arcs");

                // Lecture des stations (Noeuds) à partir de la feuille Excel
                foreach (var row in sheetNoeuds.RowsUsed().Skip(1)) // Ignore la première ligne (titres)
                {
                    int id = row.Cell(1).GetValue<int>(); // ID de la station
                    string station = row.Cell(2).GetString(); // Nom de la station

                    Noeud<string> noeud = metro.AjouterSommet(station);
                    noeuds[id] = noeud;
                }

                // Lecture des connexions en utilisant les colonnes "Précédent" et "Suivant"
                var rows = sheetNoeuds.RowsUsed().ToList(); // Convertir en liste pour un accès par index

                for (int i = 1; i < rows.Count - 1; i++) // -1 pour éviter d'accéder hors limites
                {
                    var row = rows[i];
                    var nextRow = rows[i + 1]; // Récupérer la ligne suivante

                    int id = row.Cell(1).GetValue<int>();
                    int poids = nextRow.Cell(5).GetValue<int>(); // Poids de la ligne suivante
                    var suivantObj = row.Cell(4).Value;

                    if (!row.Cell(4).IsEmpty() && row.Cell(4).TryGetValue(out int suivantId))
                    {
                        if (noeuds.ContainsKey(suivantId) && noeuds.ContainsKey(id))
                        {
                            metro.AjouterLien(noeuds[id], noeuds[suivantId], poids);
                        }
                    }
                }
                foreach (var row in sheetNoeuds.RowsUsed().Skip(1)) // Ignore la première ligne (titres)
                {
                    if(!row.Cell(6).IsEmpty())
                    {
                        int id = row.Cell(1).GetValue<int>();
                        string nom_station = row.Cell(2).GetValue<string>();
                        int poids_correspondance = row.Cell(6).GetValue<int>();
                        foreach (var noeud in noeuds)
                        {
                            if(noeud.Value.Valeur == nom_station)
                            {
                                int id_correspodance = noeud.Key;
                                if (noeuds[id].ExisteLien(noeuds[id_correspodance]) == false)
                                {
                                    metro.AjouterLien(noeuds[id], noeuds[id_correspodance], poids_correspondance);
                                }
                            }
                        }
                    }
                }
            }

            // Afficher les stations et connexions
            Console.WriteLine("Stations et connexions :");
            foreach (var noeud in metro.Sommets)
            {
                Console.WriteLine($"{noeud.Valeur} :");
                foreach (var lien in noeud.Voisins)
                {
                    Console.WriteLine($" Lien entre {lien.Noeud1.Valeur} et {lien.Noeud2.Valeur} est de poids {lien.Poids}");
                }
            }

            //Dictionary<Noeud<string>, double> plus_petit_cheminS = metro.Dijkstra(metro.Sommets[0]);


            //foreach (var kvp in plus_petit_cheminS)
            //{
            //    Console.WriteLine($"Clé: {kvp.Key.Valeur}, Valeur: {kvp.Value}");
            //}

            List<Noeud<string>> plus_petit_cheminS2 = metro.Dijkstra2(metro.Sommets[0], metro.Sommets[3]);
            Console.WriteLine("Nouveau djikstra, le chemin est ");
            foreach (Noeud<string> noeud1 in plus_petit_cheminS2)
            {
                Console.Write(noeud1.Valeur+ " ");
            }
            Console.WriteLine();
            List<Noeud<string>> plus_petit_cheminS3 = metro.BellmanFord2(metro.Sommets[0], metro.Sommets[3]);
            Console.WriteLine("Nouveau bellman, le chemin est ");
            foreach (Noeud<string> noeud1 in plus_petit_cheminS3)
            {
                Console.Write(noeud1.Valeur + " ");
            }


            //Dictionary<Noeud<string>, double> plus_petit_cheminS_bellman = metro.BellmanFord(metro.Sommets[0]);


            //foreach (var bell in plus_petit_cheminS_bellman)
            //{
            //    Console.WriteLine($"Clé: {bell.Key.Valeur}, Valeur: {bell.Value}");
            //}

            // Dessiner le graphe et sauvegarder l'image
            metro.DessinerGraphe2("graphe.png");

        }
    }
}

