using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using Newtonsoft.Json;

public class User
{
    public string userName;
    public int Stage;
}


public partial class Firebase_Manager
{
    // Dictionary -> json 파싱 위해서는 Newtonsoft.Json 필요
    // JsonConvert.SerializeObject = Dictionary -> json 파싱

    public void WriteData()
    {
        #region DEFAULT_DATA

        Data data = new Data();

        if(Data_Manager.m_Data != null)
        {
            data = Data_Manager.m_Data;
        }

        string Default_json = JsonUtility.ToJson(data);

        reference.Child("USER").Child(currentUser.UserId).Child("DATA").SetRawJsonValueAsync(Default_json).ContinueWithOnMainThread(task =>
        {
            if(!task.IsCompleted)
            {
                Debug.LogError("데이터 쓰기 실패 : " + task.Exception.ToString());
            }

        });
        #endregion

        #region CHARACTER_DATA

        string Character_json = JsonConvert.SerializeObject(Base_Manager.Data.Character_Holder);
        reference.Child("USER").Child(currentUser.UserId).Child("CHARACTER").SetRawJsonValueAsync(Character_json).ContinueWithOnMainThread(task =>
        {
            if(!task.IsCompleted)
            {
                Debug.LogError("캐릭터 데이터 쓰기 실패 : " + task.Exception.ToString());
            }
        });

        #endregion

    }

    public void ReadData()
    {
        #region DEFAULT_DATA

        reference.Child("USER").Child(currentUser.UserId).Child("DATA").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if(task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                var default_data = JsonUtility.FromJson<Data>(snapshot.GetRawJsonValue());

                Data data = new Data();
                if(default_data != null)
                {
                    data = default_data;
                }

                Data_Manager.m_Data = data;
                LoadingScene.instance.LoadingMain();
            }
            else
            {
                Debug.LogError("데이터 읽기 실패 : " + task.Exception.ToString());
            }

        });
        #endregion

        #region CHARACTER_DATA

        reference.Child("USER").Child(currentUser.UserId).Child("CHARACTER").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if(task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                var data = JsonConvert.DeserializeObject<Dictionary<string, Holder>>(snapshot.GetRawJsonValue());

                Base_Manager.Data.Character_Holder = data;
                Base_Manager.Data.Init();
            }
            else
            {
                Debug.LogError("데이터 읽기 실패 : " + task.Exception.ToString());
            }

        });
        #endregion
    }
}
