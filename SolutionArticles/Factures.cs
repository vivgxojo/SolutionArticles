using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionArticles
{
    public class Factures
    {
        public int No_Facture { get; set; }
        public List<Article> Liste_Articles { get; set; } = new List<Article>();
        public decimal Prix_Total { get; set; }
        public DateTime Date { get; set; }
        public Clients Client { get; set; }

        public decimal CalculerTaxes()
        {
            return Prix_Total * 0.15m; // Exemple de taxe 15%
        }

        public decimal CalculerTotal()
        {
            return Prix_Total + CalculerTaxes();
        }
    }
}
