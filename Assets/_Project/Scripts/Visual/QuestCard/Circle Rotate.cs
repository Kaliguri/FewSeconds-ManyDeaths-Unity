using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRotate : MonoBehaviour
{
    [SerializeField] float speedRotate;
    void Update()
    {
        /*
        var eulerAngles = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.y + speedRotate);
        */

        transform.Rotate(0,0,speedRotate);
        /*
        Vector3 pos = gameObject.transform.position;
        pos = new Vector3(pos.x, pos.y, pos.z + speedRotate);

        gameObject.transform.position = pos;
        */
    }
}
