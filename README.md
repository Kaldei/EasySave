<br />
<p align="center">
  <a href="https://dev.azure.com/ACALCGLK/EasySave/_git/EasySave?path=%2F&version=GBmaster">
    <img src="https://www.flaticon.com/svg/static/icons/svg/3790/3790894.svg" alt="Logo" width="80" height="80">
  </a>

  <h3 align="center"> EasySave </h3>

  <p align="center">
    A little backup software by ProSoft
    <br />
    <a href="https://dev.azure.com/ACALCGLK/_git/EasySave?path=%2FDOCUMENTATION.md"><strong>User documentation »</strong></a>
    <br />
    <br />
  </p>
</p>

<!-- TABLE OF CONTENTS 
<details open="open">
  <summary><h2 style="display: inline-block">Table of Contents</h2></summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#release-dates">Release dates</a></li>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#version-1.0">Version 1.0 - Console Version</a>
      <ul>
        <li><a href="#log-file">Log File</a></li>
        <li><a href="#state-file">State File</a></li>
      </ul>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgements">Acknowledgements</a></li>
  </ol>
</details>
-->


<!-- ABOUT THE PROJECT -->
## About The Project

Our team has just joined the software publisher ProSoft.
Under the responsibility of the CIO, we will be responsible for managing the “EasySave” project which consists of developing backup software.
Like any software in the ProSoft Suite, the software will fit into the pricing policy.
* Unit price: 200 € HT
* Annual maintenance contract 5/7 8-17h (updates included): 12% purchase price (tacit renewal annual contract with revaluation based on the SYNTEC index)

During this project we are responsible of the archiutecture of the application, the development and the managment of differents versions, but also the user documentation.
In order for our code to be reusable by another team, we have respected certain constraints and tools (cf <a href="#built-with">Built With</a>).

### Release dates : 
* Version 1 : 25-11-2020
* Version 2 : 07-12-2020
* Version 3 : 17-12-2020

### Built With

This is the list of the major technologies that we used in this project.

* [Visual Studio 2019](https://visualstudio.microsoft.com/fr/downloads/)
* [.Net Framework](https://docs.microsoft.com/fr-fr/dotnet/)
* [Visual Paradigm](https://online.visual-paradigm.com/fr/)


## Version 1.0 - Console Version

The specification for this version are the following :
* The software use the .Net Core.
* The software allow the user to do 5 backup works
* One backup work is defined by : 
    * Name
    * Source
    * Destination
    * A type (Complete or Differential)
* The user can request that one of the backup jobs run or that the jobs run sequentially.
* The directories (sources and targets) can be on local disks, External or network drives.
* All items in the source directory are affected by the backup.

### Log file

The software must write in real time in a daily log file the history of the actions of the backup jobs. The minimum information expected is:
* Horodatage   
* Appellation du travail de sauvegarde 
* Adresse complète du fichier Source (format UNC) 
* Adresse complète du fichier de destination (format UNC) 
* Taille du fichier  
* Temps de transfert du fichier en ms (négatif si erreur)   

### State file

The software must record in real time, in a single file, the progress of the backup jobs. The minimum information expected is:
* Timestamp
* Name of the backup job
* Status of the Backup job (ex: Active, Not Active ...)
* If the job is active
    * The total number of eligible files
    * The size of the files to be transferred
    * Progression
    * Number of remaining files
    * Size of the remaining files
    * Full address of the Source file being backed up
    * Full address of the destination file

<!-- GETTING STARTED -->
## Getting Started

You need to follow the differents steps below to run the software properly.

### Prerequisites

In order to run this project you need to have :

* Visual Studio 2019 or the .NET SDK
* Github 

### Installation

1. Clone the repo
   ```sh
   git clone https://ACALCGLK@dev.azure.com/ACALCGLK/EasySave/_git/EasySave
   ```
2. You can now open the project on Visual Studio

<!-- USAGE EXAMPLES -->
## Usage

Now that you have clone the project, you can open it. To launch the software you need to execute the EasySave.exe file or use Visual Studio.
You can now display, add, remove and execute any backup work via the main menu interface.

_For more examples, please refer to the [User Documentation](https://dev.azure.com/ACALCGLK/_git/EasySave?path=%2FDOCUMENTATION.md)_

<!-- ROADMAP 
## Roadmap

See the [open issues](https://github.com/github_username/repo_name/issues) for a list of proposed features (and known issues).


## Contributing

Contributions are what make the open source community such an amazing place to be learn, inspire, and create. Any contributions you make are **greatly appreciated**.

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request




## License

Distributed under the MIT License. See `LICENSE` for more information.

-->

<!-- CONTACT -->
## Contact

* Arthur Caldeirero - arthur.caldeirero@viacesi.fr
* Anthony Lorendeaux - anthony.lorendeaux@viacesi.fr
* Clément Gaston - clement.gaston@viacesi.fr
* Laetitia Kessas - laetitia.kessas@viacesi.fr

Project Link: [https://dev.azure.com/ACALCGLK/EasySave/_git/EasySave](https://dev.azure.com/ACALCGLK/EasySave/_git/EasySave)



<!-- ACKNOWLEDGEMENTS 
## Acknowledgements

* []()
* []()
* []()

-->