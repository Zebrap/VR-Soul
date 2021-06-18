using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class HidenGlowFade : MonoBehaviour
{
    private XRGrabInteractable xRGrabInteractable;
    private struct ShaderPropIds
    {
        public int _EmissionSign;
    }
    private ShaderPropIds shaderPropIds;
    private Material myMaterial;

    private float emissionAlpha = 0;
    private float lerp = 0;

    public float lerpScale = 0.01f;

    private float maxEmission = 1f;
    private float minEmission = 0f;

    void Start()
    {
        xRGrabInteractable = GetComponent<XRGrabInteractable>();
        var rendered = GetComponent<MeshRenderer>();
        myMaterial = Instantiate(rendered.sharedMaterial);
        rendered.material = myMaterial;

        shaderPropIds = new ShaderPropIds()
        {
            _EmissionSign = Shader.PropertyToID("_EmissionSign"),
        };

        myMaterial.SetFloat(shaderPropIds._EmissionSign, emissionAlpha);
    }
    private void OnDestroy()
    {
        if (myMaterial != null)
        {
            Destroy(myMaterial);
        }
    }

    IEnumerator FadeIn()
    {
        lerp = 0;
        while (myMaterial.GetFloat(shaderPropIds._EmissionSign) < maxEmission)
        {
            emissionAlpha = Mathf.Lerp(myMaterial.GetFloat(shaderPropIds._EmissionSign), maxEmission, lerp);
            lerp += Time.deltaTime * lerpScale;
            myMaterial.SetFloat(shaderPropIds._EmissionSign, emissionAlpha);
            yield return null;
        }
    }
    IEnumerator FadeOut()
    {
        lerp = 0;
        while (myMaterial.GetFloat(shaderPropIds._EmissionSign) > minEmission)
        {
            emissionAlpha = Mathf.Lerp(myMaterial.GetFloat(shaderPropIds._EmissionSign), minEmission, lerp);
            lerp += Time.deltaTime * lerpScale;
            myMaterial.SetFloat(shaderPropIds._EmissionSign, emissionAlpha);
            yield return null;
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
}
