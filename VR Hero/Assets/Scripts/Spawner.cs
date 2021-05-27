using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform targetForCharacters;

    public ArmyResources armyResources;

    private void Start()
    {
        SpawnCharacter();
    }

    public void SpawnCharacter()
    {
        Transform prefab = armyResources.GetCharacterToSpawn();
        if(prefab!=null){
            Transform character = Instantiate(prefab, transform);
            character.GetComponent<CharacterManager>().OnDieEvent += NextSpawn;
            if(targetForCharacters!=null)
                character.gameObject.GetComponent<AIController>().SetMyTargetTransform(targetForCharacters);
        }
    }

    public void NextSpawn(){
        SpawnCharacter();
    }


}
