using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class DeleteGroundPlaneTouchEvent : MonoBehaviour
{
    PlaneFinderBehaviour planeFinderBehaviour = null;

    public void RemovePlaneTouchEvent()
    {
        planeFinderBehaviour.OnInteractiveHitTest = null;
    }
}
