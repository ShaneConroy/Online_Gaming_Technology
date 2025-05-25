using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class GameAnalytics : MonoBehaviour
{
    public static IEnumerator PostMethod(string jsonData)
    {

        string url = "https://compucore.itcarlow.ie/Recycling_Rats_Analytics/upload_data";
        //string url = "http://localhost:5000/upload_data";
        using (UnityWebRequest request = UnityWebRequest.Put(url, jsonData))
        {
            request.method = UnityWebRequest.kHttpVerbPOST;
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");

            yield return request.SendWebRequest();

            if (!request.isNetworkError && request.responseCode == (int)HttpStatusCode.OK)
                Debug.Log("Data successfully sent to the server");
            else
                Debug.Log("Error sending data to the server: Error " + request.responseCode);
        }
    }
}

[System.Serializable]
public class GameState
{
    public int time_alive = 0;
    public string player = "Jonsey";
    public int player_wins = 0;
    public int enemy_wins = 0;
    public int number_of_player_parts = 0;
    public int number_of_enemy_parts = 0;
    public int player_parts_lost = 0;
    public int enemy_parts_lost = 0;
    public int current_round = 0;
    public string device_id = SystemInfo.deviceUniqueIdentifier;
    public string key = "MonterEnergy";
    public int AB_test = 0;
}
public class TestData
{
    public string name = "Jonsey";
    public string key = "MonterEnergy";
}