// Created by dylan@hathora.dev

using Hathora.Demos.Shared.Scripts.Client.ClientMgr;

namespace HathoraNgo.Client
{
    /// <summary>
    /// [SDK Demo] Handles the non-Player UI so we can keep the logic separate.
    /// - Generally, this is going to be pre-connection UI such as create/join lobbies.
    /// - UI OnEvent entry points from Buttons start here.
    /// - Base contains UI container and SDK demo actions; we override to send to stateMgr
    /// </summary>
    public class NgoHathoraSdkDemoUi : HathoraSdkDemoUi
    {
        private static NgoStateMgr stateMgr => 
            NgoStateMgr.Singleton;
        

        #region Init
        protected override void Awake() =>
            base.Awake(); // Logs
        
        protected override void Start() =>
            base.Start(); // subToClientMgrEvents()
        #endregion // Init
        
        
        #region UI Interactions
        public override void OnJoinLobbyAsClientBtnClick()
        {
            base.OnJoinLobbyAsClientBtnClick();
            stateMgr.StartClientFromHathoraLobbySession();
        }
        #endregion /Dynamic UI
    }
}
