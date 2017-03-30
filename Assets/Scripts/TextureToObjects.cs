using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextureToObjects : MonoBehaviour
{
    [Serializable]
    public class RGBColor
    {
        public byte r;
        public byte g;
        public byte b;

        public RGBColor()
        {
            r = 0;
            g = 0;
            b = 0;
        }
        public RGBColor(byte red, byte green, byte blue)
        {
            r = red;
            g = green;
            b = blue;
        }

        public override bool Equals(object obj)
        {
            RGBColor temp = (RGBColor)obj;
            return this.r == temp.r && this.g == temp.g && this.b == temp.b;
        }
    }

    [Serializable]
    public class ColorObject
    {
        public RGBColor color;
        public GameObject prefab;
        public GameObject parent;

        public ColorObject(RGBColor color, GameObject prefab)
        {
            this.color = color;
            this.prefab = prefab;
            parent = null;
        }

        public ColorObject(RGBColor color, GameObject prefab, GameObject parent)
        {
            this.color = color;
            this.prefab = prefab;
            this.parent = parent;
        }
    }

    public List<ColorObject> ColorsToObjects;

    private GameObject terrain;
    private Vector2 TerrainSize;

    public Texture2D Texture;
    private byte[] rawTextureData;

    private Vector3[,] TerrainGrid;

    private GameObject trees;


	// Use this for initialization
	void Start ()
	{
	    terrain = GameObject.FindGameObjectWithTag("Area");
	    trees = terrain.transform.FindChild("Trees").gameObject;
        TerrainSize.x = terrain.GetComponent<Terrain>().terrainData.size.x / 2;
        TerrainSize.y = terrain.GetComponent<Terrain>().terrainData.size.z / 2;

        TerrainGrid = new Vector3[(int)TerrainSize.x, (int)TerrainSize.y];

	    rawTextureData = Texture.GetRawTextureData();


	    for (int i = 0; i < TerrainSize.x; i++)
	    {
	        for (int j = 0; j < TerrainSize.y; j++)
	        {
	            TerrainGrid[i, j].x = i * 1.75f;
                TerrainGrid[i, j].z = j * 1.75f;
            }
	    }
	    int count = 0;
	    int index = 0;

	    RGBColor[] rawTextureColor = new RGBColor[(int)TerrainSize.x * (int)TerrainSize.y];

	    for (int i = 0; i < rawTextureData.Length; i += 3)
	    {
            rawTextureColor[count] = new RGBColor();
            rawTextureColor[count].r = rawTextureData[i];
            rawTextureColor[count].g = rawTextureData[i+1];
            rawTextureColor[count].b = rawTextureData[i+2];
            count++;
        }

	    count = 0;
        for (int y = 0; y < TerrainSize.y; y++)
        {
            for (int x = 0; x < TerrainSize.x; x++)
            {
                foreach (ColorObject ctob in ColorsToObjects)
                {
                    if (rawTextureColor[count].Equals(ctob.color))
                    {
                        GameObject go = Instantiate(ctob.prefab, TerrainGrid[x, y], Quaternion.identity);
                        if (ctob.parent != null)
                        {
                            go.transform.parent = trees.transform;
                        }
                    }
                }
                count++;
            }
        }    
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
