using System;
using System.Collections.Generic;
using System.Reflection;
using log4net;
using Mono.Addins;
using Nini.Config;
using OpenSim.Region.Framework.Interfaces;
using OpenSim.Region.Framework.Scenes;
using OpenSim.Region.CoreModules.Framework.InterfaceCommander;

[assembly: Addin("ds001RegionModule", "0.1")]
[assembly: AddinDependency("OpenSim", "0.5")]

namespace Modds001RegionModule
{

    public interface Ids001RegionModule
    {
        // Place items here that we want implementations of our interface to adhere to

    }

    // We need this for our module loader plugin. it uses mono-addins to locate
    // and manage extensions througout OpenSimulator.
    [Extension(Path = "/OpenSim/RegionModules", NodeName = "RegionModule")]
    public class ds001RegionModule : Ids001RegionModule, ISharedRegionModule
    {
        #region Fields
 
        private static readonly ILog m_log = LogManager.GetLogger (MethodBase.GetCurrentMethod ().DeclaringType);
        private Dictionary<string, Scene> m_Scenes = new Dictionary<string, Scene> ();
        public IConfigSource m_config;

        #endregion

        #region properties
        // We need to return our name when registering with the simulator. We can have it here (easy when making
        // a generic framework that can be reused. Or, it's OK to just have the Name property do it there. It is
        // here as a convenience { see Name below }
        public string m_name = "ds001RegionModule";
        #endregion properties

        #region ISharedRegionModule implementation
        public void PostInitialise ()
        {
            m_log.InfoFormat("[ds001RegionModule]: void PostInitialise ()");

            foreach(Scene s in m_Scenes.Values)
                m_log.InfoFormat("[ds001RegionModule]: void PostInitialise () for this scene");

        }

        #endregion

        #region IRegionModuleBase implementation

        public void Initialise (IConfigSource source)
        {
            // We just have this to show the sequence
            m_log.InfoFormat("[ds001RegionModule]: Initialise (IConfigSource source)");
        }

        public void Close ()
        {
            m_log.InfoFormat("[ds001RegionModule]: Close ()");
        }

        public void AddRegion (Scene scene)
        {
            m_log.InfoFormat("[ds001RegionModule]: AddRegion (Scene scene)");

            // Hook up events
            scene.EventManager.OnMakeRootAgent += HandleOnMakeRootAgent;
        }

        public void RemoveRegion (Scene scene)
        {
            // We just have this to show the sequence
            m_log.InfoFormat("[ds001RegionModule]: RemoveRegion (Scene scene)");

            // un-register our event handlers...
            scene.EventManager.OnMakeRootAgent -= HandleOnMakeRootAgent;

            // We can remove this Scene from out Dictionary - it may have others
            // if we run more than one region in the instnce
            if (m_Scenes.ContainsKey (scene.RegionInfo.RegionName)) {
                lock (m_Scenes)
                {
                    m_Scenes.Remove (scene.RegionInfo.RegionName);
                }
            }
            scene.UnregisterModuleInterface<Ids001RegionModule>(this);
        }

        public void RegionLoaded (Scene scene)
        {
            m_log.InfoFormat("[ds001RegionModule]: RegionLoaded (Scene scene)");
        }

        public string Name {

            get
            {
                 return m_name;
            }
        }

        public Type ReplaceableInterface {

            get
            {
                return null;
            }
        }
        #endregion

        #region Event Handlers

        void HandleOnMakeRootAgent(ScenePresence obj)
        {
            ScenePresence sp = (ScenePresence)obj;
            OpenSim.Framework.IClientAPI client = sp.ControllingClient;

            client.SendAgentAlertMessage(String.Format("Welcome to my sim!"), true);
            client.Kick("too late bud");

        }

        #endregion Event Handlers

        #region ds001RegionModule
        // Constructor
        public ds001RegionModule ()
        {
            m_log.InfoFormat("[ds001RegionModule]: ds001RegionModule ()");
            
        }

        // We can add convenience methods to do tedious tasks and use them
        private string GetClientName(OpenSim.Framework.IClientAPI client)
        {
            return String.Format("{0} {1}", client.FirstName, client.LastName);
        }
        #endregion
    }
}
