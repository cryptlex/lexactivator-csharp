using System;
using System.Text;
using System.Windows.Forms;
using Cryptlex;

namespace Sample
{
    public partial class ActivationForm : Form
    {
        public ActivationForm()
        {
            InitializeComponent();
            int status;
            status = LexActivator.SetProductFile("Product.dat");
            if (status != LexActivator.LA_OK)
            {
                this.statusLabel.Text = "Error setting product file: " + status.ToString();
                return;
            }
            status = LexActivator.SetVersionGUID("59A44CE9-5415-8CF3-BD54-EA73A64E9A1B", LexActivator.PermissionFlags.LA_USER);
            if (status != LexActivator.LA_OK)
            {
                this.statusLabel.Text = "Error setting version GUID: " + status.ToString();
                return;
            }
            status = LexActivator.IsProductGenuine();
            if(status == LexActivator.LA_OK || status == LexActivator.LA_GP_OVER)
            {
                this.statusLabel.Text = "Product genuinely activated!";
                this.activateBtn.Text = "Deactivate";
                this.activateTrialBtn.Enabled = false;
                return;
            }
            status = LexActivator.IsTrialGenuine();
            if (status == LexActivator.LA_OK)
            {
                uint daysLeft = 0;
                LexActivator.GetTrialDaysLeft(ref daysLeft, LexActivator.TrialType.LA_V_TRIAL);
                this.statusLabel.Text = "Trial period! Days left:" + daysLeft.ToString();
                this.activateTrialBtn.Enabled = false;
            }
            else if (status == LexActivator.LA_T_EXPIRED)
            {
                this.statusLabel.Text = "Trial has expired!";
            }
            else
            {
                this.statusLabel.Text = "Trial has not started or has been tampered: " + status.ToString();
            }
        }

        private void activateBtn_Click(object sender, EventArgs e)
        {
            int status;
            if(this.activateBtn.Text == "Deactivate")
            {
                status = LexActivator.DeactivateProduct();
                if (status == LexActivator.LA_OK)
                {
                    this.statusLabel.Text = "Product deactivated successfully";
                    this.activateBtn.Text = "Activate";
                    this.activateTrialBtn.Enabled = true;
                    return;
                }
                this.statusLabel.Text = "Error deactivating product: " + status.ToString();
                return;
            }
            status = LexActivator.SetProductKey(productKeyBox.Text);
            if (status != LexActivator.LA_OK)
            {
                this.statusLabel.Text = "Error setting product key: " + status.ToString();
                return;
            }
            LexActivator.SetExtraActivationData("sample data");
            status = LexActivator.ActivateProduct();
            if (status != LexActivator.LA_OK)
            {
                this.statusLabel.Text = "Error activating the product: " + status.ToString();
                return;
            }
            else
            {
                this.statusLabel.Text = "Activation Successful";
                this.activateBtn.Text = "Deactivate";
                this.activateTrialBtn.Enabled = false;
            }

        }

        private void activateTrialBtn_Click(object sender, EventArgs e)
        {
            int status;
            status = LexActivator.SetTrialKey("CCEAF69B-144EDE48-B763AE2F-A0957C93-98827434");
            if (status != LexActivator.LA_OK)
            {
                this.statusLabel.Text = "Error setting trial key: " + status.ToString();
                return;
            }
            status = LexActivator.ActivateTrial();
            if (status != LexActivator.LA_OK)
            {
                this.statusLabel.Text = "Error activating the trial: " + status.ToString();
                return;
            }
            else
            {
                this.statusLabel.Text = "Trial started Successful";
            }
        }
    }
}
