#region Assembly UnityEditor.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files\Unity\Hub\Editor\2021.3.9f1\Editor\Data\Managed\UnityEngine\UnityEditor.CoreModule.dll
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BrandLab360.Editor
{
    public class HUBProvider
    {
        public HUBProvider(string path) { }

        //
        // Summary:
        //     Gets Domain used to place the SettingsProvider in the tree view of the Settings
        //     window. The path should be unique among all other settings paths and should use
        //     "/" as its separator.
        public string Domain { get; protected set; }
        //
        // Summary:
        //     Gets or sets the display name of the SettingsProvider as it appears in the Settings
        //     window. If not set, the Settings window uses last token of SettingsProvider.settingsPath
        //     instead.
        public string label { get; set; }

        public int Order { get; protected set; }

        public HUBTab Tab { get; protected set; }

        //
        // Summary:
        //     Use this function to implement a handler for when the user clicks on the Settings
        //     in the Settings window. You can fetch a settings Asset or set up UIElements UI
        //     from this function.
        //
        // Parameters:
        //   searchContext:
        //     Search context in the search box on the Settings window.
        //
        //   rootElement:
        //     Root of the UIElements tree. If you add to this root, the SettingsProvider uses
        //     UIElements instead of calling SettingsProvider.OnGUI to build the UI. If you
        //     do not add to this VisualElement, then you must use the IMGUI to build the UI.
        public virtual void OnActivate(string searchContext, VisualElement rootElement) { }
        //
        // Summary:
        //     Use this function to implement a handler for when the user clicks on another
        //     setting or when the Settings window closes.
        public virtual void OnDeactivate() { }
        //
        // Summary:
        //     Use this function to draw the UI based on IMGUI. This assumes you haven't added
        //     any children to the rootElement passed to the OnActivate function.
        //
        // Parameters:
        //   searchContext:
        //     Search context for the Settings window. Used to show or hide relevant properties.
        public virtual void OnGUI(Rect position) 
        { 

        }

        public virtual void OnHeader(Rect position)
        {

        }

        public virtual void OnFooter(Rect position)
        {

        }

    }

}
