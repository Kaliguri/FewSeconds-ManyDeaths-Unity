using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRotate : MonoBehaviour
{
    [SerializeField] float speedRotate;
    [SerializeField] RectTransform lightTransform;
    void Update()
    {
        /*
        var eulerAngles = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.y + speedRotate);
        */

        transform.Rotate(0,0,speedRotate);

        /*
        var tempPos = lightTransform.position;
        lightTransform.position = new Vector3(tempPos.x,tempPos.y, 0);
        */
        
        /*
        Vector3 pos = gameObject.transform.position;
        pos = new Vector3(pos.x, pos.y, pos.z + speedRotate);

        gameObject.transform.position = pos;
        */
    }
}
