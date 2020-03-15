using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounting.Infrastructure.Core
{
    public class PaginationSet<T>
    {
        public int Page { get; set; }

        public IEnumerable<T> Items { get; set; }

        public int Count
        {
            get
            {
                return Items != null ? Items.Count() : 0;
            }
        }

        public int TotalPage { get; set; }
        public int TotalCount { get; set; }
        public int MaxPage { get; set; }
    }
}
