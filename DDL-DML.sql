--DROP DATABASE IF EXISTS food2rue;
--CREATE DATABASE food2rue;
--DROP schema IF EXISTS food2rue CASCADE;
--CREATE SCHEMA food2rue;

DROP TABLE IF EXISTS recette_ustensile;
DROP TABLE IF EXISTS ingredient_allergene;
DROP TABLE IF EXISTS allergene;
DROP TABLE IF EXISTS recette_ingredient;
DROP TABLE IF EXISTS ustensile;
DROP TABLE IF EXISTS ingredient;
DROP TABLE IF EXISTS ajoutable;
DROP TABLE IF EXISTS recette_tag;
DROP TABLE IF EXISTS publication_tag;
DROP TABLE IF EXISTS tag;
DROP TABLE IF EXISTS recette_image;
DROP TABLE IF EXISTS note;
DROP TABLE IF EXISTS aime_publication_utilisateur;
DROP TABLE IF EXISTS publication_image;
DROP TABLE IF EXISTS publication;
DROP TABLE IF EXISTS recette;
DROP TABLE IF EXISTS administrateur;
DROP TABLE IF EXISTS utilisateur;
DROP TABLE IF EXISTS video;
DROP TABLE IF EXISTS image;
DROP TABLE IF EXISTS media;
DROP TABLE IF EXISTS pays;

CREATE TABLE  pays (
  sigle VARCHAR(2),
  nom VARCHAR(255),
  PRIMARY KEY (sigle)
);

CREATE TABLE  media (
  id SERIAL,
  url_source VARCHAR(255) NOT NULL,
  PRIMARY KEY (id)
);

CREATE TABLE  image (
  id SERIAL,
  PRIMARY KEY (id),
  FOREIGN KEY (id) REFERENCES media(id)
    ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE  video (
  id SERIAL,
  PRIMARY KEY (id),
  FOREIGN KEY (id) REFERENCES media(id)
    ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE  utilisateur (
  id SERIAL,
  nom VARCHAR(255) NOT NULL,
  prenom VARCHAR(255) NOT NULL,
  email VARCHAR(255) NOT NULL UNIQUE,
  fk_photo_profil INT DEFAULT 1 NOT NULL,
  pseudo VARCHAR(255) NOT NULL,
  password VARCHAR(72) NOT NULL,
  date_naissance DATE NOT NULL,
  date_creation_profil DATE NOT NULL,
  description VARCHAR(255),
  fk_pays VARCHAR NOT NULL,
  PRIMARY KEY (id),
  FOREIGN KEY (fk_pays) REFERENCES pays(sigle) ON DELETE CASCADE ON UPDATE CASCADE,
  FOREIGN KEY (fk_photo_profil) REFERENCES image(id) ON DELETE SET DEFAULT ON UPDATE CASCADE,
  CONSTRAINT check_date CHECK (date_naissance < date_creation_profil + INTERVAL '13 years')
);

CREATE TABLE  administrateur (
  id SERIAL,
  PRIMARY KEY (id),
  FOREIGN KEY (id) REFERENCES utilisateur(id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE  recette (
  id SERIAL,
  nom VARCHAR(255) NOT NULL,
  temps_preparation INT NOT NULL,
  temps_cuisson INT,
  temps_repos INT,
  date_creation DATE NOT NULL,
  etape VARCHAR(1023) NOT NULL,
  fk_utilisateur INT NOT NULL,
  fk_video INT,
  fk_pays VARCHAR,
  PRIMARY KEY (id),
  FOREIGN KEY (fk_utilisateur) REFERENCES utilisateur(id) ON DELETE CASCADE ON UPDATE CASCADE,
  FOREIGN KEY (fk_video) REFERENCES video(id) ON DELETE SET NULL ON UPDATE CASCADE,
  FOREIGN KEY (fk_pays) REFERENCES pays(sigle) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE  publication (
  id SERIAL,
  texte VARCHAR(255) NOT NULL,
  date_publication DATE NOT NULL,
  fk_parent INT,
  fk_utilisateur INT NOT NULL,
  fk_video INT,
  fk_recette INT,
  PRIMARY KEY (id),
  FOREIGN KEY (fk_parent) REFERENCES publication(id) ON DELETE CASCADE ON UPDATE CASCADE,
  FOREIGN KEY (fk_utilisateur) REFERENCES utilisateur(id) ON DELETE CASCADE ON UPDATE CASCADE,
  FOREIGN KEY (fk_video) REFERENCES video(id) ON DELETE SET NULL ON UPDATE CASCADE,
  FOREIGN KEY (fk_recette) REFERENCES recette(id) ON DELETE SET NULL ON UPDATE CASCADE
);

CREATE TABLE  publication_image (
  fk_publication INT,
  fk_image INT,
  PRIMARY KEY (fk_publication, fk_image),
  FOREIGN KEY (fk_publication) REFERENCES publication(id) ON DELETE CASCADE ON UPDATE CASCADE,
  FOREIGN KEY (fk_image) REFERENCES image(id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE  aime_publication_utilisateur (
  fk_publication INT,
  fk_utilisateur INT,
  PRIMARY KEY (fk_publication, fk_utilisateur),
  FOREIGN KEY (fk_publication) REFERENCES publication(id) ON DELETE CASCADE ON UPDATE CASCADE,
  FOREIGN KEY (fk_utilisateur) REFERENCES utilisateur(id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE  note (
  fk_utilisateur INT,
  fk_recette INT,
  note INT NOT NULL,
  PRIMARY KEY (fk_utilisateur, fk_publication),
  FOREIGN KEY (fk_utilisateur) REFERENCES utilisateur(id) ON DELETE CASCADE ON UPDATE CASCADE,
  FOREIGN KEY (fk_publication) REFERENCES publication(id) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT check_note CHECK (note >= 0 AND note <= 5)
);

CREATE TABLE  recette_image (
  fk_recette INT,
  fk_image INT,
  PRIMARY KEY (fk_recette, fk_image),
  FOREIGN KEY (fk_recette) REFERENCES recette(id) ON DELETE CASCADE ON UPDATE CASCADE,
  FOREIGN KEY (fk_image) REFERENCES image(id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE  tag (
  id SERIAL,
  nom VARCHAR(255) NOT NULL UNIQUE,
  PRIMARY KEY (id)
);

CREATE TABLE  publication_tag (
  fk_publication INT,
  fk_tag INT,
  PRIMARY KEY (fk_publication, fk_tag),
  FOREIGN KEY (fk_publication) REFERENCES publication(id) ON DELETE CASCADE ON UPDATE CASCADE,
  FOREIGN KEY (fk_tag) REFERENCES tag(id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE  recette_tag (
  fk_recette INT,
  fk_tag INT,
  PRIMARY KEY (fk_recette, fk_tag),
  FOREIGN KEY (fk_recette) REFERENCES recette(id) ON DELETE CASCADE ON UPDATE CASCADE,
  FOREIGN KEY (fk_tag) REFERENCES tag(id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE  ajoutable (
  id SERIAL,
  nom VARCHAR(255) NOT NULL UNIQUE,
  description VARCHAR(255),
  date_publication DATE NOT NULL,
  fk_image INT NOT NULL UNIQUE,
  fk_utilisateur INT NOT NULL,
  fk_administrateur INT,
  est_valide BOOLEAN,
  PRIMARY KEY (id),
  FOREIGN KEY (fk_administrateur) REFERENCES administrateur(id) ON DELETE SET NULL ON UPDATE CASCADE,
  FOREIGN KEY (fk_utilisateur) REFERENCES utilisateur(id) ON DELETE CASCADE ON UPDATE CASCADE,
  FOREIGN KEY (fk_image) REFERENCES image(id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE  ingredient (
  id SERIAL,
  PRIMARY KEY (id),
  FOREIGN KEY (id) REFERENCES ajoutable(id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE  recette_ingredient (
  fk_recette INT,
  fk_ingredient INT,
  quantite VARCHAR(255) NOT NULL,
  PRIMARY KEY (fk_recette, fk_ingredient),
  FOREIGN KEY (fk_recette) REFERENCES recette(id) ON DELETE CASCADE ON UPDATE CASCADE,
  FOREIGN KEY (fk_ingredient) REFERENCES ingredient(id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE  allergene (
  nom VARCHAR(255),
  PRIMARY KEY (nom)
);

CREATE TABLE  ingredient_allergene (
  fk_ingredient INT,
  fk_allergene VARCHAR(255),
  PRIMARY KEY (fk_ingredient, fk_allergene),
  FOREIGN KEY (fk_ingredient) REFERENCES ingredient(id) ON DELETE CASCADE ON UPDATE CASCADE,
  FOREIGN KEY (fk_allergene) REFERENCES allergene(nom) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE  ustensile (
  id SERIAL,
  PRIMARY KEY (id),
  FOREIGN KEY (id) REFERENCES ajoutable(id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE  recette_ustensile (
  fk_recette INT,
  fk_ustensile INT,
  PRIMARY KEY (fk_recette, fk_ustensile),
  FOREIGN KEY (fk_recette) REFERENCES recette(id) ON DELETE CASCADE ON UPDATE CASCADE,
  FOREIGN KEY (fk_ustensile) REFERENCES ustensile(id) ON DELETE CASCADE ON UPDATE CASCADE
);

-- Procedures and Triggers

CREATE OR REPLACE FUNCTION check_heritage_media()  
RETURNS TRIGGER
LANGUAGE plpgsql
AS $$
BEGIN
  IF (NEW.id NOT IN ((SELECT id FROM image) UNION (SELECT id FROM video))) THEN
    RAISE EXCEPTION 'Media must be in image or video table';
  END IF;
  RETURN NEW;
END;
$$;

DROP TRIGGER IF EXISTS trigger_heritage_media ON media CASCADE;
CREATE CONSTRAINT TRIGGER trigger_heritage_media
AFTER INSERT OR UPDATE ON media
DEFERRABLE INITIALLY DEFERRED
FOR EACH ROW
EXECUTE FUNCTION check_heritage_media();

CREATE OR REPLACE FUNCTION check_heritage_image()  
RETURNS TRIGGER
LANGUAGE plpgsql
AS $$
BEGIN
  IF (NEW.id NOT IN (SELECT id FROM media)) THEN
    RAISE EXCEPTION 'Image must be in media table';
  END IF;
  RETURN NEW;
END;
$$;

DROP TRIGGER IF EXISTS trigger_heritage_image ON image CASCADE;
CREATE CONSTRAINT TRIGGER trigger_heritage_image
AFTER INSERT OR UPDATE ON image
DEFERRABLE INITIALLY DEFERRED
FOR EACH ROW
EXECUTE FUNCTION check_heritage_image();

CREATE OR REPLACE FUNCTION check_heritage_video()  
RETURNS TRIGGER
LANGUAGE plpgsql
AS $$
BEGIN
  IF (NEW.id NOT IN (SELECT id FROM media)) THEN
    RAISE EXCEPTION 'Video must be in media table';
  END IF;
  RETURN NEW;
END;
$$;

DROP TRIGGER IF EXISTS trigger_heritage_video ON video CASCADE;
CREATE CONSTRAINT TRIGGER trigger_heritage_video
AFTER INSERT OR UPDATE ON video
DEFERRABLE INITIALLY DEFERRED
FOR EACH ROW
EXECUTE FUNCTION check_heritage_video();

CREATE OR REPLACE FUNCTION check_heritage_utilisateur()  
RETURNS TRIGGER
LANGUAGE plpgsql
AS $$
BEGIN
  IF (NEW.id NOT IN (SELECT id FROM utilisateur)) THEN
    RAISE EXCEPTION 'Administrateur must be in utilisateur table';
  END IF;
  RETURN NEW;
END;
$$;

DROP TRIGGER IF EXISTS trigger_heritage_administrateur ON administrateur CASCADE;
CREATE CONSTRAINT TRIGGER trigger_heritage_administrateur
AFTER INSERT OR UPDATE ON administrateur
DEFERRABLE INITIALLY DEFERRED
FOR EACH ROW
EXECUTE FUNCTION check_heritage_utilisateur();

CREATE OR REPLACE FUNCTION check_date_recette_date_creation() RETURNS TRIGGER AS $$
DECLARE
  user_date_creation date;
BEGIN
  SELECT date_creation_profil INTO user_date_creation FROM utilisateur WHERE id = NEW.fk_utilisateur;
  IF (NEW.date_creation < user_date_creation) THEN
    RAISE EXCEPTION 'Administrateur must be in utilisateur table';
  END IF;
  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

DROP TRIGGER IF EXISTS check_date_recette_date_creation ON recette CASCADE;
CREATE CONSTRAINT TRIGGER check_date_recette_date_creation
AFTER INSERT OR UPDATE ON recette
DEFERRABLE INITIALLY DEFERRED
FOR EACH ROW
EXECUTE FUNCTION check_date_recette_date_creation();

CREATE OR REPLACE FUNCTION check_date_publication() RETURNS TRIGGER AS $$
DECLARE
  user_date_creation date;
BEGIN
  SELECT date_creation_profil INTO user_date_creation FROM utilisateur WHERE id = NEW.fk_utilisateur;
  IF (NEW.date_publication < user_date_creation) THEN
    RAISE EXCEPTION 'Administrateur must be in utilisateur table';
  END IF;
  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

DROP TRIGGER IF EXISTS check_date_publication ON publication CASCADE;
CREATE CONSTRAINT TRIGGER check_date_publication_date_publication
AFTER INSERT OR UPDATE ON publication
DEFERRABLE INITIALLY DEFERRED
FOR EACH ROW
EXECUTE FUNCTION check_date_publication();

DROP TRIGGER IF EXISTS check_date_publication ON ajoutable CASCADE;
CREATE CONSTRAINT TRIGGER check_date_ajoutable_date_publication
AFTER INSERT OR UPDATE ON ajoutable
DEFERRABLE INITIALLY DEFERRED
FOR EACH ROW
EXECUTE FUNCTION check_date_publication();

CREATE OR REPLACE FUNCTION check_heritage_ajoutable()  
RETURNS TRIGGER
LANGUAGE plpgsql
AS $$
BEGIN
  IF ((NEW.id NOT IN ((SELECT id FROM ingredient) UNION (SELECT id FROM ustensile)))) THEN
    RAISE EXCEPTION 'Ajoutable must be in ingredient or ustensile table';
  END IF;
  RETURN NEW;
END;
$$;

DROP TRIGGER IF EXISTS trigger_heritage_ajoutable ON ajoutable CASCADE;
CREATE CONSTRAINT TRIGGER trigger_heritage_ajoutable
AFTER INSERT OR UPDATE ON ajoutable
DEFERRABLE INITIALLY DEFERRED
FOR EACH ROW
EXECUTE FUNCTION check_heritage_ajoutable();

CREATE OR REPLACE FUNCTION check_heritage_ingredient()  
RETURNS TRIGGER
LANGUAGE plpgsql
AS $$
BEGIN
  IF (NEW.id NOT IN (SELECT id FROM ajoutable)) THEN
    RAISE EXCEPTION 'Ingredient must be in ajoutable table';
  END IF;
  RETURN NEW;
END;
$$;

DROP TRIGGER IF EXISTS trigger_heritage_ingredient ON ingredient CASCADE;
CREATE CONSTRAINT TRIGGER trigger_heritage_ingredient
AFTER INSERT OR UPDATE ON ingredient
DEFERRABLE INITIALLY DEFERRED
FOR EACH ROW
EXECUTE FUNCTION check_heritage_ingredient();

CREATE OR REPLACE FUNCTION check_heritage_ustensile()  
RETURNS TRIGGER
LANGUAGE plpgsql
AS $$
BEGIN
  IF (NEW.id NOT IN (SELECT id FROM ustensile)) THEN
    RAISE EXCEPTION 'Ustensile must be in ajoutable table';
  END IF;
  RETURN NEW;
END;
$$;

DROP TRIGGER IF EXISTS trigger_heritage_ustensile ON ustensile CASCADE;
CREATE CONSTRAINT TRIGGER trigger_heritage_ustensile
AFTER INSERT OR UPDATE ON ustensile
DEFERRABLE INITIALLY DEFERRED
FOR EACH ROW
EXECUTE FUNCTION check_heritage_ustensile();


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

INSERT INTO media (id, url_source)
VALUES (1, './img/photo_profile_default.png');

INSERT INTO image (id)
VALUES (1);

INSERT INTO utilisateur (id, nom, prenom, email, password, fk_photo_profil, pseudo, date_naissance, date_creation_profil, description, fk_pays)
VALUES (1, 'John', 'Doe', 'john.doe@example.com', 'password', 1, 'johndoe', '1990-01-01', '2022-01-01', 'Lorem ipsum dolor sit amet', 'US'),
        (2, 'Jane', 'Smith', 'jane.smith@example.com', 'password', 1, 'janesmith', '1995-02-15', '2022-01-01', 'Lorem ipsum dolor sit amet', 'FR'),
        (3, 'Mike', 'Johnson', 'mike.johnson@example.com', 'password', 1, 'mikejohnson', '1985-06-30', '2022-01-01', 'Lorem ipsum dolor sit amet', 'FR'),
        (4, 'Sarah', 'Williams', 'sarah.williams@example.com', 'password', 1, 'sarahwilliams', '1992-11-20', '2022-01-01', 'Lorem ipsum dolor sit amet', 'FR'),
        (5, 'David', 'Brown', 'david.brown@example.com', 'password', 1, 'davidbrown', '1998-09-10', '2022-01-01', 'Lorem ipsum dolor sit amet', 'FR');

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

INSERT INTO media (id, url_source)
VALUES (2, './img/Cuillere.png'),
        (3, './img/Fourchette.png'),
        (4, './img/Couteau.png'),
        (5, './img/Assiette.png'),
        (6, './img/Casserole.png'),
        (7, './img/Poele.png'),
        (8, './img/Pince.png'),
        (9, './img/Eplucheuse.png'),
        (10, './img/CuiseurVapeur.png');
      
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

INSERT INTO ajoutable (id, nom, description, date_publication, fk_image, fk_utilisateur, fk_administrateur, est_valide)
VALUES (1, 'Cuillere', 'Fourchette sans dent pour manger les soupes et les desserts', '2022-12-01', 2, 1, 1, true),
        (2, 'Fourchette', 'Fourchette avec 4 dents pour manger les plats', '2022-12-01', 3, 2, 1, true),
        (3, 'Couteau', 'Couteau pour couper les aliments', '2022-12-01', 4, 3, 4, true),
        (4, 'Assiette', 'Assiette pour mettre les aliments', '2022-12-01', 5, 4, 4, true),
        (5, 'Casserole', 'Casserole pour faire cuire les aliments', '2022-12-01', 6, 5, 1, true),
        (6, 'Poêle à frire', 'Poêle à frire pour faire cuire les aliments', '2022-12-01', 7, 1, 1, true),
        (7, 'Pince', 'Pince pour attraper les aliments', '2022-12-01', 8, 2, 1, true),
        (8, 'Éplucheuse', 'Éplucheuse pour éplucher les aliments', '2022-12-01', 9, 3, 1, true),
        (9, 'Cuiseur vapeur', 'Cuiseur vapeur pour cuire les aliments à la vapeur', '2022-12-01', 10, 4, 1, true);
      
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

INSERT INTO media (id, url_source)
VALUES (11, './img/pouletFermier.png'),
        (12, './img/curry.png'),
        (13, './img/riz.png'),
        (14, './img/schnaps.png');
      
INSERT INTO image (id)
VALUES (11),
     (12),
     (13),
     (14);

INSERT INTO ajoutable (id, nom, description, date_publication, fk_image, fk_utilisateur, fk_administrateur, est_valide)
VALUES (10, 'Poulet fermier', 'Poulet de ferme', '2022-12-01', 11, 2, 1, true),
        (11, 'Curry', 'Curry de la marque X', '2022-12-01', 12, 2, NULL, false),
        (12, 'Riz', 'Riz de la marque Y', '2022-12-01', 13, 2, NULL, false),
        (13, 'Schnaps', 'Schnaps de la marque Z', '2022-12-01', 14, 2, NULL, true);
      
INSERT INTO ingredient (id)
VALUES (10),
       (11),
       (12),
       (13);
      
INSERT INTO media (id, url_source)
VALUES (15, './img/pouletCurry.mp4');
      
INSERT INTO video (id)
VALUES (15);

INSERT INTO recette (id, nom, temps_preparation, temps_cuisson, temps_repos, date_creation, etape, fk_utilisateur, fk_video, fk_pays)
VALUES (1, 'Poulet au curry', 30, 30, 0, '2022-12-01', '# Etape 1 : Débaler le poulet.\n\n#Etape 2 : Mettre de l eau à bouillir.\n\n# Etape 3 : Faire mijoter le poulet dans l eau avec le curry.\n\n# Etape 4 : Servir le poulet avec du schnaps.', 1, 15, 'JP');

INSERT INTO recette_tag (fk_recette, fk_tag)
VALUES (1, 7),
    (1, 6),
    (1, 34),
    (1, 26),
    (1, 46),
    (1, 56),
    (1, 60);

INSERT INTO recette_ingredient (fk_recette, fk_ingredient, quantite)
VALUES (1, 10, '1 piece'),
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

INSERT INTO publication (id, texte, date_publication, fk_parent, fk_utilisateur, fk_video, fk_recette)
VALUES (1, 'Ma première recette ;D', '2022-12-01', NULL, 1, NULL, 1),
       (2, 'Good job', '2022-12-01', 1, 2, NULL, NULL),
       (3, 'Merci', '2022-12-01', 2, 1, NULL, NULL),
       (4, 'Je vais essayer', '2022-12-01', 1, 3, NULL, NULL),
       (5, 'Regarde mon resultat', '2022-12-01', 1, 5, NULL, NULL);

INSERT INTO media (id, url_source)
VALUES (16, './img/maversion124324.png');

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