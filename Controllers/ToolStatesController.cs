using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using makerspace_tools_api.Data;
using makerspace_tools_api.Dtos;
using makerspace_tools_api.Models;
using makerspace_tools_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace makerspace_tools_api.Controllers
{
  [Authorize]
  [Route("api/tools/{toolId}/states")]
  [ApiController]
  public class ToolStatesController : ControllerBase
  {
    private readonly IToolsRepository _repo;
    private readonly IMapper _mapper;
    private readonly IMakerspaceUserService _userService;
    public ToolStatesController(IToolsRepository repo, IMapper mapper, IMakerspaceUserService userService)
    {
      _mapper = mapper;
      _repo = repo;
      _userService = userService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetToolStates(int toolId)
    {
      var tool = await _repo.GetTool(toolId);

      var toolStatesToReturn = _mapper.Map<IEnumerable<ToolStateDto>>(tool.StateHistory);

      return Ok(toolStatesToReturn);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCurrentToolState(int id)
    {
      var toolState = await _repo.GetCurrentToolState(id);

      var toolStateToReturn = _mapper.Map<ToolStateDto>(toolState);

      return Ok(toolStateToReturn);
    }

    // POST api/tools
    [HttpPost]
    public async Task<IActionResult> AddStateToTool(int toolId, ToolStateDto toolState)
    {
      var toolStateToAdd = _mapper.Map<ToolState>(toolState);

      toolStateToAdd.ToolId = toolId;

      toolStateToAdd.WhenChanged = DateTime.Now;

      toolStateToAdd.WhoChanged = _userService.User.Username;

      await _repo.AddToolState(toolStateToAdd);
      var toolStateToReturn = _mapper.Map<ToolStateDto>(toolStateToAdd);

      return Ok(toolStateToReturn);
    }

    // PUT api/tools/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateToolState(int id, ToolStateDto toolState)
    {
      var toolStateFromRepo = await _repo.GetCurrentToolState(id);

      _mapper.Map(toolState, toolStateFromRepo);

      if(await _repo.SaveAll())
        return NoContent();

      throw new Exception($"Updating tool state {id} failed on save");
    }

    // DELETE api/tools/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteToolState(int id)
    {
        var toolStateFromRepo = await _repo.GetCurrentToolState(id);
        _repo.Delete(toolStateFromRepo);

        if(await _repo.SaveAll())
            return Ok();

        return BadRequest("Failed to delete the tool state");
    }
  }
}
