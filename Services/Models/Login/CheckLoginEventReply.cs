namespace Models.Login
{
    public class CheckLoginEventReply : EventModel
    {
        public enum State
        {
            LOGGED,
            UNLOGGED
        };

        public string Login { get; set; }
    }
}
