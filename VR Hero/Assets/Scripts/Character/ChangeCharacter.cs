using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChangeCharacter : MonoBehaviour
{
    public GameObject currentPlayer;
    public XRCharacterController XRController;

    public Soul soul;

    private void Start() {
        XRController = GetComponent<XRCharacterController>();
        ChangeCharacterObject(false, currentPlayer);
        soul.NewTarget(currentPlayer.transform);
    }

    public void ChangeToNewCharacter(GameObject selectedCharacter)
    {
        if(currentPlayer!=null) ChangeCharacterObject(true, currentPlayer);
        ChangeCharacterObject(false, selectedCharacter);
        currentPlayer = selectedCharacter;
        soul.NewTarget(currentPlayer.transform);
    }

    private void ChangeCharacterObject(bool isAi, GameObject characterObject){
        
        // active capsule
        characterObject.GetComponent<CapsuleCollider>().enabled = isAi;
        // disable character controller
        characterObject.GetComponent<CharacterController>().enabled = !isAi;

        // disable XR character controller
       // characterObject.GetComponent<XRCharacterController>().enabled = !isAi;
        if(!isAi){
            XRController.SetCharacter(characterObject.GetComponent<Animator>(), characterObject.GetComponent<CharacterController>(), characterObject.GetComponent<CharacterManager>(),characterObject.transform);
        }

        // active Enemy Controller
        characterObject.GetComponent<EnemyController>().enabled = isAi;

        // active navMesh agent
        characterObject.GetComponent<NavMeshAgent>().enabled = isAi;
    /*
        characterObject.layer = isAi ?  LayerMask.NameToLayer("Enemy") :  LayerMask.NameToLayer("Ally");

        if(isAi){
            characterObject.GetComponent<CharacterManager>().enemyLayers = LayerMask.GetMask("Ally");
        }else
        {
            characterObject.GetComponent<CharacterManager>().enemyLayers = LayerMask.GetMask("Enemy");
        }*/
    }
}
