namespace Fordere.ServiceInterface.Messages
{
    public abstract class PagedResponse
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public int? Total { get; set; }
    }
}