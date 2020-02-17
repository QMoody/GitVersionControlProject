using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BezierCurve))]
public class BezierCurveInspector : Editor
{

    private BezierCurve curve;
    private Transform handleTransform;
    private Quaternion handleRotation;
    private const int lineSteps = 10;

    private const float directionScale = 0.5f;

    private void OnSceneGUI()
    {
        curve = target as BezierCurve;
        handleTransform = curve.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;

        Vector3 p0 = ShowPoint(0);
        Vector3 p1 = ShowPoint(1);
        Vector3 p2 = ShowPoint(2);
        Vector3 p3 = ShowPoint(3);

        Handles.color = Color.gray;
        Handles.DrawLine(p0, p1);
        Handles.DrawLine(p1, p2);
        Handles.DrawLine(p2, p3);

        ShowDirections();
        Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
    }

    private void ShowDirections()
    {
        Handles.color = Color.green;
        Vector3 point = curve.GetPoint(0f);
        Handles.DrawLine(point, point + curve.GetDirection(0f) * directionScale);
        for (int i = 1; i <= lineSteps; i++)
        {
            point = curve.GetPoint(i / (float)lineSteps);
            Handles.DrawLine(point, point + curve.GetDirection(i / (float)lineSteps) * directionScale);
        }
    }    

    private Vector3 ShowPoint(int index)
    {
        Vector3 point = handleTransform.TransformPoint(curve.points[index]);
        EditorGUI.BeginChangeCheck();
        point = Handles.DoPositionHandle(point, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(curve, "Move Point");
            EditorUtility.SetDirty(curve);
            curve.points[index] = handleTransform.InverseTransformPoint(point);
        }
        return point;
    }
}

[CustomEditor(typeof(BezierSpline))]
public class BezierSplineInspector : Editor
{

    private BezierSpline spline;
    private Transform handleTransform;
    private Quaternion handleRotation;
    private const int lineSteps = 10;
    private const float handleSize = 0.04f;
    private const float pickSize = 0.06f;

    private int selectedIndex = -1;

    private const float directionScale = 0.5f;

    private const int stepsPerCurve = 10;

    //public enum BezierControlPointMode
    //{
    //    Free,
    //    Aligned,
    //    Mirrored
    //}
    private static Color[] modeColors = {
        Color.white,
        Color.yellow,
        Color.cyan
    };
    private Vector3 ShowPoint(int index)
    {
        Vector3 point = handleTransform.TransformPoint(spline.GetControlPoint(index));
        float size = HandleUtility.GetHandleSize(point);
        Handles.color = modeColors[(int)spline.GetControlPointMode(index)];
        if (Handles.Button(point, handleRotation, size * handleSize, size * pickSize, Handles.DotHandleCap))
        {
            selectedIndex = index;
            Repaint();
        }
        if (selectedIndex == index)
        {
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(spline, "Move Point");
                EditorUtility.SetDirty(spline);
                spline.SetControlPoint(index, handleTransform.InverseTransformPoint(point));
            }
        }
        return point;
    }
    private void OnSceneGUI()
    {
        spline = target as BezierSpline;
        handleTransform = spline.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;

        Vector3 p0 = ShowPoint(0);
        for (int i = 1; i < spline.ControlPointCount; i += 3)
        {
            Vector3 p1 = ShowPoint(i);
            Vector3 p2 = ShowPoint(i + 1);
            Vector3 p3 = ShowPoint(i + 2);

            Handles.color = Color.gray;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p2, p3);

            Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
            p0 = p3;
        }
        ShowDirections();
    }

    public override void OnInspectorGUI()
    {
        spline = target as BezierSpline;
        if (selectedIndex >= 0 && selectedIndex < spline.ControlPointCount)
        {
            DrawSelectedPointInspector();
        }
        if (GUILayout.Button("Add Curve"))
        {
            Undo.RecordObject(spline, "Add Curve");
            spline.AddCurve();
            EditorUtility.SetDirty(spline);
        }
    }

    private void DrawSelectedPointInspector()
    {
        GUILayout.Label("Selected Point" + selectedIndex.ToString());
        EditorGUI.BeginChangeCheck();
        Vector3 point = EditorGUILayout.Vector3Field("Position", spline.GetControlPoint(selectedIndex));
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Move Point");
            EditorUtility.SetDirty(spline);
            spline.SetControlPoint(selectedIndex, point);
        }
        EditorGUI.BeginChangeCheck();
        BezierControlPointMode mode = (BezierControlPointMode)
            EditorGUILayout.EnumPopup("Mode", spline.GetControlPointMode(selectedIndex));
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Change Point Mode");
            spline.SetControlPointMode(selectedIndex, mode);
            EditorUtility.SetDirty(spline);
        }
    }
       
    private void ShowDirections()
    {
        Handles.color = Color.green;
        Vector3 point = spline.GetPoint(0f);
        Handles.DrawLine(point, point + spline.GetDirection(0f) * directionScale);
        int steps = stepsPerCurve * spline.CurveCount;
        for (int i = 1; i <= steps; i++)
        {
            point = spline.GetPoint(i / (float)steps);
            Handles.DrawLine(point, point + spline.GetDirection(i / (float)steps) * directionScale);
        }
    }

    

    
}


