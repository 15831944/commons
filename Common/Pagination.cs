namespace System
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the <see cref="Pagination" />
    /// </summary>
    [Serializable]
    [DataContract]
    public class Pagination
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Pagination"/> class.
        /// </summary>
        public Pagination()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pagination"/> class.
        /// </summary>
        /// <param name="pageIndex">The <see cref="int"/></param>
        /// <param name="pageSize">The <see cref="int"/></param>
        public Pagination(int pageIndex, int pageSize)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 当前页
        /// </summary>
        [DataMember]
        public int PageIndex { get; set; }

        /// <summary>
        /// 页大小
        /// </summary>
        [DataMember]
        public int PageSize { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// The ToPagedData
        /// </summary>
        /// <returns>The <see cref="PagedData"/></returns>
        public PagedData ToPagedData()
        {
            var pageData = new PagedData(this);
            return pageData;
        }

        /// <summary>
        /// The ToPagedData
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>The <see cref="PagedData{T}"/></returns>
        public PagedData<T> ToPagedData<T>()
        {
            var pageData = new PagedData<T>(this);
            return pageData;
        }

        public PagedData<T> ToPagedData<T>(List<T> models)
        {
            var pageData = new PagedData<T>(this)
            {
                Models = models
            };
            return pageData;
        }

        #endregion Methods
    }
}