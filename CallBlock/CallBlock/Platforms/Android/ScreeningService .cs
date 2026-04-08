using Android.App;
using Android.Provider;
using Android.Telecom;
using Microsoft.Maui.ApplicationModel.Communication;
using System.Diagnostics;
using System.Linq;
using Android.Net;

namespace CallBlock;

[Service(Exported = true, Permission = "android.permission.BIND_SCREENING_SERVICE")]
[IntentFilter(["android.telecom.CallScreeningService"])]
public class ScreeningService : CallScreeningService
{
    public override void OnScreenCall(Call.Details callDetails)
    {
        var handle = callDetails.GetHandle();
        if (handle == null) return;
        var number = handle.SchemeSpecificPart;
        Debug.WriteLine("### call received from " + number);
        var uri = Android.Net.Uri.WithAppendedPath(ContactsContract.PhoneLookup.ContentFilterUri,
            Android.Net.Uri.Encode(callDetails.GetHandle().SchemeSpecificPart));
        var name = "?";

        Debug.WriteLine("### uri for search: " + uri);

        var contact = ContentResolver.Query(uri,
            [BaseColumns.Id, ContactsContract.PhoneLookup.InterfaceConsts.DisplayName], null, null, null);

        Debug.WriteLine("### query succeeded");

        var response = new CallResponse.Builder();
        if (contact == null || contact.Count == 0)
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