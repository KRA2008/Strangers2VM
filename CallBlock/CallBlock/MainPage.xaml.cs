namespace CallBlock
{
    public partial class MainPage
    {
        public const string IS_ON = "isOn";

        public MainPage()
        {
            InitializeComponent();
            if (!Preferences.ContainsKey(IS_ON))
            {
                Preferences.Set(IS_ON,true);
            }
            Switch.IsToggled = Preferences.Get(IS_ON, false);
        }

        private void Switch_OnToggled(object? sender, ToggledEventArgs e)
        {
            Preferences.Set(IS_ON, e.Value);
        }
    }
}
