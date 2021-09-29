using System;
using System.Collections.Generic;
using Gungeon;
using ItemAPI;
using UnityEngine;
using DirectionType = DirectionalAnimation.DirectionType;
using AnimationType = ItemAPI.CompanionBuilder.AnimationType;
using System.Collections;
using Dungeonator;
using System.Linq;

using GungeonAPI;


namespace Knives
{
    public class BabyGoodDodoGama : CompanionItem
    {
       
        private static readonly string guid = "ski_Dodogama_original";
        private static GameObject prefab = new GameObject();
        public static void Init()
        {
            string itemName = "Baby Good Dodogama";
            string resourceName = "Knives/Resources/Baby_good_dodo_main";
            GameObject obj = new GameObject();
            BabyGoodDodoGama item = obj.AddComponent<BabyGoodDodoGama>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Too Cute to hunt";
            string longDesc = "";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ski");
            item.quality = PickupObject.ItemQuality.C;
            item.CompanionGuid = guid;



            BabyGoodDodoGama.BuildPrefab();
          

        }

        public static void BuildPrefab()
        {
           
            
                
            ETGModConsole.Log("Startup");
            prefab = ItemAPI.CompanionBuilder.BuildPrefab("Baby Good Dodogama", guid, "Knives/Resources/Companions/Dodogama/IdleRight/Dodogama_idle_right_001", new IntVector2(0, 0), new IntVector2(29, 15));
            ETGModConsole.Log("build");
            //CompanionController companion = prefab.GetOrAddComponent<BabyGoodDodoGama>();
            
            //companion.aiActor.MovementSpeed = 5f;

            //Breaks HERE !!
            
            
            prefab.AddAnimation("idle_right", "Knives/Resources/Companions/Dodogama/IdleRight", 5, AnimationType.Idle, DirectionType.TwoWayHorizontal);
            ETGModConsole.Log("ir");
            prefab.AddAnimation("idle_left", "Knives/Resources/Companions/Dodogama/IdleLeft", 5, AnimationType.Idle, DirectionType.TwoWayHorizontal);
            
            prefab.AddAnimation("run_right", "Knives/Resources/Companions/Dodogama/RunRight", 7, AnimationType.Move, DirectionType.TwoWayHorizontal);
            
            prefab.AddAnimation("run_left", "Knives/Resources/Companions/Dodogama/RunLeft", 7, AnimationType.Move, DirectionType.TwoWayHorizontal);
            
            prefab.AddAnimation("attack_right", "Knives/Resources/Companions/Dodogama/AttackRight", 16, AnimationType.Idle, DirectionType.TwoWayHorizontal);
            
            prefab.AddAnimation("attack_left", "Knives/Resources/Companions/Dodogama/AttackLeft", 16, AnimationType.Idle, DirectionType.TwoWayHorizontal);
            
            var bs = prefab.GetComponent<BehaviorSpeculator>();
            ETGModConsole.Log("behaviour");
            bs.MovementBehaviors.Add(new CompanionFollowPlayerBehavior() { IdleAnimations = new string[] { "idle" } });
            ETGModConsole.Log("follow");
            bs.MovementBehaviors.Add(new SimpleCompanionBehaviours.SimpleCompanionApproach(4));
            ETGModConsole.Log("approach");
            bs.AttackBehaviors.Add(new RobotAttackBehaviour());
            ETGModConsole.Log("attack");
            ETGModConsole.Log("successfully built");
        }

        public static GameObject dodo;
        public override void Pickup(PlayerController player)
        {
           
            base.Pickup(player);
          
        }

        public override DebrisObject Drop(PlayerController player)
        {
           
            return base.Drop(player);
        }

        public class RobotAttackBehaviour : AttackBehaviorBase
        {
            public override BehaviorResult Update()
            {
                base.DecrementTimer(ref this.attackTimer, false);
                if (Owner == null)
                {
                    if (this.m_aiActor && this.m_aiActor.CompanionOwner)
                    {
                        Owner = this.m_aiActor.CompanionOwner;
                    }
                    else
                    {
                        Owner = GameManager.Instance.BestActivePlayer;
                    }
                }
                if (m_aiActor && m_aiActor.OverrideTarget == null)
                {
                    PickNewTarget();
                }
                BehaviorResult result;
                if (this.m_aiActor && this.m_aiActor.OverrideTarget)
                {
                    SpeculativeRigidbody overrideTarget = this.m_aiActor.OverrideTarget;
                    this.isInRange = (Vector2.Distance(this.m_aiActor.specRigidbody.UnitCenter, overrideTarget.UnitCenter) <= this.DesiredDistance);
                    if (isInRange)
                    {
                        if (overrideTarget != null && this.attackTimer == 0 && !isFiring)
                        {
                            this.m_aiActor.StartCoroutine(fail());
                            result = BehaviorResult.SkipAllRemainingBehaviors;
                        }
                        else result = BehaviorResult.Continue;
                    }
                    else result = BehaviorResult.Continue;
                }
                else result = BehaviorResult.Continue;
                return result;
            }

            public override float GetMaxRange()
            {
                return 5f;
            }
            public override float GetMinReadyRange()
            {
                return 5f;
            }
            public override bool IsReady()
            {
                AIActor aiActor = this.m_aiActor;
                bool flag;
                if (aiActor == null) flag = true;
                else
                {
                    SpeculativeRigidbody targetRigidbody = aiActor.TargetRigidbody;
                    Vector2? vector = (targetRigidbody != null) ? new Vector2?(targetRigidbody.UnitCenter) : null;
                    flag = (vector == null);
                }
                bool flag2 = flag;
                return !flag2 && Vector2.Distance(this.m_aiActor.specRigidbody.UnitCenter, this.m_aiActor.TargetRigidbody.UnitCenter) <= this.GetMinReadyRange();
            }

            private void PickNewTarget()
            {
                if (this.m_aiActor != null)
                {
                    if (this.Owner == null)
                    {
                        if (this.m_aiActor && this.m_aiActor.CompanionOwner)
                        {
                            Owner = this.m_aiActor.CompanionOwner;
                        }
                        else
                        {
                            Owner = GameManager.Instance.BestActivePlayer;
                        }
                    }
                    this.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear, ref this.roomEnemies);
                    for (int i = 0; i < this.roomEnemies.Count; i++)
                    {
                        AIActor aiactor = this.roomEnemies[i];
                        if (aiactor.IsHarmlessEnemy || !aiactor.IsNormalEnemy || aiactor.healthHaver.IsDead || aiactor == this.m_aiActor || aiactor.EnemyGuid == "ba928393c8ed47819c2c5f593100a5bc")
                        { this.roomEnemies.Remove(aiactor); }
                    }
                    if (this.roomEnemies.Count == 0) { this.m_aiActor.OverrideTarget = null; }
                    else
                    {
                        AIActor aiActor = this.m_aiActor;
                        AIActor aiactor2 = this.roomEnemies[UnityEngine.Random.Range(0, this.roomEnemies.Count)];
                        aiActor.OverrideTarget = ((aiactor2 != null) ? aiactor2.specRigidbody : null);
                    }
                }
            }

            private List<AIActor> roomEnemies = new List<AIActor>();

            private PlayerController Owner;

            public float TickDelay = 5f;

            public float DesiredDistance = 5f;

            private float attackTimer;

            private bool isInRange;

            private bool isFiring = false;

            private IEnumerator fail()
            {
                isFiring = true;
                this.m_aiActor.MovementSpeed = 0;
                this.m_aiActor.aiAnimator.PlayUntilFinished("attack_right");
                bool BadCode = true;
                while (this.m_aiActor.aiAnimator.IsPlaying("attack_right"))
                {
                    if (this.m_aiActor.spriteAnimator.CurrentFrame == 4 && BadCode)
                    {
                        BadCode = false;
                        bool PlayerHasSynergy = false;
                        if (Owner)
                        {
                            PlayerHasSynergy = Owner.PlayerHasActiveSynergy("Robot Influence");
                        }
                        for (int counter = 0; counter < (PlayerHasSynergy ? 18 : 12); counter++)
                        {
                            Projectile projectile2 = ((Gun)ETGMod.Databases.Items[88]).DefaultModule.projectiles[0];
                            GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, this.m_aiActor.sprite.WorldCenter, Quaternion.Euler(0f, 0f, ((PlayerHasSynergy ? 20 : 30) * counter)), true);
                            Projectile component = gameObject.GetComponent<Projectile>();
                            if (component != null)
                            {
                                component.baseData.damage -= 1;
                                component.Owner = Owner;
                                component.Shooter = Owner.specRigidbody;
                                Owner.DoPostProcessProjectile(component);

                                BulletStatusEffectItem batteryBullets = (BulletStatusEffectItem)PickupObjectDatabase.GetById(410);
                                CoreDamageTypes damageType = batteryBullets.DamageTypesToAdd;
                                if (batteryBullets.AddsDamageType)
                                {
                                    component.damageTypes |= damageType;
                                }

                                GameObject particles = batteryBullets.ParticlesToAdd;
                                if (particles)
                                {
                                    GameObject particleObject = SpawnManager.SpawnVFX(particles, true);
                                    particleObject.transform.parent = component.transform;
                                    particleObject.transform.localPosition = new Vector3(0f, 0f, 0.5f);
                                    ParticleKiller particleComponent = particleObject.GetComponent<ParticleKiller>();
                                    if (particleComponent != null)
                                    {
                                        particleComponent.Awake();
                                    }
                                }
                            }
                        }
                    }
                    yield return null;
                }
                isFiring = false;
                this.m_aiActor.MovementSpeed = 5;
                this.attackTimer = this.TickDelay;
                if (Owner && Owner.PlayerHasActiveSynergy("If the robot's paid"))
                {
                    float Nuts = Mathf.Min((float)Owner.carriedConsumables.Currency / 150, 1);
                    this.attackTimer -= Nuts;
                }
                yield break;
            }
        }
    }
}

