using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] prefabCharacter;

    public int maxSpawn = 3;
    public int countCharactersInSpawner = 10;

    public Transform targetForCharacters;

    private void Start()
    {
        SpawnCharacter();
    }

    public void SpawnCharacter()
    {
        if (transform.childCount < maxSpawn && countCharactersInSpawner > 0)
        {
            countCharactersInSpawner--;
            Transform character = Instantiate(prefabCharacter[Random.Range(0, prefabCharacter.Length)], transform);
            if (targetForCharacters != null)
                character.gameObject.GetComponent<AIController>().SetMyTargetTransform(targetForCharacters);
        }
    }


}
