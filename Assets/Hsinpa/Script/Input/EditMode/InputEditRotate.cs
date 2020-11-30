using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputEditRotate
{ 
    private Camera _camera;
    private RaycastHit[] raycastHits = new RaycastHit[1];
    private GameObject targetObject;

    bool hasHitOnPuffObj = false;
    Vector3 lastStandPoint;

    public InputEditRotate(Camera p_camera)
    {
        this._camera = p_camera;
    }

    public void SetUp(GameObject targetObject)
    {
        this.targetObject = targetObject;
    }

    #region Device Input Handler
    public void OnUpdate()
    {
        if (!hasHitOnPuffObj && Input.GetMouseButtonDown(0))
        {
            hasHitOnPuffObj = HasHitPuffObject();

            if (hasHitOnPuffObj)
            {
                lastStandPoint = Input.mousePosition;
            }
        }

        if (!hasHitOnPuffObj)
        {
            return;
        }

        ProcessRotation();

        if (Input.GetMouseButtonUp(0))
        {
            hasHitOnPuffObj = false;
        }
    }

    private bool HasHitPuffObject()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        int hitCount = Physics.RaycastNonAlloc(ray, raycastHits, 100, GeneralFlag.Layer.Anchor);

        return hitCount > 0;
    }

    private int ProcessRotation()
    {
        Vector3 currentStandPoint = Input.mousePosition;
        float direction = (currentStandPoint - lastStandPoint).x;
        direction = Mathf.Clamp(direction, -5, 5);

        Vector3 rotation = new Vector3(0, direction, 0);

        targetObject.transform.Rotate(rotation, Space.Self);

        return (direction > 0) ? 1 : -1;
    }
    #endregion
}
