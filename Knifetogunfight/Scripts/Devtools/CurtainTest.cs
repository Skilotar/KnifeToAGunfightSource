using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Dungeonator;
using GungeonAPI;

namespace Knives
{
    class CurtainTest : PlayerItem
    {
        public static void Register()
        {
            //The name of the item
            string itemName = "Curtain";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "Knives/Resources/tur_collection/tur_nonmounted_right";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<CurtainTest>();

            //Adds a sprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Queen of all LMGs";
            string longDesc = "Place the turrent mount on the battle feild to commence the Mazza! \n\n" +
                "The turret can not be placed beside walls. \nInteract with the mounting brace to equip sheila. \nDismounting can be done by, getting knocked off the turret, pressing roll, or pressing interact." +
                "\n\n\n - Knife_to_a_Gunfight";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ski");

            //Adds the actual passive effect to the item


            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 1f);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalItemCapacity, 1, StatModifier.ModifyMethod.ADDITIVE);

            //Set the rarity of the item



            item.quality = PickupObject.ItemQuality.EXCLUDED;

        }
        public override void Pickup(PlayerController player)
        {
            CurtainTest.BuildPrefab();
            user = player;
            base.Pickup(player);
        }

        protected override void DoEffect(PlayerController user)
        {

            //walls_check
            RoomHandler room;
            room = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(Vector2Extensions.ToIntVector2(user.CenterPosition, VectorConversions.Round));
            CellData cellaim = room.GetNearestCellToPosition(user.CenterPosition);
            CellData cellaimmunis = room.GetNearestCellToPosition(user.CenterPosition - new Vector2(0, 1));

            //if not in or near wall
            if (cellaim.HasWallNeighbor(true, true) == false && cellaimmunis.HasWallNeighbor(true, true) == false)
            {

                //Destroy last instance of object if availible
                if (m_Gun != null)
                {
                    UnityEngine.GameObject.DestroyObject(m_Gun);

                }


                //make new gun
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(CurtainTest.Gun, this.LastOwner.transform.position + new Vector3(0.6f, 5f, 6f), Quaternion.identity);
                gameObject.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(this.LastOwner.specRigidbody.UnitCenter, tk2dBaseSprite.Anchor.MiddleCenter);
                gameObject.GetComponent<tk2dBaseSprite>().HeightOffGround = 9;
                gameObject.GetComponent<tk2dBaseSprite>().UpdateZDepth();
                gameObject.GetComponent<ShadowSystem>().enabled = false;
                gameObject.GetComponent<ShadowSystem>().ignoreUnityLight = true;
                

                m_Gun_position = this.LastOwner.transform.position + new Vector3(0.6f, 1.05f, -6f);
                m_Gun = gameObject;
                AkSoundEngine.PostEvent("Play_tur_place", base.gameObject);
                AkSoundEngine.PostEvent("Play_tur_place", base.gameObject);
                AkSoundEngine.PostEvent("Play_tur_place", base.gameObject);
                AkSoundEngine.PostEvent("Play_tur_place", base.gameObject);

            }
            else
            {
                

                //refund charge
                FieldInfo remainingTimeCooldown = typeof(PlayerItem).GetField("remainingTimeCooldown", BindingFlags.NonPublic | BindingFlags.Instance);
                remainingTimeCooldown.SetValue(this.gameObject, 0);
            }
        }

        public static void BuildPrefab()
        {
            GameObject gameObject = SpriteBuilder.SpriteFromResource("Knives/Resources/Kali_boss_prefight_idle_001", null);
            gameObject.SetActive(false);
            GungeonAPI.FakePrefab.MarkAsFakePrefab(gameObject);
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
           
            CurtainTest.Gun = gameObject;

        }
        public bool startup = false;
        public PlayerController user;
        public Gun onturretgun;
        public bool dismount = false;

        //update is used as the mount state controller so the code can interact with player item code and interact code at the same time
        //the isOnTurret bool is the interact press state.
        public override void Update()
        {
           

            base.Update();
        }

        public static GameObject Gun;
        public static GameObject m_Gun = null;
        public Vector3 m_Gun_position;
    }
}
