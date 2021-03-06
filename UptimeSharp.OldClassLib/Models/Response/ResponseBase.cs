﻿using System.Runtime.Serialization;

namespace UptimeSharp.Models
{
  /// <summary>
  /// Base for Responses
  /// </summary>
  [DataContract]
  internal class ResponseBase
  {

    /// <summary>
    /// Gets or sets the error code.
    /// </summary>
    /// <value>
    /// The error code.
    /// </value>
    [DataMember(Name = "id")]
    public string ErrorCode { get; set; }

    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    /// <value>
    /// The error message.
    /// </value>
    [DataMember(Name = "message")]
    public string ErrorMessage { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="ResponseBase"/> is status.
    /// </summary>
    /// <value>
    ///   "ok" or "fail"
    /// </value>
    [DataMember(Name = "stat")]
    public string RawStatus { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="ResponseBase"/> is status.
    /// </summary>
    /// <value>
    ///   <c>true</c> if status is OK; otherwise, <c>false</c>.
    /// </value>
    [IgnoreDataMember]
    public bool Status
    {
      get
      {
        return RawStatus == "ok";
      }
    }
  }
}
