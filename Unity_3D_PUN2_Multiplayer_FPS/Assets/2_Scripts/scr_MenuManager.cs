using UnityEngine;

public class scr_MenuManager : MonoBehaviour
{
    public static scr_MenuManager menuManager;

    [SerializeField]
    private scr_Menu[] menus;

    private void Awake()
    {
        menuManager = this;
    }

    /// <summary>
    /// 開啟頁面
    /// </summary>
    /// <param name="menuName">有掛程式的選單</param>
    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].name == menuName)
            {
                OpenMenu(menus[i]);
            }
            else if (menus[i].isOpen)
            {
                CloseMenu(menus[i]);
            }
        }
    }

    /// <summary>
    /// 開啟頁面
    /// </summary>
    /// <param name="menu">有掛程式的選單</param>
    public void OpenMenu(scr_Menu menu)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].isOpen)
            {
                CloseMenu(menus[i]);
            }
        }
        menu.Open();
    }

    /// <summary>
    /// 關閉頁面
    /// </summary>
    /// <param name="menu">有掛程式的選單</param>
    public void CloseMenu(scr_Menu menu)
    {
        menu.Close();
    }
}
