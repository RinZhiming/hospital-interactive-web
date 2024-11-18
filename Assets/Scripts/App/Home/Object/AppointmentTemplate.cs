using System;

[Serializable]
public class Appointment : IComparable
{
    public int sort;
    public string time;
    public string date;
    public string channelName;
    public string doctorId;
    public string patientId;
    public bool hasAppointment;
    public bool hasEnd;
    
    public int CompareTo(object obj)
    {
        if (obj == null) return 1;

        if (obj is Appointment { } otherAppointmentEmpty)
        {
            return sort.CompareTo(otherAppointmentEmpty.sort);
        }
        
        throw new ArgumentException("Object is not a AppointmentEmpty");
    }
}

[Serializable]
public class AppointmentClone : IComparable<AppointmentClone>
{
    public int sort;
    public DateTime date;
    public TimeSpan startTime;
    public TimeSpan endTime;
    public string channelName;
    public string doctorId;
    public string patientId;
    public string key;
    public bool hasAppointment;
    public bool hasEnd;

    public int CompareTo(AppointmentClone other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        var dateComparison = date.CompareTo(other.date);
        if (dateComparison != 0) return dateComparison;
        return startTime.CompareTo(other.startTime);
    }
}