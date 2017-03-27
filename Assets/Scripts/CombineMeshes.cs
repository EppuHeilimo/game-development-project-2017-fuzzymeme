using UnityEngine;
using System.Collections;
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

/*
 * Combines children's meshes to one mesh.
 * Give this component to parent and set texture
 * removes all the old meshes
 * 
 * Gives significant performance boost.
 * 
 * DON'T USE IF MESHES REQUIRE INDIVIDUAL FUNCTIONS LIKE TARGETING
 * 
 */

public class CombineMeshes : MonoBehaviour
{

    public void combineMeshes()
    {
        //Save transformation information
        Vector3 position = gameObject.transform.position;
        gameObject.transform.position = Vector3.zero;
        Quaternion rotation = gameObject.transform.rotation;
        gameObject.transform.rotation = Quaternion.identity;

        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        while (i < meshFilters.Length)
        {
            if (meshFilters[i].sharedMesh != null)
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            }
            meshFilters[i].gameObject.SetActive(false);
            if (meshFilters[i].gameObject.transform.childCount == 0)
            {
                Destroy(meshFilters[i].gameObject);
            }
            i++;
        }
        gameObject.transform.GetComponent<MeshFilter>().mesh = new Mesh();
        gameObject.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine, true, true);
        gameObject.transform.gameObject.SetActive(true);

        //Reset transform
        gameObject.transform.position = position;
        gameObject.transform.rotation = rotation;

        //Add collider to mesh
        //gameObject.AddComponent<BoxCollider>();
    }
}