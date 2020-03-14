namespace Fordere.ServiceInterface.Messages
{
    public abstract class PagedRequest
    {
        private int? page;

        public int? Page
        {
            get
            {
                if (this.page.HasValue == false)
                {
                    return 1;
                }

                return this.page;
            }
            set { this.page = value; }
        }

        public int? PageSize { get; set; }

        public int Offset
        {
            get
            {
                int pagesToSkip = 0;

                if (this.Page.HasValue)
                {
                    pagesToSkip = this.Page.Value - 1;
                }

                return pagesToSkip*this.PageSize.GetValueOrDefault();
            }
        }

        public bool PagingRequested
        {
            get { return this.PageSize.HasValue; }
        }

        public void SetLimitIfNoPagingRequested(int max)
        {
            if (this.PagingRequested == false)
            {
                this.PageSize = max;
            }
        }
    }
}