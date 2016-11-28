![icon](https://github.com/matthiaslischka/StartLauncher/blob/master/startLauncher.png?raw=true)

[![GitHub release](https://img.shields.io/github/release/matthiaslischka/StartLauncher.svg)](https://github.com/matthiaslischka/StartLauncher/releases/latest)
[![Build status](https://ci.appveyor.com/api/projects/status/qy1io2k9kt00be3p?svg=true)](https://ci.appveyor.com/project/matthiaslischka/startlauncher)
![OS Requirement](https://img.shields.io/badge/OS-Windows-blue.svg)

A very lightweight launcher that uses the Windows Start Menu for custom commands.

**Configure via UI. Execute via Start Menu.**

# Still alpha
Use as developer or for testing only. App runs in system tray and minimizes back to systemtray when window is closed. Configure commands on the UI. Batch files that execute your command are created in the bin folder. Windows shortcuts are created in the current users' windows start menu. Windows indexes the shortcuts within seconds. Use vie start menu. Keeps running as taskbar program. Commands are stored in a JSON file and the file is being watched for updates as long as the program is running. For the future it is planned that the file can be placed anywhere - e.g. dropbox.
## be aware
Since it is still alpha you may experience crashes or loss of shortcuts you have already configured. Uninstall may not clear everything - although it should work but you never know.

Start Menu Folder is located here:
```
C:\Users\[YOURUSERNAME]\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Start Launcher
```
# Screenshots
![screenshot program](screenshot.png?raw=true)


![screenshot start menu](startmenu.png?raw=true)
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
Fetch all and open Git Excentions with this specific repository afterwards
```
cmd /c "cd C:\projects\startlauncher\ & git fetch --all & start gitextensions"
```
# WIP

## Next Steps
* ~~Cleanup Shortcut and Command Folder on Startup~~
* ~~List Commands in UI~~
* ~~Add/Remove Commands via UI*~~
* ~~Watch file for changes and update commands on the fly~~
* ~~Add to Start Menu without admin rights~~
* ~~Commands.json file path configurable (project settings + UI)~~
* ~~Shortcuts für Add, Remove, Edit~~
* ~~"Test Commands" button in UI edit-view~~
* ~~"Run as Administrator" option for commands~~
* ~~Add autodetected command icons~~
* (UI) Validation.
 * Name and Command mandatory
 * No special characters in name
 * Name unique
* Autostart
* ~~MSI Installer~~
* Chocolatey package
* UI styling
* UI for Changing autodetected command icon
* reevaluate architecture
