using System;
using System.Text;
using System.Windows.Forms;
using Cryptlex;

namespace Sample
{
    public partial class ActivationForm : Form
    {
        private ILexActivator lexActivator = LexActivator.Instance;

        public ActivationForm()
        {
            InitializeComponent();
            int status;
            // status = LexActivator.SetProductFile ("ABSOLUTE_PATH_OF_PRODUCT.DAT_FILE");
            status = lexActivator.SetProductData("REJGOTZEODU4RjBERjQ3MzlGQzdCODAxQzkyQ0Y5QzQ=.BlDqjH29Z2EDwuTk023GzNKUpU1Zry1G+kQLb+XgzxdwiPTWNxlvkskMhubJqZChkQSJGF2lEwuP/+PwoOuZTydRCXKAu55AGRYlUlcIeeWKbhI2yWgpIfz6gXcdAXOgtLAJTmRNrDnieVizaZAcOoDHGQTSUDV2zmcu6vlArDFdTC/SOI33W9LeK2yij5a2kNqItyPn9WFvc++J0LIiAlHmbm+MCjO415LnrmbiXMjKkC923Q7bm02+pCJXO6lWdWuq/2GrA6hN2iaMfUzdCFZdeDihO7kQwuKerMfXrJW41ADDIqXUpHzw/zshrCsDfjj17PJF8OWMSeFSvxYlfm0rulvV2CMNyfkl/sAKom/gx2zitoGQwk52EsngWdvujjyRTbWFS60lghRoX919VFwWnsILPa47g9768s5iOcGwz5wq4RxGZEEXImUJv8qQOqmEVvmeyT05YnmW51o0GmnTKhcLJLv68weFvN5ctMSA+0RksWxT+xTQcH1omHYzOG2uE+6Ad/nvESTgYqWBmd+ZcMLb11OT+2c4ijqCK5G8vsu7B9vBaR6egGSvpmP/WOgwqO222SsMmK+vLEKh52kyC04XBJeaz9SPmltYIotvyvDEtCE9xQ7uLFhv/lcwxlNJflLQaOB8/wuMlLCkMoOIvWw09V14+xYtd8pLqjtkS4rC+y3AxQmCzlJmQwDn1KkO65FBkgi0k7+6KZR4twRjJ7UrVVKhQHtq594FIk4=");
            if (status != LexActivator.StatusCodes.LA_OK)
            {
                this.statusLabel.Text = "Error setting product file: " + status.ToString();
                return;
            }
            status = lexActivator.SetProductId("bb65d1d9-34a9-4add-9f73-61fc49fc91ed", LexActivator.PermissionFlags.LA_USER);
            if (status != LexActivator.StatusCodes.LA_OK)
            {
                this.statusLabel.Text = "Error setting product id: " + status.ToString();
                return;
            }
            // Setting license callback is recommended for floating licenses
            status = lexActivator.SetLicenseCallback(LicenseCallback);
            if (status != LexActivator.StatusCodes.LA_OK)
            {
                this.statusLabel.Text = "Error setting callback function: " + status.ToString();
                return;
            }
            status = lexActivator.IsLicenseGenuine();
            if(status == LexActivator.StatusCodes.LA_OK || status == LexActivator.StatusCodes.LA_EXPIRED || status == LexActivator.StatusCodes.LA_SUSPENDED || status == LexActivator.StatusCodes.LA_GRACE_PERIOD_OVER)
            {
                //uint expiryDate = 0;
                //lexActivator.GetLicenseExpiryDate(ref expiryDate);
                //int daysLeft = (int)(expiryDate - unixTimestamp()) / 86500;
                this.statusLabel.Text = "License genuinely activated! Activation Status: " + status.ToString();
                this.activateBtn.Text = "Deactivate";
                this.activateTrialBtn.Enabled = false;

                // Checking for software release update
                // status = LexActivator.CheckForReleaseUpdate("windows", "1.0.0", "stable", SoftwareReleaseUpdateCallback);
                // if (status != LexActivator.StatusCodes.LA_OK)
                // {
                //     this.statusLabel.Text = "Error checking for software release update: " + status.ToString();
                // }
                return;
            }
            status = lexActivator.IsTrialGenuine();
            if (status == LexActivator.StatusCodes.LA_OK)
            {
                uint trialExpiryDate = 0;
                lexActivator.GetTrialExpiryDate(ref trialExpiryDate);
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
                status = lexActivator.DeactivateLicense();
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
            status = lexActivator.SetLicenseKey(productKeyBox.Text);
            if (status != LexActivator.StatusCodes.LA_OK)
            {
                this.statusLabel.Text = "Error setting license key: " + status.ToString();
                return;
            }
            status = lexActivator.SetActivationMetadata("key1", "value1");
            if (status != LexActivator.StatusCodes.LA_OK)
            {
                this.statusLabel.Text = "Error setting activation metadata: " + status.ToString();
                return;
            }
            status = lexActivator.ActivateLicense();
            if (status == LexActivator.StatusCodes.LA_OK || status == LexActivator.StatusCodes.LA_EXPIRED || status == LexActivator.StatusCodes.LA_SUSPENDED)            
            {
                this.statusLabel.Text = "Activation Successful :" + status.ToString();
                this.activateBtn.Text = "Deactivate";
                this.activateTrialBtn.Enabled = false;
            }
            else
            {
                this.statusLabel.Text = "Error activating the license: " + status.ToString();
                return;
            }

        }

        private void activateTrialBtn_Click(object sender, EventArgs e)
        {
            int status;
            status = lexActivator.SetTrialActivationMetadata("key2", "value2");
            if (status != LexActivator.StatusCodes.LA_OK)
            {
                this.statusLabel.Text = "Error setting activation metadata: " + status.ToString();
                return;
            }
            status = lexActivator.ActivateTrial();
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

        // License callback is invoked when LexActivator.IsLicenseGenuine() completes a server sync
        private void LicenseCallback(uint status)
        {
            // NOTE: Don't invoke IsLicenseGenuine(), ActivateLicense() or ActivateTrial() API functions in this callback
            string statusText;
            switch (status)
            {
                case LexActivator.StatusCodes.LA_SUSPENDED:
                    statusText = "The license has been suspended.";
                    break;
                default:
                    statusText = "License status code: " + status.ToString();
                    break;
            }
            if(this.statusLabel.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    this.statusLabel.Text = statusText;
                }));
            }
            else
            {
                this.statusLabel.Text = statusText;
            }
            
        }

        // Software release update callback is invoked when CheckForReleaseUpdate() gets a response from the server
        private void SoftwareReleaseUpdateCallback(uint status)
        {
            string statusText;
            switch (status)
            {
                case LexActivator.StatusCodes.LA_RELEASE_UPDATE_AVAILABLE:
                    statusText = "An update is available for the app.";
                    break;
                case LexActivator.StatusCodes.LA_RELEASE_NO_UPDATE_AVAILABLE:
                    // Current versiom is already latest.
                    break;
                default:
                    statusText = "Release status code: " + status.ToString();
                    break;
            }
            if(this.statusLabel.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    this.statusLabel.Text = statusText;
                }));
            }
            else
            {
                this.statusLabel.Text = statusText;
            }
        }

        private uint unixTimestamp()
        {
            return (uint)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
    }
}
