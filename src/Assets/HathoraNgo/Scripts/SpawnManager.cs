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

        
        /// <summary>Get a random spawn point's Transform position.</summary>
        public Transform GetRandomSpawnPoint()
        {
            Assert.IsNotNull(playerSpawnPoints, "Expected playerSpawnPoints != null");
            Assert.IsTrue(playerSpawnPoints.Length > 0, "Expected playerSpawnPoints.Length > 00");
            
            const int minInclusive = 0;
            int maxExclusive = playerSpawnPoints.Length;
            int randomIndex = Random.Range(minInclusive, maxExclusive);
            
            return playerSpawnPoints[randomIndex];
        }
    }
}
