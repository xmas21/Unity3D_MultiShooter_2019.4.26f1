using UnityEngine;

public class scr_Menu : MonoBehaviour
{
    public string menuName;

   // [HideInInspector]
    public bool isOpen;

    /// <summary>
    /// 顯示
    /// </summary>
    public void Open()
    {
        isOpen = true;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 關閉
    /// </summary>
    public void Close()
    {
        isOpen = false;
        gameObject.SetActive(false);
    }

}
