using UnityEngine;
using UnityEditor;
using System;

namespace BrandLab360.Editor
{
    public class ToolboxEditor : TabbedEditorWindow
    {
        private bool corePrefabFound = false;
        private EditorButtonTable[] createButtonTables;

        private AppSettings cache;

       // [MenuItem("BrandLab360/Toolbox")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<ToolboxEditor>("Toolbox").minSize = new Vector2(800, 150);
        }

        private void OnEnable()
        {
            this.windowTitle = "BRANDLAB360 Toolbox";

            AppConstReferences appReferences = Resources.Load<AppConstReferences>("AppConstReferences");

            if (appReferences != null)
            {
                cache = appReferences.Settings;
            }
            else
            {
                cache = Resources.Load<AppSettings>("ProjectAppSettings");
            }

            tabs.Clear();
            tabs.Add(new TabData("Create", renderCreateMenu));
            tabs.Add(new TabData("Add", renderAddMenu));

#if BRANDLAB360_GAMES
            tabs.Add(new TabData("Games", renderGames));
#endif

#if BRANDLAB360_VEHICLES
            tabs.Add(new TabData("Vehicles", renderVehicles));
#endif

            createButtonTables = ToolboxButtons.CreateButtonTables;
        }

        private void OnInspectorUpdate()
        {
            if (GameObject.FindFirstObjectByType<CoreManager>() != null) { corePrefabFound = true; }
            else { corePrefabFound = false; }
        }

        protected override void OnRenderGUI()
        {
            //display banner
            if (cache.brandlabLogo_Banner != null)
            {
                GUILayout.Box(cache.brandlabLogo_Banner.texture, GUILayout.ExpandWidth(true));
            }
            else
            {
                cache.brandlabLogo_Banner = Resources.Load<Sprite>("Logos/BrandLab360_Banner");
            }

            if (!corePrefabFound) { RenderInitScene(); return; }

            base.OnRenderGUI();
        }

        private void RenderInitScene()
        {
            GUILayout.Label("Core prefab not found!", CustomEditorStyles.h2);
            GUILayout.Space(10);
            if (GUILayout.Button("Add Core Prefab"))
            {
                CreateCOREObjectsEditor.CreateCORE();
                CreateCOREObjectsEditor.CreateEnvironment();

                var corePrefab = GameObject.FindFirstObjectByType<CoreManager>();
                if (corePrefab == null) { return; }

                corePrefab.transform.SetSiblingIndex(0);
                var envPrefab = GameObject.FindFirstObjectByType<Environment>();

                if (envPrefab == null) { return; }
                envPrefab.transform.SetSiblingIndex(1);
                corePrefab.SceneEnvironment = envPrefab;
            }
        }

        private void renderCreateMenu()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);
            foreach (var buttonTable in createButtonTables)
            {
                buttonTable.RenderButtonGroup(this.position.width);
            }
            GUILayout.Space(10);
            GUILayout.EndScrollView();
        }

        private void renderAddMenu()
        {

        }

#if BRANDLAB360_GAMES
        private void renderGames()
        {

        }
#endif

#if BRANDLAB360_VEHICLES
        private void renderVehicles()
        {

        }
#endif
    }
}