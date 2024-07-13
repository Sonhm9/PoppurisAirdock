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
        PlayGamesPlatform.Activate();// 구글플레이 플랫폼 활성화
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            // Firebase 초기화
            FirebaseApp app = FirebaseApp.DefaultInstance;
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            isFirebaseInitialized = true;
        });
        StartCoroutine(TryGoogleLogin());
    }

    /* 구글 로그인 버튼을 클릭하면 실행되는 함수 */
    public void OnClickGoogleLoginButton()
    {
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                Debug.Log(Social.localUser.userName + "님 환영합니다");
                Debug.Log("당신의 ID는 " + Social.localUser.id + "입니다");

                // Firebase 초기화가 완료된 후에 데이터를 저장
                if (isFirebaseInitialized)
                {
                    SaveUserDataToFirebase();
                }
                else
                {
                    Debug.Log("Firebase 초기화가 아직 완료되지 않았습니다.");
                }
            }
            else
            {
                Debug.Log("로그인에 실패했습니다");
            }
        });
    }

    private void SaveUserDataToFirebase()
    {
        // Firebase에 유저 데이터를 저장하는 메서드
        string userId = Social.localUser.id;
        string userName = Social.localUser.userName;

        User user = new User(userId, userName);
        string json = JsonUtility.ToJson(user);


        // users 노드에 유저 데이터 저장
        databaseReference.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }

    private void LoadUserDataFromFirebase()
    {
        // Firebase에서 유저 데이터를 불러오는 메서드
        string userId = Social.localUser.id;

        databaseReference.Child("users").Child(userId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            // Firebase에서 데이터 다운로드
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    // 데이터가 존재할 경우 처리
                    string json = snapshot.GetRawJsonValue();
                    User loadedUser = JsonUtility.FromJson<User>(json);

                    Debug.Log("불러온 유저 이름: " + loadedUser.userName);
                }
                else
                {
                    Debug.Log("유저 데이터가 존재하지 않습니다.");
                }
            }
            else
            {
                Debug.Log("데이터 불러오기 실패: " + task.Exception);
            }
        });
    }

    IEnumerator TryGoogleLogin()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        OnClickGoogleLoginButton();

    }
}
