
using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brave;
using UnityEngine;

namespace Knives
{
    class IllnessStatusEffectSetup
    {
    

        public static void Init()
        {

            GameActorillnessEffect Standill = StatusEffectHelper.GenerateillnessEffect(2, 1, true, new Color(0.46f, 0.59f, 0.13f), true, new Color(0.46f, 0.59f, 0.13f));
            StaticStatusEffects.StandardillnessEffect = Standill;
        }

     
    }
    public class GameActorillnessEffect : GameActorHealthEffect
    {
        public GameActorillnessEffect()
        {
            this.DamagePerSecondToEnemies = 4f;
            TintColor = new Color(0.46f, 0.59f, 0.13f);
            DeathTintColor = new Color(0.46f, 0.59f, 0.13f);
            this.AppliesTint = true;
            this.AppliesDeathTint = true;
            effectIdentifier = "illness";
            duration = 5;
            stackMode = EffectStackingMode.Refresh;
        }
        public float rampPerSecond = 1f;
        public float MaxDamagePerSecond = 60;
        public override void EffectTick(GameActor actor, RuntimeGameActorEffectData data)
        { 
            if (this.DamagePerSecondToEnemies < MaxDamagePerSecond)
            {
                this.DamagePerSecondToEnemies += rampPerSecond * Time.deltaTime;


            }
            else
            {
                this.DamagePerSecondToEnemies = MaxDamagePerSecond;
            }
            base.EffectTick(actor, data);
        }
    }
}
