<h1 align="center">💾</br>EasySave</h1>
<p align="center">
  EasySave Documentation
</p>


---
## General
When you launched EasySave, the Main Menu is displayed. 

Here are the different options you have in the Main Menu :

<p align="center">
  <img src="https://i.ibb.co/jvbg1h7/Easy-Save-Visual.png" alt="EasySave Visual" width="600">
</p>

### Backup Works Table
On the Main Menu you will find a Table that contains the Backup Works you've created.
* You can select one or more Backup Works (and even Select all Backup Works with the `Select` All Button).

### Launch a Backup
From the Menu, you can click on the light green `>` button to launch Backups:
* This button will launch Backup Works you've selected.
* In the same time the `State.json` file is updated with information on progression of the Backup.
* Every time a File is saved, the today's Log file is updated.

Notes : 
* If you launch a Differential Backup for the first time, it will automatically launch a Full Backup.
* If you launch a Differential Backup on a Encrypted Work, the encrypted files will be saved anyway because we are able to know if the file changed or not (because they are encrypted). 

### Progress Bar Color
When a Backup is running, you can see it's progression with the progress bar.
* This progress bar can take several colors depending on the state of the backup.
 Theses states can be found in the caption part at the bottom of the of the Main Menu.

### Pause a Backup
From the Menu, you can click on the orange `||` button to pause Backups.
You can resume a paused Backup by clicking on the light green `>`.

### Cancel a Backup
From the Menu, you can click on the orange `X` button to cancel Backups.
* It will stop Backups and remove theirs destination folders. 

### Remove a Work
From the Menu, you can click on the red `-` button to display the REmove Backup Works:
* This button will Delete the Backup Works you've selected (one or several).
* It's impossible to Remove a Backup if it's running.

### Add a Work
From the Menu, you can click on the green `+` button to display the Add Backup Work View:

<p align="center">
  <img src="https://i.ibb.co/4t3B42P/AddWork.png" alt="AddWork" border="0">
</p>

* You need to enter a Name between 1 to 20 characters. It has to be a unique.
* Then you need to enter a valid Source directory like this : `C:\Users\Desktop\File`.
* And also a valid destination Directory. The Destination of must not be a sub-folder of the source.
* Next you have to choose a Type of Backup (Differential / Full).
* Finally can decide to Encrypt this Work (only the extensions selected in Settings).

### Settings
From the Menu, you can click on the `Settings` button to display the Settings View:

<p align="center">
  <img src="https://i.ibb.co/k20WdPQ/Settings.png" alt="Settings" border="0">
</p>

* You can set the CryptoSoft Path (it has to be an Existing `.exe`). Please don't put another software thant CryptoSoft here.
* You can add or remove File Extensions that you want to Encrypt (for you Encrypted Backup Works). They have to start with a '`.`' .
* You can add or remove File Extensions you want to be a priority. They have to start with a '`.`').
* You can add or remove Business Softwares that will prevent Backup Works from launching if they are running.
* You can set a Maximum Files Size (in ko) that can be save simultaneously (0 to none).
* You can set you language (English or French).

### Connection to the Panel
EasySave come now with a Remote Panel (Panel Admin):

<p align="center">
  <img src="https://i.ibb.co/Mf1TK3v/Panel.png" alt="Panel" border="0">
</p>

To connect the Panel to EasySave, you have click to the `Connection` button on EasySave. Then you have to enter the IP and the Port you want to connect (for now EasySave is listening on port `8080`).

With this Panel you can:
* Follow Backups Progress (progress and state).
* Launch Backups.
* Pause Backups.
* Cancel Backups.
* Change language.


---
## Files & Storage
### Settings
All settings are store in `Settings.json`.

Note: Please don't mess with that file it could prevent EasySave to work. If you have a problem with that file, delete it. Easy Save will recreate it properly.

### Works
Works information are saved in `State.json`. You have complementary information in that file, like the last time this Work was Launched). 

Note: Please don't mess with that file it could prevent EasySave to work. If you have a problem with that file, delete it. Easy Save will recreate it properly.

### Logs
Logs are stored in the `Logs` folder. In this folder you will find daily logs with that format `YYYY-MM-DD.json`. A log contains :
* The name of the Backup Work.
* Source File saved.
* Destination where the File is saved.
* The Size of the File.
* Start time of the File Save.
* Time spent on the save of the File.
* Time spent on the encryption of the File.

