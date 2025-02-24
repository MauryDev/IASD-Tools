using IASD.Sonoplastia.Data;

namespace IASD.Sonoplastia.Services;

public interface IInformativoTools
{
    void Open();
    Task Download();
    string PreviewVideo();
    string PreviewImage();
    InformativoInfo Info { get; }
}
