using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System;
using System.Text;

namespace BrandLab360.Editor
{
    public class BrandlabHubAPIHandler : EditorWindow
    {
        public enum RequestType
        {
            Get,
            Post,
            Put,
            Delete
        }

        //private const string ProductionHost = "https://hub.brandlab360.co.uk";
        private const string DevelopmentHost = "https://dev.brandlab360.co.uk";
        private const string ApiPath = "/api/v1";
        private const string AccessEndpoint = "/auth/login";

        public static UnityWebRequest CreateWebRequest(String url, RequestType requestType = RequestType.Get, string form = "")
        {
            UnityWebRequest www = null;

            switch (requestType)
            {
                case RequestType.Get:
                    www = UnityWebRequest.Get(url);
                    break;
                case RequestType.Post:
                    www = UnityWebRequest.PostWwwForm(url, form);
                    break;
                case RequestType.Put:
                    www = UnityWebRequest.PostWwwForm(url, form);
                    www.method = "PUT";
                    break;
                case RequestType.Delete:
                    www = UnityWebRequest.Delete(url);
                    break;
            }

            return www;
        }

        public static async Task<bool> GetValidToken(string credentials, string password)
        {
            User user = new User();
            user.email = credentials;
            user.password = password;

            string jsonEntry = JsonUtility.ToJson(user);

            var jsonBytes = Encoding.UTF8.GetBytes(jsonEntry);
            string uri = DevelopmentHost + ApiPath + AccessEndpoint;

            using var www = CreateWebRequest(uri, RequestType.Post, UnityWebRequest.kHttpVerbPOST);

            www.SetRequestHeader("Content-Type", "application/json");
            www.uploadHandler = new UploadHandlerRaw(jsonBytes);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.timeout = 5;

            var operation = www.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            var result = (www.result == UnityWebRequest.Result.Success);

            if (!result)
            {
                Debug.LogWarning(www.downloadHandler.text);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);

                var response = JsonUtility.FromJson<AccessResponse>(www.downloadHandler.text);
                BrandlabHubSettings.ApiAccessToken = response.token;
            }

            return result;
        }

        public static async Task<string> GetAllProjects()
        {
            string uri = DevelopmentHost + ApiPath + "/streaming";

            using var www = CreateWebRequest(uri, RequestType.Get, "");

            www.SetRequestHeader("Content-Type", "application/json");
            var bearer = "Bearer " + BrandlabHubSettings.ApiAccessToken;
            www.SetRequestHeader("Authorization", bearer);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.timeout = 5;

            var operation = www.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            var result = (www.result == UnityWebRequest.Result.Success);

            if (!result)
            {
                Debug.LogWarning(www.downloadHandler.text);
                return "";
            }
            else
            {
                return www.downloadHandler.text;
            }
        }

        public static async Task<bool> UpdateApplication(BrandlabHubSettings.BLHubProject project)
        {
            string uri = DevelopmentHost + ApiPath + "/streaming/" + project.id;
            var jsonEntry = JsonUtility.ToJson(project);
            var jsonBytes = Encoding.UTF8.GetBytes(jsonEntry);

            using var www = CreateWebRequest(uri, RequestType.Put, "");

            www.SetRequestHeader("Content-Type", "application/json");
            var bearer = "Bearer " + BrandlabHubSettings.ApiAccessToken;
            www.SetRequestHeader("Authorization", bearer);
            www.uploadHandler = new UploadHandlerRaw(jsonBytes);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.timeout = 5;

            var operation = www.SendWebRequest();
            while (!operation.isDone)
                await Task.Yield();

            if (www.result != UnityWebRequest.Result.Success)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static async Task<bool> CreateApplication(BrandlabHubSettings.BLHubProject project)
        {
            string uri = DevelopmentHost + ApiPath + "/streaming/";
            var jsonEntry = JsonUtility.ToJson(project);
            var jsonBytes = Encoding.UTF8.GetBytes(jsonEntry);

            using var www = CreateWebRequest(uri, RequestType.Post, "");

            www.SetRequestHeader("Content-Type", "application/json");
            var bearer = "Bearer " + BrandlabHubSettings.ApiAccessToken;
            www.SetRequestHeader("Authorization", bearer);
            www.uploadHandler = new UploadHandlerRaw(jsonBytes);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.timeout = 5;

            var operation = www.SendWebRequest();
            while (!operation.isDone)
                await Task.Yield();

            if (www.result != UnityWebRequest.Result.Success)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
