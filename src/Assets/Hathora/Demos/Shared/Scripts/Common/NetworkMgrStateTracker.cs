// Created by dylan@hathora.dev

using System;
using Hathora.Cloud.Sdk.Model;
using Hathora.Core.Scripts.Runtime.Client;
using UnityEngine;

namespace Hathora.Demos.Shared.Scripts.Common
{
    /// <summary>
    /// Acts as the liason between NetworkManager and HathoraClientMgr, tracking state changes.
    /// - To use, inherit this class and find your NetworkManager state callbacks.
    ///   * On state change, call the common base callback to trigger logs + events.
    ///   * Every NetworkManager tracks states differently, hence this class being abstract.
    /// - Contains funcs like: OnClientStopped, OnLocalClientStarted.
    /// - Contains events like: OnLocalClientStoppedEvent, OnLocalClientStartedEvent.
    /// - Tracks `ClientState` like: Stopped, Starting, Started.
    /// </summary>
    public abstract class NetworkMgrStateTracker : MonoBehaviour
    {
        #region Uncomment for child architecture guide
        // protected abstract void SetSingleton();
        // protected abstract void SubToStateEvents();
        // protected abstract void StartServer();
        // protected abstract void StartClientFromHathoraLobby();
        // protected abstract void StartClientFromNetworkMgr();
        #endregion // Uncomment for child architecture guide
        
        
        #region Core vars
        public static NetworkMgrStateTracker Singleton { get; private set; }

        [SerializeField, Tooltip("Log all client state changes")]
        private bool verboseLogs = true;
        public bool VerboseLogs => verboseLogs;

        /// <summary>When a connection stops, errs out or we get d/c'd, what should we show to clients?</summary>
        protected const string CONNECTION_STOPPED_FRIENDLY_STR = "Connection Stopped";
        
        /// <summary>
        /// States the local connection can be in, following FishNet's standard.
        /// </summary>
        public enum LocalConnectionState : byte
        {
            /// <summary>Connection is fully stopped.</summary>
            Stopped = 0,
            
            /// <summary>Connection is starting but not yet established.</summary>
            Starting = 1,
            
            /// <summary>Connection is established.</summary>
            Started = 2,
            
            /// <summary>Connection is stopping.</summary>
            Stopping = 3,
        }
        #endregion // Core vars

        
        #region Local Client Events
        /// <summary>Event triggers when a NetworkManager client is attempting to connect.</summary>
        public static event Action OnLocalClientConnectingEvent;
        
        /// <summary>Event triggers when a NetworkManager client is connected and starting (but not yet started).</summary>
        public static event Action OnLocalClientStartingEvent;
        
        /// <summary>Event triggers when a NetworkManager client started.</summary>
        /// <returns>roomId, friendlyRegion</returns>
        public static event Action OnLocalClientStartedEvent;

        /// <summary>Event triggers when a NetworkManager client stopped (or disconnected).</summary>
        public static Action OnLocalClientStoppedEvent;
        
        /// <summary>Event triggers when a NetworkManager client failed to start (with UI-friendly err).</summary>
        /// <returns>friendlyReason</returns>
        public static event Action<string> OnLocalStartClientFailEvent;
        #endregion // Local Client Events
        
        
        #region Local Server Events
        /// <summary>Event triggers when a NetworkManager server started.</summary>
        public static event Action OnServerStartedEvent;

        /// <summary>Event triggers when a NetworkManager stopped.</summary>
        /// <returns>isHost</returns>
        public static event Action<bool> OnServerStoppedEvent;
        #endregion // Local Server Events
        
        
        #region Init
        /// <summary>Sets base Singleton instance</summary>
        protected virtual void Awake() =>
            setSingleton();

        /// <summary>Placeholder for future additions</summary>
        protected virtual void Start() { }
        
        /// <summary>Allow this script to be called from anywhere.</summary>
        private void setSingleton()
        {
            if (Singleton != null)
            {
                Debug.LogError($"[{nameof(NetworkMgrStateTracker)}]**ERR @ " +
                    $"{nameof(setSingleton)}: Destroying dupe");
                
                Destroy(gameObject);
                return;
            }

            Singleton = this;
        }
        #endregion // Init
        
        
        #region Client State
        /// <summary>Client state breadcrumbs</summary>
        public enum ClientTrackedState
        {
            /// <summary>Disconnected or failed to connect</summary>
            Stopped,
            
            /// <summary>We're just about to attempt to connect</summary>
            Connecting,

            /// <summary>Connected, but not yet started</summary>
            Starting,

            /// <summary>Connected and started</summary>
            Started,
        }
        
        /// <summary>What's our status?</summary>
        protected ClientTrackedState ClientState { get; set; }

        protected virtual void OnClientConnecting()
        {
            if (verboseLogs)
                Debug.Log($"[{nameof(NetworkMgrStateTracker)}] {nameof(OnClientConnecting)}");
            
            ClientState = ClientTrackedState.Connecting;
            OnLocalClientConnectingEvent?.Invoke(); 
        }
        
        /// <summary>
        /// We are already connected: We are starting, but not yet ready (!started).
        /// OnClientConnectionState() success callback when state == Started. Before this:
        /// - We already called StartClient() || StartClientToLastCachedRoom()
        /// - IsConnectingAsClient == true
        /// </summary>
        protected virtual void OnClientStarting()
        {
            if (verboseLogs)
                Debug.Log($"[{nameof(NetworkMgrStateTracker)}] {nameof(OnClientStarting)}");
            
            ClientState = ClientTrackedState.Starting;
            OnLocalClientStartingEvent?.Invoke();
        }

        /// <summary>We just started and can now run net code</summary>
        protected virtual void OnClientStarted()
        {
            if (verboseLogs)
                Debug.Log($"[{nameof(NetworkMgrStateTracker)}] {nameof(OnClientStarted)}");

            ClientState = ClientTrackedState.Started;
            OnLocalClientStartedEvent?.Invoke();
        }

        /// <summary>We were disconnected from net code</summary>
        protected virtual void OnClientStopped()
        {
            if (verboseLogs)
                Debug.Log($"[{nameof(NetworkMgrStateTracker)}] {nameof(OnClientStopped)}");
            
            ClientState = ClientTrackedState.Stopped;
            OnLocalClientStoppedEvent?.Invoke();
        }
        
        /// <summary>
        /// Tried to connect to a Server as a Client, but failed.
        /// Triggers OnLocalStartClientFailEvent -> OnClientStopped().
        /// </summary>
        /// <param name="_friendlyReason"></param>
        protected virtual void OnStartClientFail(string _friendlyReason)
        {
            if (verboseLogs)
                Debug.Log($"[{nameof(NetworkMgrStateTracker)}.{nameof(OnClientStopped)}] {_friendlyReason}");
            
            OnLocalStartClientFailEvent?.Invoke(_friendlyReason);
            OnClientStopped();
        }
        #endregion // Client State
        
        
        #region Server State
        protected virtual void OnServerStarted()
        {
            if (verboseLogs)
                Debug.Log($"[{nameof(NetworkMgrStateTracker)}] {nameof(OnServerStarted)}");
            
            OnServerStartedEvent?.Invoke();
        }

        /// <param name="_isHost">If "Host", we're both Client + Server</param>
        protected virtual void OnServerStopped(bool _isHost)
        {
            if (verboseLogs)
                Debug.Log($"[{nameof(NetworkMgrStateTracker)}] {nameof(OnServerStopped)}");
            
            OnServerStoppedEvent?.Invoke(_isHost);
        }
        #endregion // Server State
        
        
        #region Hathora
        /// <summary>
        /// Get the last queried "host:port" from a Hathora Client session.
        /// - From `HathoraClientSession.ServerConnectionInfo.ExposedPort`.
        /// </summary>
        public string GetHathoraSessionHostPort()
        {
            ExposedPort connectInfo = HathoraClientSession.Singleton.ServerConnectionInfo.ExposedPort;

            string hostPort = $"{connectInfo.Host}:{connectInfo.Port}";
            return hostPort;
        }
        #endregion Hathora
        
        
        #region Utils
        /// <summary>
        /// This was likely passed in from the UI to override the default
        /// NetworkManager (often from Standalone Client). Eg:
        /// "1.proxy.hathora.dev:12345" -> "1.proxy.hathora.dev", 12345
        /// </summary>
        /// <param name="_hostPort"></param>
        /// <returns></returns>
        protected static (string hostNameOrIp, ushort port) SplitPortFromHostOrIp(string _hostPort)
        {
            if (string.IsNullOrEmpty(_hostPort))
                return default;
            
            string[] hostPortArr = _hostPort.Split(':');
            string hostNameOrIp = hostPortArr[0];
            ushort port = ushort.Parse(hostPortArr[1]);
            
            return (hostNameOrIp, port);
        }
        #endregion // Utils
    }
}
