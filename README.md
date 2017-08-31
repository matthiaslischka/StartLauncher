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
# Some examples of useful commands for daily work
**cmd.exe is the limit. there is no limit.**

Open internet explorer with new session and specific url
```
iexplore.exe -noframemerging http://localhost/myApsNetDevProject/start.aspx
```
Open chrome with icognito modus (no cache)
```
chrome --incognito http://localhost/myAngularDevProject/#/search
```
Open windows explorer with specific path
```
explorer c:\Super\Annoying\Path\that\I\need\all\the\time
```
Open Git Extensions with specific repository
```
gitextensions openrepo C:\projects\startlauncher\
```
Fetch all and open Git Extensions with this specific repository afterwards
```
cmd /c "cd C:\projects\startlauncher\ & git fetch --all & start gitextensions"
```
Cleanup tmp folder incl. all files and subfolders
```
cmd /c "rmdir c:\tmp /S /Q & mkdir c:\tmp"
```
