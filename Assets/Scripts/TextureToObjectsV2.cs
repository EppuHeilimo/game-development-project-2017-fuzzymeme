using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Script;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

[ExecuteInEditMode]
public class TextureToObjectsV2 : MonoBehaviour
{


    public bool AdaptTerrainSizeToImage = false;
    public bool Create = false;
    public bool Paint = false;
    public bool PaintOnlyBlack = false;

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

    public List<ColorObject> ColorsToObjects = new List<ColorObject>(5);
    private GameObject terrain;

    public float tileSize = 2f;
    public Texture2D Texture;
    private byte[] rawTextureData;

    private Vector3[,] TerrainGrid;
    private int Terrain = 11;


    void Start()
    {
    #if UNITY_EDITOR
        if (EditorApplication.isPlaying) return;

        GameObject trees;
        Transform findChild = gameObject.transform.FindChild("Trees");
        if (findChild == null)
        {
            trees = new GameObject("Trees");
            trees.transform.parent = transform;
        }
        else
        {
            trees = findChild.gameObject;


        }

        var component = GetComponent<Terrain>();
        
        component.materialType = UnityEngine.Terrain.MaterialType.BuiltInLegacyDiffuse;


        GameObject enemySpawnPoints;
        Transform enemySpawnPointsTransform = gameObject.transform.FindChild("EnemySpawnPoints");
        if (enemySpawnPointsTransform == null)
        {
            enemySpawnPoints = new GameObject("EnemySpawnPoints");
            enemySpawnPoints.transform.parent = transform;
        }
        else
        {
            enemySpawnPoints = enemySpawnPointsTransform.gameObject;
        }

        GameObject teddy =Resources.Load<GameObject>("Enemies/Teddy");
        if (enemySpawnPoints.GetComponent<EnemySpawner>() == null)
        {
            EnemySpawner enemySpawner = enemySpawnPoints.AddComponent<EnemySpawner>();
        }
        //enemySpawner.EnemyPrefab = teddy;

        if (ColorsToObjects.Count == 0)
        {
            
            GameObject treePrefab = Resources.Load<GameObject>("Tree");
            GameObject enemySpawnPointPreFab = Resources.Load<GameObject>("SpawnPoint");

            ColorObject colorsToObjectTree = new ColorObject(new RGBColor(0, 0, 0), treePrefab, trees.gameObject);
            colorsToObjectTree.RandomizeRotation = true;
            colorsToObjectTree.RandomizeScale = true;
            colorsToObjectTree.RandomScaleRange = new Vector2(-0.31f,0.15f);
            ColorsToObjects.Add(colorsToObjectTree);

            ColorObject colorsToObject = new ColorObject(new RGBColor(255, 0, 0), enemySpawnPointPreFab, enemySpawnPoints.gameObject);
            ColorsToObjects.Add(colorsToObject);
        }

        if (GetComponent<Level>() == null)
        {
            gameObject.AddComponent<Level>();
        }

        gameObject.tag = "Area";
        gameObject.layer = Terrain;

        LoadAndAddTextures();
#endif
    }


    // Update is called once per frame
    void Update()
    {

#if UNITY_EDITOR
        if (EditorApplication.isPlaying) return;
        if (Create)
        {
            Create = false;
            CreateStuff();
        }


        if (Paint)
        {
            Paint = false;
            PaintTextures(GetColors(), false);
        }
        if (PaintOnlyBlack)
        {
            PaintOnlyBlack = false;
            PaintTextures(GetColors(), true);
        }
        if (AdaptTerrainSizeToImage)
        {
            UpdateTerainSize();
            AdaptTerrainSizeToImage = false;
        }

#endif

    }

    private void UpdateTerainSize()
    {
        if (Texture != null)
        {
            int newTerainWidth = Texture.width*2;
            int newTerainHeight = Texture.height*2;
            Terrain component = GetComponent<Terrain>();
            component.terrainData.size = new Vector3(newTerainWidth,100, newTerainHeight);


        }
    }


    public RGBColor[,] GetColors1()
    {
        var textureData = Texture.GetRawTextureData();
        int width = (int)Math.Sqrt(textureData.Length / 3.0);
        RGBColor[,] rawTextureColor = new RGBColor[width, width];

        for (int z = 0; z < width; z++)
        {

            for (int i = 0; i < width * 3; i += 3)
            {
                int offset = z * width * 3;
                int index = offset + i;
                RGBColor rgbColor = new RGBColor();
                rgbColor.r = textureData[index];
                rgbColor.g = textureData[index + 1];
                rgbColor.b = textureData[index + 2];



                rawTextureColor[(z), i / 3] = rgbColor;


            }

        }

        return rawTextureColor;
    }

    public RGBColor[,] GetColors()
    {
        int width1 = Texture.width;
        int height = Texture.height;


        RGBColor[,] rawTextureColor = new RGBColor[height, width1];

        for (int z = 0; z < height; z++)
        {

            for (int i = 0; i < width1; i++)
            {

                Color pixel = Texture.GetPixel(i,z);

                RGBColor rgbColor = new RGBColor();
                rgbColor.r = (byte)(255 * pixel.r);
                rgbColor.g = (byte)(255 * pixel.g);
                rgbColor.b = (byte)(255 * pixel.b);

                if (rgbColor.r < 10)
                {
                    int b = 4;
                }

                rawTextureColor[(z), i] = rgbColor;


            }

        }

        return rawTextureColor;
    }



    private void CreateStuff()
    {
        Terrain component = GetComponent<Terrain>();
        Vector3 size = component.terrainData.size;
        float xTerrainSize = size.x / tileSize;
        float zTerrainSize = size.z / tileSize;

        TerrainGrid = new Vector3[(int)xTerrainSize, (int)zTerrainSize];

        rawTextureData = Texture.GetRawTextureData();


        for (int i = 0; i < xTerrainSize; i++)
        {
            for (int j = 0; j < zTerrainSize; j++)
            {
                TerrainGrid[i, j].x = i * tileSize + transform.position.x;
                TerrainGrid[i, j].z = j * tileSize + transform.position.z;
            }
        }
        int count = 0;
        int index = 0;

        RGBColor[] rawTextureColor = new RGBColor[(int)xTerrainSize * (int)zTerrainSize];




        RGBColor[,] rgbColors = GetColors();
        var length = rgbColors.GetLength(0);
        var length1 = rgbColors.GetLength(1);
        Debug.Log(length + "            " + length1);
        Debug.Log(zTerrainSize + "     terain       " + xTerrainSize);
        for (int i = 0; i < zTerrainSize; i++)
        {
            for (int x = 0; x < xTerrainSize; x++)
            {
                var rgbColor = rgbColors[i, x];
                rawTextureColor[count] = new RGBColor();
                rawTextureColor[count].r = rgbColor.r;
                rawTextureColor[count].g = rgbColor.g;
                rawTextureColor[count].b = rgbColor.b;
                count++;

            }


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

        List<SplatPrototype> newSplatPrototypes = new List<SplatPrototype>();

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

        var count = allSplatPrototypes.Count;
        for (int i = 0; i < count; i++)
        {
            var splat = array[i];
            if (splat.texture.name.Equals("blackterrain"))
            {
                var splat0 = array[0];
                array[0] = splat;
                array[i] = splat0;

            }

            if (splat.texture.name.Equals("grassseamless"))
            {
                var splat1 = array[1];
                array[1] = splat;
                array[i] = splat1;
            }

        }


        terrainData.splatPrototypes = array;









    }





    private void PaintTextures(RGBColor[,] rawTextureColor, bool repaintOnlyBlack)
    {
        int xLenght = rawTextureColor.GetLength(1);
        int zLenght = rawTextureColor.GetLength(0);


        Terrain terain = GetComponent<Terrain>();
        TerrainData terrainData = terain.terrainData;
        int terrainWidth = terrainData.alphamapWidth;
        int terrainHeight = terrainData.alphamapHeight;

        double xScale = terrainWidth / (double)xLenght;
        double zScale = terrainHeight / (double)zLenght;

        short[,] map = new short[terrainWidth, terrainHeight];

        for (int i = 0; i < terrainWidth; i++)
        {
            for (int j = 0; j < terrainHeight; j++)
            {
                map[i, j] = 1;
            }
        }

        Stack<Coordinate> openList = new Stack<Coordinate>();
        openList.Push(new Coordinate(0, 0));

        Boolean[,] closedList = new Boolean[terrainWidth, terrainHeight];
        for (int i = 0; i < terrainWidth; i++)
        {
            for (int j = 0; j < terrainHeight; j++)
            {
                closedList[i, j] = false;
            }
        }
        int maxSize = terrainHeight * terrainWidth;
        int rounds = 0;
        while (openList.Count != 0)
        {
            rounds++;

            if (openList.Count > maxSize)
            {
                break;
            }
            var coordinate = openList.Pop();
            closedList[coordinate.X, coordinate.Z] = true;

            bool isBlackPixel = IsBlackPixel(rawTextureColor, coordinate, xScale, zScale);

            if (isBlackPixel)
            {
                Debug.Log("black pixel at: " + coordinate.X + ": " + coordinate.Z);
            }
            else
            {
                map[coordinate.Z, coordinate.X] = 0;
                Coordinate x_Minux1 = new Coordinate(coordinate.X - 1, coordinate.Z);
                Coordinate x_Plus1 = new Coordinate(coordinate.X + 1, coordinate.Z);
                Coordinate z_Minux1 = new Coordinate(coordinate.X, coordinate.Z - 1);
                Coordinate z_Plus1 = new Coordinate(coordinate.X, coordinate.Z + 1);

                if (IsValidCoordinate(x_Minux1, terrainWidth, terrainHeight, closedList))
                {
                    openList.Push(x_Minux1);
                }
                if (IsValidCoordinate(x_Plus1, terrainWidth, terrainHeight, closedList))
                {
                    openList.Push(x_Plus1);
                }
                if (IsValidCoordinate(z_Minux1, terrainWidth, terrainHeight, closedList))
                {
                    openList.Push(z_Minux1);
                }
                if (IsValidCoordinate(z_Plus1, terrainWidth, terrainHeight, closedList))
                {
                    openList.Push(z_Plus1);
                }

            }


        }



        float[,,] alphaData = terrainData.GetAlphamaps(0, 0, terrainWidth, terrainHeight);
        var numberOfSplats = terrainData.splatPrototypes.Length;
        for (int z = 0; z < terrainHeight; z++)
        {
            for (int x = 0; x < terrainWidth; x++)
            {

                short val = map[z, x];
                bool repaint = true;
                if (repaintOnlyBlack)
                {
                    repaint = val == 0;
                }

                if (repaint)
                {

                    for (int i = 0; i < numberOfSplats; i++)
                    {
                        alphaData[z, x, i] = 0;
                    }

                    if (val == 0)
                    {
                        alphaData[z, x, 0] = 1;
                    }
                    else
                    {
                        alphaData[z, x, 1] = 1;
                    }
                }
            }
        }

        terrainData.SetAlphamaps(0, 0, alphaData);




    }

    private bool IsBlackPixel(RGBColor[,] rawTextureColor, Coordinate coordinate, double xScale, double zScale)
    {
        int imageX = (int)Math.Floor(coordinate.X / xScale);
        int imageZ = (int)Math.Floor(coordinate.Z / zScale);
        RGBColor rgbColor = rawTextureColor[imageZ, imageX];
        bool blackPixel = rgbColor.r < 10 && rgbColor.b < 10 && rgbColor.g < 10;
        if (imageZ == 4 && imageX == 30)
        {
            int b = 4;
        }
        return blackPixel;
    }

    private bool IsValidCoordinate(Coordinate coordinate, int lenght, int height, Boolean[,] closedList)
    {
        if (coordinate.X < 0 || coordinate.X >= lenght)
        {
            return false;
        }
        if (coordinate.Z < 0 || coordinate.Z >= height)
        {
            return false;
        }
        bool closed = closedList[coordinate.X, coordinate.Z];
        if (closed)
        {
            return false;
        }
        return true;
    }

    public class Coordinate
    {
        protected bool Equals(Coordinate other)
        {
            return X == other.X && Z == other.Z;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Coordinate)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Z;
            }
        }

        public int X { get; set; }
        public int Z { get; set; }

        public Coordinate(int x, int z)
        {
            X = x;
            Z = z;
        }
        public Coordinate()
        {

        }
    }
}
