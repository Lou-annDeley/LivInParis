DROP DATABASE IF EXISTS Livraison;
CREATE DATABASE IF NOT EXISTS Livraison;
USE Livraison;

DROP TABLE IF EXISTS Client;
CREATE TABLE IF NOT EXISTS Client(
   id_client BIGINT,
   telephone INT,
   adresse_mail VARCHAR(50),
   ville VARCHAR(50),
   numero_de_rue INT,
   rue VARCHAR(50),
   code_postal INT,
   metro_le_plus_proche VARCHAR(50),
   PRIMARY KEY(id_client)
);

DROP TABLE IF EXISTS Cuisinier;
CREATE TABLE IF NOT EXISTS Cuisinier(
   id_Cuisinier BIGINT,
   prénom VARCHAR(50),
   nom VARCHAR(50),
   téléphone INT,
   adresse_mail VARCHAR(50),
   metro_le_plus_proche VARCHAR(50),
   rue_ VARCHAR(50),
   ville VARCHAR(50),
   numéro_rue INT,
   code_postal INT,
   id_client BIGINT NOT NULL,
   PRIMARY KEY(id_Cuisinier),
   UNIQUE(id_client),
   FOREIGN KEY(id_client) REFERENCES Client(id_client)
);

DROP TABLE IF EXISTS Ingrédients;
CREATE TABLE IF NOT EXISTS Ingrédients(
   id_Ingrédient BIGINT,
   nom VARCHAR(50),
   quantité VARCHAR(50),
   date_de_péremption DATE,
   PRIMARY KEY(id_Ingrédient)
);

DROP TABLE IF EXISTS Entreprise;
CREATE TABLE IF NOT EXISTS Entreprise(
   Siret BIGINT,
   nom_référent VARCHAR(50),
   id_client BIGINT NOT NULL,
   PRIMARY KEY(Siret),
   UNIQUE(id_client),
   FOREIGN KEY(id_client) REFERENCES Client(id_client)
);

DROP TABLE IF EXISTS particulier;
CREATE TABLE IF NOT EXISTS particulier(
   id_particulier BIGINT,
   nom VARCHAR(50),
   prenom VARCHAR(50),
   id_client BIGINT NOT NULL,
   PRIMARY KEY(id_particulier),
   UNIQUE(id_client),
   FOREIGN KEY(id_client) REFERENCES Client(id_client)
);

DROP TABLE IF EXISTS Livraison;
CREATE TABLE IF NOT EXISTS Livraison(
   id_livraison BIGINT,
   etat_livraison VARCHAR(50),
   date_de_livraison DATETIME,
   PRIMARY KEY(id_livraison)
);

DROP TABLE IF EXISTS Avis;
CREATE TABLE IF NOT EXISTS avis(
   id_avis BIGINT,
   note INT,
   commentaire VARCHAR(50),
   id_client BIGINT NOT NULL,
   id_Cuisinier BIGINT NOT NULL,
   PRIMARY KEY(id_avis),
   FOREIGN KEY(id_client) REFERENCES Client(id_client),
   FOREIGN KEY(id_Cuisinier) REFERENCES Cuisinier(id_Cuisinier)
);

DROP TABLE IF EXISTS Commande; 
CREATE TABLE IF NOT EXISTS Commande(
   id_commande BIGINT,
   addition DECIMAL(15,2),
   etat_de_la_commande VARCHAR(50),
   date_ DATETIME,
   id_client BIGINT NOT NULL,
   PRIMARY KEY(id_commande),
   FOREIGN KEY(id_client) REFERENCES Client(id_client)
);

DROP TABLE IF EXISTS Plat;
CREATE TABLE IF NOT EXISTS Plat(
   id_Plat BIGINT,
   nom VARCHAR(50),
   prix DECIMAL(15,2),
   recette VARCHAR(50),
   nb_personnes INT,
   date_de_péremption DATE,
   date_de_fabrication DATE,
   nationalité VARCHAR(50),
   régime_alimentaire VARCHAR(50),
   id_Cuisinier BIGINT NOT NULL,
   plat_du_jour BOOL,
   PRIMARY KEY(id_Plat),
   FOREIGN KEY(id_Cuisinier) REFERENCES Cuisinier(id_Cuisinier)
);

DROP TABLE IF EXISTS ligne_de_commande;
CREATE TABLE IF NOT EXISTS ligne_de_commande(
   num_ligne BIGINT,
   quantité INT,
   id_livraison BIGINT NOT NULL,
   id_Plat BIGINT NOT NULL,
   id_commande BIGINT NOT NULL,
   PRIMARY KEY(num_ligne),
   FOREIGN KEY(id_livraison) REFERENCES Livraison(id_livraison),
   FOREIGN KEY(id_Plat) REFERENCES Plat(id_Plat),
   FOREIGN KEY(id_commande) REFERENCES Commande(id_commande)
);

DROP TABLE IF EXISTS Composer;
CREATE TABLE IF NOT EXISTS composer(
   id_Plat BIGINT,
   id_Ingrédient BIGINT,
   quantité VARCHAR(50),
   PRIMARY KEY(id_Plat, id_Ingrédient),
   FOREIGN KEY(id_Plat) REFERENCES Plat(id_Plat),
   FOREIGN KEY(id_Ingrédient) REFERENCES Ingrédients(id_Ingrédient)
);

INSERT INTO Client (id_client, telephone, adresse_mail, ville, numero_de_rue, rue, code_postal, metro_le_plus_proche) 
VALUES (1, 1234567890, 'Mdurand@gmail.com', 'Paris', 15, 'Rue Cardinet', 75017, 'Cardinet'),
(2, 1234567898, 'entreprise@gmail.com', 'Paris', 15, 'Rue Cardinet', 75017, 'Cardinet');

INSERT INTO Particulier(id_particulier, nom, prenom, id_client)
VALUES(1,'Durand', 'Medhy',1);

INSERT INTO Cuisinier (id_Cuisinier, nom, prénom, téléphone, adresse_mail, ville, numéro_rue, rue_, code_postal, metro_le_plus_proche, id_client) 
VALUES (1, 'Dupond', 'Marie', 1234567890, 'Mdupond@gmail.com', 'Paris', 30, 'Rue de la République', 75011, 'République',1);

INSERT INTO Commande(id_commande, addition, etat_de_la_commande, date_,id_client)
VALUES(1,10,'valide','2025-01-10',1),(2,5,'valide', '2025-01-10',1);

INSERT INTO Plat (id_Plat, nom, prix, recette, nb_personnes, date_de_fabrication, date_de_péremption, nationalité, régime_alimentaire, id_Cuisinier) 
VALUES (1, 'Raclette', 10, 'Recette de raclette', 6, '2025-01-10', '2025-01-15', 'Française', '', 1),
(2, 'Salade de fruit', 5, 'Recette de salade de fruit', 6, '2025-01-10', '2025-01-15', 'Indifférent', 'Végétarien', 1);

INSERT INTO Ingrédients (id_Ingrédient, nom, quantité,date_de_péremption) VALUES 
(1,'raclette fromage', '250g','2025-01-15'), 
(2,'pommes_de_terre', '200g','2025-01-15'), 
(3,'jambon', '200g','2025-01-15'), 
(4,'cornichon', '3p','2025-01-15'),
(5,'fraise', '100g','2025-01-15'), 
(6,'kiwi', '100g','2025-01-15'), 
(7,'sucre', '10g','2025-01-15');

INSERT INTO Composer (id_Plat, id_Ingrédient, quantité) VALUES 
(1, (SELECT id_Ingrédient FROM Ingrédients WHERE nom='raclette fromage'), '250g'),
(1, (SELECT id_Ingrédient FROM Ingrédients WHERE nom='pommes_de_terre'), '200g'),
(1, (SELECT id_Ingrédient FROM Ingrédients WHERE nom='jambon'), '200g'),
(1, (SELECT id_Ingrédient FROM Ingrédients WHERE nom='cornichon'), '3p'),
(2, (SELECT id_Ingrédient FROM Ingrédients WHERE nom='fraise'), '100g'),
(2, (SELECT id_Ingrédient FROM Ingrédients WHERE nom='kiwi'), '100g'),
(2, (SELECT id_Ingrédient FROM Ingrédients WHERE nom='sucre'), '10g');

INSERT INTO Entreprise (Siret, nom_référent, id_client) 
VALUES (12345678901234, 'Jean Dupont', 2);

INSERT INTO Livraison (id_livraison, etat_livraison, date_de_livraison) VALUES
(1, 'Livré', '2025-02-20 14:30:00'),
(2, 'En cours', '2025-02-21 10:15:00');

INSERT INTO avis (id_avis, note, commentaire, id_client, id_Cuisinier) VALUES
(1, 5, 'Excellente cuisine', 1, 1),
(2, 4, 'Très bon repas', 2, 1);

INSERT INTO ligne_de_commande (num_ligne, quantité, id_livraison, id_Plat, id_commande) VALUES
(1, 3, 1, 1, 1);