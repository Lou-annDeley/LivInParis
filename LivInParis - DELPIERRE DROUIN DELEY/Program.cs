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

            //A FAIRE
            //
            // Client Afficher par commande
            // Cusinier Afficher
            // Commande Ajouter
            // Commande Modifier
            // Commande Afficher
            // Statistiques



            Console.WriteLine("Choisissez une action : 1. Gérer Clients | 2. Gérer Cuisiniers | 3. Gérer Commandes | 4. Statistiques | 5. Quitter");
            int choix = Convert.ToInt32(Console.ReadLine());

            while(choix != 5)
            {
                if (choix == 1) //Client
                {
                    Console.WriteLine("1. Ajouter | 2. Modifier | 3. Supprimer | 4. Afficher | 5. Quitter");
                    int choixClient = Convert.ToInt32(Console.ReadLine());

                    while(choixClient != 5)
                    {
                        if (choixClient == 1) //AJOUT
                        {
                            Console.WriteLine("Etes vous un 1.Particulier ou 2.Entreprise");
                            int statut = Convert.ToInt32(Console.ReadLine());
                            if (statut == 1)
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

                        else if (choixClient == 2) //MODIFIER
                        {
                            Console.WriteLine("Quel est votre identifiant ?");
                            int idClientModif = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Etes vous un 1. Particulier ou 2. Entreprise ?");
                            int statut = Convert.ToInt32(Console.ReadLine());
                            if(statut == 1) //MODIF PARTICULIER
                            {
                                Console.WriteLine("Que voulez vous modifier ?");
                                Console.WriteLine("1. Télephone | 2. Adresse mail | 3. Ville | 4. Numéro de rue | 5. Rue | 6. Code Postal | 7. Metro le plus proche | 8. Nom | 9. Prénom | 10.Quitter");
                                int choixClientModif = Convert.ToInt32(Console.ReadLine());
                                while (choixClientModif != 10)
                                {
                                    if (choixClientModif == 1)
                                    {
                                        Console.WriteLine("Quel est le nouveau Numéro de téléphone ?");
                                        int tel = Convert.ToInt32(Console.ReadLine());
                                        string modifClient = "update Client set telephone = " + tel + " where id_Client = " + idClientModif + ";";
                                        MySqlCommand modificationClient = maConnexion.CreateCommand();
                                        modificationClient.CommandText = modifClient;
                                        MySqlDataReader reader = modificationClient.ExecuteReader();
                                        reader.Close();
                                        modificationClient.Dispose();
                                        Console.WriteLine("Modification d'un client");
                                    }
                                    else if (choixClientModif == 2)
                                    {
                                        Console.WriteLine("Quelle est la nouvelle adresse mail ?");
                                        string mail = Console.ReadLine();
                                        string modifClient = "update Client set adresse_mail = '" + mail + "' where id_Client = " + idClientModif + ";";
                                        MySqlCommand modificationClient = maConnexion.CreateCommand();
                                        modificationClient.CommandText = modifClient;
                                        MySqlDataReader reader = modificationClient.ExecuteReader();
                                        reader.Close();
                                        modificationClient.Dispose();
                                        Console.WriteLine("Modification d'un client");
                                    }
                                    else if (choixClientModif == 3)
                                    {
                                        Console.WriteLine("Quelle est la nouvelle ville ?");
                                        string ville = Console.ReadLine();
                                        string modifClient = "update Client set ville = '" + ville + "' where id_Client = " + idClientModif + ";";
                                        MySqlCommand modificationClient = maConnexion.CreateCommand();
                                        modificationClient.CommandText = modifClient;
                                        MySqlDataReader reader = modificationClient.ExecuteReader();
                                        reader.Close();
                                        modificationClient.Dispose();
                                        Console.WriteLine("Modification d'un client");
                                    }
                                    else if (choixClientModif == 4)
                                    {
                                        Console.WriteLine("Quel est le nouveau Numéro de rue ?");
                                        int num_rue = Convert.ToInt32(Console.ReadLine());
                                        string modifClient = "update Client set numero_de_rue = " + num_rue + " where id_Client = " + idClientModif + ";";
                                        MySqlCommand modificationClient = maConnexion.CreateCommand();
                                        modificationClient.CommandText = modifClient;
                                        MySqlDataReader reader = modificationClient.ExecuteReader();
                                        reader.Close();
                                        modificationClient.Dispose();
                                        Console.WriteLine("Modification d'un client");
                                    }
                                    else if (choixClientModif == 5)
                                    {
                                        Console.WriteLine("Quelle est la nouvelle rue ?");
                                        string rue = Console.ReadLine();
                                        string modifClient = "update Client set rue = '" + rue + "' where id_Client = " + idClientModif + ";";
                                        MySqlCommand modificationClient = maConnexion.CreateCommand();
                                        modificationClient.CommandText = modifClient;
                                        MySqlDataReader reader = modificationClient.ExecuteReader();
                                        reader.Close();
                                        modificationClient.Dispose();
                                        Console.WriteLine("Modification d'un client");
                                    }
                                    else if (choixClientModif == 6)
                                    {
                                        Console.WriteLine("Quel est le nouveau code postal ?");
                                        int codep = Convert.ToInt32(Console.ReadLine());
                                        string modifClient = "update Client set code_postal = " + codep + " where id_Client = " + idClientModif + ";";
                                        MySqlCommand modificationClient = maConnexion.CreateCommand();
                                        modificationClient.CommandText = modifClient;
                                        MySqlDataReader reader = modificationClient.ExecuteReader();
                                        reader.Close();
                                        modificationClient.Dispose();
                                        Console.WriteLine("Modification d'un client");
                                    }
                                    else if (choixClientModif == 7)
                                    {
                                        Console.WriteLine("Quel est le nouveau métro le plus proche ?");
                                        string metrop = Console.ReadLine();
                                        string modifClient = "update Client set metro_le_plus_proche = '" + metrop + "' where id_Client = " + idClientModif + ";";
                                        MySqlCommand modificationClient = maConnexion.CreateCommand();
                                        modificationClient.CommandText = modifClient;
                                        MySqlDataReader reader = modificationClient.ExecuteReader();
                                        reader.Close();
                                        modificationClient.Dispose();
                                        Console.WriteLine("Modification d'un client");
                                    }
                                    else if (choixClientModif == 8)
                                    {
                                        Console.WriteLine("Quel est le nouveau nom ?");
                                        string nom = Console.ReadLine();
                                        string modifClient = "update particulier set nom = '" + nom + "' where id_particulier = " + idClientModif + ";";
                                        MySqlCommand modificationClient = maConnexion.CreateCommand();
                                        modificationClient.CommandText = modifClient;
                                        MySqlDataReader reader = modificationClient.ExecuteReader();
                                        reader.Close();
                                        modificationClient.Dispose();
                                        Console.WriteLine("Modification d'un particulier");
                                    }
                                    else if (choixClientModif == 9)
                                    {
                                        Console.WriteLine("Quel est le nouveau prénom ?");
                                        string prenom = Console.ReadLine();
                                        string modifClient = "update particulier set prenom = '" + prenom + "' where id_particulier = " + idClientModif + ";";
                                        MySqlCommand modificationClient = maConnexion.CreateCommand();
                                        modificationClient.CommandText = modifClient;
                                        MySqlDataReader reader = modificationClient.ExecuteReader();
                                        reader.Close();
                                        modificationClient.Dispose();
                                        Console.WriteLine("Modification d'un particulier");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Option invalide.");
                                    }

                                    Console.WriteLine("1. Télephone | 2. Adresse mail | 3. Ville | 4. Numéro de rue | 5. Rue | 6. Code Postal | 7. Metro le plus proche | 8. Nom | 9. Prénom | 10.Quitter");
                                    choixClientModif = Convert.ToInt32(Console.ReadLine());
                                }
                                if (choixClientModif == 10)
                                {
                                    return;
                                }
                            }

                            else if(statut == 2) //MODIF ENTREPRISE
                            {
                                Console.WriteLine("Que voulez vous modifier ?");
                                Console.WriteLine("1. Télephone | 2. Adresse mail | 3. Ville | 4. Numéro de rue | 5. Rue | 6. Code Postal | 7. Metro le plus proche | 8. Nom référent | 9.Quitter");
                                int choixClientModif = Convert.ToInt32(Console.ReadLine());
                                while (choixClientModif != 9)
                                {
                                    if (choixClientModif == 1)
                                    {
                                        Console.WriteLine("Quel est le nouveau Numéro de téléphone ?");
                                        int tel = Convert.ToInt32(Console.ReadLine());
                                        string modifClient = "update Client set telephone = " + tel + " where id_Client = " + idClientModif + ";";
                                        MySqlCommand modificationClient = maConnexion.CreateCommand();
                                        modificationClient.CommandText = modifClient;
                                        MySqlDataReader reader = modificationClient.ExecuteReader();
                                        reader.Close();
                                        modificationClient.Dispose();
                                        Console.WriteLine("Modification d'un client");
                                    }
                                    else if (choixClientModif == 2)
                                    {
                                        Console.WriteLine("Quelle est la nouvelle adresse mail ?");
                                        string mail = Console.ReadLine();
                                        string modifClient = "update Client set adresse_mail = '" + mail + "' where id_Client = " + idClientModif + ";";
                                        MySqlCommand modificationClient = maConnexion.CreateCommand();
                                        modificationClient.CommandText = modifClient;
                                        MySqlDataReader reader = modificationClient.ExecuteReader();
                                        reader.Close();
                                        modificationClient.Dispose();
                                        Console.WriteLine("Modification d'un client");
                                    }
                                    else if (choixClientModif == 3)
                                    {
                                        Console.WriteLine("Quelle est la nouvelle ville ?");
                                        string ville = Console.ReadLine();
                                        string modifClient = "update Client set ville = '" + ville + "' where id_Client = " + idClientModif + ";";
                                        MySqlCommand modificationClient = maConnexion.CreateCommand();
                                        modificationClient.CommandText = modifClient;
                                        MySqlDataReader reader = modificationClient.ExecuteReader();
                                        reader.Close();
                                        modificationClient.Dispose();
                                        Console.WriteLine("Modification d'un client");
                                    }
                                    else if (choixClientModif == 4)
                                    {
                                        Console.WriteLine("Quel est le nouveau Numéro de rue ?");
                                        int num_rue = Convert.ToInt32(Console.ReadLine());
                                        string modifClient = "update Client set numero_de_rue = " + num_rue + " where id_Client = " + idClientModif + ";";
                                        MySqlCommand modificationClient = maConnexion.CreateCommand();
                                        modificationClient.CommandText = modifClient;
                                        MySqlDataReader reader = modificationClient.ExecuteReader();
                                        reader.Close();
                                        modificationClient.Dispose();
                                        Console.WriteLine("Modification d'un client");
                                    }
                                    else if (choixClientModif == 5)
                                    {
                                        Console.WriteLine("Quelle est la nouvelle rue ?");
                                        string rue = Console.ReadLine();
                                        string modifClient = "update Client set rue = '" + rue + "' where id_Client = " + idClientModif + ";";
                                        MySqlCommand modificationClient = maConnexion.CreateCommand();
                                        modificationClient.CommandText = modifClient;
                                        MySqlDataReader reader = modificationClient.ExecuteReader();
                                        reader.Close();
                                        modificationClient.Dispose();
                                        Console.WriteLine("Modification d'un client");
                                    }
                                    else if (choixClientModif == 6)
                                    {
                                        Console.WriteLine("Quel est le nouveau code postal ?");
                                        int codep = Convert.ToInt32(Console.ReadLine());
                                        string modifClient = "update Client set code_postal = " + codep + " where id_Client = " + idClientModif + ";";
                                        MySqlCommand modificationClient = maConnexion.CreateCommand();
                                        modificationClient.CommandText = modifClient;
                                        MySqlDataReader reader = modificationClient.ExecuteReader();
                                        reader.Close();
                                        modificationClient.Dispose();
                                        Console.WriteLine("Modification d'un client");
                                    }
                                    else if (choixClientModif == 7)
                                    {
                                        Console.WriteLine("Quel est le nouveau métro le plus proche ?");
                                        string metrop = Console.ReadLine();
                                        string modifClient = "update Client set metro_le_plus_proche = '" + metrop + "' where id_Client = " + idClientModif + ";";
                                        MySqlCommand modificationClient = maConnexion.CreateCommand();
                                        modificationClient.CommandText = modifClient;
                                        MySqlDataReader reader = modificationClient.ExecuteReader();
                                        reader.Close();
                                        modificationClient.Dispose();
                                        Console.WriteLine("Modification d'un client");
                                    }
                                    else if (choixClientModif == 8)
                                    {
                                        Console.WriteLine("Quel est le nouveau nom de référent ?");
                                        string nomRef = Console.ReadLine();
                                        string modifClient = "update Entreprise set nom_référent = '" + nomRef + "' where Siret = " + idClientModif + ";";
                                        MySqlCommand modificationClient = maConnexion.CreateCommand();
                                        modificationClient.CommandText = modifClient;
                                        MySqlDataReader reader = modificationClient.ExecuteReader();
                                        reader.Close();
                                        modificationClient.Dispose();
                                        Console.WriteLine("Modification d'une entreprise");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Option invalide.");
                                    }

                                    Console.WriteLine("1. Télephone | 2. Adresse mail | 3. Ville | 4. Numéro de rue | 5. Rue | 6. Code Postal | 7. Metro le plus proche | 9. Nom référent | 10.Quitter");
                                    choixClientModif = Convert.ToInt32(Console.ReadLine());
                                }
                                if (choixClientModif == 9)
                                {
                                    return;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Option invalide.");
                            }
                        }

                        else if (choixClient == 3) //SUPPRIMER
                        {
                            Console.WriteLine("Quel est votre identifiant ?");
                            int idClientModif = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Etes vous un 1. Particulier ou 2. Entreprise ?");
                            int statut = Convert.ToInt32(Console.ReadLine());
                            if(statut == 1)
                            {
                                string suppressionClient = "delete from Client where id_Client = " + idClientModif + ";";
                                MySqlCommand suppClient = maConnexion.CreateCommand();
                                suppClient.CommandText = suppressionClient;
                                MySqlDataReader reader = suppClient.ExecuteReader();
                                reader.Close();
                                suppClient.Dispose();
                                Console.WriteLine("Suppression d'un client");

                                string suppressionParticulier = "delete from particulier where id_particulier = " + idClientModif + ";";
                                MySqlCommand suppParti = maConnexion.CreateCommand();
                                suppParti.CommandText = suppressionParticulier;
                                MySqlDataReader reader2 = suppParti.ExecuteReader();
                                reader2.Close();
                                suppParti.Dispose();
                                Console.WriteLine("Suppression d'un particulier");
                            }
                            else if (statut == 2)
                            {
                                string suppressionClient = "delete from Client where id_Client = " + idClientModif + ";";
                                MySqlCommand suppClient = maConnexion.CreateCommand();
                                suppClient.CommandText = suppressionClient;
                                MySqlDataReader reader = suppClient.ExecuteReader();
                                reader.Close();
                                suppClient.Dispose();
                                Console.WriteLine("Suppression d'un client");

                                string suppressionEntreprise = "delete from Entreprise where Siret = " + idClientModif + ";";
                                MySqlCommand suppEntr = maConnexion.CreateCommand();
                                suppEntr.CommandText = suppressionEntreprise;
                                MySqlDataReader reader2 = suppEntr.ExecuteReader();
                                reader2.Close();
                                suppEntr.Dispose();
                                Console.WriteLine("Suppression d'une entreprise");
                            }
                            else
                            {
                                Console.WriteLine("Option invalide.");
                            }
                        }

                        else if (choixClient == 4) //AFFICHER
                        {
                            Console.WriteLine("1. Par ordre alphabétique | 2. Par rue | 3. Par montant des achats cumulés | 4. Quitter");
                            int choixClientModif = Convert.ToInt32(Console.ReadLine());
                            while(choixClientModif != 4)
                            {
                                if (choixClientModif == 1) //ordre alphabétique
                                {
                                    string affichageClient = "select nom from particulier union select nom_référent as nom from entreprise order by nom asc;";
                                    MySqlCommand affichClient = maConnexion.CreateCommand();
                                    affichClient.CommandText = affichageClient;
                                    MySqlDataReader reader = affichClient.ExecuteReader();

                                    string[] valueString = new string[reader.FieldCount];

                                    while (reader.Read())
                                    {


                                        for (int i = 0; i < reader.FieldCount; i++)
                                        {
                                            valueString[i] = reader.GetValue(i).ToString();
                                            Console.Write(valueString[i] + " ");
                                        }

                                        Console.WriteLine();
                                    }

                                    reader.Close();
                                    affichClient.Dispose();
                                    Console.WriteLine("Affichage des clients");
                                }
                                else if (choixClientModif == 2) //par rue
                                {
                                    string affichageClient = "select * from Client order by rue asc;";
                                    MySqlCommand affichClient = maConnexion.CreateCommand();
                                    affichClient.CommandText = affichageClient;
                                    MySqlDataReader reader = affichClient.ExecuteReader();

                                    string[] valueString = new string[reader.FieldCount];

                                    while (reader.Read())
                                    {


                                        for (int i = 0; i < reader.FieldCount; i++)
                                        {
                                            valueString[i] = reader.GetValue(i).ToString();
                                            Console.Write(valueString[i] + " ");
                                        }

                                        Console.WriteLine();
                                    }

                                    reader.Close();
                                    affichClient.Dispose();
                                    Console.WriteLine("Affichage des clients");

                                }
                                else if (choixClientModif == 3) // par montant des achats cumulés
                                {

                                }
                                Console.WriteLine("1. Par ordre alphabétique | 2. Par rue | 3. Par montant des achats cumulés | 4. Quitter");
                                choixClientModif = Convert.ToInt32(Console.ReadLine());
                            }
                            if (choixClientModif == 4)
                            {
                                return;
                            }
                        }

                    Console.WriteLine("1. Ajouter | 2. Modifier | 3. Supprimer | 4. Afficher | 5. Quitter");
                    choixClient = Convert.ToInt32(Console.ReadLine());

                    }
                    if (choixClient == 5)
                    {
                        return;
                    }

                }
                else if (choix == 2) //Cuisiner
                {
                    Console.WriteLine("1. Ajouter | 2. Modifier | 3. Supprimer | 4. Afficher | 5. Ajouter un plat | 6. Quitter");
                    int choixCuisinier = Convert.ToInt32(Console.ReadLine());

                    while (choixCuisinier != 6)
                    {
                        if (choixCuisinier == 1) //AJOUT
                        {
                            
                            Console.WriteLine("Quel est votre identifiant cuisinier?");
                            int idcuisinier = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Quel est votre prénom?");
                            string prenomcuisinier = Console.ReadLine();
                            Console.WriteLine("Quel est votre nom?");
                            string nomcuisinier = Console.ReadLine();
                            Console.WriteLine("Quel est votre téléphone?");
                            int telCuisinier = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Quelle est votre mail");
                            string mailCuisinier = Console.ReadLine();
                            Console.WriteLine("Quel est votre métro le plus proche?");
                            string metroCuisinier = Console.ReadLine();
                            Console.WriteLine("Quelle est votre rue?");
                            string rueCuisinier = Console.ReadLine();
                            Console.WriteLine("Quelle est votre ville?");
                            string villeCuisinier = Console.ReadLine();
                            Console.WriteLine("Quelle est votre numéro de rue?");
                            int numerorueCuisinier = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Quel est votre code postal?");
                            int codepCuisinier = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Quel est votre id client (attention il doit exister)");
                            int idclient = Convert.ToInt32(Console.ReadLine());
                            string creationCuisinier = "insert into Cuisinier(id_Cuisinier,prénom,nom,téléphone,adresse_mail,metro_le_plus_proche,rue_,ville,numéro_rue,code_postal, id_client) Values(" + idcuisinier + ", '" + prenomcuisinier + "','" + nomcuisinier + "'," + telCuisinier + ",'" + mailCuisinier + "','" + metroCuisinier + "','" + rueCuisinier + "','" + villeCuisinier + "',"+ numerorueCuisinier+ ","+codepCuisinier+","+idclient+"); ";
                            MySqlCommand creaCuisinier = maConnexion.CreateCommand();
                            creaCuisinier.CommandText = creationCuisinier;
                            MySqlDataReader reader = creaCuisinier.ExecuteReader();
                            reader.Close();
                            creaCuisinier.Dispose();

                            Console.WriteLine("Combien de plats proposez-vous ?");
                            int nbPlat =  Convert.ToInt32(Console.ReadLine());  
                            for(int i = 0; i < nbPlat; i++)
                            {
                                Console.WriteLine("Quel est l'identifiant du plat?");
                                int idPlat = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("Quel est le nom du plat?");
                                string nomPlat = Console.ReadLine();
                                Console.WriteLine("Quel est le prix?");
                                int prixPlat = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("Quel est la recette?");
                                string recettePlat = Console.ReadLine();
                                Console.WriteLine("Pour combien de personnes?");
                                int nbpersonnesPlat = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("Quel est la date de péremption?");
                                DateTime dateperemptionPlat = Convert.ToDateTime(Console.ReadLine());
                                Console.WriteLine("Quelle est la date de fabrication?");
                                DateTime datefabricationPlat = Convert.ToDateTime(Console.ReadLine());
                                Console.WriteLine("Quelle est la nationalité?");
                                string nationalitePlat = Console.ReadLine();
                                Console.WriteLine("Quelle est le régime alimentaire?");
                                string regimePlat = Console.ReadLine();

                                string creationPlat = "insert into Plat(id_Plat,nom,prix,recette,nb_personnes,date_de_péremption,date_de_fabrication,nationalité,régime_alimentaire,id_Cuisinier) Values(" + idPlat + ", '" + nomPlat + "'," + prixPlat + ",'" + recettePlat + "'," + nbpersonnesPlat + ",'" + dateperemptionPlat + "','" + datefabricationPlat + "','" + nationalitePlat + "','" + regimePlat + "'," + idcuisinier + "); ";
                                MySqlCommand creaPlat = maConnexion.CreateCommand();
                                creaPlat.CommandText = creationPlat;
                                MySqlDataReader reader3 = creaPlat.ExecuteReader();
                                reader3.Close();
                                creaPlat.Dispose();

                                Console.WriteLine("Combien d'ingrédients utilisez-vous?");
                                int nbIngredients = Convert.ToInt32(Console.ReadLine());
                                for(int j = 0;  j < nbIngredients; j++)
                                {
                                    Console.WriteLine("Quel est l'identifiant de l'ingrédient?");
                                    int idIngrédient = Convert.ToInt32(Console.ReadLine());
                                    Console.WriteLine("Quel est le nom de l'ingrédient?");
                                    string nomIngredient = Console.ReadLine();
                                    Console.WriteLine("Quel est la quantité de l'ingrédient?");
                                    string quantiteIngredient = Console.ReadLine();
                                    Console.WriteLine("Quel est la date de peremption de l'ingrédient?");
                                    DateTime peremptionIngredient = Convert.ToDateTime(Console.ReadLine());
                                    string creationIngredient= "insert into Ingredients(id_Ingrédient,nom,quantité,date_de_péremption) Values(" + idIngrédient + ", '" + nomIngredient + "','" + quantiteIngredient + "','" + peremptionIngredient + "'); ";
                                    MySqlCommand creaIngredient = maConnexion.CreateCommand();
                                    creaIngredient.CommandText = creationIngredient;
                                    MySqlDataReader reader4 = creaIngredient.ExecuteReader();
                                    reader4.Close();
                                    creaIngredient.Dispose();
                                }
                            }
                        }

                        else if (choixCuisinier == 2) //MODIFIER
                        {
                            Console.WriteLine("Quel est votre id ?");
                            int idCuisinierModif = Convert.ToInt32(Console.ReadLine());


                            Console.WriteLine("Que voulez vous modifier ?");
                            Console.WriteLine("1. prénom | 2. nom | 3. téléphone | 4. adresse_mail | 5. metro_le_plus_proche | 6. rue_ | 7. ville | 8. numéro_rue | 9. code_postal | 10.Quitter");
                            int choixCuisinierModif = Convert.ToInt32(Console.ReadLine());
                            while (choixCuisinierModif != 10)
                            {
                                if (choixCuisinierModif == 1)
                                {
                                    Console.WriteLine("Quel est le nouveau prénom?");
                                    string prenom = Console.ReadLine();
                                    string modifCuisinier = "update Cuisinier set prénom = " + prenom + " where id_Cuisinier = " + idCuisinierModif + ";";
                                    MySqlCommand modificationCuisinier = maConnexion.CreateCommand();
                                    modificationCuisinier.CommandText = modifCuisinier;
                                    MySqlDataReader reader = modificationCuisinier.ExecuteReader();
                                    reader.Close();
                                    modificationCuisinier.Dispose();
                                    Console.WriteLine("Modification d'un cuisinier");
                                }
                                else if (choixCuisinierModif == 2)
                                {
                                    Console.WriteLine("Quel est le nouveau nom ?");
                                    string nom = Console.ReadLine();
                                    string modifCuisinier = "update Cuisinier set nom = '" + nom + "' where id_Cuisinier = " + idCuisinierModif + ";";
                                    MySqlCommand modificationCuisinier = maConnexion.CreateCommand();
                                    modificationCuisinier.CommandText = modifCuisinier;
                                    MySqlDataReader reader = modificationCuisinier.ExecuteReader();
                                    reader.Close();
                                    modificationCuisinier.Dispose();
                                    Console.WriteLine("Modification d'un cuisinier");
                                }
                                else if (choixCuisinierModif == 3)
                                {
                                    Console.WriteLine("Quel est le nouveau tel ?");
                                    int tel = Convert.ToInt32(Console.ReadLine());
                                    string modifCuisinier = "update Cuisinier set téléphone = '" + tel + "' where id_Cuisinier = " + idCuisinierModif + ";";
                                    MySqlCommand modificationCuisinier = maConnexion.CreateCommand();
                                    modificationCuisinier.CommandText = modifCuisinier;
                                    MySqlDataReader reader = modificationCuisinier.ExecuteReader();
                                    reader.Close();
                                    modificationCuisinier.Dispose();
                                    Console.WriteLine("Modification d'un cuisinier");
                                }
                                else if (choixCuisinierModif == 4)
                                {
                                    Console.WriteLine("Quel est le nouveau adresse mail ?");
                                    string adresse_mail = Console.ReadLine();
                                    string modifCuisinier = "update Cuisinier set adresse_mail = '" + adresse_mail + "' where id_Cuisinier = " + idCuisinierModif + ";";
                                    MySqlCommand modificationCuisinier = maConnexion.CreateCommand();
                                    modificationCuisinier.CommandText = modifCuisinier;
                                    MySqlDataReader reader = modificationCuisinier.ExecuteReader();
                                    reader.Close();
                                    modificationCuisinier.Dispose();
                                    Console.WriteLine("Modification d'un cuisinier");
                                }
                                else if (choixCuisinierModif == 5)
                                {
                                    Console.WriteLine("Quel est le nouveau metro le plus proche ?");
                                    string metro_le_plus_proche = Console.ReadLine();
                                    string modifCuisinier = "update Cuisinier set metro_le_plus_proche = '" + metro_le_plus_proche + "' where id_Cuisinier = " + idCuisinierModif + ";";
                                    MySqlCommand modificationCuisinier = maConnexion.CreateCommand();
                                    modificationCuisinier.CommandText = modifCuisinier;
                                    MySqlDataReader reader = modificationCuisinier.ExecuteReader();
                                    reader.Close();
                                    modificationCuisinier.Dispose();
                                    Console.WriteLine("Modification d'un cuisinier");
                                }
                                else if (choixCuisinierModif == 6)
                                {
                                    Console.WriteLine("Quel est le nouvelle rue ?");
                                    string rue = Console.ReadLine();
                                    string modifCuisinier = "update Cuisinier set rue_ = '" + rue + "' where id_Cuisinier = " + idCuisinierModif + ";";
                                    MySqlCommand modificationCuisinier = maConnexion.CreateCommand();
                                    modificationCuisinier.CommandText = modifCuisinier;
                                    MySqlDataReader reader = modificationCuisinier.ExecuteReader();
                                    reader.Close();
                                    modificationCuisinier.Dispose();
                                    Console.WriteLine("Modification d'un cuisinier");
                                }
                                else if (choixCuisinierModif == 7)
                                {
                                    Console.WriteLine("Quel est le nouvelle ville ?");
                                    string ville = Console.ReadLine();
                                    string modifCuisinier = "update Cuisinier set ville = '" + ville + "' where id_Cuisinier = " + idCuisinierModif + ";";
                                    MySqlCommand modificationCuisinier = maConnexion.CreateCommand();
                                    modificationCuisinier.CommandText = modifCuisinier;
                                    MySqlDataReader reader = modificationCuisinier.ExecuteReader();
                                    reader.Close();
                                    modificationCuisinier.Dispose();
                                    Console.WriteLine("Modification d'un cuisinier");
                                }
                                else if (choixCuisinierModif == 8)
                                {
                                    Console.WriteLine("Quel est le nouveau numero de rue ?");
                                    int numéro_rue = Convert.ToInt32(Console.ReadLine());
                                    string modifCuisinier = "update Cuisinier set numéro_rue = '" + numéro_rue + "' where id_Cuisinier = " + idCuisinierModif + ";";
                                    MySqlCommand modificationCuisinier = maConnexion.CreateCommand();
                                    modificationCuisinier.CommandText = modifCuisinier;
                                    MySqlDataReader reader = modificationCuisinier.ExecuteReader();
                                    reader.Close();
                                    modificationCuisinier.Dispose();
                                    Console.WriteLine("Modification d'un cuisinier");
                                }
                                else if (choixCuisinierModif == 9)
                                {
                                    Console.WriteLine("Quel est le nouveau code postal ?");
                                    int code_postal = Convert.ToInt32(Console.ReadLine()); ;
                                    string modifCuisinier = "update Cuisinier set code_postal = '" + code_postal + "' where id_Cuisinier = " + idCuisinierModif + ";";
                                    MySqlCommand modificationCuisinier = maConnexion.CreateCommand();
                                    modificationCuisinier.CommandText = modifCuisinier;
                                    MySqlDataReader reader = modificationCuisinier.ExecuteReader();
                                    reader.Close();
                                    modificationCuisinier.Dispose();
                                    Console.WriteLine("Modification d'un cuisinier");
                                }
                                else
                                {
                                    Console.WriteLine("Option invalide.");
                                }

                                Console.WriteLine("1. prénom | 2. nom | 3. téléphone | 4. adresse_mail | 5. metro_le_plus_proche | 6. rue_ | 7. ville | 8. numéro_rue | 9. code_postal | 10.Quitter");
                                choixCuisinierModif = Convert.ToInt32(Console.ReadLine());
                            }
                            if (choixCuisinierModif == 10)
                            {
                                return;
                            }
                            

                            
                        }

                        else if (choixCuisinier == 3) //SUPPRIMER
                        {
                            Console.WriteLine("Quel est votre identifiant ?");
                            int idCuisinierModif = Convert.ToInt32(Console.ReadLine());
                           
                            string suppressionPlat = "delete from Plat where id_Cuisinier = " + idCuisinierModif + ";";
                            MySqlCommand suppPlat = maConnexion.CreateCommand();
                            suppPlat.CommandText = suppressionPlat;
                            MySqlDataReader reader = suppPlat.ExecuteReader();
                            reader.Close();
                            suppPlat.Dispose();
                            Console.WriteLine("Suppression d'un cuisinier");

                            string suppressionCuisinier = "delete from Cuisinier where id_Cuisinier = " + idCuisinierModif + ";";
                            MySqlCommand suppCuisinier = maConnexion.CreateCommand();
                            suppCuisinier.CommandText = suppressionCuisinier;
                            MySqlDataReader reader2 = suppCuisinier.ExecuteReader();
                            reader2.Close();
                            suppCuisinier.Dispose();
                            Console.WriteLine("Suppression d'un cuisinier");

                        }

                        else if (choixCuisinier == 4) //AFFICHER
                        {
                            //Console.WriteLine("1. Par ordre alphabétique | 2. Par rue | 3. Par montant des achats cumulés | 4. Quitter");
                            //int choixClientModif = Convert.ToInt32(Console.ReadLine());
                            //if (choixClientModif == 1) //ordre alphabétique
                            //{
                            //    string affichageClient = "select nom from particulier union select nom_référent as nom from entreprise order by nom asc;";
                            //    MySqlCommand affichClient = maConnexion.CreateCommand();
                            //    affichClient.CommandText = affichageClient;
                            //    MySqlDataReader reader = affichClient.ExecuteReader();

                            //    string[] valueString = new string[reader.FieldCount];

                            //    while (reader.Read())
                            //    {


                            //        for (int i = 0; i < reader.FieldCount; i++)
                            //        {
                            //            valueString[i] = reader.GetValue(i).ToString();
                            //            Console.Write(valueString[i] + " ");
                            //        }

                            //        Console.WriteLine();
                            //    }

                            //    reader.Close();
                            //    affichClient.Dispose();
                            //    Console.WriteLine("Affichage des clients");
                            //}
                            //else if (choixClientModif == 2) //par rue
                            //{
                            //    string affichageClient = "select * from Client order by rue asc;";
                            //    MySqlCommand affichClient = maConnexion.CreateCommand();
                            //    affichClient.CommandText = affichageClient;
                            //    MySqlDataReader reader = affichClient.ExecuteReader();

                            //    string[] valueString = new string[reader.FieldCount];

                            //    while (reader.Read())
                            //    {


                            //        for (int i = 0; i < reader.FieldCount; i++)
                            //        {
                            //            valueString[i] = reader.GetValue(i).ToString();
                            //            Console.Write(valueString[i] + " ");
                            //        }

                            //        Console.WriteLine();
                            //    }

                            //    reader.Close();
                            //    affichClient.Dispose();
                            //    Console.WriteLine("Affichage des clients");



                            //}
                            //else if (choixClientModif == 3) // par montant des achats cumulés
                            //{

                            //}

                        }
                    Console.WriteLine("1. Ajouter | 2. Modifier | 3. Supprimer | 4. Afficher | 5. Ajouter un plat | 6. Quitter");
                    choixCuisinier = Convert.ToInt32(Console.ReadLine());
                    }
                    if (choixCuisinier == 6)
                    {
                        return;
                    }
                }
                else if (choix == 3) //Commande
                {
                    Console.WriteLine("1. Ajouter | 2. Modifier |3.Afficher |4. Quitter");
                    int choixCommande = Convert.ToInt32(Console.ReadLine());
                    while (choixCommande != 4)
                    {

                        if (choixCommande == 1) //AJOUT
                        {
                            //Demander l'id du client pour vérifier qu'il est dans la base de donnée
                            Console.WriteLine("Quel est votre identifiant commande?");
                            int idcommande = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Quel est l'addition?");
                            float addition = float.Parse(Console.ReadLine());
                            Console.WriteLine("Quel est l'etat de la commande?");
                            string etat_de_la_commande = Console.ReadLine();
                            Console.WriteLine("Quel est l'heure actuellement?");
                            DateTime datetime = DateTime.Parse(Console.ReadLine());
                            Console.WriteLine("Quelle est votre id_client");
                            int id_client = Convert.ToInt32(Console.ReadLine());
                            string creationCommande = "insert into Cuisinier(id_commande,addition,etat_de_la_commande,date_,id_client) Values(" + idcommande + ", " + addition + ",'" + etat_de_la_commande + "','" + datetime + "'," + id_client+"); ";
                            MySqlCommand creaCommande = maConnexion.CreateCommand();
                            creaCommande.CommandText = creationCommande;
                            MySqlDataReader reader = creaCommande.ExecuteReader();
                            reader.Close();
                            creaCommande.Dispose();


                        }




                        Console.WriteLine("1. Ajouter | 2. Modifier |3.Afficher |4. Quitter");
                        choixCommande = Convert.ToInt32(Console.ReadLine());
                    }
                    if(choixCommande==4)
                    {
                        return;
                    }
                }
                else if (choix == 4)
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

                else
                {
                    Console.WriteLine("Option invalide.");
                }

                Console.WriteLine("Choisissez une action : 1. Gérer Clients | 2. Gérer Cuisiniers | 3. Gérer Commandes | 4. Statistiques | 5. Quitter");
                choix = Convert.ToInt32(Console.ReadLine());

                //maConnexion.Close();
                Console.ReadLine();
            }
            if (choix == 5)
            {
                return;
            }
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

