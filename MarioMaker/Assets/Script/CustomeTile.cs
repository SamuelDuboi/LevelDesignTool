using UnityEngine;

namespace TileTool
{
    [CreateAssetMenu(fileName ="NewTile",menuName ="Custom Tile",order =13)]
    public class CustomeTile : ScriptableObject
    {
        public Texture2D texture;
        public Texture2D textureUnder;
        public KeyCode shortCut;
        public bool hasCollider;
        public bool hasUnderSprite;
        
       // public TileBehavior tileBehavior;
    }
}
