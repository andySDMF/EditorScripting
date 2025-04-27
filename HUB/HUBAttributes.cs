#region Assembly UnityEditor.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files\Unity\Hub\Editor\2021.3.9f1\Editor\Data\Managed\UnityEngine\UnityEditor.CoreModule.dll
#endregion

using System;

namespace BrandLab360.Editor
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class HUBSettingsProviderAttribute : Attribute
    {
        //
        // Summary:
        //     Creates a new HUBSettingsProviderAttribute used to register new HUBSettingsProvider.
        public HUBSettingsProviderAttribute() { }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class HUBSampleAttribute : Attribute
    {
        //
        // Summary:
        //     Creates a new HUBSampleAttribute used to register new HUBSettingsProvider.
        public HUBSampleAttribute() { }
    }
}
