// see http://opensimulator.org/wiki/OSSL_Script_Library/ModInvoke for more info
// OpenSim.ini > AllowMODFunctions = true

using System;
using System.Collections.Generic;
using System.Reflection;
using log4net;
using Mono.Addins;
using Nini.Config;
using OpenSim.Region.Framework.Interfaces;
using OpenSim.Region.Framework.Scenes;
using System.Threading;
using System.Text;
using System.Net;
using System.Net.Sockets;
using OpenMetaverse;
using OpenMetaverse.StructuredData;
using OpenSim.Framework;
using System.Text.RegularExpressions;

[assembly: Addin("dsMemRegionModule", "0.1")]
[assembly: AddinDependency("OpenSim", "0.5")]

namespace dsMemRegionModule 
{
    [Extension(Path = "/OpenSim/RegionModules", NodeName = "RegionModule")]
 
    public class dsMemRegionModule  : INonSharedRegionModule
    {
        private static readonly ILog m_log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
 
        private IConfig m_config = null;
        private bool m_enabled = true;
        private Scene m_scene = null;
 
        private IScriptModuleComms m_comms;
 
#region IRegionModule Members

        private Dictionary<string, string> dsMem = new Dictionary<string, string>();
 
        public string Name
        {
            get { return this.GetType().Name; }
        }
 
        public void Initialise(IConfigSource config)
        {
            m_log.WarnFormat("[ModInvoke] start configuration");
 
            try 
            {
                if ((m_config = config.Configs["ModInvoke"]) != null)
                    m_enabled = m_config.GetBoolean("Enabled", m_enabled);
            }
            catch (Exception e)
            {
                m_log.ErrorFormat("[ModInvoke] initialization error: {0}",e.Message);
                return;
            }
 
            m_log.ErrorFormat("[ModInvoke] module {0} enabled",(m_enabled ? "is" : "is not"));
        }
 
        public void PostInitialise()
        {
            if (m_enabled) {}
        }
 
        public void Close() { }
        public void AddRegion(Scene scene) { }
        public void RemoveRegion(Scene scene)  { }
 
        public void RegionLoaded(Scene scene)
        {
            if (m_enabled)
            {
                m_scene = scene;
                m_comms = m_scene.RequestModuleInterface<IScriptModuleComms>();
                if (m_comms == null)
                {
                    m_log.WarnFormat("[ModInvoke] ScriptModuleComms interface not defined");
                    m_enabled = false;
 
                    return;
                }

                m_comms.RegisterScriptInvocation(this, "dsMemClear");
                m_comms.RegisterScriptInvocation(this, "dsMemCount");
                m_comms.RegisterScriptInvocation(this, "dsMemGet");
                m_comms.RegisterScriptInvocation(this, "dsMemSet");

            }
        }
 
        public Type ReplaceableInterface
        {
            get { return null; }
        }
 
#endregion
 
#region ScriptInvocationInteface

        public int dsMemClear(UUID hostID, UUID scriptID)
        {
            dsMem.Clear();
            return 1;
        }

        public int dsMemCount(UUID hostID, UUID scriptID)
        {
            return dsMem.Count;
        }

        public string dsMemGet(UUID hostID, UUID scriptID, string value)
        {
            string retval = "";

            // See whether Dictionary contains this string.
            if (dsMem.ContainsKey(value))
            {
                retval = dsMem[value];
            }
            return retval;
        }

        public int dsMemSet(UUID hostID, UUID scriptID, string value1, string value2)
        {
            int retval = 1; // true

            // See whether Dictionary contains this string.
            if (dsMem.ContainsKey(value1))
            {
                dsMem[value1] = value2;
                retval = 1; // return false if value is replaced
            }
            else
            {
                dsMem.Add(value1, value2);
                retval = 0; // return false if value is added
            }

            return retval;
        }
 
#endregion
    }
}