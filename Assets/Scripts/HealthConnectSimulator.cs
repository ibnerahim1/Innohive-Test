using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FitnessApp
{
    public class HealthConnectSimulator : MonoBehaviour
    {
        [Header("UI Elements")]
        public Text statusText;
        public Button connectButton;
        public Button requestPermissionButton;
        public Button fetchDataButton;

        [Header("Integration")]
        public FitnessDataSimulator dataSimulator;
        public FitnessUIManager uiManager;

        private bool isConnected = false;
        private bool hasPermissions = false;

        void Start()
        {
            if (dataSimulator == null)
            {
                dataSimulator = FindAnyObjectByType<FitnessDataSimulator>();
            }

            if (uiManager == null)
            {
                uiManager = FindAnyObjectByType<FitnessUIManager>();
            }

            // Setup button listeners
            if (connectButton)
            {
                connectButton.onClick.AddListener(SimulateConnect);
            }

            if (requestPermissionButton)
            {
                requestPermissionButton.onClick.AddListener(SimulateRequestPermissions);
                requestPermissionButton.interactable = false; // Disabled until connected
            }

            if (fetchDataButton)
            {
                fetchDataButton.onClick.AddListener(SimulateFetchData);
                fetchDataButton.interactable = false; // Disabled until permissions granted
            }

            UpdateStatus();
        }

        public void SimulateConnect()
        {
            // Simulate connection delay
            StartCoroutine(DelayedAction(1.0f, () =>
            {
                isConnected = true;

                if (requestPermissionButton)
                {
                    requestPermissionButton.interactable = true;
                }

                UpdateStatus();
            }));
        }

        public void SimulateRequestPermissions()
        {
            if (!isConnected)
            {
                Debug.LogWarning("Cannot request permissions without connection!");
                return;
            }

            // Simulate permission request dialog and delay
            StartCoroutine(DelayedAction(1.5f, () =>
            {
                hasPermissions = true;

                if (fetchDataButton)
                {
                    fetchDataButton.interactable = true;
                }

                UpdateStatus();
            }));
        }

        public void SimulateFetchData()
        {
            if (!isConnected || !hasPermissions)
            {
                Debug.LogWarning("Cannot fetch data without connection and permissions!");
                return;
            }

            // Simulate data fetch delay
            StartCoroutine(DelayedAction(2.0f, () =>
            {
                // Generate new data
                dataSimulator.RefreshData();

                // Update UI
                if (uiManager)
                {
                    uiManager.UpdateUI();
                }

                UpdateStatus();
            }));
        }

        private void UpdateStatus()
        {
            if (statusText)
            {
                string status = "Health Connect Status:\n";

                if (!isConnected)
                {
                    status += "Not connected. Please connect to Health Connect.";
                }
                else if (!hasPermissions)
                {
                    status += "Connected to Health Connect. Permissions required.";
                }
                else
                {
                    status += "Connected to Health Connect. Permissions granted. Ready to fetch data.";
                }

                statusText.text = status;
            }
        }

        private IEnumerator DelayedAction(float delay, System.Action action)
        {
            if (statusText)
            {
                statusText.text += "\nProcessing...";
            }

            yield return new WaitForSeconds(delay);

            action?.Invoke();
        }
    }
}
