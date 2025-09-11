using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kenyapp.Models
{
    public class Pedido
    {
        public DateTime Fecha { get; set; }
        public List<Producto> Bebidas { get; set; } = new List<Producto>();
    }


}
