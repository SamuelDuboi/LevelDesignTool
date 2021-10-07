using UnityEngine;

namespace TileTool
{

    [System.Serializable]
    public class Tile 
    {
        [SerializeField]  public CustomeTile tileType;
        [SerializeField] public int index;
        [SerializeField] public bool isTileUnder;

        public Tile()
        {
            tileType = default;
        }
    }
}
