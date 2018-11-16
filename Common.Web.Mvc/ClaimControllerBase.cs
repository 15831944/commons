namespace System.Web.Mvc
{
    public abstract class ClaimControllerBase<TClaim> : Controller where TClaim : class, IClaimInfo
    {
        protected ClaimControllerBase()
    : this(null)
        {
        }

        protected ClaimControllerBase(IClaimInfo session)
        {
            this.WebSession = session;
        }

        public IClaimInfo WebSession
        {
            get => this.ClaimInfo;
            set
            {
                if (value == null)
                {
                    this.ClaimInfo = null;
                    return;
                }

                var s = value as IClaimInfo;
                this.ClaimInfo = s ?? throw new ArgumentException();
            }
        }

        public IClaimInfo ClaimInfo { get; set; }
    }
}