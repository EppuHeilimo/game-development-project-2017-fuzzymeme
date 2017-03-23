using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EntryPoint : MonoBehaviour
{
    public enum Compass
    {
        NORTH,
        EAST,
        SOUTH,
        WEST
    }

    private GameObject mainCamera;
    public Transform cameraTarget;
    public EntryPoint otherSidePoint;
    private GameObject player;
    private bool locked = false;


    //ignore up axis, y = z
    private Vector2 center;
    public Compass compassDirection;
    private Terrain parentTerrain;
    // Use this for initialization
    void Start () {

        FindParentTerrain();
		player = GameObject.FindGameObjectWithTag("Player");
	    cameraTarget = transform.FindChild("CameraTarget");
	    mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        Vector2 localCenter = new Vector2(parentTerrain.terrainData.size.x / 2, parentTerrain.terrainData.size.z / 2);
        center = new Vector2(parentTerrain.terrainData.size.x / 2 + parentTerrain.transform.position.x, parentTerrain.terrainData.size.z / 2 + parentTerrain.transform.position.z);
        Vector2 entrypointcoords = new Vector2(transform.position.x, transform.position.z);

        Vector2 heading = entrypointcoords - center;
        Vector2 direction = heading/heading.magnitude;

        float angle = Mathf.Atan2(direction.x, direction.y);
        int quadrant = Mathf.RoundToInt( 4 * angle / (2 * Mathf.PI) + 4) % 4;

        compassDirection = (Compass)quadrant;

        /*
        Vector2 north = new Vector2(center.x, center.y + localCenter.y);
        Vector2 east = new Vector2(center.x + localCenter.y, center.y);
        Vector2 south = new Vector2(center.x, center.y - localCenter.y);
        Vector2 west = new Vector2(center.x - localCenter.x, center.y);
        */

    }

    void FindParentTerrain()
    {    
        Transform parent = transform.root;  
        parentTerrain = parent.GetComponent<Terrain>();
    }


    void OnDrawGizmosSelected()
    {
        if (otherSidePoint.transform != null)
        {
            Debug.DrawLine(transform.position, otherSidePoint.transform.position, Color.blue, 100f);
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && otherSidePoint != null && !locked)
        {
            Debug.Log("Teleported to " + transform.parent.parent.name);
            player.GetComponent<NavMeshAgent>().Warp(otherSidePoint.transform.position);
            otherSidePoint.locked = true;
            mainCamera.GetComponent<CameraMovement>().ToEntryPoint(cameraTarget, otherSidePoint);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            locked = false;
        }
            
    }
}
