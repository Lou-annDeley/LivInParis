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
using DocumentFormat.OpenXml.Drawing.Diagrams;
using System.Data;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.ComponentModel.DataAnnotations;

namespace LivInParis___DELPIERRE_DROUIN_DELEY
{
    class Program
    {
        
        static void Main(string[] args)
        {
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
                    if (!row.Cell(6).IsEmpty())
                    {
                        int id = row.Cell(1).GetValue<int>();
                        string nom_station = row.Cell(2).GetValue<string>();
                        int poids_correspondance = row.Cell(6).GetValue<int>();
                        foreach (var noeud in noeuds)
                        {
                            if (noeud.Value.Valeur == nom_station)
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


// ----------------------- DEBUT DU SQL ---------------------------------------------------------------------------------------------------------------------------------------------

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


            //On doit vérifier :
            //
            // Requete de clients par date pour le cuisinier (doute)
            // Affichage Stat(COMANDES SEON NATIONALITE ET PERIODE) => Début de requête mais pas sure de si elle marche ou pas...


            //A faire
            //
            // Vérifier à chaque fois le les id n'existent pas déjà
            // Affichage Stat (Afficher la moyenne des comptes clients) => On sait pas ce que c'est...
            // 5 suggestions



            Console.WriteLine("Choisissez une action : 1. Gérer Clients | 2. Gérer Cuisiniers | 3. Gérer Commandes | 4. Statistiques |5. Avis | 6. Quitter");
            int choix = Convert.ToInt32(Console.ReadLine());

            while(choix != 6)
            {
                if (choix == 1) //CLIENT
                {
                    Console.WriteLine("1. Ajouter | 2. Modifier | 3. Supprimer | 4. Afficher | 5. Quitter");
                    int choixClient = Convert.ToInt32(Console.ReadLine());

                    while (choixClient != 5)
                    {
                        if (choixClient == 1) //AJOUTER
                        {
                            Console.WriteLine("Etes vous un 1.Particulier ou 2.Entreprise");
                            int statut = Convert.ToInt32(Console.ReadLine());
                            if (statut == 1)
                            {
                                Console.WriteLine("Quel est votre identifiant client?");
                                int idClient = Convert.ToInt32(Console.ReadLine());

                                #region
                                string affichageClient = "SELECT id_client FROM Client;";
                                MySqlCommand affichClient = maConnexion.CreateCommand();
                                affichClient.CommandText = affichageClient;
                                MySqlDataReader readerID = affichClient.ExecuteReader();

                                string[] valueString = new string[readerID.FieldCount];

                                while (readerID.Read())
                                {
                                    for (int i = 0; i < readerID.FieldCount; i++)
                                    {
                                        valueString[i] = readerID.GetValue(i).ToString();
                                        
                                        Console.Write(valueString[i] + " ");
                                    }
                                    Console.WriteLine();
                                }

                                readerID.Close();
                                affichClient.Dispose();
                                bool idValide = false;
                                do
                                {
                                    Console.WriteLine("Quel est votre identifiant client?");
                                    idClient = Convert.ToInt32(Console.ReadLine());
                                    for (int i = 0; i < valueString.Length; i++)
                                    {
                                        if (idClient == Convert.ToInt32(valueString[i]))
                                        {
                                            idValide = true;
                                            break;
                                        }
                                    }

                                } while (!idValide);
                                #endregion

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
                                string creationClient = "insert into Client(id_client, telephone, adresse_mail, ville, numero_de_rue, rue, code_postal, metro_le_plus_proche) Values(" + idClient + ", " + telClient + ",'" + mailClient + "','" + villeClient + "'," + numrueClient + ",'" + rueClient + "'," + codepClient + "," + metroClient + ");";
                                MySqlCommand creaClient = maConnexion.CreateCommand();
                                creaClient.CommandText = creationClient;
                                MySqlDataReader reader = creaClient.ExecuteReader();
                                reader.Close();
                                creaClient.Dispose();

                                Console.WriteLine("Quel est votre nom?");
                                string nomParti = Console.ReadLine();
                                Console.WriteLine("Quel est votre prénom?");
                                string prenomParti = Console.ReadLine();
                                string creationParticulier = "insert into particulier(id_particulier, nom, prenom, id_client) Values(" + idClient + ", '" + nomParti + "', '" + prenomParti + "'," + idClient + ");";
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
                                string creationClient = "insert into Client(id_client, telephone, adresse_mail, ville, numero_de_rue, rue, code_postal, metro_le_plus_proche) Values(" + idClient + ", " + telClient + ",'" + mailClient + "','" + villeClient + "'," + numrueClient + ",'" + rueClient + "'," + codepClient + "," + metroClient + ");";
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
                        } //AJOUTER

                        else if (choixClient == 2) //MODIFIER
                        {
                            Console.WriteLine("Quel est votre identifiant ?");
                            int idClientModif = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Etes vous un 1. Particulier ou 2. Entreprise ?");
                            int statut = Convert.ToInt32(Console.ReadLine());

                            if (statut == 1) //MODIF PARTICULIER
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
                                    } //TElEPHONE
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
                                    } //ADRESSE MAIL
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
                                    } //VILLE
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
                                    } //NUMERO RUE
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
                                    } //RUE
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
                                    } //CODE POSTAL
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
                                    } //METRO PROCHE
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
                                    } //NOM
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
                                    } //PRENOM
                                    else
                                    {
                                        Console.WriteLine("Option invalide.");
                                    } //OPTION INVALIDE

                                    Console.WriteLine("1. Télephone | 2. Adresse mail | 3. Ville | 4. Numéro de rue | 5. Rue | 6. Code Postal | 7. Metro le plus proche | 8. Nom | 9. Prénom | 10.Quitter");
                                    choixClientModif = Convert.ToInt32(Console.ReadLine());
                                }
                                if (choixClientModif == 10)
                                {
                                    return;
                                } //QUITTER
                            }//MODIF PARTICULIER

                            else if (statut == 2) //MODIF ENTREPRISE
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
                                    } //TELEPHONE
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
                                    } //ADRESSE MAIL
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
                                    } //VILLE
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
                                    } //NUMERO RUE
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
                                    } //RUE
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
                                    } //CODE POSTAL
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
                                    } //METRO PROCHE
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
                                    } //NOM REF
                                    else
                                    {
                                        Console.WriteLine("Option invalide.");
                                    } //OPTION INVALIDE

                                    Console.WriteLine("1. Télephone | 2. Adresse mail | 3. Ville | 4. Numéro de rue | 5. Rue | 6. Code Postal | 7. Metro le plus proche | 9. Nom référent | 10.Quitter");
                                    choixClientModif = Convert.ToInt32(Console.ReadLine());
                                }
                                if (choixClientModif == 9)
                                {
                                    return;
                                } //QUITTER
                            } //MODIF ENTREPRISE

                            else
                            {
                                Console.WriteLine("Option invalide.");
                            } //OPTION INVALIDE

                        } //MODIFIER

                        else if (choixClient == 3) //SUPPRIMER
                        {
                            Console.WriteLine("Quel est votre identifiant ?");
                            int idClientModif = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Etes vous un 1. Particulier ou 2. Entreprise ?");
                            int statut = Convert.ToInt32(Console.ReadLine());
                            if (statut == 1)
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
                        } //SUPPRIMER

                        else if (choixClient == 4) //AFFICHER
                        {
                            Console.WriteLine("1. Par ordre alphabétique | 2. Par rue | 3. Par montant des achats cumulés | 4. Quitter");
                            int choixClientModif = Convert.ToInt32(Console.ReadLine());
                            while (choixClientModif != 4)
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
                                } //ORDRE ALPHABETIQUE

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

                                } //PAR RUE

                                else if (choixClientModif == 3) // par montant des achats cumulés
                                {
                                    string affichageClient = "SELECT Client.id_client, SUM(Commande.addition) AS montant_total FROM Commande JOIN Client ON Commande.id_client = Client.id_client GROUP BY Client.id_client ORDER BY montant_total DESC;";
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
                                } //PAR MONTANT

                                else
                                {
                                    Console.WriteLine("Option Invalide.");
                                } //OPTION INVALIDE
                                Console.WriteLine("1. Par ordre alphabétique | 2. Par rue | 3. Par montant des achats cumulés | 4. Quitter");
                                choixClientModif = Convert.ToInt32(Console.ReadLine());
                            }
                            if (choixClientModif == 4)
                            {
                                return;
                            } //QUITTER
                        } //AFFICHER

                        else
                        {
                            Console.WriteLine("Option invalide.");

                        } //OPTION INVALIDE

                        Console.WriteLine("1. Ajouter | 2. Modifier | 3. Supprimer | 4. Afficher | 5. Quitter");
                        choixClient = Convert.ToInt32(Console.ReadLine());

                    }
                    if (choixClient == 5) //QUITTER
                    {
                        return;
                    } //QUITTER

                } //CLIENT

                else if (choix == 2) //CUISINIER
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
                            Console.WriteLine("Quel est votre id client? (attention il doit exister)");
                            int idclient = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Quelle est la date de votre inscription?");
                            string date_inscriptionCuisinier = Console.ReadLine();
                            string creationCuisinier = "insert into Cuisinier(id_Cuisinier,prénom,nom,téléphone,adresse_mail,metro_le_plus_proche,rue_,ville,numéro_rue,code_postal, id_client, date_inscription) Values(" + idcuisinier + ", '" + prenomcuisinier + "','" + nomcuisinier + "'," + telCuisinier + ",'" + mailCuisinier + "','" + metroCuisinier + "','" + rueCuisinier + "','" + villeCuisinier + "'," + numerorueCuisinier + "," + codepCuisinier + "," + idclient + ",'" + date_inscriptionCuisinier + "'); ";
                            MySqlCommand creaCuisinier = maConnexion.CreateCommand();
                            creaCuisinier.CommandText = creationCuisinier;
                            MySqlDataReader reader = creaCuisinier.ExecuteReader();
                            reader.Close();
                            creaCuisinier.Dispose();


                        } //AJOUTER

                        else if (choixCuisinier == 2) //MODIFIER
                        {
                            Console.WriteLine("Quel est votre id ?");
                            int idCuisinierModif = Convert.ToInt32(Console.ReadLine());

                            Console.WriteLine("Que voulez vous modifier ?");
                            Console.WriteLine("1. Prénom | 2. Nom | 3. Téléphone | 4. Adresse mail | 5. Métro le plus proche | 6. Rue | 7. Ville | 8. Numéro rue | 9. Code postal | 10. Quitter");
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
                                } //PRENOM
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
                                } //NOM
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
                                } //TELEPHONE
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
                                } //ADRESSE MAIL
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
                                } //METRO PROCHE
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
                                } //RUE
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
                                } //VILLE
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
                                } //NUMERO RUE
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
                                } //CODE POSTAL
                                else
                                {
                                    Console.WriteLine("Option invalide.");
                                } //OPTION INVALIDE

                                Console.WriteLine("1. prénom | 2. nom | 3. téléphone | 4. adresse_mail | 5. metro_le_plus_proche | 6. rue_ | 7. ville | 8. numéro_rue | 9. code_postal | 10.Quitter");
                                choixCuisinierModif = Convert.ToInt32(Console.ReadLine());
                            }
                            if (choixCuisinierModif == 10)
                            {
                                return;
                            } //QUITTER



                        } //MODIFIER

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
                        } //SUPPRIMER

                        else if (choixCuisinier == 4) //AFFICHER
                        {
                            Console.WriteLine("Quel est votre identifiant ?");
                            int idCuisinierAffiche = Convert.ToInt32(Console.ReadLine());

                            Console.WriteLine("1. Clients servis | 2. Plats par fréquence | 3. Plat du jour | 4. Quitter");
                            int choixCuisinierAfficher = Convert.ToInt32(Console.ReadLine());
                            while (choixCuisinierAfficher != 4)
                            {
                                if (choixCuisinierAfficher == 1) //clients servis
                                {
                                    Console.WriteLine("Depuis votre inscription (1) ou sur un certain interval de temps (2)?");
                                    int choixCuistoAff = Convert.ToInt32(Console.ReadLine());
                                    if (choixCuistoAff == 1) //DEPUIS INSCRIPTION
                                    {
                                        string affichageCuisinier1_1 = "SELECT Client.id_client FROM Client JOIN Commande ON Client.id_client = Commande.id_client JOIN Cuisinier ON Commande.id_client = Cuisinier.id_client WHERE Cuisinier.id_Cuisinier = " + idCuisinierAffiche + " AND Commande.date_ >= Cuisinier.date_inscription ORDER BY Commande.date_;";

                                        MySqlCommand affichCuisinier1_1 = maConnexion.CreateCommand();
                                        affichCuisinier1_1.CommandText = affichageCuisinier1_1;
                                        MySqlDataReader reader1_1 = affichCuisinier1_1.ExecuteReader();

                                        string[] valueString = new string[reader1_1.FieldCount];

                                        while (reader1_1.Read())
                                        {
                                            for (int i = 0; i < reader1_1.FieldCount; i++)
                                            {
                                                valueString[i] = reader1_1.GetValue(i).ToString();
                                                Console.Write(valueString[i] + " ");
                                            }
                                            Console.WriteLine();
                                        }
                                        reader1_1.Close();
                                        affichCuisinier1_1.Dispose();
                                        Console.WriteLine("Affichage des clients servis depuis inscription");
                                    } //DEPUIS INSCRIPTION
                                    else if (choixCuistoAff == 2) //SUR UNE PERIODE
                                    {
                                        Console.WriteLine("Quelle est la date de début?");
                                        string dateDebut = Console.ReadLine();
                                        Console.WriteLine("Quelle est la date de fin?");
                                        string dateFin = Console.ReadLine();
                                        string affichageCuisinier1_2 = "SELECT DISTINCT c.id_client, c.telephone, c.adresse_mail, c.ville" +
                                            "FROM Commande AS cmd" +
                                            "JOIN Client AS c ON cmd.id_client = c.id_client" +
                                            "JOIN ligne_de_commande AS ldc ON cmd.id_commande = ldc.id_commande" +
                                            "JOIN Plat AS p ON ldc.id_Plat = p.id_Plat" +
                                            "JOIN Cuisinier AS cuis ON p.id_Cuisinier = cuis.id_Cuisinier" +
                                            "WHERE cuis.id_Cuisinier = " + choixCuisinierAfficher +
                                            "AND cmd.date_ BETWEEN " + dateDebut + " AND " + dateFin + ");";

                                        MySqlCommand affichCuisinier1_2 = maConnexion.CreateCommand();
                                        affichCuisinier1_2.CommandText = affichageCuisinier1_2;
                                        MySqlDataReader reader1_2 = affichCuisinier1_2.ExecuteReader();

                                        string[] valueString = new string[reader1_2.FieldCount];

                                        while (reader1_2.Read())
                                        {
                                            for (int i = 0; i < reader1_2.FieldCount; i++)
                                            {
                                                valueString[i] = reader1_2.GetValue(i).ToString();
                                                Console.Write(valueString[i] + " ");
                                            }
                                            Console.WriteLine();
                                        }
                                        reader1_2.Close();
                                        affichCuisinier1_2.Dispose();
                                        Console.WriteLine("Affichage des clients servis sur une période de temps");
                                    } //SUR UNE PERIODE
                                    else
                                    {
                                        Console.WriteLine("Option invalide.");

                                    } //OPTION INVALIDE

                                } //CLIENTS SERVIS

                                else if (choixCuisinierAfficher == 2) //plats par fréquence
                                {
                                    string affichageCuisinier2 = "select nom from Plat where id_Cuisinier = " + idCuisinierAffiche + " group by nom;";
                                    MySqlCommand affichCuisinier2 = maConnexion.CreateCommand();
                                    affichCuisinier2.CommandText = affichageCuisinier2;
                                    MySqlDataReader reader2 = affichCuisinier2.ExecuteReader();

                                    string[] valueString = new string[reader2.FieldCount];

                                    while (reader2.Read())
                                    {
                                        for (int i = 0; i < reader2.FieldCount; i++)
                                        {
                                            valueString[i] = reader2.GetValue(i).ToString();
                                            Console.Write(valueString[i] + " ");
                                        }
                                        Console.WriteLine();
                                    }
                                    reader2.Close();
                                    affichCuisinier2.Dispose();
                                    Console.WriteLine("Affichage des plats du cuisinier");
                                } //PLATS FREQUENCE

                                else if (choixCuisinierAfficher == 3) // plat du jour 
                                {
                                    string affichageCuisinier3 = "select nom from Plat where id_Cuisinier = " + idCuisinierAffiche + " and plat_du_jour = true;";
                                    MySqlCommand affichCuisinier3 = maConnexion.CreateCommand();
                                    affichCuisinier3.CommandText = affichageCuisinier3;
                                    MySqlDataReader reader3 = affichCuisinier3.ExecuteReader();

                                    string[] valueString = new string[reader3.FieldCount];

                                    while (reader3.Read())
                                    {
                                        for (int i = 0; i < reader3.FieldCount; i++)
                                        {
                                            valueString[i] = reader3.GetValue(i).ToString();
                                            Console.Write(valueString[i] + " ");
                                        }
                                        Console.WriteLine();
                                    }
                                    reader3.Close();
                                    affichCuisinier3.Dispose();
                                    Console.WriteLine("Affichage du plat du jour");
                                } //PLAT DU JOUR

                                else
                                {
                                    Console.WriteLine("Option invalide.");
                                } //OPTION INVALIDE

                                Console.WriteLine("1. Clients servis | 2. Plats par fréquence | 3. Plat du jour | 4. Quitter");
                                choixCuisinierAfficher = Convert.ToInt32(Console.ReadLine());
                            }
                            if (choixCuisinierAfficher == 4)
                            {
                                return;
                            } //QUITTER

                        } //AFFICHER

                        else if (choixCuisinier == 5) //AJOUT PLAT
                        {
                            Console.WriteLine("Quel est votre identifiant ?");
                            int idCuisinier = Convert.ToInt32(Console.ReadLine());

                            Console.WriteLine("Combien de plats proposez-vous ?");
                            int nbPlat = Convert.ToInt32(Console.ReadLine());
                            for (int i = 0; i < nbPlat; i++)
                            {
                                Console.WriteLine("Quel est l'identifiant du plat numéro " + i + "?");
                                int idPlat = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("Quel est le nom du plat numéro " + i + "?");
                                string nomPlat = Console.ReadLine();
                                Console.WriteLine("Quel est le prix du plat numéro" + i + "?");
                                float prixPlat = float.Parse(Console.ReadLine());
                                Console.WriteLine("Quelle est la recette?");
                                string recettePlat = Console.ReadLine();
                                Console.WriteLine("Pour combien de personnes?");
                                int nbpersonnesPlat = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("Quelle est la date de péremption?");
                                string dateperemptionPlat = Console.ReadLine();
                                Console.WriteLine("Quelle est la date de fabrication?");
                                string datefabricationPlat = Console.ReadLine();
                                Console.WriteLine("Quelle est la nationalité?");
                                string nationalitePlat = Console.ReadLine();
                                Console.WriteLine("Quel est le régime alimentaire?");
                                string regimePlat = Console.ReadLine();
                                Console.WriteLine("Est-ce le plat du jour ?");
                                bool plat_du_jour = Convert.ToBoolean(Console.ReadLine());

                                string creationPlat = "insert into Plat(id_Plat,nom,prix,recette,nb_personnes,date_de_péremption,date_de_fabrication,nationalité,régime_alimentaire,id_Cuisinier,plat_du_jour) Values(" + idPlat + ", '" + nomPlat + "'," + prixPlat + ",'" + recettePlat + "'," + nbpersonnesPlat + ",'" + dateperemptionPlat + "','" + datefabricationPlat + "','" + nationalitePlat + "','" + regimePlat + "'," + idCuisinier + "," + plat_du_jour + "); ";
                                MySqlCommand creaPlat = maConnexion.CreateCommand();
                                creaPlat.CommandText = creationPlat;
                                MySqlDataReader reader3 = creaPlat.ExecuteReader();
                                reader3.Close();
                                creaPlat.Dispose();

                                Console.WriteLine("Combien d'ingrédients utilisez-vous?");
                                int nbIngredients = Convert.ToInt32(Console.ReadLine());
                                for (int j = 0; j < nbIngredients; j++)
                                {
                                    Console.WriteLine("Quel est l'identifiant de l'ingrédient?");
                                    int idIngrédient = Convert.ToInt32(Console.ReadLine());
                                    Console.WriteLine("Quel est le nom de l'ingrédient?");
                                    string nomIngredient = Console.ReadLine();
                                    Console.WriteLine("Quelle est la quantité de l'ingrédient que vous possédez ?");
                                    string quantiteIngredient = Console.ReadLine();
                                    Console.WriteLine("Quelle est la date de péremption de l'ingrédient?");
                                    string peremptionIngredient = Console.ReadLine();
                                    string creationIngredient = "insert into Ingrédients(id_Ingrédient,nom,quantité,date_de_péremption) Values(" + idIngrédient + ", '" + nomIngredient + "','" + quantiteIngredient + "','" + peremptionIngredient + "'); ";
                                    MySqlCommand creaIngredient = maConnexion.CreateCommand();
                                    creaIngredient.CommandText = creationIngredient;
                                    MySqlDataReader reader4 = creaIngredient.ExecuteReader();
                                    reader4.Close();
                                    creaIngredient.Dispose();

                                    Console.WriteLine("Quelle est la quantité de l'ingrédient que vous utilisez ?");
                                    string quantiteIngredientutilisee = Console.ReadLine();
                                    string creationComposee = "insert into composer(id_Plat,id_Ingrédient,quantité) Values(" + idPlat + ", " + idIngrédient + ",'" + quantiteIngredientutilisee + "'); ";
                                    MySqlCommand creaComposee = maConnexion.CreateCommand();
                                    creaComposee.CommandText = creationComposee;
                                    MySqlDataReader reader5 = creaComposee.ExecuteReader();
                                    reader5.Close();
                                    creaComposee.Dispose();
                                }
                            }
                        } //AJOUTER UN PLAT

                        else
                        {
                            Console.WriteLine("Option invalide.");

                        } //OPTION INVALIDE

                        Console.WriteLine("1. Ajouter | 2. Modifier | 3. Supprimer | 4. Afficher | 5. Ajouter un plat | 6. Quitter");
                        choixCuisinier = Convert.ToInt32(Console.ReadLine());
                    }
                    if (choixCuisinier == 6) //QUITTER
                    {
                        return;
                    } //QUITTER
                } //CUISINIER

                else if (choix == 3) //COMMANDE
                {
                    Console.WriteLine("1. Ajouter | 2. Modifier |3.Afficher |4. Quitter");
                    int choixCommande = Convert.ToInt32(Console.ReadLine());

                    while (choixCommande != 4)
                    {
                        if (choixCommande == 1) //AJOUTER
                        {
                            //Demander l'id du client pour vérifier qu'il est dans la base de donnée
                            Console.WriteLine("Quel est votre identifiant commande?");
                            int idcommande = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Quel est l'addition?");
                            float addition = float.Parse(Console.ReadLine());
                            Console.WriteLine("Quel est l'etat de la commande?");
                            string etat_de_la_commande = Console.ReadLine();
                            Console.WriteLine("Quel est l'heure actuellement?");
                            string datetime = Console.ReadLine();
                            Console.WriteLine("Quelle est votre id_client");
                            int id_client = Convert.ToInt32(Console.ReadLine());
                            string creationCommande = "insert into Commande(id_commande,addition,etat_de_la_commande,date_,id_client) Values(" + idcommande + ", " + addition + ",'" + etat_de_la_commande + "','" + datetime + "'," + id_client + ");";
                            MySqlCommand creaCommande = maConnexion.CreateCommand();
                            creaCommande.CommandText = creationCommande;
                            MySqlDataReader reader = creaCommande.ExecuteReader();
                            reader.Close();
                            creaCommande.Dispose();
                        } //AJOUTER

                        else if (choixCommande == 2) //MODIFIER
                        {
                            Console.WriteLine("Quel est l'identifiant de la commande ?");
                            int idCommande = Convert.ToInt32(Console.ReadLine());

                            Console.WriteLine("Que voulez vous modifier ?");
                            Console.WriteLine("1. Addition | 2. Etat de la commande |3. Date |4. Quitter");
                            int choixModifCommande = Convert.ToInt32(Console.ReadLine());

                            while (choixModifCommande != 4)
                            {
                                if (choixModifCommande == 1) //ADDITION
                                {
                                    Console.WriteLine("Quelle est le nouvelle addition ?");
                                    float addition = float.Parse(Console.ReadLine());
                                    string modifCommande = "update Commande set addition = " + addition + " where id_commande = " + idCommande + ";";
                                    MySqlCommand modificationCommande1 = maConnexion.CreateCommand();
                                    modificationCommande1.CommandText = modifCommande;
                                    MySqlDataReader reader1 = modificationCommande1.ExecuteReader();
                                    reader1.Close();
                                    modificationCommande1.Dispose();
                                    Console.WriteLine("Modification d'une Commande");
                                } //ADDITION

                                else if (choixModifCommande == 2) //ETAT COMMANDE
                                {
                                    Console.WriteLine("Quel est le nouvel Etat de commande ?");
                                    string etatCommande = Console.ReadLine();
                                    string modifCommande2 = "update Commande set etat_de_la_commande = '" + etatCommande + "' where id_commande = " + idCommande + ";";
                                    MySqlCommand modificationCommande2 = maConnexion.CreateCommand();
                                    modificationCommande2.CommandText = modifCommande2;
                                    MySqlDataReader reader2 = modificationCommande2.ExecuteReader();
                                    reader2.Close();
                                    modificationCommande2.Dispose();
                                    Console.WriteLine("Modification d'une Commande");
                                } //ETAT COMMANDE

                                else if (choixModifCommande == 3) //DATE
                                {
                                    Console.WriteLine("Quel est la nouvelle date ?");
                                    string date = Console.ReadLine();
                                    string modifCommande3 = "update Commande set date_ = '" + date + "' where id_commande = " + idCommande + ";";
                                    MySqlCommand modificationCommande3 = maConnexion.CreateCommand();
                                    modificationCommande3.CommandText = modifCommande3;
                                    MySqlDataReader reader3 = modificationCommande3.ExecuteReader();
                                    reader3.Close();
                                    modificationCommande3.Dispose();
                                    Console.WriteLine("Modification d'une Commande");
                                } //DATE

                                else //OPTION INVALIDE
                                {
                                    Console.WriteLine("Option invalide.");
                                } //OPTION INVALIDE
                                Console.WriteLine("Que voulez vous modifier ?");
                                Console.WriteLine("1. Addition | 2. Etat de la commande |3. Date |4. Quitter");
                                choixModifCommande = Convert.ToInt32(Console.ReadLine());
                            }
                            if (choixModifCommande == 4) //QUITTER
                            {
                                return;
                            } //QUITTER

                        } //MODIFIER

                        else if (choixCommande == 3) //AFFICHER
                        {
                            Console.WriteLine("Quel est votre identifiant ?");
                            int idCommandeAffiche = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Que voulez-vous afficher ?");
                            Console.WriteLine("1. Etat de la commande | 2. Prix moyenné au numéro | 3. Chemin de livraison | 4. Ligne de commande | 5. Quitter");
                            int choixCommandeAfficher = Convert.ToInt32(Console.ReadLine());

                            while (choixCommandeAfficher != 5)
                            {
                                if (choixCommandeAfficher == 1) //ETAT COMMANDE
                                {
                                    string affichageCommande1 = "SELECT etat_de_la_commande FROM Commande WHERE id_commande = " + idCommandeAffiche + ";";

                                    MySqlCommand affichCommande1 = maConnexion.CreateCommand();
                                    affichCommande1.CommandText = affichageCommande1;
                                    MySqlDataReader reader1 = affichCommande1.ExecuteReader();

                                    string[] valueString = new string[reader1.FieldCount];

                                    while (reader1.Read())
                                    {
                                        for (int i = 0; i < reader1.FieldCount; i++)
                                        {
                                            valueString[i] = reader1.GetValue(i).ToString();
                                            Console.Write(valueString[i] + " ");
                                        }
                                        Console.WriteLine();
                                    }
                                    reader1.Close();
                                    affichCommande1.Dispose();
                                    Console.WriteLine("Affichage des clients servis depuis inscription");
                                } //ETAT COMMANDE

                                else if (choixCommandeAfficher == 2) //PRIX MOYENNE
                                {
                                    string affichageCommande2 = "SELECT id_commande, addition FROM Commande;";

                                    MySqlCommand affichCommande2 = maConnexion.CreateCommand();
                                    affichCommande2.CommandText = affichageCommande2;
                                    MySqlDataReader reader2 = affichCommande2.ExecuteReader();

                                    string[] valueString = new string[reader2.FieldCount];

                                    while (reader2.Read())
                                    {
                                        for (int i = 0; i < reader2.FieldCount; i++)
                                        {
                                            valueString[i] = reader2.GetValue(i).ToString();
                                            Console.Write(valueString[i] + " ");
                                        }
                                        Console.WriteLine();
                                    }
                                    reader2.Close();
                                    affichCommande2.Dispose();
                                    Console.WriteLine("Affichage des clients servis depuis inscription");
                                } //PRIX MOYENNE

                                else if (choixCommandeAfficher == 3) //COURT CHEMIN
                                {
                                    string affichageCommande3_1 = "SELECT Client.metro_le_plus_proche FROM Commande " +
                                        "JOIN Client ON Client.id_client = Commande.id_client " +
                                        "WHERE Commande.id_commande = " + idCommandeAffiche + ";";

                                    MySqlCommand affichCommande3_1 = maConnexion.CreateCommand();
                                    affichCommande3_1.CommandText = affichageCommande3_1;
                                    MySqlDataReader reader3_1 = affichCommande3_1.ExecuteReader();

                                    string[] valueString3_1 = new string[reader3_1.FieldCount];

                                    while (reader3_1.Read())
                                    {
                                        for (int i = 0; i < reader3_1.FieldCount; i++)
                                        {
                                            valueString3_1[i] = reader3_1.GetValue(i).ToString();
                                            Console.Write(valueString3_1[i] + " ");
                                        }
                                        Console.WriteLine();
                                    }
                                    reader3_1.Close();
                                    affichCommande3_1.Dispose();
                                    Console.WriteLine("Affichage des metro proche du client");


                                    string affichageCommande3_2 = "SELECT Cuisinier.metro_le_plus_proche FROM Commande " +
                                        "JOIN ligne_de_commande ON Commande.id_commande = ligne_de_commande.id_commande" +
                                        " JOIN Plat ON ligne_de_commande.id_Plat = Plat.id_Plat " +
                                        "JOIN Cuisinier ON Plat.id_Cuisinier = Cuisinier.id_Cuisinier " +
                                        "WHERE Commande.id_commande = " + idCommandeAffiche + ";";

                                    MySqlCommand affichCommande3_2 = maConnexion.CreateCommand();
                                    affichCommande3_2.CommandText = affichageCommande3_2;
                                    MySqlDataReader reader3_2 = affichCommande3_2.ExecuteReader();

                                    string[] valueString3_2 = new string[reader3_2.FieldCount];

                                    while (reader3_2.Read())
                                    {
                                        for (int i = 0; i < reader3_2.FieldCount; i++)
                                        {
                                            valueString3_2[i] = reader3_2.GetValue(i).ToString();
                                            Console.Write(valueString3_2[i] + " ");
                                        }
                                        Console.WriteLine();
                                    }
                                    reader3_2.Close();
                                    affichCommande3_2.Dispose();
                                    Console.WriteLine("Affichage des metro proche du cuisinier");

                                    string station1 = valueString3_1[0];
                                    string station2 = valueString3_2[0];
                                    int idStation1 = -1;
                                    int idStation2 = -1;

                                    foreach (var station in noeuds)
                                    {
                                        if (station.Value.Valeur == station1)
                                        {
                                            idStation1 = station.Key;
                                        }
                                    }
                                    foreach (var station in noeuds)
                                    {
                                        if (station.Value.Valeur == station2)
                                        {
                                            idStation2 = station.Key;
                                        }
                                    }

                                    List<Noeud<string>> plus_petit_chemin = metro.Dijkstra(noeuds[idStation1], noeuds[idStation2]);

                                    Console.WriteLine("Nouveau djikstra, le chemin est ");
                                    foreach (Noeud<string> noeud1 in plus_petit_chemin)
                                    {
                                        Console.Write(noeud1.Valeur + " ");
                                    }
                                    Console.WriteLine();

                                    metro.DessinerGraphe("Graphe_des_stations.png");
                                    Console.WriteLine("Graphe généré");
                                    metro.MettreEnEvidenceChemin(plus_petit_chemin, "Plus_court_chemin.png");
                                    Console.WriteLine("Graphe avec le plus court chemin généré");

                                } //COURT CHEMIN

                                else if (choixCommandeAfficher==4) // Ligne de commande
                                {
                                    string affichageCommande4 = "SELECT Plat.nom FROM ligne_de_commande " +
                                        "JOIN Plat ON ligne_de_commande.id_Plat = Plat.id_Plat " +
                                        "JOIN Commande ON ligne_de_commande.id_commande = Commande.id_commande " +
                                        "WHERE ligne_de_commande.id_commande = " + idCommandeAffiche + ";";

                                    MySqlCommand affichCommande4 = maConnexion.CreateCommand();
                                    affichCommande4.CommandText = affichageCommande4;
                                    MySqlDataReader reader4 = affichCommande4.ExecuteReader();

                                    string[] valueString = new string[reader4.FieldCount];

                                    while (reader4.Read())
                                    {
                                        for (int i = 0; i < reader4.FieldCount; i++)
                                        {
                                            valueString[i] = reader4.GetValue(i).ToString();
                                            Console.Write(valueString[i] + " ");
                                        }
                                        Console.WriteLine();
                                    }
                                    reader4.Close();
                                    affichCommande4.Dispose();
                                    Console.WriteLine("Affichage des clients servis depuis inscription");

                                }

                                else //OPTION INVALIDE
                                {
                                    Console.WriteLine("Option invalide.");
                                } //OPTION INVALIDE

                                Console.WriteLine("Que voulez-vous afficher ?");
                                Console.WriteLine("1. Etat de la commande | 2. Prix moyenné au numéro | 3. Chemin de livraison | 4. Ligne de commande | 5. Quitter");
                                choixCommandeAfficher = Convert.ToInt32(Console.ReadLine());
                            }
                            if (choixCommandeAfficher == 5) //QUITTER
                            {
                                return;
                            } //QUITTER
                        } //AFFICHER

                        else
                        {
                            Console.WriteLine("Option invalide.");

                        } //OPTION INVALIDE

                        Console.WriteLine("1. Ajouter | 2. Modifier |3.Afficher |4. Quitter");
                        choixCommande = Convert.ToInt32(Console.ReadLine());
                    }
                    if (choixCommande == 4) //QUITTER
                    {
                        return;
                    } //QUITTER
                } //COMMANDES

                else if (choix == 4) //STATISTIQUES
                {
                    Console.WriteLine("1. Nombre de livraison par cuisiniers | 2. Commandes selon un période | 3. Moyenne des prix des commandes | 4. Moyenne des comptes clients | 5. Commandes selon nationnalité et période | 6. Quitter");
                    int choixStatistiques = Convert.ToInt32(Console.ReadLine());

                    while (choixStatistiques != 4)
                    {
                        if (choixStatistiques == 1) //NB LIVRAISON PAR CUISTO
                        {
                            Console.WriteLine("Quel est votre identifiant cuisinier?");
                            int idCuistoStatistiques = Convert.ToInt32(Console.ReadLine());

                            string affichageStat1 = "SELECT COUNT(Commande.id_commande) AS nombre_de_livraisons FROM Commande JOIN Cuisinier ON Commande.id_client = Cuisinier.id_client WHERE Cuisinier.id_Cuisinier = " + idCuistoStatistiques + " AND Commande.etat_de_la_commande = 'livree'";

                            MySqlCommand affichStat1 = maConnexion.CreateCommand();
                            affichStat1.CommandText = affichageStat1;
                            MySqlDataReader reader1 = affichStat1.ExecuteReader();

                            string[] valueString = new string[reader1.FieldCount];

                            while (reader1.Read())
                            {
                                for (int i = 0; i < reader1.FieldCount; i++)
                                {
                                    valueString[i] = reader1.GetValue(i).ToString();
                                    Console.Write(valueString[i] + " ");
                                }
                                Console.WriteLine();
                            }
                            reader1.Close();
                            affichStat1.Dispose();
                            Console.WriteLine("Affichage du nombre de livraison par cuisto");

                        } //NB LIVRAISON PAR CUISTO

                        else if (choixStatistiques == 2) //COMMANDES SELON PERIODE
                        {
                            Console.WriteLine("Quelle est la date de début?");
                            string dateDebut = Console.ReadLine();
                            Console.WriteLine("Quelle est la date de fin?");
                            string dateFin = Console.ReadLine();

                            string affichageStat2 = "SELECT * FROM Commande WHERE date_ BETWEEN '" + dateDebut + "' AND '" + dateFin + "';";
                            MySqlCommand affichStat2 = maConnexion.CreateCommand();
                            affichStat2.CommandText = affichageStat2;
                            MySqlDataReader reader2 = affichStat2.ExecuteReader();

                            string[] valueString = new string[reader2.FieldCount];

                            while (reader2.Read())
                            {
                                for (int i = 0; i < reader2.FieldCount; i++)
                                {
                                    valueString[i] = reader2.GetValue(i).ToString();
                                    Console.Write(valueString[i] + " ");
                                }
                                Console.WriteLine();
                            }
                            reader2.Close();
                            affichStat2.Dispose();
                            Console.WriteLine("Affichage des commandes selon une période");
                        } //COMMANDES SELON PERIODE

                        else if (choixStatistiques == 3) //MOYENNE PRIX COMMANDES
                        {
                            string affichageStat3 = "SELECT AVG(Commande.addition) AS moyenne_prix_commandes FROM Commande; ";
                            MySqlCommand affichStat3 = maConnexion.CreateCommand();
                            affichStat3.CommandText = affichageStat3;
                            MySqlDataReader reader3 = affichStat3.ExecuteReader();

                            string[] valueString = new string[reader3.FieldCount];

                            while (reader3.Read())
                            {
                                for (int i = 0; i < reader3.FieldCount; i++)
                                {
                                    valueString[i] = reader3.GetValue(i).ToString();
                                    Console.Write(valueString[i] + " ");
                                }
                                Console.WriteLine();
                            }
                            reader3.Close();
                            affichStat3.Dispose();
                            Console.WriteLine("Affichage de la moyenne des prix des commandes");
                        } //MOYENNE PRIX COMMANDES

                        else if (choixStatistiques == 4) //MOYENNE COMPTE CLIENT
                        {

                        } //MOYENNE COMPTE CLIENT

                        else if (choixStatistiques == 5) //COMANDES SEON NATIONALITE ET PERIODE
                        {
                            Console.WriteLine("Quelle est la date de début?");
                            string dateDebut = Console.ReadLine();
                            Console.WriteLine("Quelle est la date de fin?");
                            string dateFin = Console.ReadLine();
                            Console.WriteLine("Quel est l'id du client ?");
                            int idClient = Convert.ToInt32(Console.ReadLine());

                            string affichageStat5 = "SELECT Commande.id_commande, Commande.date_ FROM Commande " +
                            "JOIN Client ON Commande.id_client = Client.id_client " +
                            "JOIN Plat ON Commande.id_commande = Plat.id_Plat " +
                            "WHERE Client.id_client = " + idClient +
                            " AND Commande.date_ BETWEEN " + dateDebut + " AND " + dateFin +
                            " ORDER BY Plat.nationalité;";
                            MySqlCommand affichStat5 = maConnexion.CreateCommand();
                            affichStat5.CommandText = affichageStat5;
                            MySqlDataReader reader5 = affichStat5.ExecuteReader();

                            string[] valueString = new string[reader5.FieldCount];

                            while (reader5.Read())
                            {
                                for (int i = 0; i < reader5.FieldCount; i++)
                                {
                                    valueString[i] = reader5.GetValue(i).ToString();
                                    Console.Write(valueString[i] + " ");
                                }
                                Console.WriteLine();
                            }
                            reader5.Close();
                            affichStat5.Dispose();
                            Console.WriteLine("Affichage des commandes selon nationalité et une période");
                        } //COMANDES SEON NATIONALITE ET PERIODE
                        Console.WriteLine("1. Nombre de livraison par cuisiniers | 2. Commandes selon un période | 3. Moyenne des prix des commandes | 4. Moyenne des comptes clients | 5. Commandes selon nationnalité et période | 6. Quitter");
                        choixStatistiques = Convert.ToInt32(Console.ReadLine());
                    }
                    if (choixStatistiques == 4) //QUITTER
                    {
                        return;
                    } //QUITTER


                } //STATISTIQUES

                else if (choix == 5)
                {
                    Console.WriteLine("1. Ajouter | 2. Supprimer |3. Afficher |4. Quitter");
                    int choixAvis = Convert.ToInt32(Console.ReadLine());
                    while (choixAvis != 4)
                    {
                        if (choixAvis == 1) //AJOUTER
                        {
                            Console.WriteLine("Quel est le numero de l'avis");
                            int idavis = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Quelle est votre note sur 5");
                            int note = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Entrez votre commentaire");
                            string commentaire = Console.ReadLine();
                            Console.WriteLine("Quel est le numero du client");
                            int idclient = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Quel est le numero du cuisinier");
                            int idcuisinier = Convert.ToInt32(Console.ReadLine());
                            string creationavis = "insert into Avis(id_avis,note,commentaire,id_client,id_Cuisinier) Values(" + idavis + ", " + note + ",'" + commentaire + "'," + idclient + "," + idcuisinier +"); ";
                            MySqlCommand creaavis = maConnexion.CreateCommand();
                            creaavis.CommandText = creationavis;
                            MySqlDataReader reader = creaavis.ExecuteReader();
                            reader.Close();
                            creaavis.Dispose();



                        }
                        else if (choixAvis == 2)
                        {
                            Console.WriteLine("Quel est le numéro de votre avis ?");
                            int idavissupp = Convert.ToInt32(Console.ReadLine());

                            string suppressionavis = "delete from Avis where id_avis = " + idavissupp + ";";
                            MySqlCommand suppavis = maConnexion.CreateCommand();
                            suppavis.CommandText = suppressionavis;
                            MySqlDataReader reader = suppavis.ExecuteReader();
                            reader.Close();
                            suppavis.Dispose();
                            Console.WriteLine("Suppression d'un cuisinier");

                        }
                        else if (choixAvis == 3)
                        {
                            Console.WriteLine("1. Afficher les notes | 2. Afficher les commentaires |3. moyenne des notes pour un cuisinier |4. Liste des meilleurs cuisinier |5. Quitter");
                            int choixaffiche = Convert.ToInt32(Console.ReadLine());
                            while (choixaffiche != 5)
                            {
                                if (choixaffiche == 1)
                                {
                                    Console.WriteLine("Quel est le numéro du cuisinier");
                                    int idcuisinier = Convert.ToInt32(Console.ReadLine());
                                    string affichageNote = "SELECT note FROM avis WHERE id_Cuisinier = "+idcuisinier+";";
                                    MySqlCommand affichNote = maConnexion.CreateCommand();
                                    affichNote.CommandText = affichageNote;
                                    MySqlDataReader reader2 = affichNote.ExecuteReader();

                                    string[] valueString = new string[reader2.FieldCount];

                                    while (reader2.Read())
                                    {
                                        for (int i = 0; i < reader2.FieldCount; i++)
                                        {
                                            valueString[i] = reader2.GetValue(i).ToString();
                                            Console.Write(valueString[i] + " ");
                                        }
                                        Console.WriteLine();
                                    }
                                    reader2.Close();
                                    affichNote.Dispose();
                                    Console.WriteLine("Affichage des notes");

                                }
                                else if (choixaffiche == 2)
                                {
                                    Console.WriteLine("Quel est le numéro du cuisinier");
                                    int idcuisinier = Convert.ToInt32(Console.ReadLine());
                                    string affichageNote = "SELECT commentaire FROM avis WHERE id_Cuisinier = " + idcuisinier + ";";
                                    MySqlCommand affichNote = maConnexion.CreateCommand();
                                    affichNote.CommandText = affichageNote;
                                    MySqlDataReader reader2 = affichNote.ExecuteReader();

                                    string[] valueString = new string[reader2.FieldCount];

                                    while (reader2.Read())
                                    {
                                        for (int i = 0; i < reader2.FieldCount; i++)
                                        {
                                            valueString[i] = reader2.GetValue(i).ToString();
                                            Console.Write(valueString[i] + " ");
                                        }
                                        Console.WriteLine();
                                    }
                                    reader2.Close();
                                    affichNote.Dispose();
                                    Console.WriteLine("Affichage des commentaires");

                                }
                                else if (choixaffiche == 3)
                                {
                                    Console.WriteLine("Quel est le numéro du cuisinier");
                                    int idcuisinier = Convert.ToInt32(Console.ReadLine());
                                    string affichageNote = "SELECT AVG(Avis.note) AS moyenne FROM Avis; ";
                                    MySqlCommand affichNote = maConnexion.CreateCommand();
                                    affichNote.CommandText = affichageNote;
                                    MySqlDataReader reader2 = affichNote.ExecuteReader();

                                    string[] valueString = new string[reader2.FieldCount];

                                    while (reader2.Read())
                                    {
                                        for (int i = 0; i < reader2.FieldCount; i++)
                                        {
                                            valueString[i] = reader2.GetValue(i).ToString();
                                            Console.Write(valueString[i] + " ");
                                        }
                                        Console.WriteLine();
                                    }
                                    reader2.Close();
                                    affichNote.Dispose();
                                    Console.WriteLine("Affichage de la moyenne des notes d'un cuisinier");

                                }
                                else if (choixaffiche == 4)
                                {
                                    Console.WriteLine("Quel est le numéro du cuisinier");
                                    int idcuisinier = Convert.ToInt32(Console.ReadLine());
                                    string affichageNote = "SELECT Cuisinier.id_Cuisinier, Cuisinier.prénom, Cuisinier.nom, AVG(Avis.note) AS moyenne_notes FROM Avis JOIN Cuisinier ON Avis.id_Cuisinier = Cuisinier.id_Cuisinier GROUP BY Cuisinier.id_Cuisinier, Cuisinier.prénom, Cuisinier.nom ORDER BY moyenne_notes DESC; ";
                                    MySqlCommand affichNote = maConnexion.CreateCommand();
                                    affichNote.CommandText = affichageNote;
                                    MySqlDataReader reader2 = affichNote.ExecuteReader();

                                    string[] valueString = new string[reader2.FieldCount];

                                    while (reader2.Read())
                                    {
                                        for (int i = 0; i < reader2.FieldCount; i++)
                                        {
                                            valueString[i] = reader2.GetValue(i).ToString();
                                            Console.Write(valueString[i] + " ");
                                        }
                                        Console.WriteLine();
                                    }
                                    reader2.Close();
                                    affichNote.Dispose();
                                    Console.WriteLine("Affichage de la liste des meilleurs cuisiniers");

                                }
                                else
                                {
                                    Console.WriteLine("Option invalide");
                                }

                                Console.WriteLine("1. Afficher les notes | 2. Afficher les commentaires |3. moyenne des notes pour un cuisinier |4. Liste des meilleurs cuisinier |5. Quitter");
                                choixaffiche = Convert.ToInt32(Console.ReadLine());
                            }
                            if(choixaffiche == 5)
                            {
                                return;
                            }

                        }
                        Console.WriteLine("1. Ajouter | 2. Supprimer |3. Quitter");
                        choixAvis = Convert.ToInt32(Console.ReadLine());

                    }
                    if(choixAvis==4)
                    {
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Option invalide.");
                } //OPTION INVALIDE


                Console.WriteLine("Choisissez une action : 1. Gérer Clients | 2. Gérer Cuisiniers | 3. Gérer Commandes | 4. Statistiques |5.Avis | 6. Quitter");
                choix = Convert.ToInt32(Console.ReadLine());

                //maConnexion.Close();
                Console.ReadLine();
            }


            if (choix == 6) //QUITTER
            {
                return;
            } //QUITTER
            Console.ReadLine();

            maConnexion.Close();
            maConnexion.Dispose();
        }
    }
}

