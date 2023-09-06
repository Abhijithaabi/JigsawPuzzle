using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle
{
    public class Tile
    {
        //the padding in pixels for the jigsaw tile
        //we have having each tile of 140 by 140 pixels with 20 pixles padding

        public static int Padding = 20;

        //the actual size of the tile(minus the padding)
        public static int TileSize = 100;
        private Color TransparentColor = new Color(1, 1, 1, 1);


        #region Enumerations
        // The 4 directions
        public enum Direction
        {
            UP,
            RIGHT,
            DOWN,
            LEFT,
        }

        // The operations on each of the four directions.
        public enum PosNegType
        {
            POS,
            NEG,
            NONE,
        }
        #endregion

        //the aray of PosNegType operations to be performed on each of the four direction
        //mPosNeg is a private var that will store the type of operation that we will perform for a given direction.by default operations for all directions is set to NONE.
        private PosNegType[] mPosNeg = new PosNegType[4]
{
    PosNegType.NONE,
    PosNegType.NONE,
    PosNegType.NONE,
    PosNegType.NONE
};
        #region Properties
        public Texture2D FinalCut { get; private set; }
        //FinalCut property lets the user get access to the final texture
        //we only provide get access to this property. That means we cannot modify the FinalCut texture outside of this class.

        #endregion

        private Texture2D mOriginalTex;
        /* 
        the original texture used to create this jigsaw file. we are not going to chnage this original texture.
        instead we are going to create a new texture and set the values to either pixels from the original texture 
        or transparent (if the pixel falls outside the curves or straight lines(defined by enum PostNegType))
        */

        private bool[,] mVisited; //2d boolean array that stores whether a particular pixel is visited. need this array for flood fill

        //these are the control poits for our bezier curve
        //these control points do not change and are marked readonly
        public static readonly List<Vector2> ControlPoints = new List<Vector2>()
{
    new Vector2(0, 0),
    new Vector2(35, 15),
    new Vector2(47, 13),
    new Vector2(45, 5),
    new Vector2(48, 0),
    new Vector2(25, -5),
    new Vector2(15, -18),
    new Vector2(36, -20),
    new Vector2(64, -20),
    new Vector2(85, -18),
    new Vector2(75, -5),
    new Vector2(52, 0),
    new Vector2(55, 5),
    new Vector2(53, 13),
    new Vector2(65, 15),
    new Vector2(100, 0)
};
        private Stack<Vector2Int> mStack = new Stack<Vector2Int>(); //stack needed for the flood fill of the textures.

        //private var to keeo track of all the possible line renderers for this tile using a dictionary
        private Dictionary<(Direction, PosNegType), LineRenderer> mLineRenderers =
            new Dictionary<(Direction, PosNegType), LineRenderer>();
        public static List<Vector2> BezCurve = BezierCurves.PointList2(ControlPoints, 0.001f);


        public void SetPosNegType(Direction dir, PosNegType type)
        {
            mPosNeg[(int)dir] = type;
            //let user set the specific value of operation for a given direction
        }



        //the constructor for tile class
        public Tile(Texture2D tex)
        {  
            //  Debug.Log(" 102 Tile Constructor");
            int tileSizeWithPadding = 2 * Padding + TileSize;
            // Debug.Log(" 104 TileSizeWIthPadding " + tileSizeWithPadding);
            if (tex.width != tileSizeWithPadding ||
              tex.height != tileSizeWithPadding)
            {
                Debug.Log("Unsupported texture dimension for Jigsaw tile");
                return;
            }

            mOriginalTex = tex;
            // Debug.Log(" 113 mOriginalTex " + mOriginalTex);

            // Create a new texture with width and height as Padding + TileSize + Padding.
            FinalCut = new Texture2D(
              tileSizeWithPadding,
              tileSizeWithPadding,
              TextureFormat.ARGB32,
              false);

            // Initialize the newly create texture with transparent colour.
            for (int i = 0; i < tileSizeWithPadding; ++i)
            {
                for (int j = 0; j < tileSizeWithPadding; ++j)
                {
                    // Debug.Log(j + " 127 Tile COntructor " + TransparentColor);
                    FinalCut.SetPixel(i, j, TransparentColor);
                }
            }
        }


        public void Apply()
        {
            FloodFillInit();
            FloodFill();
            FinalCut.Apply();
        }
        public void FloodFillInit()
        {
            // Debug.Log("FloodFillInit 141");
            int tileSizeWithPadding = 2 * Padding + TileSize;

            mVisited = new bool[tileSizeWithPadding, tileSizeWithPadding];
            for (int i = 0; i < tileSizeWithPadding; ++i)
            {
                for (int j = 0; j < tileSizeWithPadding; ++j)
                {
                    mVisited[i, j] = false;
                    //initialise mVisited and set all values to false, means by default all the pixels are not visited
                }
            }

            List<Vector2> pts = new List<Vector2>();
            for (int i = 0; i < mPosNeg.Length; ++i)
            {
                pts.AddRange(CreateCurve((Direction)i, mPosNeg[i]));
                //create a points defined by bezier curves and straight lines depending on what operation set for each of the four directions
            }

            //once we have the list of points, we make all pixels that falls in this point list as mVisited to be true
            //now we should have a closed curve. to verify check by drawing the pts to a line renderer
            for (int i = 0; i < pts.Count; ++i)
            {
                mVisited[(int)pts[i].x, (int)pts[i].y] = true;
            }
            //finally we take the central pixel, mark it as visited and then push it to the stack
            Vector2Int start = new Vector2Int(tileSizeWithPadding / 2, tileSizeWithPadding / 2);

            mVisited[start.x, start.y] = true;
            mStack.Push(start);

        }

        public void FloodFill()
        {
            int width_height = Padding * 2 + TileSize;
            while (mStack.Count > 0)
            {
                //FloodFillStep();
                Vector2Int v = mStack.Pop();

                int xx = v.x;
                int yy = v.y;
                Fill(v.x, v.y);

                // check right.
                int x = xx + 1;
                int y = yy;
                if (x < width_height)
                {
                    Color c = FinalCut.GetPixel(x, y);
                    if (!mVisited[x, y])
                    {
                        mVisited[x, y] = true;
                        mStack.Push(new Vector2Int(x, y));
                    }
                }

                // check left.
                x = xx - 1;
                y = yy;
                if (x >= 0)
                {
                    Color c = FinalCut.GetPixel(x, y);
                    if (!mVisited[x, y])
                    {
                        mVisited[x, y] = true;
                        mStack.Push(new Vector2Int(x, y));
                    }
                }

                // check up.
                x = xx;
                y = yy + 1;
                if (y < width_height)
                {
                    Color c = FinalCut.GetPixel(x, y);
                    if (!mVisited[x, y])
                    {
                        mVisited[x, y] = true;
                        mStack.Push(new Vector2Int(x, y));
                    }
                }

                // check down.
                x = xx;
                y = yy - 1;
                if (y >= 0)
                {
                    Color c = FinalCut.GetPixel(x, y);
                    if (!mVisited[x, y])
                    {
                        mVisited[x, y] = true;
                        mStack.Push(new Vector2Int(x, y));
                    }
                }
            }
        }

        //fill function that sets the actual color value from the original texture to the finalCut texture 
        void Fill(int x, int y)
        {
            Color c = mOriginalTex.GetPixel(x, y);
            c.a = 1.0f;
            FinalCut.SetPixel(x, y, c);
        }

        public static List<Vector2> CreateCurve(Tile.Direction dir, PosNegType type)
        {   
            Debug.Log("248 CreateCurve");
            int padding_x = Padding;
            int padding_y = Padding;
            int sw = TileSize;
            int sh = TileSize;

            List<Vector2> pts = new List<Vector2>(Tile.BezCurve);
            switch (dir)
            {
                case Tile.Direction.UP:
                    if (type == PosNegType.POS)
                    {
                        TranslatePoints(pts, new Vector3(padding_x, padding_y + sh, 0));
                        Debug.Log("261 POS translate");
                    }
                    else if (type == PosNegType.NEG)
                    {
                        InvertY(pts);
                        TranslatePoints(pts, new Vector3(padding_x, padding_y + sh, 0));
                        Debug.Log("267 NEG translate");
                    }
                    else if (type == PosNegType.NONE)
                    {
                        pts.Clear();
                        for (int i = 0; i < 100; ++i)
                        {   
                            pts.Add(new Vector2(i + padding_x, padding_y + sh));
                            // Debug.Log(i + " 275 NONE translate");
                        }
                    }
                    break;
            }
            return pts;
        }

        //create a default LineRenderer
        public static LineRenderer CreateLineRenderer(Color color, float lineWidth = 1.0f)
        {
            GameObject obj = new GameObject();

            LineRenderer lr = obj.AddComponent<LineRenderer>();

            lr.startColor = color;
            lr.endColor = color;
            Debug.Log("3, 289 " + lr.endColor);
            lr.startWidth = lineWidth;
            Debug.Log("4, 291 " + lr.startWidth);
            lr.endWidth = lineWidth;
            Debug.Log("5, 293 " + lr.endWidth);
            lr.material = new Material(Shader.Find("Sprites/Default"));
            Debug.Log("6, 295 " + lr.material);
            Debug.Log("7, 296 CreateLineRenderer");
            return lr;
        }

        //helper method DrawCurve that renders the curve created by the creactecurve method using a linerenderer
        public void DrawCurve(Direction dir, PosNegType type, Color color)
        {
            if (!mLineRenderers.ContainsKey((dir, type)))
            {   
                Debug.Log("2, 300");
                mLineRenderers.Add((dir, type), CreateLineRenderer(color));
            }

            LineRenderer lr = mLineRenderers[(dir, type)];
            lr.startColor = color;
            lr.endColor = color;
            lr.gameObject.name = "LineRenderer_" + dir.ToString() + "_" + type.ToString();
            List<Vector2> pts = Tile.CreateCurve(dir, type);

            lr.positionCount = pts.Count;
            Debug.Log("8, 316 " + pts.Count);
            for (int i = 0; i < pts.Count; ++i)
            {   
                
                lr.SetPosition(i, pts[i]);
            }
        }
        //to get list of points for a POS operation in the UP direction, we will translate the points by the offset
        //this function takes in a list of points and translates these points by n offset
        static void TranslatePoints(List<Vector2> iList, Vector2 offset)
        {   Debug.Log(iList.Count + " iListCount 329 ");
            for (int i = 0; i < iList.Count; ++i)
            {
                // Debug.Log(i + " Translate Points 332");
                iList[i] += offset;
            }
        }
        //to get the list of points for a NEG operation in the UP direction, we'll need to invert the y values and then translate the points by offset
        //InvertY function that takes in a list of points and negates the y values
        static void InvertY(List<Vector2> iList)
        {
            for (int i = 0; i < iList.Count; ++i)
            {
                iList[i] = new Vector2(iList[i].x, -iList[i].y);
            }
        }
        //To get the list of points for a NONE operation in the UP direction, we need to accumulate the x and y values for a constant y = Padding + TileSize and x from 0 to Padding + TileSize.







    }



}

/*
To implement flood fill, we will follow the following algorithm:

Step 1: Set up the boundary of the texture based on the curves and straight lines. Mark all pixels that fall in this set of points as visited.
Step 2: Take the centre pixel of the FinalCut texture and set the colour value from the input texture’s centre pixel. Mark it as visited. Add this pixel to the stack.
Step 3: While the stack of pixels is not empty, go up, left, right and down to get the next pixel. If the next pixel is already marked as visited, then we do not process that pixel. If not, then we set that pixel as visited, set the colour from the original texture of the same pixel, and add it to the stack.
*/