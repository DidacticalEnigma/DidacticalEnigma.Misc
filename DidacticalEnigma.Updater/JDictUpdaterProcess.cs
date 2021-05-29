using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using static DidacticalEnigma.Updater.Disposable;

namespace DidacticalEnigma.Updater
{
    public abstract class JDictUpdaterProcess : UpdaterProcess
    {
        protected readonly HttpClient httpClient;
        protected readonly string url;
        protected readonly string oldPath;
        protected readonly string oldCachePath;
        protected readonly string newPath;
        protected readonly string newCachePath;
        private readonly string testedSchemaPath;

        public JDictUpdaterProcess(
            [NotNull] string name,
            [NotNull] HttpClient httpClient,
            [NotNull] string url,
            [NotNull] string oldPath,
            [NotNull] string oldCachePath,
            [NotNull] string newPath,
            [NotNull] string newCachePath,
            [CanBeNull] string testedSchemaPath = null) : base(name)
        {
            this.httpClient = httpClient;
            this.url = url;
            this.oldPath = oldPath;
            this.oldCachePath = oldCachePath;
            this.newPath = newPath;
            this.newCachePath = newCachePath;
            this.testedSchemaPath = testedSchemaPath;
        }

        protected override async Task Start()
        {
            if (!File.Exists(newPath))
            {
                try
                {
                    this.CurrentStatus = new UpdateStatus.DownloadingStatus(null);
                    await this.httpClient.GetToFileAsync(url, newPath);
                }
                catch
                {
                    File.Delete(newPath);
                    throw;
                }
            }

            this.CurrentStatus = new UpdateStatus.ProcessingStatus();
            
            var declarationsNew =
                With(File.OpenRead(newPath),file =>
                    With(new GZipStream(file, CompressionMode.Decompress), gzip =>
                        DtdUtils.JMDictXmlDeclarations(gzip)));
            var declarationsOld = 
                testedSchemaPath != null
                    ? With(File.OpenRead(testedSchemaPath), file =>
                        DtdUtils.JMDictXmlDeclarations(file))
                    : With(File.OpenRead(oldPath), file =>
                        With(new GZipStream(file, CompressionMode.Decompress), gzip =>
                            DtdUtils.JMDictXmlDeclarations(gzip)));

            var diffResult = DiffResult.Of(declarationsOld, declarationsNew);

            if (diffResult.HasDifferences)
            {
                string longMessage;
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("Show the developer this message");
                stringBuilder.AppendLine("-- BEGINNING OF DIFFERENCES");
                foreach (var oldEntry in diffResult.OldEntries)
                {
                    stringBuilder.AppendLine($"- {oldEntry}");
                }
                foreach (var newEntry in diffResult.NewEntries)
                {
                    stringBuilder.AppendLine($"+ {newEntry}");                        
                }

                stringBuilder.AppendLine("-- END OF DIFFERENCES");
                this.CurrentStatus = new UpdateStatus.FailureStatus(
                    "Program can't handle the file update. Report this as a bug in the program.",
                    stringBuilder.ToString());

            }
            else
            {
                try
                {
                    await CreateCache();

                    File.Delete(oldPath);
                    File.Move(newPath, oldPath);
                    if (oldCachePath != null)
                    {
                        File.Delete(oldCachePath);
                        File.Move(newCachePath, oldCachePath);
                    }

                    this.CurrentStatus = new UpdateStatus.SuccessStatus();
                }
                catch
                {
                    this.CurrentStatus = new UpdateStatus.FailureStatus("Cache creation failed");
                    if (oldCachePath != null)
                    {
                        File.Delete(newCachePath);
                    }
                    throw;
                }
            }
        }

        protected abstract Task CreateCache();
    }
}