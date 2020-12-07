<br />
<p align="center">
  <a href="https://dev.azure.com/ACALCGLK/EasySave/_git/EasySave?path=%2F&version=GBmaster">
    <img src="https://www.flaticon.com/svg/static/icons/svg/3790/3790894.svg" alt="Logo" width="80" height="80">
  </a>

  <h2 align="center">EasySave v2.0 (Graphical Version)</h2>
  <h3 align="center">User Documentation</h3>
</p>

----

## General
When you launched EasySave, the Main Menu is displayed. 

Here are the different options you have in the Main Menu :

<a href="https://ibb.co/FDkvDjJ"><img src="https://i.ibb.co/x7Kv7RG/4.png" alt="4" border="0"></a>

### Backup Works Table
On the Main Menu you will find a Table that contains the Backup Works you've created.
* You can sort them by Name, Source, Destination, Backup Type, if they need to be encrypted or not.
* You can select one or more Backup Works (and even Select all Backup Works with the `Select` All Button).

### Launch a Backup
From the Menu, you can click on the light green `>` button to launch Backup Works:
* This button will launch the Backup Works you've selected (one or several).
* The program will pause until the backup has finished.
* In the same time the `State.json` file is updated with information on progression of the Backup.
* Every time a File is saved, the today's Log file is updated.
* When all the Backup Works are completed, a recap of the backups that have just been made will be displayed.

Notes : 
* If you launch a Differential Backup for the first time, it will automatically launch a Full Backup.
* If you launch a Differential Backup on a Encrypted Work, the encrypted files will be saved anyway because we are able to know if the file changed or not (because they are encrypted). 

### Remove a Work
From the Menu, you can click on the red `-` button to display the REmove Backup Works:
* This button will Delete the Backup Works you've selected (one or several).

### Add a Work
From the Menu, you can click on the green `+` button to display the Add Backup Work View:

<a href="https://ibb.co/f9yjdVK"><img src="https://i.ibb.co/pQMYj6s/5.png" alt="5" border="0"></a>

* You need to enter a Name between 1 to 20 characters. It has to be a unique.
* Then you need to enter a valid Source directory like this : `C:\Users\Desktop\File`.
* And also a valid destination Directory. The Destination of must not be a sub-folder of the source.
* Next you have to choose a Type of Backup (Differential / Full).
* Finally can decide to Encrypt this Work (only the extensions selected in Settings).

### Settings
From the Menu, you can click on the `Settings` button to display the Settings View:

<a href="https://ibb.co/NrjXnp8"><img src="https://i.ibb.co/pQzgrZt/6.png" alt="6" border="0"></a>

* You can set the CryptoSoft Path (it has to be an Existing `.exe`). Please don't put another software thant CryptoSoft here.
* You can add or remove File Extensions you want to Encrypt for you Encrypted Backup Works (it has to start with a `.`).
* You can add or remove Business Softwares that will prevent Backup Works from launching if they are running.
* You can set you language (English or French).

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

