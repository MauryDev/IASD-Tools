using System.IO.Compression;
using System.Text.Json;
using System.Diagnostics;

namespace IASD.Sonoplastia.Services;

public abstract class BaseTools<T> where T : IASD.Sonoplastia.Data.FileInfo
{
    protected IWebHostEnvironment environment;
    protected T? curinfo;

    public BaseTools(IWebHostEnvironment iconfig)
    {
        environment = iconfig;
        Directory.CreateDirectory(BasePath);

        curinfo = GetCacheInfo();
    }

    protected abstract string BasePath { get; }
    protected abstract string WebBasePath { get; }

    protected abstract string VideoCache { get; }
    protected abstract string ImageCache { get; }
    protected abstract string InfoCache { get; }
    protected abstract string FileJSON { get; }

    public T Info => curinfo!;

    public async Task Download()
    {
        var oldinfo = curinfo;
        var newinfo = await GetInfo();
        var canUpdate = oldinfo == null || !oldinfo.Equals(newinfo);
        using HttpClient api = new();
        if (canUpdate || !File.Exists(VideoCache))
        {
            using var response = await api.GetStreamAsync(newinfo.Image);
            using var imagefile = File.OpenWrite(ImageCache);
            await response.CopyToAsync(imagefile);
        }
        if (canUpdate || !File.Exists(VideoCache))
        {
            using var response = await api.GetStreamAsync(newinfo.Url);
            using ZipArchive zipfile = new(response);
            var filemp4 = zipfile.Entries.First((e) => !e.FullName.Contains('/'));
            Directory.CreateDirectory(BasePath);
            filemp4.ExtractToFile(VideoCache, true);
        }
    }

    protected T? GetCacheInfo()
    {
        if (File.Exists(InfoCache))
        {
            var fileinfinfo = JsonSerializer.Deserialize<T>(File.ReadAllText(InfoCache));
            return fileinfinfo;
        }
        return null;
    }

    public Task<T> GetInfo()
    {
        curinfo = JsonSerializer.Deserialize<T>(File.ReadAllText(FileJSON));
        var cache = GetCacheInfo();
        if (cache == null || !cache.Equals(curinfo))
        {
            File.WriteAllText(InfoCache, JsonSerializer.Serialize(curinfo));
        }
        return Task.FromResult(curinfo)!;
    }

    public void Open()
    {
        Process.Start(new ProcessStartInfo()
        {
            FileName = VideoCache,
            UseShellExecute = true
        });
    }

    public string PreviewVideo()
    {
        return $"{WebBasePath}/video.mp4";
    }

    public string PreviewImage()
    {
        return $"{WebBasePath}/image.jpg";
    }
}
