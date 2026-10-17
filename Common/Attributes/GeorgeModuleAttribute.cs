using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Attributes
{
    public class GeorgeModuleAttribute:Attribute
    {
        public string MenuTitle { get; set; }
        public string ModuleName { get; set; }
        public string ViewName { get; set; }
    }
}
