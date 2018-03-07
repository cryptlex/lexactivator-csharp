using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Cryptlex
{
    public static class LexActivator
    {
        private const string DLL_FILE_NAME = "LexActivator.dll";

        /*
            In order to use "Any CPU" configuration, rename 64 bit LexActivator.dll to LexActivator64.dll and add "LA_ANY_CPU"
	        conditional compilation symbol in your project properties.
        */
#if LA_ANY_CPU
        private const string DLL_FILE_NAME_X64 = "LexActivator64.dll";
#endif
        public enum PermissionFlags : uint
        {
            LA_USER = 1,
            LA_SYSTEM = 2,
        }

        /*
            FUNCTION: SetProductFile()

            PURPOSE: Sets the absolute path of the Product.dat file.

            This function must be called on every start of your program
            before any other functions are called.

            PARAMETERS:
            * filePath - absolute path of the product file (Product.dat)

            RETURN CODES: LA_OK, LA_E_FPATH, LA_E_PFILE

            NOTE: If this function fails to set the path of product file, none of the
            other functions will work.
        */

        public static int SetProductFile(string filePath)
        {
#if LA_ANY_CPU
            return IntPtr.Size == 8 ? Native.SetProductFile_x64(filePath) : Native.SetProductFile(filePath);
#else
            return Native.SetProductFile(filePath);
#endif
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

            RETURN CODES: LA_OK, LA_E_PDATA

            NOTE: If this function fails to set the product data, none of the
            other functions will work.
        */

        public static int SetProductData(string productData)
        {
#if LA_ANY_CPU
            return IntPtr.Size == 8 ? Native.SetProductData_x64(productData) : Native.SetProductData(productData);
#else
            return Native.SetProductData(productData);
#endif
        }

        /*
            FUNCTION: SetProductVersionGuid()

            PURPOSE: Sets the version GUID of your application.

            This function must be called on every start of your program before
            any other functions are called, with the exception of SetProductFile()
            or SetProductData() function.

            PARAMETERS:
            * versionGuid - the unique version GUID of your application as mentioned
            on the product version page of your application in the dashboard.

            * flags - depending upon whether your application requires admin/root
            permissions to run or not, this parameter can have one of the following
            values: LA_SYSTEM, LA_USER

            RETURN CODES: LA_OK, LA_E_WMIC, LA_E_PFILE, LA_E_PDATA, LA_E_GUID, LA_E_PERMISSION

            NOTE: If this function fails to set the version GUID, none of the other
            functions will work.
        */

        public static int SetProductVersionGuid(string versionGuid, PermissionFlags flags)
        {
#if LA_ANY_CPU
            return IntPtr.Size == 8 ? Native.SetProductVersionGuid_x64(versionGuid, flags) : Native.SetProductVersionGuid(versionGuid, flags);
#else
            return Native.SetProductVersionGuid(versionGuid, flags);
#endif
        }

        /*
            FUNCTION: SetProductKey()

            PURPOSE: Sets the product key required to activate the application.

            PARAMETERS:
            * productKey - a valid product key generated for the application.

            RETURN CODES: LA_OK, LA_E_GUID, LA_E_PKEY
        */

        public static int SetProductKey(string productKey)
        {
#if LA_ANY_CPU
            return IntPtr.Size == 8 ? Native.SetProductKey_x64(productKey) : Native.SetProductKey(productKey);
#else
            return Native.SetProductKey(productKey);
#endif
        }

        /*
            FUNCTION: SetActivationExtraData()

            PURPOSE: Sets the extra data which you may want to fetch from the user
            at the time of activation.

            The extra data appears along with the activation details of the product key
            in dashboard.

            PARAMETERS:
            * extraData - string of maximum length 1024 characters with utf-8 encoding.

            RETURN CODES: LA_OK, LA_E_GUID, LA_E_EDATA_LEN
        */

        public static int SetActivationExtraData(string extraData)
        {
#if LA_ANY_CPU
            return IntPtr.Size == 8 ? Native.SetActivationExtraData_x64(extraData) : Native.SetActivationExtraData(extraData);
#else
            return Native.SetActivationExtraData(extraData);
#endif
        }

        /*
            FUNCTION: SetTrialActivationExtraData()

            PURPOSE: Sets the extra data which you may want to fetch from the user
            at the time of trial activation.

            The extra data appears along with the trial activation details in dashboard.

            PARAMETERS:
            * extraData - string of maximum length 1024 characters with utf-8 encoding.

            RETURN CODES: LA_OK, LA_E_GUID, LA_E_EDATA_LEN
        */

        public static int SetTrialActivationExtraData(string extraData)
        {
#if LA_ANY_CPU
            return IntPtr.Size == 8 ? Native.SetTrialActivationExtraData_x64(extraData) : Native.SetTrialActivationExtraData(extraData);
#else
            return Native.SetTrialActivationExtraData(extraData);
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

            RETURN CODES: LA_OK, LA_E_GUID, LA_E_NET_PROXY

            NOTE: Proxy settings of the computer are automatically detected. So, in most of the
            cases you don't need to care whether your user is behind a proxy server or not.
        */

        public static int SetNetworkProxy(string proxy)
        {
#if LA_ANY_CPU 
            return IntPtr.Size == 8 ? Native.SetNetworkProxy_x64(proxy) : Native.SetNetworkProxy(proxy);
#else 
            return Native.SetNetworkProxy(proxy); 
#endif
        }

        /*
            FUNCTION: GetAppVersion()

            PURPOSE: Gets the app version of the product as set in the dashboard.

            PARAMETERS:
            * appVersion - pointer to a buffer that receives the value of the string
            * length - size of the buffer pointed to by the appVersion parameter

            RETURN CODES: LA_OK, LA_E_GUID, LA_FAIL, LA_E_TIME, LA_E_BUFFER_SIZE
        */

        public static int GetAppVersion(StringBuilder appVersion, int length)
        {
#if LA_ANY_CPU
            return IntPtr.Size == 8 ? Native.GetAppVersion_x64(appVersion, length) : Native.GetAppVersion(appVersion, length);
#else 
            return Native.GetAppVersion(appVersion, length);
#endif
        }

        /*
            FUNCTION: GetProductKey()

            PURPOSE: Gets the stored product key which was used for activation.

            PARAMETERS:
            * productKey - pointer to a buffer that receives the value of the string
            * length - size of the buffer pointed to by the productKey parameter

            RETURN CODES: LA_OK, LA_E_PKEY, LA_E_GUID, LA_E_BUFFER_SIZE
        */

        public static int GetProductKey(StringBuilder productKey, int length)
        {
#if LA_ANY_CPU
            return IntPtr.Size == 8 ? Native.GetProductKey_x64(productKey, length) : Native.GetProductKey(productKey, length);
#else
            return Native.GetProductKey(productKey, length);
#endif
        }

        /*
            FUNCTION: GetProductKeyEmail()

            PURPOSE: Gets the email associated with product key used for activation.

            PARAMETERS:
            * productKey - pointer to a buffer that receives the value of the string
            * length - size of the buffer pointed to by the productKeyEmail parameter

            RETURN CODES: LA_OK, LA_E_GUID, LA_FAIL, LA_E_TIME, LA_E_BUFFER_SIZE
        */

        public static int GetProductKeyEmail(StringBuilder productKeyEmail, int length)
        {
#if LA_ANY_CPU
            return IntPtr.Size == 8 ? Native.GetProductKeyEmail_x64(productKeyEmail, length) : Native.GetProductKeyEmail(productKeyEmail, length);
#else
            return Native.GetProductKeyEmail(productKeyEmail, length);
#endif
        }

        /*
            FUNCTION: GetProductKeyExpiryDate()

            PURPOSE: Gets the product key expiry date timestamp.

            PARAMETERS:
            * expiryDate - pointer to the integer that receives the value

            RETURN CODES: LA_OK, LA_E_GUID, LA_FAIL, LA_E_TIME
        */

        public static int GetProductKeyExpiryDate(ref uint expiryDate)
        {
#if LA_ANY_CPU 
            return IntPtr.Size == 8 ? Native.GetProductKeyExpiryDate_x64(ref daysLeft) : Native.GetProductKeyExpiryDate(ref expiryDate);
#else 
            return Native.GetProductKeyExpiryDate(ref expiryDate);
#endif
        }

        /*
            FUNCTION: GetProductKeyCustomField()

            PURPOSE: Get the value of the custom field associated with the product key.

            PARAMETERS:
            * fieldId - id of the custom field whose value you want to get
            * fieldValue - pointer to a buffer that receives the value of the string
            * length - size of the buffer pointed to by the fieldValue parameter

            RETURN CODES: LA_OK, LA_E_GUID, LA_FAIL, LA_E_TIME, LA_E_CUSTOM_FIELD_ID,
            LA_E_BUFFER_SIZE
        */

        public static int GetProductKeyCustomField(string fieldId, StringBuilder fieldValue, int length)
        {
#if LA_ANY_CPU
            return IntPtr.Size == 8 ? Native.GetProductKeyCustomField_x64(fieldId, fieldValue, length) : Native.GetProductKeyCustomField(fieldId, fieldValue, length);
#else 
            return Native.GetProductKeyCustomField(fieldId, fieldValue, length);
#endif
        }

        /*
            FUNCTION: GetActivationExtraData()

            PURPOSE: Gets the value of the activation extra data.

            PARAMETERS:
            * extraData - pointer to a buffer that receives the value of the string
            * length - size of the buffer pointed to by the fieldValue parameter

            RETURN CODES: LA_OK, LA_E_GUID, LA_FAIL, LA_E_TIME, LA_E_BUFFER_SIZE
        */

        public static int GetActivationExtraData(StringBuilder extraData, int length)
        {
#if LA_ANY_CPU
            return IntPtr.Size == 8 ? Native.GetActivationExtraData_x64(extraData, length) : Native.GetActivationExtraData(extraData, length);
#else 
            return Native.GetActivationExtraData(extraData, length);
#endif
        }

        /*
            FUNCTION: GetTrialActivationExtraData()

            PURPOSE: Gets the value of the trial activation extra data.

            PARAMETERS:
            * extraData - pointer to a buffer that receives the value of the string
            * length - size of the buffer pointed to by the fieldValue parameter

            RETURN CODES: LA_OK, LA_E_GUID, LA_FAIL, LA_E_TIME, LA_E_BUFFER_SIZE
        */

        public static int GetTrialActivationExtraData(StringBuilder extraData, int length)
        {
#if LA_ANY_CPU
            return IntPtr.Size == 8 ? Native.GetTrialActivationExtraData_x64(extraData, length) : Native.GetTrialActivationExtraData(extraData, length);
#else 
            return Native.GetTrialActivationExtraData(extraData, length);
#endif
        }

        /*
            FUNCTION: GetTrialExpiryDate()

            PURPOSE: Gets the trial expiry date timestamp.

            PARAMETERS:
            * trialExpiryDate - pointer to the integer that receives the value

            RETURN CODES: LA_OK, LA_E_GUID, LA_FAIL, LA_E_TIME
        */

        public static int GetTrialExpiryDate(ref uint trialExpiryDate)
        {
#if LA_ANY_CPU 
            return IntPtr.Size == 8 ? Native.GetTrialExpiryDate_x64(ref trialExpiryDate) : Native.GetTrialExpiryDate(ref trialExpiryDate);
#else 
            return Native.GetTrialExpiryDate(ref trialExpiryDate); 
#endif
        }

        /*
            FUNCTION: GetLocalTrialExpiryDate()

            PURPOSE: Gets the trial expiry date timestamp.

            PARAMETERS:
            * trialExpiryDate - pointer to the integer that receives the value

            RETURN CODES: LA_OK, LA_E_GUID, LA_FAIL, LA_E_TIME
        */

        public static int GetLocalTrialExpiryDate(ref uint trialExpiryDate)
        {
#if LA_ANY_CPU 
            return IntPtr.Size == 8 ? Native.GetLocalTrialExpiryDate_x64(ref trialExpiryDate) : Native.GetLocalTrialExpiryDate(ref trialExpiryDate);
#else 
            return Native.GetLocalTrialExpiryDate(ref trialExpiryDate); 
#endif
        }

        /*
            FUNCTION: ActivateProduct()

            PURPOSE: Activates your application by contacting the Cryptlex servers. It
            validates the key and returns with encrypted and digitally signed token
            which it stores and uses to activate your application.

            This function should be executed at the time of registration, ideally on
            a button click.

            RETURN CODES: LA_OK, LA_EXPIRED, LA_REVOKED, LA_FAIL, LA_E_GUID, LA_E_PKEY,
            LA_E_INET, LA_E_VM, LA_E_TIME, LA_E_ACT_LIMIT, LA_E_SERVER, LA_E_CLIENT,
            LA_E_PKEY_TYPE, LA_E_COUNTRY, LA_E_IP
        */

        public static int ActivateProduct()
        {
#if LA_ANY_CPU
            return IntPtr.Size == 8 ? Native.ActivateProduct_x64() : Native.ActivateProduct();
#else
            return Native.ActivateProduct();
#endif

        }

        /*
            FUNCTION: ActivateProductOffline()

            PURPOSE: Activates your application using the offline activation response
            file.

            PARAMETERS:
            * filePath - path of the offline activation response file.

            RETURN CODES: LA_OK, LA_EXPIRED, LA_FAIL, LA_E_GUID, LA_E_PKEY, LA_E_OFILE
            LA_E_VM, LA_E_TIME, LA_E_FPATH, LA_E_OFILE_EXPIRED
        */

        public static int ActivateProductOffline(string filePath)
        {
#if LA_ANY_CPU
            return IntPtr.Size == 8 ? Native.ActivateProductOffline_x64(filePath) : Native.ActivateProductOffline(filePath);
#else 
            return Native.ActivateProductOffline(filePath); 
#endif
        }

        /*
            FUNCTION: GenerateOfflineActivationRequest()

            PURPOSE: Generates the offline activation request needed for generating
            offline activation response in the dashboard.

            PARAMETERS:
            * filePath - path of the file for the offline request.

            RETURN CODES: LA_OK, LA_FAIL, LA_E_GUID, LA_E_PKEY, LA_E_FILE_PERMISSION
        */

        public static int GenerateOfflineActivationRequest(string filePath)
        {
#if LA_ANY_CPU
            return IntPtr.Size == 8 ? Native.GenerateOfflineActivationRequest_x64(filePath) : Native.GenerateOfflineActivationRequest(filePath);
#else 
            return Native.GenerateOfflineActivationRequest(filePath);
#endif
        }

        /*
            FUNCTION: DeactivateProduct()

            PURPOSE: Deactivates the application and frees up the corresponding activation
            slot by contacting the Cryptlex servers.

            This function should be executed at the time of de-registration, ideally on
            a button click.

            RETURN CODES: LA_OK, LA_E_DEACT_LIMIT, LA_FAIL, LA_E_GUID, LA_E_TIME
            LA_E_PKEY, LA_E_INET, LA_E_SERVER, LA_E_CLIENT
        */

        public static int DeactivateProduct()
        {
#if LA_ANY_CPU
            return IntPtr.Size == 8 ? Native.DeactivateProduct_x64() : Native.DeactivateProduct();
#else
            return Native.DeactivateProduct(); 
#endif
        }

        /*
            FUNCTION: GenerateOfflineDeactivationRequest()

            PURPOSE: Generates the offline deactivation request needed for deactivation of
            the product key in the dashboard and deactivates the application.

            A valid offline deactivation file confirms that the application has been successfully
            deactivated on the user's machine.

            PARAMETERS:
            * filePath - path of the file for the offline request.

            RETURN CODES: LA_OK, LA_FAIL, LA_E_GUID, LA_E_PKEY, LA_E_FILE_PERMISSION,
            LA_E_TIME
        */

        public static int GenerateOfflineDeactivationRequest(string filePath)
        {
#if LA_ANY_CPU
            return IntPtr.Size == 8 ? Native.GenerateOfflineDeactivationRequest_x64(filePath) : Native.GenerateOfflineDeactivationRequest(filePath);
#else 
            return Native.GenerateOfflineDeactivationRequest(filePath); 
#endif
        }

        /*
            FUNCTION: IsProductGenuine()

            PURPOSE: It verifies whether your app is genuinely activated or not. The verification is
            done locally by verifying the cryptographic digital signature fetched at the time of
            activation.

            After verifying locally, it schedules a server check in a separate thread. After the
            first server sync it periodically does further syncs at a frequency set for the product
            key.

            In case server sync fails due to network error, and it continues to fail for fixed
            number of days (grace period), the function returns LA_GP_OVER instead of LA_OK.

            This function must be called on every start of your program to verify the activation
            of your app.

            RETURN CODES: LA_OK, LA_EXPIRED, LA_REVOKED, LA_GP_OVER, LA_FAIL, LA_E_GUID, LA_E_PKEY,
            LA_E_TIME

            NOTE: If application was activated offline using ActivateProductOffline() function, you
            may want to set grace period to 0 to ignore grace period.
        */

        public static int IsProductGenuine()
        {
#if LA_ANY_CPU
            return IntPtr.Size == 8 ? Native.IsProductGenuine_x64() : Native.IsProductGenuine();
#else 
            return Native.IsProductGenuine();
#endif
        }

        /*
            FUNCTION: IsProductActivated()

            PURPOSE: It verifies whether your app is genuinely activated or not. The verification is
            done locally by verifying the cryptographic digital signature fetched at the time of
            activation.

            This is just an auxiliary function which you may use in some specific cases, when you
            want to skip the server sync.

            RETURN CODES: LA_OK, LA_EXPIRED, LA_REVOKED, LA_GP_OVER, LA_FAIL, LA_E_GUID, LA_E_PKEY,
            LA_E_TIME

            NOTE: You may want to set grace period to 0 to ignore grace period.
        */

        public static int IsProductActivated()
        {
#if LA_ANY_CPU
            return IntPtr.Size == 8 ? Native.IsProductActivated_x64() : Native.IsProductActivated();
#else 
            return Native.IsProductActivated(); 
#endif
        }

        /*
            FUNCTION: ActivateTrial()

            PURPOSE: Starts the verified trial in your application by contacting the
            Cryptlex servers.

            This function should be executed when your application starts first time on
            the user's computer, ideally on a button click.

            RETURN CODES: LA_OK, LA_T_EXPIRED, LA_FAIL, LA_E_GUID, LA_E_INET,
            LA_E_VM, LA_E_TIME, LA_E_SERVER, LA_E_CLIENT, LA_E_COUNTRY, LA_E_IP
        */

        public static int ActivateTrial()
        {
#if LA_ANY_CPU 
            return IntPtr.Size == 8 ? Native.ActivateTrial_x64() : Native.ActivateTrial();
#else 
            return Native.ActivateTrial(); 
#endif
        }

        /*
            FUNCTION: IsTrialGenuine()

            PURPOSE: It verifies whether trial has started and is genuine or not. The
            verification is done locally by verifying the cryptographic digital signature
            fetched at the time of trial activation.

            This function must be called on every start of your program during the trial period.

            RETURN CODES: LA_OK, LA_T_EXPIRED, LA_FAIL, LA_E_TIME, LA_E_GUID

        */

        public static int IsTrialGenuine()
        {
#if LA_ANY_CPU 
            return IntPtr.Size == 8 ? Native.IsTrialGenuine_x64() : Native.IsTrialGenuine();
#else 
            return Native.IsTrialGenuine(); 
#endif
        }

        /*
            FUNCTION: ExtendTrial()

            PURPOSE: Extends the trial using the trial extension key generated in the dashboard
            for the product version.

            PARAMETERS:
            * trialExtensionKey - trial extension key generated for the product version

            RETURN CODES: LA_OK, LA_T_EXPIRED, LA_FAIL, LA_E_GUID, LA_E_INET,
            LA_E_VM, LA_E_TIME, LA_E_TEXT_KEY, LA_E_SERVER, LA_E_CLIENT,
            LA_E_TRIAL_NOT_EXPIRED

            NOTE: The function is only meant for verified trials.
        */

        public static int ExtendTrial(string trialExtensionKey)
        {
#if LA_ANY_CPU 
            return IntPtr.Size == 8 ? Native.ExtendTrial_x64(trialExtensionKey) : Native.ExtendTrial(trialExtensionKey);
#else 
            return Native.ExtendTrial(trialExtensionKey); 
#endif
        }

        /*
            FUNCTION: ActivateLocalTrial()

            PURPOSE: Starts the local(unverified) trial.

            This function should be executed when your application starts first time on
            the user's computer, ideally on a button click.

            PARAMETERS:
            * trialLength - trial length in days

            RETURN CODES: LA_OK, LA_LT_EXPIRED, LA_FAIL, LA_E_GUID, LA_E_TIME

            NOTE: The function is only meant for unverified trials.
        */

        public static int ActivateLocalTrial(uint trialLength)
        {
#if LA_ANY_CPU 
            return IntPtr.Size == 8 ? Native.ActivateLocalTrial_x64(trialLength) : Native.ActivateLocalTrial(trialLength);
#else 
            return Native.ActivateLocalTrial(trialLength); 
#endif
        }

        /*
            FUNCTION: IsLocalTrialGenuine()

            PURPOSE: It verifies whether trial has started and is genuine or not. The
            verification is done locally.

            This function must be called on every start of your program during the trial period.

            RETURN CODES: LA_OK, LA_LT_EXPIRED, LA_FAIL, LA_E_GUID, LA_E_TIME

            NOTE: The function is only meant for unverified trials.
        */

        public static int IsLocalTrialGenuine()
        {
#if LA_ANY_CPU 
            return IntPtr.Size == 8 ? Native.IsLocalTrialGenuine_x64() : Native.IsLocalTrialGenuine();
#else 
            return Native.IsLocalTrialGenuine(); 
#endif
        }

        /*
	        FUNCTION: ExtendLocalTrial()

	        PURPOSE: Extends the local trial

	        PARAMETERS:
	        * trialExtensionLength - number of days to extend the trial

	        RETURN CODES: LA_OK, LA_FAIL, LA_E_GUID, LA_E_TIME, LA_E_LOCAL_TRIAL_NOT_EXPIRED

	        NOTE: The function is only meant for unverified trials.
        */

        public static int ExtendLocalTrial(uint trialExtensionLength)
        {
#if LA_ANY_CPU 
            return IntPtr.Size == 8 ? Native.ExtendLocalTrial_x64(trialExtensionLength) : Native.ExtendLocalTrial(trialExtensionLength);
#else 
            return Native.ExtendLocalTrial(trialExtensionLength);
#endif
        }


        /*** Return Codes ***/

        public const int LA_OK = 0;

        public const int LA_FAIL = 1;

        /*
            CODE: LA_EXPIRED

            MESSAGE: The product key has expired or system time has been tampered
            with. Ensure your date and time settings are correct.
        */

        public const int LA_EXPIRED = 2;

        /*
            CODE: LA_REVOKED

            MESSAGE: The product key has been revoked.
        */

        public const int LA_REVOKED = 3;

        /*
            CODE: LA_GP_OVER

            MESSAGE: The grace period is over.
        */

        public const int LA_GP_OVER = 4;

        /*
            CODE: LA_T_EXPIRED

            MESSAGE: The trial has expired or system time has been tampered
            with. Ensure your date and time settings are correct.
        */

        public const int LA_T_EXPIRED = 5;

        /*
            CODE: LA_LT_EXPIRED

            MESSAGE: The local trial has expired or system time has been tampered
            with. Ensure your date and time settings are correct.
        */

        public const int LA_LT_EXPIRED = 6;

        /*
            CODE: LA_E_PFILE

            MESSAGE: Invalid or corrupted product file.
        */

        public const int LA_E_PFILE	= 7;

        /*
            CODE: LA_E_FPATH

            MESSAGE: Invalid product file path.
        */

        public const int LA_E_FPATH	= 8;

        /*
            CODE: LA_E_GUID

            MESSAGE: The version GUID doesn't match that of the product file.
        */

        public const int LA_E_GUID	= 9;

        /*
            CODE: LA_E_OFILE

            MESSAGE: Invalid offline activation response file.
        */

        public const int LA_E_OFILE	= 10;

        /*
            CODE: LA_E_PERMISSION

            MESSAGE: Insufficent system permissions. Occurs when LA_SYSTEM flag is used
            but application is not run with admin privileges.
        */

        public const int LA_E_PERMISSION = 11;

        /*
            CODE: LA_E_EDATA_LEN

            MESSAGE: Extra activation data length is more than 256 characters.
        */

        public const int LA_E_EDATA_LEN  = 12;

        /*
            CODE: LA_E_PKEY_TYPE

            MESSAGE: Invalid product key type.
        */

        public const int LA_E_PKEY_TYPE  = 13;

        /*
            CODE: LA_E_TIME

            MESSAGE: The system time has been tampered with. Ensure your date
            and time settings are correct.
        */

        public const int LA_E_TIME = 14;

        /*
            CODE: LA_E_VM

            MESSAGE: Application is being run inside a virtual machine / hypervisor,
            and activation has been disallowed in the VM.
            but
        */

        public const int LA_E_VM = 15;

        /*
            CODE: LA_E_WMIC

            MESSAGE: Fingerprint couldn't be generated because Windows Management 
            Instrumentation (WMI; service has been disabled. This error is specific
            to Windows only.
        */

        public const int LA_E_WMIC = 16;

        /*
            CODE: LA_E_TEXT_KEY

            MESSAGE: Invalid trial extension key.
        */

        public const int LA_E_TEXT_KEY = 17;

        /*
            CODE: LA_E_OFILE_EXPIRED

            MESSAGE: The offline activation response has expired.
        */

        public const int LA_E_OFILE_EXPIRED  = 18;

        /*
            CODE: LA_E_INET

            MESSAGE: Failed to connect to the server due to network error.
        */

        public const int LA_E_INET = 19;

        /*
            CODE: LA_E_PKEY

            MESSAGE: Invalid product key.
        */

        public const int LA_E_PKEY = 20;

        /*
            CODE: LA_E_BUFFER_SIZE

            MESSAGE: The buffer size was smaller than required.
        */

        public const int LA_E_BUFFER_SIZE  = 21;

        /*
            CODE: LA_E_CUSTOM_FIELD_ID

            MESSAGE: Invalid custom field id.
        */

        public const int LA_E_CUSTOM_FIELD_ID = 22;

        /*
            CODE: LA_E_NET_PROXY

            MESSAGE: Invalid network proxy.
        */

        public const int LA_E_NET_PROXY  = 23;

        /*
            CODE: LA_E_HOST_URL

            MESSAGE: Invalid Cryptlex host url.
        */

        public const int LA_E_HOST_URL = 24;

        /*
            CODE: LA_E_DEACT_LIMIT

            MESSAGE: Deactivation limit for key has reached
        */

        public const int LA_E_DEACT_LIMIT = 25;

        /*
            CODE: LA_E_ACT_LIMIT

            MESSAGE: Activation limit for key has reached
        */

        public const int LA_E_ACT_LIMIT = 26;

        /*
            CODE: LA_E_PDATA

            MESSAGE: Invalid product data
        */

        public const int LA_E_PDATA = 27;

        /*
            CODE: LA_E_TRIAL_NOT_EXPIRED

            MESSAGE: Trial has not expired.
        */

        public const int LA_E_TRIAL_NOT_EXPIRED = 28;

        /*
            CODE: LA_E_COUNTRY

            MESSAGE: Country is not allowed
        */

        public const int LA_E_COUNTRY = 29;

        /*
            CODE: LA_E_IP

            MESSAGE: IP address is not allowed
        */

        public const int LA_E_IP = 30;

        /*
            CODE: LA_E_FILE_PERMISSION

            MESSAGE: No permission to write to file
        */

        public const int LA_E_FILE_PERMISSION = 31;

        /*
            CODE: LA_E_LOCAL_TRIAL_NOT_EXPIRED

            MESSAGE: Trial has not expired.
        */

        public const int LA_E_LOCAL_TRIAL_NOT_EXPIRED = 32;

        /*
            CODE: LA_E_SERVER

            MESSAGE: Server error
        */

        public const int LA_E_SERVER = 33;

        /*
            CODE: LA_E_CLIENT

            MESSAGE: Client error
        */

        public const int LA_E_CLIENT = 34;



        static class Native
        {
            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetProductFile(string filePath);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetProductData(string productData);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetProductVersionGuid(string versionGuid, PermissionFlags flags);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetProductKey(string productKey);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetActivationExtraData(string extraData);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetTrialActivationExtraData(string extraData);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetNetworkProxy(string proxy);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetAppVersion(StringBuilder appVersion, int length);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetProductKey(StringBuilder productKey, int length);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetProductKeyEmail(StringBuilder productKeyEmail, int length);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetProductKeyExpiryDate(ref uint expiryDate);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetProductKeyCustomField(string fieldId, StringBuilder fieldValue, int length);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetActivationExtraData(StringBuilder extraData, int length);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetTrialActivationExtraData(StringBuilder extraData, int length);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetTrialExpiryDate(ref uint trialExpiryDate);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetLocalTrialExpiryDate(ref uint trialExpiryDate);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ActivateProduct();

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ActivateProductOffline(string filePath);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int GenerateOfflineActivationRequest(string filePath);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int DeactivateProduct();

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int GenerateOfflineDeactivationRequest(string filePath);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int IsProductGenuine();

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int IsProductActivated();

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ActivateTrial();

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int IsTrialGenuine();

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ExtendTrial(string trialExtensionKey);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ActivateLocalTrial(uint trialLength);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int IsLocalTrialGenuine();

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ExtendLocalTrial(uint trialExtensionLength);

#if LA_ANY_CPU

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "SetProductFile", CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetProductFile_x64(string filePath);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "SetProductData", CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetProductData_x64(string productData);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "SetProductVersionGuid", CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetProductVersionGuid_x64(string versionGuid, PermissionFlags flags);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "SetProductKey", CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetProductKey_x64(string productKey);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "SetActivationExtraData", CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetActivationExtraData_x64(string extraData);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "SetTrialActivationExtraData", CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetTrialActivationExtraData_x64(string extraData);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "SetNetworkProxy", CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetNetworkProxy_x64(string proxy);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "GetAppVersion", CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetAppVersion_x64(StringBuilder appVersion, int length);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "GetProductKey", CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetProductKey_x64(StringBuilder productKey, int length);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "GetProductKeyEmail", CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetProductKeyEmail_x64(StringBuilder productKeyEmail, int length);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "GetProductKeyExpiryDate", CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetProductKeyExpiryDate_x64(ref uint expiryDate);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "GetProductKeyCustomField", CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetProductKeyCustomField_x64(string fieldId, StringBuilder fieldValue, int length);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "GetActivationExtraData", CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetActivationExtraData_x64(StringBuilder extraData, int length);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "GetTrialActivationExtraData", CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetTrialActivationExtraData_x64(StringBuilder extraData, int length);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "GetTrialExpiryDate", CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetTrialExpiryDate_x64(ref uint trialExpiryDate);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "GetLocalTrialExpiryDate", CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetLocalTrialExpiryDate_x64(ref uint trialExpiryDate);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "ActivateProduct", CallingConvention = CallingConvention.Cdecl)]
            public static extern int ActivateProduct_x64();

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "ActivateProductOffline", CallingConvention = CallingConvention.Cdecl)]
            public static extern int ActivateProductOffline_x64(string filePath);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "GenerateOfflineActivationRequest", CallingConvention = CallingConvention.Cdecl)]
            public static extern int GenerateOfflineActivationRequest_x64(string filePath);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "DeactivateProduct", CallingConvention = CallingConvention.Cdecl)]
            public static extern int DeactivateProduct_x64();

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "GenerateOfflineDeactivationRequest", CallingConvention = CallingConvention.Cdecl)]
            public static extern int GenerateOfflineDeactivationRequest_x64(string filePath);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "IsProductGenuine", CallingConvention = CallingConvention.Cdecl)]
            public static extern int IsProductGenuine_x64();

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "IsProductActivated", CallingConvention = CallingConvention.Cdecl)]
            public static extern int IsProductActivated_x64();

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "ActivateTrial", CallingConvention = CallingConvention.Cdecl)]
            public static extern int ActivateTrial_x64();

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "IsTrialGenuine", CallingConvention = CallingConvention.Cdecl)]
            public static extern int IsTrialGenuine_x64();

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "ExtendTrial", CallingConvention = CallingConvention.Cdecl)]
            public static extern int ExtendTrial_x64(string trialExtensionKey);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "ActivateLocalTrial", CallingConvention = CallingConvention.Cdecl)]
            public static extern int ActivateLocalTrial_x64(uint trialLength);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "IsLocalTrialGenuine", CallingConvention = CallingConvention.Cdecl)]
            public static extern int IsLocalTrialGenuine_x64();

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "ExtendLocalTrial", CallingConvention = CallingConvention.Cdecl)]
            public static extern int ExtendLocalTrial_x64(uint trialExtensionLength);
            
#endif
        }
    }
}