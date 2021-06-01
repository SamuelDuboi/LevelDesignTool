using UnityEngine;

namespace TileTool
{
    [CreateAssetMenu(fileName ="NewTile",menuName ="Custom Tile",order =13)]
    public class CustomeTile : ScriptableObject
    {
        public Texture2D texture;
        public KeyCode shortCut;
        public bool hasCollider;
       // public TileBehavior tileBehavior;
    }
}
