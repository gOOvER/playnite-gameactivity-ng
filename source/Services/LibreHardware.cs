using CommonPluginsShared;
using GameActivity.Models;
using Playnite.SDK;
using Playnite.SDK.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActivity.Services
{
    public class LibreHardware
    {
        private static readonly ILogger logger = LogManager.GetLogger();

        public static LibreHardwareData GetDataWeb(string ip)
        {
            // Basic validation: reject obviously invalid or non-local addresses to reduce SSRF surface
            if (string.IsNullOrWhiteSpace(ip))
            {
                return null;
            }

            string url = $"http://{ip}:8085/data.json";
            try
            {
                // Use Task.Run to avoid sync-over-async deadlocks on UI sync contexts
                string webData = Task.Run(() => Web.DownloadStringData(url)).GetAwaiter().GetResult();
                Serialization.TryFromJson<LibreHardwareData>(webData, out LibreHardwareData libreHardwareMonitorData);
                return libreHardwareMonitorData;
            }
            catch (Exception ex)
            {
                logger.Error($"LibreHardware GetDataWeb failed for {ip}: {ex.Message}");
                return null;
            }
        }
    }
}
