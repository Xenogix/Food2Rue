BDR – Groupe 5 27/09/

Pollien Lionel – Menétrey Arthur – Mikami Johan

# Cahier des charges – BDR – Groupe 5

## Buts

Le but de ce projet est de réaliser un réseau social au sujet de la cuisine. Ce réseau social devra être
accessible depuis une page web. Il permettra aux utilisateurs d’échanger et de partager leurs
nouvelles réalisation culinaires et recettes de cuisines.

## Fonctionnalités

Les utilisateurs devront pouvoir :

- Créer des publications textuelles pouvant contenir des images
- Créer des recettes qui pourront ensuite être sauvegardée et utilisées dans des publications
    ou commentaires
- Commenter sous la publication sous forme d’un message textuel
- Afficher les commentaires à un poste
- Utiliser un bouton « J’aime » sur des publications ou commentaires
- Noter des recettes de cuisines
- S’abonner à d’autres utilisateurs
- Consulter un fil contenant les publications de ses abonnements ou globales
- Consulter un profile personnel comportant une image de profile, description et liste de ses
    publications ou recettes

## Technologies

- C# .Net Framework 7.
- Framework Blazor Web Assembly + ASP.NET

## Données et sources de données

## Vue globale

## Données Source

```
Ingrédients de cuisine
```
```
https://www.bbc.co.uk/food/ingredients
https://naehrwertdaten.ch/
https://foodb.ca
Ustensiles Création manuelle
Utilisateurs
```
```
Entrées par les utilisateurs
```
```
Recettes
Publications
Commentaires
Abonnements
```

BDR – Groupe 5 27/09/

Pollien Lionel – Menétrey Arthur – Mikami Johan

## Spécifications

Par défaut, la quantification des champs est d’une seule valeur non null si cela n’est pas explicité.

_Ingrédients de cuisine_

- Nom de l’ingrédient
- Image (pas nécessaire mais préférable)

_Ustensiles_

- Nom de l’ustensile
- Image (facultatif)

_Utilisateurs_

- Nom
- Prénom
- Email
- Pseudo
- Date de naissance
- Localisation

_Recettes_

- Nom
- Étapes [1..*]
- Tags [1..*] ̈
- Ingrédients [1..*]
- Ustensiles [0..*]
- Temps de préparation
- Temps de cuisson [0..1]
- Temps de repos [0..1]
- Date de création
- Notes mise par un utilisateurs [0..*]
- Nombre de vote

_Publications_

- Texte
- « Like » mis par un utilisateur [0..*]
- Date de création
- Image [0..*]

_Commentaire_

- Texte
- « Like » mis par un utilisateur [0..*]
- Date de création
- Image [0..*]


