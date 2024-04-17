using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Com.MyCompany.MyGame
{

    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields
        [Tooltip("The max number of players per room. When a room is full, it can't be jooned by new players, and so new room will be created")]
        [SerializeField] private byte maxPlayersPerRoom = 3;
        #endregion

        #region  Private Fields

        [SerializeField] GameObject controlPanel;
        [SerializeField] GameObject progressLabel;
        string gameVersion = "1";

        #endregion

        #region  MonoBehavior CallBacks
        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        // Start is called before the first frame update
        void Start()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
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
        /// - If not yet connected, connect this application instance to cloud network
        /// </summary>
        public void Connect()
        {
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);

            // check if connected or not, join if connected, else initialize the connection
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
        }

        #endregion

        #region MonoBehaviorPunCallbacks Callbacks
        public override void OnConnectedToMaster()
        {
            Debug.Log("OnCOnnectedToMaster() was called");
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            Debug.LogWarningFormat("OnDisconnectedFromMaster() was called with reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRandomFailed() was called.");

            // failed to join, either rooms are full or none exists. create new room
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom() was called. Now this client is in the room");
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("Loading the room for 2");
                PhotonNetwork.LoadLevel("Room for 2");
            }
        }

        #endregion

    }
}