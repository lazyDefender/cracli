using System;
using System.Collections.Generic;
using System.Text;
using MatthiWare.CommandLine.Core.Attributes;

namespace CraCli.Options
{
    
    public class ProgramOptions
    {
        [Required, Name("n", "name"), Description("Name of the project")]
        public string Name { get; set; }
    }
}
