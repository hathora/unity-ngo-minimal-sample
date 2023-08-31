// Created by dylan@hathora.dev

using Hathora.Demos.Shared.Scripts.Common;
using Unity.Netcode;

namespace HathoraNgo
{
    /// <summary>
    /// Commandline helper - run via `-mode {server|client|host}`.
    /// </summary>
    public class NgoArgHandler : HathoraArgHandlerBase
    {
        private NetworkManager netMgr =>
            NetworkManager.Singleton;
        
        
        private void Start() => 
            _ = base.InitArgsAsync();

        protected override void ArgModeStartServer()
        {
            // Ensure we haven't already started due to a potentially-conflicting auto-server-start we don't know about
            if (netMgr.IsListening)
                return;
            
            base.ArgModeStartServer();
            NgoStateMgr.Singleton.StartServer();
        }

        protected override void ArgModeStartClient()
        {
            if (netMgr.IsListening)
                return;
            
            base.ArgModeStartClient(); // Logs
            NgoStateMgr.Singleton.StartClient();
        }

        protected override void ArgModeStartHost()
        {
            if (netMgr.IsListening)
                return;
            
            base.ArgModeStartHost(); // Logs -> starts server -> starts client
        }
    }
}
