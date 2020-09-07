using Accord;
using LuzesRGB.Services.Lights;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LuzesRGB.Services.Controls
{
    public partial class EditLight : Form
    {
        public SmartLight SmartLight { get; set; }

        public EditLight(SmartLight smartLight)
        {
            InitializeComponent();
            Enum.GetValues(typeof(SmartLight.Types)).Cast<SmartLight.Types>().Each(type => cbType.Items.Add(type));
            btnRemove.Enabled = smartLight != null;
            SmartLight = smartLight ?? new SmartLight();
            tbName.Text = SmartLight.Name;
            tbIp.Text = SmartLight.IP?.ToString();
            cbType.SelectedItem = SmartLight.Type;
        }

        private void BtnOkClicked(object sender, EventArgs e)
        {
            try
            {
                if (tbName.Text.Length == 0)
                    throw new Exception("O nome não pode estar vazio");

                if (tbIp.Text.Length == 0)
                    throw new Exception("O IP não pode estar vazio");

                if (!IPAddress.TryParse(tbIp.Text, out IPAddress ipAddress))
                    throw new Exception($"'{tbIp.Text}' não é um IP válido");
                
                SmartLight.Name = tbName.Text;
                SmartLight.Type = (SmartLight.Types)cbType.SelectedItem;
                SmartLight.IP = tbIp.Text;

                DialogResult = DialogResult.OK;
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancelClicked(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void BtnRemoveClicked(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
            Close();
        }
    }
}
