<br />
<p align="center">
  <a href="https://dev.azure.com/ACALCGLK/EasySave/_git/EasySave?path=%2F&version=GBmaster">
    <img src="https://www.flaticon.com/svg/static/icons/svg/3790/3790894.svg" alt="Logo" width="80" height="80">
  </a>

  <h2 align="center">EasySave v3.0 (Graphical Version)</h2>

  <p align="center">
    A little backup software by ProSoft
    <br />
    <a href="https://dev.azure.com/ACALCGLK/_git/EasySave?path=%2FEasySave_Exe%2FUSERDOC.md"><strong>User documentation »</strong></a>
    <br />
  </p>
</p>

---

## About The Project

Our team has just joined the software publisher ProSoft.
Under the responsibility of the CIO, we will be responsible for managing the “EasySave” project which consists of developing backup software.
Like any software in the ProSoft Suite, the software will fit into the pricing policy.
* Unit price: 200 € HT
* Annual maintenance contract 5/7 8-17h (updates included): 12% purchase price (tacit renewal annual contract with revaluation based on the SYNTEC index)

During this project we are responsible of the architecture of the application, the development and the management of different versions, but also the user documentation.
In order for our code to be reusable by another team, we have respected certain constraints and tools (cf <a href="#built-with">Built With</a>).

### Built With

This is the list of the major technologies that we used in this project.

* [Visual Studio 2019](https://visualstudio.microsoft.com/fr/downloads/)
* [.Net Framework](https://docs.microsoft.com/fr-fr/dotnet/)
* [Visual Paradigm](https://online.visual-paradigm.com/fr/)

---

## EasySave v3.0 - Visual:

<a href="https://ibb.co/pjBVSXV"><img src="https://i.ibb.co/Xxr0qJ0/MainMenu.png" alt="MainMenu" border="0"></a>
---

## EasySave v3.0 - Release Notes:

### New Software:
* PanelAdmin: Remote Panel for EasySave.

### New Features: 
* Works can now Backup simultaneously (Multithreading).
* User can now Pause and Cancel Backups.
* Add some Automatic Backup Management:
  * When a Backup has Priority Files, others Backups stop.
  * When Two or Files are larger than a certain Size (chosen in parameters), only one will continue and the others will Wait.
* Add some Settings: 
  * Priority Extensions.
  * Maximum File Size that can be saved at the same time (in ko).
* EasySave can now connect to a Remote Panel using Sockets.
* EasySave is now a Single Instance Software.

### Enhancements:
* Menu design: Backups Progress Bars with colors depending on the state. 
* Add Files and Folders Browsers to Select Paths.
* Enhance Language Switching Management.

### May Come Next :
* Add a Search Bar to sort Works.
* Edit a Backup Work.
* Change Language without Restarting EasySave.

---

## Releases dates : 
### Console Versions:
* Version 1: 25-11-2020
* Version 1.1: 07-12-2020

### Graphical Versions:
* Version 2: 07-12-2020
* Version 3: 17-12-2020

---

## Getting Started

### Installation

1. If you have git, you can clone the repo with this command:
   ```sh
   git clone https://ACALCGLK@dev.azure.com/ACALCGLK/EasySave/_git/EasySave
   ```
2. Or you can directly download as Zip.

### Folders
In this project you will found different folders :
* EasySave_Code: Contains EasySave's Code.
* EasySave_Exe: Contains EasySave Executable and [User Documentation](https://dev.azure.com/ACALCGLK/_git/EasySave?path=%2FEasySave_Exe%2FUSERDOC.md).
* UML_Diagrams: Contains EasySave's UML Diagrams and [UML Documentation](https://dev.azure.com/ACALCGLK/_git/EasySave?path=%2FUML%20Diagrams%2FUMLDOC.md).
* CryptoSoft_Code: Contains CryptoSoft's Code.
* CryptoSoft_Exe: Contains CryptoSoft Executable.
* PanelAdmin_Code: Contains PanelAdmin's Code.
* PanelAdmin_Exe: Contains PanelAdmin Executable.

---

## Contact

* Arthur Caldeirero - arthur.caldeirero@viacesi.fr
* Anthony Lorendeaux - anthony.lorendeaux@viacesi.fr
* Clément Gaston - clement.gaston@viacesi.fr
* Laetitia Kessas - laetitia.kessas@viacesi.fr

Project Link: [https://dev.azure.com/ACALCGLK/EasySave/_git/EasySave](https://dev.azure.com/ACALCGLK/EasySave/_git/EasySave)
