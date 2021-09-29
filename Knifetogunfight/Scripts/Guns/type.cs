using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using ItemAPI;
using Dungeonator;
using System.Collections.Generic;
using MultiplayerBasicExample;

namespace Knives
{
    class Typewriter : AdvancedGunBehaviour
    {

        public static void Add()
        {
            // Get yourself a new gun "base" first.
            // Let's just call it "Basic Gun", and use "jpxfrd" for all sprites and as "codename" All sprites must begin with the same word as the codename. For example, your firing sprite would be named "jpxfrd_fire_001".
            Gun gun = ETGMod.Databases.Items.NewGun("Chicago Typewriter", "type");
            // "kp:basic_gun determines how you spawn in your gun through the console. You can change this command to whatever you want, as long as it follows the "name:itemname" template.
            Game.Items.Rename("outdated_gun_mods:chicago_typewriter", "ski:chicago_typewriter");
            gun.gameObject.AddComponent<Typewriter>();
            //These two lines determines the description of your gun, ".SetShortDescription" being the description that appears when you pick up the gun and ".SetLongDescription" being the description in the Ammonomicon entry. 
            gun.SetShortDescription("ImPRESSive Censorship");
            gun.SetLongDescription("This gun seeks to silence \"Radical\" views in the paper and on the streets to keep the world pure. Reloading costs 2 ammo and fires a plane, random special effect words will fire adding their effect to the plane!\n\n" +
                "Heat words will apply fire\n\n" +
                "Sick words will apply illness\n\n" +
                "Protective words will make the plane defensive\n\n" +
                "Sweet words will apply charm\n\n" +
                "Scary words will give the plane ghost friends\n\n" +
                "Hitting words will appy stun\n\n" +
                "and Spiritual words will give the plane more damage to bosses and jammed enemies!\n\n" +
                "\n\n\n - Knife_to_a_Gunfight"); ;


            gun.SetupSprite(null, "type_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 30);
            gun.SetAnimationFPS(gun.reloadAnimation, 8);
           
          
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetByEncounterName("Ak-47") as Gun, true, false);

            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.angleVariance = 5;
            gun.gunClass = GunClass.FULLAUTO;
            gun.gunHandedness = GunHandedness.TwoHanded;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            
            gun.reloadTime = 1f;
            gun.DefaultModule.numberOfShotsInClip = 30;
            gun.DefaultModule.cooldownTime = .18f;
            gun.SetBaseMaxAmmo(800);
            gun.quality = PickupObject.ItemQuality.B;
            Gun gun2 = (Gun)PickupObjectDatabase.GetById(2);
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            gun.shellCasing = gun2.shellCasing;
            gun.shellsToLaunchOnFire = 1;
            gun.PreventNormalFireAudio = true;

            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            gun.barrelOffset.transform.localPosition = new Vector3(1.3f, .4f, 0f);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 3f;
            projectile.baseData.speed *= .75f;
            projectile.baseData.range *= 1f;

           

            projectile.transform.parent = gun.barrelOffset;
            ETGMod.Databases.Items.Add(gun, null, "ANY");




        }

        protected override void OnPickedUpByPlayer(PlayerController player)
        {
           
            base.OnPickedUpByPlayer(player);
        }

       
        public System.Random rng = new System.Random();

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            if(gun.ClipShotsRemaining !=0)
            {
                int sound = rng.Next(1, 3);
                if(sound == 1)
                {
                    AkSoundEngine.PostEvent("Play_type_fire_001", base.gameObject);
                }
                else
                {
                    AkSoundEngine.PostEvent("Play_type_fire_002", base.gameObject);
                }
            }
            else
            {
                AkSoundEngine.PostEvent("Play_type_clip_empty_001", base.gameObject);
                AkSoundEngine.PostEvent("Play_type_clip_empty_001", base.gameObject);
            }

            gun.PreventNormalFireAudio = true;
        }
        public override void OnFinishAttack(PlayerController player, Gun gun)
        {


            base.OnFinishAttack(player, gun);
        }

        int stringplace = 0;
        string ChoosenWord = null;
        bool currentlydoingCombo = false;
        int color;
        bool right;
        
        public override void PostProcessProjectile(Projectile projectile)
        {
            
            int PlayerGotComboWord;
            if (currentlydoingCombo)
            {
                PlayerGotComboWord = 2;
            }
            else
            {
                PlayerGotComboWord = rng.Next(1, 24);
            }
                  
            
            if(PlayerGotComboWord != 1)
            {
                stringplace++;
                stringplace %= ApprovedString.Length;

                //standard randomness
                projectile.sprite.SetSprite(this.GetLetter(ApprovedString[stringplace]));

                

            }
            else
            {
                int selectComboEffectToAdd = rng.Next(1, 8);
               
                //delete original projectile and replace with a quick volley of a word
                
                switch (selectComboEffectToAdd)
                {
                    case 1: // fire - red
                        ChoosenWord = firelist[rng.Next(0, firelist.Count)];
                        color = 1; 
                        break;

                    case 2: // poison - green
                        ChoosenWord = poislist[rng.Next(0, poislist.Count)];
                        color = 2;
                        break;

                    case 3: // peirce - grey
                        ChoosenWord = stablist[rng.Next(0, stablist.Count)];
                        color = 3;
                        break; 

                    case 4: // love - magenta
                        ChoosenWord = charmlist[rng.Next(0, charmlist.Count)];
                        color = 4;
                        break;

                    case 5: // stun - yellow
                        ChoosenWord = stunlist[rng.Next(0, stunlist.Count)];
                        color = 5;
                        break;
                   
                    case 6:// scared - purple
                        ChoosenWord = scaredlist[rng.Next(0, scaredlist.Count)];
                        color = 6;
                        break;

                    case 7: // pure - white
                        ChoosenWord = holylist[rng.Next(0, holylist.Count)];
                        color = 7;
                        break;


                }

                //use length and direction to burst fire the word with the correct tint

                // add directionality for readability 
                PlayerController player = (PlayerController)gun.CurrentOwner;
                
                if(player.CurrentGun.CurrentAngle > 90 || player.CurrentGun.CurrentAngle < -90)
                {
                    right = false;
                }
                else
                {
                    right = true;
                }
                StartCoroutine(handlewordburst(right));
                projectile.DieInAir(true);
                
            }

            base.PostProcessProjectile(projectile);
        }
        bool didfire;
        bool didpois;
        bool didstab;
        bool didlove;
        bool didstun;
        bool didscare;
        bool didpure;
        public IEnumerator handlewordburst(bool right)
        {
            currentlydoingCombo = true;
            if (right)
            {
                ChoosenWord = Reverse(ChoosenWord);
            }

            for (int i = 0; i < ChoosenWord.Length; i++)
            {
                yield return new WaitForSecondsRealtime(.1f);
                Projectile projectile2 = ((Gun)ETGMod.Databases.Items[2]).DefaultModule.projectiles[0];
                projectile2.baseData.damage = 1f;
                GameObject gameObject2 = SpawnManager.SpawnProjectile(projectile2.gameObject, gun.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (this.Owner.CurrentGun == null) ? 0f : this.gun.CurrentOwner.CurrentGun.CurrentAngle), true);
                Projectile component2 = gameObject2.GetComponent<Projectile>();
                component2.baseData.range *= 1f;
                component2.baseData.speed *= .5f;
                component2.Owner = this.gun.CurrentOwner;
                component2.sprite.SetSprite(this.GetLetter(ChoosenWord[i]));
                switch (color)
                {
                    case 1: // fire - red
                        component2.DefaultTintColor = UnityEngine.Color.red;
                        didfire = true;
                        break;

                    case 2: // poison - green
                        component2.DefaultTintColor = UnityEngine.Color.green;
                        didpois = true;
                        break;

                    case 3: // peirce - grey
                        component2.DefaultTintColor = UnityEngine.Color.grey;
                        didstab = true;
                        break;

                    case 4: // love - magenta
                        component2.DefaultTintColor = UnityEngine.Color.magenta;
                        didlove = true;
                        break;

                    case 5: // stun - yellow
                        component2.DefaultTintColor = UnityEngine.Color.yellow;
                        didstun = true;
                        break;

                    case 6:// scared - purple
                        component2.DefaultTintColor = UnityEngine.Color.cyan;
                        didscare = true;
                        break;

                    case 7: // purify - white
                        component2.DefaultTintColor = UnityEngine.Color.white;
                        didpure = true;
                        break;
                }
                component2.AdditionalScaleMultiplier = 1.3f;
                component2.HasDefaultTint = true;
            }
           
            yield return new WaitForSecondsRealtime(1);
            ChoosenWord = null;
            currentlydoingCombo = false;
            yield break;
        }

        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
           
            if (gun.IsReloading && this.HasReloaded)
            {
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
                HasReloaded = false;
                AkSoundEngine.PostEvent("Play_type_reload_001", base.gameObject);
                player.StartCoroutine(PlaneDelay());
            }


        }

        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public IEnumerator PlaneDelay()
        {
            yield return new WaitForSecondsRealtime(.5f);
            gun.LoseAmmo(2);
            Projectile projectile = ((Gun)ETGMod.Databases.Items[477]).DefaultModule.projectiles[0];
            projectile.baseData.damage = 5f;
            GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, gun.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (this.Owner.CurrentGun == null) ? 0f : this.gun.CurrentOwner.CurrentGun.CurrentAngle), true);
            Projectile component = gameObject.GetComponent<Projectile>();
            component.baseData.range *= 1f;
            component.baseData.speed *= .5f;
            PierceProjModifier stab = component.gameObject.GetComponent<PierceProjModifier>();
            stab.penetration = 0;
            stab.penetratesBreakables = false;
            component.PenetratesInternalWalls = false;
            component.BlackPhantomDamageMultiplier = 1f;
            if (didfire)
            {
                component.AppliesFire = true;
                component.FireApplyChance = 100;
                var fireRounds = PickupObjectDatabase.GetById(295) as BulletStatusEffectItem;
                var fireEffect = fireRounds.FireModifierEffect;

                component.fireEffect = fireEffect;
                component.DefaultTintColor = UnityEngine.Color.red;
                component.HasDefaultTint = true;
            }
            if (didpois)
            {
                component.PoisonApplyChance = 100;
                component.AppliesPoison = true;
                component.DefaultTintColor = UnityEngine.Color.green;
                component.HasDefaultTint = true;
                component.healthEffect = new GameActorillnessEffect();
                
            }
            if (didstab)
            {
                component.AdditionalScaleMultiplier = 2.5f;
                component.collidesWithProjectiles = true;
                component.projectileHitHealth = 30;
                component.pierceMinorBreakables = true;
                component.baseData.range *= 2f;
                component.baseData.speed *= .45f;
                component.DefaultTintColor = UnityEngine.Color.gray;
                component.HasDefaultTint = true;
            }
            if (didlove)
            {
                component.AppliesCharm = true;
                component.CharmApplyChance = 100;
                var charmingRounds = PickupObjectDatabase.GetById(527) as BulletStatusEffectItem;
                var charmEffect = charmingRounds.CharmModifierEffect;
                component.charmEffect = charmEffect;
                component.DefaultTintColor = UnityEngine.Color.magenta;
                component.HasDefaultTint = true;
            }
            if (didstun)
            {
                component.AppliesStun = true;
                component.StunApplyChance = 100;
                component.AppliedStunDuration = 3f;
                component.DefaultTintColor = UnityEngine.Color.yellow;
                component.HasDefaultTint = true;
            }
            if (didscare)
            {
                Projectile projectile2 = ((Gun)ETGMod.Databases.Items[29]).DefaultModule.projectiles[0];
                projectile2.baseData.damage = 5f;
                GameObject gameObject2 = SpawnManager.SpawnProjectile(projectile2.gameObject, gun.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (this.Owner.CurrentGun == null) ? 0f : this.gun.CurrentOwner.CurrentGun.CurrentAngle + 18), true);
                Projectile component2 = gameObject2.GetComponent<Projectile>();
                component2.baseData.range *= 1f;
                component2.baseData.speed *= .5f;
                component2.Owner = this.Owner;

                Projectile projectile3 = ((Gun)ETGMod.Databases.Items[29]).DefaultModule.projectiles[0];
                projectile3.baseData.damage = 5f;
                GameObject gameObject3 = SpawnManager.SpawnProjectile(projectile3.gameObject, gun.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (this.Owner.CurrentGun == null) ? 0f : this.gun.CurrentOwner.CurrentGun.CurrentAngle - 18), true);
                Projectile component3 = gameObject3.GetComponent<Projectile>();
                component3.baseData.range *= 1f;
                component3.baseData.speed *= .5f;
                component3.Owner = this.Owner;
                component.DefaultTintColor = UnityEngine.Color.cyan;
            }
            if (didpure)
            {
                component.BlackPhantomDamageMultiplier = 10f;
                component.BossDamageMultiplier = 10f;
                component.DefaultTintColor = UnityEngine.Color.white;
                component.HasDefaultTint = true;
                component.OnHitEnemy += this.HolyHandler;
            }
            
            didfire = false;
            didpois = false;
            didstab = false;
            didlove = false;
            didstun = false;
            didscare = false;
            didpure = false;

            PlayerController player = (PlayerController)gun.CurrentOwner;
            component.Owner =  player;
        }
       
        public void HolyHandler(Projectile projectile,SpeculativeRigidbody enemy, bool fatal)
        {
            
            if(enemy.aiActor.healthHaver.IsBoss || enemy.aiActor.IsBlackPhantom)
            {
                if (fatal)
                {
                    AkSoundEngine.PostEvent("Play_hallelujah", base.gameObject);
                }
                enemy.aiActor.healthHaver.ApplyDamage(60, Vector2.zero, "purification", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, true);
            }
        }
      
     

        private bool HasReloaded;
        protected override void Update()
        {
            if (gun.CurrentOwner)
            {

                if (!gun.PreventNormalFireAudio)
                {
                    this.gun.PreventNormalFireAudio = true;
                }
                if (!gun.IsReloading && !HasReloaded)
                {
                    this.HasReloaded = true;
                }
            }
        }


        private string GetLetter(char c)
        {
            try
            {
                switch (c)
                {
                    case 'a':
                        return "word_projectile_A_001";
                    case 'b':
                        return "word_projectile_B_001";
                    case 'c':
                        return "word_projectile_C_001";
                    case 'd':
                        return "word_projectile_D_001";
                    case 'e':
                        return "word_projectile_B_004";
                    case 'f':
                        return "word_projectile_F_001";
                    case 'g':
                        return "word_projectile_G_001";
                    case 'h':
                        return "word_projectile_H_001";
                    case 'i':
                        return "word_projectile_I_001";
                    case 'j':
                        return "word_projectile_J_001";
                    case 'k':
                        return "word_projectile_K_001";
                    case 'l':
                        return "word_projectile_B_003";
                    case 'm':
                        return "word_projectile_M_001";
                    case 'n':
                        return "word_projectile_N_001";
                    case 'o':
                        return "word_projectile_O_001";
                    case 'p':
                        return "word_projectile_P_001";
                    case 'q':
                        return "word_projectile_Q_001";
                    case 'r':
                        return "word_projectile_R_001";
                    case 's':
                        return "word_projectile_S_001";
                    case 't':
                        return "word_projectile_B_005";
                    case 'u':
                        return "word_projectile_B_002";
                    case 'v':
                        return "word_projectile_V_001";
                    case 'w':
                        return "word_projectile_W_001";
                    case 'x':
                        return "word_projectile_X_001";
                    case 'y':
                        return "word_projectile_Y_001";
                    case 'z':
                        return "word_projectile_Z_001";
                    default:
                        return "word_projectile_Z_001";
                }
            }
            catch(Exception E)
            {   
                ETGModConsole.Log(E.ToString());
                return "word_projectile_1_001";
                
            }
        }

        public List<string> firelist = new List<string>
        {
            "fire",
            "hot",
            "burn",
            "torch",
            "sizzle",
        };

        public List<string> poislist = new List<string>
        { 
            "sick",
            "gross",
            "ew",
            "slime",
            "toxic",
        };

        public List<string> stablist = new List<string>
        {
            "shield",
            "block",
            "protect",
            "gaurd",
            "armor",
        };

        public List<string> charmlist = new List<string>
        {
            "cute",
            "charm",
            "love",
            "uwu",
            "hug",

        };

        public List<string> scaredlist = new List<string>
        {
            "worry",
            "dread",
            "fear",
            "terror",
            "anxiety",
        };


        public List<string> stunlist = new List<string>
        {
            "wham",
            "blam",
            "bam",
            "pow",
            "crack",
        };


        public List<string> holylist = new List<string>
        {
            "holy",
            "spirit",
            "cleanse",
            "purify",
            "sacntify",
        };

        public String ApprovedString = "fckvukatjcqakfxsqhir" +
                                    "gbzaxkinwxhyyxeeotuk" +
                            "dptyjgerrhkhghdoblyi" +
                            "zkuczjaagtimljfpyvps" +
                            "oztycyohlnybnivcfvgz" +
                            "lnllxaqarprvoxhhnvtm" +
                            "skzknrusetvatdgkhehg" +
                            "ldbeulzueqlwvdqlrypi" +
                            "czqpopwtoocybzrkljxu" +
                            "pvgdzfzarqupdcvcawzg" +
                            "fyilwfxxlvpybawakpoy" +
                            "gxpyxsdyaxbigbqgqmgw" +
                           "ajepexqqdenecvtzqkka" +
                            "nvbviaairyfydapqjysc" +
                            "zjskjdoeepncqytztmtn" +
                            "xusjdmdpchtcergvpado" +
                            "losdpkircugjkaoqrzkp" +
                            "ghpatujkidkiyuaunlcc" +
                            "gikblmsobiyihbzzouaw" +
                            "twztmtzqzlqlsbbmgjka" +
                            "bvbhkwflslxfostlphik" +
                            "teaakafycljfuligqxqa" +
                            "ulwehdhqklminkzzkayj" +
                            "lxnksuvsrujytxlltfje" +
                            "rryoxgnwzxqqzybfclub" +
                            "ylhjihkfdtjyccxpotfo" +
                            "mjxrnhfonxljkhitgywl" +
                            "ibqzllfymejligqbjuol" +
                            "gybfmywkqykoxqfrgfib" +
                            "gxjfnjkbkytaekurvkys" +
                            "qzcdhbkbsjatzkkkjdmj" +
                            "zecfcdnxljxromcfslsm" +
                            "swzjnshlzycytevsgiic" +
                            "ceysvsuzplqidsidpewv" +
                            "bebzgncmhhipfximweha" +
                            "reeewtxaudpbjknbjyle" +
                            "ohyroqlqnfqboomvqtfr" +
                            "isagizahnbnsuohenbis" +
                            "smgqqvbkmneviyizarjx" +
                            "rbldzvuaovniaygrhjsx" +
                            "qbktvkqbqkiezpuepktq" +
                            "kojlozqwcxwqokvlsybc" +
                            "jenwasyefqxgegafqorr" +
                            "gnjvstddbdotqredrwna" +
                            "ixldsqupssiclyjrbarp" +
                            "hldeyrudovovmivkveit" +
                            "imrtrmakoxmqnifrkpds" +
                            "tjocjbntnhdtfwppewaz" +
                            "tzaongaejuzfibnmzixw" +
                            "tpjzkiysrmnxqmguwwuh" +
                            "pfgxwlgoscskzphiiklv" +
                            "zrxinwaobnciojhikoxc" +
                            "oypfxxxevzsynrcsaqjf" +
                            "dqyfwyqvgaxllwafcllf" +
                            "bzzkbjbcuzyveyxetntb" +
                            "ohrkonffhjyofmyiiwqev" +
                            "ywwhwohptvqgidexatno" +
                            "fymbecsyoqccxhtprxde" +
                            "vxphzfeamncdwinwwpay" +
                            "fidrkrnlkaqumuewqufa" +
                            "udtvufiyqcmxzxuhevqc" +
                            "nxnmruwoucpylfjyipjk" +
                            "iafkkdsfgvxswsqqaweo" +
                            "rqdfjqnoacsypjpsodyp" +
                            "hlpbnahtlietjwgsbjae" +
                            "ntvnumdwrnbfjpxwxpui" +
                            "dpnnnlnqyjwwbrklfilj" +
                            "rzfxhcoqepfbmfwfdnsq" +
                            "dktbwueojjrrljdveoah" +
                            "enyeqfltylnfddgarcjq" +
                            "zmrzjvduylzfyqqxycvj" +
                            "fmkznexedtfkoxflzcpz" +
                            "bvdvmrjzmsonghmizrtm" +
                            "tjzzjwnwxcdsorkcjarj" +
                            "owxluzpldnfrsfplemnw" +
                            "hgypuddxmgldlewtbfnt" +
                            "urtozmqgwmstwjzjmeqb" +
                            "jdcowokkfccdxidcsxls" +
                            "qhnesjcaclfjuwyjknnf" +
                            "ldgmxiqkiqolxppgvxma" +
                            "ibhrcadleegdpcdgocft" +
                            "mutodjskonxabusiaeii" +
                            "utfudfseptylzmkksazh" +
                            "jogrnqeiimnkknybramd" +
                            "sbznrsnmfphqqpxtzpaw" +
                            "xafdksnnxizjfcnrzkut" +
                            "uosytgusrfulziwlowka" +
                            "atryxeuamfukqihbrzkb" +
                            "hqzxefwlnmklmcvltxpl" +
                            "rrehehybsdtnwhmugvhx" +
                            "zzjpnjghiyvrnmqojfhz" +
                            "ilptkdrppzjivpvutkyt" +
                            "jplpecjgihwfycdpzijj" +
                            "hhbcplpfuamhqdgddmdr" +
                            "zfahuezhtgzlrgmtajat" +
                            "ncbddrsjzjwggsxzhbkl" +
                            "saepqpsjvzfnesbjvycw" +
                            "tkbxrmsfsbqlvzomjlkxv" +
                            "jqglxarvvrpkycsmyenm";
    }
}