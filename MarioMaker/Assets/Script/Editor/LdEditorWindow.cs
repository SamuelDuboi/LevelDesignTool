using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using SamUsual.Editor;
using SamUsual.Common;

namespace TileTool{

    public class LdEditorWindow : EditorWindow      
    {
        //scriptable references
        Ld ld;
        SerializedObject serializedObject;

        //constante inspector width
        private readonly int inspectorWith = 250;
        //get the inital size of the window
        private Vector2 initialSizeTile 
        {
            get
            {
               return new Vector2(256 , 64);
            } 
        }

        //inital positions of the first tile and of the scroll bar
        private Vector2 sizeTile = Vector2.zero,
                        mainScrollBar,
                        inspectorScrollBar;

        // border of the custom button, only to make it look prettier
        private int borderSize = 2,
        // distance that the mouse as to drag in a tile to consider it selected within the rect drag
                    selectionTolerance = 32,
        // number maximum of buttons in the inspector if their is more, the rect will have a scroll
                    inspectorButtonHeightFactor = 12,
        // number of custome tiles set in this ld
                    currentCustomTileNumber=0;
        // will stock the rect of the inspector and the dimension of any button, the eraser is the default one
        private Rect inspectorRect,eraserRect;

        // set the size of the window editor in tile number
        private Rect ViewRect
        {
            get
            {
                return new Rect(new Vector2(0,32), sizeTile*12 );
            }
        }

        //set the rect that the main scroll bar will act on (editor window size - inspector size)
        private Rect ScrollViewRect
        {
            get
            {
                return new Rect(0, 0, position.width -inspectorWith, position.height);
            }
        }

        // rect use during the drag event
        private List<Rect> rectToDraw = new List<Rect>();

        //stock ever tile type to be able to draw them 
        private List <int> indexToDraw = new List<int>();

        //every texture use to draw the editor window
        public Texture2D background, inspectorText, warningText, warningBorderText, selectText, inspectorBox, inspectorBoxSelected, erasertext;

        //stock the current event to detect mouse click from mous drag
        private Event e;

        //if true, a warning window will appear on the editor window
        private bool warningPopUp;

        //stock the index of custome tile that will be drawing, actualisze for every tile
        private int currentCustomTileIndex = 0;

        Vector2 mousePosOnClick, currentMousePos;

        public void Init(  in Ld ld)
        {
            this.ld = ld;
            serializedObject = new UnityEditor.SerializedObject(ld);
            var win = EditorWindow.GetWindow(typeof(LdEditorWindow));
            background = ld.background;
            sizeTile = initialSizeTile;
            sizeTile.x = initialSizeTile.x * (ld.screenNumber + 1);
            erasertext = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Art/cursor/EraserCursor.png", typeof(Texture2D));
            currentCustomTileNumber = CursorBehavior.ChangeCursor(0, ld.newTiles, erasertext, true);


            ld.RefreshTiles();

            InitialiseLD();

            win.Show();
        }


    
        private void OnGUI()
        {
            //init what could not be set in init
            minSize = new Vector2(Screen.width * 9 / 10, Screen.height * 9 / 10);
            inspectorText = UsualFunction.MakeTex(inspectorWith, Screen.height, Color.gray);
            warningText = UsualFunction.MakeTex(400, 200, Color.gray);
            warningBorderText = UsualFunction.MakeTex(400 + borderSize*2,200 + borderSize*2, Color.red);
            inspectorBox = UsualFunction.MakeTex(inspectorWith - 20, (int)(position.height - 20) / inspectorButtonHeightFactor, new Color(0.2f, 0.2f, 0.2f));
            inspectorBoxSelected = UsualFunction.MakeTex(inspectorWith - 20, (int)(position.height - 20) / inspectorButtonHeightFactor, new Color(0.3f, 0.3f, 0.3f));


            e = Event.current;


            DrawInspector();

            DrawGridCanvas();

            ChoseInput();

            if (warningPopUp)
            {
                DrawWarning();
            }

            MouseHandler();

       

            Repaint();
            EditorUtility.SetDirty(ld);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }


        /// <summary>
        /// initialise ld values from scriptable object information
        /// </summary>
        private void InitialiseLD()
        {
            rectToDraw = new List<Rect>();
            indexToDraw = new List<int>();
            for (int y = 0; y < 13; y++)
            {
                for (int x = 0; x < 48 * (ld.screenNumber + 1); x++)
                {
                    var newRect = new Rect((x - 1) * 64, (y - 1) * 64, 64, 64);
                    var customTile = ld.tiles[y * 48 * (ld.screenNumber + 1) + x].tileType;
                    //look into every tile, if one is not empty, stock its cutom tile index in index to draw and its position in rect to draw
                    if (customTile != null)
                    {
                        rectToDraw.Add(newRect);
                        for (int i = 0; i < ld.newTiles.Count; i++)
                        {
                            if (customTile == ld.newTiles[i])
                            {
                                indexToDraw.Add(i);
                                break;
                            }
                        }
                    }

                }
            }

        }


        /// <summary>
        /// draw the window on the right that display the ld informations
        /// </summary>
        private void DrawInspector()
        {
            inspectorRect = new Rect(position.width -inspectorWith, 30, inspectorWith, position.height - 20/*for better scrolling navigation*/);

            GUI.Box(inspectorRect, inspectorText);

            var buttonInspectorRect = new Rect(inspectorRect.x, inspectorRect.y, inspectorRect.width, inspectorRect.height * (currentCustomTileNumber +8) / inspectorButtonHeightFactor);
            //place all the button in a scroll view
            inspectorScrollBar = GUI.BeginScrollView(inspectorRect, inspectorScrollBar, buttonInspectorRect,false,true);

                DrawInspectorButton();


            //create the button at the bottom of the inspector, after the custome tiles button
                var SaveSceneRect = new Rect(inspectorRect.x + 10, (buttonInspectorRect.height)-inspectorScrollBar.y -300  , inspectorWith * 0.9f, 20);
                if (GUI.Button(SaveSceneRect, "SaveScene"))
                {
                    NewSceneEditor.CreateNewScene(ld);
                }
                var buttonRect = new Rect(inspectorRect.x +10 , SaveSceneRect.y + SaveSceneRect.height +10, (inspectorWith / 2)*0.87f, 20);
                if (GUI.Button(buttonRect, "Expend"))
                {
                    AddScreen();
                }
                var removButtonRect = new Rect(buttonRect.x + buttonRect.width + 10, buttonRect.y, buttonRect.width, buttonRect.height);
                if(GUI.Button(removButtonRect, "Remove"))
                {
                    warningPopUp = true;
                }


            GUI.EndScrollView();
        }

        /// <summary>
        /// Draw a button for each custom tiles set in the serialised object
        /// </summary>
        private void DrawInspectorButton()
        {
            //draw the eraser rect that is set by default and use it as a template for each other tile button
            eraserRect = new Rect(inspectorRect.x + 10, inspectorRect.y + 10, inspectorRect.width - 20, inspectorRect.height / inspectorButtonHeightFactor);
            if (GUI.Button(eraserRect, inspectorBox))
            {
                currentCustomTileNumber = CursorBehavior.ChangeCursor(0, ld.newTiles, erasertext, true);
                currentCustomTileIndex = ld.newTiles.Count;
            }
            var EraserRecTex = new Rect(eraserRect.x + eraserRect.width - 72, eraserRect.y + eraserRect.height / 3, 32, 32);
            var eraserRectText = new Rect(eraserRect.x + 20, eraserRect.y + eraserRect.height / 3, 300, 32);
            GUI.Label(eraserRect, "Eraser");
            GUI.DrawTexture(eraserRect, erasertext, ScaleMode.ScaleToFit);

            //draw custom button and set the currentCustomTileIndex to the button that is pressed 
            for (int i = 0; i < ld.newTiles.Count; i++)
            {
                if (ld.newTiles[i] != null)
                {
                    var newTileRect = new Rect(inspectorRect.x + 10, inspectorRect.y + 25 + inspectorRect.height * (i + 2) / inspectorButtonHeightFactor, inspectorRect.width - 20, inspectorRect.height / inspectorButtonHeightFactor);
                    if (GUI.Button(newTileRect, inspectorBox))
                    {
                        currentCustomTileIndex = i;
                        currentCustomTileNumber = CursorBehavior.ChangeCursor(i, ld.newTiles);
                    }
                    var newTileRecTex = new Rect(newTileRect.x + newTileRect.width - 72, newTileRect.y + newTileRect.height / 3, 32, 32);
                    var newTileRecText = new Rect(newTileRect.x + 20, newTileRect.y + newTileRect.height / 3, 300, 32);
                    GUI.Label(newTileRecText, ld.newTiles[i].name);
                    GUI.DrawTexture(newTileRecTex, ld.newTiles[i].texture, ScaleMode.ScaleToFit);
                }
            }
        }


        /// <summary>
        /// Draw all the editor windonw exept for the inspector window
        /// </summary>
        private void DrawGridCanvas()
        {
            //the rect will be within a scroll view to ease the navigation
            mainScrollBar = GUI.BeginScrollView(ScrollViewRect, mainScrollBar, ViewRect);
            //Draw background
            for (int i = 0; i < ld.screenNumber + 1; i++)
            {
                var rect = new Rect(i * ViewRect.width / (ld.screenNumber+1), 32, ViewRect.width / (ld.screenNumber + 1), ViewRect.height);
                GUI.DrawTexture(rect, background, ScaleMode.ScaleToFit);
            }

          
            UsualEditorFunction.DrawGrid(64, .5f, Color.white, ViewRect);

            DrawLD();
            //UsualEditorFunction.DrawGrid(nodeSize.x, nodeSize.y, 50, .5f, Color.white, ViewRect);
            GUI.EndScrollView();
        }

        /// <summary>
        /// change the custome tile chosen if the player use it shortcut
        /// </summary>
        private void ChoseInput()
        {
            EditorGUIUtility.AddCursorRect(ViewRect, MouseCursor.CustomCursor);

            if (e.type == EventType.KeyDown)
            {
                for (int i = 0; i < ld.newTiles.Count; i++)
                {
                    if (e.keyCode == ld.newTiles[i].shortCut)
                    {
                        currentCustomTileIndex = i;
                        CursorBehavior.ChangeCursor(i, ld.newTiles);
                    }
                }
            }

        }

        #region LdGlobalAttributeButton
        /// <summary>
        /// if the player pressed "Remove" draw a warning that ask him if he is sur becaus its a destructive action
        /// </summary>
        private void DrawWarning()
        {
        
            var _popUpRect = new Rect(Screen.width / 3, Screen.height / 3, 400, 200);
            var borderRect = new Rect(_popUpRect.x - borderSize, _popUpRect.y - borderSize, _popUpRect.width + borderSize*2, _popUpRect.height + borderSize*2);
            GUI.Box(borderRect, warningBorderText);
            GUI.Box(_popUpRect, warningText);

            var labelRect = new Rect(_popUpRect.x + _popUpRect.width/5, _popUpRect.y + 10, _popUpRect.width / 0.5f, _popUpRect.height / 2);

            GUI.Label(labelRect, "The tiles in the last panel will be destroy. \nAre you sure you want to delete a screen ?");
            var yesButtonRect = new Rect(_popUpRect.x + 10, _popUpRect.y + _popUpRect.height * 3 / 4 - 10, _popUpRect.width / 2.2f, _popUpRect.height / 4);
            if (GUI.Button(yesButtonRect, "Yes"))
            {
                warningPopUp = false;
                RemoveScreen();
            }
            var noButtonRect = new Rect(yesButtonRect.x + yesButtonRect.width + 10 , yesButtonRect.y, yesButtonRect.width,yesButtonRect.height);

            if (GUI.Button(noButtonRect, "No"))
            {
                warningPopUp = false;
            }


        }
        /// <summary>
        /// expend the size of the tile map
        /// </summary>
        private void AddScreen()
        {

            ld.screenNumber++;
            sizeTile.x = initialSizeTile.x * (ld.screenNumber + 1);
            ld.ChangeTileNumber();
        }
        /// <summary>
        /// reduce the size of the tile map
        /// </summary>
        private void RemoveScreen()
        {
            if (ld.screenNumber > 0 )
            {
                ld.screenNumber--;
                sizeTile.x = initialSizeTile.x * (ld.screenNumber + 1);
                ld.ChangeTileNumber();
            }
        }

        #endregion
        private void MouseHandler()
        {
            var selectionRect = new Rect(mousePosOnClick.x  , mousePosOnClick.y  , currentMousePos.x, currentMousePos.y);
            if (e.type == EventType.MouseDown)
            {
                mousePosOnClick = e.mousePosition;
                currentMousePos = Vector2.zero;
                for (int y = 0; y < 13; y++)
                {
                    for (int x = 0; x < 48*(ld.screenNumber+1); x++)
                    {
                  
                        if(e.mousePosition.x +mainScrollBar.x< x*64 && e.mousePosition.x +mainScrollBar.x> (x-1)*64 && e.mousePosition.y < y*64-32 && e.mousePosition.y> (y - 1) * 64-32)
                        {
                            #region checkBoxType
                            var currentRect= (new Rect((x - 1) * 64 , (y - 1) * 64 , 64, 64));
                      
                            if (currentCustomTileIndex < ld.newTiles.Count)
                            {
                                if (!rectToDraw.Contains(currentRect))
                                {
                                    ld.ChangeTileType(x, y, ld.newTiles[currentCustomTileIndex],rectToDraw,indexToDraw,currentCustomTileIndex) ;
                                    rectToDraw.Add(currentRect);
                                    indexToDraw.Add(currentCustomTileIndex);
                                    InitialiseLD();
                                }
                                else
                                {
                                    int index = rectToDraw.IndexOf(currentRect);

                                    if (ld.ChangeTileType(x, y, ld.newTiles[currentCustomTileIndex], rectToDraw, indexToDraw, index))
                                    {
                                        indexToDraw[index] = currentCustomTileIndex;
                                        

                                    }
                                    else
                                    {
                                        rectToDraw.RemoveAt(index);
                                        indexToDraw.RemoveAt(index);
                                    }
                                    InitialiseLD();
                                }
                            }
                            else
                            {
                                int index = rectToDraw.IndexOf(currentRect);
                                ld.ChangeTileType(x, y, ld.newTiles[0], rectToDraw, indexToDraw, index,false,true);

                                if(index != -1)
                                {
                                    rectToDraw.RemoveAt(index);
                                    indexToDraw.RemoveAt(index);
                                }    
                                    InitialiseLD();

                            }


                            #endregion
                        }
                    }
                }
            }
            if(e.type == EventType.MouseDrag)
            {
                currentMousePos += e.delta;

            }
            if(e.type == EventType.MouseUp)
            {
                mousePosOnClick= Vector2.zero;
                currentMousePos = Vector2.zero;
            }
                DrawSelection(selectionRect);

        }

        private void DrawSelection(Rect selectionRect)
        {
            if(selectionRect.width != 0 && selectionRect.height != 0)
            {
                EditorGUI.DrawRect(selectionRect, new Color(0.5f, 0.5f, 0.5f, 0.5f));
                for (int y = 0; y < 13; y++)
                {
                    for (int x = 0; x < 48 * (ld.screenNumber + 1); x++)
                    {

                        if ((x-1)*64+selectionTolerance> selectionRect.x +mainScrollBar.x && x *64 -selectionTolerance < selectionRect.x+ selectionRect.width + mainScrollBar.x && (y-1)*64-32 +selectionTolerance> selectionRect.y  && y*64-32 -selectionTolerance <selectionRect.y +selectionRect.height 
                            || (x - 1) * 64 - selectionTolerance < selectionRect.x + mainScrollBar.x && x * 64 + selectionTolerance > selectionRect.x + selectionRect.width + mainScrollBar.x && (y - 1) * 64 - 32  < selectionRect.y && y * 64 - 32  > selectionRect.y + selectionRect.height
                            || (x - 1) * 64 - selectionTolerance < selectionRect.x + mainScrollBar.x && x * 64 + selectionTolerance > selectionRect.x + selectionRect.width + mainScrollBar.x && (y - 1) * 64 - 32 + selectionTolerance > selectionRect.y && y * 64 - 32 - selectionTolerance < selectionRect.y + selectionRect.height
                            || (x - 1) * 64 + selectionTolerance > selectionRect.x + mainScrollBar.x && x * 64 - selectionTolerance < selectionRect.x + selectionRect.width + mainScrollBar.x && (y - 1) * 64 - 32  < selectionRect.y && y * 64 - 32  > selectionRect.y + selectionRect.height)
                        {
                            var currentRect = (new Rect((x - 1) * 64, (y - 1) * 64, 64, 64));

                            if (currentCustomTileIndex < ld.newTiles.Count)
                            {
                                ld.ChangeTileType(x, y, ld.newTiles[currentCustomTileIndex], rectToDraw, indexToDraw, currentCustomTileIndex, true);
                                if (!rectToDraw.Contains(currentRect))
                                {
                                    rectToDraw.Add(currentRect);
                                    indexToDraw.Add(currentCustomTileIndex);
                                }
                                else
                                {
                                    int index = rectToDraw.IndexOf(currentRect);
                                    indexToDraw[index] = currentCustomTileIndex;
                                }

                                InitialiseLD();

                            }
                            else
                            {
                                int index = rectToDraw.IndexOf(currentRect);
                                ld.ChangeTileType(x, y, ld.newTiles[0], rectToDraw, indexToDraw, index, false, true);
                                if(index != -1)
                                {
                                    rectToDraw.RemoveAt(index);
                                    indexToDraw.RemoveAt(index);
                                }
                                    InitialiseLD();

                            }
                        }
                    }
                }
            }


        }

        /// <summary>
        /// draw a tile of type index to draw in the rect rectToDraw, indexToDraw and rectToDraw have the ame tile reference for the same index
        /// </summary>
        private void DrawLD()
        {
            for (int i = 0; i < rectToDraw.Count; i++)
            {
                int xOfTile = (int)rectToDraw[i].x / 64 + 1;
                int yOfTile = (int)rectToDraw[i].y / 64 + 1;
                if(!ld.tiles[yOfTile * 48 * (ld.screenNumber + 1) + xOfTile].isTileUnder)
                    GUI.DrawTexture(rectToDraw[i], ld.newTiles[indexToDraw[i]].texture, ScaleMode.ScaleToFit);
                else
                {
                    GUI.DrawTexture(rectToDraw[i], ld.newTiles[indexToDraw[i]].textureUnder, ScaleMode.ScaleToFit);
                }

            } 
        }


    }
}