using System;
using System.Collections.Generic;
using System.Linq;
using makerspace_tools_api.Models;
using Newtonsoft.Json;

namespace makerspace_tools_api.Data
{
  public class Seed
  {
    public static void SeedTools(DataContext context)
    {
      if (!context.Tools.Any())
      {
        Console.WriteLine("Reading json data");
        var toolData = System.IO.File.ReadAllText("Data/ToolSeedData.json");
        var tools = JsonConvert.DeserializeObject<List<Tool>>(toolData);
        Console.WriteLine("Tools parsed: " + tools.Count);
        var sortedTools = tools.OrderBy(t => t.Domain).ToList();

        foreach (var tool in tools)
        {
            tool.Added = DateTime.Now;
            tool.Description = "";
            tool.HomeLocation = "";
            tool.StateHistory = new ToolState[]{
                new ToolState() { State = "working", Note = "Added to the toolkit.", Tool = tool, WhenChanged = DateTime.Now, WhoChanged = "nobody" }
            };
            context.Tools.Add(tool);
        }

        context.SaveChanges();
      }
    }
  }
}