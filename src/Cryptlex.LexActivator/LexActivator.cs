using System;
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

        /*
            FUNCTION: SetProductFile()

            PURPOSE: Sets the absolute path of the Product.dat file.

            This function must be called on every start of your program
            before any other functions are called.

            PARAMETERS:
            * filePath - absolute path of the product file (Product.dat)

            RETURN CODES: LA_OK, LA_E_FILE_PATH, LA_E_PRODUCT_FILE

            NOTE: If this function fails to set the path of product file, none of the
            other functions will work.
        */
        public static int SetProductFile(string filePath)
        {
            return IntPtr.Size == 8 ? LexActivatorNative.SetProductFile_x64(filePath) : LexActivatorNative.SetProductFile(filePath);
        }

        /*
            FUNCTION: SetProductData()

            PURPOSE: Embeds the Product.dat file in the application.

            It can be used instead of SetProductFile() in case you want
            to embed the Product.dat file in your application.

            This function must be called on every start of your program
            before any other functions are called.

            PARAMETERS:
            * productData - content of the Product.dat file

            RETURN CODES: LA_OK, LA_E_PRODUCT_DATA

            NOTE: If this function fails to set the product data, none of the
            other functions will work.
        */
        public static int SetProductData(string productData)
        {
            return IntPtr.Size == 8 ? LexActivatorNative.SetProductData_x64(productData) : LexActivatorNative.SetProductData(productData);
        }

        /*
            FUNCTION: SetProductId()

            PURPOSE: Sets the product id of your application.

            This function must be called on every start of your program before
            any other functions are called, with the exception of SetProductFile()
            or SetProductData() function.

            PARAMETERS:
            * productId - the unique product id of your application as mentioned
            on the product page in the dashboard.

            * flags - depending upon whether your application requires admin/root
            permissions to run or not, this parameter can have one of the following
            values: LA_SYSTEM, LA_USER, LA_IN_MEMORY

            RETURN CODES: LA_OK, LA_E_WMIC, LA_E_PRODUCT_FILE, LA_E_PRODUCT_DATA, LA_E_PRODUCT_ID,
            LA_E_SYSTEM_PERMISSION

            NOTE: If this function fails to set the product id, none of the other
            functions will work.
        */
        public static int SetProductId(string productId, PermissionFlags flags)
        {
            return IntPtr.Size == 8 ? LexActivatorNative.SetProductId_x64(productId, flags) : LexActivatorNative.SetProductId(productId, flags);
        }

        /*
            FUNCTION: SetLicenseKey()

            PURPOSE: Sets the license key required to activate the license.

            PARAMETERS:
            * licenseKey - a valid license key.

            RETURN CODES: LA_OK, LA_E_PRODUCT_ID, LA_E_LICENSE_KEY
        */
        public static int SetLicenseKey(string licenseKey)
        {
            return IntPtr.Size == 8 ? LexActivatorNative.SetLicenseKey_x64(licenseKey) : LexActivatorNative.SetLicenseKey(licenseKey);
        }

        /*
            FUNCTION: SetLicenseUserCredential()

            PURPOSE: Sets the license user email and password for authentication.

            This function must be called before ActivateLicense() or IsLicenseGenuine()
            function if 'requireAuthentication' property of the license is set to true.

            PARAMETERS:
            * email - user email address.
            * password - user password.

            RETURN CODES: LA_OK, LA_E_PRODUCT_ID, LA_E_LICENSE_KEY
        */
        public static int SetLicenseUserCredential(string email, string password)
        {
            return IntPtr.Size == 8 ? LexActivatorNative.SetLicenseUserCredential_x64(email, password) : LexActivatorNative.SetLicenseUserCredential(email, password);
        }

        /*
            FUNCTION: SetLicenseCallback()

            PURPOSE: Sets server sync callback function.

            Whenever the server sync occurs in a separate thread, and server returns the response,
            license callback function gets invoked with the following status codes:
            LA_OK, LA_EXPIRED, LA_SUSPENDED,
            LA_E_REVOKED, LA_E_ACTIVATION_NOT_FOUND, LA_E_MACHINE_FINGERPRINT
            LA_E_AUTHENTICATION_FAILED, LA_E_COUNTRY, LA_E_INET, LA_E_SERVER,
            LA_E_RATE_LIMIT, LA_E_IP

            PARAMETERS:
            * callback - name of the callback function

            RETURN CODES: LA_OK, LA_E_PRODUCT_ID, LA_E_LICENSE_KEY
        */
        public static int SetLicenseCallback(CallbackType callback)
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

            return IntPtr.Size == 8 ? LexActivatorNative.SetLicenseCallback_x64(wrappedCallback) : LexActivatorNative.SetLicenseCallback(wrappedCallback);
        }

        /*
            FUNCTION: SetActivationMetadata()

            PURPOSE: Sets the activation metadata.

            The  metadata appears along with the activation details of the license
            in dashboard.

            PARAMETERS:
            * key - string of maximum length 256 characters with utf-8 encoding.
            * value - string of maximum length 256 characters with utf-8 encoding.

            RETURN CODES: LA_OK, LA_E_PRODUCT_ID, LA_E_LICENSE_KEY, LA_E_METADATA_KEY_LENGTH,
            LA_E_METADATA_VALUE_LENGTH, LA_E_ACTIVATION_METADATA_LIMIT
        */
        public static int SetActivationMetadata(string key, string value)
        {
            return IntPtr.Size == 8 ? LexActivatorNative.SetActivationMetadata_x64(key, value) : LexActivatorNative.SetActivationMetadata(key, value);
        }

        /*
            FUNCTION: SetTrialActivationMetadata()

            PURPOSE: Sets the trial activation metadata.

            The  metadata appears along with the trial activation details of the product
            in dashboard.

            PARAMETERS:
            * key - string of maximum length 256 characters with utf-8 encoding.
            * value - string of maximum length 256 characters with utf-8 encoding.

            RETURN CODES: LA_OK, LA_E_PRODUCT_ID, LA_E_METADATA_KEY_LENGTH,
            LA_E_METADATA_VALUE_LENGTH, LA_E_TRIAL_ACTIVATION_METADATA_LIMIT
        */
        public static int SetTrialActivationMetadata(string key, string value)
        {

            return IntPtr.Size == 8 ? LexActivatorNative.SetTrialActivationMetadata_x64(key, value) : LexActivatorNative.SetTrialActivationMetadata(key, value);
#else
            return LexActivatorNative.SetTrialActivationMetadata(key, value);
#endif

        }

        /*
            FUNCTION: SetAppVersion()

            PURPOSE: Sets the current app version of your application.

            The app version appears along with the activation details in dashboard. It
            is also used to generate app analytics.

            PARAMETERS:
            * appVersion - string of maximum length 256 characters with utf-8 encoding.

            RETURN CODES: LA_OK, LA_E_PRODUCT_ID, LA_E_APP_VERSION_LENGTH
        */
        public static int SetAppVersion(string appVersion)
        {
 
            return IntPtr.Size == 8 ? LexActivatorNative.SetAppVersion_x64(appVersion) : LexActivatorNative.SetAppVersion(appVersion);
#else 
            return LexActivatorNative.SetAppVersion(appVersion);
#endif
        }

        /*
            FUNCTION: SetNetworkProxy()

            PURPOSE: Sets the network proxy to be used when contacting Cryptlex servers.

            The proxy format should be: [protocol://][username:password@]machine[:port]

            Following are some examples of the valid proxy strings:
                - http://127.0.0.1:8000/
                - http://user:pass@127.0.0.1:8000/
                - socks5://127.0.0.1:8000/

            PARAMETERS:
            * proxy - proxy string having correct proxy format

            RETURN CODES: LA_OK, LA_E_PRODUCT_ID, LA_E_NET_PROXY

            NOTE: Proxy settings of the computer are automatically detected. So, in most of the
            cases you don't need to care whether your user is behind a proxy server or not.
        */
        public static int SetNetworkProxy(string proxy)
        {
 
            return IntPtr.Size == 8 ? LexActivatorNative.SetNetworkProxy_x64(proxy) : LexActivatorNative.SetNetworkProxy(proxy);
#else 
            return LexActivatorNative.SetNetworkProxy(proxy);
#endif
        }

        /*
            FUNCTION: GetProductMetadata()

            PURPOSE: Gets the product metadata as set in the dashboard.

            This is available for trial as well as license activations.

            PARAMETERS:
            * key - key to retrieve the value
            * value - pointer to a buffer that receives the value of the string
            * length - size of the buffer pointed to by the value parameter

            RETURN CODES: LA_OK, LA_E_PRODUCT_ID, LA_E_METADATA_KEY_NOT_FOUND, LA_E_BUFFER_SIZE
        */
        public static int GetProductMetadata(string key, StringBuilder value, int length)
        {

            return IntPtr.Size == 8 ? LexActivatorNative.GetProductMetadata_x64(key, value, length) : LexActivatorNative.GetProductMetadata(key, value, length);
#else 
            return LexActivatorNative.GetProductMetadata(key, value, length);
#endif
        }

        /*
            FUNCTION: GetLicenseMetadata()

            PURPOSE: Gets the license metadata as set in the dashboard.

            PARAMETERS:
            * key - key to retrieve the value
            * value - pointer to a buffer that receives the value of the string
            * length - size of the buffer pointed to by the value parameter

            RETURN CODES: LA_OK, LA_E_PRODUCT_ID, LA_E_METADATA_KEY_NOT_FOUND, LA_E_BUFFER_SIZE
        */
        public static int GetLicenseMetadata(string key, StringBuilder value, int length)
        {

            return IntPtr.Size == 8 ? LexActivatorNative.GetLicenseMetadata_x64(key, value, length) : LexActivatorNative.GetLicenseMetadata(key, value, length);
#else 
            return LexActivatorNative.GetLicenseMetadata(key, value, length);
#endif
        }

        /*
            FUNCTION: GetLicenseMeterAttribute()

            PURPOSE: Gets the license meter attribute allowed uses and total uses.

            PARAMETERS:
            * name - name of the meter attribute
            * allowedUses - pointer to the integer that receives the value
            * totalUses - pointer to the integer that receives the value

            RETURN CODES: LA_OK, LA_FAIL, LA_E_PRODUCT_ID, LA_E_METER_ATTRIBUTE_NOT_FOUND
        */
        public static int GetLicenseMeterAttribute(string name, ref uint allowedUses, ref uint totalUses)
        {

            return IntPtr.Size == 8 ? LexActivatorNative.GetLicenseMeterAttribute_x64(name, ref allowedUses, ref totalUses) : LexActivatorNative.GetLicenseMeterAttribute(name, ref allowedUses, ref totalUses);
#else 
            return LexActivatorNative.GetLicenseMeterAttribute(name, ref allowedUses, ref totalUses);
#endif
        }

        /*
            FUNCTION: GetLicenseKey()

            PURPOSE: Gets the license key used for activation.

            PARAMETERS:
            * licenseKey - pointer to a buffer that receives the value of the string
            * length - size of the buffer pointed to by the licenseKey parameter

            RETURN CODES: LA_OK, LA_FAIL, LA_E_PRODUCT_ID, LA_E_BUFFER_SIZE
        */
        public static int GetLicenseKey(StringBuilder licenseKey, int length)
        {

            return IntPtr.Size == 8 ? LexActivatorNative.GetLicenseKey_x64(licenseKey, length) : LexActivatorNative.GetLicenseKey(licenseKey, length);
#else
            return LexActivatorNative.GetLicenseKey(licenseKey, length);
#endif
        }

        /*
            FUNCTION: GetLicenseExpiryDate()

            PURPOSE: Gets the license expiry date timestamp.

            PARAMETERS:
            * expiryDate - pointer to the integer that receives the value

            RETURN CODES: LA_OK, LA_FAIL, LA_E_PRODUCT_ID, LA_E_TIME, LA_E_TIME_MODIFIED
        */
        public static int GetLicenseExpiryDate(ref uint expiryDate)
        {
 
            return IntPtr.Size == 8 ? LexActivatorNative.GetLicenseExpiryDate_x64(ref expiryDate) : LexActivatorNative.GetLicenseExpiryDate(ref expiryDate);
#else 
            return LexActivatorNative.GetLicenseExpiryDate(ref expiryDate);
#endif
        }

        /*
            FUNCTION: GetLicenseUserEmail()

            PURPOSE: Gets the email associated with the license user.

            PARAMETERS:
            * email - pointer to a buffer that receives the value of the string
            * length - size of the buffer pointed to by the email parameter

            RETURN CODES: LA_OK, LA_FAIL, LA_E_PRODUCT_ID, LA_E_TIME, LA_E_TIME_MODIFIED,
            LA_E_BUFFER_SIZE
        */
        public static int GetLicenseUserEmail(StringBuilder email, int length)
        {

            return IntPtr.Size == 8 ? LexActivatorNative.GetLicenseUserEmail_x64(email, length) : LexActivatorNative.GetLicenseUserEmail(email, length);
#else
            return LexActivatorNative.GetLicenseUserEmail(email, length);
#endif
        }

        /*
            FUNCTION: GetLicenseUserName()

            PURPOSE: Gets the name associated with the license user.

            PARAMETERS:
            * name - pointer to a buffer that receives the value of the string
            * length - size of the buffer pointed to by the name parameter

            RETURN CODES: LA_OK, LA_FAIL, LA_E_PRODUCT_ID, LA_E_TIME, LA_E_TIME_MODIFIED,
            LA_E_BUFFER_SIZE
        */
        public static int GetLicenseUserName(StringBuilder name, int length)
        {

            return IntPtr.Size == 8 ? LexActivatorNative.GetLicenseUserName_x64(name, length) : LexActivatorNative.GetLicenseUserName(name, length);
#else
            return LexActivatorNative.GetLicenseUserName(name, length);
#endif
        }

        /*
            FUNCTION: GetLicenseUserCompany()

            PURPOSE: Gets the company associated with the license user.

            PARAMETERS:
            * company - pointer to a buffer that receives the value of the string
            * length - size of the buffer pointed to by the company parameter

            RETURN CODES: LA_OK, LA_FAIL, LA_E_PRODUCT_ID, LA_E_TIME, LA_E_TIME_MODIFIED,
            LA_E_BUFFER_SIZE
        */
        public static int GetLicenseUserCompany(StringBuilder company, int length)
        {

            return IntPtr.Size == 8 ? LexActivatorNative.GetLicenseUserCompany_x64(company, length) : LexActivatorNative.GetLicenseUserCompany(company, length);
#else
            return LexActivatorNative.GetLicenseUserCompany(company, length);
#endif
        }

        /*
            FUNCTION: GetLicenseUserMetadata()

            PURPOSE: Gets the metadata associated with the license user.

            PARAMETERS:
            * key - key to retrieve the value
            * value - pointer to a buffer that receives the value of the string
            * length - size of the buffer pointed to by the value parameter

            RETURN CODES: LA_OK, LA_FAIL, LA_E_PRODUCT_ID, LA_E_METADATA_KEY_NOT_FOUND, LA_E_BUFFER_SIZE
        */
        public static int GetLicenseUserMetadata(string key, StringBuilder value, int length)
        {

            return IntPtr.Size == 8 ? LexActivatorNative.GetLicenseUserMetadata_x64(key, value, length) : LexActivatorNative.GetLicenseUserMetadata(key, value, length);
#else 
            return LexActivatorNative.GetLicenseUserMetadata(key, value, length);
#endif
        }

        /*
            FUNCTION: GetLicenseType()

            PURPOSE: Gets the license type (node-locked or hosted-floating).

            PARAMETERS:
            * name - pointer to a buffer that receives the value of the string
            * length - size of the buffer pointed to by the licenseType parameter

            RETURN CODES: LA_OK, LA_FAIL, LA_E_PRODUCT_ID, LA_E_TIME, LA_E_TIME_MODIFIED,
            LA_E_BUFFER_SIZE
        */
        public static int GetLicenseType(StringBuilder licenseType, int length)
        {

            return IntPtr.Size == 8 ? LexActivatorNative.GetLicenseType_x64(licenseType, length) : LexActivatorNative.GetLicenseType(licenseType, length);
#else
            return LexActivatorNative.GetLicenseType(licenseType, length);
#endif
        }

        /*
            FUNCTION: GetActivationMetadata()

            PURPOSE: Gets the activation metadata.

            PARAMETERS:
            * key - key to retrieve the value
            * value - pointer to a buffer that receives the value of the string
            * length - size of the buffer pointed to by the value parameter

            RETURN CODES: LA_OK, LA_FAIL, LA_E_PRODUCT_ID, LA_E_METADATA_KEY_NOT_FOUND, LA_E_BUFFER_SIZE
        */
        public static int GetActivationMetadata(string key, StringBuilder value, int length)
        {

            return IntPtr.Size == 8 ? LexActivatorNative.GetActivationMetadata_x64(key, value, length) : LexActivatorNative.GetActivationMetadata(key, value, length);
#else 
            return LexActivatorNative.GetActivationMetadata(key, value, length);
#endif
        }

        /*
            FUNCTION: GetActivationMeterAttributeUses()

            PURPOSE: Gets the meter attribute uses consumed by the activation.

            PARAMETERS:
            * name - name of the meter attribute
            * uses - pointer to the integer that receives the value

            RETURN CODES: LA_OK, LA_FAIL, LA_E_PRODUCT_ID, LA_E_METER_ATTRIBUTE_NOT_FOUND
        */
        public static int GetActivationMeterAttributeUses(string name, ref uint uses)
        {

            return IntPtr.Size == 8 ? LexActivatorNative.GetActivationMeterAttributeUses_x64(name, ref uses) : LexActivatorNative.GetActivationMeterAttributeUses(name, ref uses);
#else 
            return LexActivatorNative.GetActivationMeterAttributeUses(name, ref uses);
#endif
        }

        /*
            FUNCTION: GetServerSyncGracePeriodExpiryDate()

            PURPOSE: Gets the server sync grace period expiry date timestamp.

            PARAMETERS:
            * expiryDate - pointer to the integer that receives the value

            RETURN CODES: LA_OK, LA_FAIL, LA_E_PRODUCT_ID, LA_E_TIME, LA_E_TIME_MODIFIED
        */
        public static int GetServerSyncGracePeriodExpiryDate(ref uint expiryDate)
        {
 
            return IntPtr.Size == 8 ? LexActivatorNative.GetServerSyncGracePeriodExpiryDate_x64(ref expiryDate) : LexActivatorNative.GetServerSyncGracePeriodExpiryDate(ref expiryDate);
#else 
            return LexActivatorNative.GetServerSyncGracePeriodExpiryDate(ref expiryDate);
#endif
        }

        /*
            FUNCTION: GetTrialActivationMetadata()

            PURPOSE: Gets the trial activation metadata.

            PARAMETERS:
            * key - key to retrieve the value
            * value - pointer to a buffer that receives the value of the string
            * length - size of the buffer pointed to by the value parameter

            RETURN CODES: LA_OK, LA_FAIL, LA_E_PRODUCT_ID, LA_E_METADATA_KEY_NOT_FOUND, LA_E_BUFFER_SIZE
        */
        public static int GetTrialActivationMetadata(string key, StringBuilder value, int length)
        {

            return IntPtr.Size == 8 ? LexActivatorNative.GetTrialActivationMetadata_x64(key, value, length) : LexActivatorNative.GetTrialActivationMetadata(key, value, length);
#else 
            return LexActivatorNative.GetTrialActivationMetadata(key, value, length);
#endif
        }

        /*
            FUNCTION: GetTrialExpiryDate()

            PURPOSE: Gets the trial expiry date timestamp.

            PARAMETERS:
            * trialExpiryDate - pointer to the integer that receives the value

            RETURN CODES: LA_OK, LA_FAIL, LA_E_PRODUCT_ID, LA_E_TIME, LA_E_TIME_MODIFIED
        */
        public static int GetTrialExpiryDate(ref uint trialExpiryDate)
        {
 
            return IntPtr.Size == 8 ? LexActivatorNative.GetTrialExpiryDate_x64(ref trialExpiryDate) : LexActivatorNative.GetTrialExpiryDate(ref trialExpiryDate);
#else 
            return LexActivatorNative.GetTrialExpiryDate(ref trialExpiryDate);
#endif
        }

        /*
            FUNCTION: GetTrialId()

            PURPOSE: Gets the trial activation id. Used in case of trial extension.

            PARAMETERS:
            * trialId - pointer to a buffer that receives the value of the string
            * length - size of the buffer pointed to by the value parameter

            RETURN CODES: LA_OK, LA_FAIL, LA_E_PRODUCT_ID, LA_E_TIME, LA_E_TIME_MODIFIED,
            LA_E_BUFFER_SIZE
        */
        public static int GetTrialId(StringBuilder trialId, int length)
        {

            return IntPtr.Size == 8 ? LexActivatorNative.GetTrialId_x64(trialId, length) : LexActivatorNative.GetTrialId(trialId, length);
#else
            return LexActivatorNative.GetTrialId(trialId, length);
#endif
        }

        /*
            FUNCTION: GetLocalTrialExpiryDate()

            PURPOSE: Gets the trial expiry date timestamp.

            PARAMETERS:
            * trialExpiryDate - pointer to the integer that receives the value

            RETURN CODES: LA_OK, LA_FAIL, LA_E_PRODUCT_ID, LA_E_TIME_MODIFIED
        */
        public static int GetLocalTrialExpiryDate(ref uint trialExpiryDate)
        {
 
            return IntPtr.Size == 8 ? LexActivatorNative.GetLocalTrialExpiryDate_x64(ref trialExpiryDate) : LexActivatorNative.GetLocalTrialExpiryDate(ref trialExpiryDate);
#else 
            return LexActivatorNative.GetLocalTrialExpiryDate(ref trialExpiryDate);
#endif
        }

        /*
            FUNCTION: CheckForReleaseUpdate()

            PURPOSE: Checks whether a new release is available for the product.

            This function should only be used if you manage your releases through
            Cryptlex release management API.

            PARAMETERS:
            * platform - release platform e.g. windows, macos, linux
            * version - current release version
            * channel - release channel e.g. stable
            * callback - name of the callback function.

            RETURN CODES: LA_OK, LA_E_PRODUCT_ID, LA_E_LICENSE_KEY, LA_E_RELEASE_VERSION_FORMAT
        */
        public static int CheckForReleaseUpdate(string platform, string version, string channel, CallbackType callback)
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

            return IntPtr.Size == 8 ? LexActivatorNative.CheckForReleaseUpdate_x64(platform, version, channel, wrappedCallback) : LexActivatorNative.CheckForReleaseUpdate(platform, version, channel, wrappedCallback);
#else 
            return LexActivatorNative.CheckForReleaseUpdate(platform, version, channel, wrappedCallback);
#endif
        }

        /*
            FUNCTION: ActivateLicense()

            PURPOSE: Activates the license by contacting the Cryptlex servers. It
            validates the key and returns with encrypted and digitally signed token
            which it stores and uses to activate your application.

            This function should be executed at the time of registration, ideally on
            a button click.

            RETURN CODES: LA_OK, LA_EXPIRED, LA_SUSPENDED, LA_E_REVOKED, LA_FAIL, LA_E_PRODUCT_ID,
            LA_E_INET, LA_E_VM, LA_E_TIME, LA_E_ACTIVATION_LIMIT, LA_E_SERVER, LA_E_CLIENT,
            LA_E_AUTHENTICATION_FAILED, LA_E_LICENSE_TYPE, LA_E_COUNTRY, LA_E_IP, LA_E_RATE_LIMIT, LA_E_LICENSE_KEY
        */
        public static int ActivateLicense()
        {

            return IntPtr.Size == 8 ? LexActivatorNative.ActivateLicense_x64() : LexActivatorNative.ActivateLicense();
#else
            return LexActivatorNative.ActivateLicense();
#endif

        }

        /*
            FUNCTION: ActivateLicenseOffline()

            PURPOSE: Activates your licenses using the offline activation response file.

            PARAMETERS:
            * filePath - path of the offline activation response file.

            RETURN CODES: LA_OK, LA_EXPIRED, LA_FAIL, LA_E_PRODUCT_ID, LA_E_LICENSE_KEY, LA_E_OFFLINE_RESPONSE_FILE
            LA_E_VM, LA_E_TIME, LA_E_FILE_PATH, LA_E_OFFLINE_RESPONSE_FILE_EXPIRED
        */
        public static int ActivateLicenseOffline(string filePath)
        {

            return IntPtr.Size == 8 ? LexActivatorNative.ActivateLicenseOffline_x64(filePath) : LexActivatorNative.ActivateLicenseOffline(filePath);
#else 
            return LexActivatorNative.ActivateLicenseOffline(filePath);
#endif
        }

        /*
            FUNCTION: GenerateOfflineActivationRequest()

            PURPOSE: Generates the offline activation request needed for generating
            offline activation response in the dashboard.

            PARAMETERS:
            * filePath - path of the file for the offline request.

            RETURN CODES: LA_OK, LA_FAIL, LA_E_PRODUCT_ID, LA_E_LICENSE_KEY, LA_E_FILE_PERMISSION
        */
        public static int GenerateOfflineActivationRequest(string filePath)
        {

            return IntPtr.Size == 8 ? LexActivatorNative.GenerateOfflineActivationRequest_x64(filePath) : LexActivatorNative.GenerateOfflineActivationRequest(filePath);
#else 
            return LexActivatorNative.GenerateOfflineActivationRequest(filePath);
#endif
        }

        /*
            FUNCTION: DeactivateLicense()

            PURPOSE: Deactivates the license activation and frees up the corresponding activation
            slot by contacting the Cryptlex servers.

            This function should be executed at the time of de-registration, ideally on
            a button click.

            RETURN CODES: LA_OK, LA_E_DEACTIVATION_LIMIT, LA_FAIL, LA_E_PRODUCT_ID, LA_E_TIME
            LA_E_LICENSE_KEY, LA_E_INET, LA_E_SERVER, LA_E_RATE_LIMIT, LA_E_TIME_MODIFIED
        */
        public static int DeactivateLicense()
        {

            return IntPtr.Size == 8 ? LexActivatorNative.DeactivateLicense_x64() : LexActivatorNative.DeactivateLicense();
#else
            return LexActivatorNative.DeactivateLicense();
#endif
        }

        /*
            FUNCTION: GenerateOfflineDeactivationRequest()

            PURPOSE: Generates the offline deactivation request needed for deactivation of
            the license in the dashboard and deactivates the license locally.

            A valid offline deactivation file confirms that the license has been successfully
            deactivated on the user's machine.

            PARAMETERS:
            * filePath - path of the file for the offline request.

            RETURN CODES: LA_OK, LA_FAIL, LA_E_PRODUCT_ID, LA_E_LICENSE_KEY, LA_E_FILE_PERMISSION,
            LA_E_TIME, LA_E_TIME_MODIFIED
        */
        public static int GenerateOfflineDeactivationRequest(string filePath)
        {

            return IntPtr.Size == 8 ? LexActivatorNative.GenerateOfflineDeactivationRequest_x64(filePath) : LexActivatorNative.GenerateOfflineDeactivationRequest(filePath);
#else 
            return LexActivatorNative.GenerateOfflineDeactivationRequest(filePath);
#endif
        }

        /*
            FUNCTION: IsLicenseGenuine()

            PURPOSE: It verifies whether your app is genuinely activated or not. The verification is
            done locally by verifying the cryptographic digital signature fetched at the time of
            activation.

            After verifying locally, it schedules a server check in a separate thread. After the
            first server sync it periodically does further syncs at a frequency set for the license.

            In case server sync fails due to network error, and it continues to fail for fixed
            number of days (grace period), the function returns LA_GRACE_PERIOD_OVER instead of LA_OK.

            This function must be called on every start of your program to verify the activation
            of your app.

            RETURN CODES: LA_OK, LA_EXPIRED, LA_SUSPENDED, LA_GRACE_PERIOD_OVER, LA_FAIL,
            LA_E_PRODUCT_ID, LA_E_LICENSE_KEY, LA_E_TIME, LA_E_TIME_MODIFIED

            NOTE: If application was activated offline using ActivateLicenseOffline() function, you
            may want to set grace period to 0 to ignore grace period.
        */
        public static int IsLicenseGenuine()
        {

            return IntPtr.Size == 8 ? LexActivatorNative.IsLicenseGenuine_x64() : LexActivatorNative.IsLicenseGenuine();
#else 
            return LexActivatorNative.IsLicenseGenuine();
#endif
        }

        /*
            FUNCTION: IsLicenseValid()

            PURPOSE: It verifies whether your app is genuinely activated or not. The verification is
            done locally by verifying the cryptographic digital signature fetched at the time of
            activation.

            This is just an auxiliary function which you may use in some specific cases, when you
            want to skip the server sync.

            RETURN CODES: LA_OK, LA_EXPIRED, LA_SUSPENDED, LA_GRACE_PERIOD_OVER, LA_FAIL,
            LA_E_PRODUCT_ID, LA_E_LICENSE_KEY, LA_E_TIME, LA_E_TIME_MODIFIED

            NOTE: You may want to set grace period to 0 to ignore grace period.
        */
        public static int IsLicenseValid()
        {

            return IntPtr.Size == 8 ? LexActivatorNative.IsLicenseValid_x64() : LexActivatorNative.IsLicenseValid();
#else 
            return LexActivatorNative.IsLicenseValid();
#endif
        }

        /*
             FUNCTION: ActivateTrial()

             PURPOSE: Starts the verified trial in your application by contacting the
             Cryptlex servers.

             This function should be executed when your application starts first time on
             the user's computer, ideally on a button click.

             RETURN CODES: LA_OK, LA_TRIAL_EXPIRED, LA_FAIL, LA_E_PRODUCT_ID, LA_E_INET,
             LA_E_VM, LA_E_TIME, LA_E_SERVER, LA_E_CLIENT, LA_E_COUNTRY, LA_E_IP, LA_E_RATE_LIMIT
         */
        public static int ActivateTrial()
        {
 
            return IntPtr.Size == 8 ? LexActivatorNative.ActivateTrial_x64() : LexActivatorNative.ActivateTrial();
#else 
            return LexActivatorNative.ActivateTrial();
#endif
        }

        /*
            FUNCTION: ActivateTrialOffline()

            PURPOSE: Activates your trial using the offline activation response file.

            PARAMETERS:
            * filePath - path of the offline activation response file.

            RETURN CODES: LA_OK, LA_TRIAL_EXPIRED, LA_FAIL, LA_E_PRODUCT_ID, LA_E_OFFLINE_RESPONSE_FILE
            LA_E_VM, LA_E_TIME, LA_E_FILE_PATH, LA_E_OFFLINE_RESPONSE_FILE_EXPIRED
        */
        public static int ActivateTrialOffline(string filePath)
        {

            return IntPtr.Size == 8 ? LexActivatorNative.ActivateTrialOffline_x64(filePath) : LexActivatorNative.ActivateTrialOffline(filePath);
#else 
            return LexActivatorNative.ActivateTrialOffline(filePath);
#endif
        }

        /*
            FUNCTION: GenerateOfflineTrialActivationRequest()

            PURPOSE: Generates the offline trial activation request needed for generating
            offline trial activation response in the dashboard.

            PARAMETERS:
            * filePath - path of the file for the offline request.

            RETURN CODES: LA_OK, LA_FAIL, LA_E_PRODUCT_ID, LA_E_FILE_PERMISSION
        */
        public static int GenerateOfflineTrialActivationRequest(string filePath)
        {

            return IntPtr.Size == 8 ? LexActivatorNative.GenerateOfflineTrialActivationRequest_x64(filePath) : LexActivatorNative.GenerateOfflineTrialActivationRequest(filePath);
#else 
            return LexActivatorNative.GenerateOfflineTrialActivationRequest(filePath);
#endif
        }

        /*
            FUNCTION: IsTrialGenuine()

            PURPOSE: It verifies whether trial has started and is genuine or not. The
            verification is done locally by verifying the cryptographic digital signature
            fetched at the time of trial activation.

            This function must be called on every start of your program during the trial period.

            RETURN CODES: LA_OK, LA_TRIAL_EXPIRED, LA_FAIL, LA_E_TIME, LA_E_PRODUCT_ID,
            LA_E_TIME_MODIFIED
        */
        public static int IsTrialGenuine()
        {
 
            return IntPtr.Size == 8 ? LexActivatorNative.IsTrialGenuine_x64() : LexActivatorNative.IsTrialGenuine();
#else 
            return LexActivatorNative.IsTrialGenuine();
#endif
        }

        /*
            FUNCTION: ActivateLocalTrial()

            PURPOSE: Starts the local(unverified) trial.

            This function should be executed when your application starts first time on
            the user's computer.

            PARAMETERS:
            * trialLength - trial length in days

            RETURN CODES: LA_OK, LA_LOCAL_TRIAL_EXPIRED, LA_FAIL, LA_E_PRODUCT_ID, LA_E_TIME_MODIFIED

            NOTE: The function is only meant for local(unverified) trials.
        */
        public static int ActivateLocalTrial(uint trialLength)
        {
 
            return IntPtr.Size == 8 ? LexActivatorNative.ActivateLocalTrial_x64(trialLength) : LexActivatorNative.ActivateLocalTrial(trialLength);
#else 
            return LexActivatorNative.ActivateLocalTrial(trialLength);
#endif
        }

        /*
            FUNCTION: IsLocalTrialGenuine()

            PURPOSE: It verifies whether trial has started and is genuine or not. The
            verification is done locally.

            This function must be called on every start of your program during the trial period.

            RETURN CODES: LA_OK, LA_LOCAL_TRIAL_EXPIRED, LA_FAIL, LA_E_PRODUCT_ID, LA_E_TIME_MODIFIED

            NOTE: The function is only meant for local(unverified) trials.
        */
        public static int IsLocalTrialGenuine()
        {
 
            return IntPtr.Size == 8 ? LexActivatorNative.IsLocalTrialGenuine_x64() : LexActivatorNative.IsLocalTrialGenuine();
#else 
            return LexActivatorNative.IsLocalTrialGenuine();
#endif
        }

        /*
            FUNCTION: ExtendLocalTrial()

            PURPOSE: Extends the local trial.

            PARAMETERS:
            * trialExtensionLength - number of days to extend the trial

            RETURN CODES: LA_OK, LA_FAIL, LA_E_PRODUCT_ID, LA_E_TIME_MODIFIED

            NOTE: The function is only meant for local(unverified) trials.
        */
        public static int ExtendLocalTrial(uint trialExtensionLength)
        {
 
            return IntPtr.Size == 8 ? LexActivatorNative.ExtendLocalTrial_x64(trialExtensionLength) : LexActivatorNative.ExtendLocalTrial(trialExtensionLength);
#else 
            return LexActivatorNative.ExtendLocalTrial(trialExtensionLength);
#endif
        }

        /*
            FUNCTION: IncrementActivationMeterAttributeUses()

            PURPOSE: Increments the meter attribute uses of the activation.

            PARAMETERS:
            * name - name of the meter attribute
            * increment - the increment value

            RETURN CODES: LA_OK, LA_FAIL, LA_E_PRODUCT_ID, LA_E_METER_ATTRIBUTE_NOT_FOUND,
            LA_E_INET, LA_E_TIME, LA_E_SERVER, LA_E_CLIENT, LA_E_METER_ATTRIBUTE_USES_LIMIT_REACHED,
            LA_E_AUTHENTICATION_FAILED, LA_E_COUNTRY, LA_E_IP, LA_E_RATE_LIMIT, LA_E_LICENSE_KEY

        */
        public static int IncrementActivationMeterAttributeUses(string name, uint increment)
        {
 
            return IntPtr.Size == 8 ? LexActivatorNative.IncrementActivationMeterAttributeUses_x64(name, increment) : LexActivatorNative.IncrementActivationMeterAttributeUses(name, increment);
#else 
            return LexActivatorNative.IncrementActivationMeterAttributeUses(name, increment);
#endif
        }

        /*
            FUNCTION: DecrementActivationMeterAttributeUses()

            PURPOSE: Decrements the meter attribute uses of the activation.

            PARAMETERS:
            * name - name of the meter attribute
            * decrement - the decrement value

            RETURN CODES: LA_OK, LA_FAIL, LA_E_PRODUCT_ID, LA_E_METER_ATTRIBUTE_NOT_FOUND,
            LA_E_INET, LA_E_TIME, LA_E_SERVER, LA_E_CLIENT, LA_E_RATE_LIMIT, LA_E_LICENSE_KEY,
            LA_E_AUTHENTICATION_FAILED, LA_E_COUNTRY, LA_E_IP, LA_E_ACTIVATION_NOT_FOUND

            NOTE: If the decrement is more than the current uses, it resets the uses to 0.
        */
        public static int DecrementActivationMeterAttributeUses(string name, uint decrement)
        {
 
            return IntPtr.Size == 8 ? LexActivatorNative.DecrementActivationMeterAttributeUses_x64(name, decrement) : LexActivatorNative.DecrementActivationMeterAttributeUses(name, decrement);
#else 
            return LexActivatorNative.DecrementActivationMeterAttributeUses(name, decrement);
#endif
        }

        /*
            FUNCTION: ResetActivationMeterAttributeUses()

            PURPOSE: Resets the meter attribute uses consumed by the activation.

            PARAMETERS:
            * name - name of the meter attribute

            RETURN CODES: LA_OK, LA_FAIL, LA_E_PRODUCT_ID, LA_E_METER_ATTRIBUTE_NOT_FOUND,
            LA_E_INET, LA_E_TIME, LA_E_SERVER, LA_E_CLIENT, LA_E_RATE_LIMIT, LA_E_LICENSE_KEY,
            LA_E_AUTHENTICATION_FAILED, LA_E_COUNTRY, LA_E_IP, LA_E_ACTIVATION_NOT_FOUND
        */
        public static int ResetActivationMeterAttributeUses(string name)
        {
 
            return IntPtr.Size == 8 ? LexActivatorNative.ResetActivationMeterAttributeUses_x64(name) : LexActivatorNative.ResetActivationMeterAttributeUses(name);
#else 
            return LexActivatorNative.ResetActivationMeterAttributeUses(name);
#endif
        }

        /*
            FUNCTION: Reset()

            PURPOSE: Resets the activation and trial data stored in the machine.

            This function is meant for developer testing only.

            RETURN CODES: LA_OK, LA_E_PRODUCT_ID

            NOTE: The function does not reset local(unverified) trial data.
        */
        public static int Reset()
        {
 
            return IntPtr.Size == 8 ? LexActivatorNative.Reset_x64() : LexActivatorNative.Reset();
#else 
            return LexActivatorNative.Reset();
#endif
        }

        public static class StatusCodes
        {
            /*
                CODE: LA_OK

                MESSAGE: Success code.
            */
            public const int LA_OK = 0;

            /*
                CODE: LA_FAIL

                MESSAGE: Failure code.
            */
            public const int LA_FAIL = 1;

            /*
                CODE: LA_EXPIRED

                MESSAGE: The license has expired or system time has been tampered
                with. Ensure your date and time settings are correct.
            */
            public const int LA_EXPIRED = 20;

            /*
                CODE: LA_SUSPENDED

                MESSAGE: The license has been suspended.
            */
            public const int LA_SUSPENDED = 21;

            /*
                CODE: LA_GRACE_PERIOD_OVER

                MESSAGE: The grace period for server sync is over.
            */
            public const int LA_GRACE_PERIOD_OVER = 22;

            /*
                CODE: LA_TRIAL_EXPIRED

                MESSAGE: The trial has expired or system time has been tampered
                with. Ensure your date and time settings are correct.
            */
            public const int LA_TRIAL_EXPIRED = 25;

            /*
                CODE: LA_LOCAL_TRIAL_EXPIRED

                MESSAGE: The local trial has expired or system time has been tampered
                with. Ensure your date and time settings are correct.
            */
            public const int LA_LOCAL_TRIAL_EXPIRED = 26;

            /*
                CODE: LA_RELEASE_UPDATE_AVAILABLE

                MESSAGE: A new update is available for the product. This means a new release has
                been published for the product.
            */
            public const int LA_RELEASE_UPDATE_AVAILABLE = 30;

            /*
                CODE: LA_RELEASE_NO_UPDATE_AVAILABLE

                MESSAGE: No new update is available for the product. The current version is latest.
            */
            public const int LA_RELEASE_NO_UPDATE_AVAILABLE = 31;

            /*
                CODE: LA_E_FILE_PATH

                MESSAGE: Invalid file path.
            */
            public const int LA_E_FILE_PATH = 40;

            /*
                CODE: LA_E_PRODUCT_FILE

                MESSAGE: Invalid or corrupted product file.
            */
            public const int LA_E_PRODUCT_FILE = 41;

            /*
                CODE: LA_E_PRODUCT_DATA

                MESSAGE: Invalid product data.
            */
            public const int LA_E_PRODUCT_DATA = 42;

            /*
                CODE: LA_E_PRODUCT_ID

                MESSAGE: The product id is incorrect.
            */
            public const int LA_E_PRODUCT_ID = 43;

            /*
                CODE: LA_E_SYSTEM_PERMISSION

                MESSAGE: Insufficent system permissions. Occurs when LA_SYSTEM flag is used
                but application is not run with admin privileges.
            */
            public const int LA_E_SYSTEM_PERMISSION = 44;

            /*
                CODE: LA_E_FILE_PERMISSION

                MESSAGE: No permission to write to file.
            */
            public const int LA_E_FILE_PERMISSION = 45;

            /*
                CODE: LA_E_WMIC

                MESSAGE: Fingerprint couldn't be generated because Windows Management
                Instrumentation (WMI) service has been disabled. This error is specific
                to Windows only.
            */
            public const int LA_E_WMIC = 46;

            /*
                CODE: LA_E_TIME

                MESSAGE: The difference between the network time and the system time is
                more than allowed clock offset.
            */
            public const int LA_E_TIME = 47;

            /*
                CODE: LA_E_INET

                MESSAGE: Failed to connect to the server due to network error.
            */
            public const int LA_E_INET = 48;

            /*
                CODE: LA_E_NET_PROXY

                MESSAGE: Invalid network proxy.
            */
            public const int LA_E_NET_PROXY = 49;

            /*
                CODE: LA_E_HOST_URL

                MESSAGE: Invalid Cryptlex host url.
            */
            public const int LA_E_HOST_URL = 50;

            /*
                CODE: LA_E_BUFFER_SIZE

                MESSAGE: The buffer size was smaller than required.
            */
            public const int LA_E_BUFFER_SIZE = 51;

            /*
                CODE: LA_E_APP_VERSION_LENGTH

                MESSAGE: App version length is more than 256 characters.
            */
            public const int LA_E_APP_VERSION_LENGTH = 52;

            /*
                CODE: LA_E_REVOKED

                MESSAGE: The license has been revoked.
            */
            public const int LA_E_REVOKED = 53;

            /*
                CODE: LA_E_LICENSE_KEY

                MESSAGE: Invalid license key.
            */
            public const int LA_E_LICENSE_KEY = 54;

            /*
                CODE: LA_E_LICENSE_TYPE

                MESSAGE: Invalid license type. Make sure floating license
                is not being used.
            */
            public const int LA_E_LICENSE_TYPE = 55;

            /*
                CODE: LA_E_OFFLINE_RESPONSE_FILE

                MESSAGE: Invalid offline activation response file.
            */
            public const int LA_E_OFFLINE_RESPONSE_FILE = 56;

            /*
                CODE: LA_E_OFFLINE_RESPONSE_FILE_EXPIRED

                MESSAGE: The offline activation response has expired.
            */
            public const int LA_E_OFFLINE_RESPONSE_FILE_EXPIRED = 57;

            /*
                CODE: LA_E_ACTIVATION_LIMIT

                MESSAGE: The license has reached it's allowed activations limit.
            */
            public const int LA_E_ACTIVATION_LIMIT = 58;

            /*
                CODE: LA_E_ACTIVATION_NOT_FOUND

                MESSAGE: The license activation was deleted on the server.
            */
            public const int LA_E_ACTIVATION_NOT_FOUND = 59;

            /*
                CODE: LA_E_DEACTIVATION_LIMIT

                MESSAGE: The license has reached it's allowed deactivations limit.
            */
            public const int LA_E_DEACTIVATION_LIMIT = 60;

            /*
                CODE: LA_E_TRIAL_NOT_ALLOWED

                MESSAGE: Trial not allowed for the product.
            */
            public const int LA_E_TRIAL_NOT_ALLOWED = 61;

            /*
                CODE: LA_E_TRIAL_ACTIVATION_LIMIT

                MESSAGE: Your account has reached it's trial activations limit.
            */
            public const int LA_E_TRIAL_ACTIVATION_LIMIT = 62;

            /*
                CODE: LA_E_MACHINE_FINGERPRINT

                MESSAGE: Machine fingerprint has changed since activation.
            */
            public const int LA_E_MACHINE_FINGERPRINT = 63;

            /*
                CODE: LA_E_METADATA_KEY_LENGTH

                MESSAGE: Metadata key length is more than 256 characters.
            */
            public const int LA_E_METADATA_KEY_LENGTH = 64;

            /*
                CODE: LA_E_METADATA_VALUE_LENGTH

                MESSAGE: Metadata value length is more than 256 characters.
            */
            public const int LA_E_METADATA_VALUE_LENGTH = 65;

            /*
                CODE: LA_E_ACTIVATION_METADATA_LIMIT

                MESSAGE: The license has reached it's metadata fields limit.
            */
            public const int LA_E_ACTIVATION_METADATA_LIMIT = 66;

            /*
                CODE: LA_E_TRIAL_ACTIVATION_METADATA_LIMIT

                MESSAGE: The trial has reached it's metadata fields limit.
            */
            public const int LA_E_TRIAL_ACTIVATION_METADATA_LIMIT = 67;

            /*
                CODE: LA_E_METADATA_KEY_NOT_FOUND

                MESSAGE: The metadata key does not exist.
            */
            public const int LA_E_METADATA_KEY_NOT_FOUND = 68;

            /*
                CODE: LA_E_TIME_MODIFIED

                MESSAGE: The system time has been tampered (backdated).
            */
            public const int LA_E_TIME_MODIFIED = 69;

            /*
                CODE: LA_E_RELEASE_VERSION_FORMAT

                MESSAGE: Invalid version format.
            */
            public const int LA_E_RELEASE_VERSION_FORMAT = 70;

            /*
                CODE: LA_E_AUTHENTICATION_FAILED

                MESSAGE: Incorrect email or password.
            */
            public const int LA_E_AUTHENTICATION_FAILED = 71;

            /*
                CODE: LA_E_METER_ATTRIBUTE_NOT_FOUND

                MESSAGE: The meter attribute does not exist.
            */
            public const int LA_E_METER_ATTRIBUTE_NOT_FOUND = 72;

            /*
                CODE: LA_E_METER_ATTRIBUTE_USES_LIMIT_REACHED

                MESSAGE: The meter attribute has reached it's usage limit.
            */
            public const int LA_E_METER_ATTRIBUTE_USES_LIMIT_REACHED = 73;

            /*
                CODE: LA_E_VM

                MESSAGE: Application is being run inside a virtual machine / hypervisor,
                and activation has been disallowed in the VM.
            */
            public const int LA_E_VM = 80;

            /*
                CODE: LA_E_COUNTRY

                MESSAGE: Country is not allowed.
            */
            public const int LA_E_COUNTRY = 81;

            /*
                CODE: LA_E_IP

                MESSAGE: IP address is not allowed.
            */
            public const int LA_E_IP = 82;

            /*
                CODE: LA_E_RATE_LIMIT

                MESSAGE: Rate limit for API has reached, try again later.
            */
            public const int LA_E_RATE_LIMIT = 90;

            /*
                CODE: LA_E_SERVER

                MESSAGE: Server error.
            */
            public const int LA_E_SERVER = 91;

            /*
                CODE: LA_E_CLIENT

                MESSAGE: Client error.
            */
            public const int LA_E_CLIENT = 92;
        };

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void CallbackType(uint status);

        /* To prevent garbage collection of delegate, need to keep a reference */
        static readonly List<CallbackType> callbackList = new List<CallbackType>();

        
    }
}
