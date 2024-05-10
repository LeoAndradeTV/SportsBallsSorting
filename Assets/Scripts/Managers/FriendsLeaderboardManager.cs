using Steamworks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FriendsLeaderboardManager : MonoBehaviour
{
    [SerializeField] private Transform playerCardPrefab;
    [SerializeField] private Transform contentArea;
    [SerializeField] private Button toggleLeaderboardButton;
    [SerializeField] private TMP_Text toggleLeaderboardButtonText;
    [SerializeField] private TMP_Text labelText;

    private CallResult<LeaderboardFindResult_t> leaderboardResult;
    private CallResult<LeaderboardScoresDownloaded_t> leaderboardScoresDownloadedResult;
    SteamLeaderboardEntries_t leaderboardScores;
    SteamLeaderboard_t leaderboardHandle;
    LeaderboardEntry_t leaderboardEntry;
    LeaderboardScoresDownloaded_t leaderboardScoresDownloaded;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (contentArea.childCount > 0) return;

        if (SteamManager.Initialized)
        {
            var leaderboard = SteamUserStats.FindLeaderboard("Ball Blitz Highscore");

            leaderboardResult = CallResult<LeaderboardFindResult_t>.Create(OnFindLeaderboard);

            leaderboardResult.Set(leaderboard);
        }
    }

    private void OnFindLeaderboard(LeaderboardFindResult_t param, bool bIOFailure)
    {
        if (param.m_bLeaderboardFound != 1 || bIOFailure)
        {

        }
        else
        {
            leaderboardHandle = param.m_hSteamLeaderboard;
            leaderboardScoresDownloadedResult = CallResult<LeaderboardScoresDownloaded_t>.Create(OnDownloadEntries);

            var handle = SteamUserStats.DownloadLeaderboardEntries(leaderboardHandle, ELeaderboardDataRequest.k_ELeaderboardDataRequestFriends, 0, 99);
            leaderboardScoresDownloadedResult.Set(handle);
        }

    }

    private void OnDownloadEntries(LeaderboardScoresDownloaded_t param, bool bIOFailure)
    {
        if (bIOFailure)
        {
        }
        else
        {
            leaderboardScoresDownloaded = param;
            leaderboardScores = param.m_hSteamLeaderboardEntries;
            ShowGlobalLeaderboard();
        }
    }

    private void ShowGlobalLeaderboard()
    {
        if (!SteamManager.Initialized) { return; }

        for (int i = 0; i < leaderboardScoresDownloaded.m_cEntryCount; i++)
        {
            if (SteamUserStats.GetDownloadedLeaderboardEntry(leaderboardScores, i, out leaderboardEntry, new int[0], 0))
            {
                var username = SteamFriends.GetFriendPersonaName(leaderboardEntry.m_steamIDUser);
                var card = Instantiate(playerCardPrefab, contentArea);

                var playerCard = card.GetComponent<PlayerCard>();

                if (leaderboardEntry.m_steamIDUser == SteamUser.GetSteamID())
                {
                    playerCard.SetMyUsernameColor();
                }

                playerCard.InitializeInformation(leaderboardEntry.m_nGlobalRank.ToString(), username, leaderboardEntry.m_nScore.ToString());
            }
            else
            {
                Debug.Log("this failed");
            }
        }


    }
}
