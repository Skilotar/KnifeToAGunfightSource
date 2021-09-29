using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using MonoMod;
using Dungeonator;
using Gungeon;

namespace Knives
{
    class ChaoticComedy : PassiveItem
    {

        public static void Register()
        {

            string itemName = "Manic Theatre";


            string resourceName = "Knives/Resources/Chaotic Comedy";


            GameObject obj = new GameObject(itemName);


            var item = obj.AddComponent<ChaoticComedy>();


            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Put on a SHOW!";
            string longDesc = "Enemies dies in one hit, but so do you! Keep your audience on the edge of their seat while having the rolling on the floor at the same time! \n\n\n - Knife_to_a_Gunfight";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ski");

            //Adds the actual passive effect to the item
           
            item.CanBeDropped = false;

            item.IgnoredByRat = true;

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.EnemyProjectileSpeedMultiplier, .1f);

            item.quality = PickupObject.ItemQuality.EXCLUDED;


        }

        public override void Pickup(PlayerController player)
        {
            player.healthHaver.OnDamaged += this.OnDamaged;
            base.Pickup(player);
        }

        protected override void Update()
        {
            try
            {
                if(this.Owner != null)
                {
                    RoomHandler currentRoom = this.Owner.CurrentRoom;
                    foreach (AIActor aiactor in currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All))
                    {   
                        if (aiactor.healthHaver != null && aiactor.CompanionOwner != this.Owner)
                        {
                            if (aiactor.EnemyGuid != "465da2bb086a4a88a803f79fe3a27677")
                            {
                                if (aiactor.healthHaver.IsBoss)
                                {
                                    aiactor.healthHaver.SetHealthMaximum(100, null, true);
                                   

                                }
                                else
                                {
                                    aiactor.healthHaver.SetHealthMaximum(1, null, true);
                                    aiactor.LocalTimeScale = 1.25f;
                                }

                            }
                            else
                            {

                                aiactor.healthHaver.SetHealthMaximum(550, null, true);

                            }

                        }

                        if (aiactor.EnemyGuid == "6b7ef9e5d05b4f96b04f05ef4a0d1b18")
                        {
                            aiactor.Transmogrify(EnemyDatabase.GetOrLoadByGuid("98fdf153a4dd4d51bf0bafe43f3c77ff"), null);
                        }
                        


                    }

                    
                }
                
            }
            catch
            {

            }

            base.Update();
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.healthHaver.OnDamaged -= this.OnDamaged;
            return base.Drop(player);
        }
        private void OnDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
        {
            this.Owner.healthHaver.Armor = 0;
            this.Owner.healthHaver.ForceSetCurrentHealth(0);
            
            this.Owner.healthHaver.Die(Vector2.zero);
        }


    }
}
