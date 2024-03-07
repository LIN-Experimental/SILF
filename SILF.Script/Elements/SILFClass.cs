using System;
using System.Collections.Generic;
using System.Text;

namespace SILF.Script.Elements;

internal class SILFClass
{
    public string Name { get; set; }

    public List<string> Lineas { get; set; } = [];

    public List<IFunction> Functions { get; set; } = [];
}
