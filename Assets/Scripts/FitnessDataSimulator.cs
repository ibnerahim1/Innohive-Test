using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DailyFitnessData
{
    public string date;
    public int stepCount;
    public float distanceKm;

    public DailyFitnessData(string date, int stepCount, float distanceKm)
    {
        this.date = date;
        this.stepCount = stepCount;
        this.distanceKm = distanceKm;
    }
}

public class FitnessDataSimulator : MonoBehaviour
{
    [SerializeField] private bool useRandomData = true;
    [SerializeField] private bool includeToday = true;

    public List<DailyFitnessData> fitnessData = new List<DailyFitnessData>();

    // Statistics
    public int totalSteps { get; private set; }
    public float totalDistance { get; private set; }
    public float avgSteps { get; private set; }
    public float avgDistance { get; private set; }

    private void Awake()
    {
        GenerateSimulatedData();
        CalculateStatistics();
    }

    public void GenerateSimulatedData()
    {
        fitnessData.Clear();

        // Generate data for last 7 days
        DateTime currentDate = DateTime.Now;
        System.Random random = new System.Random();

        for (int i = 6; i >= 0; i--)
        {
            DateTime date;
            if (includeToday || i > 0)
            {
                date = currentDate.AddDays(-i);
            }
            else
            {
                // Skip today if not included
                continue;
            }

            int steps;
            float distance;

            if (useRandomData)
            {
                // Generate random data between realistic ranges
                // Average person walks 5,000-10,000 steps daily
                steps = random.Next(3000, 15000);

                // Average stride length is about 0.7m, so we can calculate approximate distance
                // Adding some variation for realism
                float strideVariation = (float)random.NextDouble() * 0.2f + 0.6f; // 0.6-0.8m stride
                distance = (steps * strideVariation) / 1000f; // Convert to kilometers
            }
            else
            {
                // Use predefined data for demonstration
                switch (i)
                {
                    case 6: steps = 8523; distance = 6.2f; break;
                    case 5: steps = 10234; distance = 7.5f; break;
                    case 4: steps = 7652; distance = 5.6f; break;
                    case 3: steps = 9876; distance = 7.2f; break;
                    case 2: steps = 5432; distance = 4.0f; break;
                    case 1: steps = 12543; distance = 9.1f; break;
                    case 0: steps = 6789; distance = 5.0f; break;
                    default: steps = 0; distance = 0; break;
                }
            }

            // Format date as day of week and day number (e.g., "Mon 15")
            string dateStr = date.ToString("ddd dd");

            // Add to our data list
            fitnessData.Add(new DailyFitnessData(dateStr, steps, distance));
        }
    }

    private void CalculateStatistics()
    {
        totalSteps = 0;
        totalDistance = 0;

        foreach (var day in fitnessData)
        {
            totalSteps += day.stepCount;
            totalDistance += day.distanceKm;
        }

        avgSteps = fitnessData.Count > 0 ? totalSteps / fitnessData.Count : 0;
        avgDistance = fitnessData.Count > 0 ? totalDistance / fitnessData.Count : 0;
    }

    public void RefreshData()
    {
        GenerateSimulatedData();
        CalculateStatistics();
    }
}