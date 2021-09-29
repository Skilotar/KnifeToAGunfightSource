using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ItemAPI;
using Dungeonator;
using System.Reflection;
using Random = System.Random;
using FullSerializer;
using System.Collections;
using Gungeon;
using MonoMod.RuntimeDetour;



namespace Knives
{
    public class GlassBoi : IounStoneOrbitalItem
    {
        public static void Register()
        {
            string name = "Glass Shrine";
            string resourcePath = "Knives/Resources/Glass_shrine";
            GameObject gameObject = new GameObject();
            GlassBoi rock = gameObject.AddComponent<GlassBoi>();
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
            string shortDesc = "Wait What?";
            string longDesc = "An intact glass shrine that has yet to be attached to its pedistal. The production of these shrines in the gungeon is quite common as they shatter on use." +
                "\n\n\n - Knife_to_a_Gunfight";
            rock.SetupItem(shortDesc, longDesc, "ski");
            rock.quality = PickupObject.ItemQuality.C;
            GlassBoi.BuildPrefab();

            rock.OrbitalPrefab = GlassBoi.orbitalPrefab;

            rock.Identifier = IounStoneOrbitalItem.IounStoneIdentifier.CLEAR;


        }


        public static void BuildPrefab()
        {
            bool flag = GlassBoi.orbitalPrefab != null;
            if (!flag)
            {
                GameObject gameObject = SpriteBuilder.SpriteFromResource("Knives/Resources/Glass_shrine", null);
                gameObject.name = "glass";
                SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(38, 35));
                speculativeRigidbody.CollideWithTileMap = false;
                speculativeRigidbody.CollideWithOthers = true;

                speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
                GlassBoi.orbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
                GlassBoi.orbitalPrefab.motionStyle = PlayerOrbital.OrbitalMotionStyle.ORBIT_PLAYER_ALWAYS;
                GlassBoi.orbitalPrefab.orbitDegreesPerSecond = 90f;
                GlassBoi.orbitalPrefab.shouldRotate = false;
                GlassBoi.orbitalPrefab.orbitRadius = 2.8f;


                GlassBoi.orbitalPrefab.SetOrbitalTier(0);
                UnityEngine.Object.DontDestroyOnLoad(gameObject);
                FakePrefab.MarkAsFakePrefab(gameObject);
                gameObject.SetActive(false);


            }
        }


        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);

            Stopda.guonHook = new Hook(typeof(PlayerOrbital).GetMethod("Initialize"), typeof(Stopda).GetMethod("GuonInit"));
            bool flag = player.gameObject.GetComponent<GlassBoi.BaBoom>() != null;
            bool flag2 = flag;
            bool flag3 = flag2;
            bool flag4 = flag3;
            if (flag4)
            {
                player.gameObject.GetComponent<GlassBoi.BaBoom>().Destroy();
            }
            player.gameObject.AddComponent<GlassBoi.BaBoom>();
            GameManager.Instance.OnNewLevelFullyLoaded += this.FixGuon;
            bool flag5 = this.m_extantOrbital != null;
            bool flag6 = flag5;
            bool flag7 = flag6;
            if (flag7)
            {
                if(this.Owner != null)
                {
                    Owner.healthHaver.OnDamaged += this.OnDamaged;
                }
            }


        }

        private void OnDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
        {
            PlayerController player = (PlayerController)Owner;
            player.RemovePassiveItem(PickupObjectDatabase.GetByEncounterName("Glass Shrine").PickupObjectId);
            player.GiveItem("glass_guon_stone");
            player.GiveItem("glass_guon_stone");
            player.GiveItem("glass_guon_stone");
            player.GiveItem("glass_guon_stone");
            Owner.healthHaver.OnDamaged -= this.OnDamaged;
        }

        public static void GuonInit(Action<PlayerOrbital, PlayerController> orig, PlayerOrbital self, PlayerController player)
        {
            orig(self, player);
        }
        private void FixGuon()
        {
            bool flag = base.Owner && base.Owner.GetComponent<GlassBoi.BaBoom>() != null;
            bool flag2 = flag;
            bool flag3 = flag2;
            bool flag4 = flag3;
            if (flag4)
            {
                base.Owner.GetComponent<GlassBoi.BaBoom>().Destroy();
            }
            bool flag5 = this.m_extantOrbital != null;
            bool flag6 = flag5;
            bool flag7 = flag6;
            if (flag7)
            {
                SpeculativeRigidbody specRigidbody = this.m_extantOrbital.GetComponent<PlayerOrbital>().specRigidbody;
                
            }
            PlayerController owner = base.Owner;
            owner.gameObject.AddComponent<GlassBoi.BaBoom>();
        }
       

        protected override void Update()
        {




            base.Update();
        }

        float rockpoints;

       
        public override DebrisObject Drop(PlayerController player)
        {
            Owner.healthHaver.OnDamaged -= this.OnDamaged;
            GlassBoi.speedUp = false;
            return base.Drop(player);
        }

        protected override void OnDestroy()
        {
            GlassBoi.speedUp = false;
            base.OnDestroy();
        }

        public static bool speedUp = false;
        public static PlayerOrbital orbitalPrefab;
        public List<IPlayerOrbital> orbitals = new List<IPlayerOrbital>();
        public static Hook guonHook;


        private class BaBoom : BraveBehaviour
        {
            // Token: 0x06000B0A RID: 2826 RVA: 0x0005EB58 File Offset: 0x0005CD58
            private void Start()
            {
                this.owner = base.GetComponent<PlayerController>();
            }

            // Token: 0x06000B0B RID: 2827 RVA: 0x0005EB67 File Offset: 0x0005CD67
            public void Destroy()
            {
                UnityEngine.Object.Destroy(this);
            }

            // Token: 0x040005BC RID: 1468
            private PlayerController owner;
        }
    }

}
