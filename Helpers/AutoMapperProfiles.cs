using System.Linq;
using AutoMapper;
using makerspace_tools_api.Dtos;
using makerspace_tools_api.Models;

namespace makerspace_tools_api.Helpers
{
  public class AutoMapperProfiles : Profile
  {
    public AutoMapperProfiles()
    {
      CreateMap<ToolState, ToolStateDto>();
      CreateMap<ToolStateDto, ToolState>();
      CreateMap<Tool, ToolDto>()
        .ForMember(dest => dest.CurrentState,
                   opt => opt.MapFrom(src => src.StateHistory.LastOrDefault()));
      CreateMap<ToolDto, Tool>();
      CreateMap<Tool, ToolWithHistoryDto>()
        .ForMember(dest => dest.CurrentState,
                   opt => opt.MapFrom(src => src.StateHistory.LastOrDefault()));      
    }
  }
}