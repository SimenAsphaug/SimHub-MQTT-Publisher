using System;

namespace SimHub.MQTTPublisher
{
    /// <summary>
    /// Settings class, make sure it can be correctly serialized using JSON.net
    /// </summary>
    public class SimHubMQTTPublisherPluginSettings
    {
        public string Server { get; set; } = "localhost";

        public string Topic { get; set; } = "racing/driver_name";

        public string Login { get; set; } = "admin";

        public string Password { get; set; } = "admin";

        // === CAR STATE PROPERTIES ===
        public bool Include_SpeedKmh { get; set; } = true;
        public bool Include_Rpms { get; set; } = true;
        public bool Include_Gear { get; set; } = true;
        public bool Include_Throttle { get; set; } = true;
        public bool Include_Brake { get; set; } = true;
        public bool Include_Clutch { get; set; } = true;
        public bool Include_CarCoordinates { get; set; } = false;
        public bool Include_CurrentLapTime { get; set; } = true;
        public bool Include_CarModel { get; set; } = true;
        public bool Include_CarClass { get; set; } = true;
        public bool Include_EngineIgnitionOn { get; set; } = false;
        public bool Include_EngineStarted { get; set; } = false;

        // === FLAG INFORMATION ===
        public bool Include_Flags { get; set; } = true;
        public bool Include_FlagName { get; set; } = false;
        public bool Include_GameName { get; set; } = false;
        public bool Include_DebugFlags { get; set; } = false;

        // === POSITION & TIMING DATA ===
        public bool Include_Position { get; set; } = false;
        public bool Include_PositionInClass { get; set; } = false;
        public bool Include_Gap { get; set; } = false;
        public bool Include_GapToLeader { get; set; } = false;
        public bool Include_GapToAhead { get; set; } = false;
        public bool Include_GapToBehind { get; set; } = false;
        public bool Include_LastLapTime { get; set; } = false;
        public bool Include_BestLapTime { get; set; } = false;
        public bool Include_PersonalBestLapTime { get; set; } = false;
        public bool Include_SessionBestLapTime { get; set; } = false;
        public bool Include_DeltaToSessionBest { get; set; } = false;
        public bool Include_DeltaToPersonalBest { get; set; } = false;
        public bool Include_DeltaToOptimal { get; set; } = false;
        public bool Include_Sector1Time { get; set; } = false;
        public bool Include_Sector2Time { get; set; } = false;
        public bool Include_Sector3Time { get; set; } = false;
        public bool Include_Sector1BestTime { get; set; } = false;
        public bool Include_Sector2BestTime { get; set; } = false;
        public bool Include_Sector3BestTime { get; set; } = false;
        public bool Include_CurrentSector { get; set; } = false;
        public bool Include_CurrentLap { get; set; } = false;
        public bool Include_TotalLaps { get; set; } = false;
        public bool Include_CompletedLaps { get; set; } = false;

        // === TIRE DATA ===
        public bool Include_TireTemperatures { get; set; } = false;
        public bool Include_TirePressures { get; set; } = false;
        public bool Include_TireWear { get; set; } = false;
        public bool Include_TireGrip { get; set; } = false;
        public bool Include_TireCompound { get; set; } = false;
        public bool Include_TireDirt { get; set; } = false;

        // === FUEL & ENERGY DATA ===
        public bool Include_Fuel { get; set; } = false;
        public bool Include_FuelCapacity { get; set; } = false;
        public bool Include_FuelPerLap { get; set; } = false;
        public bool Include_FuelRemaining { get; set; } = false;
        public bool Include_FuelEstimatedLaps { get; set; } = false;
        public bool Include_FuelToEnd { get; set; } = false;
        public bool Include_ERS_Data { get; set; } = false;
        public bool Include_DRS_Data { get; set; } = false;
        public bool Include_BatteryData { get; set; } = false;

        // === WEATHER & CONDITIONS ===
        public bool Include_AirTemperature { get; set; } = false;
        public bool Include_TrackTemperature { get; set; } = false;
        public bool Include_WeatherType { get; set; } = false;
        public bool Include_RainLevel { get; set; } = false;
        public bool Include_Humidity { get; set; } = false;
        public bool Include_WindData { get; set; } = false;
        public bool Include_TrackGrip { get; set; } = false;
        public bool Include_TimeOfDay { get; set; } = false;

        // === DAMAGE & MECHANICAL ===
        public bool Include_CarDamage { get; set; } = false;
        public bool Include_EngineTemperatures { get; set; } = false;
        public bool Include_BrakeTemperatures { get; set; } = false;
        public bool Include_TurboData { get; set; } = false;
        public bool Include_WearIndicators { get; set; } = false;

        // === DRIVER INPUT ===
        public bool Include_SteeringInput { get; set; } = false;
        public bool Include_PedalInputs { get; set; } = false;
        public bool Include_DriverAssists { get; set; } = false;
        public bool Include_ElectronicSystems { get; set; } = false;
        public bool Include_InputDeviceInfo { get; set; } = false;

        // === SAFETY & RACE CONTROL ===
        public bool Include_SafetyCarInfo { get; set; } = false;
        public bool Include_FlagSectors { get; set; } = false;
        public bool Include_PitInformation { get; set; } = false;
        public bool Include_RaceControl { get; set; } = false;
        public bool Include_Penalties { get; set; } = false;
        public bool Include_FormationLap { get; set; } = false;

        // === TRACK INFORMATION ===
        public bool Include_TrackName { get; set; } = false;
        public bool Include_TrackLength { get; set; } = false;
        public bool Include_TrackConfiguration { get; set; } = false;

        // === VEHICLE INFORMATION ===
        public bool Include_VehicleModel { get; set; } = false;
        public bool Include_VehicleClass { get; set; } = false;
        public bool Include_MaxRpm { get; set; } = false;

        // === SESSION INFORMATION ===
        public bool Include_SessionType { get; set; } = false;
        public bool Include_SessionTimeLeft { get; set; } = false;
        public bool Include_SessionLaps { get; set; } = false;

        // === ADVANCED DEBUGGING ===
        public bool EnableDebugMode { get; set; } = false;
    }

    public class SimHubMQTTPublisherPluginUserSettings
    {
        public Guid UserId { get; set; } = Guid.NewGuid();

    }
}