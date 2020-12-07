<br />
<p align="center">
  <a href="https://dev.azure.com/ACALCGLK/EasySave/_git/EasySave?path=%2F&version=GBmaster">
    <img src="https://www.flaticon.com/svg/static/icons/svg/3790/3790894.svg" alt="Logo" width="80" height="80">
  </a>

  <h2 align="center">EasySave v1.1 (Console Version)</h2>
  <h3 align="center">User Documentation</h3>
</p>

----

## General
When you launched EasySave, you will have to press a key to display the Menu. 

Here are the different options you have in the Menu :

<a href="https://imgbb.com/"><img src="https://i.ibb.co/RQyQPyy/2.png" alt="2" border="0"></a>

### 0. Return to Menu
From the Menu, enter 0 to return the main menu.
* After choosing one of the options, you can return to the menu by enter 0.

### 1. Show Backup Works
From the Menu, enter 1 to display Backup Works.
* It will display all the works you saved.

### 2. Add a Work
From the Menu, enter 2 to Create a Backup Work.
* First you need to enter a Name between 1 to 20 characters. It has to be a unique.
* Then you need to enter a valid Source directory like this : `C:\Users\Desktop\File`.
* And also a valid destination Directory. The Destination of must not be a sub-folder of the source.
* Finally you have to choose a Type of Backup (Differential / Full).

### 3. Launch a Backup
From the Menu, enter 3 to Launch a Backup Work.
* You can you choose between Launch one Backup (enter the number of the Backup) or Launch All Backup (enter 0).
* Once you have choose one option, the Backup will begin and a screen will display information about the progression of the Backup.
* In the same time the `State.json` file is updated with information on progression of the Backup.
* When the backup work is over a message will be prompt.
Note : If you launch a Differential Backup for the first time, it will automatically launch a Full Backup.

### 4. Remove a Work
From the Menu, enter 4 to Remove a Backup Work.
* Choose a Backup to Delete, it will be Deleted and then screen will display a confirmation message.

### 5. Choose Language
From the Menu, enter 5 to Choose your Language.
* For now the program supports English and French.

### 6. Quit

From the Menu, enter 6 to Quit the program.
* This option is to quit the app properly.

---
## Files & Storage
### Settings
Settings (Selected Language) are store in `Settings.json`

Note: Please don't mess with that file it could prevent EasySave to work. If you have a problem with that file, delete it. Easy Save will recreate it properly.

### Works
Works are saved in `State.json`. You have complementary information in that file, like the last time this Work was Launched). 

Note: Please don't mess with that file it could prevent EasySave to work. If you have a problem with that file, delete it. Easy Save will recreate it properly.

### Logs
Logs are stored in the `Logs` folder. In this folder you will find daily logs with that format `YYYY-MM-DD.json`.


