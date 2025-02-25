
using System.Text.Json.Serialization;

namespace IASD.Sonoplastia.Model;


public class VideoTool
{
    public int Id { get; set; }
    public int Type { get; set; } // 1 = informative, 2 = provai e vede
    public string Titulo { get; set; }
    public string Image { get; set; }
    public string Descricao { get; set; }
    public string Src { get; set; }
    public string Data { get; set; }
    public bool IsLocal { get; set; }


}
