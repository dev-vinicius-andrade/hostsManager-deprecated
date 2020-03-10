namespace HostsManager.Services.Entities
{
    public class Hosts
    {
        public Hosts()
        {
            Active = false;
        }
        public string Ip { get; set; }
        public string Host { get; set; }
        public bool Active { get; set; }
    }
}
