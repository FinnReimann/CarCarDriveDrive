Welcome file
Welcome file

# CarCarDriveDrive (CCDD)
German Below.

## Overview
An easy to use asset for Unity to let cars drive independently on virtual roads. 
This asset is simply dragged and dropped onto the model of the car in the Unity Inspector. The road must be surrounded by a material with the layer "RaycasterRoad". This can be a shoulder or the entire road surface.
The driving behavior can be influenced with a few, easy to understand, parameters.
This software can be used for background driving in games, to run city simulations or as AI in racing games.

![demo-gif](./InstallGuide/Demo.gif)

## Install Guide:
Download the latest release from the release list.
The package contains a unityproject with some sample scenes (tested with version 2021.3.24f).



## Zusammenfassung
Ein einfach zu nutzens Asset für Unity um Autos selbständig auf virtuellen Straßen fahren zu lassen. 
Dieses Asset wird einfach per Drag & Drop auf das Modell des Autos im Unity Inspector platziert. Die Straße muss dabei von einem Material mit dem Layer "RaycasterRoad" umgeben sein. Dies kann ein Seitenstreifen oder auch der gesamte Untergrund sein.
Das Fahrverhalten lässt sich mit wenigen, leicht verständlichen, Parametern beeinflussen.
Diese Software kann für Hintergrundfahrten in Spielen, zur Durchführung von Städtesimulationen oder als KI in Rennspielen eingesetzt werden.


## Installationsanleitung:
Lade die neuste Version von der Releaseliste.

### Nutzen der Beispielscene
Das Paket enthält ein Unityprojekt mit einer Hand voll Beispielscnene (getestet mit der Version 2021.3.24f).
Wenn das Projekt mit Unity geöffnet wird, findet man in der SampleScene einen einfachen StraßenParcourse der die Funktion des Scripts zeigt.
Es kann eins der Autos ausgewählt werden und auf dem Child Gameobject Controllsoftware können die Parameter verändert werden und des Verhalten getestet werden.

### Einbinden des Prefabs
Im Download `CarCarDriveDriveFolder\Unity\Assets\Resources` befindet sich das Car und Road Prefab. Diese können einfach ein Vorhandenes Unity Projekt gezogen werden.
Wenn nur das Car genutzt wird, muss die Seitenbegrenzung (Seitenstreifen, Untergrund oder extra angelegtes Objekt) der Layer "RaycastRoad" hinzugefügt werden. Außerdem benötigen diese Objecte ein Collider.
Alle Objekte mit diesem Layer werden von dem CCDD Script als Hinderniss erkannt.

### Eigene Implementierung
Um das Scricpt ein eine eigene Scene mit eigenem Fahrzeug zu implemntieren müssen alles Script aus dem Ordner `CarCarDriveDriveFolder\Unity\Assets\Scripts` in das eigne Projekt Kopiert werden (Die TestScript können weggelassen werden).
Dem Steuerungsscript des eigenen Fahrzeug muss von das Interface Observer implementieren.

    public class Ownt: MonoBehaviour, Observer
Dies verlangt das implemntieren der Funktion CCDDUpdate.
In dieser wird das DriveControllEvent übergeben, welches die drei Attribute
 - Accelerate (0 bis 1)
 - Brake (0 bis 1)
 - Steer (-1 bis 1; -1 ist links)
 
beinhaltet. Diese können wie im Beispiel unten in Klassenvariablen gespeichert werden um an anderer Stelle des Script (z.B. der Updatemethode) verwendet zu werden. Alternativ lassen sich dort auch direkt Steuerbefehle ausführen.

    
    public void CCDDUpdate(CCDDEvents e)  
    {  
        if (e is DriveControllEvent driveChange)  
    	    {  
    	    _acceleration = driveChange.Accelerate;  
    	    _brakeStrength = driveChange.Brake;  
    	    _targetSteering = driveChange.Steer;  
    	    }  
    }
Wenn das Objekt aktuell z.B. mit 

    SteerTheCar(Input.GetAxis("Horizontal"));
   manuell gelenkt wird, lässt es sich, nach der implementierung des Codes oben, einfach durch

       SteerTheCar(_targetSteering);

   ersetzten.


Markdown 3705 bytes 499 words 71 lines Ln 69, Col 13HTML 3020 characters 488 words 50 paragraphs
