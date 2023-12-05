DROP DATABASE IF EXISTS `food2rue`;
CREATE DATABASE IF NOT EXISTS `food2rue` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;
USE `food2rue`;

CREATE TABLE pays (
    sigle VARCHAR(2),
    nom VARCHAR(255),
    PRIMARY KEY (sigle)
);

CREATE TABLE media (
  id INT,
  url_source VARCHAR(255),
  PRIMARY KEY (id)
);

CREATE TABLE image (
  id INT,
  PRIMARY KEY (id),
  FOREIGN KEY (id) REFERENCES media(id)
);

CREATE TABLE video (
  id INT,
  PRIMARY KEY (id),
  FOREIGN KEY (id) REFERENCES media(id)
);

CREATE TABLE utilisateur (
  id INT,
  nom VARCHAR(255),
  prénom VARCHAR(255),
  email VARCHAR(255),
  fk_photo_profil INT,
  pseudo VARCHAR(255),
  date_naissance DATE,
  date_creation_profil DATE,
  description VARCHAR(255),
  fk_pays VARCHAR(2),
  PRIMARY KEY (id),
  FOREIGN KEY (fk_pays) REFERENCES pays(sigle),
  FOREIGN KEY (fk_photo_profil) REFERENCES image(id)
);

CREATE TABLE administrateur (
  id INT,
  PRIMARY KEY (id),
  FOREIGN KEY (id) REFERENCES utilisateur(id)
);

CREATE TABLE publication (
  id INT,
  texte VARCHAR(255),
  date_publication DATE,
  fk_parent INT,
  fk_utilisateur INT NOT NULL,
  fk_video INT,
  fk_recette INT,
  PRIMARY KEY (id),
  FOREIGN KEY (fk_parent) REFERENCES publication(id),
  FOREIGN KEY (fk_utilisateur) REFERENCES utilisateur(id),
  FOREIGN KEY (fk_video) REFERENCES video(id),
  FOREIGN KEY (fk_recette) REFERENCES recette(id)
);

CREATE TABLE publication_image (
  fk_publication INT,
  fk_image INT,
  PRIMARY KEY (fk_publication, fk_image),
  FOREIGN KEY (fk_publication) REFERENCES publication(id),
  FOREIGN KEY (fk_image) REFERENCES image(id)
);

CREATE TABLE aime_publication_utilisateur (
  fk_publication INT,
  fk_utilisateur INT,
  PRIMARY KEY (fk_publication, fk_utilisateur),
  FOREIGN KEY (fk_publication) REFERENCES publication(id),
  FOREIGN KEY (fk_utilisateur) REFERENCES utilisateur(id)
);

CREATE TABLE recette (
  id INT,
  nom VARCHAR(255),
  temps_preparation INT,
  temps_cuisson INT,
  temps_repos INT,
  date_creation DATE,
  etape VARCHAR(255),
  fk_utilisateur INT,
  fk_video INT,
  fk_pays VARCHAR(2),
  PRIMARY KEY (id),
  FOREIGN KEY (fk_utilisateur) REFERENCES utilisateur(id),
  FOREIGN KEY (fk_video) REFERENCES video(id),
  FOREIGN KEY (fk_pays) REFERENCES pays(sigle)
);

CREATE TABLE note (
  fk_utilisateur INT,
  fk_publication INT,
  note INT,
  PRIMARY KEY (fk_utilisateur, fk_publication),
  FOREIGN KEY (fk_utilisateur) REFERENCES utilisateur(id),
  FOREIGN KEY (fk_publication) REFERENCES publication(id)
);

CREATE TABLE recette_image (
  fk_recette INT,
  fk_image INT,
  PRIMARY KEY (fk_recette, fk_image),
  FOREIGN KEY (fk_recette) REFERENCES recette(id),
  FOREIGN KEY (fk_image) REFERENCES image(id)
);

CREATE TABLE tag (
  id INT,
  nom VARCHAR(255),
  PRIMARY KEY (id)
);

CREATE TABLE publication_tag (
  fk_publication INT,
  fk_tag INT,
  PRIMARY KEY (fk_publication, fk_tag),
  FOREIGN KEY (fk_publication) REFERENCES publication(id),
  FOREIGN KEY (fk_tag) REFERENCES tag(id)
);

CREATE TABLE recette_tag (
  fk_recette INT,
  fk_tag INT,
  PRIMARY KEY (fk_recette, fk_tag),
  FOREIGN KEY (fk_recette) REFERENCES recette(id),
  FOREIGN KEY (fk_tag) REFERENCES tag(id)
);

CREATE TABLE ajoutable (
  id INT,
  nom VARCHAR(255),
  description VARCHAR(255),
  date_publication DATE,
  fk_image INT NOT NULL,
  fk_utilisateur INT,
  fk_administrateur INT,
  est_valide BOOLEAN,
  PRIMARY KEY (id),
  FOREIGN KEY (fk_administrateur) REFERENCES administrateur(id),
  FOREIGN KEY (fk_utilisateur) REFERENCES utilisateur(id),
  FOREIGN KEY (fk_image) REFERENCES image(id)
);

CREATE TABLE ingredient (
  id INT,
  PRIMARY KEY (id),
  FOREIGN KEY (id) REFERENCES ajoutable(id)
);

CREATE TABLE recette_ingredient (
  fk_recette INT,
  fk_ingredient INT,
  quantite VARCHAR(255),
  PRIMARY KEY (fk_recette, fk_ingredient),
  FOREIGN KEY (fk_recette) REFERENCES recette(id),
  FOREIGN KEY (fk_ingredient) REFERENCES ingredient(id)
);

CREATE TABLE allergene (
  nom VARCHAR(255),
  PRIMARY KEY (nom)
);

CREATE TABLE ingredient_allergene (
  fk_ingredient INT,
  fk_allergene VARCHAR(255),
  PRIMARY KEY (fk_ingredient, fk_allergene),
  FOREIGN KEY (fk_ingredient) REFERENCES ingredient(id),
  FOREIGN KEY (fk_allergene) REFERENCES allergene(nom)
);

CREATE TABLE ustensile (
  id INT,
  nom VARCHAR(255),
  PRIMARY KEY (id),
  FOREIGN KEY (id) REFERENCES ajoutable(id)
);

CREATE TABLE recette_ustensile (
  fk_recette INT,
  fk_ustensile INT,
  PRIMARY KEY (fk_recette, fk_ustensile),
  FOREIGN KEY (fk_recette) REFERENCES recette(id),
  FOREIGN KEY (fk_ustensile) REFERENCES ustensile(id)
);


INSERT INTO pays (sigle, nom)
VALUES ('AF', 'Afghanistan'),
       ('AL', 'Albanie'),
       ('DZ', 'Algérie'),
       ('AS', 'Samoa américaines'),
       ('AD', 'Andorre'),
       ('AO', 'Angola'),
       ('AI', 'Anguilla'),
       ('AQ', 'Antarctique'),
       ('AG', 'Antigua-et-Barbuda'),
       ('AR', 'Argentine'),
       ('AM', 'Arménie'),
       ('AW', 'Aruba'),
       ('AU', 'Australie'),
       ('AT', 'Autriche'),
       ('AZ', 'Azerbaïdjan'),
       ('BS', 'Bahamas'),
       ('BH', 'Bahreïn'),
       ('BD', 'Bangladesh'),
       ('BB', 'Barbade'),
       ('BY', 'Biélorussie'),
       ('BE', 'Belgique'),
       ('BZ', 'Belize'),
       ('BJ', 'Bénin'),
       ('BM', 'Bermudes'),
       ('BT', 'Bhoutan'),
       ('BO', 'Bolivie'),
       ('BA', 'Bosnie-Herzégovine'),
       ('BW', 'Botswana'),
       ('BV', 'Île Bouvet'),
       ('BR', 'Brésil'),
       ('IO', 'Territoire britannique de l''océan Indien'),
       ('BN', 'Brunéi Darussalam'),
       ('BG', 'Bulgarie'),
       ('BF', 'Burkina Faso'),
       ('BI', 'Burundi'),
       ('KH', 'Cambodge'),
       ('CM', 'Cameroun'),
       ('CA', 'Canada'),
       ('CV', 'Cap-Vert'),
       ('KY', 'Îles Caïmans'),
       ('CF', 'République centrafricaine'),
       ('TD', 'Tchad'),
       ('CL', 'Chili'),
       ('CN', 'Chine'),
       ('CX', 'Île Christmas'),
       ('CC', 'Îles Cocos (Keeling)'),
       ('CO', 'Colombie'),
       ('KM', 'Comores'),
       ('CG', 'Congo'),
       ('CD', 'République démocratique du Congo'),
       ('CK', 'Îles Cook'),
       ('CR', 'Costa Rica'),
       ('CI', 'Côte d''Ivoire'),
       ('HR', 'Croatie'),
       ('CU', 'Cuba'),
       ('CY', 'Chypre'),
       ('CZ', 'République tchèque'),
       ('DK', 'Danemark'),
       ('DJ', 'Djibouti'),
       ('DM', 'Dominique'),
       ('DO', 'République dominicaine'),
       ('EC', 'Équateur'),
       ('EG', 'Égypte'),
       ('SV', 'El Salvador'),
       ('GQ', 'Guinée équatoriale'),
       ('ER', 'Érythrée'),
       ('EE', 'Estonie'),
       ('ET', 'Éthiopie'),
       ('FK', 'Îles Falkland (Malvinas)'),
       ('FO', 'Îles Féroé'),
       ('FJ', 'Fidji'),
       ('FI', 'Finlande'),
       ('FR', 'France'),
       ('GF', 'Guyane française'),
       ('PF', 'Polynésie française'),
       ('TF', 'Terres australes françaises'),
       ('GA', 'Gabon'),
       ('GM', 'Gambie'),
       ('GE', 'Géorgie'),
       ('DE', 'Allemagne'),
       ('GH', 'Ghana'),
       ('GI', 'Gibraltar'),
       ('GR', 'Grèce'),
       ('GL', 'Groenland'),
       ('GD', 'Grenade'),
       ('GP', 'Guadeloupe'),
       ('GU', 'Guam'),
       ('GT', 'Guatemala'),
       ('GN', 'Guinée'),
       ('GW', 'Guinée-Bissau'),
       ('GY', 'Guyana'),
       ('HT', 'Haïti'),
       ('HM', 'Îles Heard et McDonald'),
       ('VA', 'Saint-Siège (État de la Cité du Vatican)'),
       ('HN', 'Honduras'),
       ('HK', 'Hong Kong'),
       ('HU', 'Hongrie'),
       ('IS', 'Islande'),
       ('IN', 'Inde'),
       ('ID', 'Indonésie'),
       ('IR', 'Iran, République islamique d'''),
       ('IQ', 'Irak'),
       ('IE', 'Irlande'),
       ('IL', 'Israël'),
       ('IT', 'Italie'),
       ('JM', 'Jamaïque'),
       ('JP', 'Japon'),
       ('JO', 'Jordanie'),
       ('KZ', 'Kazakhstan'),
       ('KE', 'Kenya'),
       ('KI', 'Kiribati'),
       ('KP', 'Corée, République populaire démocratique de'),
       ('KR', 'Corée, République de'),
       ('KW', 'Koweït'),
       ('KG', 'Kirghizistan'),
       ('LA', 'République démocratique populaire lao'),
       ('LV', 'Lettonie'),
       ('LB', 'Liban'),
       ('LS', 'Lesotho'),
       ('LR', 'Libéria'),
       ('LY', 'Jamahiriya arabe libyenne'),
       ('LI', 'Liechtenstein'),
       ('LT', 'Lituanie'),
       ('LU', 'Luxembourg'),
       ('MO', 'Macao'),
       ('MK', 'Macédoine, ex-République yougoslave de'),
       ('MG', 'Madagascar'),
       ('MW', 'Malawi'),
       ('MY', 'Malaisie'),
       ('MV', 'Maldives'),
       ('ML', 'Mali'),
       ('MT', 'Malte'),
       ('MH', 'Îles Marshall'),
       ('MQ', 'Martinique'),
       ('MR', 'Mauritanie'),
       ('MU', 'Maurice'),
       ('YT', 'Mayotte'),
       ('MX', 'Mexique'),
       ('FM', 'Micronésie, États fédérés de'),
       ('MD', 'Moldavie, République de'),
       ('MC', 'Monaco'),
       ('MN', 'Mongolie'),
       ('MS', 'Montserrat'),
       ('MA', 'Maroc'),
       ('MZ', 'Mozambique'),
       ('MM', 'Myanmar'),
       ('NA', 'Namibie'),
       ('NR', 'Nauru'),
       ('NP', 'Népal'),
       ('NL', 'Pays-Bas'),
       ('AN', 'Antilles néerlandaises'),
       ('NC', 'Nouvelle-Calédonie'),
       ('NZ', 'Nouvelle-Zélande'),
       ('NI', 'Nicaragua'),
       ('NE', 'Niger'),
       ('NG', 'Nigéria'),
       ('NU', 'Niué'),
       ('NF', 'Île Norfolk'),
       ('MP', 'Îles Mariannes du Nord'),
       ('NO', 'Norvège'),
       ('OM', 'Oman'),
       ('PK', 'Pakistan'),
       ('PW', 'Palaos'),
       ('PS', 'Territoire palestinien occupé'),
       ('PA', 'Panama'),
       ('PG', 'Papouasie-Nouvelle-Guinée'),
       ('PY', 'Paraguay'),
       ('PE', 'Pérou'),
       ('PH', 'Philippines'),
       ('PN', 'Pitcairn'),
       ('PL', 'Pologne'),
       ('PT', 'Portugal'),
       ('PR', 'Porto Rico'),
       ('QA', 'Qatar'),
       ('RE', 'Réunion'),
       ('RO', 'Roumanie'),
       ('RU', 'Fédération de Russie'),
       ('RW', 'Rwanda'),
       ('SH', 'Sainte-Hélène'),
       ('KN', 'Saint-Kitts-et-Nevis'),
       ('LC', 'Sainte-Lucie'),
       ('PM', 'Saint-Pierre-et-Miquelon'),
       ('VC', 'Saint-Vincent-et-les Grenadines'),
       ('WS', 'Samoa'),
       ('SM', 'Saint-Marin'),
       ('ST', 'Sao Tomé-et-Principe'),
       ('SA', 'Arabie saoudite'),
       ('SN', 'Sénégal'),
       ('CS', 'Serbie-et-Monténégro'),
       ('SC', 'Seychelles'),
       ('SL', 'Sierra Leone'),
       ('SG', 'Singapour'),
       ('SK', 'Slovaquie'),
       ('SI', 'Slovénie'),
       ('SB', 'Îles Salomon'),
       ('SO', 'Somalie'),
       ('ZA', 'Afrique du Sud'),
       ('GS', 'Géorgie du Sud et les îles Sandwich du Sud'),
       ('ES', 'Espagne'),
       ('LK', 'Sri Lanka'),
       ('SD', 'Soudan'),
       ('SR', 'Suriname'),
       ('SJ', 'Svalbard et Île Jan Mayen'),
       ('SZ', 'Swaziland'),
       ('SE', 'Suède'),
       ('CH', 'Suisse'),
       ('SY', 'République arabe syrienne'),
       ('TW', 'Taïwan, Province de Chine'),
       ('TJ', 'Tadjikistan'),
       ('TZ', 'Tanzanie, République-Unie de'),
       ('TH', 'Thaïlande'),
       ('TL', 'Timor-Leste'),
       ('TG', 'Togo'),
       ('TK', 'Tokelau'),
       ('TO', 'Tonga'),
       ('TT', 'Trinité-et-Tobago'),
       ('TN', 'Tunisie'),
       ('TR', 'Turquie'),
       ('TM', 'Turkménistan'),
       ('TC', 'Îles Turks et Caïques'),
       ('TV', 'Tuvalu'),
       ('UG', 'Ouganda'),
       ('UA', 'Ukraine'),
       ('AE', 'Émirats arabes unis'),
       ('GB', 'Royaume-Uni'),
       ('US', 'États-Unis'),
       ('UM', 'Îles mineures éloignées des États-Unis'),
       ('UY', 'Uruguay'),
       ('UZ', 'Ouzbékistan'),
       ('VU', 'Vanuatu'),
       ('VE', 'Venezuela'),
       ('VN', 'Viet Nam'),
       ('VG', 'Îles Vierges britanniques'),
       ('VI', 'Îles Vierges américaines'),
       ('WF', 'Wallis-et-Futuna'),
       ('EH', 'Sahara occidental'),
       ('YE', 'Yémen'),
       ('ZM', 'Zambie'),
       ('ZW', 'Zimbabwe');


INSERT INTO tag (id, nom)
VALUES (1, 'digest'),
       (2, 'healthy'),
       (3, 'fit'),
       (4, 'fastfood maison'),
       (5, 'facile à faire'),
       (6, 'rapide à faire'),
       (7, 'anniversaire'),
       (8, 'birthday cake'),
       (9, 'viande crue'),
       (10, 'hallal'),
       (11, 'casher'),
       (12, 'végétarien'),
       (13, 'végétalien'),
       (14, 'sans gluten'),
       (15, 'sans lactose'),
       (16, 'sans oeuf'),
       (17, 'sans sucre'),
       (18, 'sans sel'),
       (19, 'sans matière grasse'),
       (20, 'sans viande'),
       (21, 'sans poisson'),
       (22, 'sans fruits de mer'),
       (23, 'sans crustacés'),
       (24, 'sans arachide'),
       (25, 'sans soja'),
       (26, 'sans sésame'),
       (27, 'sans lupin'),
       (28, 'sans moutarde'),
       (29, 'sans céleri'),
       (30, 'sans sulfites'),
       (31, 'sans fruits à coque'),
       (32, 'sans mollusques'),
       (33, 'sans alcool'),
       (34, 'sans porc'),
       (35, 'sans boeuf'),
       (36, 'sans poulet'),
       (37, 'sans agneau'),
       (38, 'sans veau'),
       (39, 'sans lapin'),
       (40, 'sans canard'),
       (41, 'sans dinde'),
       (42, 'sans oie'),
       (43, 'sans cheval'),
       (44, 'sans gibier'),
       (45, 'sans escargot'),
       (46, 'sans grenouille'),
       (47, 'sans insecte'),
       (48, 'sans viande de brousse'),
       (49, 'sans viande de chien'),
       (50, 'sans viande de chat'),
       (51, 'sans viande de rat'),
       (52, 'sans viande de serpent'),
       (53, 'sans viande de tortue'),
       (54, 'sans viande de chameau'),
       (55, 'sans viande de kangourou'),
       (56, 'sans viande de crocodile'),
       (57, 'sans viande de phoque'),
       (58, 'sans viande de baleine'),
       (59, 'sans viande de dauphin'),
       (60, 'sans viande de requin'),
       (61, 'torture de poulet');

INSERT INTO utilisateur (nom, prénom, email, fk_photo_profil, pseudo, date_naissance, date_creation_profil, description, fk_pays)
VALUES ('John', 'Doe', 'john.doe@example.com', 1, 'johndoe', '1990-01-01', '2022-01-01', 'Lorem ipsum dolor sit amet', 'US'),
       ('Jane', 'Smith', 'jane.smith@example.com', 1, 'janesmith', '1995-02-15', '2022-01-01', 'Lorem ipsum dolor sit amet', 'GB'),
       ('Mike', 'Johnson', 'mike.johnson@example.com', 1, 'mikejohnson', '1985-06-30', '2022-01-01', 'Lorem ipsum dolor sit amet', 'CA'),
       ('Sarah', 'Williams', 'sarah.williams@example.com', 1, 'sarahwilliams', '1992-11-20', '2022-01-01', 'Lorem ipsum dolor sit amet', 'AU'),
       ('David', 'Brown', 'david.brown@example.com', 1, 'davidbrown', '1998-09-10', '2022-01-01', 'Lorem ipsum dolor sit amet', 'FR');


INSERT INTO administrateur (id)
VALUES ('Admin', NULL, 'admin@example.com', 1, 'admin', '1990-01-01', '2022-01-01', 'Lorem ipsum dolor sit amet', 'US');

INSERT INTO image (url_source)
VALUES ('./img/photo_profile_default.png');

INSERT INTO allergene (nom)
VALUES ('Oeufs'),
       ('Lactose'),
       ('Mollusques'),
       ('Gluten'),
       ('Fruits à coque'),
       ('Sulfites'),
       ('Crustacés'),
       ('Arachide'),
       ('Sésame'),
       ('Lupin'),
       ('Poisson'),
       ('Soja'),
       ('Celeri'),
       ('Moutarde');

INSERT INTO image (url_source)
VALUES ('./img/Cuillere.png'),
       ('./img/Fourchette.png'),
       ('./img/Couteau.png'),
       ('./img/Assiette.png'),
       ('./img/Casserole.png'),
       ('./img/Poele.png'),
       ('./img/Pince.png'),
       ('./img/Eplucheuse.png'),
       ('./img/CuiseurVapeur.png');

INSERT INTO ustensile (nom, description, date_expiration, fk_image, fk_utilisateur, fk_administrateur, fk_pays, actif)
VALUES ('Cuillère', 'Fourchette sans dent pour manger les soupes et les desserts', '2022-12-01', 2, 1, 6, true),
       ('Fourchette', 'Fourchette avec 4 dents pour manger les plats', '2022-12-01', 3, 1, 6, true),
       ('Couteau', 'Couteau pour couper les aliments', '2022-12-01', 4, 1, 6, true),
       ('Assiette', 'Assiette pour mettre les aliments', '2022-12-01', 5, 1, 6, true),
       ('Casserole', 'Casserole pour faire cuire les aliments', '2022-12-01', 6, 1, 6, true),
       ('Poêle à frire', 'Poêle à frire pour faire cuire les aliments', '2022-12-01', 7, 1, 6, true),
       ('Pince', 'Pince pour attraper les aliments', '2022-12-01', 8, 1, 6, true),
       ('Éplucheuse', 'Éplucheuse pour éplucher les aliments', '2022-12-01', 9, 1, 6, true),
       ('Cuiseur vapeur', 'Cuiseur vapeur pour cuire les aliments à la vapeur', '2022-12-01', 10, 1, 6, true);

INSERT INTO image (url_source)
VALUES ('./img/pouletFermier.png'),
       ('./img/curry.png'),
       ('./img/riz.png'),
       ('./img/schnaps.png');

INSERT INTO ingredient (nom, description, date_expiration, fk_image, fk_utilisateur, fk_administrateur, fk_pays, actif)
VALUES ('Poulet', 'Poulet de ferme', '2022-12-01', 11, 1, 6, true),
       ('Curry', 'Curry de la marque X', '2022-12-01', 12, 1, 6, true),
       ('Riz', 'Riz de la marque Y', '2022-12-01', 13, 1, 6, true),
       ('Schnaps', 'Schnaps de la marque Z', '2022-12-01', 14, 1, 6, true);

INSERT INTO recette (nom, temps_preparation, temps_cuisson, temps_repos, date_creation, etape, fk_utilisateur, fk_video, fk_pays, actif, langue)
VALUES ('Poulet au curry', 30, 30, 0, '2022-12-01', '# Etape 1 : eplucher le poulet\n\n#Etape 2 : Plonger le poulet encore vivant dans du curry boullant\n\n# Etape 3 : Server vos invité avec du riz et du schnaps', 1, 0, 'FR')

INSERT INTO recette_tag (fk_recette, fk_tag)
VALUES (1, 61)

INSERT INTO recette_ingredient (fk_recette, fk_ingredient, quantite)
VALUES (1, 1, '1 poulet'),
       (1, 2, '5 cuillère à soupe'),
       (1, 3, '2 dose'),
       (1, 4, '1 verre');

INSERT INTO recette_ustensile (fk_recette, fk_ustensile)
VALUES (1, 1),
       (1, 2),
       (1, 3),
       (1, 4),
       (1, 5),
       (1, 6),
       (1, 7),
       (1, 8),
       (1, 9);



INSERT INTO publication (texte, date_publication, fk_parent, fk_utilisateur, fk_video, fk_recette)
VALUES ('Ma première recette ;D', '2022-12-01', NULL, 1, NULL, 1),
       ('Good job', '2022-12-01', 1, 2, NULL, NULL),
       ('Merci', '2022-12-01', 2, 1, NULL, NULL),
       ('Je vais essayer', '2022-12-01', 1, 3, NULL, NULL),
       ('Regarde mon resulta', '2022-12-01', 1, 5, NULL, NULL);


INSERT INTO image (url_source)
VALUES ('./img/maversion124324.png')

INSERT INTO publication_image (fk_publication, fk_image)
VALUES (5, 15);

INSERT INTO aime_publication_utilisateur (fk_publication, fk_utilisateur)
VALUES (1, 2),
       (1, 3),
       (1, 3),
       (1, 5),
       (5, 1),
       (5, 2),
       (5, 3),
       (5, 4);