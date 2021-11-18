using RoR2;
using R2API;
using BepInEx;
using UnityEngine;
using R2API.Utils;
using MonoMod.Cil;
using BepInEx.Configuration;

namespace GlassConfig
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.Moffein.GlassConfig", "Glass Config", "1.0.0")]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.DifferentModVersionsAreOk)]
    public class GlassConfig : BaseUnityPlugin
    {
        public static float damageMult = 5f;
        public static float healthFactor = 10f;
        public void Awake()
        {
            ReadConfig();

            IL.RoR2.CharacterBody.RecalculateStats += (il) =>
            {
                ILCursor c = new ILCursor(il);
                c.GotoNext(
                     x => x.MatchLdloc(46),
                     x => x.MatchBrfalse(out _)
                    );
                c.GotoNext(
                     x => x.MatchLdcR4(5f)
                    );
                c.Next.Operand = damageMult;

                c.GotoNext(
                     x => x.MatchLdloc(46),
                     x => x.MatchBrfalse(out _)
                    );
                c.GotoNext(
                     x => x.MatchLdcR4(10f)
                    );
                c.Next.Operand = healthFactor;
            };
        }
        private void ReadConfig()
        {
            damageMult = base.Config.Bind<float>(new ConfigDefinition("General", "Damage Multiplier"), 5f, new ConfigDescription("Multiply damage by this number when Glass is active.")).Value;
            healthFactor = base.Config.Bind<float>(new ConfigDefinition("General", "Health Divisor"), 10f, new ConfigDescription("Divide health by this number when Glass is active.")).Value;
            //Prevent divide by zero
            if (healthFactor <= 0f)
            {
                healthFactor = 1f;
            }
        }
    }
}
