using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using makerspace_tools_api.Models;
using Microsoft.EntityFrameworkCore;

namespace makerspace_tools_api.Data
{
  public class ToolsRepository : IToolsRepository
  {
    private readonly DataContext _context;
    public ToolsRepository(DataContext context) => _context = context;

    public void Add<T>(T entity) where T : class
    {
      _context.Add(entity);
    }

    public void Delete<T>(T entity) where T : class
    {
      _context.Remove(entity);
    }

    public async Task<Tool> GetTool(int id)
    {
      return await _context.Tools.Include(ts => ts.StateHistory).FirstOrDefaultAsync(t => t.Id == id).ConfigureAwait(false);
    }

    public async Task<IEnumerable<Tool>> GetTools()
    {
      return await _context.Tools.Include(ts => ts.StateHistory).ToListAsync().ConfigureAwait(false);
    }

    public async Task<ToolState> GetCurrentToolState(int id)
    {
      return await _context.ToolStates.FirstOrDefaultAsync(ts => ts.Id == id).ConfigureAwait(false);
    }

    public async Task<IEnumerable<ToolState>> GetToolStates(int toolId)
    {
      return await _context
         .ToolStates
         .Where(ts => ts.ToolId == toolId)
         .ToListAsync()
         .ConfigureAwait(false);
    }

    public async Task<Tool> AddTool(Tool tool)
    {
      await _context.Tools.AddAsync(tool).ConfigureAwait(false);
      await _context.SaveChangesAsync().ConfigureAwait(false);

      return tool;
    }

    public async Task<ToolState> AddToolState(ToolState toolState)
    {
      await _context.ToolStates.AddAsync(toolState).ConfigureAwait(false);
      await _context.SaveChangesAsync().ConfigureAwait(false);

      return toolState;
    }

    public async Task<bool> SaveAll()
    {
      return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
    }
  }
}