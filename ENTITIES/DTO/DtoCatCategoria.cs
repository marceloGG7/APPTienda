using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.DTO
{
    public record DtoCatCategoria
    {
        public int IdCategoria { get; set; } = int.MinValue;
        public string Categoria { get; set;} = string.Empty;
    }
}
