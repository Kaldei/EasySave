<h1 align="center">💾</br>EasySave</h1>
<p align="center">
  Diagrams Documentation
</p>


---
## Use Case
To start the conception of EasySave 3.0, it is important to define the new needs. So we updated our Use Case Diagram. We added the Remote User as a new User. This new user can see live the progress of the backups. Connected to a socket, for each backup, the user can :
* Pause (make pause after the current file transfer)
* Play (start or resume pause)
* Cancel (to stop the backup)


## Activity 
After that, we detailed the Activity Diagram. The main change here is how the backup works. They can be launched in parallel and respect new conditions :
* Checks if a backup with priority file is running.
* Prohibits the transfer of two files larger than n KO at the same time.

Settings for priority files and maximum simultaneous KO size file are added to the EasySave Setting.


## Class
Principal updates are to enable EasySave for socket communication, add new backup interactions, and add new properties backup : 
* We implemented multi-threading with AutoResetEvent in the MenuViewModel.
* Add functions to connect EasySave with the Remote User with sockets.
* Add new attributes and functions in Setting and SettingsView Class for priority extensions files and maximum simultaneous KO size file.
 

## Sequence
We delete the Display Works Diagram Sequence because all the works are displayed when EasySave is started.
Also? we change the Launch Backup Diagram Sequence. For more visibility we remove the link to CryptoSoft and highlight the different user interactions of the backup (Play, Pause, Cancel).


## State
And finally, we add a state diagram to highlight the different states of a work.
