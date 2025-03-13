using Microsoft.EntityFrameworkCore;
using System.Text;
using TransportManagement.Models.Orders;

namespace TransportManagement.Services.OrderReport
{
    public class OrderReportService : IOrderReportService
    {
        private readonly TransportManagementDbContext _context;

        public OrderReportService(TransportManagementDbContext context)
        {
            _context = context;
        }


        public async Task<List<OrderModel>> GetCompletedOrder()
        {
            return await _context.Orders.Where(o => o.Status == OrderStatus.Zakończone).ToListAsync();
        }

        public async Task<List<OrderModel>> GetCompletedOrderForDriver(string driverEmail)
        {
            var completedOrders = await GetCompletedOrder();
            return completedOrders.Where(o => o.DriverEmail == driverEmail).ToList();
        }

        public async Task<byte[]> GeneratePdfReportForDriver(string driverEmail, int year, int month)
        {
            var completedOrders = await GetCompletedOrderForDriver(driverEmail);

            var filteredOrders = completedOrders
                .Where(o => o.StartDate.Year == year && o.StartDate.Month == month)
                .ToList();

            if (!filteredOrders.Any())
            {
                throw new Exception("Brak zleceń dla podanego kierowcy w tym okresie.");
            }

            var htmlContent = GenerateReportHtml(filteredOrders);

            var pdf = GeneratePdfFromHtml(htmlContent);

            return pdf;
        }

        private string GenerateReportHtml(List<OrderModel> orders)
        {
            var sb = new StringBuilder();

            sb.Append("<h1>Raport zleceń dla kierowcy</h1>");
            sb.Append("<table border='1' style='width: 100%; border-collapse: collapse;'>");
            sb.Append("<tr><th>Numer zlecenia</th><th>Data początkowa</th><th>Data końcowa</th><th>Wartość</th><th>Miejsce załadunku</th><th>Miejsce rozładunku</th><th>Typ ładunku</th></tr>");

            foreach (var order in orders)
            {
                sb.Append("<tr>");
                sb.Append($"<td>{order.OrderNumber}</td>");
                sb.Append($"<td>{order.StartDate.ToString("yyyy-MM-dd")}</td>");
                sb.Append($"<td>{order.EndDate.ToString("yyyy-MM-dd")}</td>");
                sb.Append($"<td>{order.Revenue:C}</td>");
                sb.Append($"<td>{order.PickupLocation}</td>");
                sb.Append($"<td>{order.DeliveryLocation}</td>");
                sb.Append($"<td>{order.LoadType}</td>");
                sb.Append("</tr>");
            }

            sb.Append("</table>");

            return sb.ToString();
        }

        private byte[] GeneratePdfFromHtml(string htmlContent)
        {
            var Renderer = new HtmlToPdf();
            var pdfDocument = Renderer.RenderHtmlAsPdf(htmlContent);

            return pdfDocument.BinaryData;
        }
    }
}
