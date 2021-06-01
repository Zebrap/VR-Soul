using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XRCharacterController : MonoBehaviour
{
    public float speed = 5.0f;

    public Transform head = null;
    private Transform mesh = null;
    public XRController controllerRight = null;
    public XRController controllerLeft = null;

    private Animator animator = null;
    private CharacterController character = null;

    private CharacterManager characterManager;

    private Vector3 currentDirection = Vector3.zero;

    private bool isPressAttack = false;

    public void SetCharacter(Animator animator, CharacterController characterController, CharacterManager characterManager, Transform mesh)
    {
        this.animator = animator;
        this.character = characterController;
        this.characterManager = characterManager;
        this.mesh = mesh;
    }

    private void Update()
    {
        if (controllerRight.enableInputActions && character != null)
        {
            if (characterManager.isAlive)
            {
                AttackInput(controllerRight.inputDevice);
                UseAbility(controllerRight.inputDevice);
            }
        }
        if (controllerLeft.enableInputActions && character != null)
        {
            if (characterManager.isAlive)
            {
                CheckFormMovment(controllerLeft.inputDevice);
            }
        }
    }

    private void CheckFormMovment(InputDevice device)
    {
        if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 joystickDirection) && characterManager.canMove)
        {
            CalculateDirection(joystickDirection);

            MoveCharacter();

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
        Vector3 movment = currentDirection * speed;

        character.SimpleMove(movment);

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

   /* private void JumpInput(InputDevice device)
    {

        if (device.TryGetFeatureValue(CommonUsages.secondaryButton, out bool isPressing))
        {
            if (isPressing && !characterManager.isJumping && characterManager.canMove)
            {
                characterManager.Jump();
            }
        }
    }*/
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
}
