# BLEforUNITY
Simple C# script for Unity to connect BLE with Arduino microcontrollers. 
Needs Unity Bluetooth LE (BLE)" plugin to your Unity project. You can download it from the Unity Asset Store.

Create a new C# script in your Unity project and name it "BluetoothController". Attach this script to a GameObject in your scene.
Add public fields for statusText, receivedText, arduinoUUID, serviceUUID, and characteristicUUID to the script. 
The statusText and receivedText fields should be assigned to UI Text objects in your scene, and the arduinoUUID, serviceUUID,
