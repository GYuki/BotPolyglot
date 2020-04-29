namespace Session.API.Model
{
    public enum AuthType
    {
        Telegram,
        VK
    }

    public enum State
    {
        Idle,
        LanguageChoose,
        ModeChoose,
        ArcadeAction,
        TutorialAction
    }
}