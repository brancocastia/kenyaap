using kenyapp.Models;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows;
using System.Text;

public class ImpresoraService
{
    public void ImprimirTicket(Pedido pedido)
    {
        decimal total = 0;
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("KENYA BAR");
        sb.AppendLine("----------------------");
        sb.AppendLine($"Fecha: {pedido.Fecha}");

        foreach (var bebida in pedido.Bebidas)
        {
            sb.AppendLine($"{bebida.Nombre} - ${bebida.Precio}");
            total += bebida.Precio;
        }

        sb.AppendLine("----------------------");
        sb.AppendLine($"TOTAL: ${total}");
        sb.AppendLine("Gracias por su compra. Portate bien pajero, gonzalo vigila");

        string texto = sb.ToString();

        PrintDialog pd = new PrintDialog();
        if (pd.ShowDialog() == true)
        {
            FlowDocument doc = new FlowDocument(new Paragraph(new Run(texto)))
            {
                Name = "Ticket",
                FontSize = 14,
                FontFamily = new System.Windows.Media.FontFamily("Consolas"),
                PageWidth = 300
            };

            pd.PrintDocument(((IDocumentPaginatorSource)doc).DocumentPaginator, "Ticket KenyBar");
        }
    }
}


