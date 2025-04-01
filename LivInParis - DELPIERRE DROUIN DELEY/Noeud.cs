using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace LivInParis___DELPIERRE_DROUIN_DELEY
{
    public class Noeud<T>
    {
        public T Valeur { get; set; }  // Stocke la valeur du nœud (ex: une station de métro)
        public List<Lien<T>> Voisins { get; }  // Liste des liens (connexions) vers d'autres nœuds
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        /// <summary>
        /// Constructeur Noeud
        /// </summary>
        /// <param name="valeur"></param>
        public Noeud(T valeur)
        {
            Valeur = valeur;
            Voisins = new List<Lien<T>>();
        }

        /// <summary>
        /// Fonction qui recherche si un lien existe entre deux noeuds
        /// </summary>
        /// <param name="autreNoeud"></param>
        /// <returns></returns>
        public bool ExisteLien(Noeud<T> autreNoeud)
        {
            foreach (var lien in Voisins)
            {
                if (lien.Noeud1.Equals(autreNoeud) || lien.Noeud2.Equals(autreNoeud))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
