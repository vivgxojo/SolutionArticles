using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionArticles
{
    public class Section
    {
        public int No { get; set; }
        public string Nom_Section { get; set; }
        public ObservableCollection<Article> List_Articles { get; set; } = new ObservableCollection<Article>();

        public void AjouterArticle(Article article)
        {
            List_Articles.Add(article);
            article.Section = this;
        }

        public void FaireInventaire(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine($"Inventaire de la section {Nom_Section} :");
                foreach (var article in List_Articles)
                {
                    writer.WriteLine($"{article.Nom_Article}: {article.Quantite_Stock} en stock");
                }
            }
        }
    }
}
