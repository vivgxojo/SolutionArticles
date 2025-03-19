using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionArticles
{
    public class Fournisseur
    {
        public string Nom_Entreprise { get; set; }
        public string Contact { get; set; }

        public string Appeler()
        {
            return $"Contacter {Nom_Entreprise} au {Contact}";
        }
    }
}
