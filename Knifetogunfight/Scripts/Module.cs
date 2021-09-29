using ItemAPI;
using GungeonAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using MonoMod.RuntimeDetour;
using UnityEngine;
using Dungeonator;
using UnityEngine.UI;
using SaveAPI;


namespace Knives
{
    
    public class Module : ETGModule
    {
        public static string ZipFilePath;
        public static string FilePath;
        public static readonly string MOD_NAME = "Knife_to_a_Gunfight";
        public static readonly string VERSION = "1.0.9.7";
        public static readonly string TEXT_COLOR = "#5deba4";

        public static string LocalShaderNameGetHook(Func<PlayerController, string> orig, PlayerController self)
        {   // rollin glow stuffs
            if (!GameOptions.SupportsStencil)
            {
                return "Brave/PlayerShaderNoStencil";
            }
            if (self.name == "PlayerRollin(Clone)")
            {
                Material mat = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);
                mat.SetTexture("_MainTexture", self.sprite.renderer.material.GetTexture("_MainTex"));
                mat.SetColor("_EmissiveColor", new Color32(225, 225, 225, 225));//this is the color you want to make glow!
                mat.SetFloat("_EmissiveThresholdSensitivity", .25f); // sensitivity
                mat.SetFloat("_EmissiveColorPower", 0f);//this makes the color brighter
                mat.SetFloat("_EmissivePower", 50);// this is how much glow there is, 150 is usually high but when its only the eyes and eyes are 1 or 2 pixels, its not that high. anyway, you should play around with is.
                self.sprite.renderer.material = mat;
                return mat.shader.name;
            }
            return orig(self);
        }
       


        public override void Start()
        {
            try
            {
                SaveAPIManager.Setup("ski");
                Hooks.Init();
                ShrineFactory.Init();
                GungeonAPI.FakePrefabHooks.Init();
                GungeonAPI.Tools.Init();
                ItemAPI.FakePrefabHooks.Init();
                ItemAPI.Tools.Init();
                ItemBuilder.Init();
                //sound stuffs
                ZipFilePath = this.Metadata.Archive;
                FilePath = this.Metadata.Directory;
                //register all items and synergies. if text at the bottom doesnt fire something along the way crashed/produced and error
                //Not all scripts are loaded some are saved for a later date.


                // general passive
                Dizzyring.Register();
                Spring_roll.Register();
                Salmon_roll.Register();
                dragun_roll.Register();
                Long_roll_boots.Register();
                Rocket_boots.Register();
                Fly_Friend.Register();
                Space_hammer.Register();
                Sus_rounds.Register();
                nightmare_mode.Register();
                Fates_blessing.Register();
                daft_helm.Register();
                punk_helm.Register();
                book.Register();
                clean_soul.Register();
                stardust.Register();
                loan.Register();
                tabletech_dizzy.Register();
                SCP_323.Register();
                Super_fly.Register();
                Im_blue.Register();
                bad_attitude.Register();
                rubber_man.Register();
                Survivor.Register();
                speedster.Register();
                Danger_dance.Register();
                disco_inferno.Register();
                persuasive_bullets.Register();
                Slide_tech.Register();
                PeaceStandard.Register();
                SpeedyChamber.Register();
                ChamberofChambers.Register();
                menacing_aura.Register();
                Malware.Register();
                Queasy.Register();
                Farsighted.Register();
                ten_gallon.Register();
                BleakBubbles.Register();
                TableTech_AmpedCover.Register();
                War_paint.Register();
                Neon_bullets.Register();
                FractBullets.Register();
                ChaoticComedy.Register();
                SmokeAmmolet.Register();
                

                // general active
                Led_Maiden.Register();
                jojo_arrow.Register();
                nano_boost.Register();
                rad_board.Register();
                koolbucks.Register();
                sandvich.Register();
                dog.Register();
                power_bracer.Register();
                roundabout.Register();
                Eye_of_the_tiger.Register();
                Luft_balloons.Register();
                vodoo_kit.Register();
                BloodyNapkin.Register();
                Pig_Whistle.Register();
                shield.Register();
                AndroidReactorCore.Register();
                GnatHat.Register();
                HotelCaliforniaSpecial.Register();
                MindControlHeadband.Register();
                Dullahan_curse.Register();
                //rage called anger effect before anger existed
                Rage_shield.Register();
               
                Sheila.Register();
                HolyGrenade.Register();
                RedTape.Init();

                //Guns
                hail_2_u.Add();
                
                //fourth_wall_breaker.Add();
                Za_hando.Add();
                violin.Add();
                Queen.Add();
                Lance.Add();
                MagicHat.Add();
                Lil_Boom.Add();
                BlackStabbith.Add();
                Ball.Add();
                harpoon.Add();
                Mozam.Add();
                GunLance.Add();
                hot_coffee.Add();
                NewNewCopperChariot.Add();
                Hells_bells.Add();
                Succ.Add();
                Sheila_LMG.Add();
                TaurenTails.Add();
                ToyAK.Add();
                Neon.Add();
                Maw.Add();
                FireStrike.Add();
                //BeatDownUnder.Add();
                Catalyzer.Add();
                ChargeRifle.Add();
                Baba.Add();
                HotShot.Add();
                Typewriter.Add();
                Hippo.Add();
                MonkeyBarrel.Add();
                MBSynergyForm.Add();
                Double.Add();
                Ratapult.Add();
                Levoluer.Add();
                

                //Devtools
                noclip.Register();
                ActiveCharger.Register();
                CurtainTest.Register();

                //companions
                //BabyGoodDodoGama.Init();

                //orbitals
                Stopda.Register();
                Masks_Com.Register();
                Masks_trag.Register();
                GlassBoi.Register();

                

                //unfinished or unfunctional

                //synergies
                GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new Customsynergiesknives.tomislav() }).ToArray();
                GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new Customsynergiesknives.Daft_Punk() }).ToArray();
                GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new Customsynergiesknives.Super_Duper_Fly() }).ToArray();
                GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new Customsynergiesknives.split() }).ToArray();
                GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new Customsynergiesknives.flurry_of_blows() }).ToArray();
                GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new Customsynergiesknives.BEEES() }).ToArray();
                GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new Customsynergiesknives.nano() }).ToArray();
                GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new Customsynergiesknives.Big_problem() }).ToArray();
                GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new Customsynergiesknives.lich() }).ToArray();
                GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new Customsynergiesknives.Chariot() }).ToArray();
                GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new Customsynergiesknives.the_World_revolving() }).ToArray();
                GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new Customsynergiesknives.doubleStandard() }).ToArray();
                GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new Customsynergiesknives.Mozam_hammer() }).ToArray();
                GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new Customsynergiesknives.Mozam_fools() }).ToArray();
                GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new Customsynergiesknives.Mozam_Throw() }).ToArray();
                //GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new Customsynergiesknives.Mozam_Shatter() }).ToArray();
                GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new Customsynergiesknives.Mozam_mazoM() }).ToArray();
                GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new Customsynergiesknives.MonsterHunter() }).ToArray();
                GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new Customsynergiesknives.AC() }).ToArray();
                GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new Customsynergiesknives.DC() }).ToArray();
                GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new Customsynergiesknives.Mas_Queso() }).ToArray();
                //GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new Customsynergiesknives.Apex() }).ToArray();
                GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new Customsynergiesknives.Banana() }).ToArray();
                GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new Customsynergiesknives.BarrelBros() }).ToArray();
                CustomSynergies.Add("Apex Predator", new List<string> { "ski:monkey_barrel", "ak47" }, null, false);


                AudioResourceLoader.InitAudio();

               
                Hook hook = new Hook(
                    typeof(PlayerController).GetProperty("LocalShaderName", BindingFlags.Public | BindingFlags.Instance).GetGetMethod(),
                    typeof(Module).GetMethod("LocalShaderNameGetHook")
                    );

                tinyAmmo.Register();
                ShrineFactory.Init();

               

                //shrines
                Theatre_Twins.Add();
                ShrineFactory.PlaceBreachShrines();


                //Mac items
                //SideStep.Register();
                //Lefty.Add();
                //Righty.Add();
                //DocBar.Register();
                //SecondWind.Register();
                //Mac items

                //gunjam 2 electric boogaloo
                //Overkill.Register();
                //Volt.Add();
                //gunjam 2

                DualGunsManager.AddDual();
                SynergyFormInitialiser.AddSynergyForms();
                StaticStatusEffects.InitCustomEffects();
                IllnessStatusEffectSetup.Init();

                ETGMod.AIActor.OnPostStart = (Action<AIActor>)Delegate.Combine(ETGMod.AIActor.OnPostStart, new Action<AIActor>(AssignUnlock));
                System.Random rng = new System.Random();

               
                if (rng.Next(1,10) == 1)
                {
                    Log($"Don't bring a {MOD_NAME} v{VERSION}. You'll Win!", TEXT_COLOR);
                }
                else
                {
                    Log($"Don't bring a {MOD_NAME} v{VERSION}. You'll lose!", TEXT_COLOR);
                }
               

                List<String> tasks = new List<string>
                {
                    AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.BEAT_DRAGUN_WITH_MANIC) ? "--- Completed! Earned Mask Twins!\n" : "-- Defeat the Dragun during the Manic Theatre Challenge\n",
                    AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.AMMO_STARVED) ? "--- Completed! Earned Hipp0!\n" : "-- Have two of your guns run out of ammo at once\n",
                };
                ETGModConsole.Log("<size=100><color=#5deba4>Knife to a gunfight Unlock tasks list-</color></size>");
                foreach (String task in tasks)
                {
                    ETGModConsole.Log(task); ;
                }



                
            }
            catch (Exception e)
            {
                ETGModConsole.Log($"<color=#{TEXT_COLOR}>{MOD_NAME}: {e.Message}</color>");
                ETGModConsole.Log(e.StackTrace);

                Log(e.Message);
                Log("\t" + e.StackTrace);
                Log($"Something in Knife_to_a_gunfight broke somewhere...", TEXT_COLOR);
                Log($"If you're reading this please tell me at the gungeon discord and screenshot the error.", TEXT_COLOR);
            }

            try
            {
                //supid punt gun ruins everything

                punt.Add();
                GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[] { new Customsynergiesknives.Iron_grip() }).ToArray();

            }
            catch(Exception e)
            {
                ETGModConsole.Log($"<color=#{TEXT_COLOR}>{MOD_NAME}: {e.Message}</color>");
                ETGModConsole.Log(e.StackTrace);

                Log(e.Message);
                Log("\t" + e.StackTrace);
                Log($"The punt gun broke again...", TEXT_COLOR);
                Log($"If you're reading this please try reverifying your game files to fix the punt gun.", TEXT_COLOR);
            }


        }

        public static void LateStart(Action<Foyer> orig, Foyer self)
        {
            orig(self);
            //late after all else Init
            try
            {
              
            }
            catch (Exception e)
            {
                ETGModConsole.Log($"<color=#{TEXT_COLOR}>{MOD_NAME}: {e.Message}</color>");
                ETGModConsole.Log(e.StackTrace);

                Log(e.Message);
                Log("\t" + e.StackTrace);
                Log($"Something in Knife_to_a_gunfight broke somewhere...", TEXT_COLOR);
                Log($"If you're reading this please tell me at the gungeon discord and screenshot the error.", TEXT_COLOR);
            }
           
        }


        public static void Log(string text, string color= "#5deba4")
        {
            ETGModConsole.Log($"<color={color}>{text}</color>");
        }

        private void AssignUnlock(AIActor target)
        {
            PlayableCharacters characterIdentity = GameManager.Instance.PrimaryPlayer.characterIdentity;
            
            //Dragun unlock
            if (target.EnemyGuid == "465da2bb086a4a88a803f79fe3a27677")
            {
                target.healthHaver.OnDeath += (obj) =>
                {
                    //Dragun Kill
                    if ((GameManager.Instance.PrimaryPlayer.HasPickupID(ETGMod.Databases.Items["Manic Theatre"].PickupObjectId)))
                    {
                        AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.BEAT_DRAGUN_WITH_MANIC, true);
                    }
                   
                };
            }
        }
        public static void RunStartHook(Action<PlayerController, float> orig, PlayerController self, float invisibleDelay)
        {
            orig(self, invisibleDelay);
            self.gameObject.GetOrAddComponent<UnlockablesTracker>();
            
        }
        public override void Exit() 
        { 
        
        }
        public override void Init() 
        {

        }
    }
}
