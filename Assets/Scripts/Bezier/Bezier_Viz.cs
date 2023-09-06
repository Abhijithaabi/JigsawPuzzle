using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier_Viz : MonoBehaviour
{
    public List<Vector2> ControlPoints;
    public GameObject PointPrefab;
    // [SerializeField] Color LineColour;

    // [SerializeField] Color BezierCurveColour;
    public float LineWidth;

    public List<GameObject> mPointGameObjects;

    private LineRenderer[] mLineRenderers;

    private LineRenderer CreateLine()
    {
        GameObject obj = new GameObject();
        LineRenderer lr = obj.AddComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = Color.white;
        lr.endColor = Color.white;
        lr.startWidth = LineWidth;
        lr.endWidth = LineWidth;
        return lr;
    }
    /*
    this fn will show our lines and curves. we want to have two LineRenderer, the first one is to show straight lines connecting the various 
    control points, and the second one to show the bezier curve. We created a default lineRenderer above
    */



    // Start is called before the first frame update
    void Start()
    {
        //Create the two LineRenderers
        mLineRenderers = new LineRenderer[2];
        mLineRenderers[0] = CreateLine();
        mLineRenderers[1] = CreateLine();

        //set a name to the game objects for the LineRenderers to distinguih them
        mLineRenderers[0].gameObject.name = "LineRenderer_obj_0";
        mLineRenderers[1].gameObject.name = "LineRenderer_obj_1";

        //create the instances of pointPrefab to show the control points
        for (int i = 0; i < ControlPoints.Count; ++i)
        {
            GameObject obj = Instantiate(PointPrefab,
              ControlPoints[i],
              Quaternion.identity);
            obj.name = "ControlPoint_" + i.ToString();
            mPointGameObjects.Add(obj);
        }

    }

    //Update is called once per frame
    void Update()
    {
        LineRenderer lineRenderer = mLineRenderers[0];
        LineRenderer curveRenderer = mLineRenderers[1];

        List<Vector2> pts = new List<Vector2>();

        for (int k = 0; k < mPointGameObjects.Count; ++k)
        {
            pts.Add(mPointGameObjects[k].transform.position);
            // Debug.Log("line1 creating");
            //accumulating the points from the list of the game instantiated game objects that represnt the control points
        }
        //getting the position of the control points from the instantiated game objects for points. this is beacuse we may change 
        //the position of the control points by clicking on any one of the them and relocate.

        //create a line for showing the straight lines between control points
        lineRenderer.positionCount = pts.Count;
        for (int i = 0; i < pts.Count; ++i)
        {

            lineRenderer.SetPosition(i, pts[i]);
            // Debug.Log("Line2 creating");
            //now we are setting those pts(points) to the first LineRenderer

        }

        //we take the control points from the list of points in the scene to recalculate points every frame
        List<Vector2> curve = BezierCurves.PointList2(pts, 0.01f); //getting bezier points by calling the static BezierCurve.PointLits2, returns a list of Vector2
        //0.01f = 100 t , so 101 points
        curveRenderer.startColor = Color.red;
        curveRenderer.endColor = Color.red;
        curveRenderer.positionCount = curve.Count;
        for (int i = 0; i < curve.Count; ++i)
        {
            curveRenderer.SetPosition(i, curve[i]);
        }
        //set those points to curverRenderer and set color to bezierColor
    }

    void OnGUI()
    {
        Event e = Event.current;
        if (e.isMouse)
        {
            if (e.clickCount == 2 && e.button == 0)
            {
                Vector2 rayPos = new Vector2(
                    Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                    Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

                InsertNewControlPoint(rayPos);
            }
        }
    }

    void InsertNewControlPoint(Vector2 p)
    {
        if (mPointGameObjects.Count >= 16)
        {
            Debug.Log("Cannot create any new control points. Max number is 16");
            return;
        }
        GameObject obj = Instantiate(PointPrefab, p, Quaternion.identity);
        obj.name = "ControlPoint_" + mPointGameObjects.Count.ToString();
        mPointGameObjects.Add(obj);
    }

}
