using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using IPGeo.Data;
using Microsoft.Extensions.Hosting;

namespace IPGeo.DatabaseUpdater
{
    public sealed class IPGeoDatabaseUpdaterService : IHostedService, IDisposable
    {
        private static readonly string csvFileName = "IP2LOCATION-LITE-DB1.CSV";
        private static readonly string csvPath = Path.Combine(Path.GetTempPath(), csvFileName);
        private static readonly string csvZipFileName = csvFileName + ".ZIP";
        private static readonly string fileServerUrl = "http://download.ip2location.com/lite/";
        private static readonly string csvZipFileUrl = fileServerUrl + csvZipFileName;
        private static readonly string csvZipPath = Path.Combine(Path.GetTempPath(), csvZipFileName);

        private IPGeoContext _context;
        private Timer _timer;

        public IPGeoDatabaseUpdaterService(IPGeoContext context)
        {
            _context = context;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(
               async (e) =>
                {
                    _context.Database.EnsureCreated();

                    var strategy = await ResolveSetDataStrategyAsync();
                    _context.SetDataStrategy = strategy;

                    var webClient = new WebClient();
                    webClient.DownloadFileCompleted += DownloadIP2LocationCSVFileCompleted;

                    webClient.DownloadFileAsync(new Uri(csvZipFileUrl), csvZipPath);
                },
                null,
                TimeSpan.Zero,
                TimeSpan.FromMinutes(5));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private async Task<ISetDataStrategy> ResolveSetDataStrategyAsync()
        {
            var history = _context.History.FirstOrDefault();
            if (history == null)
            {
                return new SetInitialDataStrategy(_context);
            }

            var date = await GetUploadIP2LocationCSVFileDateAsync();
            var shouldBeUpdated = date.CompareTo(history.UpdatedAt) > 0;

            if (shouldBeUpdated)
            {
                return new UpdateDataStrategy(_context);
            }

            return null;
        }

        private async Task<DateTime> GetUploadIP2LocationCSVFileDateAsync()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, fileServerUrl));
            var htmlString = await response.Content.ReadAsStringAsync();
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlString);
            var xpathSelectNodeWithDate = $"(//tr[./td[./a[text()= '{csvZipFileName}']]]//td[3])";
            var dateNode = htmlDocument.DocumentNode.SelectSingleNode(xpathSelectNodeWithDate);
            var dateString = dateNode.InnerText;
            if (DateTime.TryParse(dateString, out DateTime date))
            {
                return date;
            }

            return DateTime.MinValue;
        }

        private void DownloadIP2LocationCSVFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                ExtractIP2LocationCSVFileFromZip();
                UpdateDatabase();

                File.Delete(csvPath);
                File.Delete(csvZipPath);
            }
        }

        private void ExtractIP2LocationCSVFileFromZip()
        {
            // https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-compress-and-extract-files

            using (var zipArchive = ZipFile.OpenRead(csvZipPath))
            {
                foreach (ZipArchiveEntry entry in zipArchive.Entries)
                {
                    if (!entry.FullName.Equals(csvFileName, StringComparison.OrdinalIgnoreCase))
                        continue;

                    var destinationPath = Path.GetFullPath(csvPath);

                    entry.ExtractToFile(destinationPath, true);

                    break;
                }
            }
        }

        private void UpdateDatabase()
        {
            _context.SetDataStrategy = new SetInitialDataStrategy(_context);
            _context.SetData(csvPath);
        }
    }
}
