using Android.App;
using Android.Provider;
using Android.Telecom;
using System.Diagnostics;
using Android.Database;

namespace CallBlock;

[Service(Exported = true, Permission = "android.permission.BIND_SCREENING_SERVICE")]
[IntentFilter(["android.telecom.CallScreeningService"])]
public class ScreeningService : CallScreeningService
{
    public override void OnScreenCall(Call.Details callDetails)
    {
        var handle = callDetails.GetHandle();
        ICursor contact = null;

        if (handle != null)
        {
            var number = handle.SchemeSpecificPart;
            Debug.WriteLine("### call received from " + number);
            var uri = Android.Net.Uri.WithAppendedPath(ContactsContract.PhoneLookup.ContentFilterUri,
                Android.Net.Uri.Encode(callDetails.GetHandle().SchemeSpecificPart));
            var name = "?";

            Debug.WriteLine("### uri for search: " + uri);

            contact = ContentResolver.Query(uri,
                [BaseColumns.Id, ContactsContract.PhoneLookup.InterfaceConsts.DisplayName], null, null, null);

            Debug.WriteLine("### query succeeded");
        }

        var response = new CallResponse.Builder();
        if (handle == null || 
            contact == null || 
            contact.Count == 0)
        {
            Debug.WriteLine("### no contacts found!");
            response.SetDisallowCall(true);
            response.SetRejectCall(true);
            response.SetSkipNotification(true);
        }
        else
        {
            Debug.WriteLine("### contacts found: " + contact.Count);
        }

        contact?.Close();

        RespondToCall(callDetails, response.Build());
    }
}