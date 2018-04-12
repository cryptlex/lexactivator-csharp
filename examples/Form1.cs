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
            if (status != LexActivator.StatusCodes.LA_OK)
            {
                this.statusLabel.Text = "Error setting product file: " + status.ToString();
                return;
            }
            status = LexActivator.SetProductId("PASTE_PRODUCT_ID", LexActivator.PermissionFlags.LA_USER);
            if (status != LexActivator.StatusCodes.LA_OK)
            {
                this.statusLabel.Text = "Error setting product id: " + status.ToString();
                return;
            }
            status = LexActivator.IsLicenseGenuine();
            if(status == LexActivator.StatusCodes.LA_OK || status == LexActivator.StatusCodes.LA_EXPIRED || status == LexActivator.StatusCodes.LA_SUSPENDED || status == LexActivator.StatusCodes.LA_GRACE_PERIOD_OVER)
            {
                // uint expiryDate = 0;
                // LexActivator.GetLicenseExpiryDate(ref expiryDate);
                // int daysLeft = (int)(expiryDate - unixTimestamp()) / 86500;
                this.statusLabel.Text = "License genuinely activated! Activation Status: " + status.ToString();
                this.activateBtn.Text = "Deactivate";
                this.activateTrialBtn.Enabled = false;
                return;
            }
            status = LexActivator.IsTrialGenuine();
            if (status == LexActivator.StatusCodes.LA_OK)
            {
                uint trialExpiryDate = 0;
                LexActivator.GetTrialExpiryDate(ref trialExpiryDate);
                int daysLeft = (int)(trialExpiryDate - unixTimestamp()) / 86500;
                this.statusLabel.Text = "Trial period! Days left:" + daysLeft.ToString();
                this.activateTrialBtn.Enabled = false;
            }
            else if (status == LexActivator.StatusCodes.LA_TRIAL_EXPIRED)
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
                status = LexActivator.DeactivateLicense();
                if (status == LexActivator.StatusCodes.LA_OK)
                {
                    this.statusLabel.Text = "License deactivated successfully";
                    this.activateBtn.Text = "Activate";
                    this.activateTrialBtn.Enabled = true;
                    return;
                }
                this.statusLabel.Text = "Error deactivating license: " + status.ToString();
                return;
            }
            status = LexActivator.SetLicenseKey(productKeyBox.Text);
            if (status != LexActivator.StatusCodes.LA_OK)
            {
                this.statusLabel.Text = "Error setting license key: " + status.ToString();
                return;
            }
            status = LexActivator.SetActivationMetadata("key1", "value1");
            if (status != LexActivator.StatusCodes.LA_OK)
            {
                this.statusLabel.Text = "Error setting activation metadata: " + status.ToString();
                return;
            }
            status = LexActivator.ActivateLicense();
            if (status != LexActivator.StatusCodes.LA_OK)
            {
                this.statusLabel.Text = "Error activating the license: " + status.ToString();
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
            status = LexActivator.SetTrialActivationMetadata("key2", "value2");
            if (status != LexActivator.StatusCodes.LA_OK)
            {
                this.statusLabel.Text = "Error setting activation metadata: " + status.ToString();
                return;
            }
            status = LexActivator.ActivateTrial();
            if (status != LexActivator.StatusCodes.LA_OK)
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
