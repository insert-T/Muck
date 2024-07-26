using MuckHack.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MuckHack.Features.Visuals
{
    public class RenderESP
    {
        public static bool Enabled = false;
        public static bool renderPlayer = false;
        public static bool renderChest = false;
        public static bool renderMob = false;
        public static bool renderHitableRock = false;
        public static void Toggle4()
        {
            Enabled = !Enabled;
        }

        public static void RenderAll()
        {
            if (Enabled)
            {
                if (renderPlayer)
                {
                    Render();
                }
                if (renderChest)
                {
                    Render2();
                }
                if (renderMob)
                {
                    Render3();
                }
                if (renderHitableRock)
                {
                    Render4();
                }

            }
        }

        

        public static void Render()
        {
            if (Enabled)
            {
                foreach (OnlinePlayer onlinePlayer in UnityEngine.Object.FindObjectsOfType(typeof(OnlinePlayer)) as OnlinePlayer[])
                {
                    int num = (int)Vector3.Distance(PlayerStatus.Instance.transform.position, onlinePlayer.transform.position);
                    GUIStyle guistyle = new GUIStyle();
                    guistyle.normal.textColor = Color.blue;
                    Vector3 vector = Camera.main.WorldToScreenPoint(onlinePlayer.transform.position);
                    if (vector.z > 0f)
                    {
                        GUI.Label(new Rect(vector.x, (float)Screen.height - vector.y, 0f, 0f), onlinePlayer.name.Replace("(Clone)", "") + " [" + num.ToString() + "m]", guistyle);
                        Drawing.DrawLine(new Vector2((float)(Screen.width / 2), (float)(Screen.height - 1)), new Vector2(vector.x, (float)Screen.height - vector.y), Color.red, 2f);
                    }
                }
            }
        }
        public static void Render2()
        {
            if (Enabled)
            {
                foreach (Chest chest in UnityEngine.Object.FindObjectsOfType<Chest>())
                {
                    Vector3 drawPos = Camera.main.WorldToScreenPoint(chest.transform.position);

                    if (drawPos.z < 0)
                    {
                        continue;
                    }

                    drawPos.y = Screen.height - drawPos.y;

                    float distance = Vector3.Distance(Camera.main.transform.position, chest.transform.position);
                    float size = 1000 / distance;

                    RenderUtils.DrawBox(drawPos.x - 15, drawPos.y - 15, size, size * 1.7f, Color.green, 2);
                    GUI.Label(new Rect(drawPos.x - 50, drawPos.y - 30, 100, 20), "<color=yellow>Chest</color>", GUI.skin.label);
                }
            }
        }

        public static void Render3()
        {

            if (Enabled)
            {
                foreach (Mob chest in UnityEngine.Object.FindObjectsOfType<Mob>())
                {
                    Vector3 position = chest.transform.position;
                    Vector3 vector = Camera.main.WorldToScreenPoint(position);
                    int num = (int)Math.Ceiling((double)Vector3.Distance(Camera.main.WorldToScreenPoint(position), chest.transform.position));
                    Vector3 drawPos = Camera.main.WorldToScreenPoint(chest.transform.position);

                    if (drawPos.z < 0)
                    {
                        continue;
                    }

                    drawPos.y = Screen.height - drawPos.y;

                    float distance = Vector3.Distance(Camera.main.transform.position, chest.transform.position);
                    float size = 1000 / distance;

                    RenderUtils.DrawBox(drawPos.x - 15, drawPos.y - 25, size, size * 1.7f, Color.yellow, 2);
                    GUI.Label(new Rect(drawPos.x - 50, drawPos.y - 30, 100, 20), "<color=red>Mob</color>", GUI.skin.label);
                }
            }
        }
        public static void Render4()
        {

            if (Enabled)
            {
                foreach (HitableRock hitableRock in UnityEngine.Object.FindObjectsOfType(typeof(HitableRock)) as HitableRock[])
                {
                    GUIStyle guistyle4 = new GUIStyle();
                    guistyle4.normal.textColor = Color.yellow;
                    Vector3 vector4 = Camera.main.WorldToScreenPoint(hitableRock.transform.position);
                    if (vector4.z > 0f)
                    {
                        vector4.z = vector4.y + (float)Screen.height;
                        GUI.Label(new Rect(vector4.x, (float)Screen.height - vector4.y, 0f, 0f), hitableRock.name.Replace("(Clone)", "") + " [" + hitableRock.hp.ToString() + " HP]", guistyle4);
                    }
                }
            }
        }
    }
}
