public class TwitchSensitiveInformation
{
	private const string PREFS_OAUTH_KEY = "TwitchOauthKey";

	// TODO: Fix this
	public static string GetOauth()
	{
#if UNITY_EDITOR
		return UnityEditor.EditorPrefs.GetString(PREFS_OAUTH_KEY, null);
#else
		return null;
#endif
	}

	// TODO: Fix this
	public static void SetOauth(string oauth)
	{
#if UNITY_EDITOR
		UnityEditor.EditorPrefs.SetString(PREFS_OAUTH_KEY, oauth);
#else

#endif
	}
}