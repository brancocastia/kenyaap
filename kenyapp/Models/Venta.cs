using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kenyapp.Models
{
    public class Venta
    {
        public int Id { get; set; }
        public int BebidaId { get; set; }
        public DateTime FechaHora { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
    }
}
