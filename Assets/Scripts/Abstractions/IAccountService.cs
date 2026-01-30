namespace BoardAdventures.Abstractions
{
    public interface IAccountService
    {
        public string Nickname { get; set; }
        void CheckAuth();
    }
}