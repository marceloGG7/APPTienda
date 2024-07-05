using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.DTO
{
    public record DtoConsulInventario
    {
        public int SKU { get; set; } = int.MinValue;
        public string Descrip { get; set; } = string.Empty;
        public decimal Pventa { get; set; } = decimal.MinValue;
        public int Existencias { get; set; } = int.MinValue;
        public string Categoria { get; set; } = string.Empty;
        public decimal IVA { get; set; } = decimal.MinValue;
        public string FecRegistro { get; set; } = string.Empty;
        public string Estatus { get; set; } = string.Empty;
    }
}
