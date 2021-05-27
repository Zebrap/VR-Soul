using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChangeCharacter : MonoBehaviour
{
    public GameObject currentPlayer;
    public XRCharacterController XRController;

    public Soul soul;

    private void Start()
    {
        XRController = GetComponent<XRCharacterController>();
        if (currentPlayer != null)
        {
            ChangeCharacterObject(false, currentPlayer);
            soul.NewTarget(currentPlayer.transform);
        }
    }

    public void ChangeToNewCharacter(GameObject selectedCharacter)
    {
        if (currentPlayer != null) ChangeCharacterObject(true, currentPlayer);
        ChangeCharacterObject(false, selectedCharacter);
        currentPlayer = selectedCharacter;
        soul.NewTarget(selectedCharacter.transform);
    }

    private void ChangeCharacterObject(bool isAi, GameObject characterObject)
    {

        if (characterObject.GetComponent<CharacterManager>().isAlive)
        {
            // active capsule
            characterObject.GetComponent<CapsuleCollider>().enabled = isAi;

            // disable character controller
            characterObject.GetComponent<CharacterController>().enabled = !isAi;

            // active Enemy Controller
            characterObject.GetComponent<AIController>().enabled = isAi;

            // active navMesh agent
            characterObject.GetComponent<NavMeshAgent>().enabled = isAi;
        }

        // set XR character controller
        if (!isAi)
        {
            XRController.SetCharacter(characterObject.GetComponent<Animator>(), characterObject.GetComponent<CharacterController>(), characterObject.GetComponent<CharacterManager>(), characterObject.transform);
        }
    }
}
