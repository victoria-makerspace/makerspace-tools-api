using makerspace_tools_api.Models;
using Microsoft.EntityFrameworkCore;

namespace makerspace_tools_api.Data
{
  public class DataContext : DbContext
  {
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Tool> Tools { get; set; }
    public DbSet<ToolState> ToolStates { get; set; }
  }
}