// see OpenSim.Region.OptionalModules.Example.BareBonesNonShared for more info

using System;
using System.Collections.Generic;
using System.Reflection;
using log4net;
using Mono.Addins;
using Nini.Config;
using OpenSim.Region.Framework.Interfaces;
using OpenSim.Region.Framework.Scenes;

[assembly: Addin("ds001RegionModule", "0.1")]
[assembly: AddinDependency("OpenSim", "0.5")]

namespace Modds001RegionModule
{

    //[Extension(Path = "/OpenSim/RegionModules", NodeName = "RegionModule", Id = "ds001RegionModule")]
    [Extension(Path = "/OpenSim/RegionModules", NodeName = "RegionModule")]

    public class ds001RegionModule : INonSharedRegionModule
    {
        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public string Name { get { return "ds001RegionModule"; } }        
        
        public Type ReplaceableInterface { get { return null; } }
        
        public void Initialise(IConfigSource source)
        {
            m_log.DebugFormat("[ds001RegionModule]: INITIALIZED MODULE");
        }
        
        public void Close()
        {
            m_log.DebugFormat("[ds001RegionModule]: CLOSED MODULE");
        }
        
        public void AddRegion(Scene scene)
        {
            m_log.DebugFormat("[ds001RegionModule]: REGION {0} ADDED", scene.RegionInfo.RegionName);
        }
        
        public void RemoveRegion(Scene scene)
        {
            m_log.DebugFormat("[ds001RegionModule]: REGION {0} REMOVED", scene.RegionInfo.RegionName);
        }        
        
        public void RegionLoaded(Scene scene)
        {
            m_log.DebugFormat("[ds001RegionModule]: REGION {0} LOADED", scene.RegionInfo.RegionName);
        }                
    }
}