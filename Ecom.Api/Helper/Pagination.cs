namespace Ecom.Api.Helper
{
    public class Pagination<T>where T : class
    {
        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<T> Data { get; set; }

        public Pagination(int pageNum, int pageSize, int totalCount, IEnumerable<T> data)
        {
            PageNum = pageNum;
            PageSize = pageSize;
            TotalCount = totalCount;
            Data = data;
        }
    }
}
