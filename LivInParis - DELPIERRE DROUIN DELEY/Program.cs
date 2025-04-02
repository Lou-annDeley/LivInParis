using System;
using System.Collections.Generic;
using System.IO;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using SkiaSharp;
using MySql.Data.MySqlClient;
using DocumentFormat.OpenXml.EMMA;
using Mysqlx.Crud;
using System.Net.Sockets;
using DocumentFormat.OpenXml.Drawing;
using MySqlX.XDevAPI;

namespace LivInParis___DELPIERRE_DROUIN_DELEY
{
    class Program
    {
        
        static void Main(string[] args)
        {

            MySqlConnection maConnexion = null;
            try
            {
                string connectionString = "SERVER=localhost; PORT=3306;" + "DATABASE=Livraison;" + "UID=root;PASSWORD=root";
                maConnexion = new MySqlConnection(connectionString);
                maConnexion.Open();

            }
            catch (MySqlException e)
            {
                Console.WriteLine("ErreurConnexion :" + e.ToString());
                return;
            }


            Console.WriteLine("Choisissez une action : 1. Gérer Clients | 2. Gérer Cuisiniers | 3. Gérer Commandes | 4. Statistiques | 5. Quitter");
            string choix = Console.ReadLine();

            if (choix == "1")
            {
                Console.WriteLine("1. Ajouter | 2. Modifier | 3. Supprimer | 4. Afficher | 5. Retour");
                string choixClient = Console.ReadLine();

                if (choixClient == "1")
                {
                    Console.WriteLine("Etes vous un 1.Particulier ou 2.Entreprise");
                    int statut = Convert.ToInt32(Console.ReadLine());
                    if(statut == 1)
                    {
                        Console.WriteLine("Quel est votre identifiant client?");
                        int idClient = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Quel est votre numéro de téléphone?");
                        int telClient = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Quel est votre adresse mail?");
                        string mailClient = Console.ReadLine();
                        Console.WriteLine("Quel est votre ville?");
                        string villeClient = Console.ReadLine();
                        Console.WriteLine("Quel est votre numéro de rue?");
                        int numrueClient = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Quel est votre rue?");
                        string rueClient = Console.ReadLine();
                        Console.WriteLine("Quel est votre code postal?");
                        int codepClient = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Quel est le métro le plus proche?");
                        string metroClient = Console.ReadLine();
                        string creationClient = "insert into Client(id_particulier, telephone, adresse_mail, ville, numero_de_rue, rue, code_postal, metro_le_plus_proche) Values(" + idClient + ", " + telClient + ",'" + mailClient + "','" + villeClient + "'," + numrueClient + ",'" + rueClient + "'," + codepClient + "," + metroClient + ");";
                        MySqlCommand creaClient = maConnexion.CreateCommand();
                        creaClient.CommandText = creationClient;
                        MySqlDataReader reader = creaClient.ExecuteReader();
                        reader.Close();
                        creaClient.Dispose();

                        Console.WriteLine("Quel est votre nom?");
                        string nomParti = Console.ReadLine();
                        Console.WriteLine("Quel est votre prénom?");
                        string prenomParti = Console.ReadLine();
                        string creationParticulier = "insert into particulier(id_client, nom, prenom, id_client) Values(" + idClient + ", '" + nomParti + "', '" + prenomParti + "'," + idClient + ");";
                        MySqlCommand creaParti = maConnexion.CreateCommand();
                        creaParti.CommandText = creationParticulier;
                        MySqlDataReader reader2 = creaParti.ExecuteReader();
                        reader2.Close();
                        creaParti.Dispose();
                        Console.WriteLine("Ajout d'un particulier");
                    }
                    else if (statut == 2)
                    {
                        Console.WriteLine("Quel est votre identifiant client?");
                        int idClient = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Quel est votre numéro de téléphone?");
                        int telClient = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Quel est votre adresse mail?");
                        string mailClient = Console.ReadLine();
                        Console.WriteLine("Quel est votre ville?");
                        string villeClient = Console.ReadLine();
                        Console.WriteLine("Quel est votre numéro de rue?");
                        int numrueClient = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Quel est votre rue?");
                        string rueClient = Console.ReadLine();
                        Console.WriteLine("Quel est votre code postal?");
                        int codepClient = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Quel est le métro le plus proche?");
                        string metroClient = Console.ReadLine();
                        string creationClient = "insert into Client(id_particulier, telephone, adresse_mail, ville, numero_de_rue, rue, code_postal, metro_le_plus_proche) Values(" + idClient + ", " + telClient + ",'" + mailClient + "','" + villeClient + "'," + numrueClient + ",'" + rueClient + "'," + codepClient + "," + metroClient + ");";
                        MySqlCommand creaClient = maConnexion.CreateCommand();
                        creaClient.CommandText = creationClient;
                        MySqlDataReader reader = creaClient.ExecuteReader();
                        reader.Close();
                        creaClient.Dispose();

                        Console.WriteLine("Quel est votre nom de référent?");
                        string nomrefEnt = Console.ReadLine();
                        Console.WriteLine("Quel est votre siret d'entreprise?");
                        int siretEnt = Convert.ToInt32(Console.ReadLine());
                        string creationEntreprise = "insert into Entreprise(Siret, nom_référent, id_client) Values(" + siretEnt + ", '" + nomrefEnt + "'," + idClient + ");";
                        MySqlCommand creaEnt = maConnexion.CreateCommand();
                        creaEnt.CommandText = creationEntreprise;
                        MySqlDataReader reader2 = creaEnt.ExecuteReader();
                        reader2.Close();
                        creaEnt.Dispose();
                        Console.WriteLine("Ajout d'une entreprise");
                    }
                }


                else if (choixClient == "2")
                {
                    Console.WriteLine("Quel est votre identifiant ?");
                    int idClientModif = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Que voulez vous modifier ?");

                    Console.WriteLine("1. Télephone | 2. Adresse mail | 3. Ville | 4. Numéro de rue | 5. Rue | 6. Code Postal | 7. Metro le plus proche | 8.Quitter");
                    int choixClientModif = Convert.ToInt32(Console.ReadLine());
                    while (choixClientModif != 8)
                    {
                        if (choixClientModif == 1)
                        {
                            Console.WriteLine("Quel est le nouveau Numéro de téléphone ?");
                            int tel = Convert.ToInt32(Console.ReadLine());
                            string modifClient = "update Client set telephone = " + tel + " where id_Client = " + idClientModif + ";";
                            MySqlCommand creaClient = maConnexion.CreateCommand();
                            creaClient.CommandText = modifClient;
                            MySqlDataReader reader = creaClient.ExecuteReader();
                            reader.Close();
                            creaClient.Dispose();
                            Console.WriteLine("Modification d'un client");
                        }
                        else if (choixClientModif == 2)
                        {
                            Console.WriteLine("Quelle est la nouvelle adresse mail ?");
                            string mail = Console.ReadLine();
                            string modifClient = "update Client set adresse_mail = '" + mail + "' where id_Client = " + idClientModif + ";";
                            MySqlCommand creaClient = maConnexion.CreateCommand();
                            creaClient.CommandText = modifClient;
                            MySqlDataReader reader = creaClient.ExecuteReader();
                            reader.Close();
                            creaClient.Dispose();
                            Console.WriteLine("Modification d'un client");
                        }
                        else if (choixClientModif == 3)
                        {
                            Console.WriteLine("Quelle est la nouvelle ville ?");
                            string ville = Console.ReadLine();
                            string modifClient = "update Client set ville = '" + ville + "' where id_Client = " + idClientModif + ";";
                            MySqlCommand creaClient = maConnexion.CreateCommand();
                            creaClient.CommandText = modifClient;
                            MySqlDataReader reader = creaClient.ExecuteReader();
                            reader.Close();
                            creaClient.Dispose();
                            Console.WriteLine("Modification d'un client");
                        }
                        else if (choixClientModif == 4)
                        {
                            Console.WriteLine("Quel est le nouveau Numéro de rue ?");
                            int num_rue = Convert.ToInt32(Console.ReadLine());
                            string modifClient = "update Client set numero_de_rue = " + num_rue + " where id_Client = " + idClientModif + ";";
                            MySqlCommand creaClient = maConnexion.CreateCommand();
                            creaClient.CommandText = modifClient;
                            MySqlDataReader reader = creaClient.ExecuteReader();
                            reader.Close();
                            creaClient.Dispose();
                            Console.WriteLine("Modification d'un client");
                        }
                        else if (choixClientModif == 5)
                        {
                            Console.WriteLine("Quelle est la nouvelle rue ?");
                            string rue = Console.ReadLine();
                            string modifClient = "update Client set rue = '" + rue + "' where id_Client = " + idClientModif + ";";
                            MySqlCommand creaClient = maConnexion.CreateCommand();
                            creaClient.CommandText = modifClient;
                            MySqlDataReader reader = creaClient.ExecuteReader();
                            reader.Close();
                            creaClient.Dispose();
                            Console.WriteLine("Modification d'un client");
                        }
                        else if (choixClientModif == 6)
                        {
                            Console.WriteLine("Quel est le nouveau code postal ?");
                            int codep = Convert.ToInt32(Console.ReadLine());
                            string modifClient = "update Client set code_postal = " + codep + " where id_Client = " + idClientModif + ";";
                            MySqlCommand creaClient = maConnexion.CreateCommand();
                            creaClient.CommandText = modifClient;
                            MySqlDataReader reader = creaClient.ExecuteReader();
                            reader.Close();
                            creaClient.Dispose();
                            Console.WriteLine("Modification d'un client");
                        }
                        else if (choixClientModif == 7)
                        {
                            Console.WriteLine("Quel est le nouveau métro le plus proche ?");
                            string metrop = Console.ReadLine();
                            string modifClient = "update Client set metro_le_plus_proche = '" + metrop + "' where id_Client = " + idClientModif + ";";
                            MySqlCommand creaClient = maConnexion.CreateCommand();
                            creaClient.CommandText = modifClient;
                            MySqlDataReader reader = creaClient.ExecuteReader();
                            reader.Close();
                            creaClient.Dispose();
                            Console.WriteLine("Modification d'un client");
                        }

                        Console.WriteLine("1. Télephone | 2. Adresse mail | 3. Ville | 4. Numéro de rue | 5. Rue | 6. Code Postal | 7. Metro le plus proche | 8.Quitter");
                        choixClientModif = Convert.ToInt32(Console.ReadLine());
                    }
                    if (choixClientModif == 8)
                    {
                        return;
                    }
                }

                else if (choixClient == "3")
                {
                    Console.WriteLine("Quel est votre identifiant ?");
                    int idClientModif = Convert.ToInt32(Console.ReadLine());
                    string modifClient = "delete (*) from Client where id_Client = " + idClientModif + ";";
                    MySqlCommand creaClient = maConnexion.CreateCommand();
                    creaClient.CommandText = modifClient;
                    MySqlDataReader reader = creaClient.ExecuteReader();
                    reader.Close();
                    creaClient.Dispose();
                    Console.WriteLine("Suppression d'un client");
                }

                else if (choixClient == "4")
                {
                    Console.WriteLine("1. Par ordre alphabétique | 2. Par rue | 3. Par montant des achats cumulés | 4. Quitter");
                    int choixClientModif = Convert.ToInt32(Console.ReadLine());
                    if (choixClientModif == 1)
                    {
                        string affichageClient = "select * from Client order by (select nom from particulier) asc;";
                        MySqlCommand affichClient = maConnexion.CreateCommand();
                        affichClient.CommandText = affichageClient;
                        MySqlDataReader reader = affichClient.ExecuteReader();

                        string[] valueString = new string[reader.FieldCount];

                        while (reader.Read())
                        {
                            

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string first_name = (string)reader[i];
                                Console.WriteLine(first_name);
                                valueString[i] = reader.GetValue(i).ToString();
                                Console.Write(valueString[i] + " ");
                            }

                            Console.WriteLine();
                        }

                        reader.Close();
                        affichClient.Dispose();
                        Console.WriteLine("Affichage des clients");
                    }
                    else if (choixClientModif == 2)
                    {

                    }
                    else if (choixClientModif == 3)
                    {

                    }
                }
            }
            else if (choix == "2")
            {
                Console.WriteLine("1. Ajouter | 2. Modifier | 3. Supprimer | 4. Afficher | 5. Retour");
                string choixCuisinier = Console.ReadLine();

                if (choixCuisinier == "1") Console.WriteLine("Ajout d'un cuisinier");
                else if (choixCuisinier == "2") Console.WriteLine("Modification d'un cuisinier");
                else if (choixCuisinier == "3") Console.WriteLine("Suppression d'un cuisinier");
                else if (choixCuisinier == "4") Console.WriteLine("Affichage des cuisiniers");
            }
            else if (choix == "3")
            {
                Console.WriteLine("Gestion des commandes");
            }
            else if (choix == "4")
            {
                Console.WriteLine("Statistiques");

                //string query = "SELECT cuisinier_id, COUNT(*) AS total FROM Commande GROUP BY cuisinier_id";
                //using (MySqlConnection conn = new MySqlConnection(connectionString))
                //{
                //    conn.Open();
                //    MySqlCommand cmd = new MySqlCommand(query, conn);
                //    MySqlDataReader reader = cmd.ExecuteReader();
                //    while (reader.Read())
                //    {
                //        Console.WriteLine($"Cuisinier {reader["cuisinier_id"]}: {reader["total"]} livraisons");
                //    }
                //}

                //Console.Write("Numéro de commande: ");
                //int idCommande = int.Parse(Console.ReadLine());
                //string prixQuery = "SELECT prix FROM Commande WHERE id_commande = @id";
                //using (MySqlConnection conn = new MySqlConnection(connectionString))
                //{
                //    conn.Open();
                //    MySqlCommand cmd = new MySqlCommand(prixQuery, conn);
                //    cmd.Parameters.AddWithValue("@id", idCommande);
                //    object result = cmd.ExecuteScalar();
                //    if (result != null)
                //    {
                //        Console.WriteLine($"Le prix de la commande est : {result} euros");
                //    }
                //    else
                //    {
                //        Console.WriteLine("Commande non trouvée.");
                //    }
                //}
            }
            else if (choix == "5")
            {
                return;
            }
            else
            {
                Console.WriteLine("Option invalide.");
            }
            //maConnexion.Close();
            Console.ReadLine();


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
                foreach (var row in sheetNoeuds.RowsUsed().Skip(1)) // Ignore la première ligne (titres)
                {
                    if(!row.Cell(6).IsEmpty())
                    {
                        int id = row.Cell(1).GetValue<int>();
                        string nom_station = row.Cell(2).GetValue<string>();
                        int poids_correspondance = row.Cell(6).GetValue<int>();
                        foreach (var noeud in noeuds)
                        {
                            if(noeud.Value.Valeur == nom_station)
                            {
                                int id_correspodance = noeud.Key;
                                if (noeuds[id].ExisteLien(noeuds[id_correspodance]) == false)
                                {
                                    metro.AjouterLien(noeuds[id], noeuds[id_correspodance], poids_correspondance);
                                }
                            }
                        }
                    }
                }
            }



            //Afficher les chemins les plus courts

            //List<Noeud<string>> plus_petit_cheminS2 = metro.Dijkstra(noeuds[25], noeuds[235]);
            //Console.WriteLine("Nouveau djikstra, le chemin est ");
            //foreach (Noeud<string> noeud1 in plus_petit_cheminS2)
            //{
            //    Console.Write(noeud1.Valeur + " ");
            //}
            //Console.WriteLine();

            //List<Noeud<string>> plus_petit_cheminS3 = metro.BellmanFord(noeuds[1], noeuds[38]);
            //Console.WriteLine("Nouveau bellman, le chemin est ");
            //foreach (Noeud<string> noeud1 in plus_petit_cheminS3)
            //{
            //    Console.Write(noeud1.Valeur + " -> ");
            //}

            // Appel de FloydWarshall
            List<Noeud<string>> plus_petit_cheminS3 = metro.FloydWarshall(noeuds[1], noeuds[38]);
            Console.WriteLine("Nouveau bellman, le chemin est ");
            foreach (Noeud<string> noeud1 in plus_petit_cheminS3)
            {
                Console.Write(noeud1.Valeur + " -> ");
            }


            // Dessiner le graphe et sauvegarder l'image
            metro.DessinerGraphe("graphe.png");
            Console.WriteLine("Graphe généré");
            metro.MettreEnEvidenceChemin(plus_petit_cheminS3, "court_chemin.png");
            Console.WriteLine("Graphe avec plus court chemin généré");



        }
       
    }
}

