using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BrandLab360.Editor
{
    public static class ToolboxButtons
    {
        public static EditorButtonData[] ButtonsCore = new EditorButtonData[]
        {
            new EditorButtonData("Spawn Point", Resources.Load<Texture2D>("TB-Spawn"), CreateSpawnPointEditor.CreateSpawn),
            new EditorButtonData("Label", Resources.Load<Texture2D>("TB-Label"), CreateLabelCanvasEditor.Create2DLabel),
            new EditorButtonData("Music", Resources.Load<Texture2D>("TB-Music"), CreateGenericsEditor.CreateMusic)
        };

        public static EditorButtonData[] ButtonsTriggers = new EditorButtonData[]
        {
            new EditorButtonData("Interactive", Resources.Load<Texture2D>("TB-Interact"), CreateGenericsEditor.CreateInteractiveTrigger),
            new EditorButtonData("Analytics", Resources.Load<Texture2D>("TB-Analytics"), CreateAnalyticsTriggerEditor.CreateTrigger),
            new EditorButtonData("Hint", Resources.Load<Texture2D>("TB-Hint"), CreateGenericsEditor.CreateHintTrigger),
            new EditorButtonData("Popup", Resources.Load<Texture2D>("TB-Popup"), CreateGenericsEditor.CreatePopupTrigger),
            new EditorButtonData("Audio", Resources.Load<Texture2D>("TB-Audio"), CreateGenericsEditor.CreateAudioTrigger),
            new EditorButtonData("Switch Scene", Resources.Load<Texture2D>("TB-Exit"), CreateSwitchSceneTriggerEditor.CreateTrigger),
            new EditorButtonData("Voice Area", Resources.Load<Texture2D>("TB-Mic"), CreateVoiceTriggerEditor.Create),
            new EditorButtonData("Placement", Resources.Load<Texture2D>("TB-Placement"), CreateProductPlacementTypeEditor.CreateTrigger)
        };

        public static EditorButtonData[] ButtonsTags = new EditorButtonData[]
        {
            new EditorButtonData("Grab", Resources.Load<Texture2D>("TB-Grab"), CreateGenericsEditor.Create3DGrabButton),
            new EditorButtonData("Interact", Resources.Load<Texture2D>("TB-Interact"), CreateGenericsEditor.Create3DInteractButton),
            new EditorButtonData("Information", Resources.Load<Texture2D>("TB-Info"), CreateGenericsEditor.Create3DInformationButton),
            new EditorButtonData("Like", Resources.Load<Texture2D>("TB-Like"), CreateLikeEditor.Create),
            new EditorButtonData("PopupTag Video", Resources.Load<Texture2D>("TB-Tag-Video"), CreatePopupTagEditor.CreateVideo),
            new EditorButtonData("PopupTag Image", Resources.Load<Texture2D>("TB-Tag-Image"), CreatePopupTagEditor.CreateImage),
            new EditorButtonData("PopupTag Web", Resources.Load<Texture2D>("TB-Tag-Web"), CreatePopupTagEditor.CreateWebsite),
            new EditorButtonData("PopupTag Text", Resources.Load<Texture2D>("TB-Tag-Text"), CreatePopupTagEditor.CreateText),
            new EditorButtonData("Switch Scene", Resources.Load<Texture2D>("TB-Exit"), CreateSwitchSceneTriggerEditor.CreateButton)
        };

        public static EditorButtonData[] ButtonsMap = new EditorButtonData[]
        {
            new EditorButtonData("Topdown Camera", Resources.Load<Texture2D>("TB-Top-Cam"), CreateTopDownCameraEditor.Create),
            new EditorButtonData("Topdown Canvas", Resources.Load<Texture2D>("TB-Map"), CreateTopDownCanvasEditor.Create),
            new EditorButtonData("Teleport Point", Resources.Load<Texture2D>("TB-Target"), CreateTeleportEditor.CreatePoint),
            new EditorButtonData("Key Object", Resources.Load<Texture2D>("TB-Key"), CreateTopDownCanvasEditor.CreateKey)
        };

        public static EditorButtonData[] ButtonsProduct = new EditorButtonData[]
        {
            new EditorButtonData("Assortment Wall", Resources.Load<Texture2D>("TB-Wall"), CreateAssortmentEditor.CreateWall),
            new EditorButtonData("Product Wall", Resources.Load<Texture2D>("TB-Wall"), CreateProductPlacementTypeEditor.CreateWall),
            new EditorButtonData("Assortment Table", Resources.Load<Texture2D>("TB-Table"), CreateAssortmentEditor.CreateTable),
            new EditorButtonData("Product Table", Resources.Load<Texture2D>("TB-Table"), CreateProductPlacementTypeEditor.CreateTable),
            new EditorButtonData("Assortment Rail", Resources.Load<Texture2D>("TB-Rail"), CreateAssortmentEditor.CreateRail),
            new EditorButtonData("Product Rail", Resources.Load<Texture2D>("TB-Rail"), CreateProductPlacementTypeEditor.CreateRail)
        };

        public static EditorButtonData[] ButtonsScreens = new EditorButtonData[]
        {
            new EditorButtonData("Video Screen", Resources.Load<Texture2D>("TB-Video"), CreateVideoScreenEditor.Create),
            new EditorButtonData("Upload Screen", Resources.Load<Texture2D>("TB-Upload"), CreateWorldContentScreenEditor.CreateAll)
        };

        public static EditorButtonData[] ButtonsBoards = new EditorButtonData[]
        {
            new EditorButtonData("Pin Board", Resources.Load<Texture2D>("TB-Pinboard"), CreateNoticeBoardEditor.CreatePinboard),
            new EditorButtonData("Noitce Board", Resources.Load<Texture2D>("TB-Noticeboard"), CreateNoticeBoardEditor.CreateNoticeBoard)
        };

        public static EditorButtonData[] ButtonsBot = new EditorButtonData[]
        {
            new EditorButtonData("Mascot 2D", Resources.Load<Texture2D>("TB-Mascot"), CreateMascotEditor.CreateCanvasMascot),
            new EditorButtonData("Bot Spawn Area", Resources.Load<Texture2D>("TB-Spawn"), CreateNPCBotSpawnAreaEditor.Create),
            new EditorButtonData("Static Bot", Resources.Load<Texture2D>("TB-Bot"), CreateNPCStaticBotEditor.Create)
        };

        public static EditorButtonData[] ButtonsMisc = new EditorButtonData[]
        {
            new EditorButtonData("Drop Point", Resources.Load<Texture2D>("TB-Target"), CreatePickupObjectsEditor.Create2DDropPoint),
            new EditorButtonData("Asset Bundle Loader", Resources.Load<Texture2D>("TB-Cloud-Download"), CreateAssetBundleLoaderEditor.CreateAssetBundleLoader)
        };

        public static EditorButtonTable[] CreateButtonTables = new EditorButtonTable[]
        {
            new EditorButtonTable("Core", ButtonsCore, 2, true),
            new EditorButtonTable("Triggers", ButtonsTriggers, 3, true),
            new EditorButtonTable("3D Buttons", ButtonsTags, 3, true),
            new EditorButtonTable("Map", ButtonsMap, 2, true),
            new EditorButtonTable("Product Collections", ButtonsProduct, 2, true),
            new EditorButtonTable("Content Screens", ButtonsScreens, 2, true),
            new EditorButtonTable("Noticeboards", ButtonsBoards, 2, true),
            new EditorButtonTable("Bots", ButtonsBot, 2, true),
            new EditorButtonTable("Misc", ButtonsMisc, 2, true)
        };
    }
}