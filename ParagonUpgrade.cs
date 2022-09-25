using MelonLoader;
using BTD_Mod_Helper;
using moeztower;
using BTD_Mod_Helper.Api.Towers;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Utils;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Attack;
using BTD_Mod_Helper.Extensions;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Api.Display;
using Assets.Scripts.Unity.Display;
using System.Linq;
using System.Drawing;
using Assets.Scripts.Models.Towers.Behaviors.Emissions;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Unity;
using Assets.Scripts.Simulation.Towers;
using Assets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Assets.Scripts.Unity.Towers.Upgrades;
using Assets.Scripts.Models.Towers.Behaviors.Abilities;
using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using System.Threading;
using System.Threading.Tasks;
using UnhollowerBaseLib;

namespace moezparagon
{
    public class SuperMoez : ModParagonUpgrade<MoezTrigui>
    {
        public override int Cost => 1500000;
        public override string Description => "Turns into a very powerful monkey that acts like the 5-5-5";
        public override string DisplayName => "Super Moez";
        public override bool RemoveAbilities => false;
        public override void ApplyUpgrade(TowerModel TowerModel)
        {
            var attackModel = TowerModel.GetAttackModel();
            var projectile = attackModel.weapons[0].projectile;
            projectile.GetBehavior<DamageModel>().maxDamage = 50;
            projectile.GetBehavior<DamageModel>().damage = 40;
            attackModel.weapons[0].Rate /= 4;
        }
    }
}