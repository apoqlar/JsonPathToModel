using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonPathToModel.Tests.ModelData;

public class SampleModelDates
{
    public string Id { get; set; } 
    public SampleModelDatesNested? Nested { get; set; } = new();
    public List<SampleModelDatesNested>? NestedList { get; set; } = [];
    public Dictionary<string, SampleModelDatesNested>? NestedDictionary { get; set; } = [];
}

public class SampleModelDatesNested
{
    public DateTimeOffset Date { get; set; }
    public DateTimeOffset? DateNullable { get; set; }
    public DateOnly DateOnly { get; set; }
    public DateOnly? DateOnlyNullable { get; set; }
}
