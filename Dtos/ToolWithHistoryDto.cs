using System;
using System.Collections.Generic;

namespace makerspace_tools_api.Dtos
{
    public class ToolWithHistoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Domain { get; set; }
        public string HomeLocation { get; set; }
        public DateTime Added { get; set; }
        public ToolStateDto CurrentState { get; set; }        
        public ICollection<ToolStateDto> StateHistory { get; set; }
    }
}