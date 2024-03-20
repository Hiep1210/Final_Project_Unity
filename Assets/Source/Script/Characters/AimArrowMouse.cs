using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimArrowMouse : MonoBehaviour
{
    public Camera mainCamera;


    // Update is called once per frame
    void Update()
    {
        if (mainCamera != null)
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 mousePosToWorld = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, mainCamera.nearClipPlane));
            transform.position = mousePosToWorld;
        }
    }
}
