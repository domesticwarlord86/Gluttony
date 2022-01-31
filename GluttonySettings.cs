using ff14bot.Managers;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Gluttony
{
    public partial class GluttonySettings : Form
    {
        private Dictionary<uint, string> foodDict;
        
        public static bool loading = false;

        public GluttonySettings()
        {
            foodDict = new Dictionary<uint, string>();
            InitializeComponent();
            loading = true;
            UpdateFood();

            if (InventoryManager.FilledSlots.ContainsFooditem(Settings.Instance.Id)) { foodDropBox.SelectedValue = Settings.Instance.Id; }
            
            spiritBindCheckBox1.Checked = Settings.Instance.SpiritPotionsEnabled;
            
            loading = false;
        }

        private void FoodDropBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Instance.Id = (uint)foodDropBox.SelectedValue;
            Settings.Instance.Save();
        }

        private void FoodDropBox_Click(object sender, EventArgs e) { UpdateFood(); }

        private void UpdateFood()
        {
            foodDict.Clear();

            foreach (var item in InventoryManager.FilledSlots.GetFoodItems())
            {
                foodDict[item.TrueItemId] = "(" + item.Count + ")" + item.Name + (item.IsHighQuality ? " HQ" : "");
            }

            foodDropBox.DataSource = new BindingSource(foodDict, null);
            foodDropBox.DisplayMember = "Value";
            foodDropBox.ValueMember = "Key";
        }

        private void spiritBindCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            Settings.Instance.SpiritPotionsEnabled = spiritBindCheckBox1.Checked;
            Settings.Instance.Save();
        }
    }
}
