using ClosedXML.Excel;
using TallerMecanico.ViewModels;

namespace TallerMecanico.Services
{
    public class ReportService
    {
        public byte[] GenerarExcelProductos(List<ProductoDto> productos)
        {
            using var libro = new XLWorkbook();
            var hoja = libro.Worksheets.Add("Inventario");

            // Encabezados
            hoja.Cell(1, 1).Value = "Nombre";
            hoja.Cell(1, 2).Value = "Precio";
            hoja.Cell(1, 3).Value = "Stock";
            hoja.Range("A1:C1").Style.Font.Bold = true;

            // Llenar Datos
            for (int i = 0; i < productos.Count; i++)
            {
                hoja.Cell(i + 2, 1).Value = productos[i].Nombre;
                hoja.Cell(i + 2, 2).Value = productos[i].Precio;
                hoja.Cell(i + 2, 3).Value = productos[i].Stock;
            }

            // Formato de tabla y números
            hoja.Columns().AdjustToContents();

            using var memoria = new MemoryStream();
            libro.SaveAs(memoria);
            return memoria.ToArray();
        }
    }
}