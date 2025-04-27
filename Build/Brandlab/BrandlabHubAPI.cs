using Defective.JSON;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BrandLab360.Editor
{
    public class BrandlabHubAPI
    {
        public string hubCredentials = "";
        public string hubCredentialPassword = "";
        public BrandlabHubSettings.BLHubProject Project = new BrandlabHubSettings.BLHubProject();

        public bool editingID = false;
        public bool editingInfo = false;
        public bool projectFound = false;

        public BrandlabHubSettings.BLHubProjectWrapper AllProjects = new BrandlabHubSettings.BLHubProjectWrapper();

        public string previousProjectName = "";

        public void GetCredentials()
        {
            hubCredentials = BrandlabHubSettings.ApiCredentials;
            hubCredentialPassword = BrandlabHubSettings.ApiCredentialPassword;
        }

        public void SetCredentials(string str, string pass)
        {
            if (str != hubCredentials)
            {
                BrandlabHubSettings.IsAdmin = "FALSE";
            }

            hubCredentials = str;
            hubCredentialPassword = pass;
            BrandlabHubSettings.ApiCredentials = hubCredentials;
            BrandlabHubSettings.ApiCredentialPassword = hubCredentialPassword;
        }

        public async void SaveProject()
        {
            if (Project != null)
            {
                BrandlabHubSettings.ApiProject = Project;

                //need to push to HUB
                bool updated = await UpdateProject();

                if (updated)
                {
                    Debug.Log("Project successfully updated to Brandlab HUB");
                }
                else
                {
                    Debug.Log("Project failed to update to Brandlab HUB");
                }
            }
        }

        public async void GetAllProjects()
        {
            AllProjects.projects = new List<BrandlabHubSettings.BLHubProject>();

            string json = await AuthenticateProject();

            if (!string.IsNullOrEmpty(json) || !json.Equals("[]"))
            {
                var data = new JSONObject(json);

                foreach (JSONObject obj in data.list)
                {
                    BrandlabHubSettings.BLHubProject pro = new BrandlabHubSettings.BLHubProject();
                    //set project data
                    pro.id = obj.GetField("id").intValue.ToString();
                    pro.name = obj.GetField("name").stringValue;
                    pro.app_type = obj.GetField("app_type").stringValue;
                    pro.sdk_version = obj.GetField("sdk_version").stringValue;
                    pro.furioos_id = obj.GetField("furioos_id").stringValue;
                    pro.project_id = obj.GetField("project_id").stringValue;
                    pro.model_id = obj.GetField("model_id").stringValue;
                    pro.logo_url = obj.GetField("logo_url").stringValue;
                    pro.logo_width = obj.GetField("logo_width").stringValue;
                    pro.bg_color = obj.GetField("bg_color").stringValue;
                    pro.dark_mode = obj.GetField("dark_mode").boolValue;

                    AllProjects.projects.Add(pro);
                }
            }
        }

        public async void FindProject(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                Debug.Log("Project ID cannot by null");
                return;
            }

            string json = await AuthenticateProject();

            if (!string.IsNullOrEmpty(json) || !json.Equals("[]"))
            {
                var data = new JSONObject(json);

                foreach (JSONObject obj in data.list)
                {
                    if (obj.GetField("name").stringValue.Equals(name))
                    {
                        Project = new BrandlabHubSettings.BLHubProject();
                        //set project data
                        Project.id = obj.GetField("id").intValue.ToString();
                        BrandlabHubSettings.ApiProjectID = Project.id;

                        Project.name = obj.GetField("name").stringValue;
                        Project.app_type = obj.GetField("app_type").stringValue;
                        Project.sdk_version = obj.GetField("sdk_version").stringValue;
                        Project.furioos_id = obj.GetField("furioos_id").stringValue;
                        Project.project_id = obj.GetField("project_id").stringValue;
                        Project.model_id = obj.GetField("model_id").stringValue;
                        Project.logo_url = obj.GetField("logo_url").stringValue;
                        Project.logo_width = obj.GetField("logo_width").stringValue;
                        Project.bg_color = obj.GetField("bg_color").stringValue;
                        Project.dark_mode = obj.GetField("dark_mode").boolValue;
                        //Project.password = obj.GetField("password").stringValue;
                        // Project.password_confirmation = obj.GetField("password_confirmation").stringValue;

                        BrandlabHubSettings.ApiProject = Project;
                        projectFound = true;

                        if (!previousProjectName.Equals(Project.name))
                        {
                            SaveProject();
                            previousProjectName = Project.name;
                        }

                        break;
                    }
                }
            }

            if (!projectFound)
            {
                //create new project
                if (string.IsNullOrEmpty(Project.name))
                {
                    Debug.Log("New project creation failed. Application Name cannot be null");
                    return;
                }

                bool success = await CreateProject();

                if (success)
                {
                    Debug.Log("New Application created");
                    FindProject(Project.name);
                }
                else
                {
                    Debug.Log("New Application failed to create");
                }
            }
        }

        public async void GetAuthentication()
        {
            bool valid = await ValidToken();
            BrandlabHubSettings.IsAdmin = valid ? "TRUE" : "FALSE";
        }

        private async Task<bool> ValidToken()
        {
            return await BrandlabHubAPIHandler.GetValidToken(hubCredentials, hubCredentialPassword);
        }

        private async Task<string> AuthenticateProject()
        {
            return await BrandlabHubAPIHandler.GetAllProjects();
        }

        private async Task<bool> UpdateProject()
        {
            return await BrandlabHubAPIHandler.UpdateApplication(Project);
        }

        private async Task<bool> CreateProject()
        {
            return await BrandlabHubAPIHandler.CreateApplication(Project);
        }
    }
}
