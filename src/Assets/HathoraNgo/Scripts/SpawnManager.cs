// Created by dylan@hathora.dev

using HathoraNgo.Server;
using UnityEngine;
using UnityEngine.Assertions;

namespace HathoraNgo
{
    /// <summary>
    /// Stores Transform information for Server/PlayerSpawner.
    /// - Singleton.
    /// </summary>
    public class SpawnManager : MonoBehaviour
    {
        public static SpawnManager Singleton { get; private set; }
        
        [SerializeField]
        private Transform[] playerSpawnPoints;

        private int roundRobinIndex = 0; 

        
        #region Init
        private void Awake() =>
            setSingleton();

        /// <summary>Set a singleton instance - we'll only ever have one instance of this.</summary>
        private void setSingleton()
        {
            if (Singleton != null)
            {
                Debug.LogError($"[{nameof(PlayerSpawner)}.{nameof(setSingleton)}] " +
                    "Error: Destroying dupe");
                
                Destroy(gameObject);
                return;
            }
            
            Singleton = this;
        }
        #endregion // Init


        /// <summary>Asserts that spawns exist, and has at least 1</summary>
        private void validateSpawns()
        {
            Assert.IsNotNull(playerSpawnPoints, "Expected playerSpawnPoints != null");
            Assert.IsTrue(playerSpawnPoints.Length > 0, "Expected playerSpawnPoints.Length > 0");
        }
        
        /// <summary>Get a random spawn point's Transform position.</summary>
        public Transform GetRandomSpawnPoint()
        {
            validateSpawns();
            
            const int minInclusive = 0;
            int maxExclusive = playerSpawnPoints.Length;
            int randomIndex = Random.Range(minInclusive, maxExclusive);
            
            return playerSpawnPoints[randomIndex];
        }
        
        /// <summary>Get a fixed spawn point's Transform position, rotating from 1st to last spawn points.</summary>
        public Transform GetRoundRobinSpawnPoint()
        {
            validateSpawns();
            return playerSpawnPoints[roundRobinIndex++];
        }
    }
}
