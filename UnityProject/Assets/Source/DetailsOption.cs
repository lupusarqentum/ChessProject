using UnityEngine;

public struct DetailsOption
{
    public readonly string DetailsCode;
    public readonly Texture2D Icon;

    public DetailsOption(string detailsCode, Texture2D icon)
    {
        DetailsCode = detailsCode;
        Icon = icon;
    }
}