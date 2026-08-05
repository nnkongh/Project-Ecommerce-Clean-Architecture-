using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Shared
{
    public class PagedResult<T>
    {
        public IReadOnlyList<T> Items { get; init; }
        public int TotalItems { get; init; }
        public int PageIndex { get; init; }   
        public int PageSize { get; init; }

        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
        public PagedResult() { }
        public PagedResult(IReadOnlyList<T> items, int totalItems, int pageIndex, int pageSize)
        {
            Items = items;
            TotalItems = totalItems;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }


    }
}
