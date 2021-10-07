using UnityEngine;
using UnityEditor;

namespace TileTool
{
	[CustomEditor(typeof(CustomeTile))]
	public class CustomTileEditor : Editor
	{
		//reference of the scriptable
		CustomeTile customeTile;

		private void OnEnable()
		{
			customeTile = target as CustomeTile;
		}


		public override void OnInspectorGUI()
		{
			customeTile.shortCut = (KeyCode)EditorGUILayout.EnumPopup("Shortcut ", customeTile.shortCut);
			customeTile.texture = (Texture2D)EditorGUILayout.ObjectField("Texture", customeTile.texture, typeof(Texture2D), true);
            if (customeTile.hasUnderSprite)
            {
				customeTile.textureUnder = (Texture2D)EditorGUILayout.ObjectField("Texture Under", customeTile.textureUnder, typeof(Texture2D), true);
			}
			customeTile.hasUnderSprite = EditorGUILayout.Toggle("Has UnderSprite", customeTile.hasUnderSprite);
			customeTile.hasCollider = EditorGUILayout.Toggle("Has collider", customeTile.hasCollider);
			//customeTile.tileBehavior = (TileBehavior)EditorGUILayout.ObjectField("Script", customeTile.tileBehavior, typeof(TileBehavior), true);
			Repaint();
			EditorUtility.SetDirty(customeTile);
			serializedObject.ApplyModifiedProperties();
			serializedObject.Update();
		}
	}
}