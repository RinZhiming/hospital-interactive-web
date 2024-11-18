using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicalModel
{
    public string PatientName { get; set; }
    public DateTime CurrentDate { get; private set; } = DateTime.Now;
    public string DoctorName { get; set; }
    public List<Medical> Medicals { get; set; } = new();
    public string DoctorId { get; set; }
    public float TotalCost { get; set; }
    public AppointmentClone Appointment { get; set; }
}

[Serializable]
public class Medical
{
    public string name;
    public int number;
    public float cost;
}

[Serializable]
public class MedicalSummary
{ 
    public string patientId;
    public string patientName;
    public string doctorName;
    public string doctorId;
    public string dateTime;
    public string time;
    public float totalCost;
    public Medical[] medicals;
}