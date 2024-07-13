using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using GooglePlayGames;

public class FirebaseGoogleAuth : MonoBehaviour
{
    DatabaseReference databaseReference;
    private bool isFirebaseInitialized = false;

    private void Start()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();// �����÷��� �÷��� Ȱ��ȭ
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            // Firebase �ʱ�ȭ
            FirebaseApp app = FirebaseApp.DefaultInstance;
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            isFirebaseInitialized = true;
        });
        StartCoroutine(TryGoogleLogin());
    }

    /* ���� �α��� ��ư�� Ŭ���ϸ� ����Ǵ� �Լ� */
    public void OnClickGoogleLoginButton()
    {
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                Debug.Log(Social.localUser.userName + "�� ȯ���մϴ�");
                Debug.Log("����� ID�� " + Social.localUser.id + "�Դϴ�");

                // Firebase �ʱ�ȭ�� �Ϸ�� �Ŀ� �����͸� ����
                if (isFirebaseInitialized)
                {
                    SaveUserDataToFirebase();
                }
                else
                {
                    Debug.Log("Firebase �ʱ�ȭ�� ���� �Ϸ���� �ʾҽ��ϴ�.");
                }
            }
            else
            {
                Debug.Log("�α��ο� �����߽��ϴ�");
            }
        });
    }

    private void SaveUserDataToFirebase()
    {
        // Firebase�� ���� �����͸� �����ϴ� �޼���
        string userId = Social.localUser.id;
        string userName = Social.localUser.userName;

        User user = new User(userId, userName);
        string json = JsonUtility.ToJson(user);


        // users ��忡 ���� ������ ����
        databaseReference.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }

    private void LoadUserDataFromFirebase()
    {
        // Firebase���� ���� �����͸� �ҷ����� �޼���
        string userId = Social.localUser.id;

        databaseReference.Child("users").Child(userId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            // Firebase���� ������ �ٿ�ε�
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    // �����Ͱ� ������ ��� ó��
                    string json = snapshot.GetRawJsonValue();
                    User loadedUser = JsonUtility.FromJson<User>(json);

                    Debug.Log("�ҷ��� ���� �̸�: " + loadedUser.userName);
                }
                else
                {
                    Debug.Log("���� �����Ͱ� �������� �ʽ��ϴ�.");
                }
            }
            else
            {
                Debug.Log("������ �ҷ����� ����: " + task.Exception);
            }
        });
    }

    IEnumerator TryGoogleLogin()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        OnClickGoogleLoginButton();

    }
}
