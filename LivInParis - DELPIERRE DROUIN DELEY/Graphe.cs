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
        public List<Noeud<T>> Sommets { get; } 

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

        public List<Lien<T>> Kruskal()
        {
            var arbre = new List<Lien<T>>();
            var ensemble = new Dictionary<Noeud<T>, Noeud<T>>();

            
            foreach (var noeud in Sommets)
                ensemble[noeud] = noeud;

            
            Noeud<T> Find(Noeud<T> n)
            {
                if (!ensemble.ContainsKey(n)) return n;
                if (!ReferenceEquals(ensemble[n], n))
                    ensemble[n] = Find(ensemble[n]);
                return ensemble[n];
            }

            
            void Union(Noeud<T> a, Noeud<T> b)
            {
                var rootA = Find(a);
                var rootB = Find(b);
                if (!ReferenceEquals(rootA, rootB))
                    ensemble[rootA] = rootB;
            }

            
            var liens = new List<Lien<T>>();
            var dejaVu = new HashSet<(Noeud<T>, Noeud<T>)>();

            foreach (var noeud in Sommets)
            {
                foreach (var lien in noeud.Voisins)
                {
                    var key = (lien.Noeud1, lien.Noeud2);
                    var keyInverse = (lien.Noeud2, lien.Noeud1);

                    if (!dejaVu.Contains(key) && !dejaVu.Contains(keyInverse))
                    {
                        liens.Add(lien);
                        dejaVu.Add(key);
                    }
                }
            }

            var liensTries = liens.OrderBy(l => l.Poids).ToList();

            foreach (var lien in liensTries)
            {
                var racine1 = Find(lien.Noeud1);
                var racine2 = Find(lien.Noeud2);

                if (!ReferenceEquals(racine1, racine2))
                {
                    arbre.Add(lien);
                    Union(racine1, racine2);
                }
            }

            return arbre;
        }








        public void DessinerArbreKruskal2(string outputPath)
        {
            int width = 6000, height = 4000;
            var bitmap = new SKBitmap(width, height);
            var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.White);

            var paintEdge = new SKPaint { Color = SKColors.Black, StrokeWidth = 6 };
            var fontPaint = new SKPaint { Color = SKColors.Black, TextSize = 60, IsAntialias = true };
            var paintNode = new SKPaint { Color = SKColors.Red, Style = SKPaintStyle.Fill };

            var arbre = Kruskal();

            
            var noeuds = arbre.SelectMany(l => new[] { l.Noeud1, l.Noeud2 }).Distinct().ToList();

          
            var positions = new Dictionary<Noeud<T>, SKPoint>();
            float centerX = width / 2f, centerY = height / 2f;
            float radius = Math.Min(width, height) / 2f - 300;
            int n = noeuds.Count;

            for (int i = 0; i < n; i++)
            {
                double angle = 2 * Math.PI * i / n;
                float x = centerX + (float)(radius * Math.Cos(angle));
                float y = centerY + (float)(radius * Math.Sin(angle));
                positions[noeuds[i]] = new SKPoint(x, y);
            }

          
            foreach (var lien in arbre)
            {
                var p1 = positions[lien.Noeud1];
                var p2 = positions[lien.Noeud2];
                canvas.DrawLine(p1, p2, paintEdge);

                var middle = new SKPoint((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
                canvas.DrawText(lien.Poids.ToString(), middle.X, middle.Y, fontPaint);
            }

           
            foreach (var (noeud, point) in positions)
            {
                canvas.DrawCircle(point, 20, paintNode);
                canvas.DrawText(noeud.ToString(), point.X + 25, point.Y + 25, fontPaint);
            }

            using (var image = SKImage.FromBitmap(bitmap))
            using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
            using (var stream = File.OpenWrite(outputPath))
            {
                data.SaveTo(stream);
            }
        }

        public void ChargerCoordonneesDepuisExcel(string excelPath)
        {
            int k = 0;
            using (var workbook = new XLWorkbook(excelPath))
            {
                var sheetNoeuds = workbook.Worksheet("Noeuds");

                foreach (var row in sheetNoeuds.RowsUsed().Skip(1))
                {
                    double latitude = 0;
                    double longitude = 0;

                    if (!double.TryParse(row.Cell(5).GetString().Trim(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out latitude))
                        continue;

                    if (!double.TryParse(row.Cell(4).GetString().Trim(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out longitude))
                        continue;

                    if (k < Sommets.Count)
                    {
                        Sommets[k].Latitude = latitude;
                        Sommets[k].Longitude = longitude;
                        k++;
                    }
                }
            }
        }
        public void DessinerArbreKruskal(string outputPath)
        {
           
            ChargerCoordonneesDepuisExcel("MetroParis.xlsx");

            int width = 6000, height = 4000;
            var bitmap = new SKBitmap(width, height);
            var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.White);

            var paintEdge = new SKPaint { Color = SKColors.Blue, StrokeWidth = 6 };
            var fontPaint = new SKPaint { Color = SKColors.Black, TextSize = 50, IsAntialias = true };
            var paintNode = new SKPaint { Color = SKColors.Red, Style = SKPaintStyle.Fill };

            var arbre = Kruskal();

            double minLat = 48.819106595610265;
            double maxLat = 48.897802691407826;
            double minLong = 2.2570461929221497;
            double maxLong = 2.4405400954061127;

            var positions = new Dictionary<Noeud<T>, SKPoint>();

            foreach (var lien in arbre)
            {
                foreach (var noeud in new[] { lien.Noeud1, lien.Noeud2 })
                {
                    if (!positions.ContainsKey(noeud))
                    {
                        float x = (float)((noeud.Longitude - minLong) / (maxLong - minLong) * width);
                        float y = (float)((noeud.Latitude - minLat) / (maxLat - minLat) * height);
                        positions[noeud] = new SKPoint(x, y);
                    }
                }
            }

            foreach (var lien in arbre)
            {
                var p1 = positions[lien.Noeud1];
                var p2 = positions[lien.Noeud2];
                canvas.DrawLine(p1, p2, paintEdge);

                var middle = new SKPoint((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2 - 20);
                canvas.DrawText(lien.Poids.ToString(), middle, fontPaint);
            }

            foreach (var (noeud, point) in positions)
            {
                canvas.DrawCircle(point, 20, paintNode);
                canvas.DrawText(noeud.Valeur.ToString(), point.X + 10, point.Y - 30, fontPaint);
            }

            using (var image = SKImage.FromBitmap(bitmap))
            using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
            using (var stream = File.OpenWrite(outputPath))
            {
                data.SaveTo(stream);
            }
        }


        public void DessinerGraphe(string outputPath)
        {
            int width = 6000, height = 4000; 
            var bitmap = new SKBitmap(width, height);
            var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.White);

            var paintEdge = new SKPaint { Color = SKColors.Black, StrokeWidth = 6 };
            var fontPaint = new SKPaint { Color = SKColors.Black, TextSize = 60, IsAntialias = true };

            Dictionary<Noeud<T>, SKPoint> positions = new Dictionary<Noeud<T>, SKPoint>();

            
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

           
            var couleursWelshPowell = ColorierWelshPowell();

          
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

                
                int numeroCouleur = couleursWelshPowell[noeud];
                SKColor couleur = palette[(numeroCouleur - 1) % palette.Count]; 

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
            int width = 6000, height = 4000; 
            var bitmap = new SKBitmap(width, height);
            var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.White);

            var paintEdge = new SKPaint { Color = SKColors.Black, StrokeWidth = 6 };
            var fontPaint = new SKPaint { Color = SKColors.Black, TextSize = 60, IsAntialias = true };

            Dictionary<Noeud<T>, SKPoint> positions = new Dictionary<Noeud<T>, SKPoint>();

            
            Random rand = new Random();
            foreach (var noeud in Sommets)
            {
                float x = (float)(rand.NextDouble() * (width - 200) + 100); 
                float y = (float)(rand.NextDouble() * (height - 200) + 100);
                positions[noeud] = new SKPoint(x, y);
            }

           
            foreach (var noeud in Sommets)
            {
                foreach (var lien in noeud.Voisins)
                {
                    if (!positions.ContainsKey(lien.Noeud2)) continue;

                    SKPoint point1 = positions[noeud];
                    SKPoint point2 = positions[lien.Noeud2];
                    canvas.DrawLine(point1, point2, paintEdge);

                    SKPoint middle = new SKPoint((point1.X + point2.X) / 2, (point1.Y + point2.Y) / 2);
                    canvas.DrawText(lien.Poids.ToString(), middle.X, middle.Y, fontPaint);
                }
            }

            
            foreach (var noeud in Sommets)
            {
                if (!positions.ContainsKey(noeud)) continue;

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








        /// <summary>
        /// Fonction qui met en évidence le plus court chemin à emprunter
        /// </summary>
        /// <param name="chemin"></param>
        /// <param name="outputPath"></param>
        public void MettreEnEvidenceChemin(List<Noeud<T>> chemin, string outputPath)
        {
            int width = 6000, height = 4000;
            var bitmap = new SKBitmap(width, height);
            var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.White);

            var paintEdge = new SKPaint { Color = SKColors.Black, StrokeWidth = 6 }; 
            var paintEdgeHighlighted = new SKPaint { Color = SKColors.Blue, StrokeWidth = 10 };
            var fontPaint = new SKPaint { Color = SKColors.Black, TextSize = 60, IsAntialias = true };

            Dictionary<Noeud<T>, SKPoint> positions = new Dictionary<Noeud<T>, SKPoint>();

            
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

            
            for (int i = 0; i < chemin.Count - 1; i++)
            {
                Noeud<T> noeud1 = chemin[i];
                Noeud<T> noeud2 = chemin[i + 1];

                SKPoint point1 = positions[noeud1];
                SKPoint point2 = positions[noeud2];

                canvas.DrawLine(point1, point2, paintEdgeHighlighted); 
            }

            
            foreach (var noeud in Sommets)
            {
                SKPoint pos = positions[noeud];
                var paintNode = new SKPaint { Color = chemin.Contains(noeud) ? SKColors.Blue : SKColors.Red, Style = SKPaintStyle.Fill };
                canvas.DrawCircle(pos, 15, paintNode); 
            }

         
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

            
            foreach (var sommet in Sommets)
            {
                degres[sommet] = sommet.Voisins.Count;
            }

            
            var sommetsTries = new List<Noeud<T>>(Sommets);
            sommetsTries.Sort((a, b) => degres[b].CompareTo(degres[a]));

            int couleurActuelle = 0;

            
            while (sommetsTries.Count > 0)
            {
                couleurActuelle++;

                var aColorier = new List<Noeud<T>>();

                foreach (var sommet in sommetsTries)
                {
                    bool conflit = false;

                    
                    foreach (var voisin in sommet.Voisins)
                    {
                        var autre = voisin.AutreSommet(sommet);
                        if (couleurs.TryGetValue(autre, out int couleurVoisin) && couleurVoisin == couleurActuelle)
                        {
                            conflit = true;
                            break;
                        }
                    }

                    
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

              
                foreach (var sommet in aColorier)
                {
                    sommetsTries.Remove(sommet);
                }
            }

            return couleurs;
        }

        public void DessinerGrapheColorie(string cheminFichier)
        {
            var couleurs = ColorierWelshPowell();

            int largeur = 1000;
            int hauteur = 800;
            int rayon = 15;

            
            SKColor[] palette = new SKColor[]
            {
        SKColors.Red, SKColors.Green, SKColors.Blue, SKColors.Orange,
        SKColors.Purple, SKColors.Teal, SKColors.Brown, SKColors.Magenta,
        SKColors.Cyan, SKColors.Yellow, SKColors.Lime, SKColors.Pink
            };

            
            Dictionary<Noeud<T>, SKPoint> positions = new Dictionary<Noeud<T>, SKPoint>();
            int n = Sommets.Count;
            float angleStep = 2 * (float)Math.PI / n;
            float rayonGraphe = Math.Min(largeur, hauteur) / 2.5f;
            float centreX = largeur / 2;
            float centreY = hauteur / 2;

            for (int i = 0; i < Sommets.Count; i++)
            {
                var angle = i * angleStep;
                float x = centreX + rayonGraphe * (float)Math.Cos(angle);
                float y = centreY + rayonGraphe * (float)Math.Sin(angle);
                positions[Sommets[i]] = new SKPoint(x, y);
            }

            using (var bitmap = new SKBitmap(largeur, hauteur))
            using (var canvas = new SKCanvas(bitmap))
            using (var paint = new SKPaint())
            {
                canvas.Clear(SKColors.White);

                paint.IsAntialias = true;
                paint.StrokeWidth = 2;

                
                foreach (var sommet in Sommets)
                {
                    foreach (var lien in sommet.Voisins)
                    {
                        var autre = lien.AutreSommet(sommet);

                        if (positions.TryGetValue(sommet, out SKPoint p1) &&
                            positions.TryGetValue(autre, out SKPoint p2))
                        {
                            canvas.DrawLine(p1, p2, paint);
                        }
                    }
                }

                
                foreach (var sommet in Sommets)
                {
                    var couleurIndice = couleurs.ContainsKey(sommet) ? couleurs[sommet] % palette.Length : 0;
                    paint.Color = palette[couleurIndice];

                    var position = positions[sommet];
                    canvas.DrawCircle(position, rayon, paint);

                    
                    paint.Color = SKColors.Black;
                    paint.TextSize = 12;
                    var texte = sommet.Valeur.ToString();
                    canvas.DrawText(texte, position.X + 10, position.Y, paint);
                }

                
                using (var image = SKImage.FromBitmap(bitmap))
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                using (var stream = File.OpenWrite(cheminFichier))
                {
                    data.SaveTo(stream);
                }
            }

            Console.WriteLine($"Graphe colorié sauvegardé dans : {cheminFichier}");
        }




    }
}
