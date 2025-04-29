using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LivInParis___DELPIERRE_DROUIN_DELEY
{
    public class Client
    {
        public int id_client { get; set; }
        public int telephone { get; set; }
        public string adresse_mail { get; set; }
        public string ville { get; set; }
        public int numero_de_rue { get; set; }
        public string rue { get; set; }
        public int code_postal { get; set; }
        public string metro_le_plus_proche { get; set; }

        public Client(int id_client, int telephone, string adresse_mail, string ville, int numero_de_rue, string rue,  int code_postal, string metro_le_plus_proche)
        {
            this.id_client = id_client;
            this.telephone = telephone;
            this.adresse_mail = adresse_mail;
            this.ville = ville;
            this.numero_de_rue = numero_de_rue;
            this.rue = rue;
            this.code_postal = code_postal;
            this.metro_le_plus_proche = metro_le_plus_proche; 
        }

        public Client() { }

    }
}
