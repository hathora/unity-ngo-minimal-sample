// Created by dylan@hathora.dev

using Unity.Netcode;
using UnityEngine;

namespace HathoraNgo.Client
{
    /// <summary>
    /// Assign to Player prefab. On spawn (if IsLocalPlayer), we'll spawn the local player mgr
    /// </summary>
    public class SpawnLocalPlayerMgr : NetworkBehaviour
    {
        [SerializeField]
        private GameObject LocalClientUiMgr;


        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn(); // Hover for docs

            if (!IsLocalPlayer)
                return;

            Debug.Log($"Network-Spawning {LocalClientUiMgr}");
            GameObject instantiatedObject = Instantiate(LocalClientUiMgr);
            
            // Get the NetworkObject of the spawned obj
            NetworkObject networkObject = instantiatedObject.GetComponent<NetworkObject>();
            if (networkObject)
                networkObject.Spawn(destroyWithScene: true);
        }
    }
}
