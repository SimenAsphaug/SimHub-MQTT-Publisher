using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SimHub.MQTTPublisher.ViewModels
{
    internal class SimHubMQTTPublisherPluginUIModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _server;

        private string _topic;

        private string _login;

        private string _password;

        private Guid _UserId;

        // === CAR STATE PROPERTIES ===
        private bool _include_SpeedKmh = true;
        private bool _include_Rpms = true;
        private bool _include_Gear = true;
        private bool _include_Throttle = true;
        private bool _include_Brake = true;
        private bool _include_Clutch = true;
        private bool _include_CarCoordinates = false;
        private bool _include_CurrentLapTime = true;
        private bool _include_CarModel = true;
        private bool _include_CarClass = true;
        private bool _include_EngineIgnitionOn = false;
        private bool _include_EngineStarted = false;

        // === FLAG INFORMATION ===
        private bool _include_Flags = true;
        private bool _include_FlagName = false;
        private bool _include_GameName = false;
        private bool _include_DebugFlags = false;

        // === POSITION & TIMING DATA ===
        private bool _include_Position = false;
        private bool _include_PositionInClass = false;
        private bool _include_Gap = false;
        private bool _include_GapToLeader = false;
        private bool _include_GapToAhead = false;
        private bool _include_GapToBehind = false;
        private bool _include_LastLapTime = false;
        private bool _include_BestLapTime = false;
        private bool _include_PersonalBestLapTime = false;
        private bool _include_SessionBestLapTime = false;
        private bool _include_DeltaToSessionBest = false;
        private bool _include_DeltaToPersonalBest = false;
        private bool _include_DeltaToOptimal = false;
        private bool _include_Sector1Time = false;
        private bool _include_Sector2Time = false;
        private bool _include_Sector3Time = false;
        private bool _include_Sector1BestTime = false;
        private bool _include_Sector2BestTime = false;
        private bool _include_Sector3BestTime = false;
        private bool _include_CurrentSector = false;
        private bool _include_CurrentLap = false;
        private bool _include_TotalLaps = false;
        private bool _include_CompletedLaps = false;

        // === TIRE DATA ===
        private bool _include_TireTemperatures = false;
        private bool _include_TirePressures = false;
        private bool _include_TireWear = false;
        private bool _include_TireGrip = false;
        private bool _include_TireCompound = false;
        private bool _include_TireDirt = false;

        // === FUEL & ENERGY DATA ===
        private bool _include_Fuel = false;
        private bool _include_FuelCapacity = false;
        private bool _include_FuelPerLap = false;
        private bool _include_FuelRemaining = false;
        private bool _include_FuelEstimatedLaps = false;
        private bool _include_FuelToEnd = false;
        private bool _include_ERS_Data = false;
        private bool _include_DRS_Data = false;
        private bool _include_BatteryData = false;

        // === WEATHER & CONDITIONS ===
        private bool _include_AirTemperature = false;
        private bool _include_TrackTemperature = false;
        private bool _include_WeatherType = false;
        private bool _include_RainLevel = false;
        private bool _include_Humidity = false;
        private bool _include_WindData = false;
        private bool _include_TrackGrip = false;
        private bool _include_TimeOfDay = false;

        // === DAMAGE & MECHANICAL ===
        private bool _include_CarDamage = false;
        private bool _include_EngineTemperatures = false;
        private bool _include_BrakeTemperatures = false;
        private bool _include_TurboData = false;
        private bool _include_WearIndicators = false;

        // === DRIVER INPUT ===
        private bool _include_SteeringInput = false;
        private bool _include_PedalInputs = false;
        private bool _include_DriverAssists = false;
        private bool _include_ElectronicSystems = false;
        private bool _include_InputDeviceInfo = false;

        // === SAFETY & RACE CONTROL ===
        private bool _include_SafetyCarInfo = false;
        private bool _include_FlagSectors = false;
        private bool _include_PitInformation = false;
        private bool _include_RaceControl = false;
        private bool _include_Penalties = false;
        private bool _include_FormationLap = false;

        // === TRACK INFORMATION ===
        private bool _include_TrackName = false;
        private bool _include_TrackLength = false;
        private bool _include_TrackConfiguration = false;

        // === VEHICLE INFORMATION ===
        private bool _include_VehicleModel = false;
        private bool _include_VehicleClass = false;
        private bool _include_MaxRpm = false;

        // === SESSION INFORMATION ===
        private bool _include_SessionType = false;
        private bool _include_SessionTimeLeft = false;
        private bool _include_SessionLaps = false;

        // === ADVANCED DEBUGGING ===
        private bool _enableDebugMode = false;

        public string Server
        {
            get => _server;
            set
            {
                _server = value;
                OnPropertyChanged();
            }
        }

        public string Topic
        {
            get => _topic;
            set
            {
                _topic = value;
                OnPropertyChanged();
            }
        }

        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public Guid UserId
        {
            get => _UserId;
            set
            {
                _UserId = value;
                OnPropertyChanged();
            }
        }

        // === CAR STATE PROPERTIES ===
        public bool Include_SpeedKmh
        {
            get => _include_SpeedKmh;
            set
            {
                _include_SpeedKmh = value;
                OnPropertyChanged();
            }
        }

        public bool Include_Rpms
        {
            get => _include_Rpms;
            set
            {
                _include_Rpms = value;
                OnPropertyChanged();
            }
        }

        public bool Include_Gear
        {
            get => _include_Gear;
            set
            {
                _include_Gear = value;
                OnPropertyChanged();
            }
        }

        public bool Include_Throttle
        {
            get => _include_Throttle;
            set
            {
                _include_Throttle = value;
                OnPropertyChanged();
            }
        }

        public bool Include_Brake
        {
            get => _include_Brake;
            set
            {
                _include_Brake = value;
                OnPropertyChanged();
            }
        }

        public bool Include_Clutch
        {
            get => _include_Clutch;
            set
            {
                _include_Clutch = value;
                OnPropertyChanged();
            }
        }

        public bool Include_CarCoordinates
        {
            get => _include_CarCoordinates;
            set
            {
                _include_CarCoordinates = value;
                OnPropertyChanged();
            }
        }

        public bool Include_CurrentLapTime
        {
            get => _include_CurrentLapTime;
            set
            {
                _include_CurrentLapTime = value;
                OnPropertyChanged();
            }
        }

        public bool Include_CarModel
        {
            get => _include_CarModel;
            set
            {
                _include_CarModel = value;
                OnPropertyChanged();
            }
        }

        public bool Include_CarClass
        {
            get => _include_CarClass;
            set
            {
                _include_CarClass = value;
                OnPropertyChanged();
            }
        }

        public bool Include_EngineIgnitionOn
        {
            get => _include_EngineIgnitionOn;
            set
            {
                _include_EngineIgnitionOn = value;
                OnPropertyChanged();
            }
        }

        public bool Include_EngineStarted
        {
            get => _include_EngineStarted;
            set
            {
                _include_EngineStarted = value;
                OnPropertyChanged();
            }
        }

        // === FLAG INFORMATION ===
        public bool Include_Flags
        {
            get => _include_Flags;
            set
            {
                _include_Flags = value;
                OnPropertyChanged();
            }
        }

        public bool Include_FlagName
        {
            get => _include_FlagName;
            set
            {
                _include_FlagName = value;
                OnPropertyChanged();
            }
        }

        public bool Include_GameName
        {
            get => _include_GameName;
            set
            {
                _include_GameName = value;
                OnPropertyChanged();
            }
        }

        public bool Include_DebugFlags
        {
            get => _include_DebugFlags;
            set
            {
                _include_DebugFlags = value;
                OnPropertyChanged();
            }
        }

        // === POSITION & TIMING DATA ===
        public bool Include_Position
        {
            get => _include_Position;
            set
            {
                _include_Position = value;
                OnPropertyChanged();
            }
        }

        public bool Include_PositionInClass
        {
            get => _include_PositionInClass;
            set
            {
                _include_PositionInClass = value;
                OnPropertyChanged();
            }
        }

        public bool Include_Gap
        {
            get => _include_Gap;
            set
            {
                _include_Gap = value;
                OnPropertyChanged();
            }
        }

        public bool Include_GapToLeader
        {
            get => _include_GapToLeader;
            set
            {
                _include_GapToLeader = value;
                OnPropertyChanged();
            }
        }

        public bool Include_GapToAhead
        {
            get => _include_GapToAhead;
            set
            {
                _include_GapToAhead = value;
                OnPropertyChanged();
            }
        }

        public bool Include_GapToBehind
        {
            get => _include_GapToBehind;
            set
            {
                _include_GapToBehind = value;
                OnPropertyChanged();
            }
        }

        public bool Include_LastLapTime
        {
            get => _include_LastLapTime;
            set
            {
                _include_LastLapTime = value;
                OnPropertyChanged();
            }
        }

        public bool Include_BestLapTime
        {
            get => _include_BestLapTime;
            set
            {
                _include_BestLapTime = value;
                OnPropertyChanged();
            }
        }

        public bool Include_PersonalBestLapTime
        {
            get => _include_PersonalBestLapTime;
            set
            {
                _include_PersonalBestLapTime = value;
                OnPropertyChanged();
            }
        }

        public bool Include_SessionBestLapTime
        {
            get => _include_SessionBestLapTime;
            set
            {
                _include_SessionBestLapTime = value;
                OnPropertyChanged();
            }
        }

        public bool Include_DeltaToSessionBest
        {
            get => _include_DeltaToSessionBest;
            set
            {
                _include_DeltaToSessionBest = value;
                OnPropertyChanged();
            }
        }

        public bool Include_DeltaToPersonalBest
        {
            get => _include_DeltaToPersonalBest;
            set
            {
                _include_DeltaToPersonalBest = value;
                OnPropertyChanged();
            }
        }

        public bool Include_DeltaToOptimal
        {
            get => _include_DeltaToOptimal;
            set
            {
                _include_DeltaToOptimal = value;
                OnPropertyChanged();
            }
        }

        public bool Include_Sector1Time
        {
            get => _include_Sector1Time;
            set
            {
                _include_Sector1Time = value;
                OnPropertyChanged();
            }
        }

        public bool Include_Sector2Time
        {
            get => _include_Sector2Time;
            set
            {
                _include_Sector2Time = value;
                OnPropertyChanged();
            }
        }

        public bool Include_Sector3Time
        {
            get => _include_Sector3Time;
            set
            {
                _include_Sector3Time = value;
                OnPropertyChanged();
            }
        }

        public bool Include_Sector1BestTime
        {
            get => _include_Sector1BestTime;
            set
            {
                _include_Sector1BestTime = value;
                OnPropertyChanged();
            }
        }

        public bool Include_Sector2BestTime
        {
            get => _include_Sector2BestTime;
            set
            {
                _include_Sector2BestTime = value;
                OnPropertyChanged();
            }
        }

        public bool Include_Sector3BestTime
        {
            get => _include_Sector3BestTime;
            set
            {
                _include_Sector3BestTime = value;
                OnPropertyChanged();
            }
        }

        public bool Include_CurrentSector
        {
            get => _include_CurrentSector;
            set
            {
                _include_CurrentSector = value;
                OnPropertyChanged();
            }
        }

        public bool Include_CurrentLap
        {
            get => _include_CurrentLap;
            set
            {
                _include_CurrentLap = value;
                OnPropertyChanged();
            }
        }

        public bool Include_TotalLaps
        {
            get => _include_TotalLaps;
            set
            {
                _include_TotalLaps = value;
                OnPropertyChanged();
            }
        }

        public bool Include_CompletedLaps
        {
            get => _include_CompletedLaps;
            set
            {
                _include_CompletedLaps = value;
                OnPropertyChanged();
            }
        }

        // === TIRE DATA ===
        public bool Include_TireTemperatures
        {
            get => _include_TireTemperatures;
            set
            {
                _include_TireTemperatures = value;
                OnPropertyChanged();
            }
        }

        public bool Include_TirePressures
        {
            get => _include_TirePressures;
            set
            {
                _include_TirePressures = value;
                OnPropertyChanged();
            }
        }

        public bool Include_TireWear
        {
            get => _include_TireWear;
            set
            {
                _include_TireWear = value;
                OnPropertyChanged();
            }
        }

        public bool Include_TireGrip
        {
            get => _include_TireGrip;
            set
            {
                _include_TireGrip = value;
                OnPropertyChanged();
            }
        }

        public bool Include_TireCompound
        {
            get => _include_TireCompound;
            set
            {
                _include_TireCompound = value;
                OnPropertyChanged();
            }
        }

        public bool Include_TireDirt
        {
            get => _include_TireDirt;
            set
            {
                _include_TireDirt = value;
                OnPropertyChanged();
            }
        }

        // === FUEL & ENERGY DATA ===
        public bool Include_Fuel
        {
            get => _include_Fuel;
            set
            {
                _include_Fuel = value;
                OnPropertyChanged();
            }
        }

        public bool Include_FuelCapacity
        {
            get => _include_FuelCapacity;
            set
            {
                _include_FuelCapacity = value;
                OnPropertyChanged();
            }
        }

        public bool Include_FuelPerLap
        {
            get => _include_FuelPerLap;
            set
            {
                _include_FuelPerLap = value;
                OnPropertyChanged();
            }
        }

        public bool Include_FuelRemaining
        {
            get => _include_FuelRemaining;
            set
            {
                _include_FuelRemaining = value;
                OnPropertyChanged();
            }
        }

        public bool Include_FuelEstimatedLaps
        {
            get => _include_FuelEstimatedLaps;
            set
            {
                _include_FuelEstimatedLaps = value;
                OnPropertyChanged();
            }
        }

        public bool Include_FuelToEnd
        {
            get => _include_FuelToEnd;
            set
            {
                _include_FuelToEnd = value;
                OnPropertyChanged();
            }
        }

        public bool Include_ERS_Data
        {
            get => _include_ERS_Data;
            set
            {
                _include_ERS_Data = value;
                OnPropertyChanged();
            }
        }

        public bool Include_DRS_Data
        {
            get => _include_DRS_Data;
            set
            {
                _include_DRS_Data = value;
                OnPropertyChanged();
            }
        }

        public bool Include_BatteryData
        {
            get => _include_BatteryData;
            set
            {
                _include_BatteryData = value;
                OnPropertyChanged();
            }
        }

        // === WEATHER & CONDITIONS ===
        public bool Include_AirTemperature
        {
            get => _include_AirTemperature;
            set
            {
                _include_AirTemperature = value;
                OnPropertyChanged();
            }
        }

        public bool Include_TrackTemperature
        {
            get => _include_TrackTemperature;
            set
            {
                _include_TrackTemperature = value;
                OnPropertyChanged();
            }
        }

        public bool Include_WeatherType
        {
            get => _include_WeatherType;
            set
            {
                _include_WeatherType = value;
                OnPropertyChanged();
            }
        }

        public bool Include_RainLevel
        {
            get => _include_RainLevel;
            set
            {
                _include_RainLevel = value;
                OnPropertyChanged();
            }
        }

        public bool Include_Humidity
        {
            get => _include_Humidity;
            set
            {
                _include_Humidity = value;
                OnPropertyChanged();
            }
        }

        public bool Include_WindData
        {
            get => _include_WindData;
            set
            {
                _include_WindData = value;
                OnPropertyChanged();
            }
        }

        public bool Include_TrackGrip
        {
            get => _include_TrackGrip;
            set
            {
                _include_TrackGrip = value;
                OnPropertyChanged();
            }
        }

        public bool Include_TimeOfDay
        {
            get => _include_TimeOfDay;
            set
            {
                _include_TimeOfDay = value;
                OnPropertyChanged();
            }
        }

        // === DAMAGE & MECHANICAL ===
        public bool Include_CarDamage
        {
            get => _include_CarDamage;
            set
            {
                _include_CarDamage = value;
                OnPropertyChanged();
            }
        }

        public bool Include_EngineTemperatures
        {
            get => _include_EngineTemperatures;
            set
            {
                _include_EngineTemperatures = value;
                OnPropertyChanged();
            }
        }

        public bool Include_BrakeTemperatures
        {
            get => _include_BrakeTemperatures;
            set
            {
                _include_BrakeTemperatures = value;
                OnPropertyChanged();
            }
        }

        public bool Include_TurboData
        {
            get => _include_TurboData;
            set
            {
                _include_TurboData = value;
                OnPropertyChanged();
            }
        }

        public bool Include_WearIndicators
        {
            get => _include_WearIndicators;
            set
            {
                _include_WearIndicators = value;
                OnPropertyChanged();
            }
        }

        // === DRIVER INPUT ===
        public bool Include_SteeringInput
        {
            get => _include_SteeringInput;
            set
            {
                _include_SteeringInput = value;
                OnPropertyChanged();
            }
        }

        public bool Include_PedalInputs
        {
            get => _include_PedalInputs;
            set
            {
                _include_PedalInputs = value;
                OnPropertyChanged();
            }
        }

        public bool Include_DriverAssists
        {
            get => _include_DriverAssists;
            set
            {
                _include_DriverAssists = value;
                OnPropertyChanged();
            }
        }

        public bool Include_ElectronicSystems
        {
            get => _include_ElectronicSystems;
            set
            {
                _include_ElectronicSystems = value;
                OnPropertyChanged();
            }
        }

        public bool Include_InputDeviceInfo
        {
            get => _include_InputDeviceInfo;
            set
            {
                _include_InputDeviceInfo = value;
                OnPropertyChanged();
            }
        }

        // === SAFETY & RACE CONTROL ===
        public bool Include_SafetyCarInfo
        {
            get => _include_SafetyCarInfo;
            set
            {
                _include_SafetyCarInfo = value;
                OnPropertyChanged();
            }
        }

        public bool Include_FlagSectors
        {
            get => _include_FlagSectors;
            set
            {
                _include_FlagSectors = value;
                OnPropertyChanged();
            }
        }

        public bool Include_PitInformation
        {
            get => _include_PitInformation;
            set
            {
                _include_PitInformation = value;
                OnPropertyChanged();
            }
        }

        public bool Include_RaceControl
        {
            get => _include_RaceControl;
            set
            {
                _include_RaceControl = value;
                OnPropertyChanged();
            }
        }

        public bool Include_Penalties
        {
            get => _include_Penalties;
            set
            {
                _include_Penalties = value;
                OnPropertyChanged();
            }
        }

        public bool Include_FormationLap
        {
            get => _include_FormationLap;
            set
            {
                _include_FormationLap = value;
                OnPropertyChanged();
            }
        }

        // === TRACK INFORMATION ===
        public bool Include_TrackName
        {
            get => _include_TrackName;
            set
            {
                _include_TrackName = value;
                OnPropertyChanged();
            }
        }

        public bool Include_TrackLength
        {
            get => _include_TrackLength;
            set
            {
                _include_TrackLength = value;
                OnPropertyChanged();
            }
        }

        public bool Include_TrackConfiguration
        {
            get => _include_TrackConfiguration;
            set
            {
                _include_TrackConfiguration = value;
                OnPropertyChanged();
            }
        }

        // === VEHICLE INFORMATION ===
        public bool Include_VehicleModel
        {
            get => _include_VehicleModel;
            set
            {
                _include_VehicleModel = value;
                OnPropertyChanged();
            }
        }

        public bool Include_VehicleClass
        {
            get => _include_VehicleClass;
            set
            {
                _include_VehicleClass = value;
                OnPropertyChanged();
            }
        }

        public bool Include_MaxRpm
        {
            get => _include_MaxRpm;
            set
            {
                _include_MaxRpm = value;
                OnPropertyChanged();
            }
        }

        // === SESSION INFORMATION ===
        public bool Include_SessionType
        {
            get => _include_SessionType;
            set
            {
                _include_SessionType = value;
                OnPropertyChanged();
            }
        }

        public bool Include_SessionTimeLeft
        {
            get => _include_SessionTimeLeft;
            set
            {
                _include_SessionTimeLeft = value;
                OnPropertyChanged();
            }
        }

        public bool Include_SessionLaps
        {
            get => _include_SessionLaps;
            set
            {
                _include_SessionLaps = value;
                OnPropertyChanged();
            }
        }

        // === ADVANCED DEBUGGING ===
        public bool EnableDebugMode
        {
            get => _enableDebugMode;
            set
            {
                _enableDebugMode = value;
                OnPropertyChanged();
            }
        }

        // Plugin information properties
        public string PluginTitle
        {
            get
            {
                var assembly = Assembly.GetExecutingAssembly();
                var titleAttribute = assembly.GetCustomAttribute<AssemblyTitleAttribute>();
                return titleAttribute?.Title ?? "SimHub MQTT Publisher";
            }
        }

        public string PluginVersion
        {
            get
            {
                var assembly = Assembly.GetExecutingAssembly();
                var version = assembly.GetName().Version;
                return $"Version {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            }
        }

        public string PluginCopyright
        {
            get
            {
                var assembly = Assembly.GetExecutingAssembly();
                var copyrightAttribute = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>();
                return copyrightAttribute?.Copyright ?? "Copyright © 2025";
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}