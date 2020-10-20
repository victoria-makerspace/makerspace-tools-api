using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using makerspace_tools_api.Data;
using makerspace_tools_api.Dtos;
using makerspace_tools_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace makerspace_tools_api.Controllers
{
  // [AllowAnonymous]
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class ToolsController : ControllerBase
  {
    private readonly IToolsRepository _repo;
    private readonly IMapper _mapper;
    public ToolsController(IToolsRepository repo, IMapper mapper)
    {
      _mapper = mapper;
      _repo = repo;

    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetTools()
    {
      var tools = await _repo.GetTools();

      var toolsToReturn = _mapper.Map<IEnumerable<ToolDto>>(tools);

      return Ok(toolsToReturn);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTool(int id)
    {
      var tool = await _repo.GetTool(id);

      var toolToReturn = _mapper.Map<ToolWithHistoryDto>(tool);

      return Ok(toolToReturn);
    }

    // POST api/tools
    [HttpPost]
    public async Task<IActionResult> Post(ToolDto tool)
    {
      var toolToAdd = _mapper.Map<Tool>(tool);

      toolToAdd.Added = DateTime.Now;

      if(tool.CurrentState != null) {
        toolToAdd.StateHistory = new ToolState[] { _mapper.Map<ToolState>(tool.CurrentState) };
      }
      else {
        toolToAdd.StateHistory = new ToolState[]{
          new ToolState() { State = "new", Note = "Added to the toolkit", Tool = toolToAdd, WhenChanged = DateTime.Now }
        };
      }


      var newTool = await _repo.AddTool(toolToAdd);
      var toolToReturn = _mapper.Map<ToolDto>(toolToAdd);

      return Ok(toolToReturn);
    }

    // PUT api/tools/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, ToolDto tool)
    {
      var toolFromRepo = await _repo.GetTool(id);

      _mapper.Map(tool, toolFromRepo);

      if(await _repo.SaveAll())
        return NoContent();

      throw new Exception($"Updating tool {id} failed on save");
    }

    // DELETE api/tools/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var toolFromRepo = await _repo.GetTool(id);
        _repo.Delete(toolFromRepo);

        if(await _repo.SaveAll())
            return Ok();

        return BadRequest("Failed to delete the tool");
    }
  }
}
