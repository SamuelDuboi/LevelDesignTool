using System.Collections.Generic;
using UnityEngine;


namespace TileTool
{

    [System.Serializable]
    [CreateAssetMenu(fileName = "NewLD", menuName = "LD", order = 12)]
    public class Ld : ScriptableObject
    {
        public Texture2D background;

        //nuber of expension use on this ld , one expension as 48 tiles of width
        [SerializeField] public int screenNumber = 0;

        //for some reason I cant save a 2d array
        [SerializeField] public Tile[] tiles = new Tile[48 * 13];

        //path of the scene in the asset data base
        [SerializeField] public string scenePath;

        //name of the scriptable that will also be the name of the scene
        [SerializeField] public string LdName;

        [SerializeField] public List<CustomeTile> newTiles;

        /// <summary>
        /// replace every tiles in oldTiles in tiles with their emplacement. Break when the old tiles have been all replace or when the tiles are full (it destruct the previous tiles)
        /// </summary>
        public void ChangeTileNumber()
        {
            var oldTiles = tiles;
            tiles = new Tile[48 * (screenNumber + 1) * 13];
            RefreshTiles();
            for (int y = 0; y < 13; y++)
            {

                for (int i = 0; i < oldTiles.Length / 13; i++)
                {
                    if (oldTiles[y * oldTiles.Length / 13 + i] != null && tiles[y * tiles.Length / 13 + i] != null)
                    {
                        tiles[y * tiles.Length / 13 + i] = oldTiles[y * oldTiles.Length / 13 + i];
                    }
                    else
                        break;
                }

            }

        }


        /// <summary>
        /// actualise the good number of tile that need to be drawn, change when the user press Expand or Remove
        /// </summary>
        public void RefreshTiles()
        {
            for (int y = 0; y < 13; y++)
            {
                for (int x = 0; x < 48 * (screenNumber + 1); x++)
                {
                    if (tiles[y * 48 * (screenNumber + 1) + x] == null)
                        tiles[y * 48 * (screenNumber + 1) + x] = new Tile();
                }
            }

        }

        /// <summary>
        /// will change the tile type every time a grid is press
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>

        public bool ChangeTileType(int x, int y, CustomeTile tile, bool isSelection = false, bool erase = false)
        {
            if (!isSelection)
            {
                if (!erase)
                {
                    if (tiles[y * 48 * (screenNumber + 1) + x].tileType != tile)
                    {
                        tiles[y * 48 * (screenNumber + 1) + x].tileType = tile;
                        return true;
                    }
                    else
                    {
                        tiles[y * 48 * (screenNumber + 1) + x].tileType = null;
                        return false;
                    }

                }
                else
                {
                    tiles[y * 48 * (screenNumber + 1) + x].tileType = null;
                    return false;
                }
            }
            else
            {
                tiles[y * 48 * (screenNumber + 1) + x].tileType = tile;
                return true;
            }



        }

        /// <summary>
        /// add custom tiles to this ld
        /// </summary>
        public void AddNewTiles()
        {
            if (newTiles == null)
            {
                newTiles = new List<CustomeTile>();
            }
            newTiles.Add(CreateInstance(typeof(CustomeTile)) as CustomeTile);
        }

        /// <summary>
        /// remove custome tile to this ld
        /// </summary>
        public void RemoveNewTiles()
        {
            newTiles.RemoveAt(newTiles.Count - 1);
        }
    }
}