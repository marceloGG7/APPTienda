using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.DTO
{
    public record DtoRepVentas
    {
        public int Ticket {  get; set; } = 0;
        public decimal Total { get; set; } = 0;
        public decimal Pago { get; set; } = 0;
        public decimal Cambio { get; set;} = 0;
        public string FecVenta { get; set; } = string.Empty;
        public List<DtoRepVentaDetalle> VentaDetalle { get; set; } = [];

    }
}
