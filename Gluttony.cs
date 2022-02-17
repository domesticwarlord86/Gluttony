using Buddy.Coroutines;
using ff14bot;
using ff14bot.AClasses;
using ff14bot.Behavior;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Enums;
using ff14bot.NeoProfiles;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Collections.Generic;
using TreeSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using ff14bot.RemoteWindows;


namespace Gluttony
{
    public class Gluttony : BotPlugin
    {
        private Composite _coroutine;
        private GluttonySettings _settingsForm;
        private static uint _foodBuff = 48;
        private static uint _medBuff = 49;

        public override string Author { get { return "DomesticWarlord"; } }
        public override string Name => "Gluttony";
        public override Version Version { get { return new Version(1, 0, 0); } }
        
        public override async void OnInitialize()
        {
            _coroutine = new ActionRunCoroutine(r => PluginTask());
        }

        public override void OnEnabled()
        {
            TreeRoot.OnStart += OnBotStart;
            TreeRoot.OnStop += OnBotStop;
            TreeHooks.Instance.OnHooksCleared += OnHooksCleared;

            if (TreeRoot.IsRunning) { AddHooks(); }
        }

        public override void OnDisabled()
        {
            TreeRoot.OnStart -= OnBotStart;
            TreeRoot.OnStop -= OnBotStop;
            RemoveHooks();
        }

        public override void OnShutdown() { OnDisabled(); }

        public override bool WantButton { get { return true; } }

        public override void OnButtonPress()
        {
            if (_settingsForm == null || _settingsForm.IsDisposed || _settingsForm.Disposing) { _settingsForm = new GluttonySettings(); }
            _settingsForm.ShowDialog();
        }

        private void AddHooks()
        {
            Logging.Write(Colors.Aquamarine, "Adding Gluttony Hook");
            TreeHooks.Instance.AddHook("TreeStart", _coroutine);
        }

        private void RemoveHooks()
        {
            Logging.Write(Colors.Aquamarine, "Removing Gluttony Hook");
            TreeHooks.Instance.RemoveHook("TreeStart", _coroutine);
        }

        private void OnBotStop(BotBase bot) { RemoveHooks(); }

        private void OnBotStart(BotBase bot) { AddHooks(); }

        private void OnHooksCleared(object sender, EventArgs e) { RemoveHooks(); }

        internal async Task<bool> PluginTask()
        {
            if (Core.Me.IsCasting || Core.Me.IsMounted || Core.Me.InCombat || Talk.DialogOpen ||
                MovementManager.IsOccupied) return false;

            if (!Core.Player.HasAura(_foodBuff))
            {
                if (Settings.Instance.Id == 0 || !InventoryManager.FilledSlots.ContainsFooditem(Settings.Instance.Id))
                {
                    return false;
                }
                await EatFood();
            }
            
            if (!Core.Player.HasAura(_medBuff) && Settings.Instance.SpiritPotionsEnabled)
            {
                if (!InventoryManager.FilledSlots.Any(i =>
                    i.RawItemId == 7059 || i.RawItemId == 19885 || i.RawItemId == 27960))
                {
                    Logging.Write(Colors.Aquamarine, "Spiritbound potions are enabled, but none in inventory.");
                    return false;
                }
                await DrinkPotion();
            }
            
            
            //Don't block the logic below us in the tree.
            return false;
        }

        private static async Task<bool> EatFood()
        {
            if (!Core.Player.HasAura(_foodBuff))
            {
                if (Settings.Instance.Id == 0 || !InventoryManager.FilledSlots.ContainsFooditem(Settings.Instance.Id))
                {
                    return false;
                }


                var item = InventoryManager.FilledSlots.GetFoodItem(Settings.Instance.Id);

                if (item == null) return false;

                Logging.Write(Colors.Aquamarine, "Gluttony: Eating " + item.Name);
                item.UseItem();
                await Coroutine.Wait(5000, () => Core.Player.HasAura(_foodBuff));
            }
            return true;
        }
        
        private static async Task<bool> DrinkPotion()
        {
            if (!Settings.Instance.SpiritPotionsEnabled) return true;
            if (!Core.Player.HasAura(_medBuff))
            {
                    
                uint[] potions = new uint[] {7059, 19885,27960};
                    
                if (InventoryManager.FilledSlots.Any(i => potions.Contains(i.RawItemId)))
                {
                    var item = InventoryManager.FilledSlots.First(i => potions.Contains(i.RawItemId));
                    if (item.CanUse())
                    {
                        Logging.Write(Colors.Aquamarine, "Gluttony: Drinking " + item.Name);
                        item.UseItem();
                    }
                }
                    
            }


            return true;
        }
    }
    
    public static class Helpers 
    {
        private static bool IsFoodItem(this BagSlot slot) =>  (slot.Item.EquipmentCatagory == ItemUiCategory.Meal || slot.Item.EquipmentCatagory == ItemUiCategory.Ingredient);
        public static IEnumerable<BagSlot> GetFoodItems(this IEnumerable<BagSlot> bags) => bags.Where(s => s.IsFoodItem());
        public static bool ContainsFooditem(this IEnumerable<BagSlot> bags, uint id) => bags.Select(s => s.TrueItemId).Contains(id);
        public static BagSlot GetFoodItem(this IEnumerable<BagSlot> bags, uint id) => bags.First(s => s.TrueItemId == id);
    }    

    public class Settings : JsonSettings
    {
        private static Settings _instance;
        public static Settings Instance { get { return _instance ?? (_instance = new Settings()); ; } }

        public Settings() : base(Path.Combine(CharacterSettingsDirectory, "Gluttony.json")) { }

        [Setting]
        public uint Id { get; set; }
        
        private bool _spiritPotionEnabled;
        [DefaultValue(false)]
        public bool SpiritPotionsEnabled
        {
            get => _spiritPotionEnabled;
            set
            {
                if (_spiritPotionEnabled != value)
                {
                    _spiritPotionEnabled = value;
                    //Save();
                }
            }
        }
    }
}
