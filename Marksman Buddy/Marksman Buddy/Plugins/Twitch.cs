using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu.Values;
using Marksman_Buddy.Internal;

namespace Marksman_Buddy.Plugins
{
    internal class Twitch : PluginBase
    {
        private static Spell.Skillshot _W;
        private static Spell.Active _E;
        private static readonly int[] _EDamage = {20, 35, 50, 65, 80};

        public Twitch()
        {
            _SetupMenu();
            _SetupSpells();
            Game.OnTick += Game_OnTick;
        }

        private void _SetupSpells()
        {
            _W = new Spell.Skillshot(SpellSlot.W, 900, SkillShotType.Circular, 250, 1400, 275);
            _E = new Spell.Active(SpellSlot.E, 1200);
        }

        private static void _SetupMenu()
        {
            Variables.Config.AddGroupLabel("Combo");
            Variables.Config.Add("Twitch.UseECombo", new Slider("Cast E at x Stacks", 5, 1, 5));
            Variables.Config.Add("Twitch.UseWCombo", new CheckBox("Use W in Comco"));
            Variables.Config.AddGroupLabel("Harrass");
            Variables.Config.Add("Twitch.UseEHarrass", new Slider("Cast E at x Stacks", 3, 1, 5));
            Variables.Config.Add("Twitch.UseWHarrass", new CheckBox("Use W in Harras", false));
            Variables.Config.AddGroupLabel("Misc");
            Variables.Config.Add("Twitch.KS", new CheckBox("Use E to KS"));
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (Variables.ComboMode)
            {
                _Combo();
            }

            if (Variables.HarassMode)
            {
                _Harrass();
            }

            _KillSteal();
        }

        private static void _Harrass()
        {
            var WTarget = TargetSelector.GetTarget(_W.Range, DamageType.True);
            if (Variables.Config["Twitch.UseWHarrass"].Cast<CheckBox>().CurrentValue && !_W.IsOnCooldown)
                _W.Cast(WTarget);
            foreach (
                var hero in
                    ObjectManager.Get<AIHeroClient>()
                        .Where(x => x.Position.Distance(ObjectManager.Player.Position) < 1200))
            {
                if (hero.GetBuffCount("twitchdeadlyvenom") >=
                    Variables.Config["Twitch.UseEHarrass"].Cast<Slider>().CurrentValue)
                {
                    _E.Cast();
                }
            }
        }

        private static void _KillSteal()
        {
            foreach (
                var hero in
                    ObjectManager.Get<AIHeroClient>()
                        .Where(x => x.Position.Distance(ObjectManager.Player.Position) < 1200))
            {
                if (_ECanKill(hero) && Variables.Config["Twitch.KS"].Cast<CheckBox>().CurrentValue)
                {
                    _E.Cast();
                }
            }
        }

        private static void _Combo()
        {
            var WTarget = TargetSelector.GetTarget(_W.Range, DamageType.True);
            if (Variables.Config["Twitch.UseWCombo"].Cast<CheckBox>().CurrentValue && !_W.IsOnCooldown)
            {
                _W.Cast(WTarget);
            }

            foreach (
                var hero in
                    ObjectManager.Get<AIHeroClient>()
                        .Where(x => x.Position.Distance(ObjectManager.Player.Position) < 1200))
            {
                if (hero.GetBuffCount("twitchdeadlyvenom") >=
                    Variables.Config["Twitch.UseECombo"].Cast<Slider>().CurrentValue)
                {
                    _E.Cast();
                }
            }
        }

        private static bool _ECanKill(Obj_AI_Base unit)
        {
            var EDamage =
                Convert.ToSingle(unit.GetBuffCount("twitchdeadlyvenom")*
                                 (_EDamage[_E.Level] + ObjectManager.Player.TotalAttackDamage*0.25 +
                                  ObjectManager.Player.TotalMagicalDamage*0.2)) - 20.0f; //Damage Calc is off
            return ObjectManager.Player.CalculateDamageOnUnit(unit, DamageType.Physical, EDamage) > unit.Health;
        }
    }
}