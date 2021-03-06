﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UptimeSharp.Models;

namespace UptimeSharp
{
  /// <summary>
  /// UptimeClient
  /// </summary>
  public partial class UptimeClient
  {

    /// <summary>
    /// Retrieves specified monitors from UptimeRobot
    /// </summary>
    /// <param name="monitorIDs">The monitor IDs.</param>
    /// <param name="includeDetails">if set to <c>true</c> [include details (log, alerts and response times)].</param>
    /// <param name="customUptimeRatio">The custom uptime ratio.</param>
    /// <param name="responseTimesAverage">The response times average in minutes, which is used as the calculation base for the response times.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// Monitor List
    /// </returns>
    /// <exception cref="UptimeSharpException"></exception>
    public async Task<List<Models.Monitor>> GetMonitors(
      string[] monitorIDs = null,
      bool includeDetails = true,
      float[] customUptimeRatio = null,
      int responseTimesAverage = 0,
      CancellationToken cancellationToken = default(CancellationToken))
    {
      RetrieveParameters parameters = new RetrieveParameters()
      {
        Monitors = monitorIDs,
        CustomUptimeRatio = customUptimeRatio,
        ShowAlerts = includeDetails ? 1 : 0,
        ShowLog = includeDetails,
        ShowResponseTimes = includeDetails ? 1 : 0,
        ResponseTimeAverage = responseTimesAverage,
        LogsLimit = 25,
        ResponseTimesLimit = 25
      };

      RetrieveResponse response = await Request<RetrieveResponse>("getMonitors", cancellationToken, parameters.Convert());

      int timezoneOffset = 0;
      if ((response != null) && Int32.TryParse(response.Timezone, out timezoneOffset) && (timezoneOffset != 0) && (response.Monitors != null))
      {
         foreach(Models.Monitor monitor in response.Monitors)
         {
            if (monitor.ResponseTimes != null)
            {
              foreach (Models.ResponseTime responseTime in monitor.ResponseTimes)
              {
                responseTime.Date = responseTime.Date.AddMinutes(timezoneOffset);
              }
            }

            if (monitor.Log != null)
            {
              foreach (Models.Log log in monitor.Log)
              {
                log.Date = log.Date.AddMinutes(timezoneOffset);
              }
            }
         }
      }

      return response.Monitors ?? new List<Models.Monitor>();
    }


    /// <summary>
    /// Retrieves a monitor from UptimeRobot
    /// </summary>
    /// <param name="monitorId">a specific monitor ID</param>
    /// <param name="includeDetails">if set to <c>true</c> [include details (log, alerts and response times)].</param>
    /// <param name="customUptimeRatio">The custom uptime ratio.</param>
    /// <param name="responseTimesAverage">The response times average in minutes, which is used as the calculation base for the response times.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// The Monitor
    /// </returns>
    /// <exception cref="UptimeSharpException"></exception>
    public async Task<Models.Monitor> GetMonitor(
      string monitorId,
      bool includeDetails = true,
      float[] customUptimeRatio = null,
      int responseTimesAverage = 0,
      CancellationToken cancellationToken = default(CancellationToken))
    {
      List<Models.Monitor> monitors = await GetMonitors(new string[] { monitorId }, includeDetails, customUptimeRatio, responseTimesAverage, cancellationToken);

      return monitors != null && monitors.Count > 0 ? monitors[0] : null;
    }


    /// <summary>
    /// Deletes a monitor
    /// </summary>
    /// <param name="monitorId">a specific monitor ID</param>
    /// <returns>
    /// Success state
    /// </returns>
    /// <exception cref="UptimeSharpException"></exception>
    public async Task<bool> DeleteMonitor(string monitorId, CancellationToken cancellationToken = default(CancellationToken))
    {
      return (await Request<DefaultResponse>("deleteMonitor", cancellationToken, new Dictionary<string, string>()
      {
        { "monitorID", monitorId }
      })).Success;
    }


    /// <summary>
    /// Deletes a monitor
    /// </summary>
    /// <param name="monitor">The monitor.</param>
    /// <returns>
    /// Success state
    /// </returns>
    /// <exception cref="UptimeSharpException"></exception>
    public async Task<bool> DeleteMonitor(Models.Monitor monitor, CancellationToken cancellationToken = default(CancellationToken))
    {
      return await DeleteMonitor(monitor.ID, cancellationToken);
    }


    /// <summary>
    /// Creates a monitor.
    /// </summary>
    /// <param name="name">The name of the new monitor.</param>
    /// <param name="target">The URI or IP to watch.</param>
    /// <param name="type">The type of the monitor.</param>
    /// <param name="subtype">The subtype of the monitor (if port).</param>
    /// <param name="port">The port (only for Subtype.Custom).</param>
    /// <param name="keywordValue">The keyword value.</param>
    /// <param name="keywordType">Type of the keyword.</param>
    /// <param name="alerts">An ID list of existing alerts to notify.</param>
    /// <param name="HTTPUsername">The HTTP username.</param>
    /// <param name="HTTPPassword">The HTTP password.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// New Monitor (without details)
    /// </returns>
    /// <exception cref="UptimeSharpException"></exception>
    public async Task<Models.Monitor> AddMonitor(
      string name,
      string target,
      Models.Type type = Models.Type.HTTP,
      Subtype subtype = Subtype.Unknown,
      int? port = null,
      string keywordValue = null,
      KeywordType keywordType = KeywordType.Unknown,
      string[] alerts = null,
      string HTTPUsername = null,
      string HTTPPassword = null,
      CancellationToken cancellationToken = default(CancellationToken))
    {

      MonitorParameters parameters = new MonitorParameters()
      {
        Name = name,
        Target = target,
        Type = type,
        Subtype = subtype,
        Port = port,
        KeywordType = keywordType,
        KeywordValue = keywordValue,
        Alerts = alerts,
        HTTPPassword = HTTPPassword,
        HTTPUsername = HTTPUsername
      };

      Models.Monitor monitor = (await Request<AddMonitorResponse>("newMonitor", cancellationToken, parameters.Convert())).Monitor;

      if (monitor != null)
      {
        monitor.Name = name;
        monitor.Target = target;
        monitor.Type = type;
        monitor.Subtype = subtype;
        monitor.Port = port;
        monitor.KeywordType = keywordType;
        monitor.KeywordValue = keywordValue;
        monitor.HTTPPassword = HTTPPassword;
        monitor.HTTPUsername = HTTPUsername;
      }

      return monitor;
    }


    /// <summary>
    /// Edits a monitor.
    /// </summary>
    /// <param name="monitor">The monitor.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// Success state
    /// </returns>
    /// <exception cref="UptimeSharpException"></exception>
    public async Task<bool> ModifyMonitor(Models.Monitor monitor, CancellationToken cancellationToken = default(CancellationToken))
    {
      List<string> alerts = null;
      if (monitor.Alerts != null)
      {
        alerts = monitor.Alerts.Select(item => item.ID).ToList();
      }

      MonitorParameters parameters = new MonitorParameters()
      {
        Name = monitor.Name,
        Target = monitor.Target != null ? monitor.Target : null,
        Port = monitor.Port,
        HTTPPassword = monitor.HTTPPassword,
        HTTPUsername = monitor.HTTPUsername,
        KeywordType = monitor.KeywordType,
        KeywordValue = monitor.KeywordValue,
        Subtype = monitor.Subtype,
        Alerts = alerts != null ? alerts.ToArray() : null
      };

      Dictionary<string, string> paramList = parameters.Convert();

      // fix bad behaviour in API if no subtype is submitted
      if (parameters.Subtype == Subtype.Unknown)
      {
        paramList.Add("monitorSubType", "0");
      }

      paramList.Add("monitorID", monitor.ID.ToString());

      return (await Request<DefaultResponse>("editMonitor", cancellationToken, paramList)).Success;
    }


    /// <summary>
    /// Pause a monitor
    /// </summary>
    /// <param name="monitorId">a specific monitor ID</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// Success state
    /// </returns>
    /// <exception cref="System.NotImplementedException">not available yet</exception>
    /// <exception cref="UptimeSharpException"></exception>
    public async Task<bool> PauseMonitor(string monitorId, CancellationToken cancellationToken = default(CancellationToken))
    {
      throw new NotImplementedException("not available yet");
    }


    /// <summary>
    /// Pause a monitor
    /// </summary>
    /// <param name="monitor">The monitor.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// Success state
    /// </returns>
    /// <exception cref="System.NotImplementedException">not available yet</exception>
    /// <exception cref="UptimeSharpException"></exception>
    public async Task<bool> PauseMonitor(Models.Monitor monitor, CancellationToken cancellationToken = default(CancellationToken))
    {
      return await PauseMonitor(monitor.ID, cancellationToken);
    }


    /// <summary>
    /// Resume a monitor
    /// </summary>
    /// <param name="monitorId">a specific monitor ID</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// Success state
    /// </returns>
    /// <exception cref="System.NotImplementedException">not available yet</exception>
    /// <exception cref="UptimeSharpException"></exception>
    public async Task<bool> ResumeMonitor(string monitorId, CancellationToken cancellationToken = default(CancellationToken))
    {
      throw new NotImplementedException("not available yet");
    }


    /// <summary>
    /// Resume a monitor
    /// </summary>
    /// <param name="monitor">The monitor.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// Success state
    /// </returns>
    /// <exception cref="System.NotImplementedException">not available yet</exception>
    /// <exception cref="UptimeSharpException"></exception>
    public async Task<bool> ResumeMonitor(Models.Monitor monitor, CancellationToken cancellationToken = default(CancellationToken))
    {
      return await ResumeMonitor(monitor.ID, cancellationToken);
    }
  }
}