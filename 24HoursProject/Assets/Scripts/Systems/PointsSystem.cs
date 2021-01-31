using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*********************************************************************************************
 * Points System
 * Author: Muniz
 * Created: 22/01/2021 : dd/mm/yyyy
 * Date Last Update: 22/01/2021
 * 
 * Class created  with the goal of handling all types of point systems 
 * 
 * *******************************************************************************************/



/// <summary>
/// This class handles any type of Point System. Use available Constructors. The Class is entire based on int values
/// </summary>
[Serializable]
public class PointsSystem
{
   
    public int maxPoints { get; private set; }
    public int currentPoints;
    int startingPoints;
    public event EventHandler<OnPointsDataEventArgs> OnPointsDecreased;
    public event EventHandler<OnPointsDataEventArgs> OnPointsIncreased;
    public event EventHandler<OnPointsDataEventArgs> OnPointsChanged;
    public event EventHandler<OnPointsDataEventArgs> OnPointsZero;
    public event EventHandler<OnPointsDataEventArgs> OnPointsMax;

    /// <summary>
    /// Class to store data to use in the events triggers
    /// </summary>
    public class OnPointsDataEventArgs : EventArgs
    {
        public int CurrentPointsEventArgs;

    }

    /// <summary>
    /// Constructor that requires <paramref name="maxpoints"/> and <paramref name="startingpoints"/> to be set
    /// <para>(<paramref name="maxpoints"/> = Int32.Max and <paramref name="startingpoints"/> = 0 by default)</para>
    /// </summary>
    /// <param name="maxpoints"> The max set of points which this instance will have.</param>
    /// <param name="startingpoints">The starting points of this instance.</param>
    public PointsSystem(int maxpoints = Int32.MaxValue, int startingpoints = 0)
    {
        maxPoints = maxpoints;
        currentPoints = startingpoints;
        startingPoints = startingpoints;
    }


    /// <summary>Removes X (<paramref name="value"/>) points from this PointSystem's Current Points.</summary>
    /// <remarks>  
    /// If, after the calculation, the amount
    /// of points is > 0 then it  triggers OnPointsDecreased and OnPointsChanged events. If, after the calculation, the amount
    /// of points is <= 0 then it  triggers OnPointsDecreased, OnPointsChanged and OnPointsZero events.
    ///</remarks>
    /// <param name="value">value to be subtracted from this PointSystem 's Current Points.</param> 
    public void RemovePoints(int value)
    {
        int aux = currentPoints - value; 

        if (aux > 0)
        {

            currentPoints = aux;
            OnPointsDataEventArgs EventArgsData = new OnPointsDataEventArgs { CurrentPointsEventArgs = currentPoints };
            OnPointsDecreased?.Invoke(this, EventArgsData);
            OnPointsChanged?.Invoke(this, EventArgsData);

        }

        else
        {
            currentPoints = 0;
            OnPointsDataEventArgs EventArgsData = new OnPointsDataEventArgs { CurrentPointsEventArgs = currentPoints };
            OnPointsDecreased?.Invoke(this, EventArgsData);
            OnPointsChanged?.Invoke(this, EventArgsData);
            OnPointsZero?.Invoke(this, EventArgsData);

        }
    }

    /// <summary>Adds X (<paramref name="value"/>) points to this PointSystem's Current Points.</summary>
    /// <remarks>
    /// If, after the calculation, the amount
    /// of points is < MaxPoints, then it  triggers OnPointsIncreased and OnPointsChanged events. If, after the calculation, the amount
    /// of points is >= MaxPoints, then it  triggers OnPointsIncreased, OnPointsChanged and OnPointsMax events.
    ///</remarks>
    /// <param name="value">value to be summed to this PointSystem's Current Points.</param>
    public void AddValue(int value)
    {
        int aux = currentPoints + value;// Propriedade auxiliar para não precisar repetir função
        if (aux >= maxPoints)
        {
            currentPoints = maxPoints;
            OnPointsDataEventArgs EventArgsData = new OnPointsDataEventArgs { CurrentPointsEventArgs = currentPoints };
            OnPointsMax?.Invoke(this, EventArgsData);

            OnPointsIncreased?.Invoke(this, EventArgsData);
            OnPointsChanged?.Invoke(this, EventArgsData);

        }
        else
        {
            currentPoints = aux;
            OnPointsDataEventArgs EventArgsData = new OnPointsDataEventArgs { CurrentPointsEventArgs = currentPoints };
            OnPointsIncreased?.Invoke(this, EventArgsData);
            OnPointsChanged?.Invoke(this, EventArgsData);
        }

    }
    /// <summary>
    /// Set CurrentPoints variable to 0
    /// </summary>
    /// <remarks>Triggers the OnPointsChanged and OnPointsZero events</remarks>
    public void ResetPoints()
    {
        currentPoints = startingPoints;


        OnPointsDataEventArgs EventArgsData = new OnPointsDataEventArgs { CurrentPointsEventArgs = currentPoints };
        OnPointsChanged?.Invoke(this, EventArgsData);
        OnPointsZero?.Invoke(this, EventArgsData);

    }
    /// <summary>
    /// Returns the current amout of points
    /// </summary>
    /// <returns>Current points of this instance</returns>
    public int GetCurrentPoints()
    {
        return currentPoints;
    }
    /// <summary>
    /// Return current points percentage based on MaxPoints of the instance
    /// </summary>
    /// <returns>
    /// Returns a float value between 0 and 1
    /// </returns>
    public float GetPointsPercentage()
    {
        return (float)currentPoints / maxPoints;
    }



}
