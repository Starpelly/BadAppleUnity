using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Video;

public class BadApple : MonoBehaviour
{
    public GameObject tilePrefab;

    public Vector2 aspectRatio;

    [SerializeField]
    private float tileSize = 1f;

    private List<List<List<int>>> frameValue;
    public List<List<GameObject>> tiles = new List<List<GameObject>>();

    public TextAsset jsonFile;

    public VideoPlayer videoPlayer;

    void Start()
    {
        Application.targetFrameRate = 24;
        frameValue = (List<List<List<int>>>)JsonConvert.DeserializeObject(jsonFile.text, typeof(List<List<List<int>>>));
        GenerateGrid();
    }


    private void GenerateGrid()
    {
        GameObject referenceTile = Instantiate(tilePrefab);

        for (int row = 0; row < aspectRatio.y; row++)
        {
            tiles.Add(new List<GameObject>());
            for (int col = 0; col < aspectRatio.x; col++)
            {
                GameObject tile = Instantiate(referenceTile, transform);
                tile.name = $"{col + 1}, {row + 1}";
                tile.GetComponent<SpriteRenderer>().color = Color.black;
                tiles[row].Add(tile);

                float posX = col * tileSize;
                float posY = row * -tileSize;

                tile.transform.position = new Vector2(posX, posY);
            }
        }

        Destroy(referenceTile);
    }

    void Update()
    {
        UpdateFrame();
    }

    void UpdateFrame()
    {
        int frame = (int)videoPlayer.frame; // OR Time.frameCount
        for (int y = 0; y < frameValue[frame].Count; y++)
        {
            for (int x = 0; x < frameValue[frame][y].Count; x++)
            {
                int binary = frameValue[frame][y][x];
                // turning them on or off is a lot faster than changing the colour
                if (binary == 1)
                    tiles[y][x].SetActive(true);
                else
                    tiles[y][x].SetActive(false);
            }
        }
    }
}
