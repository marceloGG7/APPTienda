using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.ENTITY
{
    public record INVENTARIO
    {
        public string Descrip { get; set; } = string.Empty;
        public decimal PVenta { get; set; } = decimal.MinValue;
        public int Existencias { get; set; } = int.MinValue;
        public int IdCategoria  { get; set;} = int.MinValue;
        public decimal IVA {  get; set; } = decimal.MinValue;
        public string? SKU {  get; set; }
        public string? CB { get; set; }

    }
}
