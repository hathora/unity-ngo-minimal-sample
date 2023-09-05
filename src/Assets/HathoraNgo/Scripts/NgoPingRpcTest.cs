// Created by Unity | https://docs-multiplayer.unity3d.com/netcode/current/tutorials/get-started-ngo/index.html
// Edited by dylan@hathora.dev

using Unity.Netcode;
using UnityEngine;

namespace HathoraNgo
{
    /// <summary>Client:server ping:pong test via RPC.</summary>
    public class NgoPingRpcTest : NetworkBehaviour
    {
        private readonly NetworkVariable<ulong> numTimesRpcdToServer = new();
        
        private static bool pressedInput_R() => 
            Input.GetKeyDown(KeyCode.R);

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn(); // Hover for docs
            
            if (!IsServer)
                Debug.Log($"[{nameof(NgoPingRpcTest)}.OnNetworkSpawn] {wrapStrInYellow("Press `R` for RPC test")}");
        }

        private void Update()
        {
            if (!pressedInput_R())
                return;
                
            Debug.Log($"[{nameof(NgoPingRpcTest)}.Update] {wrapStrInYellow("Pressed `R`")}");
            
            if ((IsServer && !IsHost) || !IsOwner)
                return;
            
            Debug.Log($"[{nameof(NgoPingRpcTest)}.Update] {wrapStrInYellow("Pinging server...")}");
            pingServerRpc(NetworkObjectId);
        }

        [ClientRpc]
        private void pongClientRpc(ulong _srcNetworkObjectId)
        {
            string msgReceived = wrapStrInYellow(
                $"Client received the RPC #{numTimesRpcdToServer.Value} " +
                $"on NetworkObject #{_srcNetworkObjectId}");

            Debug.Log($"[{nameof(NgoPingRpcTest)}.{nameof(pongClientRpc)}] {msgReceived}");
        }

        [ServerRpc]
        private void pingServerRpc(ulong _srcNetworkObjectId)
        {
            Debug.Log($"[{nameof(NgoPingRpcTest)}.{nameof(pingServerRpc)}] Server received " +
                $"the RPC #{++numTimesRpcdToServer.Value} on NetworkObject #{_srcNetworkObjectId}");
            
            pongClientRpc(_srcNetworkObjectId);
        }
        
        private string wrapStrInYellow(string _str) =>
            $"<color=yellow>{_str}</color>";
    }
}
