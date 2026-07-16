using CommunityToolkit.Mvvm.Messaging;

namespace CallBlock
{
    public partial class MainPage
    {
        public const string IS_ON_KEY = "isOn";

        public MainPage()
        {
            InitializeComponent();
            if (!Preferences.ContainsKey(IS_ON_KEY))
            {
                Preferences.Set(IS_ON_KEY,false);
            }
            Switch.IsToggled = Preferences.Get(IS_ON_KEY, false);
        }

        private void Switch_OnToggled(object? sender, ToggledEventArgs e)
        {
            if (e.Value)
            {
                WeakReferenceMessenger.Default.Send(new TurnedOnMessage(true));
            }
            Preferences.Set(IS_ON_KEY, e.Value);
        }
    }
}
