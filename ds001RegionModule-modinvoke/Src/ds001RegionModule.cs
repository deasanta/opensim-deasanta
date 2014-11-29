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

[assembly: Addin("ds001RegionModule", "0.1")]
[assembly: AddinDependency("OpenSim", "0.5")]

namespace Modds001RegionModule 
{
    [Extension(Path = "/OpenSim/RegionModules", NodeName = "RegionModule")]
 
    public class Modds001RegionModule  : INonSharedRegionModule
    {
        private static readonly ILog m_log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
 
        private IConfig m_config = null;
        private bool m_enabled = true;
        private Scene m_scene = null;
 
        private IScriptModuleComms m_comms;
 
#region IRegionModule Members
 
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
 
                m_comms.RegisterScriptInvocation(this,"ModTest0");
                m_comms.RegisterScriptInvocation(this,"ModTest1");
                m_comms.RegisterScriptInvocation(this,"ModTest2");
                m_comms.RegisterScriptInvocation(this,"ModTest3");
                m_comms.RegisterScriptInvocation(this,"ModTest4");
                m_comms.RegisterScriptInvocation(this,"ModTest5");
                m_comms.RegisterScriptInvocation(this,"ModTest6");
                m_comms.RegisterScriptInvocation(this,"ModTest7");
                m_comms.RegisterScriptInvocation(this,"ModTest8");
 
                // Register some constants as well
                m_comms.RegisterConstant("ModConstantInt1",25);
                m_comms.RegisterConstant("ModConstantFloat1",25.000f);
                m_comms.RegisterConstant("ModConstantString1","abcdefg");
            }
        }
 
        public Type ReplaceableInterface
        {
            get { return null; }
        }
 
#endregion
 
#region ScriptInvocationInteface
        public string ModTest0(UUID hostID, UUID scriptID)
        {
            m_log.WarnFormat("[ModInvoke] ModTest0 parameter");
            return "";
        }
 
        public string ModTest1(UUID hostID, UUID scriptID, string value)
        {
            m_log.WarnFormat("[ModInvoke] ModTest1 parameter: {0}",value);
            return value;
        }
 
        public int ModTest2(UUID hostID, UUID scriptID, int value)
        {
            m_log.WarnFormat("[ModInvoke] ModTest2 parameter: {0}",value);
            return value;
        }
 
        public float ModTest3(UUID hostID, UUID scriptID, float value)
        {
            m_log.WarnFormat("[ModInvoke] ModTest3 parameter: {0}",value);
            return value;
        }
 
        public UUID ModTest4(UUID hostID, UUID scriptID, UUID value)
        {
            m_log.WarnFormat("[ModInvoke] ModTest4 parameter: {0}",value.ToString());
            return value;
        }
 
        public OpenMetaverse.Vector3 ModTest5(UUID hostID, UUID scriptID, OpenMetaverse.Vector3 value)
        {
            m_log.WarnFormat("[ModInvoke] ModTest5 parameter: {0}",value.ToString());
            return value;
        }
 
        public OpenMetaverse.Quaternion ModTest6(UUID hostID, UUID scriptID, OpenMetaverse.Quaternion value)
        {
            m_log.WarnFormat("[ModInvoke] ModTest6 parameter: {0}",value.ToString());
            return value;
        }
 
        public object[] ModTest7(UUID hostID, UUID scriptID, int count, string val)
        {
            object[] result = new object[count];
            for (int i = 0; i < count; i++)
                result[i] = val;
 
            return result;
        }
 
        public object[] ModTest8(UUID hostID, UUID scriptID, object[] lparm)
        {
            object[] result = new object[lparm.Length];
 
            for (int i = 0; i < lparm.Length; i++)
                result[lparm.Length - i - 1] = lparm[i];
 
            return result;
        }
 
#endregion
    }
}