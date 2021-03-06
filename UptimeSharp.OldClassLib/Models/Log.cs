﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UptimeSharp.Models
{
  /// <summary>
  /// The Log Model
  /// </summary>
  [DataContract]
  public class Log
  {
    /// <summary>
    /// Gets or sets the log type.
    /// </summary>
    /// <value>
    /// The type.
    /// </value>
    [DataMember(Name = "type")]
    public LogType Type { get; set; }

    /// <summary>
    /// Gets or sets the date time, when the log action appeared.
    /// </summary>
    /// <value>
    /// The date.
    /// </value>
    [DataMember(Name = "datetime")]
    public DateTime Date { get; set; }

    /// <summary>
    /// Gets or sets the alerts, which were notified when the log action appeared.
    /// </summary>
    /// <value>
    /// The alert contacts.
    /// </value>
    [DataMember(Name = "alertcontact")]
    public List<Alert> Alerts { get; set; }
  }



  /// <summary>
  /// The type of the log entry.
  /// </summary>
  public enum LogType
  {
    /// <summary>
    /// Down
    /// </summary>
    Down = 1,
    /// <summary>
    /// Up
    /// </summary>
    Up = 2,
    /// <summary>
    /// Started
    /// </summary>
    Started = 98,
    /// <summary>
    /// Paused
    /// </summary>
    Paused = 99
  }
}
