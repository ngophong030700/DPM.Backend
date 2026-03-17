using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.Domain.Enum
{
    public enum SortDirectionEnum
    {
        [Description("Asc")]
        Asc = 1,

        [Description("Desc")]
        Desc = 2
    }
}
