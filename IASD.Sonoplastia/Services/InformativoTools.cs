using IASD.Sonoplastia.Data;
using System.IO.Compression;

namespace IASD.Sonoplastia.Services
{
    public class InformativoTools : BaseTools<InformativoInfo>, IInformativoTools
    {
        public InformativoTools(IWebHostEnvironment iconfig) : base(iconfig) { }

        protected override string BasePath => Path.Combine(environment.WebRootPath, "Informativo");
        protected override string VideoCache => Path.Combine(BasePath, "video.mp4");
        protected override string ImageCache => Path.Combine(BasePath, "image.jpg");
        protected override string InfoCache => Path.Combine(BasePath, "info.json");
        protected override string FileJSON => "https://raw.githubusercontent.com/MauryDev/IASD-Tools/refs/heads/master/API/Data/informativo.json";

        protected override string WebBasePath => "Informativo";
        protected override Task<Stream> OnVideoDownload(Stream stream)
        {
            using ZipArchive zipfile = new(stream);
            var filemp4 = zipfile.Entries.First((e) => !e.FullName.Contains('/'));
            return Task.FromResult(filemp4.Open());
        }

    }
}
