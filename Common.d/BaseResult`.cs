namespace System
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the <see cref="BaseResult{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    [DataContract]
    public class BaseResult<T>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseResult{T}"/> class.
        /// </summary>
        public BaseResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseResult{T}"/> class.
        /// </summary>
        /// <param name="success">The <see cref="bool"/></param>
        public BaseResult(bool success)
        {
            this.Success = success;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseResult{T}"/> class.
        /// </summary>
        /// <param name="success">The <see cref="bool"/></param>
        /// <param name="data">The <see cref="T"/></param>
        public BaseResult(bool success, T data)
        {
            this.Success = success;
            this.Data = data;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseResult{T}"/> class.
        /// </summary>
        /// <param name="success">The <see cref="bool"/></param>
        /// <param name="data">The <see cref="T"/></param>
        /// <param name="message">The <see cref="string"/></param>
        public BaseResult(bool success, T data, string message)
        {
            this.Success = success;
            this.Data = data;
            this.Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseResult{T}"/> class.
        /// </summary>
        /// <param name="success">The <see cref="bool"/></param>
        /// <param name="data">The <see cref="T"/></param>
        /// <param name="message">The <see cref="string"/></param>
        /// <param name="status">The <see cref="ResultStatus"/></param>
        public BaseResult(bool success, T data, string message, ResultStatus status)
        {
            this.Success = success;
            this.Data = data;
            this.Message = message;
            this.Status = status;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the Data
        /// </summary>
        [DataMember]
        public T Data { get; set; }

        /// <summary>
        /// Gets or sets the Message
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the Status
        /// </summary>
        [DataMember]
        public ResultStatus Status { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Success
        /// </summary>
        [DataMember]
        public bool Success { get; set; }

        #endregion Properties
    }
}