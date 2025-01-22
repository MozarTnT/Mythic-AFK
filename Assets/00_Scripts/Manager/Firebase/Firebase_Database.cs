using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class User
{
    public string userName;
    public int Stage;
}

public partial class Firebase_Manager
{
    public void WriteData()
    {
        User user = new User();
        user.userName = currentUser.UserId;
        user.Stage = Base_Manager.Data.Stage;

        string json = JsonUtility.ToJson(user);

        reference.Child("USER").Child(currentUser.UserId).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
        {
            if(task.IsCompleted)
            {
                Debug.Log("데이터 쓰기 성공");
            }
            else
            {
                Debug.LogError("데이터 쓰기 실패 : " + task.Exception.ToString());
            }
        });
    }

    public void ReadData()
    {
        reference.Child("USER").Child(currentUser.UserId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if(task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                User user = JsonUtility.FromJson<User>(snapshot.GetRawJsonValue());
                Debug.Log("사용자 이름 : " + user.userName);
                Debug.Log("스테이지 : " + user.Stage);
            }
            else
            {
                Debug.LogError("데이터 읽기 실패 : " + task.Exception.ToString());
            }

        });
    }
   
}
