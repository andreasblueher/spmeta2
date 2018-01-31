using System;
using System.Linq;
using System.Security;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.ModelHosts;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class WebApplicationModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(WebApplicationDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var farmModelHost = modelHost.WithAssertAndCast<FarmModelHost>("modelHost", value => value.RequireNotNull());
            var webApplicationDefinition = model.WithAssertAndCast<WebApplicationDefinition>("model", value => value.RequireNotNull());

            DeployWebApplication(farmModelHost, farmModelHost.HostFarm, webApplicationDefinition);
        }

        public override void WithResolvingModelHost(ModelHostResolveContext modelHostContext)
        {
            var modelHost = modelHostContext.ModelHost;
            var model = modelHostContext.Model;
            var childModelType = modelHostContext.ChildModelType;
            var action = modelHostContext.Action;

            var webApplicationDefinition = model.WithAssertAndCast<WebApplicationDefinition>("model", value => value.RequireNotNull());

            if (modelHost is WebApplicationModelHost)
            {
                base.WithResolvingModelHost(modelHostContext);
                return;
            }

            var farmModelHost = modelHost.WithAssertAndCast<FarmModelHost>("modelHost", value => value.RequireNotNull());

            var webApps = SPWebService.ContentService.WebApplications;
            var existingWebApp = FindWebApplication(webApplicationDefinition, webApps);

            if (existingWebApp == null)
            {
                throw new SPMeta2Exception(string.Format("Cannot find web aplication by definition:{0}", webApplicationDefinition));
            }

            var webAppModelHost = ModelHostBase.Inherit<WebApplicationModelHost>(farmModelHost, h =>
            {
                h.HostWebApplication = existingWebApp;
            });

            action(webAppModelHost);
        }

        private void DeployWebApplication(FarmModelHost farmModelHost,
            SPFarm farm,
            WebApplicationDefinition definition)
        {
            var webApps = SPWebService.ContentService.WebApplications;
            var existingWebApp = FindWebApplication(definition, webApps);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = existingWebApp,
                ObjectType = typeof(SPWebApplication),
                ObjectDefinition = definition,
                ModelHost = farmModelHost
            });

            if (existingWebApp == null)
            {
                var webAppBuilder = new SPWebApplicationBuilder(farm);

                webAppBuilder.Port = definition.Port;
                webAppBuilder.ApplicationPoolId = definition.ApplicationPoolId;

                if (!string.IsNullOrEmpty(definition.ManagedAccount))
                {
                    webAppBuilder.IdentityType = IdentityType.SpecificUser;

                    var managedAccounts = new SPFarmManagedAccountCollection(SPFarm.Local);
                    var maccount = managedAccounts.FindOrCreateAccount(definition.ManagedAccount);

                    webAppBuilder.ManagedAccount = maccount;
                }
                else
                {
                    webAppBuilder.ApplicationPoolUsername = definition.ApplicationPoolUsername;

                    var password = new SecureString();

                    foreach (char c in definition.ApplicationPoolPassword)
                        password.AppendChar(c);

                    webAppBuilder.ApplicationPoolPassword = password;
                }

                webAppBuilder.CreateNewDatabase = definition.CreateNewDatabase;

                webAppBuilder.DatabaseName = definition.DatabaseName;
                webAppBuilder.DatabaseServer = definition.DatabaseServer;

                webAppBuilder.UseNTLMExclusively = definition.UseNTLMExclusively;

                webAppBuilder.HostHeader = definition.HostHeader;
                webAppBuilder.AllowAnonymousAccess = definition.AllowAnonymousAccess;
                webAppBuilder.UseSecureSocketsLayer = definition.UseSecureSocketsLayer;

                var webApp = webAppBuilder.Create();
                webApp.Provision();

                webApp.BrowserFileHandling = (SPBrowserFileHandling)(int)definition.BrowserFileHandling;
                webApp.Update();

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = webApp,
                    ObjectType = typeof(SPWebApplication),
                    ObjectDefinition = definition,
                    ModelHost = farmModelHost
                });
            }
            else
            {
                existingWebApp.BrowserFileHandling = (SPBrowserFileHandling)(int)definition.BrowserFileHandling;
                existingWebApp.Update();

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = existingWebApp,
                    ObjectType = typeof(SPWebApplication),
                    ObjectDefinition = definition,
                    ModelHost = farmModelHost
                });
            }
        }

        private static SPWebApplication FindWebApplication(WebApplicationDefinition definition, SPWebApplicationCollection webApps)
        {
            var existingWebApp = webApps.FirstOrDefault(w =>
            {
                var webAppUri = w.GetResponseUri(SPUrlZone.Default);

                return webAppUri.Port == definition.Port;
            });
            return existingWebApp;
        }

        #endregion
    }
}
