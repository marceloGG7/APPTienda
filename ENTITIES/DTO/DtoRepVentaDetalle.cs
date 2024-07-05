using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.DTO
{
    public record DtoRepVentaDetalle
    {
        public string Articulo { get; set; } = string.Empty;
        public int Cantidad {  get; set; }  = 0;
        public decimal Pventa { get; set; } = 0;
        public decimal IVA { get; set; } = 0;
        public decimal Total {  get; set; } = 0;
    }
}
