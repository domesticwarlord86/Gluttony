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


namespace Gluttony
{
    public class Gluttony : BotPlugin
    {
        private Composite _coroutine;
        private GluttonySettings _settingsForm;
        private static uint _buff = 48;

        public override string Author { get { return "DomesticWarlord"; } }
        public override string Name => "Gluttony";
        public override Version Version { get { return new Version(1, 0, 0); } }
        
        public override async void OnInitialize()
        {
            /*
            if (!Core.Player.HasAura(_buff))
            {
                await EatFood();
            }*/
            _coroutine = new Decorator(c => !Core.Player.HasAura(48), new ActionRunCoroutine(r => EatFood()));
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
            Logging.Write(Colors.Aquamarine, "Removing Gluttony Hook");A
            TreeHooks.Instance.RemoveHook("TreeStart", _coroutine);
        }

        private void OnBotStop(BotBase bot) { RemoveHooks(); }

        private void OnBotStart(BotBase bot) { AddHooks(); }

        private void OnHooksCleared(object sender, EventArgs e) { RemoveHooks(); }
        
        private static async Task<bool> EatFood()
        {
            if (!Core.Player.HasAura(_buff))
            {
                if (Settings.Instance.Id == 0 || !InventoryManager.FilledSlots.ContainsFooditem(Settings.Instance.Id))
                {
                    return false;
                }


                var item = InventoryManager.FilledSlots.GetFoodItem(Settings.Instance.Id);

                if (item == null) return false;

                Logging.Write(Colors.Aquamarine, "Gluttony: Eating " + item.Name);
                item.UseItem();
                await Coroutine.Wait(5000, () => Core.Player.HasAura(_buff));
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
    }
}
