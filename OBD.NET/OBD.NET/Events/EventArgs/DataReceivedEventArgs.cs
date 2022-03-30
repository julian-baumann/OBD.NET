﻿using OBD.NET.OBDData;

namespace OBD.NET.Events.EventArgs;

public class DataReceivedEventArgs<T> where T : IOBDData
{
    #region Properties & Fields

    public T Data { get; }
    public DateTime Timestamp { get; }

    #endregion

    #region Constructors

    public DataReceivedEventArgs(T data, DateTime timestamp)
    {
        Data = data;
        Timestamp = timestamp;
    }

    #endregion
}