﻿using FDRWebsite.Shared.Abstraction;

namespace FDRWebsite.Shared.Models
{
    public class Publication : IIdentifiable<int>
    {
        public int ID { get; set; }

        public required String Texte { get; set; }

        public required DateOnly Date_Publication { get; set; }

        public Publication? Parent { get; set; }

        public required User Utilisateur { get; set; }

        public Video? Video { get; set; }

        public Recette? Recette { get; set; }
    }
}