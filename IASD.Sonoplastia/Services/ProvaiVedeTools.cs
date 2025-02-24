using IASD.Sonoplastia.Data;

namespace IASD.Sonoplastia.Services
{
    public class ProvaiVedeTools : BaseTools<ProvaiVedeInfo>, IProvaiVedeTools
    {
        public ProvaiVedeTools(IWebHostEnvironment iconfig) : base(iconfig) { }

        protected override string BasePath => Path.Combine(environment.WebRootPath, "ProvaiVede");
        protected override string VideoCache => Path.Combine(BasePath, "video.mp4");
        protected override string ImageCache => Path.Combine(BasePath, "image.jpg");
        protected override string InfoCache => Path.Combine(BasePath, "info.json");
        protected override string FileJSON => "C:\\Users\\Maury\\source\\repos\\IASD Tools\\API\\Data\\provai-e-vede.json";

        protected override string WebBasePath => "ProvaiVede";
    }
}
