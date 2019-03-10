This is a temporary solution for adding CF2 support to UFE 1.7.1.

----------
WARNING 
----------
Importing this package may break your existing CF 1.x prefabs! 
Backup your project before importing this package!

--------------
Instructions
--------------

1. Import CF2. (Don't try to manually import CF2-UFE add-on - it will not work with UFE 1.7.1)

2. If present, delete all Control Freak 1.x files and folders: 
	/Plugins/Control-Freak/
	/Plugins/Control-Freak-Presets/
	/Plugisn/Control-Freak-Sample-GUI-Images/
	/Plugins/Editor/Control-Freak-Editor/
	/Control-Freak-Demos/		

3. Import this package. This will unpack all the prefabs and scripts to 'UFE/ThirdPartyAssets/Control-Freak-2-Hack/' folder. 

4. Open UFE Global editor. In the 'Input' section, pick 'Control Freak' as the input manager and in the 'Control Freak Preferences' foldout, assign one of the prefabs from '__UFE-Temporary-CF2-Solution/Prefabs/'.

That's all.


-------------------
Notes
-------------------
  
Be warned, this solution is not perfect - for example, controls will be visible over the pause menu.
 
To use custom CF2 Input rigs with UFE, please attach the 'Touch Controller' component to the root object of rig prefab.
