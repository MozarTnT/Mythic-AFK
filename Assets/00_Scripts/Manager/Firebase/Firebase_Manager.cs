using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;


public partial class Firebase_Manager // 파이어베이스 관리자 클래스 (기능 분리를 위해 partial 클래스로 분리)
{
    // 파이어베이스 인증 객체
    private FirebaseAuth auth;
    // 현재 로그인한 사용자 객체
    private FirebaseUser currentUser;
    // 파이어베이스 데이터베이스 참조 객체
    public DatabaseReference reference;

    public void Init()
    {
        // Firebase SDK의 모든 필수 구성요소가 있는지 확인하고 없으면 수정
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => 
        {
            if(task.Result == DependencyStatus.Available)
            {
                // 파이어베이스 인증 객체 초기화
                auth = FirebaseAuth.DefaultInstance;
                // 현재 로그인한 사용자 객체 초기화
                currentUser = auth.CurrentUser;
                // 파이어베이스 데이터베이스 참조 객체 초기화
                reference = FirebaseDatabase.DefaultInstance.RootReference;

                GuestLogin();
                Debug.Log("Firebase 초기화 성공!");
            }
            else
            {
                Debug.Log("Firebase 초기화 실패: " + task.Exception.ToString());
            }
        });
    }
}
