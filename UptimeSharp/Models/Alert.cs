﻿using Newtonsoft.Json;

namespace UptimeSharp.Models
{
  /// <summary>
  /// The Alert Model
  /// </summary>
  [JsonObject]
  public class Alert
  {
    /// <summary>
    /// Gets or sets the ID.
    /// </summary>
    /// <value>
    /// The ID.
    /// </value>
    [JsonProperty("id")]
    public string ID { get; set; }

    /// <summary>
    /// Gets or sets the alert type.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    [JsonProperty("type")]
    public AlertType Type { get; set; }

    /// <summary>
    /// Gets or sets the alert status.
    /// </summary>
    /// <value>
    /// The status.
    /// </value>
    [JsonProperty("status")]
    public AlertStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the alert value.
    /// </summary>
    /// <value>
    /// The value - Phone Number / E-Mail / Account
    /// </value>
    [JsonProperty("value")]
    public string Value { get; set; }
  }



  /// <summary>
  /// The type of the alert contact notified.
  /// </summary>
  public enum AlertType
  {
    /// <summary>
    /// SMS
    /// </summary>
    SMS = 1,
    /// <summary>
    /// E-Mail
    /// </summary>
    Email = 2,
    /// <summary>
    /// Twitter DM
    /// </summary>
    Twitter = 3,
    /// <summary>
    /// Boxcar
    /// </summary>
    Boxcar = 4
  }


  /// <summary>
  /// The status of the alert contact.
  /// </summary>
  public enum AlertStatus
  {
    /// <summary>
    /// Unknown
    /// </summary>
    Unknown,
    /// <summary>
    /// Not activated
    /// </summary>
    NotActicated = 0,
    /// <summary>
    /// Paused
    /// </summary>
    Paused = 1,
    /// <summary>
    /// Active
    /// </summary>
    Active = 2
  }
}
