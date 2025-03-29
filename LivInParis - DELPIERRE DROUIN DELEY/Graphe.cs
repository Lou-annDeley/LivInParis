using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using SkiaSharp;


namespace LivInParis___DELPIERRE_DROUIN_DELEY
{
    public class Graphe<T>
    {
        public List<Noeud<T>> Sommets { get; }  // Liste des nœuds du graphe

        public Graphe()
        {
            Sommets = new List<Noeud<T>>();
        }

        public Noeud<T> AjouterSommet(T valeur)
        {
            Noeud<T> noeud = new Noeud<T>(valeur);
            Sommets.Add(noeud);
            return noeud;
        }

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
            int width = 600, height = 600;
            var bitmap = new SKBitmap(width, height);
            var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.White);

            var paintNode = new SKPaint { Color = SKColors.Red, Style = SKPaintStyle.Fill };
            var paintEdge = new SKPaint { Color = SKColors.Black, StrokeWidth = 2 };
            var fontPaint = new SKPaint { Color = SKColors.White, TextSize = 14 };

            int rayon = 250; // Rayon du cercle
            SKPoint centre = new SKPoint(width / 2, height / 2);
            Dictionary<Noeud<T>, SKPoint> positions = new Dictionary<Noeud<T>, SKPoint>();

            int totalNoeuds = Sommets.Count;
            int index = 0;

            // Calculer les positions des nœuds sur un cercle
            foreach (var noeud in Sommets)
            {
                double angle = 2 * Math.PI * index / totalNoeuds;
                float x = centre.X + (float)(rayon * Math.Cos(angle));
                float y = centre.Y + (float)(rayon * Math.Sin(angle));
                positions[noeud] = new SKPoint(x, y);
                index++;
            }

            // Dessiner les arêtes (lien entre les nœuds)
            foreach (var noeud in Sommets)
            {
                foreach (var lien in noeud.Voisins)
                {
                    Noeud<T> voisin = lien.Noeud2;
                    SKPoint point1 = positions[noeud];
                    SKPoint point2 = positions[voisin];
                    canvas.DrawLine(point1, point2, paintEdge); // Lien entre les nœuds
                }
            }

            // Dessiner les nœuds (cercles)
            foreach (var noeud in Sommets)
            {
                SKPoint pos = positions[noeud];
                canvas.DrawCircle(pos, 15, paintNode); // Cercle représentant le nœud
                canvas.DrawText(noeud.Valeur.ToString(), pos.X - 5, pos.Y + 5, fontPaint); // Texte du nœud
            }

            // Sauvegarder l'image dans le fichier de sortie
            using (var image = SKImage.FromBitmap(bitmap))
            using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
            using (var stream = System.IO.File.OpenWrite(outputPath))
            {
                data.SaveTo(stream);
            }
        }

        public Dictionary<Noeud<T>, double> Dijkstra(Noeud<T> source)
        {
            // Dictionnaire des distances depuis la source
            Dictionary<Noeud<T>, double> distances = new Dictionary<Noeud<T>, double>();
            Dictionary<Noeud<T>, Noeud<T>> predecesseurs = new Dictionary<Noeud<T>, Noeud<T>>();
            HashSet<Noeud<T>> nonVisites = new HashSet<Noeud<T>>(Sommets);

            // Initialisation : toutes les distances à l'infini sauf la source
            foreach (var noeud in Sommets)
            {
                distances[noeud] = double.PositiveInfinity;
            }
            distances[source] = 0;

            while (nonVisites.Count > 0)
            {
                // Sélectionner le nœud avec la plus petite distance
                Noeud<T> noeudActuel = nonVisites.OrderBy(n => distances[n]).First();
                nonVisites.Remove(noeudActuel);

                // Parcourir ses voisins
                foreach (var lien in noeudActuel.Voisins)
                {
                    Noeud<T> voisin = lien.Noeud1.Equals(noeudActuel) ? lien.Noeud2 : lien.Noeud1;
                    if (!nonVisites.Contains(voisin)) continue;

                    double nouvelleDistance = distances[noeudActuel] + lien.Poids;
                    if (nouvelleDistance < distances[voisin])
                    {
                        distances[voisin] = nouvelleDistance;
                        predecesseurs[voisin] = noeudActuel;
                    }
                }
            }

            return distances;
        }

        public Dictionary<Noeud<T>, double> BellmanFord(Noeud<T> source)
        {
            // Dictionnaire des distances depuis la source
            Dictionary<Noeud<T>, double> distances = new Dictionary<Noeud<T>, double>();
            Dictionary<Noeud<T>, Noeud<T>> predecesseurs = new Dictionary<Noeud<T>, Noeud<T>>();

            // Initialisation : toutes les distances à l'infini sauf la source
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

            return distances;
        }

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
