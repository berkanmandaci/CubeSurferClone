using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Inputs;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
     #region Singleton
        public static PlayerController Instance { get { return instance; } }
        private static PlayerController instance;
    
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }
        #endregion
    
        #region PlayerStates
    
        public enum PlayerStates
        {
            START,
            RUN,
            GAMEEND
        }
        private PlayerStates playerState;
    
        public void StartRun()
        {
            playerState = PlayerStates.RUN;
            animator.SetTrigger("SlowRun");
        }
    
        public void WinTheGame(Transform kissTransform)
        {
            playerState = PlayerStates.GAMEEND;
        }

        public void LoseTheGame()
        {
            playerState = PlayerStates.GAMEEND;
        }
    
        #endregion
    
        [SerializeField] private InputSettings inputSettings;
        [SerializeField] private Transform sideMovementRoot;
        [SerializeField] Animator animator;
        [SerializeField] Animator maleAnimator;
        [SerializeField] CinemachineVirtualCamera camera;
    
        [SerializeField]
        [BoxGroup("Speed Settings")]
        private float forwardMovementSpeed = 1f, sideMovementSensitivity = 1f, rotationSpeed = 1f;
    
        [SerializeField]
        [BoxGroup("Limits")]
        private Transform leftLimit, rightLimit, leftLimitWithEnemy, rightLimitWithEnemy;
        
        private float leftLimitX, rightLimitX;
    
        void Update()
        {
            if (playerState == PlayerStates.RUN)
            {
                HandleForwardMovement();
                HandleSideMovement();
            }
        }
        
        #region Movement
    
        private void HandleForwardMovement()
        {
            transform.Translate(Vector3.forward * forwardMovementSpeed * Time.deltaTime);
        }
    
        private void HandleSideMovement()
        {
            var localPos = sideMovementRoot.localPosition;
            localPos += Vector3.right * inputSettings.InputDrag.x * sideMovementSensitivity;
    
            localPos.x = Mathf.Clamp(localPos.x, leftLimitX, rightLimitX);
    
            sideMovementRoot.localPosition = localPos;
        }
        #endregion
}

