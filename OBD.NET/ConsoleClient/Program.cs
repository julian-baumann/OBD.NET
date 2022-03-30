﻿using OBD.NET.Communication;
using OBD.NET.Devices;
using OBD.NET.Extensions;
using OBD.NET.Logging;
using OBD.NET.OBDData;
using OBD.NET.OBDData._00_1F;
using OBD.NET.OBDData._40_5F;

namespace ConsoleClient;

public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("Parameter ComPort needed.");

            IEnumerable<string> availablePorts = SerialConnection.GetAvailablePorts();

            Console.WriteLine("\nAvailable ports:");

            foreach (string port in availablePorts)
            {
                Console.WriteLine(port);
            }

            return;
        }

        string comPort = args[0];

        using SerialConnection connection = new SerialConnection(comPort);
        using ELM327 dev = new ELM327(connection, new OBDConsoleLogger(OBDLogLevel.Debug));

        dev.SubscribeDataReceived<EngineRPM>((sender, data) => Console.WriteLine("EngineRPM: " + data.Data.Rpm));
        dev.SubscribeDataReceived<EngineFuelRate>((sender, data) => Console.WriteLine("VehicleSpeed: " + data.Data));

        dev.SubscribeDataReceived<IOBDData>((sender, data) => Console.WriteLine($"PID {data.Data.PID.ToHexString()}: {data.Data}"));

        dev.Initialize();
        dev.RequestData<FuelType>();

        for (int i = 0; i < 5; i++)
        {
            dev.RequestData<EngineRPM>();
            dev.RequestData<EngineFuelRate>();
            Thread.Sleep(1000);
        }

        Console.ReadLine();

        //Async example
        // MainAsync(comPort).Wait();

        //Console.ReadLine();
    }

    /// <summary>
    /// Async example using new RequestDataAsync
    /// </summary>
    /// <param name="comPort">The COM port.</param>
    /// <returns></returns>
    public static async Task MainAsync(string comPort)
    {
        using SerialConnection connection = new SerialConnection(comPort);
        using ELM327 dev = new ELM327(connection, new OBDConsoleLogger(OBDLogLevel.Debug));

        dev.Initialize();

        EngineRPM engineRpm = await dev.RequestDataAsync<EngineRPM>();
        Console.WriteLine("Data: " + engineRpm.Rpm);

        engineRpm = await dev.RequestDataAsync<EngineRPM>();
        Console.WriteLine("Data: " + engineRpm.Rpm);

        VehicleSpeed vehicleSpeed = await dev.RequestDataAsync<VehicleSpeed>();
        Console.WriteLine("Data: " + vehicleSpeed.Speed);

        engineRpm = await dev.RequestDataAsync<EngineRPM>();
        Console.WriteLine("Data: " + engineRpm.Rpm);
    }
}