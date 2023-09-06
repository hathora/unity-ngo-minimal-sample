// Created by dylan@hathora.dev

using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Hathora.Demos.Shared.Scripts.Client.Player
{
    /// <summary>
    /// This UI doesn't need to spawn on the network, but will be called when the client connects to show UI.
    /// </summary>
    public class HathoraLocalClientUiMgr : NetworkBehaviour
    {
        #region Vars
        public static HathoraLocalClientUiMgr Singleton { get; private set; }
        
        [SerializeField]
        private TextMeshProUGUI playerInfoTxt;
        
        private const string HATHORA_VIOLET_COLOR_HEX = "#EEDDFF";
        static string headerBoldColorBegin => $"<b><color={HATHORA_VIOLET_COLOR_HEX}>";
        const string headerBoldColorEnd = "</color></b>";
        #endregion // Vars


        #region Init
        private void Awake()
        {
            setSingleton();
            
            Debug.Log($"[{nameof(HathoraLocalClientUiMgr)}] {nameof(Awake)} - Setting !Active until connected.");
            gameObject.SetActive(false);
        }

        /// <summary>Set a singleton instance - we'll only ever have one instance of this.</summary>
        private void setSingleton()
        {
            if (Singleton != null)
            {
                Destroy(gameObject);
                throw new Exception($"[{nameof(HathoraLocalClientUiMgr)}.{nameof(setSingleton)}] " +
                    "Error: Destroying dupe");
            }
            
            Singleton = this;
        }
        #endregion // Init
        

        public void OnConnected(
            string _clientId,
            int _numClientsConnected)
        {
            Debug.Log($"[{nameof(HathoraLocalClientUiMgr)}] {nameof(OnConnected)}");
            
            playerInfoTxt.text =
                $"{headerBoldColorBegin}ClientId:{headerBoldColorEnd} {_clientId}\n" +
                $"{headerBoldColorBegin}NumClients:{headerBoldColorEnd} {_numClientsConnected}";
            gameObject.SetActive(true);
        }
    }
}
