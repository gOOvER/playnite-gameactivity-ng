using GameActivity;
using GameActivity.Services;
using GameActivity.Services.HardwareMonitoring.Models;
using Playnite.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActivity.Services.HardwareMonitoring.Core
{
	public abstract class BaseHardwareProvider : IHardwareDataProvider
	{
		protected static readonly ILogger logger = LogManager.GetLogger();
		protected bool _initialized;
		protected bool _disposed;
		private readonly object _initLock = new object();
		private DateTime _lastInitAttempt = DateTime.MinValue;
		private static readonly TimeSpan ReconnectInterval = TimeSpan.FromSeconds(30);

		/// <summary>Live plugin database (static singleton). Use for current <see cref="GameActivitySettings"/>.</summary>
		private GameActivityDatabase PluginDatabase => GameActivity.PluginDatabase;

		/// <summary>Current plugin settings from the database (same reference as persisted after <c>EndEdit</c>).</summary>
		protected GameActivitySettings LivePluginSettings
		{
			get
			{
				GameActivityDatabase db = PluginDatabase;
				if (db == null)
				{
					return null;
				}
				return db.PluginSettings;
			}
		}

		public abstract string ProviderName { get; }
		public abstract ProviderCapabilities Capabilities { get; }

		public virtual bool IsAvailable
		{
			get
			{
				if (_disposed) return false;
				if (_initialized) return true;

				if (Capabilities != null && Capabilities.RequiresExternalApp)
				{
					lock (_initLock)
					{
						if (_disposed) return false;
						if (_initialized) return true;

						if ((DateTime.UtcNow - _lastInitAttempt) > ReconnectInterval)
						{
							return Initialize();
						}
					}
				}

				return false;
			}
		}

		public int FailureCount { get; protected set; }

		public virtual bool Initialize()
		{
			lock (_initLock)
			{
				if (_disposed) return false;

				_lastInitAttempt = DateTime.UtcNow;
				try
				{
					_initialized = InitializeInternal();
					if (_initialized)
					{
						logger.Info($"[{ProviderName}] Initialized successfully");
					}
					return _initialized;
				}
				catch (Exception ex)
				{
					logger.Error(ex, $"[{ProviderName}] Initialization failed");
					_initialized = false;
					return false;
				}
			}
		}

		protected abstract bool InitializeInternal();

		public HardwareMetrics GetMetrics()
		{
			if (!IsAvailable)
			{
				return new HardwareMetrics();
			}

			try
			{
				return GetMetricsInternal();
			}
			catch (Exception ex)
			{
				FailureCount++;
				logger.Error(ex, $"[{ProviderName}] Failed to get metrics");
				throw;
			}
		}

		protected abstract HardwareMetrics GetMetricsInternal();

		public virtual void Reset()
		{
			lock (_initLock)
			{
				FailureCount = 0;
				_lastInitAttempt = DateTime.MinValue;
				logger.Info($"[{ProviderName}] Reset completed");
			}
		}

		public virtual void Dispose()
		{
			lock (_initLock)
			{
				if (_disposed) return;

				DisposeInternal();
				_disposed = true;
				_initialized = false;

				logger.Info($"[{ProviderName}] Disposed");
			}
		}

		protected virtual void DisposeInternal() { }
	}
}