using UnityEngine;
using UnityEditor;
using TileTool;
[CustomEditor(typeof(Ld))]
public class LdEditor : Editor {

	Ld ldEditor;

	private bool newTilesFoldout;

	private void OnEnable() {
		ldEditor = target as Ld;
	}

	
	public override void OnInspectorGUI()
	{
		ldEditor.LdName = EditorGUILayout.TextField("Name", ldEditor.LdName);
		ldEditor.background = (Texture2D)EditorGUILayout.ObjectField("Background",ldEditor.background, typeof(Texture2D), true) ;

		EditorGUILayout.Space(20);
		newTilesFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(newTilesFoldout, "Custom tiles");
		if (newTilesFoldout)
		{
			EditorGUILayout.LabelField("Your prefabs textures need to have a size a 64/64", EditorStyles.helpBox);
			if(ldEditor.newTiles != null)
            {
				for (int i = 0; i < ldEditor.newTiles.Count; i++)
				{
					ldEditor.newTiles[i] = (CustomeTile)EditorGUILayout.ObjectField(ldEditor.newTiles[i], typeof(CustomeTile), true);
					ldEditor.newTiles[i].texture = (Texture2D) EditorGUILayout.ObjectField("",ldEditor.newTiles[i].texture, typeof(Texture2D),true);
				}
            }

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("+"))
			{
				ldEditor.AddNewTiles();
			}
			if (GUILayout.Button("-"))
			{
				ldEditor.RemoveNewTiles();
			}
			EditorGUILayout.EndHorizontal();
		}
		if (ldEditor.background !=null &&  ldEditor.name !=  null)
        {
			if (GUILayout.Button("Open Editor"))
			{
				var win = EditorWindow.GetWindow<LdEditorWindow>(false, "Ld Editor", true) as LdEditorWindow;
				win.Init(ldEditor);
			}

		}
		else
		{
			EditorGUILayout.LabelField("Write a name to open a scene and a background so it can be pretty");
		}



		Repaint(); 
		EditorUtility.SetDirty(ldEditor);
		serializedObject.ApplyModifiedProperties();
		serializedObject.Update();
	}
}
