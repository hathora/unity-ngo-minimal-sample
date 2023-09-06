// Created by dylan@hathora.dev

using System;
using Hathora.Demos.Shared.Scripts.Client.Player;
using Hathora.Demos.Shared.Scripts.Common;
using HathoraNgo.Client;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace HathoraNgo
{
    /// <summary>
    /// Acts as the liason between NetworkManager and HathoraClientMgr.
    /// - This child class tracks Ngo NetworkManager state changes, and:
    ///   * Handles setting the NetworkManager [host|ip]:port.
    ///   * Can talk to Hathora scripts to get cached host:port.
    ///   * Can initialize or stop NetworkManager connections.
    ///   * Tells base to log + trigger OnDone() events other scripts subcribe to.
    /// - Base contains funcs like: StartServer, StartClient, StartHost.
    /// - Base contains events like: OnLocalClientStarted, OnClientStopped.
    /// - Base tracks `ClientState` like: Stopped, Starting, Started.
    /// - (!) The current state of NGO is still early; they do not yet support WebGL or host names.
    /// </summary>
    public class NgoStateMgr : NetworkMgrStateTracker
    {
        #region vars
        /// <summary>
        /// `New` keyword overrides base Singleton when accessing child directly.
        /// </summary>
        public new static NgoStateMgr Singleton { get; private set; }

        private static NetworkManager netMgr => 
            NetworkManager.Singleton;
        
        private static UnityTransport transport => 
            netMgr.NetworkConfig.NetworkTransport as UnityTransport;

        /// <summary>
        /// - Set by Server @ setClientCountServerRpc
        /// - When updated, Clients get notified via onUpdatedClientCountClientRpc
        /// </summary>
        private NetworkVariable<byte> clientCount = new NetworkVariable<byte>(0);
        #endregion // vars

        
        #region Init
        /// <summary>Set Singleton instance</summary>
        protected override void Awake()
        {
            base.Awake(); // Sets base singleton
            setSingleton();
        }

        /// <summary>Subscribe to NetworkManager Client state changes</summary>
        protected override void Start()
        {
            base.Start();
            subToNgoStateEvents();
        }
        

        #region Init -> Events
        /// <summary>
        /// We must unsubscribe to these at OnDestroy.
        /// (!) Events must not use anon lambdas => otherwise, we can't `-=`
        ///     destroy them later to prevent mem leaks/unexpected behaviour.
        /// </summary>
        private void subToNgoStateEvents()
        {
            // General
            netMgr.OnTransportFailure += OnClientTransportFailureWrapper;
            
            // IsLocalPlayer Client events >> In order of events fired
            // (!) bug: In NGO "Host" mode, "started" seems to unexpectedly trigger before "starting"
            netMgr.OnClientStarted += OnClientStarted;
            netMgr.OnClientConnectedCallback += OnClientStartingWrapper;
            netMgr.OnClientStopped += OnClientStoppedWrapper;
            netMgr.OnClientDisconnectCallback += OnClientStoppedWrapper;
            
            // Server events >> In order of events fired
            netMgr.OnServerStarted += OnServerStarted;
            netMgr.OnServerStopped += OnServerStopped;
        }
        
        /// <summary>Wrapper needs to add `friendlyReason` err string</summary>
        private void OnClientTransportFailureWrapper() => 
            OnStartClientFail("Transport Error");

        /// <summary>Wrapper considers NGO's `OnClientStopped(isServer)`</summary>
        /// <param name="_isServer"></param>
        private void OnClientStoppedWrapper(bool _isServer) => OnClientStopped();
        
        /// <summary>Wrapper considers NGO's `OnClientDisconnectedCallback(clientId)`</summary>
        /// <param name="_clientId"></param>
        private void OnClientStoppedWrapper(ulong _clientId) => OnClientStopped();
        
        /// <summary>Wrapper considers NGO's `OnClientConnectedCallback(clientId)`</summary>
        /// <param name="_clientId"></param>
        private void OnClientStartingWrapper(ulong _clientId) => OnClientStarting();
        #endregion Init -> // Events

        
        /// <summary>Allow this script to be called from anywhere.</summary>
        private void setSingleton()
        {
            if (Singleton != null)
            {
                Debug.LogError($"[{nameof(NgoStateMgr)}]**ERR @ " +
                    $"{nameof(setSingleton)}: Destroying dupe");
                
                Destroy(gameObject);
                return;
            }

            Singleton = this;
        }
        #endregion // Init
        
        
        #region NetworkManager Host
        // ##################################################################################
        // In most NetCode, you don't need to handle host (StartServer->StartClient is same).
        // However, in NGO: This is not the case.
        // ##################################################################################
        
        public void StartHost() =>
            netMgr.StartHost();
        
        public void StopHost() =>
            stopClientServerHost();
        #endregion NetworkManager Host
        
        
        #region NetworkManager Client
        ///<summary>
        /// Connect to the NetworkManager Server as a NetworkManager Client using custom host:ip.
        /// We'll set the host:ip to the NetworkManger -> then call StartClientFromNetworkMgr().
        /// </summary>
        /// <param name="_hostPort">
        /// Contains "host:port" - eg: "1.proxy.hathora.dev:12345".
        /// (!) Unity NGO specifically requires an IP; not a host
        /// </param>
        /// <returns>
        /// startedConnection; to *attempt* the connection (isValid pre-connect vals); we're not connected yet.
        /// </returns>
        public bool StartClient(string _hostPort)
        {
            // Wrong overload?
            if (string.IsNullOrEmpty(_hostPort))
                return StartClient();
            
            string logPrefix = $"[{nameof(NgoStateMgr)}] {nameof(StartClient)}]"; 
            Debug.Log($"{logPrefix} Start");
            
            (string hostNameOrIp, ushort port) hostPortContainer = SplitPortFromHostOrIp(_hostPort);
            bool hasHost = !string.IsNullOrEmpty(hostPortContainer.hostNameOrIp);
            bool hasPort = hostPortContainer.port > 0;

            // Start Ngo Client via selected Transport
            if (!hasHost)
            {
                Debug.LogError($"{logPrefix} !hasHost (from provided `{_hostPort}`): " +
                    "Instead, using default NetworkSettings config");
            }
            else if (!hasPort)
            {
                Debug.LogError($"{logPrefix} !hasPort (from provided `{_hostPort}`): " +
                    "Instead, using default NetworkSettings config");
            }
            else
            {
                // Set custom host:port 1st
                Debug.Log($"{logPrefix} w/Custom hostPort: " +
                    $"`{hostPortContainer.hostNameOrIp}:{hostPortContainer.port}`");

                // (!) Unity NGO specifically requires an IP; not a host
                transport.ConnectionData.Address = hostPortContainer.hostNameOrIp;
                transport.ConnectionData.Port = hostPortContainer.port;
            }
            
            return StartClient();
        }
        
        /// <summary>
        /// Connect to the NetworkManager Server as a NetworkManager Client using NetworkManager host:ip.
        /// This will trigger `OnClientConnecting()` related events.
        ///
        /// TRANSPORT VALIDATION:
        /// - TODO WebGL Validation: !Support in the current version of NGO (although allegedly in beta).
        /// </summary>
        /// <returns>
        /// startedConnection; to *attempt* the connection (isValid pre-connect vals); we're not connected yet.
        /// </returns>
        public bool StartClient()
        {
            string logPrefix = $"[{nameof(NgoStateMgr)}.{nameof(StartClient)}";
            Debug.Log($"{logPrefix} Start");
            
            // Validate
            bool isReadyToConnect = validateIsReadyToConnect();
            if (!isReadyToConnect)
                return false; // !startedConnection
            
            // Log "host:port (transport)" -> Connect using NetworkManager settings
            string ipPort = $"{transport.ConnectionData.Address}:{transport.ConnectionData.Port}";
            string transportName = transport.GetType().Name;
            Debug.Log($"[{logPrefix} Connecting to `{ipPort}` via`{transportName}` transport");
            
            base.OnClientConnecting(); // => callback @ OnClientConected() || OnLocalStartClientFail()
            bool startedConnection = netMgr.StartClient();
            return startedConnection;
        }
        
        /// <summary>
        /// Starts a NetworkManager Client using Hathora lobby session [last queried] cached host:port.
        /// Connect with info from `HathoraClientSession.ServerConnectionInfo.ExposedPort`,
        /// replacing the NetworkManager host:port.
        /// </summary>
        /// <returns>
        /// startedConnection; to *attempt* the connection (isValid pre-connect vals); we're not connected yet.
        /// </returns>
        public bool StartClientFromHathoraLobbySession()
        {
            string ipPort = GetHathoraSessionHostPort(_useIpInsteadOfHost: true); // NGO will reject host names
            return StartClient(ipPort);
        }
        
        /// <summary>Starts a NetworkManager Client.</summary>
        public void StopClient() =>
            stopClientServerHost();
        
        /// <summary>We're about to connect to a server as a Client - ensure we're ready.</summary>
        /// <returns>isValid</returns>
        private bool validateIsReadyToConnect()
        {
            Debug.Log($"[{nameof(NgoStateMgr)}] {nameof(validateIsReadyToConnect)}");
            
            if (netMgr == null)
            {
                base.OnStartClientFail("!NetworkManager");
                return false; // !isSuccess
            }

            // Validate state: We should be stopped (so we don't connect 2x) -> Stop connection 1st?
            if (netMgr.IsConnectedClient)
                stopClientServerHost();

            #region TODO: Validate WebGL Transport
//             SomeWebglTransport someWebglTransport = transport as SomeWebglTransport; 
//             
// #if UNITY_WEBGL
//             Assert.IsNotNull(someWebglTransport, "Expected NetworkManager to use " +
//                 $"{nameof(someWebglTransport)} for WebGL build -- if more transports for WebGL " +
//                 "came out later, edit this Assert script");
// #else
//             Assert.IsNull(someWebglTransport, "!Expected NetworkManager to use " +
//                 $"{nameof(someWebglTransport)} for !WebGL build - Set NetworkManager "+
//                 "transport to, for example, `UnityTransport` (supporting UDP).");
// #endif
            #endregion // TODO: Validate WebGL Transport
            
            // Success - ready to connect
            return true;
        }

        protected override void OnServerStarted()
        {
            base.OnServerStarted(); // Logs + triggers OnServerStartedEvent 
            setClientCountServerRpc(); // Callback => onUpdatedClientCountClientRpc
        }

        /// <summary>
        /// The server just updated clientCount
        /// </summary>
        [ClientRpc]
        private void onUpdatedClientCountClientRpc()
        {
            string clientId = netMgr.LocalClient.ClientId.ToString();
            byte numClientsConnected = clientCount.Value;

            NgoLocalClientUiMgr.Singleton.OnNumClientsChanged(clientId, numClientsConnected);
        }
        #endregion // NetworkManager Client
        
        
        #region NetworkManager Server
        [ServerRpc]
        private void setClientCountServerRpc()
        {
            string logPrefix = $"[{nameof(NgoStateMgr)}.{nameof(setClientCountServerRpc)}]";
            
            try
            {
                clientCount.Value = (byte)netMgr.ConnectedClientsIds.Count;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error: {e}");
                throw;
            }
            
            Debug.Log($"{logPrefix} Server set {nameof(clientCount)}=={clientCount}");
            onUpdatedClientCountClientRpc();
        }
        
        /// <summary>Starts a NetworkManager local Server.</summary>
        public void StartServer() =>
            netMgr.StartServer();

        /// <summary>Stops a NetworkManager local Server. Discards all queued messages.</summary>
        public void StopServer() =>
            stopClientServerHost();
        #endregion // NetworkManager Server


        #region Cleanup
        private void stopClientServerHost() => 
            netMgr.Shutdown(discardMessageQueue: true);
        
        /// <summary>We must unsubscribe to events originally subbed to @ Awake</summary>
        private void OnDestroy()
        {
            if (netMgr == null)
                return; // Perhaps already destroyed
            
            // General
            netMgr.OnTransportFailure -= OnClientTransportFailureWrapper;
            
            // IsLocalPlayer Client events >>
            netMgr.OnClientStarted -= OnClientStarted;
            netMgr.OnClientStopped -= OnClientStoppedWrapper;
            netMgr.OnClientConnectedCallback -= OnClientStartingWrapper;
            netMgr.OnClientDisconnectCallback -= OnClientStoppedWrapper;
            
            //// Server events >> TODO
            // netMgr.OnServerStarted -= () =>
            // netMgr.OnServerStopped -= (bool _isClient) => 
        }
        #endregion // Cleanup
    }
}
