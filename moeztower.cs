using moeztower;
using MelonLoader;
using BTD_Mod_Helper;
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
using Assets.Scripts.Models.Towers.Filters;

[assembly: MelonInfo(typeof(moeztower.moeztower), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
namespace moeztower
{
    public class MoezTrigui : ModTower
    {
        public override string BaseTower => TowerType.DartMonkey;
        public override string TowerSet => TowerSetType.Military;
        public override string Description => "throws gamepads at bloons that have good damage and great pierce";
        public override int Cost => 6000;
        public override int TopPathUpgrades => 5;
        public override int MiddlePathUpgrades => 5;
        public override int BottomPathUpgrades => 5;
        public override ParagonMode ParagonMode => ParagonMode.Base555;
        public override void ModifyBaseTowerModel(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            towerModel.range += 20;
            attackModel.range += 20;
            attackModel.weapons[0].Rate = 0.25f;
            var projectile = attackModel.weapons[0].projectile;
            projectile.GetBehavior<DamageModel>().maxDamage = 12;
            projectile.GetBehavior<DamageModel>().damage = 8;
            projectile.ApplyDisplay<GamepadDisplay>();
            projectile.maxPierce += 15;
            projectile.pierce += 9;
        }
    }
    public class moeztower : BloonsTD6Mod
    {
        public override void OnApplicationStart()
        {
            ModHelper.Msg<moeztower>("moeztower loaded!");
        }
    }
    public class MoezTriguiBaseDisplay : ModTowerDisplay<MoezTrigui>
    {
        public override string BaseDisplay => GetDisplay(TowerType.BoomerangMonkey);
        public override bool UseForTower(int[] tiers)
        {
            return tiers[1] < 3;
        }

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            SetMeshTexture(node, Name);
        }
    }
    public class ProGamerDisplay : ModTowerDisplay<MoezTrigui>
    {
        public override string BaseDisplay => GetDisplay(TowerType.Benjamin);
        public override bool UseForTower(int[] tiers)
        {
            return tiers[1] >= 3;
        }

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            SetMeshTexture(node, Name);
        }
    }
    public class FasterThrowing : ModUpgrade<MoezTrigui>
    {
        public override int Path => TOP;
        public override int Tier => 1;
        public override int Cost => 2100;
        public override string Description => "attacks faster";
        public override void ApplyUpgrade(TowerModel TowerModel)
        {
            var attackModel = TowerModel.GetAttackModel();
            attackModel.weapons[0].Rate /= 1.5f;
        }
    }
    public class SeekingGamepads : ModUpgrade<MoezTrigui>
    {
        public override int Path => TOP;
        public override int Tier => 2;
        public override int Cost => 7500;
        public override string Description => "gamepads automaticly follow bloons(act like seeking shurikens)";
        public override void ApplyUpgrade(TowerModel TowerModel)
        {
            var attackModel = TowerModel.GetAttackModel();
            var projectile = attackModel.weapons[0].projectile;
            projectile.GetBehavior<TravelStraitModel>().Lifespan = 10;
            var behavior = new TrackTargetModel("SeekingGamepad", 999, true, true, 360, true, 999, true, true);
            projectile.AddBehavior(behavior);
        }
    }
    public class DoubleShot : ModUpgrade<MoezTrigui>
    {
        public override int Path => TOP;
        public override int Tier => 3;
        public override int Cost => 10500;
        public override string Description => "Throws two gamepads per shot";
        public override void ApplyUpgrade(TowerModel TowerModel)
        {
            TowerModel.GetWeapon().emission = new ArcEmissionModel("DoubleGamepad", 2, 0, 10, null, false);
        }
    }
    public class QuadShot : ModUpgrade<MoezTrigui>
    {
        public override int Path => TOP;
        public override int Tier => 4;
        public override int Cost => 40700;
        public override string Description => "you know where this is going already";
        public override void ApplyUpgrade(TowerModel TowerModel)
        {
            TowerModel.GetWeapon().emission = new ArcEmissionModel("QuadGamepad", 4, 0, 10, null, false);
        }
    }
    public class Bullethell : ModUpgrade<MoezTrigui>
    {
        public override int Path => TOP;
        public override int Tier => 5;
        public override int Cost => 135800;
        public override string Description => "throws 8 gamepads per shot+double attack speed";
        public override void ApplyUpgrade(TowerModel TowerModel)
        {
            var attackModel = TowerModel.GetAttackModel();
            TowerModel.GetWeapon().emission = new ArcEmissionModel("BulletHellGamepad", 8, 0, 10, null, false);
            attackModel.weapons[0].Rate /= 2;
        }
    }
    public class LongerRange : ModUpgrade<MoezTrigui>
    {
        public override int Path => MIDDLE;
        public override int Tier => 1;
        public override int Cost => 2000;
        public override string Description => "reaches much longer";
        public override void ApplyUpgrade(TowerModel TowerModel)
        {
            var attackModel = TowerModel.GetAttackModel();
            TowerModel.range += 20;
            attackModel.range += 20;
        }
    }
    public class GamerEyes : ModUpgrade<MoezTrigui>
    {
        public override int Path => MIDDLE;
        public override int Tier => 2;
        public override int Cost => 1800;
        public override string Description => "reaches even longer and can hit camo bloons";
        public override int Priority => -1;
        public override void ApplyUpgrade(TowerModel TowerModel)
        {
            TowerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            var attackModel = TowerModel.GetAttackModel();
            TowerModel.range += 10;
            attackModel.range += 10;
        }
    }
    public class ProGamer : ModUpgrade<MoezTrigui>
    {
        public override int Path => MIDDLE;
        public override int Tier => 3;
        public override int Cost => 17000;
        public override string Description => "turns into a gamer that has more damage, more pierce,longer range and faster attack speed";
        public override void ApplyUpgrade(TowerModel TowerModel)
        {
            var attackModel = TowerModel.GetAttackModel();
            var projectile = attackModel.weapons[0].projectile;
            TowerModel.range *= 1.25f;
            attackModel.range *= 1.25f;
            attackModel.weapons[0].Rate /= 1.25f;
            projectile.GetBehavior<DamageModel>().maxDamage *= 1.25f;
            projectile.GetBehavior<DamageModel>().damage *= 1.25f;
            projectile.maxPierce *= 1.25f;
            projectile.pierce *= 1.25f;
        }
    }
   public class SweatyGamer : ModUpgrade<MoezTrigui>
   {
        public override int Path => MIDDLE;
        public override int Tier => 4;
        public override int Cost => 30000;
        public override string Description => "he better not throw that game(gets sweat ability:attacks super fast with double damage and pierce";
        public override void ApplyUpgrade(TowerModel TowerModel)
        {
            var ability = new AbilityModel("Sweat", "Sweat",
                "attacks super fast with double damage and pierce", 1, 0,
                GetSpriteReference(Icon), 44f, null, false, false, null,
                0, 0, 9999999, false, false);
            var activateAttackModel = new ActivateAttackModel("Sweat", 10, true,
            new Il2CppReferenceArray<AttackModel>(1), true, false, false, false, false);
            ability.AddBehavior(activateAttackModel);
            ability.icon = GetSpriteReference(mod, "Sweat-Icon");
            TowerModel.AddBehavior(ability);
            var attackModel = activateAttackModel.attacks[0] = TowerModel.GetAttackModel().Duplicate();
            var projectile = attackModel.weapons[0].projectile;
            projectile.ApplyDisplay<GamepadDisplay>();
            projectile.GetBehavior<DamageModel>().maxDamage *= 2;
            projectile.GetBehavior<DamageModel>().damage *= 2;
            attackModel.weapons[0].Rate /= 4;
        }
    }
    public class SuperGamer : ModUpgrade<MoezTrigui>
    {
        public override int Path => MIDDLE;
        public override int Tier => 5;
        public override int Cost => 110000;
        public override string Description => "sweat becomes permenant and ability is 2 times more effective";
        public override void ApplyUpgrade(TowerModel TowerModel)
        {
            var attackModel = TowerModel.GetAttackModel();
            var projectile = attackModel.weapons[0].projectile;
            projectile.GetBehavior<DamageModel>().maxDamage *= 2;
            projectile.GetBehavior<DamageModel>().damage *= 2;
            attackModel.weapons[0].Rate /= 4;
            TowerModel.RemoveBehavior<AbilityModel>();
            var ability2 = new AbilityModel("SuperSweat", "Sweat",
            "attacks ultra fast with quad damage and pierce", 1, 0,
            GetSpriteReference(Icon), 40f, null, false, false, null,
            0, 0, 9999999, false, false);
            var activateAttackModel = new ActivateAttackModel("SuperSweat", 15, true,
            new Il2CppReferenceArray<AttackModel>(1), true, false, false, false, false);
            ability2.AddBehavior(activateAttackModel);
            TowerModel.AddBehavior(ability2);
            var attack = activateAttackModel.attacks[0] = TowerModel.GetAttackModel().Duplicate();
            var proj = attackModel.weapons[0].projectile;
            proj.ApplyDisplay<GamepadDisplay>();
            proj.GetBehavior<DamageModel>().maxDamage *= 4;
            proj.GetBehavior<DamageModel>().damage *= 4;
            attack.weapons[0].Rate /= 8;
        }
    }
    public class StrongerThrows : ModUpgrade<MoezTrigui>
    {
        public override int Path => BOTTOM;
        public override int Tier => 1;
        public override int Cost => 3700;
        public override string Description => "throws gamepads very strong that they deal 25% moez damage and pierce";
        public override void ApplyUpgrade(TowerModel TowerModel)
        {
            var attackModel = TowerModel.GetAttackModel();
            var projectile = attackModel.weapons[0].projectile;
            projectile.GetBehavior<DamageModel>().maxDamage *= 1.25f;
            projectile.GetBehavior<DamageModel>().damage *= 1.25f;
            projectile.maxPierce *= 1.25f;
            projectile.pierce *= 1.25f;
        }
    }
    public class SuperThrow : ModUpgrade<MoezTrigui>
    {
        public override int Path => BOTTOM;
        public override int Tier => 2;
        public override int Cost => 2200;
        public override string Description => "the throws are super strong that they cut through lead bloons(attacks also have 10% moez damage and pierce)";
        public override void ApplyUpgrade(TowerModel TowerModel)
        {
            var attackModel = TowerModel.GetAttackModel();
            var projectile = attackModel.weapons[0].projectile;
            projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            projectile.GetBehavior<DamageModel>().maxDamage *= 1.1f;
            projectile.GetBehavior<DamageModel>().damage *= 1.1f;
            projectile.maxPierce *= 1.1f;
            projectile.pierce *= 1.1f;
        }
    }
    public class MoabDestroyer : ModUpgrade<MoezTrigui>
    {
        public override int Path => BOTTOM;
        public override int Tier => 3;
        public override int Cost => 8900;
        public override string Description => "does double damage to moab class bloons";
        public override void ApplyUpgrade(TowerModel TowerModel)
        {
            var attackModel = TowerModel.GetAttackModel();
            var projectile = attackModel.weapons[0].projectile;
            var damage = projectile.GetBehavior<DamageModel>().damage;
            projectile.AddBehavior(new DamageModifierForTagModel("MoabClassDamage", "Moabs", 1, damage, false, true));
        }
    }
    public class LayerPierce : ModUpgrade<MoezTrigui>
    {
        public override int Path => BOTTOM;
        public override int Tier => 4;
        public override int Cost => 39500;
        public override string Description => "the stronger the MOAB,the more damage it does(stacks on moab destroyer)and deals 50% more damage to fortified bloons";
        public override void ApplyUpgrade(TowerModel TowerModel)
        {
            var attackModel = TowerModel.GetAttackModel();
            var projectile = attackModel.weapons[0].projectile;
            var damage = projectile.GetBehavior<DamageModel>().damage;
            projectile.AddBehavior(new DamageModifierForTagModel("FortifiedDamage", "Fortified", 1, damage * 0.5f, false, true));
            projectile.AddBehavior(new DamageModifierForTagModel("BfbBonusDamage", "Bfb", 1, damage * 1, false, true));
            projectile.AddBehavior(new DamageModifierForTagModel("ZomgBonusDamage", "Zomg", 1, damage, false, true));
            projectile.AddBehavior(new DamageModifierForTagModel("BadBonusDamage", "Bad", 1, damage * 2, false, true));
        }
    }
    public class OneHitKO : ModUpgrade<MoezTrigui>
    {
        public override int Path => BOTTOM;
        public override int Tier => 5;
        public override int Cost => 225000;
        public override string Description => "Insta-kills any bloon/moab it hits but attack speed becomes very slow and has half pierce";
        public override void ApplyUpgrade(TowerModel TowerModel)
        {
            var attackModel = TowerModel.GetAttackModel();
            var projectile = attackModel.weapons[0].projectile;
            projectile.GetBehavior<DamageModel>().damage = 999999;
            projectile.GetBehavior<DamageModel>().maxDamage = 999999;
            attackModel.weapons[0].Rate *= 4;
            projectile.pierce /= 2;
        }
    }
}