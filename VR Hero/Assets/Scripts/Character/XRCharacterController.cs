using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XRCharacterController : MonoBehaviour
{
    public float speed = 5.0f;

    public Transform head = null;
    public Transform mesh = null;
    public XRController controller = null;

    private Animator animator = null;
    private CharacterController character = null;

    private CharacterManager characterManager;

    private Vector3 currentDirection = Vector3.zero;

    private bool isPressAttack = false;

    public void SetCharacter(Animator animator, CharacterController characterController, CharacterManager characterManager)
    {
        this.animator = animator;
        this.character = characterController;
        this.characterManager = characterManager;
    }

    private void Update()
    {
        if (controller.enableInputActions && character != null)
        {
            if (characterManager.isAlive)
            {
                AttackInput(controller.inputDevice);
                JumpInput(controller.inputDevice);
                CheckFormMovment(controller.inputDevice);
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
            if (isPressing && !characterManager.isJumping && characterManager.canMove)
            {
                characterManager.Jump();
            }
        }
    }
}