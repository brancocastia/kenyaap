using ESC_POS_USB_NET.Printer;
using kenyapp.Models;
using System.Text;

namespace kenyapp.Services
{
    public class ImpresoraService
    {
        private readonly string _printerName;

        public ImpresoraService(string printerName)
        {
            _printerName = printerName;
        }

        public void ImprimirTicket(Pedido pedido)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var printer = new Printer(_printerName);

            decimal total = 0;

            // --- Cabecera ---
            /*printer.AlignCenter();
            printer.BoldMode("KENYA BAR");
            printer.NewLine();
            printer.Append("----------------------");
            printer.NewLine();
            printer.Append($"Fecha: {pedido.Fecha}");
            printer.NewLine();*/

            // --- Bebidas ---
            foreach (var bebida in pedido.Bebidas)
            {
                printer.AlignLeft();
                printer.Append($"{bebida.Nombre} - ${bebida.Precio}");
                printer.NewLine();
                total += bebida.Precio;
            }

            // --- Totales ---
            printer.AlignRight();
            printer.BoldMode($"TOTAL: ${total}");
            printer.NewLine();

            // --- Mensaje final ---
          /*  printer.AlignCenter();
            printer.Append("Gracias y portate bien pajero gonza vigila");
            printer.NewLine();*/

            // --- Avanza y corta ---
            printer.NewLines(3);
            printer.FullPaperCut();

            printer.PrintDocument();
        }
    }
}
