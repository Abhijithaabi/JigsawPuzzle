using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BezierCurves
{

    private static float[] factorial = new float[]
  {
        1.0f,
        1.0f,
        2.0f,
        6.0f,
        24.0f,
        120.0f,
        720.0f,
        5040.0f,
        40320.0f,
        362880.0f,
        3628800.0f,
        39916800.0f,
        479001600.0f,
        6227020800.0f,
        87178291200.0f,
        1307674368000.0f,
        20922789888000.0f,
  };
    //containst the n!(factorial) of n = 0 -> 16

    private static float Binomial(int n, int i)
    {
        float ni;
        float a1 = factorial[n];
        float a2 = factorial[i];
        float a3 = factorial[n - i];
        ni = a1 / (a2 * a3);
        return ni;
    }
    //calculation binomial coefficients for bexier equation

    private static float Bernstein(int n, int i, float t)
    {
        float t_i = Mathf.Pow(t, i);
        float t_n_minus_i = Mathf.Pow((1 - t), (n - i));

        float basis = Binomial(n, i) * t_i * t_n_minus_i;
        return basis;
    }
    //calculation of Bernstein basis polynomials for bezier equation


    //Point3 static fnction so that we dont have to instantiate a bezier curver class to get bezier points given a set control points

    public static Vector3 Point3(float t, List<Vector3> controlPoints)
    {
        int N = controlPoints.Count - 1;
        if (N > 16)
        {
            Debug.Log("You have used more than 16 control points. The maximum control points allowed is 16.");
            controlPoints.RemoveRange(16, controlPoints.Count - 16);
        }
        if (t <= 0) return controlPoints[0];
        if (t >= 1) return controlPoints[controlPoints.Count - 1];

        Vector3 p = new Vector3();

        for (int i = 0; i < controlPoints.Count; ++i)
        {
            Vector3 bn = Bernstein(N, i, t) * controlPoints[i];
            p += bn;
        }

        return p;
    }
    //calculated bezier point for a given set of control points at an interval t

    public static List<Vector3> PointList3(
  List<Vector3> controlPoints,
  float interval = 0.01f)
    {
        int N = controlPoints.Count - 1;
        if (N > 16)
        {
            Debug.Log("You have used more than 16 control points. " +
              "The maximum control points allowed is 16.");
            controlPoints.RemoveRange(16, controlPoints.Count - 16);
        }

        List<Vector3> points = new List<Vector3>();
        for (float t = 0.0f; t <= 1.0f + interval - 0.0001f; t += interval)
        {
            Vector3 p = new Vector3();
            for (int i = 0; i < controlPoints.Count; ++i)
            {
                Vector3 bn = Bernstein(N, i, t) * controlPoints[i];
                p += bn;
            }
            points.Add(p);
        }

        return points;
    }

    public static Vector2 Point2(float t, List<Vector2> controlPoints)
    {
        int N = controlPoints.Count - 1;
        if (N > 16)
        {
            Debug.Log("You have used more than 16 control points. The maximum control points allowed is 16.");
            controlPoints.RemoveRange(16, controlPoints.Count - 16);
        }

        if (t <= 0) return controlPoints[0];
        if (t >= 1) return controlPoints[controlPoints.Count - 1];

        Vector2 p = new Vector2();

        for (int i = 0; i < controlPoints.Count; ++i)
        {
            Vector2 bn = Bernstein(N, i, t) * controlPoints[i];
            p += bn;
        }

        return p;
    }
    public static List<Vector2> PointList2(
      List<Vector2> controlPoints,
      float interval = 0.01f)
    {
        int N = controlPoints.Count - 1;
        if (N > 16)
        {
            Debug.Log("You have used more than 16 control points. " +
              "The maximum control points allowed is 16.");
            controlPoints.RemoveRange(16, controlPoints.Count - 16);
        }

        List<Vector2> points = new List<Vector2>();
        for (float t = 0.0f; t <= 1.0f + interval - 0.0001f; t += interval)
        {
            Vector2 p = new Vector2();
            for (int i = 0; i < controlPoints.Count; ++i)
            {
                Vector2 bn = Bernstein(N, i, t) * controlPoints[i];
                p += bn;
            }
            points.Add(p);
        }

        return points;
    }
    //implemented the method that returns the list of points representing the bezier curve
    //similarly for Vector2

}


//reference - https://faramira.com/implement-bezier-curve-using-csharp-in-unity/