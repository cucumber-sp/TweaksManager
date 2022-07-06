using UnityEngine;
using SFS;
using SFS.Parts;

public static class AllTexturesInSkinMenu_CucumberSP
{
    public static void Load()
    {
string[] tag = new[] { "tank", "fairing", "cone" };
foreach (ColorTexture texture in Base.partsLoader.colorTextures.Values)
    texture.tags = tag;
foreach (ShapeTexture texture in Base.partsLoader.shapeTextures.Values)
    texture.tags = tag;
    }
}
//MainClassName: AllTexturesInSkinMenu_CucumberSP