using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.DTO
{
    public class DtoCarrito
    {
        public int IdInventario { get; set; } = int.MinValue;
        public string Descrip { get; set; } = string.Empty;
        public int Cantidad { get; set; } =  int.MinValue;
        public decimal Precio { get; set; } = decimal.MinValue; 
        public decimal Subtotal { get; set; } = decimal.MinValue;
        public decimal Total { get; set; } = decimal.MinValue;
        public decimal IVA { get; set; } = decimal.MinValue;
    }
}
