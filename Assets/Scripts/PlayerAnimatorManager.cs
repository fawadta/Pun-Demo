using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Com.MyCompany.MyGame
{
    public class PlayerAnimatorManager : MonoBehaviourPun
    {
        #region Private Fields

        [SerializeField]
        private float directionDampTime = 0.25f;
        Animator animator;

        #endregion

        #region MonoBehaviour CallBacks
        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!photonView.IsMine && PhotonNetwork.IsConnected)
                return;
            // Prevent control is connected to Photon and represent the localPlayer
            // if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            // {
            //     return;
            // }

            // failSafe is missing Animator component on GameObject
            if (!animator)
            {
                return;
            }

            // deal with Jumping
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // only allow jumping if we are running.
            if (stateInfo.IsName("Base Layer.Run"))
            {
                // When using trigger parameter
                if (Input.GetKeyDown(KeyCode.J)) animator.SetTrigger("Jump");
            }

            // deal with movement
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // prevent negative Speed.
            if (v < 0)
            {
                v = 0;
            }

            // set the Animator Parameters
            animator.SetFloat("Speed", h * h + v * v);
            animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
        }
        #endregion
    }
}
