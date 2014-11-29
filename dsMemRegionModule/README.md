Region module for OpenSimulator
===============================

Region module to add shared memory to a simulator instance that can be called from a script.

The prebuild.xml file is important as it assist in integrating the module into opensim when the prebuild script is run.
Region module for OpenSimulator

Copy the dsMemRegionModule folder into your (opensim)/addon-modules folder and run the prebiuld script.

Edit opensim.ini and update it as shown below:
	AllowMODFunctions = true