using UnityEngine;
using System.Collections;
 
public class CameraFacingBillboard : MonoBehaviour
{
    public Camera m_Camera;


    void Start()
    {
        m_Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void Update()
    {
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
            m_Camera.transform.rotation * Vector3.up);
    }
    //void Update()
    //{
    //    //transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
    //    //    m_Camera.transform.rotation * Vector3.up);


    //    Vector3 player = transform.parent.localRotation.eulerAngles;

    //    Quaternion quaternion = Quaternion.Euler(-player[0], -player[1], -player[2]);

    //    transform.rotation = quaternion;
    //    //Vector3 current = transform.localRotation.eulerAngles;
    //    //Vector3 diff = player -current ;
    //    //transform.Rotate(diff);

    //    //Vector3 eulerAngles1 = transform.parent.rotation.eulerAngles;
    //    //GameObject parentGO = transform.parent.gameObject;
    //    //float f = parentGO.transform.rotation.y;

    //    //Vector3 vector3 = new Vector3(-eulerAngles[0], -eulerAngles[1], -eulerAngles[2]);

    //    //transform.rotation.se
    //    //if (vector3 != transform.rotation.eulerAngles)
    //    //{
    //    //    vector3 = vector3 - transform.rotation.eulerAngles;
    //    //    transform.Rotate(vector3);
    //    //}
       

    //}
}