using Firebase.Auth;
using UnityEngine;

public partial class Firebase_Manager
{
    public void GuestLogin() // 게스트 로그인
    {
        if(auth.CurrentUser != null)
        {
            Debug.Log("이미 로그인 상태입니다. : " + auth.CurrentUser.UserId);
            return;
        }

        auth.SignInAnonymouslyAsync().ContinueWith(task => 
        {
            if(task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("게스트 로그인 실패: " + task.Exception);
                return;
            }
            
            FirebaseUser user = task.Result.User;
            // Unique ID
            Debug.Log("게스트 로그인 성공! 사용자 ID : " + user.UserId);

        });
    }
    
}
