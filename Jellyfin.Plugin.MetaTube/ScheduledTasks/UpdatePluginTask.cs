#if __EMBY__
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Jellyfin.Plugin.MetaTube.Extensions;
using MediaBrowser.Common;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Net;
using MediaBrowser.Model.IO;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.Tasks;
using HttpRequestOptions = MediaBrowser.Common.Net.HttpRequestOptions;

namespace Jellyfin.Plugin.MetaTube.ScheduledTasks;

public class UpdatePluginTask : IScheduledTask
{
    private readonly IApplicationHost _applicationHost;
    private readonly IApplicationPaths _applicationPaths;
    private readonly IHttpClient _httpClient;
    private readonly ILogger _logger;
    private readonly IZipClient _zipClient;

    public UpdatePluginTask(IApplicationHost applicationHost, IApplicationPaths applicationPaths,
        IHttpClient httpClient, ILogManager logManager, IZipClient zipClient)
    {
        _applicationHost = applicationHost;
        _applicationPaths = applicationPaths;
        _httpClient = httpClient;
        _logger = logManager.CreateLogger<UpdatePluginTask>();
        _zipClient = zipClient;
    }

    private static string CurrentVersion => Assembly.GetExecutingAssembly().GetName().Version?.ToString();

    public string Key => $"{Plugin.Instance.Name}UpdatePlugin";

    public string Name => "更新插件";

    public string Description => "更新插件到最新版本";

    public string Category => Plugin.Instance.Name;

    public IEnumerable<TaskTriggerInfo> GetDefaultTriggers()
    {
        yield return new TaskTriggerInfo
        {
            Type = TaskTriggerInfo.TriggerDaily,
            TimeOfDayTicks = TimeSpan.FromHours(5).Ticks
        };
    }

    public async Task Execute(CancellationToken cancellationToken, IProgress<double> progress)
    {
        await Task.Yield();
        progress?.Report(0);

        try
        {
            var apiResult = JsonSerializer.Deserialize<ApiResponseInfo>(await _httpClient.Get(new HttpRequestOptions
            {
                Url = "https://api.github.com/repos/li-peifeng/iMetatube/releases/latest",
                CancellationToken = cancellationToken,
                AcceptHeader = "application/json",
                EnableDefaultUserAgent = true
            }).ConfigureAwait(false));

            var currentVersion = ParseVersion(CurrentVersion);
            var remoteVersion = ParseVersion(apiResult?.TagName);

            if (currentVersion.CompareTo(remoteVersion) < 0)
            {
                _logger.Info("查询到新版本: {0}", remoteVersion);

                var url = apiResult?.Assets
                    .Where(asset => asset.Name.StartsWith("Emby") && asset.Name.EndsWith(".zip")).ToArray()
                    .FirstOrDefault()
                    ?.BrowserDownloadUrl;
                if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                    throw new Exception("下载地址无效");

                var zipStream = await _httpClient.Get(new HttpRequestOptions
                {
                    Url = url,
                    CancellationToken = cancellationToken,
                    EnableDefaultUserAgent = true,
                    Progress = progress
                }).ConfigureAwait(false);

                _zipClient.ExtractAllFromZip(zipStream, _applicationPaths.PluginsPath, true);

                _logger.Info("插件更新完成");

                _applicationHost.NotifyPendingRestart();
            }
            else
            {
                _logger.Info("不需要更新");
            }
        }
        catch (Exception e)
        {
            _logger.Error("更新错误: {0}", e.Message);
        }

        progress?.Report(100);
    }

    private static Version ParseVersion(string v)
    {
        return new Version(v.StartsWith("v") ? v[1..] : v);
    }

    private class ApiResponseInfo
    {
        [JsonPropertyName("tag_name")]
        public string TagName { get; set; }

        [JsonPropertyName("assets")]
        public ApiAssetInfo[] Assets { get; set; }
    }

    private class ApiAssetInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("browser_download_url")]
        public string BrowserDownloadUrl { get; set; }
    }
}

#endif