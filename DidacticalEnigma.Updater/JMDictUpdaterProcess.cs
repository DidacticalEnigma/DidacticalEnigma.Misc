using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using JDict;
using static DidacticalEnigma.Updater.Disposable;

namespace DidacticalEnigma.Updater
{
    public class JMDictUpdaterProcess : UpdaterProcess
    {
        private readonly HttpClient httpClient;
        private readonly string url;
        private readonly string oldPath;
        private readonly string oldCachePath;
        private readonly string newPath;
        private readonly string newCachePath;

        public JMDictUpdaterProcess(
            HttpClient httpClient,
            string url,
            string oldPath,
            string oldCachePath,
            string newPath,
            string newCachePath) : base("JMDict")
        {
            this.httpClient = httpClient;
            this.url = url;
            this.oldPath = oldPath;
            this.oldCachePath = oldCachePath;
            this.newPath = newPath;
            this.newCachePath = newCachePath;
        }

        protected override async Task Start()
        {
            this.CurrentStatus = new UpdateStatus.DownloadingStatus(null);
            await this.httpClient.GetToFileAsync(url, newPath);
            this.CurrentStatus = new UpdateStatus.ProcessingStatus();
            
            var declarationsNew =
                With(File.OpenRead(newPath),file =>
                    With(new GZipStream(file, CompressionMode.Decompress), gzip =>
                        DtdUtils.JMDictXmlDeclarations(gzip)));
            var declarationsOld = 
                With(File.OpenRead(oldPath), file =>
                    With(new GZipStream(file, CompressionMode.Decompress), gzip =>
                        DtdUtils.JMDictXmlDeclarations(gzip)));

            var diffResult = DiffResult.Of(declarationsOld, declarationsNew);

            if (diffResult.HasDifferences)
            {
                this.CurrentStatus = new UpdateStatus.FailureStatus("program can't handle the file update");
            }
            else
            {
                try
                {
                    using (var jmdictLookup = await JMDictLookup.CreateAsync(newPath, newCachePath))
                    {

                    }
                    
                    File.Move(oldPath, newPath, overwrite: true);
                    File.Move(oldCachePath, newCachePath, overwrite: true);
                    this.CurrentStatus = new UpdateStatus.SuccessStatus();
                }
                catch
                {
                    this.CurrentStatus = new UpdateStatus.FailureStatus("cache creation failed");
                    File.Delete(newCachePath);
                    throw;
                }
            }
        }
    }
}