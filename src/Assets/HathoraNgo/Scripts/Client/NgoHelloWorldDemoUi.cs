// Created by dylan@hathora.dev

using System.Text.RegularExpressions;
using Hathora.Core.Scripts.Runtime.Common.Utils;
using Hathora.Demos.Shared.Scripts.Client.ClientMgr;
using UnityEngine.Assertions;

namespace HathoraNgo.Client
{
    /// <summary>
    /// [HELLO WORLD DEMO] Handles UI clicks -> Calls stateMgr events.
    /// - Base logs UI interactions / handles WebGL workarounds.
    /// </summary>
    public class NgoHelloWorldDemoUi : HathoraHelloWorldDemoUi
    {
        private static NgoStateMgr stateMgr => 
            NgoStateMgr.Singleton;
        
        
        #region UI Interactions
        public override void OnStartServerBtnClick()
        {
            base.OnStartServerBtnClick(); // Logs
            stateMgr.StartServer();
        }
        
        /// <summary>Considers the Hello Wrold demo input field for a custom host:port.
        /// </summary>
        /// <param name="_hostPortOverride"></param>
        public override void OnStartClientBtnClick(string _hostPortOverride = null)
        {
            // We want to override hostPort from the input field - np if null
            _hostPortOverride = ClientConnectInputField.text.Trim();
            
            // Cleanup, if empty string, since we have 2 overloads later
            if (_hostPortOverride == "")
                _hostPortOverride = null;
            
            if (!string.IsNullOrEmpty(_hostPortOverride))
            {
                // Validate input: "{ip||host}:{port}" || "localhost:7777"
                string pattern = HathoraUtils.GetHostIpPortPatternStr();
                bool isHostIpPatternMatch = Regex.IsMatch(_hostPortOverride, pattern);
                Assert.IsTrue(isHostIpPatternMatch, "Expected 'host:port' pattern, " +
                    "such as '1.proxy.hathora.dev:7777' || 'localhost:7777' || '192.168.1.1:7777");    
            }
            
            base.OnStartClientBtnClick(_hostPortOverride); // Logs
            stateMgr.StartClient(_hostPortOverride);
        }

        public override void OnStartHostBtnClick()
        {
            base.OnStartHostBtnClick(); // Logs
            stateMgr.StartHost();
        }

        public override void OnStopServerBtnClick()
        {
            base.OnStopServerBtnClick(); // Logs
            stateMgr.StopServer();
        }

        public override void OnStopClientBtnClick()
        {
            base.OnStopClientBtnClick(); // Logs
            stateMgr.StopClient();
        }
        
        public override void OnStopHostBtnClick()
        {
            base.OnStopHostBtnClick();
            stateMgr.StopHost();
        }
        #endregion // UI Interactions
    }
}
