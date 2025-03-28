﻿using System;
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

            Dictionary<Noeud<string>, double> plus_petit_cheminS = metro.Dijkstra(metro.Sommets[0]);
            
            
            foreach (var kvp in plus_petit_cheminS)
            {
                Console.WriteLine($"Clé: {kvp.Key}, Valeur: {kvp.Value}");
            }
            

            // Dessiner le graphe et sauvegarder l'image
            metro.DessinerGraphe("graphe.png");
        }
    }
}

