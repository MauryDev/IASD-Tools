using IASD.Sonoplastia.Data;

namespace IASD.Sonoplastia.Services
{
    public interface IBaseTools<T> where T : class
    {
        void Open();
        Task Download();
        string PreviewVideo();
        string PreviewImage();
        T Info { get; }
    }
}
