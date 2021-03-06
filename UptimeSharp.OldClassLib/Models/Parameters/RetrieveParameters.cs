﻿using RestSharp;
using System;
using System.Collections.Generic;

namespace UptimeSharp.Models
{
  /// <summary>
  /// All parameters which can be passed for monitor retrieval
  /// </summary>
  internal class RetrieveParameters
  {
    /// <summary>
    /// List of monitor ids
    /// </summary>
    /// <value>
    /// The monitors.
    /// </value>
    public int[] Monitors { get; set; }

    /// <summary>
    /// Defines the number of days to calculate the uptime ratio
    /// </summary>
    /// <value>
    /// The custom uptime ratio.
    /// </value>
    public float[] CustomUptimeRatio { get; set; }

    /// <summary>
    /// Defines if the logs of each monitor will be returned
    /// </summary>
    /// <value>
    /// The log bool.
    /// </value>
    public bool? ShowLog { get; set; }

    /// <summary>
    /// Defines if the alert contacts set for the monitor to be returned
    /// </summary>
    /// <value>
    /// The alert contacts bool.
    /// </value>
    public bool? ShowAlerts { get; set; }


    /// <summary>
    /// Converts this instance to a parameter list.
    /// </summary>
    /// <returns>Parameter list</returns>
    public List<Parameter> Convert()
    {
      List<Parameter> parameters = new List<Parameter>();

      if (Monitors != null && Monitors.Length > 0)
      {
        parameters.Add(UptimeClient.Parameter("monitors", String.Join("-", Monitors)));
      }
      if (CustomUptimeRatio != null && CustomUptimeRatio.Length > 0)
      {
        parameters.Add(UptimeClient.Parameter("customUptimeRatio", String.Join("-", CustomUptimeRatio)));
      }

      parameters.Add(UptimeClient.Parameter("showMonitorAlertContacts", (bool)ShowAlerts ? "1" : "0"));

      if (ShowLog.HasValue)
      {
        string showLog = (bool)ShowLog ? "1" : "0";
        parameters.Add(UptimeClient.Parameter("logs", showLog));
        parameters.Add(UptimeClient.Parameter("alertContacts", showLog));
        parameters.Add(UptimeClient.Parameter("showTimezone", showLog));
      }

      return parameters;
    }
  }
}
