using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SamUsual
{
    namespace Editor
    {
        public enum Plan
        {
            Horizontal,
            Vertical
        }
        public static class UsualEditorFunction
        {
            #region Plus Minus Buttons
            /// <summary>
            /// Automatic Set Up of an [+] and a [-] buttons and attribute a funtion to them. Buttons are in Minibutton Style and Horizontal.
            /// </summary>
            /// <param name="plusEffect">Effect of the [+] buttons</param>
            /// <param name="minusEffect">Effect of the [-] buttons</param>
            public static void PlusMinusButtons(Action plusEffect, Action minusEffect)
            {

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("+"))
                {
                    plusEffect();
                }
                if (GUILayout.Button("-"))
                {
                    minusEffect();
                }
                EditorGUILayout.EndHorizontal();
            }

            /// <summary>
            /// Automatic Set Up of an [+] and a [-] buttons and attribute a funtion to them. Buttons are in Minibutton Style.
            /// </summary>
            /// <param name="plusEffect">Effect of the [+] buttons</param>
            /// <param name="minusEffect">Effect of the [-] buttons</param>
            /// <param name="plan">Plan of the mapping of buttons</param>
            public static void PlusMinusButtons(Action plusEffect, Action minusEffect, Plan plan)
            {
                switch (plan)
                {
                    case Plan.Horizontal:
                        EditorGUILayout.BeginHorizontal();
                        break;
                    case Plan.Vertical:
                        EditorGUILayout.BeginVertical();
                        break;
                }

                if (GUILayout.Button("+", EditorStyles.miniButton))
                {
                    plusEffect();
                }
                if (GUILayout.Button("-", EditorStyles.miniButton))
                {
                    minusEffect();
                }

                switch (plan)
                {
                    case Plan.Horizontal:
                        EditorGUILayout.EndHorizontal();
                        break;
                    case Plan.Vertical:
                        EditorGUILayout.EndVertical();
                        break;
                }
            }

            /// <summary>
            /// Automatic Set Up of an [+] and a [-] buttons and attribute a funtion to them. 
            /// </summary>
            /// <param name="plusEffect">Effect of the [+] buttons</param>
            /// <param name="minusEffect">Effect of the [-] buttons</param>
            /// <param name="plan">Plan of the mapping of buttons</param>
            /// <param name="style">Style of buttons</param>
            public static void PlusMinusButtons(Action plusEffect, Action minusEffect, Plan plan, GUIStyle style, string plusName = "+", string minusName = "-")
            {
                switch (plan)
                {
                    case Plan.Horizontal:
                        EditorGUILayout.BeginHorizontal();
                        break;
                    case Plan.Vertical:
                        EditorGUILayout.BeginVertical();
                        break;
                }

                if (GUILayout.Button(plusName, style))
                {
                    plusEffect();
                }
                if (GUILayout.Button(minusName, style))
                {
                    minusEffect();
                }

                switch (plan)
                {
                    case Plan.Horizontal:
                        EditorGUILayout.EndHorizontal();
                        break;
                    case Plan.Vertical:
                        EditorGUILayout.EndVertical();
                        break;
                }
            }

            #endregion
            #region Attribute and Display Serialized Property
            /// <summary>
            /// Return a serialized property and display it as a layout 
            /// </summary>
            /// <param name="serializedObject"></param>
            /// <param name="name">name of the targeted property</param>
            /// <returns>The targeted property</returns>
            public static SerializedProperty AtributeAndDisplayeSerializedProperty(this SerializedObject serializedObject, string name)
            {
                //Attribute
                var _property = serializedObject.FindProperty(name);
                //Display
                EditorGUILayout.PropertyField(_property);

                return _property;
            }
            /// <summary>
            /// Return a serialized property and display it as a layout 
            /// </summary>
            /// <param name="serializedObject"></param>
            /// <param name="name">name of the targeted property</param>
            /// <param label="label">the label displayed on the Editor</param>
            /// <returns>The targeted property</returns>
            public static SerializedProperty AtributeAndDisplayeSerializedProperty(this SerializedObject serializedObject, in string name, string label)
            {
                //Attribute
                var _property = serializedObject.FindProperty(name);
                //Display
                EditorGUILayout.PropertyField(_property, new GUIContent(label));

                return _property;
            }

            /// <summary>
            /// Return a serialized property and display it as a layout 
            /// </summary>
            /// <param name="serializedObject"></param>
            /// <param name="name">name of the targeted property</param>
            /// <returns>The targeted property</returns>
            public static SerializedProperty AtributeAndDisplayeSerializedProperty(this SerializedProperty serializedObject, in string name)
            {
                //Attribute
                var _property = serializedObject.FindPropertyRelative(name);
                //Display
                EditorGUILayout.PropertyField(_property);

                return _property;
            }

            /// <summary>
            /// Return a serialized property and display it as a layout 
            /// </summary>
            /// <param name="serializedObject"></param>
            /// <param name="name">name of the targeted property</param>
            /// <param label="label">the label displayed on the Editor</param>
            /// <returns>The targeted property</returns>
            public static SerializedProperty AtributeAndDisplayeSerializedProperty(this SerializedProperty serializedObject, in string name, string label)
            {
                //Attribute
                var _property = serializedObject.FindPropertyRelative(name);
                //Display
                EditorGUILayout.PropertyField(_property, new GUIContent(label));

                return _property;
            }


            #endregion

            ///<summary>
            /// Create a grid in an inspector nor a window
            ///</summary>
            public static void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor, Rect canevas)
            {

                int widthDivs = Mathf.CeilToInt(canevas.width / gridSpacing);
                int heightDivs = Mathf.CeilToInt(canevas.height / gridSpacing);

                Handles.BeginGUI();

                Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

                for (int i = 1; i < widthDivs; i++)
                {
                    //Define the start and end point of the line and Draw the vertical lines
                    Vector3 _startPointV = new Vector3(gridSpacing * i, -gridSpacing+128, 0);
                    Vector3 _endPointV = new Vector3(gridSpacing * i, canevas.height, 0f);

                    Handles.DrawLine(_startPointV, _endPointV);
                }

                for (int j = 1; j < heightDivs; j++)
                {
                    //Define the start and end point of the line and Draw the horizontal lines
                    Vector3 _startPointV = new Vector3(-gridSpacing, gridSpacing * j, 0);
                    Vector3 _endPointV = new Vector3(canevas.width, gridSpacing * j, 0f);

                    Handles.DrawLine(_startPointV, _endPointV);
                }

                Handles.color = Color.white;
                Handles.EndGUI();
            }
            ///<summary>
            /// Create a grid in an inspector nor a window
            ///</summary>
            public static void DrawGrid(float gridSpacingX, float gridSpacingY, float gridOpacity, Color gridColor, Rect canevas)
            {

                int widthDivs = Mathf.CeilToInt(canevas.width / gridSpacingX);
                int heightDivs = Mathf.CeilToInt(canevas.height / gridSpacingY);

                Handles.BeginGUI();

                Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

                for (int i = 0; i < widthDivs; i++)
                {
                    //Define the start and end point of the line and Draw the vertical lines
                    Vector3 _startPointV = new Vector3(gridSpacingX * i, -gridSpacingY, 0);
                    Vector3 _endPointV = new Vector3(gridSpacingX * i, canevas.height, 0f);

                    Handles.DrawLine(_startPointV, _endPointV);
                }

                for (int j = 0; j < heightDivs; j++)
                {
                    //Define the start and end point of the line and Draw the horizontal lines
                    Vector3 _startPointV = new Vector3(-gridSpacingX, gridSpacingY * j, 0);
                    Vector3 _endPointV = new Vector3(canevas.width, gridSpacingY * j, 0f);

                    Handles.DrawLine(_startPointV, _endPointV);
                }

                Handles.color = Color.white;
                Handles.EndGUI();
            }

            ///<summary>
            /// Create a grid in an inspector nor a window
            ///</summary>
            public static void DrawGrid(float gridSpacingX, float gridSpacingY, float spaceBetCollumnAndLine, float gridOpacity, Color gridColor, Rect canevas)
            {

                int widthDivs = Mathf.CeilToInt(canevas.width / gridSpacingX);
                int heightDivs = Mathf.CeilToInt(canevas.height / gridSpacingY);

                Handles.BeginGUI();

                Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

                for (int i = 0; i < widthDivs; i++)
                {
                    //Define the start and end point of the line and Draw the vertical lines
                    Vector3 _startPointV = new Vector3(gridSpacingX * i + (i + 1) * spaceBetCollumnAndLine, -gridSpacingY, 0);
                    Vector3 _endPointV = new Vector3(gridSpacingX * i + (i + 1) * spaceBetCollumnAndLine, canevas.height, 0f);

                    Handles.DrawLine(_startPointV, _endPointV);

                    //Define the start and end point of the line and Draw the vertical lines
                    Vector3 _startPointVend = new Vector3(gridSpacingX * (i + 1) + (i + 1) * spaceBetCollumnAndLine, -gridSpacingY, 0);
                    Vector3 _endPointVend = new Vector3(gridSpacingX * (i + 1) + (i + 1) * spaceBetCollumnAndLine, canevas.height, 0f);

                    Handles.DrawLine(_startPointVend, _endPointVend);
                }

                for (int j = 0; j < heightDivs; j++)
                {
                    //Define the start and end point of the line and Draw the horizontal lines
                    Vector3 _startPointV = new Vector3(-gridSpacingX, gridSpacingY * j + (j + 1) * spaceBetCollumnAndLine, 0);
                    Vector3 _endPointV = new Vector3(canevas.width, gridSpacingY * j + (j + 1) * spaceBetCollumnAndLine, 0f);

                    Handles.DrawLine(_startPointV, _endPointV);

                    Vector3 _startPointVend = new Vector3(-gridSpacingX, gridSpacingY * (j + 1) + (j + 1) * spaceBetCollumnAndLine, 0);
                    Vector3 _endPointVend = new Vector3(canevas.width, gridSpacingY * (j + 1) + (j + 1) * spaceBetCollumnAndLine, 0f);
                    Handles.DrawLine(_startPointVend, _endPointVend);

                }

                Handles.color = Color.white;
                Handles.EndGUI();
            }


            public static void DrawButtonWithActionLayout(Texture buttonTex, GUIStyle buttonStyle, Action action)
            {
                if (GUILayout.Button(buttonTex, buttonStyle))
                {
                    action.Invoke();
                }
            }
            public static void DrawButtonWithActionLayout<T>(Texture buttonTex, GUIStyle buttonStyle, Action<T> action, T argument)
            {
                if (GUILayout.Button(buttonTex, buttonStyle))
                {
                    action.Invoke(argument);
                }
            }
            public static void DrawButtonWithAction(Rect position, Texture buttonTex, GUIStyle buttonStyle, Action action)
            {
                if (GUI.Button(position, buttonTex, buttonStyle))
                {
                    action.Invoke();
                }
            }
            public static void DrawButtonWithAction<T>(Rect position, Texture buttonTex, GUIStyle buttonStyle, Action<T> action, T argument)
            {
                if (GUI.Button(position, buttonTex, buttonStyle))
                {
                    action.Invoke(argument);
                }
            }

            public static void DrawButtonWithActionLayout(string buttonLabel, GUIStyle buttonStyle, Action action)
            {
                if (GUILayout.Button(buttonLabel, buttonStyle))
                {
                    action.Invoke();
                }
            }
            public static void DrawButtonWithActionLayout<T>(string buttonLabel, GUIStyle buttonStyle, Action<T> action, T argument)
            {
                if (GUILayout.Button(buttonLabel, buttonStyle))
                {
                    action.Invoke(argument);
                }
            }
            public static void DrawButtonWithAction(Rect position, string buttonLabel, GUIStyle buttonStyle, Action action)
            {
                if (GUI.Button(position, buttonLabel, buttonStyle))
                {
                    action.Invoke();
                }
            }
            public static void DrawButtonWithAction<T>(Rect position, string buttonLabel, GUIStyle buttonStyle, Action<T> action, T argument)
            {
                if (GUI.Button(position, buttonLabel, buttonStyle))
                {
                    action.Invoke(argument);
                }
            }

        }
    }
}
