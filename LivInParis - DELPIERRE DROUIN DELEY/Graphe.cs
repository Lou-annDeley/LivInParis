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
using DocumentFormat.OpenXml.Vml;

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
            int width = 6000, height = 4000; // Taille de l'image
            var bitmap = new SKBitmap(width, height);
            var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.White);

            var paintEdge = new SKPaint { Color = SKColors.Black, StrokeWidth = 6 };
            var fontPaint = new SKPaint { Color = SKColors.Black, TextSize = 60, IsAntialias = true };

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
                    if (!double.TryParse(row.Cell(5).GetString().Trim(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out latitude))
                    {
                        Console.WriteLine($"Erreur de conversion de la latitude pour la station {row.Cell(2).GetString()}");
                        continue; // Si la conversion échoue, passer à la ligne suivante
                    }

                    // Essayer de récupérer la longitude
                    if (!double.TryParse(row.Cell(4).GetString().Trim(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out longitude))
                    {
                        Console.WriteLine($"Erreur de conversion de la longitude pour la station {row.Cell(2).GetString()}");
                        continue; // Si la conversion échoue, passer à la ligne suivante
                    }

                    // Assigner la latitude et la longitude aux nœuds
                    Sommets[k].Latitude = latitude;
                    Sommets[k].Longitude = longitude;
                    k++;
                }
            }

            double minLat = 48.819106595610265;
            double maxLat = 48.897802691407826;
            double minLong = 2.2570461929221497;
            double maxLong = 2.4405400954061127;

            foreach (var noeud in Sommets)
            {
                float x = (float)((noeud.Longitude - minLong) / (maxLong - minLong) * width);
                float y = (float)((noeud.Latitude - minLat) / (maxLat - minLat) * height);
                positions[noeud] = new SKPoint(x, y);
            }

            foreach (var noeud in Sommets)
            {

                if (noeud.Voisins.Count == 1)
                {
                    Noeud<T> voisin2 = noeud.Voisins[0].Noeud2;
                    SKPoint point3 = positions[noeud];
                    SKPoint point4 = positions[voisin2];

                    canvas.DrawLine(point3, point4, paintEdge);

                    SKPoint middle2 = new SKPoint((point3.X + point4.X) / 2, (point3.Y + point4.Y) / 2);
                    canvas.DrawText(noeud.Voisins[0].Poids.ToString(), middle2.X, middle2.Y, fontPaint);
                }
                else
                {
                    foreach (var lien in noeud.Voisins.Skip(1))
                    {
                        Noeud<T> voisin = lien.Noeud2;
                        SKPoint point1 = positions[noeud];
                        SKPoint point2 = positions[voisin];

                        canvas.DrawLine(point1, point2, paintEdge);

                        SKPoint middle = new SKPoint((point1.X + point2.X) / 2, (point1.Y + point2.Y) / 2);
                        canvas.DrawText(lien.Poids.ToString(), middle.X, middle.Y, fontPaint);
                    }
                }
            }

            foreach (var noeud in Sommets)
            {
                SKPoint pos = positions[noeud];
                var paintNode = new SKPaint { Color = SKColors.Red, Style = SKPaintStyle.Fill };
                canvas.DrawCircle(pos, 15, paintNode);
            }

            using (var image = SKImage.FromBitmap(bitmap))
            using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
            using (var stream = File.OpenWrite(outputPath))
            {
                data.SaveTo(stream);
            }
        }

        public void DessinerGraphe2(string outputPath)
        {
            int width = 6000, height = 4000;
            var bitmap = new SKBitmap(width, height);
            var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.White);

            var paintEdge = new SKPaint { Color = SKColors.Black, StrokeWidth = 6 };
            var fontPaint = new SKPaint { Color = SKColors.Black, TextSize = 60, IsAntialias = true };

            Dictionary<Noeud<T>, SKPoint> positions = new Dictionary<Noeud<T>, SKPoint>();

            // --- Welsh-Powell pour récupérer les couleurs des noeuds ---
            var couleursWelshPowell = ColorierWelshPowell();

            // Palette des couleurs
            List<SKColor> palette = new List<SKColor>
    {
        SKColors.Blue,
        SKColors.Red,
        SKColors.Green,
        SKColors.Orange,
        SKColors.Purple,
        SKColors.Brown,
        SKColors.Cyan,
        SKColors.Magenta
    };

            // Chargement des données depuis Excel
            int k = 0;
            using (var workbook = new XLWorkbook("MetroParis.xlsx"))
            {
                var sheetNoeuds = workbook.Worksheet("Noeuds");

                foreach (var row in sheetNoeuds.RowsUsed().Skip(1))
                {
                    double latitude = 0;
                    double longitude = 0;

                    if (!double.TryParse(row.Cell(5).GetString().Trim(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out latitude))
                    {
                        Console.WriteLine($"Erreur de conversion de la latitude pour la station {row.Cell(2).GetString()}");
                        continue;
                    }

                    if (!double.TryParse(row.Cell(4).GetString().Trim(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out longitude))
                    {
                        Console.WriteLine($"Erreur de conversion de la longitude pour la station {row.Cell(2).GetString()}");
                        continue;
                    }

                    Sommets[k].Latitude = latitude;
                    Sommets[k].Longitude = longitude;
                    k++;
                }
            }

            double minLat = 48.819106595610265;
            double maxLat = 48.897802691407826;
            double minLong = 2.2570461929221497;
            double maxLong = 2.4405400954061127;

            foreach (var noeud in Sommets)
            {
                float x = (float)((noeud.Longitude - minLong) / (maxLong - minLong) * width);
                float y = (float)((noeud.Latitude - minLat) / (maxLat - minLat) * height);
                positions[noeud] = new SKPoint(x, y);
            }

            foreach (var noeud in Sommets)
            {
                if (noeud.Voisins.Count == 1)
                {
                    Noeud<T> voisin2 = noeud.Voisins[0].Noeud2;
                    SKPoint point3 = positions[noeud];
                    SKPoint point4 = positions[voisin2];

                    canvas.DrawLine(point3, point4, paintEdge);

                    SKPoint middle2 = new SKPoint((point3.X + point4.X) / 2, (point3.Y + point4.Y) / 2);
                    canvas.DrawText(noeud.Voisins[0].Poids.ToString(), middle2.X, middle2.Y, fontPaint);
                }
                else
                {
                    foreach (var lien in noeud.Voisins.Skip(1))
                    {
                        Noeud<T> voisin = lien.Noeud2;
                        SKPoint point1 = positions[noeud];
                        SKPoint point2 = positions[voisin];

                        canvas.DrawLine(point1, point2, paintEdge);

                        SKPoint middle = new SKPoint((point1.X + point2.X) / 2, (point1.Y + point2.Y) / 2);
                        canvas.DrawText(lien.Poids.ToString(), middle.X, middle.Y, fontPaint);
                    }
                }
            }

            foreach (var noeud in Sommets)
            {
                SKPoint pos = positions[noeud];

                // --- Ici on applique la bonne couleur depuis Welsh-Powell ---
                int numeroCouleur = couleursWelshPowell[noeud]; // 1, 2, 3, etc.
                SKColor couleur = palette[(numeroCouleur - 1) % palette.Count]; // Choisir couleur

                var paintNode = new SKPaint { Color = couleur, Style = SKPaintStyle.Fill };

                canvas.DrawCircle(pos, 15, paintNode);
            }

            using (var image = SKImage.FromBitmap(bitmap))
            using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
            using (var stream = File.OpenWrite(outputPath))
            {
                data.SaveTo(stream);
            }
        }

        public void DessinerGrapheCuisiniersClients(string outputPath)
        {
            int width = 3000, height = 2000; // Taille de l'image
            var bitmap = new SKBitmap(width, height);
            var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.White);

            var paintEdge = new SKPaint { Color = SKColors.Black, StrokeWidth = 4 };
            var fontPaint = new SKPaint { Color = SKColors.Black, TextSize = 40, IsAntialias = true };
            var paintNodeClient = new SKPaint { Color = SKColors.Blue, Style = SKPaintStyle.Fill };
            var paintNodeCuisinier = new SKPaint { Color = SKColors.Green, Style = SKPaintStyle.Fill };

            Dictionary<Noeud<T>, SKPoint> positions = new Dictionary<Noeud<T>, SKPoint>();

            // Séparer les nœuds clients et cuisiniers
            var clients = Sommets.Where(n => n.Valeur.ToString().StartsWith("Client")).ToList();
            var cuisiniers = Sommets.Where(n => n.Valeur.ToString().StartsWith("Cuisinier")).ToList();

            int nbClients = clients.Count;
            int nbCuisiniers = cuisiniers.Count;

            int margin = 200;

            // Placer les clients à gauche
            for (int i = 0; i < nbClients; i++)
            {
                float x = margin;
                float y = margin + i * (height - 2 * margin) / Math.Max(nbClients - 1, 1);
                positions[clients[i]] = new SKPoint(x, y);
            }

            // Placer les cuisiniers à droite
            for (int i = 0; i < nbCuisiniers; i++)
            {
                float x = width - margin;
                float y = margin + i * (height - 2 * margin) / Math.Max(nbCuisiniers - 1, 1);
                positions[cuisiniers[i]] = new SKPoint(x, y);
            }

            // Dessiner les arêtes
            foreach (var noeud in Sommets)
            {
                foreach (var lien in noeud.Voisins)
                {
                    SKPoint point1 = positions[noeud];
                    SKPoint point2 = positions[lien.Noeud2];
                    canvas.DrawLine(point1, point2, paintEdge);

                    // Dessiner le poids au milieu de la ligne
                    SKPoint middle = new SKPoint((point1.X + point2.X) / 2, (point1.Y + point2.Y) / 2);
                    canvas.DrawText(lien.Poids.ToString(), middle.X, middle.Y, fontPaint);
                }
            }

            // Dessiner les nœuds
            foreach (var noeud in Sommets)
            {
                SKPoint pos = positions[noeud];
                var paintNode = noeud.Valeur.ToString().StartsWith("Client") ? paintNodeClient : paintNodeCuisinier;
                canvas.DrawCircle(pos, 20, paintNode);
                // Afficher le nom à côté du point
                canvas.DrawText(noeud.Valeur.ToString(), pos.X + 25, pos.Y + 10, fontPaint);
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
        /// Fonction qui met en évidence le plus court chemin à emprunter
        /// </summary>
        /// <param name="chemin"></param>
        /// <param name="outputPath"></param>
        public void MettreEnEvidenceChemin(List<Noeud<T>> chemin, string outputPath)
        {
            int width = 6000, height = 4000; // Taille de l'image
            var bitmap = new SKBitmap(width, height);
            var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.White);

            var paintEdge = new SKPaint { Color = SKColors.Black, StrokeWidth = 6 }; // Liens normaux
            var paintEdgeHighlighted = new SKPaint { Color = SKColors.Blue, StrokeWidth = 10 }; // Liens en gras pour le chemin
            var fontPaint = new SKPaint { Color = SKColors.Black, TextSize = 60, IsAntialias = true };

            Dictionary<Noeud<T>, SKPoint> positions = new Dictionary<Noeud<T>, SKPoint>();

            // Calcul des positions des nœuds
            double minLat = 48.819106595610265;
            double maxLat = 48.897802691407826;
            double minLong = 2.2570461929221497;
            double maxLong = 2.4405400954061127;

            foreach (var noeud in Sommets)
            {
                float x = (float)((noeud.Longitude - minLong) / (maxLong - minLong) * width);
                float y = (float)((noeud.Latitude - minLat) / (maxLat - minLat) * height);
                positions[noeud] = new SKPoint(x, y);
            }

            // Dessiner tous les liens normaux
            foreach (var noeud in Sommets)
            {

                if (noeud.Voisins.Count == 1)
                {
                    Noeud<T> voisin2 = noeud.Voisins[0].Noeud2;
                    SKPoint point3 = positions[noeud];
                    SKPoint point4 = positions[voisin2];

                    canvas.DrawLine(point3, point4, paintEdge);

                    SKPoint middle2 = new SKPoint((point3.X + point4.X) / 2, (point3.Y + point4.Y) / 2);
                    canvas.DrawText(noeud.Voisins[0].Poids.ToString(), middle2.X, middle2.Y, fontPaint);
                }
                else
                {
                    foreach (var lien in noeud.Voisins.Skip(1))
                    {
                        Noeud<T> voisin = lien.Noeud2;
                        SKPoint point1 = positions[noeud];
                        SKPoint point2 = positions[voisin];

                        canvas.DrawLine(point1, point2, paintEdge);

                        SKPoint middle = new SKPoint((point1.X + point2.X) / 2, (point1.Y + point2.Y) / 2);
                        canvas.DrawText(lien.Poids.ToString(), middle.X, middle.Y, fontPaint);
                    }
                }
            }

            // Mettre en évidence les liens du plus court chemin
            for (int i = 0; i < chemin.Count - 1; i++)
            {
                Noeud<T> noeud1 = chemin[i];
                Noeud<T> noeud2 = chemin[i + 1];

                SKPoint point1 = positions[noeud1];
                SKPoint point2 = positions[noeud2];

                canvas.DrawLine(point1, point2, paintEdgeHighlighted); // Dessiner les liens du chemin en bleu épais
            }

            // Dessiner les nœuds (stations)
            foreach (var noeud in Sommets)
            {
                SKPoint pos = positions[noeud];
                var paintNode = new SKPaint { Color = chemin.Contains(noeud) ? SKColors.Blue : SKColors.Red, Style = SKPaintStyle.Fill };
                canvas.DrawCircle(pos, 15, paintNode); // Cercle représentant la station
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
        /// Fonction du plus court chemin : Dijkstra
        /// </summary>
        /// <param name="depart"></param>
        /// <param name="arrivee"></param>
        /// <returns></returns>
        /// 
        public List<Noeud<T>> Dijkstra(Noeud<T> source, Noeud<T> destination)
        {
            Dictionary<Noeud<T>, double> distances = new Dictionary<Noeud<T>, double>();
            Dictionary<Noeud<T>, Noeud<T>> predecesseurs = new Dictionary<Noeud<T>, Noeud<T>>();
            Dictionary<Noeud<T>, bool> visited = new Dictionary<Noeud<T>, bool>();

            foreach (Noeud<T> noeud in Sommets)
            {
                distances[noeud] = double.MaxValue;
                visited[noeud] = false;
                predecesseurs[noeud] = null;
            }
            distances[source] = 0;

            for (int i = 0; i < Sommets.Count; i++)
            {
                Noeud<T> noeudActuel = TrouverNoeudMin(distances, visited);
                if (noeudActuel == null || noeudActuel.Equals(destination))
                    break;

                visited[noeudActuel] = true;

                foreach (Lien<T> lien in noeudActuel.Voisins)
                {
                    Noeud<T> voisin = lien.Noeud1.Equals(noeudActuel) ? lien.Noeud2 : lien.Noeud1;
                    if (!visited[voisin] && distances[noeudActuel] + lien.Poids < distances[voisin])
                    {
                        distances[voisin] = distances[noeudActuel] + lien.Poids;
                        predecesseurs[voisin] = noeudActuel;
                    }
                }
            }

            return ReconstruireChemin(predecesseurs, destination);
        }

        /// <summary>
        /// Sous fonction qui trouve le Noeud Min
        /// </summary>
        /// <param name="distances"></param>
        /// <param name="visited"></param>
        /// <returns></returns>
        private Noeud<T> TrouverNoeudMin(Dictionary<Noeud<T>, double> distances, Dictionary<Noeud<T>, bool> visited)
        {
            double minDistance = double.MaxValue;
            Noeud<T> minNoeud = null;

            foreach (Noeud<T> noeud in Sommets)
            {
                if (!visited[noeud] && distances[noeud] < minDistance)
                {
                    minDistance = distances[noeud];
                    minNoeud = noeud;
                }
            }
            return minNoeud;
        }

        /// <summary>
        /// Sous fonction qui reconstruit le chemin
        /// </summary>
        /// <param name="predecesseurs"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        private List<Noeud<T>> ReconstruireChemin(Dictionary<Noeud<T>, Noeud<T>> predecesseurs, Noeud<T> destination)
        {
            List<Noeud<T>> chemin = new List<Noeud<T>>();
            Noeud<T> noeudActuel = destination;

            while (noeudActuel != null)
            {
                chemin.Insert(0, noeudActuel);
                noeudActuel = predecesseurs[noeudActuel];
            }

            return chemin;
        }


        /// <summary>
        /// Fonction du plus court chemin : BellmanFord
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<Noeud<T>> BellmanFord(Noeud<T> source, Noeud<T> destination)
        {
            Dictionary<Noeud<T>, double> distances = new Dictionary<Noeud<T>, double>();
            Dictionary<Noeud<T>, Noeud<T>> predecesseurs = new Dictionary<Noeud<T>, Noeud<T>>();

            foreach (Noeud<T> noeud in Sommets)
            {
                distances[noeud] = double.MaxValue;
                predecesseurs[noeud] = null;
            }
            distances[source] = 0;

            for (int i = 0; i < Sommets.Count - 1; i++)
            {
                foreach (Noeud<T> noeud in Sommets)
                {
                    foreach (Lien<T> lien in noeud.Voisins)
                    {
                        Noeud<T> voisin = lien.Noeud1.Equals(noeud) ? lien.Noeud2 : lien.Noeud1;
                        if (distances[noeud] != double.MaxValue && distances[noeud] + lien.Poids < distances[voisin])
                        {
                            distances[voisin] = distances[noeud] + lien.Poids;
                            predecesseurs[voisin] = noeud;
                        }
                    }
                }
            }

            foreach (Noeud<T> noeud in Sommets)
            {
                foreach (Lien<T> lien in noeud.Voisins)
                {
                    Noeud<T> voisin = lien.Noeud1.Equals(noeud) ? lien.Noeud2 : lien.Noeud1;
                    if (distances[noeud] != double.MaxValue && distances[noeud] + lien.Poids < distances[voisin])
                    {
                        throw new Exception("Graph contains a negative weight cycle");
                    }
                }
            }

            return ReconstruireChemin(predecesseurs, destination);
        }


        /// <summary>
        /// Fonction du plus court chemin : FloydWarshall
        /// </summary>
        /// <returns></returns>
        public List<Noeud<T>> FloydWarshall(Noeud<T> source, Noeud<T> destination)
        {
            int n = Sommets.Count;
            if (n == 0) return new List<Noeud<T>>();

            Dictionary<Noeud<T>, int> indexMap = new Dictionary<Noeud<T>, int>();
            Noeud<T>[] noeuds = Sommets.ToArray();

            for (int i = 0; i < n; i++)
            {
                indexMap[noeuds[i]] = i;
            }

            double[,] distances = new double[n, n];
            int[,] next = new int[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                    {
                        distances[i, j] = 0;
                        next[i, j] = i;
                    }
                    else
                    {
                        distances[i, j] = double.PositiveInfinity;
                        next[i, j] = -1;
                    }
                }
            }

            foreach (var noeud in Sommets)
            {
                int i = indexMap[noeud];
                foreach (var lien in noeud.Voisins)
                {
                    Noeud<T> voisin = lien.Noeud1.Equals(noeud) ? lien.Noeud2 : lien.Noeud1;
                    int j = indexMap[voisin];
                    distances[i, j] = lien.Poids;
                    next[i, j] = j;
                }
            }

            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (distances[i, k] != double.PositiveInfinity && distances[k, j] != double.PositiveInfinity)
                        {
                            double newDist = distances[i, k] + distances[k, j];
                            if (newDist < distances[i, j])
                            {
                                distances[i, j] = newDist;
                                next[i, j] = next[i, k];
                            }
                        }
                    }
                }
            }

            if (next[indexMap[source], indexMap[destination]] == -1)
            {
                return new List<Noeud<T>>();
            }

            List<Noeud<T>> path = new List<Noeud<T>>();
            int current = indexMap[source];
            int target = indexMap[destination];

            while (current != target)
            {
                path.Add(noeuds[current]);
                current = next[current, target];
                if (current == -1) return new List<Noeud<T>>();
            }
            path.Add(noeuds[target]);

            return path;
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


        public Dictionary<Noeud<T>, int> ColorierWelshPowell()
        {
            var couleurs = new Dictionary<Noeud<T>, int>();
            var degres = new Dictionary<Noeud<T>, int>();

            // 1. Calculer le degré de chaque sommet
            foreach (var sommet in Sommets)
            {
                degres[sommet] = sommet.Voisins.Count;
            }

            // 2. Trier par degré décroissant
            var sommetsTries = new List<Noeud<T>>(Sommets);
            sommetsTries.Sort((a, b) => degres[b].CompareTo(degres[a]));

            int couleurActuelle = 0;

            // 3. Tant qu'il reste des sommets non coloriés
            while (sommetsTries.Count > 0)
            {
                couleurActuelle++;

                var aColorier = new List<Noeud<T>>();

                foreach (var sommet in sommetsTries)
                {
                    bool conflit = false;

                    // Vérifier s'il est adjacent à un sommet déjà colorié avec couleurActuelle
                    foreach (var voisin in sommet.Voisins)
                    {
                        var autre = voisin.AutreSommet(sommet);
                        if (couleurs.TryGetValue(autre, out int couleurVoisin) && couleurVoisin == couleurActuelle)
                        {
                            conflit = true;
                            break;
                        }
                    }

                    // Vérifier s'il est adjacent à un sommet qu'on vient de colorier dans ce tour
                    foreach (var dejaColorie in aColorier)
                    {
                        foreach (var lien in dejaColorie.Voisins)
                        {
                            var autre = lien.AutreSommet(dejaColorie);
                            if (autre.Equals(sommet))
                            {
                                conflit = true;
                                break;
                            }
                        }
                        if (conflit)
                            break;
                    }

                    if (!conflit)
                    {
                        couleurs[sommet] = couleurActuelle;
                        aColorier.Add(sommet);
                    }
                }

                // Retirer les sommets coloriés du tri
                foreach (var sommet in aColorier)
                {
                    sommetsTries.Remove(sommet);
                }
            }

            return couleurs;
        }



    }
}
