![icon](https://github.com/matthiaslischka/StartLauncher/blob/master/startLauncher.png?raw=true)

[![Build status](https://ci.appveyor.com/api/projects/status/qy1io2k9kt00be3p?svg=true)](https://ci.appveyor.com/project/matthiaslischka/startlauncher)

A very lightweight launcher that uses the Windows Start Menu for custom commands.

**Configure via UI. Execute via Start Menu.**

# How to use it
ATM no installer, no nothing. Run exe and configure commands on the ui. Batch files that execute your command are created in the bin folder. Windows shortcuts are created in the current users' windows start menu. Windows indexes the shortcuts within seconds. Use vie start menu. Keeps running as taskbar program. Commands are stored in a JSON file and the file is being watched for updates as long as the program is running. For the future it is planned that the file can be placed anywhere - e.g. dropbox.
## No Uninstaller!
Be Aware! Creates a folder in your users start menu. You have to delete that by hand if you want to "uninstall" the program completely.

Folder is located here:
```
C:\Users\[YOURUSERNAME]\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Start Launcher
```
# Screenshots
![screenshot program](https://raw.githubusercontent.com/matthiaslischka/startlauncher/master/screenshot.png?raw=true)


![screenshot start menu](https://raw.githubusercontent.com/matthiaslischka/startlauncher/master/startmenu.png?raw=true)
# Useful commands for daily work
**cmd.exe is the limit. there is no limit.**

Open internet explorer with new session and specific url
```
iexplore.exe -noframemerging http://localhost/myDevProject/start.aspx
```
Open windows explorer with specific path
```
explorer c:\Super\Annoying\Path\that\I\need\all\the\time
```
Open Git Extensions with specific repository
```
gitextensions openrepo C:\projects\startlauncher\
```
# WIP

## Next Steps
* ~~Cleanup Shortcut and Command Folder on Startup~~
* ~~List Commands in UI~~
* ~~Add/Remove Commands via UI*~~
* ~~Watch file for changes and update commands on the fly~~
* ~~Add to Start Menu without admin rights~~
* ~~Commands.json file path configurable (project settings + UI)~~
* Shortcuts für Add, Remove, Edit
* "Test Commands" button in UI edit-view
* "Run as Administrator" option for commands
* ~~Add autodetected command icons~~
* (UI) Validation.
 * Name and Command mandatory
 * No special characters in name
 * Name unique
* Autostart
* Chocolatey package
* UI styling
* UI for Changing autodetected command icon

## Further ideas
* Multiple commands files combinable from different locations
