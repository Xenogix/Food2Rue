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
  prénom VARCHAR(255) NOT NULL,
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
  fk_publication INT,
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
