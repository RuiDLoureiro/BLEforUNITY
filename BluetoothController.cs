using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BluetoothLE;

public class BluetoothController : MonoBehaviour
{
    public Text statusText;
    public Text receivedText;
    public string arduinoUUID = "arduino UUID here";
    public string serviceUUID = "service UUID here";
    public string characteristicUUID = "your characteristic UUID here";

    private BluetoothLEHardwareInterface ble;
    private bool connected = false;
    private byte[] buffer = new byte[256];

    private void Start()
    {
        statusText.text = "Connecting...";
        BluetoothLEHardwareInterface.Initialize(true, false, () => {
            Debug.Log("BluetoothLEHardwareInterface initialized");
            Connect();
        }, (error) => {
            Debug.Log("BluetoothLEHardwareInterface initialization error: " + error);
        });
    }

    private void Connect()
    {
        ble = BluetoothLEHardwareInterface.Initialize(true, false, () => {
            ble.ConnectToPeripheral(arduinoUUID, (address) => {
                Debug.Log("Connected to " + address);
                connected = true;
                statusText.text = "Connected";
                ble.SubscribeCharacteristicWithDeviceAddress(serviceUUID, characteristicUUID, null, (address, uuid, value) => {
                    string receivedString = Encoding.ASCII.GetString(value);
                    receivedText.text = "Received: " + receivedString;
                });
            }, (address) => {
                Debug.Log("Disconnected from " + address);
                connected = false;
                statusText.text = "Disconnected";
                ble.DeInitialize(() => {
                    Debug.Log("BluetoothLEHardwareInterface disconnected");
                });
            });
        }, (error) => {
            Debug.Log("BluetoothLEHardwareInterface initialization error: " + error);
        });
    }

    private void SendData(string data)
    {
        if (!connected)
        {
            Debug.Log("Not connected to Arduino");
            return;
        }

        byte[] bytes = Encoding.ASCII.GetBytes(data);
        ble.WriteCharacteristic(serviceUUID, characteristicUUID, bytes, bytes.Length, true, (characteristicUUID) => {
            Debug.Log("Sent: " + data);
        });
    }

    // Call this method to send data to the Arduino
    public void OnSendButtonClicked()
    {
        string data = "Hello, Arduino!";
        SendData(data);
    }

    private void OnApplicationQuit()
    {
        if (connected)
        {
            ble.DisconnectPeripheral(arduinoUUID, (address) => {
                Debug.Log("Disconnected from " + address);
            });
        }

        ble.DeInitialize(() => {
            Debug.Log("BluetoothLEHardwareInterface deinitialized");
        });
    }
}
