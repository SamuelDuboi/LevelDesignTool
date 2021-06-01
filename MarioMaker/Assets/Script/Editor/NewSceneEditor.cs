using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;


namespace TileTool
{

    public class NewSceneEditor : Object
    {
        /// <summary>
        /// creat or udpate a scene for the current ediotr window
        /// </summary>
        /// <param name="ld"></param>
        public static void CreateNewScene(Ld ld)
        {
            GameObject parentGameObject;
            GameObject parentBackgrounds;

            UnityEngine.SceneManagement.Scene newScene;
            //creat a new scene if id doesnt exist and add it to the Scene folder
            if (ld.scenePath == null || ld.scenePath == string.Empty)
            {
                newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
                EditorSceneManager.SaveScene(newScene, "Assets/Scene/" + ld.LdName + ".unity");

            }
            //Destroy the main parent of the scne  to creat a new one from scratch
            else
            {
                newScene = EditorSceneManager.OpenScene(ld.scenePath);
                parentGameObject = GameObject.Find("Tiles");
                parentBackgrounds = GameObject.Find("Backgrounds");
                DestroyImmediate(parentGameObject);
                DestroyImmediate(parentBackgrounds);
            }
            parentGameObject = new GameObject("Tiles");
            parentBackgrounds = new GameObject("Backgrounds");
            //instantiate all the tile of the editor window
            var boxNumber = 0;
            for (int y = 0; y < 13; y++)
            {
                for (int x = 0; x < 48 * (ld.screenNumber + 1); x++)
                {
                    if (ld.tiles[y * 48 * (ld.screenNumber + 1) + x].tileType != null)
                    {
                        CreatTiles(ld.tiles[y * 48 * (ld.screenNumber + 1) + x].tileType, boxNumber, parentGameObject, new Vector3(x, 12 - y, 0));
                        boxNumber++;
                    }
                }
            }
            for (int i = 0; i < ld.screenNumber; i++)
            {
                GameObject backGround = new GameObject();
                var sprite=  backGround.AddComponent<SpriteRenderer>();
                var spriteRect = new Rect(0, 0, ld.background.width, ld.background.height);
                sprite.sprite = Sprite.Create(ld.background,spriteRect , Vector2.one * 0.5f, 64);
                sprite.sortingOrder = -1;
                //value founded with playtest
                backGround.transform.position = new Vector3(29.6f + 60f * (i ), 8.22f, 0);
                backGround.transform.localScale = new Vector3(5f,5f, 0);
                if (i < 10)
                    backGround.name = "Background 0" + i.ToString();
                else
                    backGround.name = "Background " + i.ToString();
                backGround.transform.SetParent(parentBackgrounds.transform);
            }
            //save the created scene
            EditorSceneManager.SaveScene(newScene, newScene.path);
            ld.scenePath = newScene.path;

            EditorUtility.SetDirty(ld);
        }

        /// <summary>
        /// create a tile accoring to its custome tile
        /// </summary>
        /// <param name="customeTile"> type of custome tile</param>
        /// <param name="typeNumber">index of current GO of this type </param>
        /// <param name="parrentGameObject"> a parent to link every type so it's not messy</param>
        /// <param name="position"></param>
        /// <param name="ld"></param>
        private static void CreatTiles(CustomeTile customeTile, int typeNumber, GameObject parrentGameObject, Vector3 position)
        {
            var current = new GameObject(customeTile.name + " " + typeNumber.ToString());
            current.transform.SetParent(parrentGameObject.transform);
            current.transform.position = position;
            var sprite = current.AddComponent<SpriteRenderer>();
            var spriteRect = new Rect(0, 0, customeTile.texture.width, customeTile.texture.height);
            sprite.sprite = Sprite.Create(customeTile.texture, spriteRect, Vector2.one * 0.5f, customeTile.texture.width);

            if (customeTile.hasCollider)
            {
                 var collider = current.AddComponent<BoxCollider2D>();
            }
            //never managed to make this work
         /*   if (customeTile.tileBehavior)
            {
                var type = customeTile.tileBehavior.GetType();
                current.AddComponent(type);
            }*/
        }


    }
}