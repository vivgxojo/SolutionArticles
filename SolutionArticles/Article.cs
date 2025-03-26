using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionArticles
{
    public class Article : INotifyPropertyChanged // IMPLÉMENTER INotifyPropertyChanged POUR FAIRE LA MISE À JOUR
    {
        public string Code { get; set; }
        public string Nom_Article { get; set; }
        public decimal Prix_Achat { get; set; }
        public decimal Prix_Vente { get; set; }

        private int _quantiteStock;                 // ENCAPSULER L'ATTRIBUT QUI DOIT DÉCLENCHER LA MISE À JOUR
        public int Quantite_Stock {
            get=>_quantiteStock; 
            set
            {
                if (_quantiteStock != value)
                {
                    _quantiteStock = value;
                    OnPropertyChanged(nameof(Quantite_Stock));
                }
            }
        }
        public Section Section { get; set; }
        public Fournisseur Fournisseur { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;  // IMPLÉMENTER L'INTERFACE
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); //DÉCLENCHER L'ÉVÈNEMENT
        }

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
