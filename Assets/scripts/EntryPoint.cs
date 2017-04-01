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
    private GameObject blockingÓbject;
    private NavMeshObstacle obstacle;

    private VinesAnimation vineAnim;
    private GameObject vine;
    
    //ignore up axis, y = z
    private Vector2 center;
    public Compass compassDirection;
    private Terrain parentTerrain;
    public Transform playerTeleportPoint;
    // Use this for initialization
    void Start ()
    {
        obstacle = GetComponent<NavMeshObstacle>();
        FindParentTerrain();
		player = GameObject.FindGameObjectWithTag("Player");
	    cameraTarget = transform.FindChild("CameraTarget");
	    mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        blockingÓbject = transform.FindChild("Tree").gameObject;
        vine = transform.FindChild("Vines").gameObject;
        vineAnim = vine.GetComponent<VinesAnimation>();
        playerTeleportPoint = transform.FindChild("PlayerTeleportPoint");
        
        Vector2 localCenter = new Vector2(parentTerrain.terrainData.size.x / 2, parentTerrain.terrainData.size.z / 2);
        center = new Vector2(parentTerrain.terrainData.size.x / 2 + parentTerrain.transform.position.x, parentTerrain.terrainData.size.z / 2 + parentTerrain.transform.position.z);
        Vector2 entrypointcoords = new Vector2(transform.position.x, transform.position.z);

        Vector2 heading = entrypointcoords - center;
        Vector2 direction = heading/heading.magnitude;

        float angle = Mathf.Atan2(direction.x, direction.y);
        int quadrant = Mathf.RoundToInt( 4 * angle / (2 * Mathf.PI) + 4) % 4;

        compassDirection = (Compass)quadrant;

        switch (compassDirection)
        {
                case Compass.EAST:
                cameraTarget.position = new Vector3(transform.position.x + 40f, transform.position.y, transform.position.z);
                break;
                case Compass.NORTH:
                cameraTarget.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 40f);
                break;
                case Compass.SOUTH:
                cameraTarget.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 40f);
                break;
                case Compass.WEST:
                cameraTarget.position = new Vector3(transform.position.x - 40f, transform.position.y, transform.position.z);
                break;
        }

    }

    void FindParentTerrain()
    {    
        Transform parent = transform.root;  
        parentTerrain = parent.GetComponent<Terrain>();
    }

    void OnDrawGizmosSelected()
    {

    }

    public void OpenPath()
    {
        if (vine.activeSelf)
        {
            vineAnim.Open();
            obstacle.enabled = false;
        }
    }

    public void ClosePath()
    {
        if (vine.activeSelf)
        {
            vineAnim.Close();
            obstacle.enabled = true;
        }
    }

    public void Init(bool isEntry)
    {
        blockingÓbject.SetActive(!isEntry);
        vine.SetActive(isEntry);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && otherSidePoint != null && !locked)
        {
            player.GetComponent<NavMeshAgent>().Warp(otherSidePoint.playerTeleportPoint.position);
            mainCamera.GetComponent<CameraMovement>().ToEntryPoint(cameraTarget, otherSidePoint);
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().SetCurrentArea(otherSidePoint.parentTerrain.GetComponent<Level>());
        }
    }

    void OnTriggerExit(Collider other)
    {
            
    }
}
