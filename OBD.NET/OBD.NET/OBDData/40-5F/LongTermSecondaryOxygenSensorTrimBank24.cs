﻿using OBD.NET.DataTypes;

namespace OBD.NET.OBDData._40_5F;

public class LongTermSecondaryOxygenSensorTrimBank24 : AbstractOBDData
{
    #region Properties & Fields

    public Percent Bank2 => new Percent((A / 1.28) - 100, -100, 99.2);
    public Percent Bank4 => new Percent((B / 1.28) - 100, -100, 99.2);

    #endregion

    #region Constructors

    public LongTermSecondaryOxygenSensorTrimBank24()
        : base(0x58, 2)
    { }

    #endregion

    #region Methods

    public override string ToString() => Bank2.ToString();

    #endregion
}