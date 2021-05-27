using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XROffsetGrabInteractable : XRGrabInteractable
{
    private Vector3 initialAttachLocalPOs;
    private Quaternion initialAttachLockaRot;
    private void Start()
    {
        if (!attachTransform)
        {
            GameObject grab = new GameObject("Grab Pivot");
            grab.transform.SetParent(transform, false);
            attachTransform = grab.transform;
        }

        initialAttachLocalPOs = attachTransform.localPosition;
        initialAttachLockaRot = attachTransform.localRotation;
    }

    [System.Obsolete]
    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        if(interactor is XRDirectInteractor)
        {
            attachTransform.position = interactor.transform.position;
            attachTransform.rotation = interactor.transform.rotation;
        }
        else
        {
            attachTransform.position = initialAttachLocalPOs;
            attachTransform.rotation = initialAttachLockaRot;
        }
        base.OnSelectEntered(interactor);
    }
}
