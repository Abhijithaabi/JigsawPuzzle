using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteUtils
{
    public static Sprite CreateSpriteFromTexture2D(
        Texture2D SpriteTexture,
        int x,
        int y,
        int w,
        int h,
        float PixelsPerUnit = 1.0f,
        SpriteMeshType spriteType = SpriteMeshType.Tight)
    {
        Sprite NewSprite = Sprite.Create(
            SpriteTexture,
            new Rect(x, y, w, h),
            new Vector2(0, 0),
            PixelsPerUnit,
            0,
            spriteType);
        return NewSprite;
    }

    public static Texture2D LoadTexture(string resourcePath)
    {
        Texture2D tex = Resources.Load<Texture2D>(resourcePath);
        return tex;
    }
}

/*
The first method, "CreateSpriteFromTexture2D", takes in several parameters such as a Texture2D (the sprite texture), the x and y coordinates of the sprite, the width and height of the sprite, 
the number of pixels per unit, and the type of sprite mesh. This method 
uses the UnityEngine.Sprite class's Create method to create a new Sprite
 from the given texture, using the given x, y, width, and height values 
 to define the portion of the texture that the sprite should use. It
  also sets the sprite's pivot point to (0,0) and assigns the given 
  pixels per unit and sprite mesh type. Finally, it returns the created 
  sprite.


The second method, "LoadTexture", takes in a string resourcePath and
 uses Unity's Resources.Load method to load a Texture2D from the given
  path in the project's Resources folder and returns the loaded Texture2D.
*/