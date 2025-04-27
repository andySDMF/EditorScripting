using System.Collections.Generic;

public static class CoreLayersTags
{
    public static Dictionary<int, string> Layers = new Dictionary<int, string>()
    {
        { 31, "TopDownHidden" },
        { 30, "TopDownVisible" },
        { 29, "NPC" },
        { 28, "Door" },
        { 27, "AvatarConfig" },
        { 26, "Avatar" },
        { 25, "Infotags" },
        { 24, "WheelControllerIgnore" }
    };

    public static List<string> Tags = new List<string>()
    {
        "UI",
        "PureWebInputHandler",
        "AvatarConfiguration",
        "Ball",
        "Target",
        "Road",
        "Vehicle",
        "Environment",
        "Water"
    };
}
