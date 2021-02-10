﻿using System.Collections.Generic;
using System.Linq;
using TempusHiring.BusinessLogic.Abstract;

namespace TempusHiring.BusinessLogic.Pagination
{
    public class PagedResult<T> : PagedResultBase where T : class
    {
        public IEnumerable<T> Results { get; set; }
        public PagedResult() => Results = Enumerable.Empty<T>();
    }
}