using System.Collections;
using System.Collections.Generic;
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
        //json 형식으로 데이터 저장

        User user = new User();
        user.userName = currentUser.UserId;
        user.Stage = Base_Manager.Data.Stage;

        string json = JsonUtility.ToJson(user);

        reference.Child("USER").Child(user.userName).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
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
        
    }
   
}
