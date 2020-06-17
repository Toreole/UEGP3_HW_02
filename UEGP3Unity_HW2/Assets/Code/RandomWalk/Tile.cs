using UnityEngine;

namespace UEGP3.RandomWalk
{
    public class Tile 
    {
        private SpriteRenderer renderer;
        private bool state = false;

        public Tile(SpriteRenderer r)
        {
            renderer = r;
        }

        public bool State 
        {
            get => state;
            set => state = value;
        }

        public GameObject GameObject => renderer.gameObject;

        public void SetColor(Color c) => renderer.color = c;

    }
}