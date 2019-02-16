namespace System
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the <see cref="PagedData{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    [DataContract]
    public class PagedData<T>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedData{T}"/> class.
        /// </summary>
        public PagedData()
            : this(new List<T>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedData{T}"/> class.
        /// </summary>
        /// <param name="currentPage">The <see cref="int"/></param>
        /// <param name="pageSize">The <see cref="int"/></param>
        public PagedData(int currentPage, int pageSize)
            : this(new List<T>(), 0, currentPage, pageSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedData{T}"/> class.
        /// </summary>
        /// <param name="totalCount">The <see cref="int"/></param>
        /// <param name="currentPage">The <see cref="int"/></param>
        /// <param name="pageSize">The <see cref="int"/></param>
        public PagedData(int totalCount, int currentPage, int pageSize)
            : this(new List<T>(), totalCount, currentPage, pageSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedData{T}"/> class.
        /// </summary>
        /// <param name="models">The <see cref="List{T}"/></param>
        public PagedData(List<T> models)
            : this(models, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedData{T}"/> class.
        /// </summary>
        /// <param name="models">The <see cref="List{T}"/></param>
        /// <param name="totalCount">The <see cref="int"/></param>
        public PagedData(List<T> models, int totalCount)
            : this(models, totalCount, 0, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedData{T}"/> class.
        /// </summary>
        /// <param name="models">The <see cref="List{T}"/></param>
        /// <param name="totalCount">The <see cref="int"/></param>
        /// <param name="currentPage">The <see cref="int"/></param>
        /// <param name="pageSize">The <see cref="int"/></param>
        public PagedData(List<T> models, int totalCount, int currentPage, int pageSize)
        {
            this.Models = models;
            this.TotalCount = totalCount;
            this.CurrentPage = currentPage;
            this.PageSize = pageSize;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedData{T}"/> class.
        /// </summary>
        /// <param name="paging">The <see cref="Pagination"/></param>
        public PagedData(Pagination paging)
            : this(paging.PageIndex, paging.PageSize)
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the CurrentPage
        /// </summary>
        [DataMember]
        public int CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets the Models
        /// </summary>
        [DataMember]
        public List<T> Models { get; set; }

        /// <summary>
        /// Gets or sets the PageSize
        /// </summary>
        [DataMember]
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the TotalCount
        /// </summary>
        [DataMember]
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets the TotalPage
        /// </summary>
        [DataMember]
        public int TotalPage
        {
            get
            {
                if (this.PageSize == 0)
                {
                    return 0;
                }
                else
                {
                    if (this.TotalCount > 0)
                    {
                        return this.TotalCount % this.PageSize == 0 ? this.TotalCount / this.PageSize : this.TotalCount / this.PageSize + 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        #endregion Properties
    }
}