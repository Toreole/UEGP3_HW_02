using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace UEGP3.RandomWalk
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField]
        Vector2Int gridSize;
        [SerializeField]
        float targetPercentFill;
        [SerializeField]
        GameObject tilePrefab;
        [SerializeField, Range(0.01f, 0.99f)]
        Transform tileHolder;
        [SerializeField, Range(1, 1000)]
        protected float generationFrameRate = 60;
        [SerializeField]
        protected Color activeColor, inactiveColor;
        [SerializeField]
        protected Color tColor = Color.red;
        [SerializeField]
        protected bool useCustomSeed = false;
        [SerializeField]
        protected int seed;

        int filledTiles = 0;
        Vector2Int tPosition;
        Tile[,] grid;
        int totalTiles;

        private void Start() 
        {
            Generate();
        }

        public void Clear()
        {
            foreach(var tile in grid)
            {
                Destroy(tile.GameObject);
            }
            grid = new Tile[1,1];
            tPosition = Vector2Int.zero;
        }

        public void Generate()
        {
            //set up grid.
            grid = new Tile[gridSize.x, gridSize.y];
            totalTiles = gridSize.x * gridSize.y;
            //Instantiate prefabs.
            for(int x = 0; x < gridSize.x; x++)
            {
                for(int y = 0; y < gridSize.y; y++)
                {
                    var inst = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity, tileHolder);
                    inst.transform.localPosition = new Vector3(x, y);
                    Tile t = new Tile(inst.GetComponent<SpriteRenderer>());
                    grid[x, y] = t;
                }
            }
            StartCoroutine(DoSteps());
        }

        IEnumerator DoSteps()
        {
            //init random.
            Random rng = useCustomSeed ? new Random(seed) : new Random();
            //initialize player position
            tPosition = new Vector2Int(rng.Next(0,gridSize.x), rng.Next(0, gridSize.y));
            float simulationTime = Time.time;
            while(filledTiles < totalTiles * targetPercentFill)
            {
                //update last tile.
                var tile = grid[tPosition.x, tPosition.y];
                tile.SetColor(tile.State? activeColor : inactiveColor);

                //Step.
                var dir = rng.Next(0, 4);
                if(dir == 0)
                    tPosition.x++;
                else if(dir == 1)
                    tPosition.y ++;
                else if(dir == 2)
                    tPosition.x--;
                else
                    tPosition.y--;
                tPosition.x = Mathf.Clamp(tPosition.x, 0, gridSize.x -1);
                tPosition.y = Mathf.Clamp(tPosition.y, 0, gridSize.y -1);

                tile = grid[tPosition.x, tPosition.y];
                tile.SetColor(tColor);
                if(!tile.State)
                {
                    filledTiles++;
                    tile.State = true;
                }
                simulationTime += 1f/generationFrameRate;
                if(simulationTime > Time.time)
                    yield return new WaitUntil(() => simulationTime < Time.time);
            }
        }
    }
}