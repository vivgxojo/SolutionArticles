using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionArticles
{
    public class Article
    {
        public string Code { get; set; }
        public string Nom_Article { get; set; }
        public decimal Prix_Achat { get; set; }
        public decimal Prix_Vente { get; set; }
        public int Quantite_Stock { get; set; }
        public Section Section { get; set; }
        public Fournisseur Fournisseur { get; set; }

        public void Approvisionner(int quantite)
        {
            Quantite_Stock += quantite;
        }

        public void Vendre(int quantite)
        {
            if (Quantite_Stock >= quantite)
                Quantite_Stock -= quantite;
        }

        public void Deplacer(Section nouvelleSection)
        {
            //Supprimer l'article de l'ancienne section
            Section.List_Articles.Remove(this);
            //Ajouter l'article dans la liste de la nouvelle section
            nouvelleSection.AjouterArticle(this);
            //Modifier la section associé à l'article
            Section = nouvelleSection;
        }
    }
}
