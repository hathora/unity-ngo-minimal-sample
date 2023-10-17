// Created by dylan@hathora.dev

using System;
using System.ComponentModel.Design;
using Hathora.Demos.Shared.Scripts.Client.Player;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace HathoraNgo.Client
{
    /// <summary>
    /// This UI doesn't need to spawn on the network, but will be called when the client connects to show UI.
    /// </summary>
    public class NgoLocalClientUiMgr : MonoBehaviour
    {
        #region Vars
        public static NgoLocalClientUiMgr Singleton { get; private set; }
        
        [SerializeField]
        private TextMeshProUGUI playerInfoTxt;

        [SerializeField] 
        private GameObject UiContainer;
        
        private const string HATHORA_VIOLET_COLOR_HEX = "#EEDDFF";
        static string headerBoldColorBegin => $"<b><color={HATHORA_VIOLET_COLOR_HEX}>";
        const string headerBoldColorEnd = "</color></b>";
        
        private NetworkManager netMgr =>
            NetworkManager.Singleton;
        #endregion // Vars


        #region Init
        private void Awake()
        {
            setSingleton();
            
            Debug.Log($"[{nameof(NgoLocalClientUiMgr)}] {nameof(Awake)} - Setting !Active until connected.");
        }

        private void Start()
        {
            netMgr.OnClientConnectedCallback += OnClientConnected;
        }

        private void OnClientConnected(ulong _clientId)
        {
            if (_clientId == netMgr.LocalClientId)
                UpdateUi();
        }

        /// <summary>Set a singleton instance - we'll only ever have one instance of this.</summary>
        private void setSingleton()
        {
            if (Singleton != null)
            {
                Destroy(gameObject);
                throw new Exception($"[{nameof(NgoLocalClientUiMgr)}.{nameof(setSingleton)}] " +
                    "Error: Destroying dupe");
            }
            
            Singleton = this;
        }
        #endregion // Init

        
        /// <summary>Sets local player UI data from cached vals, last known from Server.</summary>
        public void UpdateUi()
        {
            playerInfoTxt.text = $"{headerBoldColorBegin}LocalClientId:{headerBoldColorEnd} {netMgr.LocalClientId}";
            UiContainer.SetActive(true);
        }
    }
}
