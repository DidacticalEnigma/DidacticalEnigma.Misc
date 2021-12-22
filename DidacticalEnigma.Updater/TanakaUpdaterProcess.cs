using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DidacticalEnigma.Core.Models.DataSources;
using DidacticalEnigma.Core.Models.LanguageService;
using JDict;

namespace DidacticalEnigma.Updater;

public class TanakaUpdaterProcess : UpdaterProcess
{
    private readonly HttpClient httpClient;
    private readonly string url;
    private readonly string oldPath;
    private readonly string newPath;
    private readonly string oldCachePath;
    private readonly string newCachePath;
    private readonly IMorphologicalAnalyzer<IpadicEntry> analyzer;

    public TanakaUpdaterProcess(
        HttpClient httpClient,
        string url,
        string oldPath,
        string newPath,
        string oldCachePath,
        string newCachePath,
        IMorphologicalAnalyzer<IpadicEntry> analyzer) : base("Tanaka Corpus")
    {
        this.httpClient = httpClient;
        this.url = url;
        this.oldPath = oldPath;
        this.newPath = newPath;
        this.oldCachePath = oldCachePath;
        this.newCachePath = newCachePath;
        this.analyzer = analyzer;
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

        try
        {
            var tanaka = new Tanaka(newPath, Encoding.UTF8);
            using (var corpus = new Corpus(tanaka.AllSentences, analyzer, newCachePath))
            {
            
            }

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