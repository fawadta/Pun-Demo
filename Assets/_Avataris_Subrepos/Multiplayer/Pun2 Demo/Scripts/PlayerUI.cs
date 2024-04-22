using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Com.MyCompany.MyGame
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI playerNameText;
        [SerializeField] Vector3 screenOffset = new Vector3(0, 3, 0);
        private PlayerManager target;

        float characterControllerHeight;
        Transform targetTransform;
        Renderer targetRenderer;
        CanvasGroup canvasGroup;
        Vector3 targetPosition;
        private void Awake()
        {
            transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
            canvasGroup = GetComponent<CanvasGroup>();
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (target == null)
            {
                Destroy(this.gameObject);
                return;
            }
        }

        void LateUpdate()
        {
            // Do not show the UI if we are not visible to the camera, thus avoid potential bugs with seeing the UI, but not the player itself.
            if (targetRenderer != null)
            {
                canvasGroup.alpha = targetRenderer.isVisible ? 1f : 0f;
            }

            // #Critical
            // Follow the Target GameObject on screen.
            if (targetTransform != null)
            {
                targetPosition = targetTransform.position;
                targetPosition.y += characterControllerHeight;

                transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
            }
        }

        public void SetTarget(PlayerManager _target)
        {
            if (_target == null)
            {
                Debug.LogError("<Color=Red><b>Missing</b></Color> PlayerManager target for PlayerUI.SetTarget");
                return;
            }
            target = _target;
            targetTransform = target.GetComponent<Transform>();
            targetRenderer = target.GetComponentInChildren<Renderer>();


            CharacterController _characterController = target.GetComponent<CharacterController>();

            // Get data from the Player that won't change during the lifetime of this Component
            if (_characterController != null)
            {
                characterControllerHeight = _characterController.height;
            }

            if (playerNameText != null)
                playerNameText.text = target.photonView.Owner.NickName;
        }
    }
}
