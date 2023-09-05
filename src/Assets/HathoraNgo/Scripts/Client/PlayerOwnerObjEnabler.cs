// Created by dylan@hathora.dev

using Unity.Netcode;
using UnityEngine;

namespace HathoraNgo.Client
{
    /// <summary>
    /// Attach to your Networked Player. On connected, set these active. 
    /// </summary>
    public class PlayerOwnerObjEnabler : NetworkBehaviour
    {
        [SerializeField]
        private GameObject ownerObjContainer;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn(); // Hover to see docs
            
            Debug.Log($"[{nameof(PlayerOwnerObjEnabler)}.{nameof(OnNetworkSpawn)}] " +
                "Setting owner object container active.");
            
            ownerObjContainer.SetActive(true);
        }
        
        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn(); // Hover to see docs
            Debug.Log($"[{nameof(PlayerOwnerObjEnabler)}.{nameof(OnNetworkDespawn)}] " +
                "Setting owner object container !active.");
            
            ownerObjContainer.SetActive(false);
        }
    }
}
