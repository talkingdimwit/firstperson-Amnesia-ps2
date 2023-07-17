using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
namespace CandiceAIforGames.AI.Editors
{
    //[CustomEditor(typeof(CandiceDrop))]
    public class CandiceDrop_Editor : Editor
    {

        private CandiceDrop drop;
        private SerializedObject soTarget;
        private SerializedProperty soDropType;

        GUIStyle guiStyle = new GUIStyle();

        void onEnable()
        {
            drop = (CandiceDrop)target;
            soTarget = new SerializedObject(drop);
            soDropType = soTarget.FindProperty("dType");
            guiStyle.fontSize = 14;
            guiStyle.fontStyle = FontStyle.Bold;
        }

        public override void OnInspectorGUI()
        {
            GUIStyle style = new GUIStyle();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            style.normal.textColor = Color.red;
            Texture2D image = (Texture2D)Resources.Load("CandiceLogo");
            GUIContent label = new GUIContent(image);
            GUILayout.Label(label);

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            label = new GUIContent("Candice Drop");
            guiStyle.normal.textColor = EditorStyles.label.normal.textColor;
            GUILayout.Label(label, guiStyle);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(6);
            EditorGUI.BeginChangeCheck();

            if (EditorGUI.EndChangeCheck())
            {
                GUI.FocusControl(null);
            }
            GUILayout.Space(8);
            GUILayout.BeginVertical("box");
            EditorGUI.BeginChangeCheck();
            GUILayout.Label("Selected Drop Type", guiStyle);

            label = new GUIContent("Selected Drop Type", "The type of drop this is. Health, Damage, Speed boost, or if it is an inventory item.");
            drop.dType = (DropType)EditorGUILayout.EnumPopup(label, drop.dType);

            if (EditorGUI.EndChangeCheck())
            {

            }

            GUILayout.EndVertical();

        }


    }
}