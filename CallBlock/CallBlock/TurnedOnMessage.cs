using CommunityToolkit.Mvvm.Messaging.Messages;

namespace CallBlock;

public class TurnedOnMessage : ValueChangedMessage<bool>
{
    public TurnedOnMessage(bool value) : base(value) {}
}