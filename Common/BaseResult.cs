namespace System
{
  using System.Runtime.Serialization;

  /// <summary>
  /// Defines the <see cref="BaseResult" />
  /// </summary>
  [Serializable]
  [DataContract]
  public class BaseResult : BaseResult<object>
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseResult"/> class.
    /// </summary>
    public BaseResult()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseResult"/> class.
    /// </summary>
    /// <param name="success">The <see cref="bool"/></param>
    public BaseResult(bool success)
    {
      this.Success = success;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseResult"/> class.
    /// </summary>
    /// <param name="success">The <see cref="bool"/></param>
    /// <param name="data">The <see cref="object"/></param>
    public BaseResult(bool success, object data)
    {
      this.Success = success;
      this.Data = data;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseResult"/> class.
    /// </summary>
    /// <param name="success">The <see cref="bool"/></param>
    /// <param name="data">The <see cref="object"/></param>
    /// <param name="message">The <see cref="string"/></param>
    public BaseResult(bool success, object data, string message)
    {
      this.Success = success;
      this.Data = data;

      this.Message = message;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseResult"/> class.
    /// </summary>
    /// <param name="success">The <see cref="bool"/></param>
    /// <param name="data">The <see cref="object"/></param>
    /// <param name="message">The <see cref="string"/></param>
    /// <param name="status">The <see cref="ResultStatus"/></param>
    public BaseResult(bool success, object data, string message, ResultStatus status)
    {
      this.Success = success;
      this.Data = data;
      this.Message = message;
      this.Status = status;
    }

    #endregion Constructors
  }
}