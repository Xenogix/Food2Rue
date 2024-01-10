BEGIN TRANSACTION;

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

INSERT INTO tag (nom)
VALUES ('digest'),
        ('healthy'),
        ('fit'),
        ('fastfood maison'),
        ('facile à faire'),
        ('rapide à faire'),
        ('anniversaire'),
        ('birthday cake'),
        ('viande crue'),
        ('hallal'),
        ('casher'),
        ('végétarien'),
        ('végétalien'),
        ('sans gluten'),
        ('sans lactose'),
        ('sans oeuf'),
        ('sans sucre'),
        ('sans sel'),
        ('sans matière grasse'),
        ('sans viande'),
        ('sans poisson'),
        ('sans fruits de mer'),
        ('sans crustacés'),
        ('sans arachide'),
        ('sans soja'),
        ('sans sésame'),
        ('sans lupin'),
        ('sans moutarde'),
        ('sans céleri'),
        ('sans sulfites'),
        ('sans fruits à coque'),
        ('sans mollusques'),
        ('sans alcool'),
        ('sans porc'),
        ('sans boeuf'),
        ('sans poulet'),
        ('sans agneau'),
        ('sans veau'),
        ('sans lapin'),
        ('sans canard'),
        ('sans dinde'),
        ('sans oie'),
        ('sans cheval'),
        ('sans gibier'),
        ('sans escargot'),
        ('sans grenouille'),
        ('sans insecte'),
        ('sans viande de brousse'),
        ('sans viande de chien'),
        ('sans viande de chat'),
        ('sans viande de rat'),
        ('sans viande de serpent'),
        ('sans viande de tortue'),
        ('sans viande de chameau'),
        ('sans viande de kangourou'),
        ('sans viande de crocodile'),
        ('sans viande de phoque'),
        ('sans viande de baleine'),
        ('sans viande de dauphin'),
        ('sans viande de requin');

INSERT INTO media (url_source)
VALUES ('./img/photo_profile_default.png');

INSERT INTO image (id)
VALUES (1);

INSERT INTO utilisateur (nom, prénom, email, password, fk_photo_profil, pseudo, date_naissance, date_creation_profil, description, fk_pays)
VALUES ('John', 'Doe', 'john.doe@example.com', 'password', 1, 'johndoe', '1990-01-01', '2022-01-01', 'Lorem ipsum dolor sit amet', 65),
        ('Jane', 'Smith', 'jane.smith@example.com', 'password', 1, 'janesmith', '1995-02-15', '2022-01-01', 'Lorem ipsum dolor sit amet', 43),
        ('Mike', 'Johnson', 'mike.johnson@example.com', 'password', 1, 'mikejohnson', '1985-06-30', '2022-01-01', 'Lorem ipsum dolor sit amet', 87),
        ('Sarah', 'Williams', 'sarah.williams@example.com', 'password', 1, 'sarahwilliams', '1992-11-20', '2022-01-01', 'Lorem ipsum dolor sit amet', 100),
        ('David', 'Brown', 'david.brown@example.com', 'password', 1, 'davidbrown', '1998-09-10', '2022-01-01', 'Lorem ipsum dolor sit amet', 73);

INSERT INTO administrateur (id)
VALUES (1), (4);

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

INSERT INTO media (url_source)
VALUES ('./img/Cuillere.png'),
        ('./img/Fourchette.png'),
        ('./img/Couteau.png'),
        ('./img/Assiette.png'),
        ('./img/Casserole.png'),
        ('./img/Poele.png'),
        ('./img/Pince.png'),
        ('./img/Eplucheuse.png'),
        ('./img/CuiseurVapeur.png');
      
INSERT INTO image (id)
VALUES (2),
     (3),
     (4),
     (5),
     (6),
     (7),
     (8),
     (9),
     (10);

INSERT INTO ajoutable (nom, description, date_publication, fk_image, fk_utilisateur, fk_administrateur, est_valide)
VALUES ('Cuillere', 'Fourchette sans dent pour manger les soupes et les desserts', '2022-12-01', 2, 1, 1, true),
        ('Fourchette', 'Fourchette avec 4 dents pour manger les plats', '2022-12-01', 3, 2, 1, true),
        ('Couteau', 'Couteau pour couper les aliments', '2022-12-01', 4, 3, 4, true),
        ('Assiette', 'Assiette pour mettre les aliments', '2022-12-01', 5, 4, 4, true),
        ('Casserole', 'Casserole pour faire cuire les aliments', '2022-12-01', 6, 5, 1, true),
        ('Poêle à frire', 'Poêle à frire pour faire cuire les aliments', '2022-12-01', 7, 1, 1, true),
        ('Pince', 'Pince pour attraper les aliments', '2022-12-01', 8, 2, 1, true),
        ('Éplucheuse', 'Éplucheuse pour éplucher les aliments', '2022-12-01', 9, 3, 1, true),
        ('Cuiseur vapeur', 'Cuiseur vapeur pour cuire les aliments à la vapeur', '2022-12-01', 10, 4, 1, true);
      
INSERT INTO ustensile (id)
VALUES (1),
       (2),
       (3),
       (4),
       (5),
       (6),
       (7),
       (8),
       (9);

INSERT INTO media (url_source)
VALUES ('./img/pouletFermier.png'),
        ('./img/curry.png'),
        ('./img/riz.png'),
        ('./img/schnaps.png');
      
INSERT INTO image (id)
VALUES (11),
     (12),
     (13),
     (14);

INSERT INTO ajoutable (nom, description, date_publication, fk_image, fk_utilisateur, fk_administrateur, est_valide)
VALUES ('Poulet fermier', 'Poulet de ferme', '2022-12-01', 11, 2, 1, true),
        ('Curry', 'Curry de la marque X', '2022-12-01', 12, 2, NULL, false),
        ('Riz', 'Riz de la marque Y', '2022-12-01', 13, 2, NULL, false),
        ('Schnaps', 'Schnaps de la marque Z', '2022-12-01', 14, 2, NULL, true);
      
INSERT INTO ingredient (id)
VALUES (10),
       (11),
       (12),
       (13);
      
INSERT INTO media (url_source)
VALUES ('./img/pouletCurry.mp4');
      
INSERT INTO video (id)
VALUES (15);

INSERT INTO recette (nom, temps_preparation, temps_cuisson, temps_repos, date_creation, etape, fk_utilisateur, fk_video, fk_pays)
VALUES ('Poulet au curry', 30, 30, 0, '2022-12-01', '# Etape 1 : Débaler le poulet.\n\n#Etape 2 : Mettre de l eau à bouillir.\n\n# Etape 3 : Faire mijoter le poulet dans l eau avec le curry.\n\n# Etape 4 : Servir le poulet avec du schnaps.', 1, 15, 73);

INSERT INTO recette_tag (fk_recette, fk_tag)
VALUES (1, 7),
    (1, 6),
    (1, 34),
    (1, 26),
    (1, 46),
    (1, 56),
    (1, 60);

INSERT INTO recette_ingredient (fk_recette, fk_ingredient, quantite)
VALUES (1, 10, '1 poulet'),
       (1, 11, '5 cuillère à soupe'),
       (1, 12, '2 dose'),
       (1, 13, '1 verre');

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

INSERT INTO media (url_source)
VALUES ('./img/maversion124324.png');

INSERT INTO image (id)
VALUES (16);

INSERT INTO publication_image (fk_publication, fk_image)
VALUES (5, 16);

INSERT INTO aime_publication_utilisateur (fk_publication, fk_utilisateur)
VALUES (1, 2),
       (1, 3),
       (1, 5),
       (5, 1),
       (5, 2),
       (5, 3),
       (5, 4);

COMMIT TRANSACTION;