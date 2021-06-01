using UnityEngine;

namespace TileTool
{

    [System.Serializable]
    public class Tile 
    {
        [SerializeField]  public CustomeTile tileType;
        [SerializeField] public int index;

        public Tile()
        {
            tileType = default;
        }
    }
}
