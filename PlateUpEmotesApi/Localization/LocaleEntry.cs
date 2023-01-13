namespace PlateUpEmotesApi.Localization;

[Serializable]
public struct LocaleEntry
{
    public string Token;
    public string Text;

    public LocaleEntry(string token, string text)
    {
        Token = token;
        Text = text;
    }
}