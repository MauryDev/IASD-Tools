using System.Text.Json;
using System.Diagnostics;
using System.Security.Cryptography.Xml;
using System;

namespace IASD.Sonoplastia.Services;

public abstract class BaseTools<T> where T : Data.FileInfo
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
        var canUpdate = oldinfo == null || oldinfo.Data != newinfo.Data;
        using HttpClient api = new();
        if (canUpdate || !File.Exists(VideoCache))
        {
            using var response = await api.GetStreamAsync(newinfo.Image);
            var imgstream = await OnImageDownload(response);
            using var imagefile = File.OpenWrite(ImageCache);
            await imgstream.CopyToAsync(imagefile);
        }
        if (canUpdate || !File.Exists(VideoCache))
        {
            using var response = await api.GetStreamAsync(newinfo.Url);
            var videostream = await OnVideoDownload(response);
            var fstream = File.OpenWrite(VideoCache);
            await videostream.CopyToAsync(fstream);
          
        }
        await Task.Delay(100);
    }
    protected virtual Task<Stream> OnVideoDownload(Stream stream)
    {
        return Task.FromResult(stream);
    }
    protected virtual Task<Stream> OnImageDownload(Stream stream)
    {
        return Task.FromResult(stream);
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

    public async Task<T> GetInfo()
    {
        using HttpClient api = new();
        var jsonapi = await api.GetStringAsync(FileJSON);
        curinfo = JsonSerializer.Deserialize<T>(jsonapi);
        var cache = GetCacheInfo();
        if (cache == null || cache.Data != curinfo.Data)
        {
            File.WriteAllText(InfoCache, JsonSerializer.Serialize(curinfo));
        }
        return curinfo!;
    }

    public void Open()
    {
        var info = new ProcessStartInfo()
        {
            FileName = "C:\\Users\\Maury\\source\\repos\\IASD Tools\\VideoPlay\\bin\\Debug\\net8.0-windows\\VideoPlay.exe",
        };
        info.ArgumentList.Add(VideoCache);
        Process.Start(info);
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
