
namespace APGLogs.DomainHelper.Attributes
{
    public class PagingQuery
    {
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
