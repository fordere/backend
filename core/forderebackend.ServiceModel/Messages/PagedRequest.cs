namespace forderebackend.ServiceModel.Messages
{
    public abstract class PagedRequest
    {
        private int? page;

        public int? Page
        {
            get
            {
                if (page.HasValue == false) return 1;

                return page;
            }
            set => page = value;
        }

        public int? PageSize { get; set; }

        public int Offset
        {
            get
            {
                var pagesToSkip = 0;

                if (Page.HasValue) pagesToSkip = Page.Value - 1;

                return pagesToSkip * PageSize.GetValueOrDefault();
            }
        }

        public bool PagingRequested => PageSize.HasValue;

        public void SetLimitIfNoPagingRequested(int max)
        {
            if (PagingRequested == false) PageSize = max;
        }
    }
}