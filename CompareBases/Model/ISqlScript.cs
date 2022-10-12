using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompareBases.Model
{
    interface ISqlScript
    {
        bool WithTable { get; set; }
        bool TableWithTrigger { get; set; }
        string FilterPrefix { get; set; }
        List<string> FilterIgnoreByPrefix { get; set; }
        List<string> FilterIgnoreByPostfix { get; set; }

        string GetScriptCountObjects();

        string GetScriptDefinitionObjects();
    }
}
