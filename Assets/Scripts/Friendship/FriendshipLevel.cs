using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// This is the friendship level meter in the game.
public class FriendshipLevel : MonoBehaviour
{
    [SerializeField]
    private FriendData friend;
    [SerializeField]
    private RectTransform rt;
    [SerializeField]
    private Image image;

    private void Update()
    {
        if (friend.Friend > 300)
        {
            rt.anchoredPosition = new Vector2(-150 + (300 / 2), 0);
            rt.sizeDelta = new Vector2(300, rt.sizeDelta.y);
        }
        else
        {
            rt.anchoredPosition = new Vector2(-150 + (friend.Friend / 2), 0);
            rt.sizeDelta = new Vector2(friend.Friend, rt.sizeDelta.y);
        }
    }
}
