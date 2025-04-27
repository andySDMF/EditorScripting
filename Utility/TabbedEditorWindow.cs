using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BrandLab360.Editor
{
    public class TabbedEditorWindow : EditorWindow
    {
        protected int tabIndex = 0;
        protected string windowTitle = "Tabbed Editor Window";
        protected List<TabData> tabs = new List<TabData>();
        protected static Vector2 scrollPosition = Vector2.zero;
        
        private int tabCache = 0;

        private void OnGUI()
        {
            OnRenderGUI();

            if (tabCache != tabIndex)
            {
                tabCache = tabIndex;
                scrollPosition = Vector2.zero;
            }
        }

        protected virtual void OnRenderGUI()
        {
            OnRenderTitle();
            OnRenderTabs();
            tabs[tabIndex].Callback.Invoke();
        }

        protected virtual void OnRenderTitle()
        {
            GUILayout.Space(15);
            GUILayout.Label(windowTitle, CustomEditorStyles.h1);
            GUILayout.Space(10);
        }

        protected virtual void OnRenderTabs()
        {
            GUILayout.Space(10);
            renderTabs();

            CustomEditorStyles.DrawHorizontalLine(CustomEditorStyles.bl360Color);
        }

        private void renderTabs()
        {
            GUILayoutOption tabWidth = GUILayout.Width(this.position.width / tabs.Count);
            GUILayout.BeginHorizontal();

            for (int i =0; i<tabs.Count; i++)
            {
                renderTab(i, tabs[i].Label, tabWidth);
            }

            GUILayout.EndHorizontal();
        }

        private void renderTab(int tabIndex, string tabName, GUILayoutOption tabWidth)
        {
            if(tabIndex < 0 || tabIndex > tabs.Count-1) { return; }

            if (GUILayout.Toggle(getTabState(tabIndex), tabName, CustomEditorStyles.getSelectedTabStyle(getTabState(tabIndex)), tabWidth))
            {
                this.tabIndex = tabIndex;
            }
        }

        private bool getTabState(int index)
        {
            if (index == tabIndex) return true;

            return false;
        }
    }

    public class TabData
    {
        public string Label { get; private set; }
        public System.Action Callback { get; private set; }

        public TabData(string label, System.Action callback)
        {
            Label = label;
            Callback = callback;
        }
    }
}