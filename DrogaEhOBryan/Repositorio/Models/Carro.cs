using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Repositorio.Models
{
    public class Carro
    {
        public String image { get; set; }

        public String title { get; set; }

        public int start_production { get; set; }

        public String Class { get; set; }

        public int Id { get; set; }

        public Marca Marca { get; set; }

        public int MarcaId { get; set; }
    }

    public class Marca
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Logo { get; set; }

        public ICollection<Carro> Carros { get; set; }
    }
}
