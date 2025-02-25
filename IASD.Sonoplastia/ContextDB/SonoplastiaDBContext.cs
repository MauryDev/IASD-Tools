using IASD.Sonoplastia.Model;
using Microsoft.EntityFrameworkCore;

namespace IASD.Sonoplastia.ContextDB;

public class SonoplastiaDBContext : DbContext
{
    public SonoplastiaDBContext(DbContextOptions<SonoplastiaDBContext> options) : base(options) { }
    public DbSet<AudioFundo> AudioFundos { get; set; }
    public DbSet<VideoTool> VideoTools { get; set; }

}
