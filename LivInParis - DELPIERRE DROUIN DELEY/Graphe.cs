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

    }
}
