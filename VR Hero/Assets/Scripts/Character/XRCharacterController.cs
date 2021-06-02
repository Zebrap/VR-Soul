using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XRCharacterController : MonoBehaviour
{
    public Transform head = null;
    private Transform mesh = null;
    public XRController controllerRight = null;
    public XRController controllerLeft = null;

    private Animator animator = null;
    private CharacterController characterControll = null;

    private CharacterManager characterManager;

    private Vector3 currentDirection = Vector3.zero;

    private bool isPressAttack = false;

    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float playerSpeed = 5.0f;
    public float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    public void SetCharacter(Animator animator, CharacterController characterController, CharacterManager characterManager, Transform mesh)
    {
        this.animator = animator;
        this.characterControll = characterController;
        this.characterManager = characterManager;
        this.mesh = mesh;
    }

    private void Update()
    {
        if (characterControll != null)
        {
            if (characterManager.isAlive)
            {
                if (controllerRight.enableInputActions)
                {
                    AttackInput(controllerRight.inputDevice);
                    JumpInput(controllerRight.inputDevice);
                    RotateCharacter(controllerRight.inputDevice);
                }
                if (controllerLeft.enableInputActions)
                {
                    CheckFormMovment(controllerLeft.inputDevice);
                    UseAbility(controllerLeft.inputDevice);
                }
                MoveCharacter();
            }
        }

    }

    private void CheckFormMovment(InputDevice device)
    {
        if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 joystickDirection) && characterManager.canMove)
        {
            CalculateDirection(joystickDirection);

            OrientMesh();
        }

        Animate();
    }
    private void CalculateDirection(Vector2 joystrickDirection)
    {
        Vector3 newDirection = new Vector3(joystrickDirection.x, 0, joystrickDirection.y);

        Vector3 headRotation = new Vector3(0, head.transform.eulerAngles.y, 0);

        currentDirection = Quaternion.Euler(headRotation) * newDirection;
    }
    private void MoveCharacter()
    {
        if (characterManager.canMove)
        {
            groundedPlayer = characterControll.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }
            characterControll.Move(currentDirection * Time.deltaTime * playerSpeed);
            /*  if (currentDirection != Vector3.zero)
              {
                  characterControll.transform.forward = currentDirection;
              }
              */
            playerVelocity.y += gravityValue * Time.deltaTime;
            characterControll.Move(playerVelocity * Time.deltaTime);
        }

        /*     Vector3 movment = currentDirection * speed;

             character.SimpleMove(movment);*/

        /*character.Move(movment * Time.deltaTime);
        private float gravity = 9.8f;
        Vector3 gravityVector = Vector3.zero;
        if(!character.isGrounded){
            gravityVector.y -= gravity;
        }
        character.Move(gravityVector * Time.deltaTime);*/
    }

    private void OrientMesh()
    {
        if (currentDirection != Vector3.zero)
        {
            mesh.transform.forward = currentDirection;
        }
    }

    private void Animate()
    {
        float blend = currentDirection.magnitude;
        animator.SetFloat("Move", blend);
    }

    private void AttackInput(InputDevice device)
    {
        if (device.TryGetFeatureValue(CommonUsages.primaryButton, out bool isPressing))
        {
            if (isPressing)
            {
                if (!isPressAttack)
                {
                    isPressAttack = true;
                    characterManager.Attack();
                }
            }
            else
            {
                isPressAttack = false;
            }
        }
    }

    private void JumpInput(InputDevice device)
    {

        if (device.TryGetFeatureValue(CommonUsages.secondaryButton, out bool isPressing))
        {
            if (isPressing && characterControll.isGrounded && characterManager.canMove)
            {
                characterManager.Jump();
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            }
        }
    }
    private void UseAbility(InputDevice device)
    {
        if (device.TryGetFeatureValue(CommonUsages.secondaryButton, out bool isPressing))
        {
            if (isPressing && characterManager.canMove)
            {
                characterManager.SpecialAttack();
            }
        }
    }

    private void RotateCharacter(InputDevice device)
    {

        if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 joystickDirection) && characterManager.canMove)
        {

            Vector3 newDirection = new Vector3(joystickDirection.x, 0, joystickDirection.y);

            Vector3 headRotation = new Vector3(0, head.transform.eulerAngles.y, 0);

            Vector3 faceToDirection = Quaternion.Euler(headRotation) * newDirection;
            if (faceToDirection != Vector3.zero)
            {
                mesh.transform.forward = faceToDirection;
            }
        }
    }
}
