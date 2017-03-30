using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeTrees : MonoBehaviour
{
    private Transform[] transforms;
    public Vector2 scaleRange;
    // Use this for initialization
    void Start ()
    {
        scaleRange = new Vector2(0, 0.4f);
        transforms = GetComponentsInChildren<Transform>();
        RandomizeRotation();
	    RandomizeScale();
        //GetComponent<CombineMeshes>().combineMeshes();
	}

    void RandomizeRotation()
    {
        foreach (Transform trans in transforms)
        {
            if(trans.CompareTag("Tree"))
                trans.Rotate(Vector3.up, Random.Range(0.0f, 360.0f));
        }
    }
    void RandomizeScale()
    {
        foreach (Transform trans in transforms)
        {
            if (trans.CompareTag("Tree"))
                trans.localScale = new Vector3(trans.localScale.x + Random.Range(scaleRange.x, scaleRange.y), trans.localScale.y + Random.Range(scaleRange.x, scaleRange.y), trans.localScale.z + Random.Range(scaleRange.x, scaleRange.y));
        }
    }
}
