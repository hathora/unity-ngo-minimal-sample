// Created by dylan@hathora.dev

using Unity.Netcode;
using UnityEngine;

namespace HathoraNgo.Server
{
    /// <summary>
    /// [SERVER] NetworkBehaviour script to set transforms on network start.
    /// Selects a random spawn point from preselected Transforms. 
    /// </summary>
    public class PlayerSpawner : NetworkBehaviour
    {
        private SpawnManager spawnMgr =>
            SpawnManager.Singleton;
        

        public override void OnNetworkSpawn()
        {
            if (!IsServer)
                return;
            
            Transform selectedSpawnPoint = spawnMgr.GetRandomSpawnPoint();
            this.transform.position = selectedSpawnPoint.position;
        }
    }
}
