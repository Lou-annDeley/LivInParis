CREATE DATABASE  IF NOT EXISTS `livraison` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `livraison`;
-- MySQL dump 10.13  Distrib 8.0.41, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: livraison
-- ------------------------------------------------------
-- Server version	8.0.41

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `avis`
--

DROP TABLE IF EXISTS `avis`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `avis` (
  `id_avis` bigint NOT NULL,
  `note` int DEFAULT NULL,
  `commentaire` varchar(50) DEFAULT NULL,
  `id_client` bigint NOT NULL,
  `id_Cuisinier` bigint NOT NULL,
  PRIMARY KEY (`id_avis`),
  KEY `id_client` (`id_client`),
  KEY `id_Cuisinier` (`id_Cuisinier`),
  CONSTRAINT `avis_ibfk_1` FOREIGN KEY (`id_client`) REFERENCES `client` (`id_client`),
  CONSTRAINT `avis_ibfk_2` FOREIGN KEY (`id_Cuisinier`) REFERENCES `cuisinier` (`id_Cuisinier`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `avis`
--

LOCK TABLES `avis` WRITE;
/*!40000 ALTER TABLE `avis` DISABLE KEYS */;
INSERT INTO `avis` VALUES (1,5,'Excellente cuisine !',1,1),(2,4,'Très bon repas.',2,2),(3,5,'Délicieux, je recommande.',3,3),(4,3,'Correct sans plus.',4,4),(5,4,'Sympa et rapide.',5,5),(6,5,'Génial !',6,6),(7,2,'Déçu du plat.',7,7),(8,4,'Bon rapport qualité/prix.',8,8),(9,5,'Rien à dire !',9,9),(10,3,'Bof.',10,10);
/*!40000 ALTER TABLE `avis` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `client`
--

DROP TABLE IF EXISTS `client`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `client` (
  `id_client` bigint NOT NULL,
  `telephone` int DEFAULT NULL,
  `adresse_mail` varchar(50) DEFAULT NULL,
  `ville` varchar(50) DEFAULT NULL,
  `numero_de_rue` int DEFAULT NULL,
  `rue` varchar(50) DEFAULT NULL,
  `code_postal` int DEFAULT NULL,
  `metro_le_plus_proche` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id_client`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `client`
--

LOCK TABLES `client` WRITE;
/*!40000 ALTER TABLE `client` DISABLE KEYS */;
INSERT INTO `client` VALUES (1,1234567890,'client1@gmail.com','Paris',10,'Rue Lafayette',75010,'Gare du Nord'),(2,1234567891,'client2@gmail.com','Paris',15,'Boulevard Haussmann',75009,'Chaussée d\'Antin'),(3,1234567892,'client3@gmail.com','Paris',22,'Rue de Rennes',75006,'Montparnasse'),(4,1234567893,'client4@gmail.com','Paris',8,'Avenue de Clichy',75017,'La Fourche'),(5,1234567894,'client5@gmail.com','Paris',5,'Rue Mouffetard',75005,'Place Monge'),(6,1234567895,'client6@gmail.com','Paris',7,'Rue Oberkampf',75011,'Parmentier'),(7,1234567896,'client7@gmail.com','Paris',3,'Rue Saint-Honoré',75001,'Châtelet'),(8,1234567897,'client8@gmail.com','Paris',12,'Avenue Victor Hugo',75116,'Victor Hugo'),(9,1234567898,'client9@gmail.com','Paris',9,'Rue Lecourbe',75015,'Sèvres-Lecourbe'),(10,1234567899,'client10@gmail.com','Paris',14,'Rue du Faubourg Saint-Antoine',75012,'Ledru-Rollin'),(11,1234567800,'client11@gmail.com','Paris',21,'Boulevard Voltaire',75011,'Oberkampf'),(12,1234567801,'client12@gmail.com','Paris',17,'Avenue Gambetta',75020,'Gambetta'),(13,1234567802,'client13@gmail.com','Paris',11,'Rue Saint-Maur',75011,'Parmentier'),(14,1234567803,'client14@gmail.com','Paris',16,'Rue Vaugirard',75015,'Pasteur'),(15,1234567804,'client15@gmail.com','Paris',19,'Rue Monge',75005,'Censier-Daubenton'),(16,1234567805,'client16@gmail.com','Paris',6,'Rue Damrémont',75018,'Lamarck-Caulaincourt'),(17,1234567806,'client17@gmail.com','Paris',25,'Rue de Belleville',75020,'Jourdain'),(18,1234567807,'client18@gmail.com','Paris',4,'Rue Rivoli',75004,'Hôtel de Ville'),(19,1234567808,'client19@gmail.com','Paris',13,'Rue de Tolbiac',75013,'Tolbiac'),(20,1234567809,'client20@gmail.com','Paris',20,'Rue Lamarck',75018,'Lamarck-Caulaincourt');
/*!40000 ALTER TABLE `client` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `commande`
--

DROP TABLE IF EXISTS `commande`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `commande` (
  `id_commande` bigint NOT NULL,
  `addition` decimal(15,2) DEFAULT NULL,
  `etat_de_la_commande` varchar(50) DEFAULT NULL,
  `date_` datetime DEFAULT NULL,
  `id_client` bigint NOT NULL,
  PRIMARY KEY (`id_commande`),
  KEY `id_client` (`id_client`),
  CONSTRAINT `commande_ibfk_1` FOREIGN KEY (`id_client`) REFERENCES `client` (`id_client`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `commande`
--

LOCK TABLES `commande` WRITE;
/*!40000 ALTER TABLE `commande` DISABLE KEYS */;
INSERT INTO `commande` VALUES (1,25.00,'valide','2025-01-10 00:00:00',1),(2,30.00,'en attente','2025-01-11 00:00:00',2),(3,45.00,'livrée','2025-01-12 00:00:00',3),(4,20.00,'annulée','2025-01-13 00:00:00',4),(5,60.00,'valide','2025-01-14 00:00:00',5),(6,15.00,'valide','2025-01-15 00:00:00',6),(7,35.00,'livrée','2025-01-16 00:00:00',7),(8,50.00,'en cours','2025-01-17 00:00:00',8),(9,40.00,'valide','2025-01-18 00:00:00',9),(10,22.00,'en cours','2025-01-19 00:00:00',10),(11,55.00,'livrée','2025-01-20 00:00:00',11),(12,38.00,'valide','2025-01-21 00:00:00',12),(13,27.00,'en attente','2025-01-22 00:00:00',13),(14,48.00,'annulée','2025-01-23 00:00:00',14),(15,33.00,'valide','2025-01-24 00:00:00',15),(16,29.00,'livrée','2025-01-25 00:00:00',16),(17,66.00,'en cours','2025-01-26 00:00:00',17),(18,44.00,'valide','2025-01-27 00:00:00',18),(19,39.00,'livrée','2025-01-28 00:00:00',19),(20,50.00,'en attente','2025-01-29 00:00:00',20),(28,15.00,'livre','2025-04-30 00:00:00',1);
/*!40000 ALTER TABLE `commande` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `composer`
--

DROP TABLE IF EXISTS `composer`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `composer` (
  `id_Plat` bigint NOT NULL,
  `id_Ingrédient` bigint NOT NULL,
  `quantité` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id_Plat`,`id_Ingrédient`),
  KEY `id_Ingrédient` (`id_Ingrédient`),
  CONSTRAINT `composer_ibfk_1` FOREIGN KEY (`id_Plat`) REFERENCES `plat` (`id_Plat`),
  CONSTRAINT `composer_ibfk_2` FOREIGN KEY (`id_Ingrédient`) REFERENCES `ingrédients` (`id_Ingrédient`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `composer`
--

LOCK TABLES `composer` WRITE;
/*!40000 ALTER TABLE `composer` DISABLE KEYS */;
INSERT INTO `composer` VALUES (1,1,'300g'),(1,2,'500g'),(1,3,'200g'),(1,4,'100g'),(2,5,'150g'),(2,6,'100g'),(2,7,'50g'),(3,8,'200g'),(3,9,'300g'),(4,10,'100g'),(4,11,'150g'),(5,12,'3 pièces'),(5,13,'250g'),(6,14,'150g'),(7,15,'400g'),(8,16,'300g'),(9,11,'150g'),(10,17,'300g'),(11,14,'150g');
/*!40000 ALTER TABLE `composer` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cuisinier`
--

DROP TABLE IF EXISTS `cuisinier`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cuisinier` (
  `id_Cuisinier` bigint NOT NULL,
  `prénom` varchar(50) DEFAULT NULL,
  `nom` varchar(50) DEFAULT NULL,
  `téléphone` int DEFAULT NULL,
  `adresse_mail` varchar(50) DEFAULT NULL,
  `metro_le_plus_proche` varchar(50) DEFAULT NULL,
  `rue_` varchar(50) DEFAULT NULL,
  `ville` varchar(50) DEFAULT NULL,
  `numéro_rue` int DEFAULT NULL,
  `code_postal` int DEFAULT NULL,
  `id_client` bigint NOT NULL,
  `date_inscription` date DEFAULT NULL,
  PRIMARY KEY (`id_Cuisinier`),
  UNIQUE KEY `id_client` (`id_client`),
  CONSTRAINT `cuisinier_ibfk_1` FOREIGN KEY (`id_client`) REFERENCES `client` (`id_client`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cuisinier`
--

LOCK TABLES `cuisinier` WRITE;
/*!40000 ALTER TABLE `cuisinier` DISABLE KEYS */;
INSERT INTO `cuisinier` VALUES (1,'Marie','Legrand',1234512345,'marie.legrand@gmail.com','Parmentier','Rue Oberkampf','Paris',22,75011,1,NULL),(2,'Paul','Garcia',1234512346,'paul.garcia@gmail.com','Châtelet','Rue de Rivoli','Paris',33,75004,2,NULL),(3,'Isabelle','Fontaine',1234512347,'isabelle.fontaine@gmail.com','Rue du Bac','Rue du Bac','Paris',44,75007,3,NULL),(4,'Lucas','Lemoine',1234512348,'lucas.lemoine@gmail.com','La Muette','Avenue Mozart','Paris',55,75016,4,NULL),(5,'Camille','Faure',1234512349,'camille.faure@gmail.com','Blanche','Rue Lepic','Paris',66,75018,5,NULL),(6,'Sophie','Pires',1234512350,'sophie.pires@gmail.com','Étienne Marcel','Rue Montorgueil','Paris',77,75002,6,NULL),(7,'Hugo','Muller',1234512351,'hugo.muller@gmail.com','Château d\'Eau','Boulevard de Strasbourg','Paris',88,75010,7,NULL),(8,'Léa','Perrin',1234512352,'lea.perrin@gmail.com','Châtelet','Rue Saint-Denis','Paris',99,75001,8,NULL),(9,'Thomas','Garnier',1234512353,'thomas.garnier@gmail.com','Bibliothèque','Avenue de France','Paris',101,75013,9,NULL),(10,'Laura','Roussel',1234512354,'laura.roussel@gmail.com','Sèvres-Lecourbe','Rue Blomet','Paris',111,75015,10,NULL),(11,'Axel','Roux',1234512355,'axel.roux@gmail.com','Cluny-La Sorbonne','Boulevard Saint-Michel','Paris',121,75005,11,NULL),(12,'Amélie','Collet',1234512356,'amelie.collet@gmail.com','Rambuteau','Rue des Archives','Paris',131,75003,12,NULL),(13,'Maxime','Lopez',1234512357,'maxime.lopez@gmail.com','Censier-Daubenton','Rue Claude Bernard','Paris',141,75005,13,NULL),(14,'Sarah','Fleury',1234512358,'sarah.fleury@gmail.com','Charonne','Rue de Charonne','Paris',151,75011,14,NULL),(15,'Eva','Blanc',1234512359,'eva.blanc@gmail.com','Filles du Calvaire','Rue de Turenne','Paris',161,75003,15,NULL),(16,'Antoine','Guerin',1234512360,'antoine.guerin@gmail.com','Ménilmontant','Rue Oberkampf','Paris',171,75011,16,NULL),(17,'Emma','Marchand',1234512361,'emma.marchand@gmail.com','Jourdain','Rue de Belleville','Paris',181,75020,17,NULL),(18,'Nathan','Roy',1234512362,'nathan.roy@gmail.com','Place Monge','Rue Mouffetard','Paris',191,75005,18,NULL),(19,'Manon','Noel',1234512363,'manon.noel@gmail.com','Luxembourg','Rue Saint-Jacques','Paris',201,75005,19,NULL),(20,'Adam','Gauthier',1234512364,'adam.gauthier@gmail.com','Bastille','Rue de la Roquette','Paris',211,75011,20,NULL);
/*!40000 ALTER TABLE `cuisinier` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `entreprise`
--

DROP TABLE IF EXISTS `entreprise`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `entreprise` (
  `Siret` bigint NOT NULL,
  `nom_référent` varchar(50) DEFAULT NULL,
  `id_client` bigint NOT NULL,
  PRIMARY KEY (`Siret`),
  UNIQUE KEY `id_client` (`id_client`),
  CONSTRAINT `entreprise_ibfk_1` FOREIGN KEY (`id_client`) REFERENCES `client` (`id_client`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `entreprise`
--

LOCK TABLES `entreprise` WRITE;
/*!40000 ALTER TABLE `entreprise` DISABLE KEYS */;
/*!40000 ALTER TABLE `entreprise` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ingrédients`
--

DROP TABLE IF EXISTS `ingrédients`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ingrédients` (
  `id_Ingrédient` bigint NOT NULL,
  `nom` varchar(50) DEFAULT NULL,
  `quantité` varchar(50) DEFAULT NULL,
  `date_de_péremption` date DEFAULT NULL,
  PRIMARY KEY (`id_Ingrédient`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ingrédients`
--

LOCK TABLES `ingrédients` WRITE;
/*!40000 ALTER TABLE `ingrédients` DISABLE KEYS */;
INSERT INTO `ingrédients` VALUES (1,'Fromage','300g','2025-01-15'),(2,'Pommes de terre','500g','2025-01-15'),(3,'Jambon','200g','2025-01-15'),(4,'Cornichons','100g','2025-01-15'),(5,'Fraises','150g','2025-01-16'),(6,'Kiwi','100g','2025-01-16'),(7,'Sucre','50g','2025-01-16'),(8,'Saumon','200g','2025-01-17'),(9,'Riz vinaigré','300g','2025-01-17'),(10,'Tomate','100g','2025-01-18'),(11,'Mozzarella','150g','2025-01-18'),(12,'Tortilla','3 pièces','2025-01-19'),(13,'Viande hachée','250g','2025-01-19'),(14,'Pois chiches','150g','2025-01-20'),(15,'Poulet','400g','2025-01-20'),(16,'Semoule','300g','2025-01-21'),(17,'Pâtes','300g','2025-01-22'),(18,'Épinards','150g','2025-01-22'),(19,'Tofu','200g','2025-01-23'),(20,'Crevettes','200g','2025-01-24');
/*!40000 ALTER TABLE `ingrédients` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ligne_de_commande`
--

DROP TABLE IF EXISTS `ligne_de_commande`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ligne_de_commande` (
  `num_ligne` bigint NOT NULL,
  `quantité` int DEFAULT NULL,
  `id_livraison` bigint NOT NULL,
  `id_Plat` bigint NOT NULL,
  `id_commande` bigint NOT NULL,
  PRIMARY KEY (`num_ligne`),
  KEY `id_livraison` (`id_livraison`),
  KEY `id_Plat` (`id_Plat`),
  KEY `id_commande` (`id_commande`),
  CONSTRAINT `ligne_de_commande_ibfk_1` FOREIGN KEY (`id_livraison`) REFERENCES `livraison` (`id_livraison`),
  CONSTRAINT `ligne_de_commande_ibfk_2` FOREIGN KEY (`id_Plat`) REFERENCES `plat` (`id_Plat`),
  CONSTRAINT `ligne_de_commande_ibfk_3` FOREIGN KEY (`id_commande`) REFERENCES `commande` (`id_commande`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ligne_de_commande`
--

LOCK TABLES `ligne_de_commande` WRITE;
/*!40000 ALTER TABLE `ligne_de_commande` DISABLE KEYS */;
INSERT INTO `ligne_de_commande` VALUES (1,2,1,1,1),(2,1,2,2,2),(3,3,3,3,3),(4,2,4,4,4),(5,1,5,5,5),(6,1,6,6,6),(7,2,7,7,7),(8,1,8,8,8),(9,3,9,9,9),(10,2,10,10,10),(11,2,1,2,1);
/*!40000 ALTER TABLE `ligne_de_commande` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `livraison`
--

DROP TABLE IF EXISTS `livraison`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `livraison` (
  `id_livraison` bigint NOT NULL,
  `etat_livraison` varchar(50) DEFAULT NULL,
  `date_de_livraison` datetime DEFAULT NULL,
  PRIMARY KEY (`id_livraison`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `livraison`
--

LOCK TABLES `livraison` WRITE;
/*!40000 ALTER TABLE `livraison` DISABLE KEYS */;
INSERT INTO `livraison` VALUES (1,'Livré','2025-02-20 14:30:00'),(2,'En cours','2025-02-21 10:15:00'),(3,'Livré','2025-02-22 11:00:00'),(4,'En attente','2025-02-23 09:30:00'),(5,'Livré','2025-02-24 18:00:00'),(6,'Livré','2025-02-25 20:00:00'),(7,'En cours','2025-02-26 12:00:00'),(8,'Livré','2025-02-27 13:00:00'),(9,'En attente','2025-02-28 15:00:00'),(10,'Livré','2025-03-01 17:00:00');
/*!40000 ALTER TABLE `livraison` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `particulier`
--

DROP TABLE IF EXISTS `particulier`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `particulier` (
  `id_particulier` bigint NOT NULL,
  `nom` varchar(50) DEFAULT NULL,
  `prenom` varchar(50) DEFAULT NULL,
  `id_client` bigint NOT NULL,
  PRIMARY KEY (`id_particulier`),
  UNIQUE KEY `id_client` (`id_client`),
  CONSTRAINT `particulier_ibfk_1` FOREIGN KEY (`id_client`) REFERENCES `client` (`id_client`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `particulier`
--

LOCK TABLES `particulier` WRITE;
/*!40000 ALTER TABLE `particulier` DISABLE KEYS */;
INSERT INTO `particulier` VALUES (1,'Durand','Diana',1),(2,'Martin','Chloé',2),(3,'Bernard','Lucas',3),(4,'Dubois','Emma',4),(5,'Thomas','Léo',5),(6,'Robert','Manon',6),(7,'Richard','Arthur',7),(8,'Petit','Alice',8),(9,'Simon','Nathan',9),(10,'Michel','Julia',10),(11,'Lefevre','Noah',11),(12,'Moreau','Inès',12),(13,'Fournier','Enzo',13),(14,'Girard','Anna',14),(15,'Andre','Tom',15),(16,'Mercier','Lina',16),(17,'Dupont','Hugo',17),(18,'Lambert','Eva',18),(19,'Bonnet','Louis',19),(20,'Francois','Clara',20);
/*!40000 ALTER TABLE `particulier` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `plat`
--

DROP TABLE IF EXISTS `plat`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `plat` (
  `id_Plat` bigint NOT NULL,
  `nom` varchar(50) DEFAULT NULL,
  `prix` decimal(15,2) DEFAULT NULL,
  `recette` varchar(50) DEFAULT NULL,
  `nb_personnes` int DEFAULT NULL,
  `date_de_péremption` date DEFAULT NULL,
  `date_de_fabrication` date DEFAULT NULL,
  `nationalité` varchar(50) DEFAULT NULL,
  `régime_alimentaire` varchar(50) DEFAULT NULL,
  `id_Cuisinier` bigint NOT NULL,
  `plat_du_jour` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id_Plat`),
  KEY `id_Cuisinier` (`id_Cuisinier`),
  CONSTRAINT `plat_ibfk_1` FOREIGN KEY (`id_Cuisinier`) REFERENCES `cuisinier` (`id_Cuisinier`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `plat`
--

LOCK TABLES `plat` WRITE;
/*!40000 ALTER TABLE `plat` DISABLE KEYS */;
INSERT INTO `plat` VALUES (1,'Raclette',12.00,'Recette de raclette',4,'2025-01-15','2025-01-10','Française','',1,NULL),(2,'Salade de fruits',8.00,'Recette salade fruits',2,'2025-01-16','2025-01-11','Française','Végétarien',2,NULL),(3,'Sushi',15.00,'Recette sushi',2,'2025-01-17','2025-01-12','Japonaise','Sans gluten',3,NULL),(4,'Pizza Margherita',10.00,'Recette pizza',4,'2025-01-18','2025-01-13','Italienne','Végétarien',4,NULL),(5,'Tacos',9.00,'Recette tacos',3,'2025-01-19','2025-01-14','Mexicaine','',5,NULL),(6,'Burger végétarien',11.00,'Recette burger',2,'2025-01-20','2025-01-15','Américaine','Végétarien',6,NULL),(7,'Poulet basquaise',14.00,'Recette poulet',4,'2025-01-21','2025-01-16','Française','',7,NULL),(8,'Couscous',13.00,'Recette couscous',6,'2025-01-22','2025-01-17','Maghrébine','',8,NULL),(9,'Quiche Lorraine',10.00,'Recette quiche',4,'2025-01-23','2025-01-18','Française','',9,NULL),(10,'Lasagnes',12.00,'Recette lasagnes',6,'2025-01-24','2025-01-19','Italienne','',10,NULL),(11,'Falafel',9.00,'Recette falafel',3,'2025-01-25','2025-01-20','Libanaise','Végétarien',11,NULL),(12,'Pad Thaï',13.00,'Recette Pad Thai',4,'2025-01-26','2025-01-21','Thaïlandaise','',12,NULL),(13,'Chili sin carne',11.00,'Recette chili',4,'2025-01-27','2025-01-22','Mexicaine','Végétarien',13,NULL),(14,'Soupe Pho',14.00,'Recette Pho',2,'2025-01-28','2025-01-23','Vietnamienne','',14,NULL),(15,'Ramen',15.00,'Recette ramen',2,'2025-01-29','2025-01-24','Japonaise','',15,NULL),(16,'Tarte Tatin',8.00,'Recette tarte',6,'2025-01-30','2025-01-25','Française','Végétarien',16,NULL),(17,'Poke bowl',13.00,'Recette poke bowl',2,'2025-01-31','2025-01-26','Hawaïenne','Végétarien',17,NULL),(18,'Bouillabaisse',18.00,'Recette bouillabaisse',4,'2025-02-01','2025-01-27','Française','',18,NULL),(19,'Moussaka',14.00,'Recette moussaka',6,'2025-02-02','2025-01-28','Grecque','',19,NULL),(20,'Biryani',15.00,'Recette biryani',4,'2025-02-03','2025-01-29','Indienne','',20,NULL);
/*!40000 ALTER TABLE `plat` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `transaction`
--

DROP TABLE IF EXISTS `transaction`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `transaction` (
  `id_client` bigint NOT NULL,
  `id_Cuisinier` bigint NOT NULL,
  `effectue` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id_client`,`id_Cuisinier`),
  KEY `id_Cuisinier` (`id_Cuisinier`),
  CONSTRAINT `transaction_ibfk_1` FOREIGN KEY (`id_client`) REFERENCES `client` (`id_client`),
  CONSTRAINT `transaction_ibfk_2` FOREIGN KEY (`id_Cuisinier`) REFERENCES `cuisinier` (`id_Cuisinier`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `transaction`
--

LOCK TABLES `transaction` WRITE;
/*!40000 ALTER TABLE `transaction` DISABLE KEYS */;
INSERT INTO `transaction` VALUES (1,1,1),(1,2,0),(2,1,1),(2,3,1),(3,2,0),(3,3,1);
/*!40000 ALTER TABLE `transaction` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-04-30 10:54:41
