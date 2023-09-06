using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using Puzzle;



public class TileGeneration : MonoBehaviour
{
    public string ImageFileName;
    private Texture2D mTextureOriginal;

    // Start is called before the first frame update
    void Start()
    {
        CreateBaseTexture();
        TestTileCurves();
    }

    void CreateBaseTexture()
    {
        // Load the main image.
        mTextureOriginal = SpriteUtils.LoadTexture(ImageFileName);
        if (!mTextureOriginal.isReadable)
        {
            Debug.Log("Texture is not readable");
            return;
        }

        SpriteRenderer spriteRenderer =
      gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = SpriteUtils.CreateSpriteFromTexture2D(
            mTextureOriginal,
            0,
            0,
            mTextureOriginal.width,
            mTextureOriginal.height);

    }

    void TestTileCurves()
    {   Debug.Log("1 43 tileGeneration");
        Puzzle.Tile tile = new Puzzle.Tile(mTextureOriginal);
        tile.DrawCurve(Puzzle.Tile.Direction.UP, Puzzle.Tile.PosNegType.POS, Color.red);
        // tile.DrawCurve(Puzzle.Tile.Direction.UP, Puzzle.Tile.PosNegType.NEG, Color.green);
        // tile.DrawCurve(Puzzle.Tile.Direction.UP, Puzzle.Tile.PosNegType.NONE, Color.white);
    }
    // Update is called once per frame
    void Update()
    {

    }
}

internal class Tile
{
}