using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

        public bool ChangeTileType(int x, int y, CustomeTile tile, List<Rect> rectToDraw, List<int> indexToDraw, int index, bool isSelection = false, bool erase = false)
        {
            if (!isSelection)
            {
                if (!erase)
                {
                    if (tiles[y * 48 * (screenNumber + 1) + x].tileType != tile)
                    {
                        tiles[y * 48 * (screenNumber + 1) + x].tileType = tile;
                        tiles[y * 48 * (screenNumber + 1) + x].isTileUnder = false;
                        TryTileUnder(x, y, tile, rectToDraw, indexToDraw, index, isSelection, erase);
                        return true;
                    }
                    else
                    {
                        tiles[y * 48 * (screenNumber + 1) + x].tileType = null;
                        tiles[y * 48 * (screenNumber + 1) + x].isTileUnder = false;
                        if (tile.hasUnderSprite)
                            tiles[(y + 1) * 48 * (screenNumber + 1) + x].isTileUnder = false;
                        for (int i = y - 1; i * 48 * (screenNumber + 1) + x > 0; i--)
                        {
                            if (tiles[i * 48 * (screenNumber + 1) + x].tileType == tile)
                            {
                                tiles[i * 48 * (screenNumber + 1) + x].tileType = null;
                                tiles[i * 48 * (screenNumber + 1) + x].isTileUnder = false;

                            }
                        }
                        return false;
                    }

                }
                else
                {

                    CustomeTile _oldTileTipe = tiles[y * 48 * (screenNumber + 1) + x].tileType;
                    tiles[y * 48 * (screenNumber + 1) + x].tileType = null;
                    tiles[y * 48 * (screenNumber + 1) + x].isTileUnder = false;
                    if (tiles[(y + 1) * 48 * (screenNumber + 1) + x].isTileUnder)
                        tiles[(y +1)* 48 * (screenNumber + 1) + x].isTileUnder = false;


                    for(int i = y - 1; i * 48 * (screenNumber + 1) + x > 0; i--)
                        {
                        if (tiles[i * 48 * (screenNumber + 1) + x].tileType == _oldTileTipe)
                        {
                            tiles[i * 48 * (screenNumber + 1) + x].tileType = null;
                            tiles[i * 48 * (screenNumber + 1) + x].isTileUnder = false;

                        }
                    }
                    return false;
                }
            }
            else
            {

                tiles[y * 48 * (screenNumber + 1) + x].tileType = tile;
                tiles[y * 48 * (screenNumber + 1) + x].isTileUnder = false;

                TryTileUnder(x, y, tile, rectToDraw, indexToDraw, index, isSelection, erase);
                return true;

            }

        }


        public void TryTileUnder(int x, int y, CustomeTile tile, List<Rect> rectToDraw, List<int> indexToDraw, int index, bool isSelection = false, bool erase = false)
        {
            //if there is a tile above
            tiles[(y + 1) * 48 * (screenNumber + 1) + x].isTileUnder = false;
            if ((y - 1) * 48 * (screenNumber + 1) + x < tiles.Length - 1)
            {
                //if the tile above is suppose to have a tile under him
                if (tiles[(y - 1) * 48 * (screenNumber + 1) + x].tileType != null && tiles[(y - 1) * 48 * (screenNumber + 1) + x].tileType.hasUnderSprite)
                {
                    //if they are the same tile type
                    if (tiles[(y - 1) * 48 * (screenNumber + 1) + x].tileType == tile)
                    {
                        for (int i = y; i * 48 * (screenNumber + 1) + x < tiles.Length; i++)
                        {
                            //while the tile under has nothing in it
                            if (tiles[i * 48 * (screenNumber + 1) + x].tileType == null || tiles[i * 48 * (screenNumber + 1) + x].tileType == tile)
                            {
                                tiles[i * 48 * (screenNumber + 1) + x].tileType = tile;
                                tiles[i * 48 * (screenNumber + 1) + x].tileType.hasUnderSprite = tile;
                                tiles[i * 48 * (screenNumber + 1) + x].isTileUnder = true;

                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    //if they are not the same type, delete all the previus cutome tile
                    else
                    {
                        CustomeTile _aboveTile = tiles[(y - 1) * 48 * (screenNumber + 1) + x].tileType;
                        //while there is a tile above with the same tile type delete this type
                        for (float i = y + 1; i * 48 * (screenNumber + 1) + x < tiles.Length; i++)
                        {
                            if (tiles[(int)(i * 48 * (screenNumber + 1) + x)].tileType == _aboveTile)
                            {
                                tiles[(int)(i * 48 * (screenNumber + 1) + x)].tileType = null;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            if (tile.hasUnderSprite)
            {
                CustomeTile _oldTile = tiles[y * 48 * (screenNumber + 1) + x].tileType;

                for (int i = y + 1; i * 48 * (screenNumber + 1) + x < tiles.Length; i++)
                {
                    //while the tile under has nothing in it
                    if (tiles[i * 48 * (screenNumber + 1) + x].tileType == null || tiles[i * 48 * (screenNumber + 1) + x].tileType == _oldTile || tiles[i * 48 * (screenNumber + 1) + x].tileType == tile)
                    {
                        var currentRect = (new Rect((x - 1) * 64, (i - 1) * 64, 64, 64));
                        tiles[i * 48 * (screenNumber + 1) + x].tileType = tile;
                        tiles[i * 48 * (screenNumber + 1) + x].tileType.hasUnderSprite = tile;

                        if (i != y)
                        {
                            tiles[i * 48 * (screenNumber + 1) + x].isTileUnder = true;
                        }

                    }
                    else
                    {
                        break;
                    }
                }
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