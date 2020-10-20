using System;

namespace makerspace_tools_api.Dtos
{
    public class ToolStateDto
    {
        public int Id { get; set; }
        public string State { get; set; }
        public string Note { get; set; }
        public DateTime WhenChanged { get; set; }
        public string WhoChanged { get; set; }
    }
}