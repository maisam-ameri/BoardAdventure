namespace BoardAdventures.Authentication
{
    public interface IAccountService
    {
        public string Nickname { get; set; }
        void CheckAuth();
    }
}