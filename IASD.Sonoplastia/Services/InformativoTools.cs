

using IASD.Sonoplastia.Data;
using System.IO.Compression;
using System.Text.Json;
using System.Diagnostics;

namespace IASD.Sonoplastia.Services;

public class InformativoTools : IInformativoTools
{
    IWebHostEnvironment environment;
    InformativoInfo? curinfo;
    public InformativoTools(IWebHostEnvironment iconfig)
    {
        environment = iconfig;
        curinfo = GetCacheInfo();
    }
    string InformativoPath => Path.Combine(environment.WebRootPath, "Informativo");
    string InformativoVideoCache => Path.Combine(InformativoPath, "video.mp4");
    string InformativoImageCache => Path.Combine(InformativoPath, "image.jpg");

    string InformativoInfoCache => Path.Combine(InformativoPath, "info.json");
    static string FileJSON => "C:\\Users\\Maury\\source\\repos\\IASD Tools\\API\\Data\\informativo.json";

    public InformativoInfo Info => curinfo!;

    public async Task Download()
    {
        var oldinfo = curinfo;
        var newinfo = await GetInfo();
        var canUpdate = oldinfo == null || oldinfo.Data != newinfo.Data;
        using HttpClient api = new();
        if (canUpdate || !File.Exists(InformativoVideoCache))
        {
            using var response = await api.GetStreamAsync(newinfo.Image);
            using var imagefile = File.OpenWrite(InformativoImageCache);
            await response.CopyToAsync(imagefile);
        }
        if (canUpdate || !File.Exists(InformativoVideoCache))
        {
            using var response = await api.GetStreamAsync(newinfo.Url);
            using ZipArchive zipfile = new(response);
            var filemp4 = zipfile.Entries.First((e) => !e.FullName.Contains('/'));
            Directory.CreateDirectory(InformativoPath);
            filemp4.ExtractToFile(InformativoVideoCache, true);
        }
    }

    InformativoInfo? GetCacheInfo()
    {
        var pathinfo = InformativoInfoCache;
        if (File.Exists(pathinfo))
        {
            var fileinfinfo = JsonSerializer.Deserialize<InformativoInfo>(File.ReadAllText(pathinfo));
            return fileinfinfo;
        } 
        return null;
    }

    public Task<InformativoInfo> GetInfo()
    {
        curinfo = JsonSerializer.Deserialize<InformativoInfo>(File.ReadAllText(FileJSON));
        var cache = GetCacheInfo();
        if (cache == null || cache.Data != curinfo.Data)
        {
            File.WriteAllText(InformativoInfoCache, JsonSerializer.Serialize(curinfo));
        }
        return Task.FromResult(curinfo)!;
    }

    public void Open()
    {
        Process.Start(new ProcessStartInfo() {
            FileName = InformativoVideoCache,
            UseShellExecute = true
        });
    }

    public string PreviewVideo()
    {
        return "Informativo/video.mp4";
    }

    public string PreviewImage()
    {
        return "Informativo/image.jpg";
    }
}
