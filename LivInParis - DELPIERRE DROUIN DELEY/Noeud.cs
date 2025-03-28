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

        public Noeud(T valeur)
        {
            Valeur = valeur;
            Voisins = new List<Lien<T>>();
        }



    }
}
