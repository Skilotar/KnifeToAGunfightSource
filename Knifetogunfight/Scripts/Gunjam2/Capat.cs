using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using MonoMod;
using Gungeon;
using Dungeonator;

namespace Knives
{
    public class Overkill : PassiveItem
    {
        public static void Register()
        {
            string itemName = "Overkill Capacitor";

            string resourceName = "Knives/Resources/Overkill";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<Overkill>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Fatal Capacity";
            string longDesc = "Boosts knockback and creates electricity archs between any kill and a single nearby ally. Damage of the connection is based on the max health of the enemy killed. " +
                "Originally a moral boosting unit for warforged allowing them to gain power from their fallen." +
                "\n\n\n -Knife_to_a_Gunfight";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ski");

            //Adds the actual passive effect to the item
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.KnockbackMultiplier, .2f);

            item.quality = PickupObject.ItemQuality.B;


        }
        public override void Pickup(PlayerController player)
        {
            player.OnDealtDamageContext += this.OnDealtDamageContext;
            base.Pickup(player);

        }
        public float raduis = 7f;

        public void OnDealtDamageContext(PlayerController player, float damage, bool fatal, HealthHaver hitenemy)
        {
            if (hitenemy != null)
            {
                if (fatal)
                {
                    // on fatal hit will spawn two connecting projectiles that will form a chain lightning and disipate quickly

                    
                    RoomHandler room = player.CurrentRoom;
                    AIActor enemy = room.GetNearestEnemy(hitenemy.sprite.WorldCenter, out raduis,true,true);

                    if (enemy != hitenemy.aiActor && enemy != null && Vector2.Distance(enemy.sprite.WorldCenter, hitenemy.sprite.WorldCenter) < 7)
                    {


                        //spawns on courpse
                        Projectile projectile2 = ((Gun)ETGMod.Databases.Items[330]).DefaultModule.projectiles[0];
                        GameObject gameObject2 = SpawnManager.SpawnProjectile(projectile2.gameObject, hitenemy.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (player.CurrentGun == null) ? 0f : player.CurrentGun.CurrentAngle), true);
                        Projectile component2 = gameObject2.GetComponent<Projectile>();

                        component2.Owner = player;
                        component2.Shooter = player.specRigidbody;
                        component2.baseData.damage = 0f;
                        component2.AdditionalScaleMultiplier = .25f;
                        component2.baseData.speed = .05f;
                        component2.baseData.range = .1f;
                        component2.HasDefaultTint = true;
                        component2.DefaultTintColor = new Color(0.024f, 0.79f, 0.92f);

                        ChainLightningModifier chain2 = component2.gameObject.GetOrAddComponent<ChainLightningModifier>();
                        chain2.DamagesEnemies = true;
                        chain2.DamagesPlayers = false;
                        chain2.damagePerHit = hitenemy.GetMaxHealth() *.8f;
                        chain2.RequiresSameProjectileClass = true;
                        chain2.maximumLinkDistance = 10f;
                        

                        PierceProjModifier stab2 = component2.gameObject.GetOrAddComponent<PierceProjModifier>();
                        stab2.penetration = 50;



                        //spawns on nearest enemy
                        Projectile projectile3 = ((Gun)ETGMod.Databases.Items[330]).DefaultModule.projectiles[0];
                        GameObject gameObject3 = SpawnManager.SpawnProjectile(projectile3.gameObject, enemy.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (player.CurrentGun == null) ? 0f : player.CurrentGun.CurrentAngle), true);
                        Projectile component3 = gameObject3.GetComponent<Projectile>();

                        component3.Owner = player;
                        component3.Shooter = player.specRigidbody;
                        component3.baseData.damage = 0f;
                        component3.AdditionalScaleMultiplier = .25f;
                        component3.baseData.speed = .05f;
                        component3.baseData.range = .1f;
                        component3.HasDefaultTint = true;
                        component3.DefaultTintColor = new Color(0.024f, 0.79f, 0.92f);

                        PierceProjModifier stab3 = component3.gameObject.GetOrAddComponent<PierceProjModifier>();
                        stab3.penetration = 50;

                        ChainLightningModifier chain3 = component3.gameObject.GetOrAddComponent<ChainLightningModifier>();
                        chain3.DamagesEnemies = true;
                        chain3.DamagesPlayers = false;
                        chain3.damagePerHit = hitenemy.GetMaxHealth() * .8f; 
                        chain3.RequiresSameProjectileClass = true;
                        chain3.maximumLinkDistance = 10f;

                      

                    }

                }
            }
        }

      

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnDealtDamageContext -= this.OnDealtDamageContext;
            return base.Drop(player);
        }
    }
}