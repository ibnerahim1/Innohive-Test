using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FitnessApp
{
    public class FitnessUIManager : MonoBehaviour
    {
        public FitnessDataSimulator dataSimulator;
        public Transform dataContainer;
        public GameObject dayDataPrefab;
        public Text titleText;
        public Text totalStepsText;
        public Text totalDistanceText;
        public Button refreshButton;
        
        private List<GameObject> dataItems = new List<GameObject>();
        
        void Start()
        {
            if (dataSimulator == null)
            {
                dataSimulator = FindObjectOfType<FitnessDataSimulator>();
            }
            
            if (refreshButton)
            {
                refreshButton.onClick.AddListener(RefreshData);
            }
            
            UpdateUI();
        }
        
        public void RefreshData()
        {
            dataSimulator.RefreshData();
            UpdateUI();
        }
        
        public void UpdateUI()
        {
            // Clear existing items
            foreach (var item in dataItems)
            {
                Destroy(item);
            }
            dataItems.Clear();
            
            // Update title
            if (titleText)
            {
                titleText.text = "Fitness Data - Last 7 Days";
            }
            
            // Create data items
            if (dataContainer && dayDataPrefab)
            {
                foreach (var dayData in dataSimulator.fitnessData)
                {
                    GameObject newItem = Instantiate(dayDataPrefab, dataContainer);
                    dataItems.Add(newItem);
                    
                    // Get references to text components
                    Text[] texts = newItem.GetComponentsInChildren<Text>();
                    if (texts.Length >= 3)
                    {
                        texts[0].text = dayData.date;
                        texts[1].text = dayData.stepCount.ToString() + " steps";
                        texts[2].text = dayData.distanceKm.ToString("F1") + " km";
                    }
                }
            }
            
            // Update statistics
            if (totalStepsText)
            {
                totalStepsText.text = "Total Steps: " + dataSimulator.totalSteps.ToString();
            }
            
            if (totalDistanceText)
            {
                totalDistanceText.text = "Total Distance: " + dataSimulator.totalDistance.ToString("F1") + " km";
            }
        }
    }
}
