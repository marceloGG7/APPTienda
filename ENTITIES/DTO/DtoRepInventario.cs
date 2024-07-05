using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.DTO
{
    public record DtoRepInventario
    {
        public string SKU { get; set; } = string.Empty;
        public string CB { get; set; } = string.Empty;
        public string Descrip { get; set; } = string.Empty;
        public decimal PVenta { get; set; } = decimal.MinValue;
        public int Existencias { get; set; } = int.MinValue;
        public string Categoria { get; set; } = string.Empty;
        public decimal IVA { get; set; } = decimal.MinValue;
        public string Estatus { get; set; } = string.Empty;
        public string FecAlta { get; set; }= string.Empty;
    }
}
