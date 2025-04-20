using UnityEngine;
using UnityEngine.UI;

public class GSC_GraphicElement : GSC_ElementView
{
    public void GetGraphicElement(GSC_ElementView Base)
    {
        if(Base is GSC_GraphicElement graphic)
        {
            GetGraphicFromContainer(graphic.GetContainer());
        }
    }

    private GSC_GraphicContainer GetContainer()
    {
        RawImage rawImage = gameObject.GetComponent<RawImage>();
        if(rawImage != null)
        {
            return new GSC_TextureContainer(rawImage.texture as Texture2D);
        }
        Image image = gameObject.GetComponent<Image>();
        if (image != null)
        {
            return new GSC_SpriteContainer(image.sprite);
        }
        return null;
    }

    public void GetGraphicFromContainer(GSC_GraphicContainer grp)
    {
        if(grp == null) return;
        if(grp is GSC_SpriteContainer spr)
        {
            RawImage rawImage = gameObject.GetComponent<RawImage>();
            if (rawImage != null) Destroy(rawImage);

            Image image = gameObject.GetOrAddComponent<Image>();
            image.sprite = spr._Sprite;
        }
        else if(grp is GSC_TextureContainer tex)
        {
            Image image = gameObject.GetComponent<Image>();
            if (image != null) Destroy(image);

            RawImage rawImage = gameObject.GetOrAddComponent<RawImage>();
            rawImage.texture = tex._Texture;
        }
    }
}
