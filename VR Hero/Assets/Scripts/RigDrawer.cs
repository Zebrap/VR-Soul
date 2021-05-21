using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.InteractionSubsystems;

public class RigDrawer : MonoBehaviour
{
    public GameObject origin = null;
    public float multipleSize;
    private void OnDrawGizmos()
    {
        if(origin != null){
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(origin.transform.position, 
                new Vector3(origin.transform.localScale.x * multipleSize,
                 origin.transform.localScale.y * multipleSize,
                 origin.transform.localScale.z * multipleSize));
        }
    }
}
