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
    public class HathoraClientMgrHelloWorldDemoUi : MonoBehaviour
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

        
        private void Awake()
        {
            #if UNITY_WEBGL
            // TODO: Instead of hiding these btns, we may want to just disable it and show a memo beside it
            Debug.Log("[HathoraClientMgrHelloWorldDemoUi.Awake] Hiding " +
                "netStartHostBtn and server btn for WebGL builds - note that clipboard " +
                "btns will !work unless using a custom type made to work with webgl");
            
            netStartServerBtn.gameObject.SetActive(false);
            #endif
        }
    }
}
