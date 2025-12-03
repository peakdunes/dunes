using DUNES.Shared.TemporalModels;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace DUNES.UI.Interfaces.Print
{
    public class PdfDocumentService : IPdfDocumentService
    {
        public async Task CreatePackingListPdfAsync(string fullPath, TorderRepairTm model, CancellationToken ct)
        {
            var directory = Path.GetDirectoryName(fullPath);
            if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(20);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    // HEADER
                    page.Header().Column(column =>
                    {
                        // 1) Fila con logo izquierda + textos derecha
                        column.Item().Row(row =>
                        {
                            // 🔹 Logo a la izquierda
                            row.ConstantItem(80).Image("wwwroot/logos/logozebra.jpg");
                            // Puedes usar byte[] también:
                            // row.ConstantItem(80).Image(logoBytes);

                            // 🔹 Textos a la derecha (dos líneas)
                            row.RelativeItem().Column(colRight =>
                            {
                                // Primera línea: ZEBRA SOUTH REPAIR DEPOT (derecha)
                                colRight.Item()
                                    .AlignRight()
                                    .Text("ZEBRA SOUTH REPAIR DEPOT")
                                    .FontSize(12)
                                    .SemiBold();

                                // Segunda línea: Packing List (debajo, también derecha)
                                colRight.Item()
                                    .AlignRight()
                                    .Text("Packing List")
                                    .FontSize(16)
                                    .SemiBold();
                            });
                        });

                        // 2) Línea debajo del header
                        column.Item().LineHorizontal(1);
                    });

                    // CONTENIDO (por ahora vacío)
                    page.Content().Column(col =>
                    {
                        col.Item().Text("Aquí va el contenido del reporte...");
                    });

                    page.Footer().AlignCenter().Text("Footer demo").FontSize(8);
                });
            });


            await Task.Run(() => document.GeneratePdf(fullPath), ct);
        }

        //public Task CreatePackingListPdfAsync(string fullPath, TorderRepairTm model, CancellationToken ct)
        //{
        //    throw new NotImplementedException();
        //}



    }
}
