using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Runtime.Remoting.Messaging;
using System.Web;
using static GameSettings;
using Unity.Collections;
using MuckHack.Features.Visuals;

namespace Muck.Hack
{
    internal class MainController : MonoBehaviour
    {



        private bool Menu1;
        private bool Menu2;
        private bool Menu3;
        private bool Menu4;
        private bool Menu5;

        private static bool godModeEnabled = false;
        private bool toggleHealth = false;


        //       private Rect MenuRect8 = new Rect(LEV-PRAV, VERH-NIZ, Shiri, Vyso);

        private Rect MenuRect1 = new Rect(155, 155, 355, 155);
        private Rect MenuRect2 = new Rect(515, 155, 355, 155);
        private Rect MenuRect5 = new Rect(515, 310, 355, 155);

        private Rect MenuRect3 = new Rect(874, 155, 355, 155);
        private Rect MenuRect4 = new Rect(1240, 155, 355, 155);


        public static void SpawnRandomMob()
        {
            MobManager instance = MobManager.Instance;
            int num = UnityEngine.Random.Range(0, MobSpawner.Instance.allMobs.Length);
            MobType mobType = MobSpawner.Instance.allMobs[num];
            int nextId = MobManager.Instance.GetNextId();
            MobSpawner.Instance.ServerSpawnNewMob(nextId, mobType.id, PlayerStatus.Instance.gameObject.transform.position, 1f, 1f, 0, -1);
        }
        public static void Dupe()
        {
            InventoryItem currentItem = InventoryUI.Instance.hotbar.currentItem;
            if (currentItem != null)
            {
                currentItem.amount *= 2;
                currentItem.max *= 2;
            }

        }
        public GameSettings.GameMode gameMode { get; set; }
        public GameSettings.FriendlyFire friendlyFire { get; set; }
        public GameSettings.Difficulty difficulty { get; set; }
        private List<HitableMob> allMobs = new List<HitableMob>();


        private void Ent(int win)
        {
            HitableMob[] mobs = UnityEngine.Object.FindObjectsOfType<HitableMob>();
            allMobs.AddRange(mobs);
            GUILayout.BeginVertical();

            // Перебираем всех мобов и создаем кнопку для каждого
            foreach (var mob in allMobs)
            {
                GUILayout.BeginHorizontal();

                // Отображаем имя моба
                GUILayout.Label("<b><color=red>" + mob.name + "</color></b>");

                // Создаем кнопку для убийства моба
                if (GUILayout.Button("Kill", GUILayout.Width(50)))
                {
                    mob.Hit(9999, 9999f, 1, Vector3.zero, 1);

                }

                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
        }


        private void Stats(int win)
        {
            GUILayout.Label("Stats");
            if (GUILayout.Button("Add kills", new GUILayoutOption[0]))
            {
                foreach (KeyValuePair<int, PlayerManager> pair in GameManager.players)
                {
                    if (pair.Key == LocalClient.instance.myId)
                        pair.Value.kills++;
                }
            }
            if (GUILayout.Button("Picup all", new GUILayoutOption[0]))
            {
                PickupInteract[] array4 = UnityEngine.Object.FindObjectsOfType<PickupInteract>();
                for (int k = 0; k < array4.Length; k++)
                {
                    array4[k].Interact();
                }
            }
            GUILayout.Label("");
            if (GUILayout.Button("Open all chest", new GUILayoutOption[0]))
            {
                LootContainerInteract[] array8 = UnityEngine.Object.FindObjectsOfType<LootContainerInteract>();
                for (int l = 0; l < array8.Length; l++)
                {
                    ClientSend.PickupInteract(array8[l].GetId());
                }
            }
            if (GUILayout.Button("Dupe item (current item x2)", new GUILayoutOption[0]))
            {
                Dupe();
            }

            GUILayout.Label("");
            GUILayout.Label("Spawn");
            if (GUILayout.Button("Spawn BossShrine mob", new GUILayoutOption[0]))
            {
                MobManager instance = MobManager.Instance;
                int nextId = MobManager.Instance.GetNextId();
                MobSpawner.Instance.ServerSpawnNewMob(nextId, (int)Mob.BossType.BossShrine, PlayerStatus.Instance.gameObject.transform.position, 1f, 1f, 0, -1);
            }
            if (GUILayout.Button("Spawn Enemy mob", new GUILayoutOption[0]))
            {
                MobManager instance = MobManager.Instance;
                int nextId = MobManager.Instance.GetNextId();
                MobSpawner.Instance.ServerSpawnNewMob(nextId, (int)MobType.MobBehaviour.Enemy, PlayerStatus.Instance.gameObject.transform.position, 1f, 1f, 0, -1);
            }
            if (GUILayout.Button("Spawn Neutral mob", new GUILayoutOption[0]))
            {
                MobManager instance = MobManager.Instance;
                int nextId = MobManager.Instance.GetNextId();
                MobSpawner.Instance.ServerSpawnNewMob(nextId, (int)MobType.MobBehaviour.Neutral, PlayerStatus.Instance.gameObject.transform.position, 1f, 1f, 0, -1);
            }

            if (GUILayout.Button("Spawn BossNight mob", new GUILayoutOption[0]))
            {
                MobManager instance = MobManager.Instance;
                int nextId = MobManager.Instance.GetNextId();
                MobSpawner.Instance.ServerSpawnNewMob(nextId, (int)Mob.BossType.BossNight, PlayerStatus.Instance.gameObject.transform.position, 1f, 1f, 0, -1);
            }
            if (GUILayout.Button("Spawn random mob", new GUILayoutOption[0]))
            {
                MobManager instance = MobManager.Instance;
                int num = UnityEngine.Random.Range(0, MobSpawner.Instance.allMobs.Length);
                MobType mobType = MobSpawner.Instance.allMobs[num];
                int nextId = MobManager.Instance.GetNextId();
                MobSpawner.Instance.ServerSpawnNewMob(nextId, mobType.id, PlayerStatus.Instance.gameObject.transform.position, 1f, 1f, 0, -1);
            }

            GUILayout.Label("Kill");
            if (GUILayout.Button("Kill all", new GUILayoutOption[0]))
            {
                Hitable[] array5 = UnityEngine.Object.FindObjectsOfType<Hitable>();
                for (int l = 0; l < array5.Length; l++)
                {
                    array5[l].Hit(9999, 9999f, 1, Vector3.zero, 1);
                }
            }
            if (GUILayout.Button("Kill all mobs", new GUILayoutOption[0]))
            {
                HitableMob[] array6 = UnityEngine.Object.FindObjectsOfType<HitableMob>();
                for (int l = 0; l < array6.Length; l++)
                {
                    array6[l].Hit(9999, 9999f, 1, Vector3.zero, 1);
                }
            }
            GUILayout.Label("Destroy");
            if (GUILayout.Button("Destroy resources ", new GUILayoutOption[0]))
            {
                HitableResource[] array3 = UnityEngine.Object.FindObjectsOfType<HitableResource>();
                for (int l = 0; l < array3.Length; l++)
                {
                    array3[l].Hit(9999, 9999f, 1, Vector3.zero, 1);
                }
            }

            if (GUILayout.Button("Destroy rock ", new GUILayoutOption[0]))
            {
                HitableRock[] array2 = UnityEngine.Object.FindObjectsOfType<HitableRock>();
                for (int l = 0; l < array2.Length; l++)
                {
                    array2[l].Hit(9999, 9999f, 1, Vector3.zero, 1);
                }
            }

            if (GUILayout.Button("Destroy all tree", new GUILayoutOption[0]))
            {
                HitableTree[] array = UnityEngine.Object.FindObjectsOfType<HitableTree>();
                for (int l = 0; l < array.Length; l++)
                {
                    array[l].Hit(9999, 9999f, 1, Vector3.zero, 1);
                }
            }
            if (GUILayout.Button("Destroy all Chest", new GUILayoutOption[0]))
            {
                HitableChest[] array4 = UnityEngine.Object.FindObjectsOfType<HitableChest>();
                for (int l = 0; l < array4.Length; l++)
                {
                    array4[l].Hit(9999, 9999f, 1, Vector3.zero, 1);
                }
            }

        }
        private bool toggleSP = false;
        private bool toggleshield = false;
        private void MainMenu(int win)
        {
            toggleHealth = GUILayout.Toggle(toggleHealth, "Infinity health");
            toggleSP = GUILayout.Toggle(toggleSP, "Speed hack");

            if (toggleHealth)
            {
                PlayerStatus.Instance.maxShield += 1000000;
                PlayerStatus.Instance.shield = (float)(PlayerStatus.Instance.maxShield);
            }

            if (toggleSP)
            {
                PlayerStatus.Instance.currentSpeedArmorMultiplier = 50f;
            }
            else
            {
                PlayerStatus.Instance.currentSpeedArmorMultiplier = 1f;
            }

            if (GUILayout.Button("Set health 100 ", new GUILayoutOption[0]))
            {

                PlayerStatus.Instance.hp = 100;

            }

            if (GUILayout.Button("Infinity stamina ", new GUILayoutOption[0]))
            {

                PlayerStatus.Instance.stamina = 1000000000;

            }
            if (GUILayout.Button("Infinity food ", new GUILayoutOption[0]))
            {
                PlayerStatus.Instance.hunger = 100000000;

            }

        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Insert))
            {
                Debug.Log("Insert key pressed");
                Menu1 = !Menu1;
                Menu2 = !Menu2;
                Menu3 = !Menu3;
                Menu4 = !Menu4;
                Menu5 = !Menu5;

            }
        }

    }
}
