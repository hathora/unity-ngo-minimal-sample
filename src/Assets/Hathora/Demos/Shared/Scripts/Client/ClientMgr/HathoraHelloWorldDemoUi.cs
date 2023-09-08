// Created by dylan@hathora.dev

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hathora.Demos.Shared.Scripts.Client.ClientMgr
{
    /// <summary>
    /// [HELLO WORLD DEMO] Handles UI to keep logic separate.
    /// - Container for UI, notably ClientConnectInputField (for custom host:port).
    /// - Hides "Start Server" btn on WebGL.
    /// - (!) There currently is !Ngo support for WebGL, but there will be soon.
    /// </summary>
    public class HathoraHelloWorldDemoUi : MonoBehaviour
    {
        #region Vars
        [Header("Hello World Demo")]
        [SerializeField, Tooltip("When we connect as a Client, this is " +
             "the optional host:ip optionally passed along")]
        private TMP_InputField clientConnectInputField;
        public TMP_InputField ClientConnectInputField => clientConnectInputField;

        [SerializeField, Tooltip("WebGL builds should hide this")]
        private Button netStartServerBtn;
        #endregion // Vars

        
        /// <summary>Handles WebGL workarounds</summary>
        protected virtual void Awake()
        {
            #if UNITY_WEBGL
            // TODO: Instead of hiding these btns, we may want to just disable it and show a memo beside it
            Debug.Log("[HathoraHelloWorldDemoUi.Awake] Hiding " +
                "netStartHostBtn and server btn for WebGL builds - note that clipboard " +
                "btns will !work unless using a custom type made to work with webgl");
            
            netStartServerBtn.gameObject.SetActive(false);
            #endif
        }
        
        protected virtual void Start()
        {
        }
        
        
        #region UI Interactions
        public virtual void OnStartServerBtnClick()
        {
            Debug.Log($"{nameof(HathoraHelloWorldDemoUi)} {nameof(OnStartServerBtnClick)}");
            // stateMgr.StartServer();
        }

        /// <param name="_hostPortOverride">host:port provided by Hathora</param>
        public virtual void OnStartClientBtnClick(string _hostPortOverride = null)
        {
            Debug.Log($"{nameof(HathoraHelloWorldDemoUi)} {nameof(OnStartClientBtnClick)} " +
                $"_hostPortOverride=={_hostPortOverride} (if null, we'll get from NetworkManager)");
            // stateMgr.StartClient();
        }

        public virtual void OnStartHostBtnClick()
        {
            Debug.Log($"{nameof(HathoraHelloWorldDemoUi)} {nameof(OnStartHostBtnClick)}");
            // stateMgr.StartHost();
        }

        public virtual void OnStopServerBtnClick()
        {
            Debug.Log($"{nameof(HathoraHelloWorldDemoUi)} {nameof(OnStopServerBtnClick)}");
            // stateMgr.StopServer();
        }

        public virtual void OnStopClientBtnClick()
        {
            Debug.Log($"{nameof(HathoraHelloWorldDemoUi)} {nameof(OnStopClientBtnClick)}");
            // stateMgr.StopClient();
        }
        
        public virtual void OnStopHostBtnClick()
        {
            Debug.Log($"{nameof(HathoraHelloWorldDemoUi)} {nameof(OnStopHostBtnClick)}");
            // stateMgr.StopHost();
        }
        #endregion // UI Interactions
    }
}
