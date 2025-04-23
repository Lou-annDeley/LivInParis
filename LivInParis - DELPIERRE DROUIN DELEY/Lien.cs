using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivInParis___DELPIERRE_DROUIN_DELEY
{
    public class Lien<T>
    {
        public Noeud<T> Noeud1 { get; }  // Premier nœud (station)
        public Noeud<T> Noeud2 { get; }  // Second nœud (station connectée)
        public double Poids { get; }  // Distance ou temps entre les deux nœuds

        /// <summary>
        /// Constructeur Lien
        /// </summary>
        /// <param name="noeud1"></param>
        /// <param name="noeud2"></param>
        /// <param name="poids"></param>
        public Lien(Noeud<T> noeud1, Noeud<T> noeud2, double poids)
        {
            Noeud1 = noeud1;
            Noeud2 = noeud2;
            Poids = poids;
        }

        public Noeud<T> AutreSommet(Noeud<T> noeud)
        {
            return noeud.Equals(Noeud1) ? Noeud2 : Noeud1;
        }
    }
}
