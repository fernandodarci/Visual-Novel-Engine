using UnityEngine;

public class GSC_TextureContainer : GSC_GraphicContainer
{
    public Texture2D _Texture;

    public GSC_TextureContainer(Texture2D texture)
    {
        _Texture = texture;
    }
}