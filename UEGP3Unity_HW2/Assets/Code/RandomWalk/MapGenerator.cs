using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using UEGP3.Core;

namespace UEGP3.RandomWalk
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField]
        protected Vector2Int gridSize;
        [SerializeField, Range(0.01f, 0.99f)]
        protected float targetPercentFill;
        [SerializeField]
        protected GameObject tilePrefab;
        [SerializeField]
        protected Transform tileHolder;
        [SerializeField, Range(1, 10000)]
        protected float generationFrameRate = 60;
        [SerializeField]
        protected Color activeColor, inactiveColor;
        [SerializeField]
        protected Color tColor = Color.red;
        [SerializeField]
        protected bool useCustomSeed = false;
        [SerializeField]
        protected int seed;
        [SerializeField]
        protected bool fillHoles = false;
        [SerializeField]
        protected CheckType checkType = CheckType.Full8Directional;
        [SerializeField]
        protected float flightChance = 0.05f;
        [SerializeField]
        protected Vector2Int flightDistance;

        int filledTiles = 0;
        Vector2Int tPosition;
        Tile[,] grid;
        int totalTiles;

        public bool IsDone => filledTiles >= totalTiles * targetPercentFill;

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

        //YO: This could be done with a single Texture2D / one Sprite, which would be much more optimized.
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
            Random flightRng = useCustomSeed ? new Random(seed) : new Random();
            
            //initialize player position
            tPosition = new Vector2Int(rng.Next(0,gridSize.x), rng.Next(0, gridSize.y));
            float simulationTime = Time.time;
            Vector2Int direction = new Vector2Int(0, 0);
            while(!IsDone)
            {
                //update last tile.
                var tile = grid[tPosition.x, tPosition.y];
                tile.SetColor(tile.State? activeColor : inactiveColor);

                if(flightRng.NextDouble() < flightChance)
                {
                    //random angle between 0 and 360.
                    float angle = (float)flightRng.NextDouble() * 360f;
                    //distance of the flight
                    int distance = flightRng.Next(flightDistance.x, flightDistance.y);
                    //Create vector. 
                    Vector2 flight = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up;
                    flight *= distance;
                    //move the tunnler.
                    tPosition += new Vector2Int((int)flight.x, (int)flight.y);;
                }
                else 
                {
                    //Step.
                    var dir = rng.Next(0, 4);
                    //dir = 0 and dir = 1 will not change the direction.
                    if(dir == 0)
                        tPosition.x++;
                    else if(dir == 1)
                        tPosition.y++;
                    else if(dir == 2)
                        tPosition.x--;
                    else
                        tPosition.y--;
                }

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
            if(fillHoles)
                FillHoles(GetEvaluator());
        }

        //Fills all 1x1 holes in the generation, looks very smooth.
        #if UNITY_EDITOR
        public void FillHoles(System.Func<int, int, bool> evaluator)
        #else
        void FillHoles(System.Func<int, int, bool> evaluator)
        #endif
        {
            //iterate over the entire grid.
            for(int x = 0; x < gridSize.x; x++)
            {
                for(int y = 0; y < gridSize.y; y++)
                {
                    if(!grid[x, y].State)
                    {
                        //check whether this is a 1x1 hole or not.
                        if(evaluator(x , y))
                        {
                            //Debug.Log($"Filled hole at {x}, {y}");
                            grid[x, y].State = true;
                            grid[x, y].SetColor(activeColor);
                        }
                    }
                }
            }
        }

        //Gets the "is this tile a hole" evaluator for the set checkType.
        System.Func<int, int, bool> GetEvaluator()
        {
            //setup evaluator
            if(checkType == CheckType.VerticalCross)
                return CardinalCheck;
            else if(checkType == CheckType.Full8Directional)
                return FullCheck;
            return DiagonalCheck;

        }

#if UNITY_EDITOR
        public bool CardinalCheck(int x, int y)
#else
        bool CardinalCheck(int x, int y)
#endif
        {
            if(x > 0)
                if(!grid[x-1, y].State)
                    return false;
            if(x < gridSize.x-1)
                if(!grid[x+1, y].State)
                    return false;

            if(y > 0)
                if(!grid[x, y-1].State)
                    return false;
            if(y < gridSize.y-1)
                if(!grid[x, y+1].State)
                    return false;
            return true;
        }
        
#if UNITY_EDITOR
        public bool FullCheck(int x, int y)
#else
        bool FullCheck(int x, int y)
#endif
        {
            for(int dx = x-1; dx <= x+1; dx ++)
            {
                if(dx < 0 || dx >= gridSize.x)
                    continue;
                for(int dy = y-1; dy <= y+1; dy++)
                {
                    if(dy < 0 || dy >= gridSize.y)
                        continue;
                    if(dx == x && dy == y)
                        continue;
                    if(!grid[dx, dy].State)
                        return false;
                }
            }
            return true;
        }
        
#if UNITY_EDITOR
        public bool DiagonalCheck(int x, int y)
#else
        bool DiagonalCheck(int x, int y)
#endif
        {
            if(x > 0)
            {
                if(y > 0)
                {
                    if(!grid[x - 1, y -1 ].State)
                        return false;
                }
                if(y < gridSize.y - 1)
                {
                    if(!grid[x - 1, y + 1].State)
                        return false;
                }
            }
            if(x < gridSize.x - 1)
            {
                if(y > 0)
                {
                    if(!grid[x + 1, y - 1].State)
                        return false;
                }
                if(y < gridSize.y - 1)
                {
                    if(!grid[x + 1, y + 1].State)
                        return false;
                }
            }
            return true;
        }

        public enum CheckType 
        {
            Full8Directional, DiagonalCross, VerticalCross
        }
    }
}