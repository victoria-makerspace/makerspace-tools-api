using System.Collections.Generic;
using System.Threading.Tasks;
using makerspace_tools_api.Models;

namespace makerspace_tools_api.Data
{
  public interface IToolsRepository
  {
    void Add<T>(T entity) where T : class;
    void Delete<T>(T entity) where T : class;
    Task<bool> SaveAll();
    Task<IEnumerable<Tool>> GetTools();
    Task<Tool> GetTool(int id);
    Task<IEnumerable<ToolState>> GetToolStates(int toolId);
    Task<ToolState> GetCurrentToolState(int id);
    Task<Tool> AddTool(Tool tool);
    Task<ToolState> AddToolState(ToolState toolState);
  }
}