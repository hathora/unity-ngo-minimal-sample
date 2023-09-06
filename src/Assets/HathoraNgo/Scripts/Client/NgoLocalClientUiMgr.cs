// Created by dylan@hathora.dev

using System;
using Hathora.Demos.Shared.Scripts.Client.Player;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace HathoraNgo.Client
{
    /// <summary>
    /// This UI doesn't need to spawn on the network, but will be called when the client connects to show UI.
    /// </summary>
    public class NgoLocalClientUiMgr : NetworkBehaviour
    {
        #region Vars
        public static NgoLocalClientUiMgr Singleton { get; private set; }
        
        [SerializeField]
        private TextMeshProUGUI playerInfoTxt;
        
        private const string HATHORA_VIOLET_COLOR_HEX = "#EEDDFF";
        static string headerBoldColorBegin => $"<b><color={HATHORA_VIOLET_COLOR_HEX}>";
        const string headerBoldColorEnd = "</color></b>";

        private string clientId;
        private int numClientsConnected;
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


        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            if (IsLocalPlayer)
                UpdateUi();
        }

        /// <summary>Sets local player UI data from cached vals, last known from Server.</summary>
        public void UpdateUi()
        {
            playerInfoTxt.text =
                $"{headerBoldColorBegin}ClientId:{headerBoldColorEnd} {clientId}\n" +
                $"{headerBoldColorBegin}NumClients:{headerBoldColorEnd} {numClientsConnected}";
            
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Called by Server when an updated numClientsConnected occurs
        /// </summary>
        /// <param name="_clientId"></param>
        /// <param name="_numClientsConnected"></param>
        public void OnNumClientsChanged(
            string _clientId,
            byte _numClientsConnected)
        {
            Debug.Log($"[{nameof(HathoraLocalClientUiMgr)}] {nameof(OnNumClientsChanged)}");

            this.clientId = _clientId;
            this.numClientsConnected = _numClientsConnected;
            UpdateUi();
        }
    }
}
