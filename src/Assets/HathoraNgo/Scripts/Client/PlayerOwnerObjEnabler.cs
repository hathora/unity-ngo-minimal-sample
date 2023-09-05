// Created by dylan@hathora.dev

using Hathora.Demos.Shared.Scripts.Client.Player;
using Unity.Netcode;
using UnityEngine;

namespace HathoraNgo.Client
{
    /// <summary>
    /// Attach to your Networked Player. On connected, set these active.
    /// - Player Camera
    /// - This also toggles LocalPlayerCanvas on Spawn/Despawn
    /// </summary>
    public class PlayerOwnerObjEnabler : NetworkBehaviour
    {
        [SerializeField]
        private GameObject ownerObjContainer;

        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn(); // Hover to see docs
            
            Debug.Log($"[{nameof(PlayerOwnerObjEnabler)}.{nameof(OnNetworkSpawn)}] " +
                "Setting owner object container (+LocalPlayerCanvas) active.");
            
            togglePlayerOwnerObjs(_enable: true);
        }
        
        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn(); // Hover to see docs
            Debug.Log($"[{nameof(PlayerOwnerObjEnabler)}.{nameof(OnNetworkDespawn)}] " +
                "Setting owner object container (+LocalPlayerCanvas) !active.");

            togglePlayerOwnerObjs(_enable: false);
        }

        private void togglePlayerOwnerObjs(bool _enable)
        {
            ownerObjContainer.SetActive(_enable);
            HathoraLocalClientUiMgr.Singleton.gameObject.SetActive(_enable);
        }
    }
}
