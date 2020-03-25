namespace HostsManager.Application.WPF.Configuration
{
    public class ProfileWindowConfigurations
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public  string BackgroundColor { get; set; }
        public LabelConfiguration HeaderProfileNameLabelConfiguration { get; set; }
        public TextboxConfiguration HeaderProfileNameTextBoxConfiguration { get; set; }
        public LabelConfiguration ProfileEditorLabelConfiguration { get; set; }
        public TextboxConfiguration ProfileEditorTextBoxConfiguration { get; set; }
        public ButtonConfiguration SaveButtonConfiguration { get; set; }
        public ButtonConfiguration CancelButtonConfiguration { get; set; }
        public ButtonConfiguration DeleteButtonConfiguration { get; set; }
    }
}