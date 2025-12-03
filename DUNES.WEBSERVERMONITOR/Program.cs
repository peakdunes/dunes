using DUNES.Shared.DTOs.WebService;
using DUNES.WEBSERVERMONITOR;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

internal class Program
{
    private static async Task<int> Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        try
        {
            // 1. Cargar configuración SIN ConfigurationBuilder
            string appSettingsPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            Console.WriteLine($"[DEBUG] Buscando config en: {appSettingsPath}");
          

            if (!File.Exists(appSettingsPath))
            {
                Console.WriteLine($"[ERROR] No se encontró appsettings.json en: {appSettingsPath}");
                return 1;
            }

            var jsonText = await File.ReadAllTextAsync(appSettingsPath, Encoding.UTF8);
            var settings = JsonSerializer.Deserialize<AppSettings>(jsonText) ?? new AppSettings();

            if (string.IsNullOrWhiteSpace(settings.LogDir) ||
                string.IsNullOrWhiteSpace(settings.ApiBaseUrl) ||
                string.IsNullOrWhiteSpace(settings.EndpointPath))
            {
                Console.WriteLine("[ERROR] Configuración incompleta en appsettings.json");
                return 1;
            }

            // 2. Resolver la fecha (imitando tu PowerShell)
            var targetDate = ResolveDate(settings.DateText, settings.UseDaysBack);
            Console.WriteLine($"[INFO] Fecha efectiva: {targetDate:yyyy-MM-dd}");

            // 3. Construir nombre de log: "log YEAR MONTH DAY.txt"
            string logName = $"log {targetDate.Year} {targetDate.Month} {targetDate.Day}.txt";
            string logPath = Path.Combine(settings.LogDir, logName);

            if (!File.Exists(logPath))
            {
                Console.WriteLine($"[WARN] No encontré el archivo: {logPath}");
                return 0; // para que la tarea programada no lo tome como fallo grave
            }

            Console.WriteLine($"[INFO] Leyendo log: {logPath}");

            var lines = await File.ReadAllLinesAsync(logPath, Encoding.UTF8);
            if (lines.Length == 0)
            {
                Console.WriteLine("[INFO] Log vacío. Nada que procesar.");
                return 0;
            }

            // 4. Patrones (mismos que tu script PowerShell)
            var rxTimestamp12 = new Regex(
                @"^(?<date>\d{1,2}/\d{1,2}/\d{4}) (?<time>\d{1,2}:\d{2}:\d{2}) (?<ampm>AM|PM):",
                RegexOptions.Compiled);

            var rxTimestamp24 = new Regex(
                @"^(?<date>\d{1,2}/\d{1,2}/\d{4}) (?<time>\d{2}:\d{2}:\d{2}):",
                RegexOptions.Compiled);

            var rxCallStart = new Regex(
                @"Step1\s+Enter\s+to\s+RepairRequest",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

            var rxError = new Regex(
                @"(timeout expired|error|exception|fail(ed)?|cannot|FATAL|The timeout period elapsed prior to completion)",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

            int totalCalls = 0;
            int totalErrors = 0;

            var perHourCalls = new System.Collections.Generic.Dictionary<DateTime, int>();
            var perHourErrors = new System.Collections.Generic.Dictionary<DateTime, int>();

            var culture = CultureInfo.InvariantCulture;

            // 5. Recorrer líneas del log
            foreach (var rawLine in lines)
            {
                var line = rawLine ?? string.Empty;
                DateTime? dt = null;

                // Intentar timestamp 12h
                var m12 = rxTimestamp12.Match(line);

                string test = string.Empty;
                if (m12.Success)
                {
                    var stamp = $"{m12.Groups["date"].Value} {m12.Groups["time"].Value} {m12.Groups["ampm"].Value}";
                    if (DateTime.TryParseExact(stamp, "M/d/yyyy h:mm:ss tt", culture,
                        DateTimeStyles.None, out var parsed12))
                    {
                        dt = parsed12;
                    }

                    test = stamp.ToString();
                }

               
                // Intentar timestamp 24h si 12h no sirvió
                if (!dt.HasValue)
                {
                    var m24 = rxTimestamp24.Match(line);
                    if (m24.Success)
                    {
                        var stamp = $"{m24.Groups["date"].Value} {m24.Groups["time"].Value}";
                        if (DateTime.TryParseExact(stamp, "M/d/yyyy HH:mm:ss", culture,
                            DateTimeStyles.None, out var parsed24))
                        {
                            dt = parsed24;
                        }
                    }
                }

                if (!dt.HasValue)
                    continue; // no se pudo parsear timestamp, se salta

                var dth = new DateTime(dt.Value.Year, dt.Value.Month, dt.Value.Day, dt.Value.Hour, 0, 0);

                // Contar llamadas
                if (rxCallStart.IsMatch(line))
                {
                    totalCalls++;
                    if (!perHourCalls.ContainsKey(dth)) perHourCalls[dth] = 0;
                    perHourCalls[dth]++;
                }

                // Contar errores
                if (rxError.IsMatch(line))
                {
                    totalErrors++;
                    if (!perHourErrors.ContainsKey(dth)) perHourErrors[dth] = 0;
                    perHourErrors[dth]++;
                }
            }

            Console.WriteLine($"[INFO] TotalCalls={totalCalls}, TotalErrors={totalErrors}");

          

            // 6. Preparar HttpClient y JSON
            using var httpClient = new HttpClient
            {
                BaseAddress = new Uri(settings.ApiBaseUrl)
            };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };

            var allHours = perHourCalls.Keys.Union(perHourErrors.Keys)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            int okCount = 0;
            int failCount = 0;

            // Para generar el summary.txt igual que el ps1
            var summaryLines = new System.Collections.Generic.List<string>
            {
                $"Fecha: {targetDate:yyyy-MM-dd}",
                $"Archivo: {logName}",
                "--------------------------------------------------",
                $"Total de llamadas: {totalCalls}",
                $"Total de errores:  {totalErrors}  ({(totalCalls > 0 ? (double)totalErrors / totalCalls : 0):P1})",
                "",
                "hora                 llamadas   errores   %error",
                "------------------------------------------------"
            };

            foreach (var hour in allHours)
            {
                var c = perHourCalls.TryGetValue(hour, out var v1) ? v1 : 0;
                var e = perHourErrors.TryGetValue(hour, out var v2) ? v2 : 0;

                decimal pct = 0;
                if (c > 0)
                {
                    pct = Math.Round((decimal)e / c * 100m, 1, MidpointRounding.AwayFromZero);
                }

                summaryLines.Add(
                    string.Format("{0,-20} {1,8}   {2,7}   {3,6}",
                        hour.ToString("yyyy-MM-dd HH:00"),
                        c,
                        e,
                        pct));

                var dto = new MvcWebServiceHourlySummaryDto
                {
                    Year = hour.Year,
                    Month = (byte)hour.Month,
                    Day = (byte)hour.Day,
                    Hour = (byte)hour.Hour,
                    TotalCalls = c,
                    TotalErrors = e,
                    ErrorRate = pct,              // el API lo puede recalcular, pero va algo coherente
                    Source = settings.DefaultSource,
                    LastUpdatedUtc = DateTime.UtcNow
                };

                var dtoJson = JsonSerializer.Serialize(dto, jsonOptions);
                var content = new StringContent(dtoJson, Encoding.UTF8, "application/json");

                var endpoint = settings.EndpointPath.TrimStart('/');
                var response = await httpClient.PostAsync(endpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"[OK] {hour:yyyy-MM-dd HH:00} Calls={c} Errors={e} Source={dto.Source}");
                    okCount++;
                }
                else
                {
                    var body = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[ERROR] {hour:yyyy-MM-dd HH:00} Status={(int)response.StatusCode} {response.StatusCode}; Body={body}");
                    failCount++;
                }
            }

            Console.WriteLine($"[INFO] Horas procesadas: OK={okCount}, Fail={failCount}");

            // 7. Guardar summary en archivo (igual que tu ps1)
            if (!string.IsNullOrWhiteSpace(settings.OutDir))
            {
                if (!Directory.Exists(settings.OutDir))
                {
                    Directory.CreateDirectory(settings.OutDir);
                }

                string outName = $"summary {targetDate:yyyy-MM-dd}.txt";
                string outPath = Path.Combine(settings.OutDir, outName);
                await File.WriteAllLinesAsync(outPath, summaryLines, Encoding.UTF8);
                Console.WriteLine($"[INFO] Resumen generado: {outPath}");
            }

            //Console.WriteLine($"[INFO] tarea terminada");
          

            return failCount > 0 ? 2 : 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[FATAL] {ex.Message}");
            
            return 99;
        }
    }

    /// <summary>
    /// Replica la lógica de DateText / DaysBack:
    /// - Si DateText trae texto: hoy/ayer/anteayer o fecha.
    /// - Si no, usa UseDaysBack.
    /// </summary>
    private static DateTime ResolveDate(string? dateText, int useDaysBack)
    {
        if (!string.IsNullOrWhiteSpace(dateText))
        {
            var t = dateText.Trim().ToLowerInvariant();
            if (t is "hoy" or "today")
                return DateTime.Today;
            if (t is "ayer" or "yesterday")
                return DateTime.Today.AddDays(-1);
            if (t is "anteayer" or "day-before-yesterday")
                return DateTime.Today.AddDays(-2);

            var formats = new[]
            {
                "yyyy-MM-dd","dd/MM/yyyy","MM/dd/yyyy",
                "dd-MM-yyyy","MM-dd-yyyy","yyyy/MM/dd"
            };

            foreach (var f in formats)
            {
                if (DateTime.TryParseExact(t, f, CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out var parsedExact))
                {
                    return parsedExact.Date;
                }
            }

            if (DateTime.TryParse(t, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out var parsedLoose))
            {
                return parsedLoose.Date;
            }

            throw new Exception($"No pude interpretar la fecha '{dateText}'. Usa formatos como 2025-11-10 o palabras: hoy/ayer/anteayer.");
        }

        // Sin DateText -> usar DaysBack
        return DateTime.Today.AddDays(-1 * useDaysBack).Date;
    }
}
