using System;

namespace makerspace_tools_api.Models
{
    public class ToolState
    {
        public int Id { get; set; }
        public string State { get; set; }
        public string Note { get; set; }
        public DateTime WhenChanged { get; set; }
        public Tool Tool { get; set; }
        public int ToolId { get; set; }
        public string WhoChanged { get; set; }
    }
}