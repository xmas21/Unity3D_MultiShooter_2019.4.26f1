using UnityEngine;

public abstract class scr_Item : MonoBehaviour
{
    [Header("武器資訊")]
    public scr_ItemInfo itemInfo;
    [Header("武器物件")]
    public GameObject itemGameObject;

    public abstract void Use();
}
