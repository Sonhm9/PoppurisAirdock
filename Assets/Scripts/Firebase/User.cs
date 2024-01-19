using UnityEngine;

public class User : MonoBehaviour
{
    public string userId;
    public string userName;

    public User(string id, string name)
    {
        userId = id;
        userName = name;
    }
}
