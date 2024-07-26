using BepInEx.Logging;
using MuckHack.Features.Visuals;
using System;
using System.Collections.Generic;
using UnityEngine;
using static MuckHack.Features.Visuals.RenderESP;
using static TextureData;

namespace Mucks
{
    internal class MAIN : MonoBehaviour
    {
        public static ManualLogSource insl = BepInEx.Logging.Logger.CreateLogSource("Ins main");
        private bool flyHack = false; 
        private void OnGUI()
        {
            if (Menu1)
            {
                MenuRect1 = GUILayout.Window(1, MenuRect1, MainMenu, "Player Menu | Insightware");
            }
            if (Menu2)
            {

                MenuRect2 = GUILayout.Window(2, MenuRect2, EspMenu, "Esp Menu | Insightware");
            }
            if (Menu3)
            {

                MenuRect3 = GUILayout.Window(3, MenuRect3, Stats, "Gameplay Menu | Insightware");
            }
            if (Menu4)
            {

                MenuRect4 = GUILayout.Window(4, MenuRect4, Ent, "Teleport menu | Insightware");
            }
            if (RenderESP.Enabled)
            {
                RenderESP.RenderAll();
            }   
        }
        private bool Menu1;
        private bool Menu2;
        private bool Menu3;
        private bool Menu4;
        private bool Menu5;
        private Rect MenuRect1 = new Rect(155, 155, 355, 155);
        private Rect MenuRect2 = new Rect(515, 155, 355, 155);
        private Rect MenuRect5 = new Rect(515, 310, 355, 155);
        private Rect MenuRect3 = new Rect(874, 155, 355, 155);
        private Rect MenuRect4 = new Rect(1240, 155, 355, 155);
        public GameSettings.GameMode gameMode { get; set; }
        public GameSettings.FriendlyFire friendlyFire { get; set; }
        public GameSettings.Difficulty difficulty { get; set; }
        private List<HitableMob> allMobs = new List<HitableMob>();

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

        private bool toggleSP = false;
        private bool toggleshield = false;
        private  bool godModeEnabled = false;
        private bool toggleHealth = false;
        public List<PlayerManager> allPlayers;

        private void Ent(int win)
        {
            if (GUILayout.Button("Teleport settings:", new GUILayoutOption[0]))
            {
                aty = !aty;
                if (aty)
                {
                    GUILayout.Label("Keypad 5 - teleport up");
                }
            }
            if (GUILayout.Button("Fly hack", new GUILayoutOption[0]))
            {
                flyHack = !flyHack;
            }

        }

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

        private void EspMenu(int win)
        {
            renderPlayer = GUILayout.Toggle(renderPlayer, "Toggle ESP player", GUILayout.Height(25f));
            renderChest = GUILayout.Toggle(renderChest, "Toggle ESP Chest", GUILayout.Height(25f));
            renderMob = GUILayout.Toggle(renderMob, "Toggle ESP Mob", GUILayout.Height(25f));
            renderHitableRock = GUILayout.Toggle(renderHitableRock, "Toggle ESP Resources", GUILayout.Height(25f));
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
        public void FixedUpdate()
        {
            if (flyHack)
            {
                if (!Input.GetKey(InputManager.jump) || !Input.GetKey(InputManager.forward) || !Input.GetKey(InputManager.backwards) || !Input.GetKey(InputManager.left) || !Input.GetKey(InputManager.right))
                {
                    Rigidbody rb1 = PlayerMovement.Instance.GetRb();
                    rb1.AddForce(Vector3.up * 65);
                }
            }
        }
        public bool aty = false;
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
            if (flyHack)
            {
                PlayerMovement.Instance.GetRb().velocity = new Vector3(0f, 0f, 0f);
                float num = Input.GetKey(KeyCode.LeftShift) ? 0.5f : (Input.GetKey(InputManager.sprint) ? 1f : 0.5f);
                if (Input.GetKey(InputManager.jump))
                {
                    PlayerStatus.Instance.transform.position = new Vector3(PlayerStatus.Instance.transform.position.x, PlayerStatus.Instance.transform.position.y + num, PlayerStatus.Instance.transform.position.z);
                }
                Vector3 position = PlayerStatus.Instance.transform.position;
                if (Input.GetKey(InputManager.forward))
                {
                    PlayerStatus.Instance.transform.position = new Vector3(position.x + Camera.main.transform.forward.x * Camera.main.transform.up.y * num, position.y + Camera.main.transform.forward.y * num, position.z + Camera.main.transform.forward.z * Camera.main.transform.up.y * num);
                }
                if (Input.GetKey(InputManager.backwards))
                {
                    PlayerStatus.Instance.transform.position = new Vector3(position.x - Camera.main.transform.forward.x * Camera.main.transform.up.y * num, position.y - Camera.main.transform.forward.y * num, position.z - Camera.main.transform.forward.z * Camera.main.transform.up.y * num);
                }
                if (Input.GetKey(InputManager.right))
                {
                    PlayerStatus.Instance.transform.position = new Vector3(position.x + Camera.main.transform.right.x * num, position.y, position.z + Camera.main.transform.right.z * num);
                }
                if (Input.GetKey(InputManager.left))
                {
                    PlayerStatus.Instance.transform.position = new Vector3(position.x - Camera.main.transform.right.x * num, position.y, position.z - Camera.main.transform.right.z * num);
                }
            }
            if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                Vector3 playerPosition = PlayerStatus.Instance.gameObject.transform.position;
                PlayerStatus.Instance.gameObject.transform.position = new Vector3(playerPosition.x, playerPosition.y + 5f, playerPosition.z);
            }


        }
    }
}
