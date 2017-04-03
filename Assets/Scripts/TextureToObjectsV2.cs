using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

[ExecuteInEditMode]
public class TextureToObjectsV2 : MonoBehaviour
{


    public bool Create = false;

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

        public bool RandomizeScale = false;
        public bool RandomizeRotation = false;
        public Vector2 RandomScaleRange = new Vector2(0.0f, 0.4f);

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

    public float tileSize = 2f;
    public Texture2D Texture;
    private byte[] rawTextureData;

    private Vector3[,] TerrainGrid;

    private GameObject trees;





    // Update is called once per frame
    void Update () {

        if (Create)
        {
            Create = false;
            LoadAndAddTextures();
            CreateStuff();
        }
    }


    private void CreateStuff()
    {
        terrain = GameObject.FindGameObjectWithTag("Area");
        Terrain component = terrain.GetComponent<Terrain>();
        float xTerrainSize = component.terrainData.size.x / tileSize;
        float zTerrainSize = component.terrainData.size.z / tileSize;

        TerrainGrid = new Vector3[(int)xTerrainSize, (int)zTerrainSize];

        rawTextureData = Texture.GetRawTextureData();


        for (int i = 0; i < xTerrainSize; i++)
        {
            for (int j = 0; j < zTerrainSize; j++)
            {
                TerrainGrid[i, j].x = i * tileSize;
                TerrainGrid[i, j].z = j * tileSize;
            }
        }
        int count = 0;
        int index = 0;

        RGBColor[] rawTextureColor = new RGBColor[(int)xTerrainSize * (int)zTerrainSize];

        for (int i = 0; i < rawTextureData.Length; i += 3)
        {
            rawTextureColor[count] = new RGBColor();
            rawTextureColor[count].r = rawTextureData[i];
            rawTextureColor[count].g = rawTextureData[i + 1];
            rawTextureColor[count].b = rawTextureData[i + 2];
            count++;
        }

        count = 0;
        for (int y = 0; y < zTerrainSize; y++)
        {
            for (int x = 0; x < xTerrainSize; x++)
            {
                foreach (ColorObject ctob in ColorsToObjects)
                {
                    if (rawTextureColor[count].Equals(ctob.color))
                    {
                        GameObject go = Instantiate(ctob.prefab, TerrainGrid[x, y], Quaternion.identity);
                        if (ctob.RandomizeScale)
                        {
                            go.transform.localScale = new Vector3(go.transform.localScale.x + UnityEngine.Random.Range(ctob.RandomScaleRange.x, ctob.RandomScaleRange.y),
                                 go.transform.localScale.y + UnityEngine.Random.Range(ctob.RandomScaleRange.x, ctob.RandomScaleRange.y),
                                 go.transform.localScale.z + UnityEngine.Random.Range(ctob.RandomScaleRange.x, ctob.RandomScaleRange.y));
                        }
                        if (ctob.RandomizeRotation)
                        {
                            go.transform.Rotate(Vector3.up, UnityEngine.Random.Range(0.0f, 360.0f));
                        }
                        if (ctob.parent != null)
                        {
                            go.transform.parent = ctob.parent.transform;
                        }


                    }
                }
                count++;
            }
        }
    }

    private void LoadAndAddTextures()
    {
        Texture2D[] loadAll = Resources.LoadAll<Texture2D>("Terrain");

        List<Texture2D> normalTextures = new List<Texture2D>();
        List<Texture2D> textures = new List<Texture2D>();
        foreach (var texture in loadAll)
        {
            string textureName = texture.name;
            textureName = textureName.ToLower();

            if (textureName.EndsWith("_normal"))
            {
                normalTextures.Add(texture);
            }
            else
            {
                textures.Add(texture);
            }
        }

        List< SplatPrototype > newSplatPrototypes = new List<SplatPrototype>();

        while (normalTextures.Count != 0)
        {
            Texture2D normalTexture = normalTextures[0];
            normalTextures.Remove(normalTexture);
            string normalTextureName = normalTexture.name;
            var startOfNormal = normalTextureName.IndexOf("_normal");
            string textureName = normalTextureName.Substring(0, startOfNormal);
            Texture2D find = textures.Find(d => d.name.Equals(textureName));
            if (find != null)
            {
                textures.Remove(find);
                SplatPrototype newSplatPrototype = new SplatPrototype();
                newSplatPrototype.texture = find;
                newSplatPrototype.normalMap = normalTexture;
                newSplatPrototypes.Add(newSplatPrototype);

            }
        }
        while (textures.Count != 0)
        {

            SplatPrototype newSplatPrototype = new SplatPrototype();
            newSplatPrototype.texture = textures[0];
            textures.Remove(textures[0]);
            newSplatPrototypes.Add(newSplatPrototype);
           
        }



        Terrain terain = GetComponent<Terrain>();
        TerrainData terrainData = terain.terrainData;

        List<SplatPrototype> allSplatPrototypes = new List<SplatPrototype>();

        SplatPrototype[] prototypes = terrainData.splatPrototypes;
        foreach (var prototype in prototypes)
        {
            string textureName = prototype.texture.name;
            SplatPrototype find = newSplatPrototypes.Find(prototype1 => prototype1.texture.name.Equals(textureName));
            if (find == null)
            {
                allSplatPrototypes.Add(prototype);
            }
            
        }
        allSplatPrototypes.AddRange(newSplatPrototypes);
        SplatPrototype[] array = allSplatPrototypes.ToArray();
    
        terrainData.splatPrototypes = array;
     


   

    }

    void Blub()
    {


        


        // Get the attached terrain component
        Terrain terrain = GetComponent<Terrain>();

        // Get a reference to the terrain data
        TerrainData terrainData = terrain.terrainData;

        // Splatmap data is stored internally as a 3d array of floats, so declare a new empty array ready for your custom splatmap data:
        float[,,] splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        for (int y = 0; y < terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < terrainData.alphamapWidth; x++)
            {
                // Normalise x/y coordinates to range 0-1 
                float y_01 = (float)y / (float)terrainData.alphamapHeight;
                float x_01 = (float)x / (float)terrainData.alphamapWidth;

                // Sample the height at this location (note GetHeight expects int coordinates corresponding to locations in the heightmap array)
                float height = terrainData.GetHeight(Mathf.RoundToInt(y_01 * terrainData.heightmapHeight), Mathf.RoundToInt(x_01 * terrainData.heightmapWidth));

                // Calculate the normal of the terrain (note this is in normalised coordinates relative to the overall terrain dimensions)
                Vector3 normal = terrainData.GetInterpolatedNormal(y_01, x_01);

                // Calculate the steepness of the terrain
                float steepness = terrainData.GetSteepness(y_01, x_01);

                // Setup an array to record the mix of texture weights at this point
                float[] splatWeights = new float[terrainData.alphamapLayers];

                // CHANGE THE RULES BELOW TO SET THE WEIGHTS OF EACH TEXTURE ON WHATEVER RULES YOU WANT

                // Texture[0] has constant influence
                splatWeights[0] = 0.5f;

                // Texture[1] is stronger at lower altitudes
                splatWeights[1] = Mathf.Clamp01((terrainData.heightmapHeight - height));

                // Texture[2] stronger on flatter terrain
                // Note "steepness" is unbounded, so we "normalise" it by dividing by the extent of heightmap height and scale factor
                // Subtract result from 1.0 to give greater weighting to flat surfaces
                splatWeights[2] = 1.0f - Mathf.Clamp01(steepness * steepness / (terrainData.heightmapHeight / 5.0f));

                // Texture[3] increases with height but only on surfaces facing positive Z axis 
                splatWeights[3] = height * Mathf.Clamp01(normal.z);

                // Sum of all textures weights must add to 1, so calculate normalization factor from sum of weights
                float z = splatWeights.Sum();

                // Loop through each terrain texture
                for (int i = 0; i < terrainData.alphamapLayers; i++)
                {

                    // Normalize so that sum of all texture weights = 1
                    splatWeights[i] /= z;

                    // Assign this point to the splatmap array
                    splatmapData[x, y, i] = splatWeights[i];
                }
            }
        }

        // Finally assign the new splatmap to the terrainData:
        terrainData.SetAlphamaps(0, 0, splatmapData);
    }

}
