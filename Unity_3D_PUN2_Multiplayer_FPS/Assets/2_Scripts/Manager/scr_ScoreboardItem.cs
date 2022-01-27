using Photon.Realtime;
using TMPro;
using UnityEngine;

public class scr_ScoreboardItem : MonoBehaviour
{
    public TMP_Text usernameText;
    public TMP_Text killText;
    public TMP_Text deathText;

    public void Initialize(Player player)
    {
        usernameText.text = player.NickName;
    }

}
