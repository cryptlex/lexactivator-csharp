﻿using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;

namespace Cryptlex
{
    public static class LexActivator
    {
        public enum PermissionFlags : uint
        {
            LA_USER = 1,
            LA_SYSTEM = 2,
            LA_IN_MEMORY = 4
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void CallbackType(uint status);

        /* To prevent garbage collection of delegate, need to keep a reference */
        static readonly List<CallbackType> callbackList = new List<CallbackType>();

        /// <summary>
        /// Sets the absolute path of the Product.dat file.
        /// 
        /// This function must be called on every start of your program
        /// before any other functions are called.
        /// </summary>
        /// <param name="filePath">absolute path of the product file (Product.dat)</param>
        public static void SetProductFile(string filePath)
        {
            int status = IntPtr.Size == 8 ? LexActivatorNative.SetProductFile_x64(filePath) : LexActivatorNative.SetProductFile(filePath);
            if (LexStatusCodes.LA_OK != status)
            {
                throw new LexActivatorException(status);
            }
        }

        /// <summary>
        /// Embeds the Product.dat file in the application.
        /// 
        /// It can be used instead of SetProductFile() in case you want
        /// to embed the Product.dat file in your application.
        /// 
        /// This function must be called on every start of your program
        /// before any other functions are called.
        /// </summary>
        /// <param name="productData">content of the Product.dat file</param>
        public static void SetProductData(string productData)
        {
            int status = IntPtr.Size == 8 ? LexActivatorNative.SetProductData_x64(productData) : LexActivatorNative.SetProductData(productData);
            if (LexStatusCodes.LA_OK != status)
            {
                throw new LexActivatorException(status);
            }
        }

        /// <summary>
        /// Sets the product id of your application.
        /// 
        /// This function must be called on every start of your program before
        /// any other functions are called, with the exception of SetProductFile()
        /// or SetProductData() function.
        /// </summary>
        /// <param name="productId">the unique product id of your application as mentioned on the product page in the dashboard</param>
        /// <param name="flags">depending upon whether your application requires admin/root permissions to run or not, this parameter can have one of the following values: LA_SYSTEM, LA_USER, LA_IN_MEMORY</param>
        public static void SetProductId(string productId, PermissionFlags flags)
        {
            int status = IntPtr.Size == 8 ? LexActivatorNative.SetProductId_x64(productId, flags) : LexActivatorNative.SetProductId(productId, flags);
            if (LexStatusCodes.LA_OK != status)
            {
                throw new LexActivatorException(status);
            }
        }

        /// <summary>
        /// Sets the license key required to activate the license.
        /// </summary>
        /// <param name="licenseKey">a valid license key</param>
        public static void SetLicenseKey(string licenseKey)
        {
            int status = IntPtr.Size == 8 ? LexActivatorNative.SetLicenseKey_x64(licenseKey) : LexActivatorNative.SetLicenseKey(licenseKey);
            if (LexStatusCodes.LA_OK != status)
            {
                throw new LexActivatorException(status);
            }
        }

        /// <summary>
        /// Sets the license user email and password for authentication.
        /// 
        /// This function must be called before ActivateLicense() or IsLicenseGenuine()
        /// function if 'requireAuthentication' property of the license is set to true.
        /// </summary>
        /// <param name="email">user email address</param>
        /// <param name="password">user password</param>
        public static void SetLicenseUserCredential(string email, string password)
        {
            int status = IntPtr.Size == 8 ? LexActivatorNative.SetLicenseUserCredential_x64(email, password) : LexActivatorNative.SetLicenseUserCredential(email, password);
            if (LexStatusCodes.LA_OK != status)
            {
                throw new LexActivatorException(status);
            }
        }

        /// <summary>
        /// Sets server sync callback function.
        /// 
        /// Whenever the server sync occurs in a separate thread, and server returns the response,
        /// license callback function gets invoked with the following status codes:
        /// LA_OK, LA_EXPIRED, LA_SUSPENDED, LA_E_REVOKED, LA_E_ACTIVATION_NOT_FOUND,
        /// LA_E_MACHINE_FINGERPRINT, LA_E_AUTHENTICATION_FAILED, LA_E_COUNTRY, LA_E_INET,
        /// LA_E_SERVER, LA_E_RATE_LIMIT, LA_E_IP
        /// </summary>
        /// <param name="callback"></param>
        public static void SetLicenseCallback(CallbackType callback)
        {
            var wrappedCallback = callback;
#if NETFRAMEWORK
            var syncTarget = callback.Target as System.Windows.Forms.Control;
            if (syncTarget != null)
            {
                wrappedCallback = (v) => syncTarget.Invoke(callback, new object[] { v });
            }
#endif
            callbackList.Add(wrappedCallback);

            int status = IntPtr.Size == 8 ? LexActivatorNative.SetLicenseCallback_x64(wrappedCallback) : LexActivatorNative.SetLicenseCallback(wrappedCallback);
            if (LexStatusCodes.LA_OK != status)
            {
                throw new LexActivatorException(status);
            }
        }

        /// <summary>
        /// Sets the activation metadata.
        /// 
        /// The  metadata appears along with the activation details of the license
        /// in dashboard.
        /// </summary>
        /// <param name="key">string of maximum length 256 characters with utf-8 encoding</param>
        /// <param name="value">string of maximum length 256 characters with utf-8 encoding</param>
        public static void SetActivationMetadata(string key, string value)
        {
            int status = IntPtr.Size == 8 ? LexActivatorNative.SetActivationMetadata_x64(key, value) : LexActivatorNative.SetActivationMetadata(key, value);
            if (LexStatusCodes.LA_OK != status)
            {
                throw new LexActivatorException(status);
            }
        }

        /// <summary>
        /// Sets the trial activation metadata.
        /// 
        /// The  metadata appears along with the trial activation details of the product
        /// in dashboard.
        /// </summary>
        /// <param name="key">string of maximum length 256 characters with utf-8 encoding</param>
        /// <param name="value">string of maximum length 256 characters with utf-8 encoding</param>
        public static void SetTrialActivationMetadata(string key, string value)
        {
            int status = IntPtr.Size == 8 ? LexActivatorNative.SetTrialActivationMetadata_x64(key, value) : LexActivatorNative.SetTrialActivationMetadata(key, value);
            if (LexStatusCodes.LA_OK != status)
            {
                throw new LexActivatorException(status);
            }
        }

        /// <summary>
        /// Sets the current app version of your application.
        /// 
        /// The app version appears along with the activation details in dashboard. It
        /// is also used to generate app analytics.
        /// </summary>
        /// <param name="appVersion"></param>
        public static void SetAppVersion(string appVersion)
        {
            int status = IntPtr.Size == 8 ? LexActivatorNative.SetAppVersion_x64(appVersion) : LexActivatorNative.SetAppVersion(appVersion);
            if (LexStatusCodes.LA_OK != status)
            {
                throw new LexActivatorException(status);
            }
        }

        /// <summary>
        /// Sets the network proxy to be used when contacting Cryptlex servers.
        /// 
        /// The proxy format should be: [protocol://][username:password@]machine[:port]
        /// 
        /// NOTE: Proxy settings of the computer are automatically detected. So, in most of the
        /// cases you don't need to care whether your user is behind a proxy server or not.
        /// </summary>
        /// <param name="proxy"></param>
        public static void SetNetworkProxy(string proxy)
        {
            int status = IntPtr.Size == 8 ? LexActivatorNative.SetNetworkProxy_x64(proxy) : LexActivatorNative.SetNetworkProxy(proxy);
            if (LexStatusCodes.LA_OK != status)
            {
                throw new LexActivatorException(status);
            }
        }

        /// <summary>
        /// Gets the product metadata as set in the dashboard.
        /// 
        /// This is available for trial as well as license activations.
        /// </summary>
        /// <param name="key">metadata key to retrieve the value</param>
        /// <returns>Returns the value of metadata for the key.</returns>
        public static string GetProductMetadata(string key)
        {
            var builder = new StringBuilder(256);
            int status = IntPtr.Size == 8 ? LexActivatorNative.GetProductMetadata_x64(key, builder, builder.Length) : LexActivatorNative.GetProductMetadata(key, builder, builder.Length);
            if (LexStatusCodes.LA_OK == status)
            {
                return builder.ToString();
            }
            throw new LexActivatorException(status);
        }

        /// <summary>
        /// Gets the license metadata of the license.
        /// </summary>
        /// <param name="key">metadata key to retrieve the value</param>
        /// <returns>Returns the value of metadata for the key.</returns>
        public static string GetLicenseMetadata(string key)
        {
            var builder = new StringBuilder(256);
            int status = IntPtr.Size == 8 ? LexActivatorNative.GetLicenseMetadata_x64(key, builder, builder.Length) : LexActivatorNative.GetLicenseMetadata(key, builder, builder.Length);
            if (LexStatusCodes.LA_OK == status)
            {
                return builder.ToString();
            }
            throw new LexActivatorException(status);
        }

        /// <summary>
        /// Gets the license meter attribute allowed uses and total uses.
        /// </summary>
        /// <param name="name">name of the meter attribute</param>
        /// <returns>Returns the values of meter attribute allowed and total uses.</returns>
        public static LicenseMeterAttribute GetLicenseMeterAttribute(string name)
        {
            ref uint allowedUses, totalUses;
            int status = IntPtr.Size == 8 ? LexActivatorNative.GetLicenseMeterAttribute_x64(name, ref allowedUses, ref totalUses) : LexActivatorNative.GetLicenseMeterAttribute(name, ref allowedUses, ref totalUses);
            if (LexStatusCodes.LA_OK == status)
            {
                return new LicenseMeterAttribute(name, allowedUses, totalUses);
            }
            throw new LexActivatorException(status);
        }

        /// <summary>
        /// Gets the license key used for activation.
        /// </summary>
        /// <returns>Returns the license key.</returns>
        public static string GetLicenseKey()
        {
            var builder = new StringBuilder(256);
            int status = IntPtr.Size == 8 ? LexActivatorNative.GetLicenseKey_x64(builder, builder.Length) : LexActivatorNative.GetLicenseKey(builder, builder.Length);
            if (LexStatusCodes.LA_OK == status)
            {
                return builder.ToString();
            }
            throw new LexActivatorException(status);
        }

        /// <summary>
        /// Gets the license expiry date timestamp.
        /// </summary>
        /// <returns>Returns the timestamp.</returns>
        public static int GetLicenseExpiryDate()
        {
            ref uint expiryDate;
            int status = IntPtr.Size == 8 ? LexActivatorNative.GetLicenseExpiryDate_x64(ref expiryDate) : LexActivatorNative.GetLicenseExpiryDate(ref expiryDate);
            switch (status)
            {
                case LexStatusCodes.LA_OK:
                    return expiryDate;
                case LexStatusCodes.LA_FAIL:
                    return 0;
                default:
                    throw new LexActivatorException(status);
            }
        }

        /// <summary>
        /// Gets the email associated with the license user.
        /// </summary>
        /// <returns>Returns the license user email.</returns>
        public static string GetLicenseUserEmail()
        {
            var builder = new StringBuilder(256);
            int status = IntPtr.Size == 8 ? LexActivatorNative.GetLicenseUserEmail_x64(builder, builder.Length) : LexActivatorNative.GetLicenseUserEmail(builder, builder.Length);
            if (LexStatusCodes.LA_OK == status)
            {
                return builder.ToString();
            }
            throw new LexActivatorException(status);
        }

        /// <summary>
        /// Gets the name associated with the license user.
        /// </summary>
        /// <returns>Returns the license user name.</returns>
        public static string GetLicenseUserName()
        {
            var builder = new StringBuilder(256);
            int status = IntPtr.Size == 8 ? LexActivatorNative.GetLicenseUserName_x64(builder, builder.Length) : LexActivatorNative.GetLicenseUserName(builder, builder.Length);
            if (LexStatusCodes.LA_OK == status)
            {
                return builder.ToString();
            }
            throw new LexActivatorException(status);
        }

        /// <summary>
        /// Gets the company associated with the license user.
        /// </summary>
        /// <returns>Returns the license user company.</returns>
        public static string GetLicenseUserCompany()
        {
            var builder = new StringBuilder(256);
            int status = IntPtr.Size == 8 ? LexActivatorNative.GetLicenseUserCompany_x64(builder, builder.Length) : LexActivatorNative.GetLicenseUserCompany(builder, builder.Length);
            if (LexStatusCodes.LA_OK == status)
            {
                return builder.ToString();
            }
            throw new LexActivatorException(status);
        }

        /// <summary>
        /// Gets the metadata associated with the license user.
        /// </summary>
        /// <param name="key">key to retrieve the value</param>
        /// <returns>Returns the value of metadata for the key.</returns>
        public static string GetLicenseUserMetadata(string key)
        {
            var builder = new StringBuilder(256);
            int status = IntPtr.Size == 8 ? LexActivatorNative.GetLicenseUserMetadata_x64(key, builder, builder.Length) : LexActivatorNative.GetLicenseUserMetadata(key, builder, builder.Length);
            if (LexStatusCodes.LA_OK == status)
            {
                return builder.ToString();
            }
            throw new LexActivatorException(status);
        }

        /// <summary>
        /// Gets the license type (node-locked or hosted-floating).
        /// </summary>
        /// <returns>Returns the license type.</returns>
        public static string GetLicenseType()
        {
            var builder = new StringBuilder(256);
            int status = IntPtr.Size == 8 ? LexActivatorNative.GetLicenseType_x64(builder, builder.Length) : LexActivatorNative.GetLicenseType(builder, builder.Length);
            if (LexStatusCodes.LA_OK == status)
            {
                return builder.ToString();
            }
            throw new LexActivatorException(status);
        }

        /// <summary>
        /// Gets the activation metadata.
        /// </summary>
        /// <param name="key">key to retrieve the value</param>
        /// <returns>Returns the value of metadata for the key.</returns>
        public static string GetActivationMetadata(string key)
        {
            var builder = new StringBuilder(256);
            int status = IntPtr.Size == 8 ? LexActivatorNative.GetActivationMetadata_x64(key, builder, builder.Length) : LexActivatorNative.GetActivationMetadata(key, builder, builder.Length);
            if (LexStatusCodes.LA_OK == status)
            {
                return builder.ToString();
            }
            throw new LexActivatorException(status);
        }

        /// <summary>
        /// Gets the meter attribute uses consumed by the activation.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Returns the value of meter attribute uses by the activation.</returns>
        public static int GetActivationMeterAttributeUses(string name)
        {
            ref uint uses;
            int status = IntPtr.Size == 8 ? LexActivatorNative.GetActivationMeterAttributeUses_x64(name, ref uses) : LexActivatorNative.GetActivationMeterAttributeUses(name, ref uses);
            if (LexStatusCodes.LA_OK == status)
            {
                return uses;
            }
            throw new LexActivatorException(status);
        }

        /// <summary>
        /// Gets the server sync grace period expiry date timestamp.
        /// </summary>
        /// <returns>Returns server sync grace period expiry date timestamp.</returns>
        public static int GetServerSyncGracePeriodExpiryDate()
        {
            ref uint expiryDate;
            int status = IntPtr.Size == 8 ? LexActivatorNative.GetServerSyncGracePeriodExpiryDate_x64(ref expiryDate) : LexActivatorNative.GetServerSyncGracePeriodExpiryDate(ref expiryDate);
            switch (status)
            {
                case LA_OK:
                    return expiryDate;
                case LA_FAIL:
                    return 0;
                default:
                    throw new LexActivatorException(status);
            }
        }

        /// <summary>
        /// Gets the trial activation metadata.
        /// </summary>
        /// <param name="key">key to retrieve the value</param>
        /// <returns>Returns the value of metadata for the key.</returns>
        public static string GetTrialActivationMetadata(string key)
        {
            var builder = new StringBuilder(256);
            int status = IntPtr.Size == 8 ? LexActivatorNative.GetTrialActivationMetadata_x64(key, builder, builder.Length) : LexActivatorNative.GetTrialActivationMetadata(key, builder, builder.Length);
            if (LexStatusCodes.LA_OK == status)
            {
                return builder.ToString();
            }
            throw new LexActivatorException(status);
        }

        /// <summary>
        /// Gets the trial expiry date timestamp.
        /// </summary>
        /// <returns>Returns trial expiry date timestamp.</returns>
        public static int GetTrialExpiryDate()
        {
            ref uint trialExpiryDate;
            int status = IntPtr.Size == 8 ? LexActivatorNative.GetTrialExpiryDate_x64(ref trialExpiryDate) : LexActivatorNative.GetTrialExpiryDate(ref trialExpiryDate);
            switch (status)
            {
                case LA_OK:
                    return trialExpiryDate;
                case LA_FAIL:
                    return 0;
                default:
                    throw new LexActivatorException(status);
            }
        }

        /// <summary>
        /// Gets the trial activation id. Used in case of trial extension.
        /// </summary>
        /// <returns>Returns the trial id.</returns>
        public static string GetTrialId()
        {
            var builder = new StringBuilder(256);
            int status = IntPtr.Size == 8 ? LexActivatorNative.GetTrialId_x64(builder, builder.Length) : LexActivatorNative.GetTrialId(builder, builder.Length);
            if (LexStatusCodes.LA_OK == status)
            {
                return builder.ToString();
            }
            throw new LexActivatorException(status);
        }

        /// <summary>
        /// Gets the trial expiry date timestamp.
        /// </summary>
        /// <returns>Returns trial expiry date timestamp.</returns>
        public static int GetLocalTrialExpiryDate()
        {
            ref uint trialExpiryDate;
            int status = IntPtr.Size == 8 ? LexActivatorNative.GetLocalTrialExpiryDate_x64(ref trialExpiryDate) : LexActivatorNative.GetLocalTrialExpiryDate(ref trialExpiryDate);
            switch (status)
            {
                case LA_OK:
                    return trialExpiryDate;
                case LA_FAIL:
                    return 0;
                default:
                    throw new LexActivatorException(status);
            }
        }

        /// <summary>
        /// Checks whether a new release is available for the product.
        /// 
        /// This function should only be used if you manage your releases through
        /// Cryptlex release management API.
        /// </summary>
        /// <param name="platform">release platform e.g. windows, macos, linux</param>
        /// <param name="version">current release version</param>
        /// <param name="channel">release channel e.g. stable</param>
        /// <param name="callback">name of the callback function</param>
        public static void CheckForReleaseUpdate(string platform, string version, string channel, CallbackType callback)
        {
            var wrappedCallback = callback;
#if NETFRAMEWORK
            var syncTarget = callback.Target as System.Windows.Forms.Control;
            if (syncTarget != null)
            {
                wrappedCallback = (v) => syncTarget.Invoke(callback, new object[] { v });
            }
#endif
            callbackList.Add(wrappedCallback);
            int status = IntPtr.Size == 8 ? LexActivatorNative.CheckForReleaseUpdate_x64(platform, version, channel, wrappedCallback) : LexActivatorNative.CheckForReleaseUpdate(platform, version, channel, wrappedCallback);
            if (LexStatusCodes.LA_OK != status)
            {
                throw new LexActivatorException(status);
            }
        }

        /// <summary>
        /// Activates the license by contacting the Cryptlex servers. It
        /// validates the key and returns with encrypted and digitally signed token
        /// which it stores and uses to activate your application.
        /// 
        /// This function should be executed at the time of registration, ideally on
        /// a button click.
        /// </summary>
        /// <returns>LA_OK, LA_EXPIRED, LA_SUSPENDED, LA_FAIL</returns>
        public static int ActivateLicense()
        {
            int status = IntPtr.Size == 8 ? LexActivatorNative.ActivateLicense_x64() : LexActivatorNative.ActivateLicense();
            switch (status)
            {
                case LexStatusCodes.LA_OK:
                    return LexStatusCodes.LA_OK;
                case LexStatusCodes.LA_EXPIRED:
                    return LexStatusCodes.LA_EXPIRED;
                case LexStatusCodes.LA_SUSPENDED:
                    return LexStatusCodes.LA_SUSPENDED;
                case LexStatusCodes.LA_FAIL:
                    return LexStatusCodes.LA_FAIL;
                default:
                    throw new LexActivatorException(status);
            }
        }

        /// <summary>
        /// Activates your licenses using the offline activation response file.
        /// </summary>
        /// <param name="filePath">path of the offline activation response file</param>
        /// <returns>LA_OK, LA_EXPIRED, LA_SUSPENDED, LA_FAIL</returns>
        public static int ActivateLicenseOffline(string filePath)
        {
            int status = IntPtr.Size == 8 ? LexActivatorNative.ActivateLicenseOffline_x64(filePath) : LexActivatorNative.ActivateLicenseOffline(filePath);
            switch (status)
            {
                case LexStatusCodes.LA_OK:
                    return LexStatusCodes.LA_OK;
                case LexStatusCodes.LA_EXPIRED:
                    return LexStatusCodes.LA_EXPIRED;
                case LexStatusCodes.LA_SUSPENDED:
                    return LexStatusCodes.LA_SUSPENDED;
                case LexStatusCodes.LA_FAIL:
                    return LexStatusCodes.LA_FAIL;
                default:
                    throw new LexActivatorException(status);
            }
        }

        /// <summary>
        /// Generates the offline activation request needed for generating
        /// offline activation response in the dashboard.
        /// </summary>
        /// <param name="filePath">path of the file for the offline request.</param>
        public static void GenerateOfflineActivationRequest(string filePath)
        {
            int status = IntPtr.Size == 8 ? LexActivatorNative.GenerateOfflineActivationRequest_x64(filePath) : LexActivatorNative.GenerateOfflineActivationRequest(filePath);
            if (LexStatusCodes.LA_OK != status)
            {
                throw new LexActivatorException(status);
            }
        }

        /// <summary>
        /// Deactivates the license activation and frees up the corresponding activation
        /// slot by contacting the Cryptlex servers.
        /// 
        /// This function should be executed at the time of de-registration, ideally on
        /// a button click.
        /// </summary>
        /// <returns>LA_OK, LA_FAIL</returns>
        public static int DeactivateLicense()
        {
            int status = IntPtr.Size == 8 ? LexActivatorNative.DeactivateLicense_x64() : LexActivatorNative.DeactivateLicense();
            switch (status)
            {
                case LexStatusCodes.LA_OK:
                    return LexStatusCodes.LA_OK;
                case LexStatusCodes.LA_FAIL:
                    return LexStatusCodes.LA_FAIL;
                default:
                    throw new LexActivatorException(status);
            }
        }

        /// <summary>
        /// Generates the offline deactivation request needed for deactivation of
        /// the license in the dashboard and deactivates the license locally.
        /// 
        /// A valid offline deactivation file confirms that the license has been successfully
        /// deactivated on the user's machine.
        /// </summary>
        /// <param name="filePath">path of the file for the offline request</param>
        /// <returns>LA_OK, LA_FAIL</returns>
        public static int GenerateOfflineDeactivationRequest(string filePath)
        {
            int status = IntPtr.Size == 8 ? LexActivatorNative.GenerateOfflineDeactivationRequest_x64(filePath) : LexActivatorNative.GenerateOfflineDeactivationRequest(filePath);
            switch (status)
            {
                case LexStatusCodes.LA_OK:
                    return LexStatusCodes.LA_OK;
                case LexStatusCodes.LA_FAIL:
                    return LexStatusCodes.LA_FAIL;
                default:
                    throw new LexActivatorException(status);
            }
        }

        /// <summary>
        /// It verifies whether your app is genuinely activated or not. The verification is
        /// done locally by verifying the cryptographic digital signature fetched at the time of activation.
        /// 
        /// After verifying locally, it schedules a server check in a separate thread. After the
        /// first server sync it periodically does further syncs at a frequency set for the license.
        /// 
        /// In case server sync fails due to network error, and it continues to fail for fixed
        /// number of days (grace period), the function returns LA_GRACE_PERIOD_OVER instead of LA_OK.
        /// 
        /// This function must be called on every start of your program to verify the activation
        /// of your app.
        /// </summary>
        /// <returns>LA_OK, LA_EXPIRED, LA_SUSPENDED, LA_GRACE_PERIOD_OVER, LA_FAIL</returns>
        public static int IsLicenseGenuine()
        {
            int status = IntPtr.Size == 8 ? LexActivatorNative.IsLicenseGenuine_x64() : LexActivatorNative.IsLicenseGenuine();
            switch (status)
            {
                case LexStatusCodes.LA_OK:
                    return LexStatusCodes.LA_OK;
                case LexStatusCodes.LA_EXPIRED:
                    return LexStatusCodes.LA_EXPIRED;
                case LexStatusCodes.LA_SUSPENDED:
                    return LexStatusCodes.LA_SUSPENDED;
                case LexStatusCodes.LA_GRACE_PERIOD_OVER:
                    return LexStatusCodes.LA_GRACE_PERIOD_OVER;
                case LexStatusCodes.LA_FAIL:
                    return LexStatusCodes.LA_FAIL;
                default:
                    throw new LexActivatorException(status);
            }
        }

        /// <summary>
        /// It verifies whether your app is genuinely activated or not. The verification is
        /// done locally by verifying the cryptographic digital signature fetched at the time of activation.
        /// 
        /// This is just an auxiliary function which you may use in some specific cases, when you
        /// want to skip the server sync.
        /// 
        /// NOTE: You may want to set grace period to 0 to ignore grace period.
        /// </summary>
        /// <returns>LA_OK, LA_EXPIRED, LA_SUSPENDED, LA_GRACE_PERIOD_OVER, LA_FAIL</returns>
        public static int IsLicenseValid()
        {
            int status = IntPtr.Size == 8 ? LexActivatorNative.IsLicenseValid_x64() : LexActivatorNative.IsLicenseValid();
            switch (status)
            {
                case LexStatusCodes.LA_OK:
                    return LexStatusCodes.LA_OK;
                case LexStatusCodes.LA_EXPIRED:
                    return LexStatusCodes.LA_EXPIRED;
                case LexStatusCodes.LA_SUSPENDED:
                    return LexStatusCodes.LA_SUSPENDED;
                case LexStatusCodes.LA_GRACE_PERIOD_OVER:
                    return LexStatusCodes.LA_GRACE_PERIOD_OVER;
                case LexStatusCodes.LA_FAIL:
                    return LexStatusCodes.LA_FAIL;
                default:
                    throw new LexActivatorException(status);
            }
        }

        /// <summary>
        /// Starts the verified trial in your application by contacting the
        /// Cryptlex servers.
        /// 
        /// This function should be executed when your application starts first time on
        /// the user's computer, ideally on a button click.
        /// </summary>
        /// <returns>LA_OK, LA_TRIAL_EXPIRED</returns>
        public static int ActivateTrial()
        {
            int status = IntPtr.Size == 8 ? LexActivatorNative.ActivateTrial_x64() : LexActivatorNative.ActivateTrial();
            switch (status)
            {
                case LexStatusCodes.LA_OK:
                    return LexStatusCodes.LA_OK;
                case LexStatusCodes.LA_TRIAL_EXPIRED:
                    return LexStatusCodes.LA_TRIAL_EXPIRED;
                case LexStatusCodes.LA_FAIL:
                    return LexStatusCodes.LA_FAIL;
                default:
                    throw new LexActivatorException(status);
            }
        }

        /// <summary>
        /// Activates your trial using the offline activation response file.
        /// </summary>
        /// <param name="filePath">path of the offline activation response file.</param>
        /// <returns>LA_OK, LA_TRIAL_EXPIRED, LA_FAIL</returns>
        public static int ActivateTrialOffline(string filePath)
        {
            int status = IntPtr.Size == 8 ? LexActivatorNative.ActivateTrialOffline_x64(filePath) : LexActivatorNative.ActivateTrialOffline(filePath);
            switch (status)
            {
                case LexStatusCodes.LA_OK:
                    return LexStatusCodes.LA_OK;
                case LexStatusCodes.LA_TRIAL_EXPIRED:
                    return LexStatusCodes.LA_TRIAL_EXPIRED;
                case LexStatusCodes.LA_FAIL:
                    return LexStatusCodes.LA_FAIL;
                default:
                    throw new LexActivatorException(status);
            }
        }

        /// <summary>
        /// Generates the offline trial activation request needed for generating
        /// offline trial activation response in the dashboard.
        /// </summary>
        /// <param name="filePath">path of the file for the offline request</param>
        public static void GenerateOfflineTrialActivationRequest(string filePath)
        {
            int status = IntPtr.Size == 8 ? LexActivatorNative.GenerateOfflineTrialActivationRequest_x64(filePath) : LexActivatorNative.GenerateOfflineTrialActivationRequest(filePath);
            if (LexStatusCodes.LA_OK != status)
            {
                throw new LexActivatorException(status);
            }
        }

        /// <summary>
        /// It verifies whether trial has started and is genuine or not. The
        /// verification is done locally by verifying the cryptographic digital signature
        /// fetched at the time of trial activation.
        /// 
        /// This function must be called on every start of your program during the trial period.
        /// </summary>
        /// <returns>LA_OK, LA_TRIAL_EXPIRED, LA_FAIL</returns>
        public static int IsTrialGenuine()
        {
            int status = IntPtr.Size == 8 ? LexActivatorNative.IsTrialGenuine_x64() : LexActivatorNative.IsTrialGenuine();
            switch (status)
            {
                case LexStatusCodes.LA_OK:
                    return LexStatusCodes.LA_OK;
                case LexStatusCodes.LA_TRIAL_EXPIRED:
                    return LexStatusCodes.LA_TRIAL_EXPIRED;
                case LexStatusCodes.LA_FAIL:
                    return LexStatusCodes.LA_FAIL;
                default:
                    throw new LexActivatorException(status);
            }
        }

        /// <summary>
        /// Starts the local (unverified) trial.
        /// 
        /// This function should be executed when your application starts first time on
        /// the user's computer.
        /// </summary>
        /// <param name="trialLength">trial length in days</param>
        /// <returns>LA_OK, LA_LOCAL_TRIAL_EXPIRED, LA_FAIL</returns>
        public static int ActivateLocalTrial(uint trialLength)
        {
            int status = IntPtr.Size == 8 ? LexActivatorNative.ActivateLocalTrial_x64(trialLength) : LexActivatorNative.ActivateLocalTrial(trialLength);
            switch (status)
            {
                case LexStatusCodes.LA_OK:
                    return LexStatusCodes.LA_OK;
                case LexStatusCodes.LA_LOCAL_TRIAL_EXPIRED:
                    return LexStatusCodes.LA_LOCAL_TRIAL_EXPIRED;
                case LexStatusCodes.LA_FAIL:
                    return LexStatusCodes.LA_FAIL;
                default:
                    throw new LexActivatorException(status);
            }
        }

        /// <summary>
        /// It verifies whether trial has started and is genuine or not. The
        /// verification is done locally.
        /// 
        /// This function must be called on every start of your program during the trial period.
        /// 
        /// NOTE: The function is only meant for local (unverified) trials.
        /// </summary>
        /// <returns>LA_OK, LA_LOCAL_TRIAL_EXPIRED, LA_FAIL</returns>
        public static int IsLocalTrialGenuine()
        {
            int status = IntPtr.Size == 8 ? LexActivatorNative.IsLocalTrialGenuine_x64() : LexActivatorNative.IsLocalTrialGenuine();
            switch (status)
            {
                case LexStatusCodes.LA_OK:
                    return LexStatusCodes.LA_OK;
                case LexStatusCodes.LA_LOCAL_TRIAL_EXPIRED:
                    return LexStatusCodes.LA_LOCAL_TRIAL_EXPIRED;
                case LexStatusCodes.LA_FAIL:
                    return LexStatusCodes.LA_FAIL;
                default:
                    throw new LexActivatorException(status);
            }
        }

        /// <summary>
        /// Extends the local trial.
        /// 
        /// NOTE: The function is only meant for local (unverified) trials.
        /// </summary>
        /// <param name="trialExtensionLength">number of days to extend the trial</param>
        /// <returns>LA_OK, LA_FAIL</returns>
        public static int ExtendLocalTrial(uint trialExtensionLength)
        {
            int status = IntPtr.Size == 8 ? LexActivatorNative.ExtendLocalTrial_x64(trialExtensionLength) : LexActivatorNative.ExtendLocalTrial(trialExtensionLength);
            switch (status)
            {
                case LexStatusCodes.LA_OK:
                    return LexStatusCodes.LA_OK;
                case LexStatusCodes.LA_FAIL:
                    return LexStatusCodes.LA_FAIL;
                default:
                    throw new LexActivatorException(status);
            }
        }

        /// <summary>
        /// Increments the meter attribute uses of the activation.
        /// </summary>
        /// <param name="name">name of the meter attribute</param>
        /// <param name="increment">the increment value</param>
        public static void IncrementActivationMeterAttributeUses(string name, uint increment)
        {
            int status = IntPtr.Size == 8 ? LexActivatorNative.IncrementActivationMeterAttributeUses_x64(name, increment) : LexActivatorNative.IncrementActivationMeterAttributeUses(name, increment);
            if (LexStatusCodes.LA_OK != status)
            {
                throw new LexActivatorException(status);
            }
        }

        /// <summary>
        /// Decrements the meter attribute uses of the activation.
        /// </summary>
        /// <param name="name">name of the meter attribute</param>
        /// <param name="decrement">the decrement value</param>
        public static void DecrementActivationMeterAttributeUses(string name, uint decrement)
        {
            int status = IntPtr.Size == 8 ? LexActivatorNative.DecrementActivationMeterAttributeUses_x64(name, decrement) : LexActivatorNative.DecrementActivationMeterAttributeUses(name, decrement);
            if (LexStatusCodes.LA_OK != status)
            {
                throw new LexActivatorException(status);
            }
        }

        /// <summary>
        /// Resets the meter attribute uses consumed by the activation.
        /// </summary>
        /// <param name="name">name of the meter attribute</param>
        public static void ResetActivationMeterAttributeUses(string name)
        {
            int status = IntPtr.Size == 8 ? LexActivatorNative.ResetActivationMeterAttributeUses_x64(name) : LexActivatorNative.ResetActivationMeterAttributeUses(name);
            if (LexStatusCodes.LA_OK != status)
            {
                throw new LexActivatorException(status);
            }
        }

        /// <summary>
        /// Resets the activation and trial data stored in the machine.
        /// 
        /// This function is meant for developer testing only.
        /// 
        /// NOTE: The function does not reset local (unverified) trial data.
        /// </summary>
        public static void Reset()
        {
            int status = IntPtr.Size == 8 ? LexActivatorNative.Reset_x64() : LexActivatorNative.Reset();
            if (LexStatusCodes.LA_OK != status)
            {
                throw new LexActivatorException(status);
            }
        }
    }
}
