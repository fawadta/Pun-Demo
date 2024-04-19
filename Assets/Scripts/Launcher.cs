using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

namespace Com.MyCompany.MyGame
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields
        [Tooltip("The max number of players per room. When a room is full, it can't be jooned by new players, and so new room will be created")]
        [SerializeField] private byte maxPlayersPerRoom = 3;
        #endregion

        #region  Private Fields

        [SerializeField] GameObject loadingPanel;
        [SerializeField] GameObject controlPanel;
        [SerializeField] GameObject progressLabel;

        [SerializeField] TMP_InputField creatRoomInput;
        [SerializeField] TMP_InputField joinRoomInput;

        string gameVersion = "1";

        #endregion

        #region  MonoBehavior CallBacks
        private void Awake()
        {
            //PhotonNetwork.AutomaticallySyncScene = true;
        }

        // Start is called before the first frame update
        void Start()
        {
            loadingPanel.SetActive(true);
            controlPanel.SetActive(false);
            progressLabel.SetActive(false);

            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Start the connection process.
        /// - If already connected, we attempt joining a random room
        /// </summary>
        public void CreateRoom()
        {
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);

            // check if connected or not, join if connected
            if (PhotonNetwork.IsConnected)
            {
                //PhotonNetwork.JoinRandomRoom();
                PhotonNetwork.CreateRoom(creatRoomInput.text);
                //PhotonNetwork.JoinRoom("first");
            }
        }
        public void JoinRoom()
        {
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRoom(joinRoomInput.text);
            }
        }
        public void QuitApplication()
        {
            Application.Quit();
        }
        #endregion

        #region MonoBehaviorPunCallbacks Callbacks
        public override void OnConnectedToMaster()
        {
            Debug.Log("OnCOnnectedToMaster() was called");

            PhotonNetwork.JoinLobby();

        }
        public override void OnJoinedLobby()
        {
            loadingPanel.SetActive(false);
            controlPanel.SetActive(true);
            progressLabel.SetActive(false);

        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            loadingPanel.SetActive(true);
            controlPanel.SetActive(false);
            progressLabel.SetActive(false);
            Debug.LogWarningFormat("OnDisconnectedFromMaster() was called with reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRandomFailed() was called.");

            // failed to join, either rooms are full or none exists. create new room
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }
        public override void OnCreatedRoom()
        {
            Debug.Log("OncreatedRoom() was called.");
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom() was called. Now this client is in the room");
            //if (PhotonNetwork.CurrentRoom.PlayerCount >= 1)
            //{
            //Debug.Log("Loading the room for 2");
            PhotonNetwork.LoadLevel("Multiplayer Room");
            //}
        }

        #endregion

    }
}