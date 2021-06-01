using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class InteractObject : MonoBehaviour
{
    public GameObject hoverSign;

    private XRGrabInteractable xRGrabInteractable;

    public bool lerpTransparent = false;
    private List<Material> materials;

    private float alpha = 0;
    private float lerp = 0;
    private byte a;

    private bool isHover;
    private void Start()
    {
        xRGrabInteractable = GetComponent<XRGrabInteractable>();
        materials = new List<Material>();
        foreach (Material material in hoverSign.GetComponent<Renderer>().materials)
        {
            materials.Add(material);
        }
    }

    public void ShowSign()
    {
        StopCoroutine("FadeOut");
        StartCoroutine("FadeIn");
    }

    public void HideSign()
    {
        if (!xRGrabInteractable.isSelected)
        {
            StopCoroutine("FadeIn");
            StartCoroutine("FadeOut");
        }
    }

    IEnumerator FadeIn()
    {
        lerp = 0;
        while (lerp < 5f)
        {
            foreach (Material material in materials)
            {
                Color32 col = material.color;

                alpha = Mathf.Lerp(col.a, 120f, lerp);

                a = (byte)alpha;
                lerp += Time.deltaTime * 0.05f;
                col.a = a;
                material.color = col;
            }
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        lerp = 0;
        while (lerp < 5f)
        {
            foreach (Material material in materials)
            {
                Color32 col = material.color;

                alpha = Mathf.Lerp(col.a, 0f, lerp);

                a = (byte)alpha;
                lerp += Time.deltaTime * 0.1f;
                col.a = a;
                material.color = col;
            }
            yield return null;
        }
    }
}
