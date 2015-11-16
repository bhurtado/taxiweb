using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxWeb.Model
{
    public class Importador
    {
        public int id { get; set; }
        public string Comprador { get; set; }
        public string descricao { get; set; }
        public decimal Preco { get; set; }
        public int quantidade { get; set; }
        public string endereco { get; set; }
        public string fornecedor { get; set; }

        public string chave { get; set; }
    }
}