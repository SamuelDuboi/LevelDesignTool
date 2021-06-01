using System.Collections.Generic;
using UnityEngine;

namespace TileTool
{
    public class CursorBehavior : MonoBehaviour
    {
        /// <summary>
        /// change the cursor and return an int equal to the custome tile list count
        /// </summary>
        /// <param name="currentype"></param>
        /// <param name="position"></param>
        /// <param name="newTiles"></param>
        /// <param name="isEraser"></param>
        /// <returns></returns>
        public static int ChangeCursor(int index, List<CustomeTile> newTiles, Texture2D text = null, bool isEraser = false)
        {
            if (!isEraser)
            {
                Cursor.SetCursor(newTiles[index].texture, Vector2.zero, CursorMode.Auto);
            }
            else
            {
                Cursor.SetCursor(text, Vector2.zero, CursorMode.Auto);
            }
            return newTiles.Count;
        }


    }
}