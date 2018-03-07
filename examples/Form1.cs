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
            // status = LexActivator.SetProductFile ("ABSOLUTE_PATH_OF_PRODUCT.DAT_FILE");
            status = LexActivator.SetProductData("PASTE_CONTENT_OF_PRODUCT.DAT_FILE");
            if (status != LexActivator.LA_OK)
            {
                this.statusLabel.Text = "Error setting product file: " + status.ToString();
                return;
            }
            status = LexActivator.SetProductVersionGuid("PASTE_PRODUCT_VERSION_GUID", LexActivator.PermissionFlags.LA_USER);
            if (status != LexActivator.LA_OK)
            {
                this.statusLabel.Text = "Error setting version GUID: " + status.ToString();
                return;
            }
            status = LexActivator.IsProductGenuine();
            if(status == LexActivator.LA_OK || status == LexActivator.LA_EXPIRED || status == LexActivator.LA_REVOKED || status == LexActivator.LA_GP_OVER)
            {
                //uint expiryDate = 0;
                //LexActivator.GetProductKeyExpiryDate(ref expiryDate);
                //int daysLeft = (int)(expiryDate - unixTimestamp()) / 86500;
                this.statusLabel.Text = "Product genuinely activated! Activation Status: " + status.ToString();
                this.activateBtn.Text = "Deactivate";
                this.activateTrialBtn.Enabled = false;
                return;
            }
            status = LexActivator.IsTrialGenuine();
            if (status == LexActivator.LA_OK)
            {
                uint trialExpiryDate = 0;
                LexActivator.GetTrialExpiryDate(ref trialExpiryDate);
                int daysLeft = (int)(trialExpiryDate - unixTimestamp()) / 86500;
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
            status = LexActivator.SetActivationExtraData("SAMPLE DATA");
            if (status != LexActivator.LA_OK)
            {
                this.statusLabel.Text = "Error setting activation extra data: " + status.ToString();
                return;
            }
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
            status = LexActivator.SetTrialActivationExtraData("SAMPLE DATA");
            if (status != LexActivator.LA_OK)
            {
                this.statusLabel.Text = "Error setting activation extra data: " + status.ToString();
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

        private uint unixTimestamp()
        {
            return (uint)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
    }
}
