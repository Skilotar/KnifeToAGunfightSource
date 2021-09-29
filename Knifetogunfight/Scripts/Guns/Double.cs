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
using MonoMod;

namespace Knives
{	
    class Double :AdvancedGunBehaviour
    {
		
		public static void Add()
		{
			System.Random rng = new System.Random();
			Gun gun = ETGMod.Databases.Items.NewGun("Messenger", "tap");
			Game.Items.Rename("outdated_gun_mods:messenger", "ski:messenger");
			gun.gameObject.AddComponent<Double>();
			gun.SetShortDescription("There and back");
			gun.SetLongDescription("Trigger fires on press and on release. A high action coach gun used by the postal messengers of the gundrominian region in order to transfer parcels safely. \n\n- Knife_to_a_gunfight");
			gun.SetupSprite(null, "tap_idle_001", 1);
			GunExt.SetAnimationFPS(gun, gun.shootAnimation, 9);
			GunExt.SetAnimationFPS(gun, gun.reloadAnimation, 7);
			GunExt.SetAnimationFPS(gun, gun.idleAnimation, 1);
			GunExt.SetAnimationFPS(gun, gun.emptyReloadAnimation, 8);

			for (int i = 0; i < 10; i++)
			{
				gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(157) as Gun, true, false);
				gun.gunSwitchGroup = (PickupObjectDatabase.GetById(157) as Gun).gunSwitchGroup;

			}
			int counter = 0;
			foreach (ProjectileModule projectileModule in gun.Volley.projectiles)
			{	
				projectileModule.ammoCost = 1;
				projectileModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
				projectileModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
				projectileModule.cooldownTime = 1;
				
				projectileModule.numberOfShotsInClip = 2;
				Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(projectileModule.projectiles[0]);
				projectile.gameObject.SetActive(false);
				projectileModule.projectiles[0] = projectile;
				projectile.baseData.damage = 3f;
				if (counter <= 5) // creates a distinct pattern of two arching rows
				{
					projectileModule.angleVariance = 8f;
					
				}
				if (counter > 5)
				{
					projectileModule.angleVariance = 4f;
					projectile.baseData.speed = projectile.baseData.speed + 5f;
				}
				projectile.AdditionalScaleMultiplier = .5f;
				FakePrefab.MarkAsFakePrefab(projectile.gameObject);
				UnityEngine.Object.DontDestroyOnLoad(projectile);
				
				gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SHOTGUN;
				gun.DefaultModule.projectiles[0] = projectile;
				bool flag = projectileModule != gun.DefaultModule;
				counter++;
			}
			
			gun.reloadTime = 1.5f;
			gun.SetBaseMaxAmmo(2000);
			gun.CurrentAmmo = 2000;
			gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(157) as Gun).muzzleFlashEffects;
			gun.quality = PickupObject.ItemQuality.B;
			gun.encounterTrackable.EncounterGuid = "Pop pop";
			gun.gunClass = GunClass.SHOTGUN;

			tk2dSpriteAnimationClip fireClip = gun.sprite.spriteAnimator.GetClipByName("tap_reload"); // 4 frames
			float[] offsetsX = new float[] { 0.0f, 0.1f, 0.1f, 0.1f };
			float[] offsetsY = new float[] { 0f, -.4f, -.4f, -.4f};
			for (int i = 0; i < offsetsX.Length && i < offsetsY.Length && i < fireClip.frames.Length; i++)
			{
				int id = fireClip.frames[i].spriteId;
				fireClip.frames[i].spriteCollection.spriteDefinitions[id].position0.x += offsetsX[i];
				fireClip.frames[i].spriteCollection.spriteDefinitions[id].position0.y += offsetsY[i];
				fireClip.frames[i].spriteCollection.spriteDefinitions[id].position1.x += offsetsX[i];
				fireClip.frames[i].spriteCollection.spriteDefinitions[id].position1.y += offsetsY[i];
				fireClip.frames[i].spriteCollection.spriteDefinitions[id].position2.x += offsetsX[i];
				fireClip.frames[i].spriteCollection.spriteDefinitions[id].position2.y += offsetsY[i];
				fireClip.frames[i].spriteCollection.spriteDefinitions[id].position3.x += offsetsX[i];
				fireClip.frames[i].spriteCollection.spriteDefinitions[id].position3.y += offsetsY[i];
			}

			tk2dSpriteAnimationClip fireClip2 = gun.sprite.spriteAnimator.GetClipByName("tap_empty_reload"); // 7 frames
			float[] offsetsX2 = new float[] { 0.0f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0f };
			float[] offsetsY2 = new float[] { 0f, -.4f, -.4f, -.4f, -.4f, -.4f, -.4f };
			for (int i = 0; i < offsetsX2.Length && i < offsetsY2.Length && i < fireClip2.frames.Length; i++)
			{
				int id = fireClip2.frames[i].spriteId;
				fireClip2.frames[i].spriteCollection.spriteDefinitions[id].position0.x += offsetsX2[i];
				fireClip2.frames[i].spriteCollection.spriteDefinitions[id].position0.y += offsetsY2[i];
				fireClip2.frames[i].spriteCollection.spriteDefinitions[id].position1.x += offsetsX2[i];
				fireClip2.frames[i].spriteCollection.spriteDefinitions[id].position1.y += offsetsY2[i];
				fireClip2.frames[i].spriteCollection.spriteDefinitions[id].position2.x += offsetsX2[i];
				fireClip2.frames[i].spriteCollection.spriteDefinitions[id].position2.y += offsetsY2[i];
				fireClip2.frames[i].spriteCollection.spriteDefinitions[id].position3.x += offsetsX2[i];
				fireClip2.frames[i].spriteCollection.spriteDefinitions[id].position3.y += offsetsY2[i];
			}
			ETGMod.Databases.Items.Add(gun, null, "ANY");


		}

		public System.Random rand = new System.Random();
		public override void PostProcessProjectile(Projectile projectile)
		{
			
		}


        public override void OnReload(PlayerController player, Gun gun)
        {
			gun.shellsToLaunchOnReload = gun.ClipCapacity - gun.ClipShotsRemaining;
			base.OnReload(player, gun);
			gun.reloadTime = 1.5f;
		}

		
		public bool holding;
		protected override void Update()
		{
			base.Update();
			if (this.Owner != null)
			{

				BraveInput instanceForPlayer = BraveInput.GetInstanceForPlayer((this.gun.CurrentOwner as PlayerController).PlayerIDX);
				if (instanceForPlayer.ActiveActions.ShootAction.IsPressed && Time.timeScale != 0 && gun.ClipShotsRemaining != 0 && !instanceForPlayer.ActiveActions.MapAction.IsPressed)
				{
					holding = true;
					gun.reloadTime = .75f;

				}
				if (!instanceForPlayer.ActiveActions.ShootAction.IsPressed && holding && Time.timeScale != 0 && !instanceForPlayer.ActiveActions.MapAction.IsPressed)
				{
					gun.reloadTime = 1.5f;
					gun.ClearCooldowns();
					gun.Attack();
					holding = false;
				}

				
			}

		}


		private void OnDestroy()
		{

		}

	}
}



