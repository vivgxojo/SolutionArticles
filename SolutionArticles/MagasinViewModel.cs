using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SolutionArticles
{
    public class MagasinViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Section> _sections;
        private int _indexSectionActuelle;
        private ObservableCollection<Article> _articlesAffiches; 
        private Article _articleSelectionne;
        private ObservableCollection<Article> _panier;

        public ObservableCollection<Article> ArticlesAffiches
        {
            get => _articlesAffiches;
            set { _articlesAffiches = value; 
                OnPropertyChanged(nameof(ArticlesAffiches));
                OnPropertyChanged(nameof(ArticleSelectionne));
            }
        }

        public Article ArticleSelectionne
        {
            get => _articleSelectionne;
            set { _articleSelectionne = value; OnPropertyChanged(nameof(ArticleSelectionne)); }
        }

        public ObservableCollection<Article> Panier
        {
            get => _panier;
            set { _panier = value; OnPropertyChanged(nameof(Panier)); }
        }

        public ICommand SectionPrecedenteCommand { get; }
        public ICommand SectionSuivanteCommand { get; }
        public ICommand AjouterAuPanierCommand { get; }
        public ICommand AfficherFactureCommand { get; }

        public MagasinViewModel()
        {
            // Initialisation des sections avec des articles
            _sections = new ObservableCollection<Section>
            {
                new Section { No = 1, Nom_Section = "Électronique", List_Articles = new ObservableCollection<Article>
                    {
                        new Article { Code = "A1", Nom_Article = "Ordinateur", Prix_Vente = 1200m, Quantite_Stock = 5,
                                      Section = null, Fournisseur = new Fournisseur { Nom_Entreprise = "TechCorp", Contact = "123-456-7890" }},
                        new Article { Code = "A2", Nom_Article = "Smartphone", Prix_Vente = 800m, Quantite_Stock = 3,
                                      Section = null, Fournisseur = new Fournisseur { Nom_Entreprise = "PhoneWorld", Contact = "987-654-3210" }}
                    }
                },
                new Section { No = 2, Nom_Section = "Maison", List_Articles = new ObservableCollection<Article>
                    {
                        new Article { Code = "B1", Nom_Article = "Aspirateur", Prix_Vente = 250m, Quantite_Stock = 2,
                                      Section = null, Fournisseur = new Fournisseur { Nom_Entreprise = "CleanPro", Contact = "555-333-1111" }}
                    }
                }
            };

            // Lier les articles aux sections
            foreach (var section in _sections)
            {
                foreach (var article in section.List_Articles)
                    article.Section = section;
            }

            // Affichage des articles de la première section
            _indexSectionActuelle = 0;
            ArticlesAffiches = _sections[_indexSectionActuelle].List_Articles;
            Panier = new ObservableCollection<Article>();

            // Initialisation des commandes
            SectionPrecedenteCommand = new RelayCommand(SectionPrecedente, () => _indexSectionActuelle > 0);
            SectionSuivanteCommand = new RelayCommand(SectionSuivante, () => _indexSectionActuelle < _sections.Count - 1);
            AjouterAuPanierCommand = new RelayCommand(AjouterAuPanier, () => ArticleSelectionne != null);
            AfficherFactureCommand = new RelayCommand(AfficherFacture, () => Panier.Count > 0);
        }

        private void SectionPrecedente()
        {
            if (_indexSectionActuelle > 0)
            {
                _indexSectionActuelle--;
                ArticlesAffiches = _sections[_indexSectionActuelle].List_Articles;
            }
        }

        private void SectionSuivante()
        {
            if (_indexSectionActuelle < _sections.Count - 1)
            {
                _indexSectionActuelle++;
                ArticlesAffiches = _sections[_indexSectionActuelle].List_Articles;
            }
        }

        private void AjouterAuPanier()
        {
            if (ArticleSelectionne != null && ArticleSelectionne.Quantite_Stock > 0)
            {
                ArticleSelectionne.Quantite_Stock--;
                Panier.Add(new Article
                {
                    Code = ArticleSelectionne.Code,
                    Nom_Article = ArticleSelectionne.Nom_Article,
                    Prix_Vente = ArticleSelectionne.Prix_Vente,
                    Quantite_Stock = 1
                });

                //OnPropertyChanged(nameof(ArticlesAffiches));
               
                if (ArticleSelectionne.Quantite_Stock == 0)
                {
                    MessageBox.Show($"Stock épuisé pour {ArticleSelectionne.Nom_Article}.\n" +
                                    $"Contactez le fournisseur : {ArticleSelectionne.Fournisseur.Nom_Entreprise} - {ArticleSelectionne.Fournisseur.Contact}",
                                    "Alerte Stock", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                //FORCER LA MISE À JOUR : ceci est un patch, pas l'idéal.
                //LA BONNE FAÇON EST en modifiant Article.
                //ArticlesAffiches = new ObservableCollection<Article>();
                //ArticlesAffiches = _sections[_indexSectionActuelle].List_Articles;
            }
        }
        
        private void AfficherFacture() 
        {
            // CODE CORRIGÉ POUR UTILISER FACTURES:

            // Création de la facture avec les articles du panier
            Factures facture = new Factures
            {
                Liste_Articles = Panier.ToList(),
                Prix_Total = Panier.Sum(a => a.Prix_Vente),
                Date = DateTime.Now // Ajout de la date actuelle
            };

            // Calcul des taxes et du total
            decimal taxes = facture.CalculerTaxes();
            decimal totalAvecTaxes = facture.CalculerTotal();

            // Construction du message
            string message = "Facture :\n";
            foreach (var article in facture.Liste_Articles)
            {
                message += $"{article.Nom_Article} - {article.Prix_Vente:C}\n";
            }
            message += $"\nTotal: {facture.Prix_Total:C}\nTaxes: {taxes:C}\nTotal à payer: {totalAvecTaxes:C}";

            // Affichage de la facture
            MessageBox.Show(message, "Facture", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}