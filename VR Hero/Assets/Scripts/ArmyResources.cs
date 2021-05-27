using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmyResources : MonoBehaviour
{
    public Text textCount;
    public Transform[] prefabCharacter;

    private int army = 20;
    public int CountArmy
    {
        get { return army; }
        set
        {
            army = value;
            textCount.text = army.ToString();
        }
    }

    private void Start() {
        
            textCount.text = army.ToString();
    }

    public Transform GetCharacterToSpawn()
    {
        if (CountArmy > 0)
        {
            CountArmy--;
            return prefabCharacter[Random.Range(0, prefabCharacter.Length)];
        }
        return null;
    }
}
