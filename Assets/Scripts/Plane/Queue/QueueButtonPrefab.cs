using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueButtonPrefab : MonoBehaviour
{
    public void DequeueButton()
    {
        PlaneProductManager.instance.DequeuePlane();

    }
}
