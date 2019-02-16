namespace System
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="PagedData" />
    /// </summary>
    [Serializable]
    public class PagedData : PagedData<object>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedData"/> class.
        /// </summary>
        public PagedData()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedData"/> class.
        /// </summary>
        /// <param name="currentPage">The <see cref="int"/></param>
        /// <param name="pageSize">The <see cref="int"/></param>
        public PagedData(int currentPage, int pageSize)
            : base(currentPage, pageSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedData"/> class.
        /// </summary>
        /// <param name="totalCount">The <see cref="int"/></param>
        /// <param name="currentPage">The <see cref="int"/></param>
        /// <param name="pageSize">The <see cref="int"/></param>
        public PagedData(int totalCount, int currentPage, int pageSize)
            : base(new List<object>(), totalCount, currentPage, pageSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedData"/> class.
        /// </summary>
        /// <param name="models">The <see cref="List{object}"/></param>
        public PagedData(List<object> models)
            : base(models, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedData"/> class.
        /// </summary>
        /// <param name="models">The <see cref="List{object}"/></param>
        /// <param name="totalCount">The <see cref="int"/></param>
        public PagedData(List<object> models, int totalCount)
            : base(models, totalCount, 0, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedData"/> class.
        /// </summary>
        /// <param name="models">The <see cref="List{object}"/></param>
        /// <param name="totalCount">The <see cref="int"/></param>
        /// <param name="currentPage">The <see cref="int"/></param>
        /// <param name="pageSize">The <see cref="int"/></param>
        public PagedData(List<object> models, int totalCount, int currentPage, int pageSize)
            : base(models, totalCount, currentPage, pageSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedData"/> class.
        /// </summary>
        /// <param name="paging">The <see cref="Pagination"/></param>
        public PagedData(Pagination paging)
            : base(paging)
        {
        }

        #endregion Constructors
    }
}