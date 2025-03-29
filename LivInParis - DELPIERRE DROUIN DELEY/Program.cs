using System;
using System.Collections.Generic;
using System.IO;
using ClosedXML.Excel;
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
                foreach (var row in sheetNoeuds.RowsUsed().Skip(1)) // Ignore la première ligne (titres)
                {
                    int id = row.Cell(1).GetValue<int>();
                    var precedentObj = row.Cell(3).Value; // Colonne Précédent
                    var suivsantObj = row.Cell(4).Value; // Colonne Suivant

                    // Si la valeur du précédent est valide, on crée un lien
                    // Vérifie si la cellule contient un nombre avant la conversion
                    if (!row.Cell(3).IsEmpty() && row.Cell(3).TryGetValue(out int precedentId))
                    {
                        if (noeuds.ContainsKey(precedentId) && noeuds.ContainsKey(id))
                        {
                            metro.AjouterLien(noeuds[precedentId], noeuds[id], 2.0);
                        }
                    }

                    if (!row.Cell(4).IsEmpty() && row.Cell(4).TryGetValue(out int suivantId))
                    {
                        if (noeuds.ContainsKey(suivantId) && noeuds.ContainsKey(id))
                        {
                            metro.AjouterLien(noeuds[id], noeuds[suivantId], 2.0);
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
                    Console.WriteLine($"  - Connecté à {lien.Noeud2.Valeur} avec poids {lien.Poids}");
                }
            }

            // Dessiner le graphe et sauvegarder l'image
            metro.DessinerGraphe("graphe.png");
        }
    }
}

