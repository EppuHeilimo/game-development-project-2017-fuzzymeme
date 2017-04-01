using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Minimap : MonoBehaviour
    {
        private Transform currentArea;
        private Camera fogOfWarCamera;
        private Transform fogOfWarPlane;
        private Camera miniMapCamera;

        // Use this for initialization
        void Start ()
        {
            fogOfWarCamera = transform.FindChild("FogOfWarCamera").GetComponent<Camera>();
            fogOfWarPlane = transform.FindChild("FogOfWarPlane");
            miniMapCamera = GetComponent<Camera>();
        }
	
        // Update is called once per frame
        void Update () {
		
        }

        public void SetArea(Transform area)
        {
            currentArea = area;
            Vector3 size = currentArea.GetComponent<Terrain>().terrainData.size;
            Vector3 center = new Vector3(size.x / 2, 0, size.z / 2);
            transform.position = new Vector3(currentArea.transform.position.x + center.x, 50, currentArea.transform.position.z + center.z);
            miniMapCamera.orthographicSize = size.x / 2;
            fogOfWarCamera.orthographicSize = size.x/2;
            fogOfWarCamera.targetTexture = area.GetComponent<Level>().fogOfWarTexture;
            miniMapCamera.targetTexture = area.GetComponent<Level>().minimapTexture;
            fogOfWarPlane.GetComponent<Renderer>().material.SetTexture("_MainTex", fogOfWarCamera.targetTexture);
            GameObject.FindGameObjectWithTag("CanvasMinimap").GetComponent<RawImage>().texture =
                miniMapCamera.targetTexture;

            Texture2D texture = new Texture2D(100, 100);
        }
    }
}
