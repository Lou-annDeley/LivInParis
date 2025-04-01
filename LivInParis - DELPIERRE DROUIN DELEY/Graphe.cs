using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using SkiaSharp;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Runtime.CompilerServices;

namespace LivInParis___DELPIERRE_DROUIN_DELEY
{
    public class Graphe<T>
    {
        public List<Noeud<T>> Sommets { get; }  // Liste des nœuds du graphe

        /// <summary>
        /// Constructeur Graphe
        /// </summary>
        public Graphe()
        {
            Sommets = new List<Noeud<T>>();
        }

        /// <summary>
        /// Fonction qui ajoute un sommet au graphe
        /// </summary>
        /// <param name="valeur"></param>
        /// <returns></returns>
        public Noeud<T> AjouterSommet(T valeur)
        {
            Noeud<T> noeud = new Noeud<T>(valeur);
            Sommets.Add(noeud);
            return noeud;
        }

        /// <summary>
        /// Fonction qui ajoute un lien entre deux noeuds
        /// </summary>
        /// <param name="noeud1"></param>
        /// <param name="noeud2"></param>
        /// <param name="poids"></param>
        public void AjouterLien(Noeud<T> noeud1, Noeud<T> noeud2, double poids)
        {
            Lien<T> lien = new Lien<T>(noeud1, noeud2, poids);
            noeud1.Voisins.Add(lien);
            noeud2.Voisins.Add(lien);
        }


        /// <summary>
        /// Dessin du graphe
        /// </summary>
        /// <param name="outputPath"></param>
        /// 
        public void DessinerGraphe(string outputPath)
        {
            int width = 3000, height = 2000; // Taille de l'image
            var bitmap = new SKBitmap(width, height);
            var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.White);

            var paintEdge = new SKPaint { Color = SKColors.Black, StrokeWidth = 4 };
            var fontPaint = new SKPaint { Color = SKColors.White, TextSize = 50, IsAntialias = true };

            Dictionary<Noeud<T>, SKPoint> positions = new Dictionary<Noeud<T>, SKPoint>();

            // Charger les données depuis le fichier Excel
            int k = 0;
            using (var workbook = new XLWorkbook("MetroParis.xlsx"))
            {
                var sheetNoeuds = workbook.Worksheet("Noeuds");

                // Lecture des stations (Noeuds) à partir de la feuille Excel
                foreach (var row in sheetNoeuds.RowsUsed().Skip(1)) // Ignore la première ligne (titres)
                {
                    double latitude = 0;
                    double longitude = 0;

                    // Essayer de récupérer la latitude
                    if (!row.Cell(5).TryGetValue<double>(out latitude))
                    {
                        Console.WriteLine($"Erreur de conversion de la latitude pour la station {row.Cell(2).GetString()}");
                        continue; // Si la conversion échoue, passer à la ligne suivante
                    }

                    // Essayer de récupérer la longitude
                    if (!row.Cell(4).TryGetValue<double>(out longitude))
                    {
                        Console.WriteLine($"Erreur de conversion de la longitude pour la station {row.Cell(2).GetString()}");
                        continue; // Si la conversion échoue, passer à la ligne suivante
                    }

                    Console.WriteLine($"Latitude: {latitude}, Longitude: {longitude}");

                    // Assigner la latitude et la longitude aux nœuds
                    Sommets[k].Latitude = latitude;
                    Console.WriteLine("ATTENTION" + latitude);
                    Sommets[k].Longitude = longitude;
                    k++;
                }
            }

            double minLat = 48.819106595610265;
            double maxLat = 48.897802691407826;
            double minLong = 2.2570461929221497;
            double maxLong = 2.4405400954061127;

            // Calculer les positions des nœuds (les convertir en pixels en fonction des latitudes et longitudes)
            foreach (var noeud in Sommets)
            {
                float x = (float)((noeud.Longitude - minLong) / (maxLong - minLong) * width);
                float y = (float)((noeud.Latitude - minLat) / (maxLat - minLat) * height);
                positions[noeud] = new SKPoint(x, y);
            }

            // Dessiner les arêtes (liens entre les nœuds)
            foreach (var noeud in Sommets)
            {
                foreach (var lien in noeud.Voisins)
                {
                    Noeud<T> voisin = lien.Noeud2;
                    SKPoint point1 = positions[noeud];
                    SKPoint point2 = positions[voisin];
                    canvas.DrawLine(point1, point2, paintEdge); // Dessiner le lien entre les nœuds
                }
            }

            // Dessiner les nœuds (stations)
            foreach (var noeud in Sommets)
            {
                SKPoint pos = positions[noeud];
                var paintNode = new SKPaint { Color = SKColors.Red, Style = SKPaintStyle.Fill };
                canvas.DrawCircle(pos, 15, paintNode); // Dessiner le cercle représentant le nœud
                canvas.DrawText(noeud.Valeur.ToString(), pos.X - 5, pos.Y + 5, fontPaint); // Texte du nœud
            }

            // Sauvegarder l'image
            using (var image = SKImage.FromBitmap(bitmap))
            using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
            using (var stream = File.OpenWrite(outputPath))
            {
                data.SaveTo(stream);
            }
        }



        /// <summary>
        /// Algo du plus petit chemin : Dijkstra
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        //public Dictionary<Noeud<T>, double> Dijkstra(Noeud<T> source)
        //{
        //    // Dictionnaire des distances depuis la source
        //    Dictionary<Noeud<T>, double> distances = new Dictionary<Noeud<T>, double>();
        //    Dictionary<Noeud<T>, Noeud<T>> predecesseurs = new Dictionary<Noeud<T>, Noeud<T>>();
        //    HashSet<Noeud<T>> nonVisites = new HashSet<Noeud<T>>(Sommets);

        //    // Initialisation : toutes les distances à l'infini sauf la source
        //    foreach (var noeud in Sommets)
        //    {
        //        distances[noeud] = double.PositiveInfinity;
        //    }
        //    distances[source] = 0;

        //    while (nonVisites.Count > 0)
        //    {
        //        // Sélectionner le nœud avec la plus petite distance
        //        Noeud<T> noeudActuel = nonVisites.OrderBy(n => distances[n]).First();
        //        nonVisites.Remove(noeudActuel);

        //        // Parcourir ses voisins
        //        foreach (var lien in noeudActuel.Voisins)
        //        {
        //            Noeud<T> voisin = lien.Noeud1.Equals(noeudActuel) ? lien.Noeud2 : lien.Noeud1;
        //            if (!nonVisites.Contains(voisin)) continue;

        //            double nouvelleDistance = distances[noeudActuel] + lien.Poids;
        //            if (nouvelleDistance < distances[voisin])
        //            {
        //                distances[voisin] = nouvelleDistance;
        //                predecesseurs[voisin] = noeudActuel;
        //            }
        //        }
        //    }

        //    return distances;
        //}

        public List<Noeud<T>> Dijkstra2(Noeud<T> depart, Noeud<T> arrivee)
        {
            Dictionary<Noeud<T>, double> distances = new Dictionary<Noeud<T>, double>();
            Dictionary<Noeud<T>, Noeud<T>> predecesseurs = new Dictionary<Noeud<T>, Noeud<T>>();

            foreach (var noeud in Sommets)
            {
                distances[noeud] = double.PositiveInfinity;
                predecesseurs[noeud] = null;
            }
            distances[depart] = 0;

            var filePriorite = new SortedSet<Noeud<T>>(Comparer<Noeud<T>>.Create((a, b) =>
            {
                return distances[a].CompareTo(distances[b]);
            }));

            foreach (var noeud in Sommets)
            {
                filePriorite.Add(noeud);
            }

            while (filePriorite.Count > 0)
            {
                Noeud<T> noeudActuel = filePriorite.Min;
                filePriorite.Remove(noeudActuel);

                if (noeudActuel.Equals(arrivee))
                {
                    break;
                }

                foreach (var lien in noeudActuel.Voisins)
                {
                    Noeud<T> voisin = lien.Noeud1.Equals(noeudActuel) ? lien.Noeud2 : lien.Noeud1;
                    double nouvelleDistance = distances[noeudActuel] + lien.Poids;

                    if (nouvelleDistance < distances[voisin])
                    {
                        distances[voisin] = nouvelleDistance;
                        predecesseurs[voisin] = noeudActuel;
                        filePriorite.Remove(voisin);
                        filePriorite.Add(voisin);
                    }
                }
            }

            List<Noeud<T>> chemin = new List<Noeud<T>>();
            Noeud<T> noeudCourant = arrivee;
            while (noeudCourant != null)
            {
                chemin.Insert(0, noeudCourant);
                noeudCourant = predecesseurs[noeudCourant];
            }

            return chemin; // Retourne uniquement la liste des nœuds empruntés
        }





        /// <summary>
        /// Algo du plus petit chemin : BellmanFord
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        //public Dictionary<Noeud<T>, double> BellmanFord(Noeud<T> source)
        //{
        //    // Dictionnaire des distances depuis la source
        //    Dictionary<Noeud<T>, double> distances = new Dictionary<Noeud<T>, double>();
        //    Dictionary<Noeud<T>, Noeud<T>> predecesseurs = new Dictionary<Noeud<T>, Noeud<T>>();

        //    // Initialisation : toutes les distances à l'infini sauf la source
        //    foreach (var noeud in Sommets)
        //    {
        //        distances[noeud] = double.PositiveInfinity;
        //        predecesseurs[noeud] = null;
        //    }
        //    distances[source] = 0;

        //    // Boucle principale de Bellman-Ford
        //    int n = Sommets.Count;
        //    for (int i = 0; i < n - 1; i++)
        //    {
        //        foreach (var noeud in Sommets)
        //        {
        //            foreach (var lien in noeud.Voisins)
        //            {
        //                Noeud<T> voisin = lien.Noeud1.Equals(noeud) ? lien.Noeud2 : lien.Noeud1;
        //                double nouvelleDistance = distances[noeud] + lien.Poids;

        //                if (nouvelleDistance < distances[voisin])
        //                {
        //                    distances[voisin] = nouvelleDistance;
        //                    predecesseurs[voisin] = noeud;
        //                }
        //            }
        //        }
        //    }

        //    // Vérification des cycles de poids négatif
        //    foreach (var noeud in Sommets)
        //    {
        //        foreach (var lien in noeud.Voisins)
        //        {
        //            Noeud<T> voisin = lien.Noeud1.Equals(noeud) ? lien.Noeud2 : lien.Noeud1;
        //            if (distances[noeud] + lien.Poids < distances[voisin])
        //            {
        //                throw new Exception("Il y a un circuit/cycle de poids négatif");
        //            }
        //        }
        //    }

        //    return distances;
        //}

        public List<Noeud<T>> BellmanFord2(Noeud<T> source, Noeud<T> destination)
        {
            Dictionary<Noeud<T>, double> distances = new Dictionary<Noeud<T>, double>();
            Dictionary<Noeud<T>, Noeud<T>> predecesseurs = new Dictionary<Noeud<T>, Noeud<T>>();

            // Initialisation des distances à l'infini sauf la source
            foreach (var noeud in Sommets)
            {
                distances[noeud] = double.PositiveInfinity;
                predecesseurs[noeud] = null;
            }
            distances[source] = 0;

            // Boucle principale de Bellman-Ford
            int n = Sommets.Count;
            for (int i = 0; i < n - 1; i++)
            {
                foreach (var noeud in Sommets)
                {
                    foreach (var lien in noeud.Voisins)
                    {
                        Noeud<T> voisin = lien.Noeud1.Equals(noeud) ? lien.Noeud2 : lien.Noeud1;
                        double nouvelleDistance = distances[noeud] + lien.Poids;

                        if (nouvelleDistance < distances[voisin])
                        {
                            distances[voisin] = nouvelleDistance;
                            predecesseurs[voisin] = noeud;
                        }
                    }
                }
            }

            // Vérification des cycles de poids négatif
            foreach (var noeud in Sommets)
            {
                foreach (var lien in noeud.Voisins)
                {
                    Noeud<T> voisin = lien.Noeud1.Equals(noeud) ? lien.Noeud2 : lien.Noeud1;
                    if (distances[noeud] + lien.Poids < distances[voisin])
                    {
                        throw new Exception("Il y a un circuit/cycle de poids négatif");
                    }
                }
            }

            // Construction du chemin le plus court jusqu'à la destination
            List<Noeud<T>> chemin = new List<Noeud<T>>();
            Noeud<T> courant = destination;

            if (distances[destination] == double.PositiveInfinity)
            {
                return chemin; // Aucun chemin trouvé
            }

            while (courant != null)
            {
                chemin.Insert(0, courant);
                courant = predecesseurs[courant];
            }

            return chemin;
        }

        /// <summary>
        /// Algo du plus petit chemin : FloydWarshall
        /// </summary>
        /// <returns></returns>
        public double[,] FloydWarshall()
        {
            int n = Sommets.Count;
            if (n == 0) return new double[0, 0];

            Dictionary<Noeud<T>, int> indexMap = new Dictionary<Noeud<T>, int>();
            Noeud<T>[] noeuds = Sommets.ToArray();

            // Associer chaque nœud à un index unique
            for (int i = 0; i < n; i++)
            {
                indexMap[noeuds[i]] = i;
            }

            // Initialisation de la matrice des distances
            double[,] distances = new double[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                        distances[i, j] = 0; // Distance à soi-même = 0
                    else
                        distances[i, j] = double.PositiveInfinity; // Initialisation à l'infini
                }
            }

            // Remplir la matrice avec les poids des arêtes existantes
            foreach (var noeud in Sommets)
            {
                int i = indexMap[noeud];
                foreach (var lien in noeud.Voisins)
                {
                    Noeud<T> voisin = lien.Noeud1.Equals(noeud) ? lien.Noeud2 : lien.Noeud1;
                    int j = indexMap[voisin];
                    distances[i, j] = lien.Poids;
                }
            }

            // Algorithme de Floyd-Warshall
            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (distances[i, k] != double.PositiveInfinity &&
                            distances[k, j] != double.PositiveInfinity)
                        {
                            distances[i, j] = Math.Min(distances[i, j], distances[i, k] + distances[k, j]);
                        }
                    }
                }
            }

            return distances;
        }

        /// <summary>
        /// Sous fonction de FloydWarshall pour la matrice de distances
        /// </summary>
        /// <param name="distances"></param>
        public void AfficherMatriceDistances(double[,] distances)
        {
            int n = distances.GetLength(0);
            Console.WriteLine("Matrice des plus courts chemins (Floyd-Warshall) :");

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (distances[i, j] == double.PositiveInfinity)
                        Console.Write("INF\t");
                    else
                        Console.Write($"{distances[i, j]}\t");
                }
                Console.WriteLine();
            }
        }
    }
}
