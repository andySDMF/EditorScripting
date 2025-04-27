using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BrandLab360.Editor
{
    public class BrandlabHubSettings
    {
        [System.Serializable]
        public class BLHubProjectWrapper
        {
            public List<BLHubProject> projects;
        }

        [System.Serializable]
        public class BLHubProject
        {
            public string id = "";
            public string name = "";
            public string app_type = "";
            public string sdk_version = "";
            public string furioos_id = "";
            public string project_id = "";
            public string model_id = "";
            public string logo_url = "";
            public string logo_width = "400px";
            public string bg_color = "#000000";
            public bool dark_mode = true;
            public string password = "";
            public string password_confirmation = "";
        }

        public static string ApiCredentials
        {
            get
            {
                return EditorPrefs.GetString("BRANDLABAPI-CREDENTIALS", "");
            }
            set
            {
                EditorPrefs.SetString("BRANDLABAPI-CREDENTIALS", value);
            }
        }

        public static string ApiCredentialPassword
        {
            get
            {
                return EditorPrefs.GetString("BRANDLABAPI-CREDENTIALSPASSWORD", "");
            }
            set
            {
                EditorPrefs.SetString("BRANDLABAPI-CREDENTIALSPASSWORD", value);
            }
        }

        public static string ApiAccessToken
        {
            get
            {
                return EditorPrefs.GetString("BRANDLABAPI-TOKEN", "");
            }
            set
            {
                EditorPrefs.SetString("BRANDLABAPI-TOKEN", value);
            }
        }

        public static string ApiProjectID
        {
            get
            {
                return EditorPrefs.GetString("BRANDLABAPI-PROJECTID", "");
            }
            set
            {
                EditorPrefs.SetString("BRANDLABAPI-PROJECTID", value);
            }
        }

        public static BLHubProject ApiProject
        {
            get
            {
                BLHubProject temp = JsonUtility.FromJson<BLHubProject>(EditorPrefs.GetString("BRANDLABAPI-PROJECTJSON", ""));

                return temp;
            }
            set
            {
                EditorPrefs.SetString("BRANDLABAPI-PROJECTJSON", JsonUtility.ToJson(value));
            }
        }

        public static string IsAdmin
        {
            get
            {
                return EditorPrefs.GetString("BRANDLABAPI-ISADMIN", "");
            }
            set
            {
                EditorPrefs.SetString("BRANDLABAPI-ISADMIN", value);
            }
        }

        public static string IsLicienced
        {
            get
            {
                return EditorPrefs.GetString("BRANDLABAPI-ISLICIENCED", "");
            }
            set
            {
                EditorPrefs.SetString("BRANDLABAPI-ISLICIENCED", value);
            }
        }
    }
}
