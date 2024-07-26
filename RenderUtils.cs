using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MuckHack.Utils
{
    public class RenderUtils
    {

        public static GUIStyle StringStyle { get; set; } = new GUIStyle(GUI.skin.label);

        public static Color Color
        {
            get
            {
                return GUI.color;
            }
            set
            {
                GUI.color = value;
            }
        }
        public static void DrawString(Vector2 position, string label, bool centered = true)
        {
            GUIContent content = new GUIContent(label);
            Vector2 vector = RenderUtils.StringStyle.CalcSize(content);
            Vector2 position2 = centered ? (position - vector / 2f) : position;
            GUI.Label(new Rect(position2, vector), content);
        }

        public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color, float width)
        {
            Matrix4x4 matrix = GUI.matrix;
            bool flag = !RenderUtils.lineTex;
            if (flag)
            {
                RenderUtils.lineTex = new Texture2D(1, 1);
            }
            Color color2 = GUI.color;
            GUI.color = color;
            float num = Vector3.Angle(pointB - pointA, Vector2.right);
            bool flag2 = pointA.y > pointB.y;
            if (flag2)
            {
                num = -num;
            }
            GUIUtility.ScaleAroundPivot(new Vector2((pointB - pointA).magnitude, width), new Vector2(pointA.x, pointA.y + 0.5f));
            GUIUtility.RotateAroundPivot(num, pointA);
            GUI.DrawTexture(new Rect(pointA.x, pointA.y, 1f, 1f), RenderUtils.lineTex);
            GUI.matrix = matrix;
            GUI.color = color2;
        }
        public static void DrawBox(float x, float y, float w, float h, Color color, float thickness)
        {
            RenderUtils.DrawLine(new Vector2(x, y), new Vector2(x + w, y), color, thickness);
            RenderUtils.DrawLine(new Vector2(x, y), new Vector2(x, y + h), color, thickness);
            RenderUtils.DrawLine(new Vector2(x + w, y), new Vector2(x + w, y + h), color, thickness);
            RenderUtils.DrawLine(new Vector2(x, y + h), new Vector2(x + w, y + h), color, thickness);
        }

        public static Texture2D lineTex;
    }
    public class Drawing
    {

        public static void DrawLine(Rect rect)
        {
            Drawing.DrawLine(rect, GUI.contentColor, 1f);
        }


        public static void DrawLine(Rect rect, Color color)
        {
            Drawing.DrawLine(rect, color, 1f);
        }


        public static void DrawLine(Rect rect, float width)
        {
            Drawing.DrawLine(rect, GUI.contentColor, width);
        }


        public static void DrawLine(Rect rect, Color color, float width)
        {
            Drawing.DrawLine(new Vector2(rect.x, rect.y), new Vector2(rect.x + rect.width, rect.y + rect.height), color, width);
        }

        public static void DrawLine(Vector2 pointA, Vector2 pointB)
        {
            Drawing.DrawLine(pointA, pointB, GUI.contentColor, 1f);
        }


        public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color)
        {
            Drawing.DrawLine(pointA, pointB, color, 1f);
        }


        public static void DrawLine(Vector2 pointA, Vector2 pointB, float width)
        {
            Drawing.DrawLine(pointA, pointB, GUI.contentColor, width);
        }


        public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color, float width)
        {
            Matrix4x4 matrix = GUI.matrix;
            if (!Drawing.lineTex)
            {
                Drawing.lineTex = new Texture2D(1, 1);
            }
            Color color2 = GUI.color;
            GUI.color = color;
            float num = Vector3.Angle(pointB - pointA, Vector2.right);
            if (pointA.y > pointB.y)
            {
                num = -num;
            }
            GUIUtility.ScaleAroundPivot(new Vector2((pointB - pointA).magnitude, width), new Vector2(pointA.x, pointA.y + 0.5f));
            GUIUtility.RotateAroundPivot(num, pointA);
            GUI.DrawTexture(new Rect(pointA.x, pointA.y, 1f, 1f), Drawing.lineTex);
            GUI.matrix = matrix;
            GUI.color = color2;
        }


        public static void DrawBox(float x, float y, float w, float h, Color color)
        {
            Drawing.DrawLine(new Vector2(x, y), new Vector2(x + w, y), color);
            Drawing.DrawLine(new Vector2(x, y), new Vector2(x, y + h), color);
            Drawing.DrawLine(new Vector2(x + w, y), new Vector2(x + w, y + h), color);
            Drawing.DrawLine(new Vector2(x, y + h), new Vector2(x + w, y + h), color);
        }

        public static Texture2D lineTex;
    }

}
