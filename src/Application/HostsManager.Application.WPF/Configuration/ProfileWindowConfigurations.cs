namespace HostsManager.Application.WPF.Configuration
{
    public class ProfileWindowConfigurations
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public  string BackgroundColor { get; set; }
        public HeaderConfiguration HeaderConfiguration { get; set; }
        public TextboxConfiguration TextboxConfiguration { get; set; }
        public ButtonConfiguration SaveButtonConfiguration { get; set; }
        public ButtonConfiguration CancelButtonConfiguration { get; set; }
    }
}