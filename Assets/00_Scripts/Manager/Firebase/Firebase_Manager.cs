using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;

public partial class Firebase_Manager
{
    private FirebaseAuth auth;
    private FirebaseUser currentUser;
    public DatabaseReference reference;
    public void Init()
    {
        // 파이어베이스 초기화
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => 
        {
            if(task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                currentUser = auth.CurrentUser;
                reference = FirebaseDatabase.DefaultInstance.RootReference;

                GuestLogin();
                Debug.Log("Firebase 초기화 성공!");
            }
            else
            {
                Debug.LogError("Firebase 초기화 실패: " + task.Exception.ToString());
            }
        });
    }

    
}
