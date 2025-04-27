using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BrandLab360.Editor
{
    public class EditorButtonTable
    {
        private string title;
        private EditorButtonData[] buttons;
        private int columns;
        private bool useIcons;

        public EditorButtonTable(string title, EditorButtonData[] buttons, int columns, bool useIcons)
        {
            this.title = title;
            this.buttons = buttons;
            this.columns = columns;
            this.useIcons = useIcons;
        }

        public void RenderButtonGroup(float windowWidth)
        {
            GUILayout.Space(20);
            EditorGUILayout.BeginVertical(CustomEditorStyles.backgroundBox);
            GUILayout.Space(10);
            GUILayout.Label(title, CustomEditorStyles.h2);
            GUILayout.Space(10);

            float buttonWidth = (windowWidth - 30) / columns;

            for (int i = 0; i < buttons.Length; i += columns)
            {
                GUILayout.BeginHorizontal();
                for (int j = 0; j < columns && (i + j) < buttons.Length; j++)
                {
                    RenderButton(buttons[i + j], buttonWidth);
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.Space(10);
            EditorGUILayout.EndVertical();
        }

        private void RenderButton(EditorButtonData data, float maxWidth)
        {
            int textOffset = 5;
            Rect buttonRect = GUILayoutUtility.GetRect(new GUIContent(data.Label, data.Icon), CustomEditorStyles.tableButton, GUILayout.Width(maxWidth), GUILayout.ExpandWidth(false));

            if (GUI.Button(buttonRect, "", CustomEditorStyles.tableButton))
            {
                data.Callback?.Invoke();
            }

            float iconPadding = 0;

            if (useIcons)
            {
                float iconHeight = CustomEditorStyles.tableButton.fixedHeight - 5;
                float iconWidth = (data.Icon.width / (float)data.Icon.height) * iconHeight;
                Rect iconRect = new Rect(buttonRect.x + textOffset, buttonRect.y + 2.5f, iconWidth, iconHeight);
                if(!EditorGUIUtility.isProSkin) { GUI.color = Color.black; }
                GUI.DrawTexture(iconRect, data.Icon);
                GUI.color = Color.white;
                iconPadding = iconWidth;
                textOffset = 10;
            }
            Rect labelRect = new Rect(buttonRect.x + textOffset + iconPadding + textOffset, buttonRect.y, buttonRect.width - iconPadding - 2 * textOffset, buttonRect.height);
            GUI.Label(labelRect, data.Label);
        }
    }
}