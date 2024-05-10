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
        PlayGamesPlatform.Activate();//�����÷��� �÷��� Ȱ��ȭ
        //���� �Լ��� �����ϸ� Social.Active= PlayGamesPlatform.Instance�� �ȴ�
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

    /* Firebase�� ���� �����͸� �����ϴ� �Լ� */
    private void SaveUserDataToFirebase()
    {
        string userId = Social.localUser.id;
        string userName = Social.localUser.userName;

        // ���÷� 'users' ��忡 ���� ������ ����
        User user = new User(userId, userName);
        string json = JsonUtility.ToJson(user);

        // Firebase�� ������ ���ε�
        databaseReference.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }

    /* Firebase���� ���� �����͸� �ҷ����� �Լ� */
    private void LoadUserDataFromFirebase()
    {
        string userId = Social.localUser.id;

        // Firebase���� ������ �ٿ�ε�
        databaseReference.Child("users").Child(userId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    // �����Ͱ� ������ ��� ó��
                    string json = snapshot.GetRawJsonValue();
                    User loadedUser = JsonUtility.FromJson<User>(json);

                    Debug.Log("�ҷ��� ���� �̸�: " + loadedUser.userName);
                    // �߰������� �ʿ��� ó�� ����
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
