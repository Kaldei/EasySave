<br />
<p align="center">
  <a href="https://dev.azure.com/ACALCGLK/EasySave/_git/EasySave?path=%2F&version=GBmaster">
    <img src="https://www.flaticon.com/svg/static/icons/svg/3790/3790894.svg" alt="Logo" width="80" height="80">
  </a>

  <h2 align="center">EasySave v2.0 (Graphical Version)</h2>
  <h3 align="center">UML Documentation</h3>
</p>

---

## Use Case

To start the conception of EasySave 2.0, it is important to define the new needs. So we updated our Use Case Diagram. We added the Settings part and CrytpoSoft as a new Actor. CrytpoSoft is used with the "Launch Backup Work" part. 

## Activity 

After that, we detailed the Activity Diagram. The main change here is that we added the "Change Settings" part.

## Class

As we change from Console Application to WPF Application, this diagram is the one that received the most change : 
* We implemented a WPF Architecture (MainWindow, ObservableObject).
* We shared ours ViewModel and View in multiple ViewModels and Views. 
* We added Settings Class.

## Sequence

And finally, Sequence Diagrams where corrected with the right actors (EasySave, External Memory, CryptoSoft).