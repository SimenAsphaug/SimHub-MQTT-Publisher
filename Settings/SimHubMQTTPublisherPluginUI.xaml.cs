using SimHub.MQTTPublisher.ViewModels;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Microsoft.Win32;
using System.IO;
using Newtonsoft.Json;
using System.Threading;
using System.Windows.Navigation;

namespace SimHub.MQTTPublisher.Settings
{
    /// <summary>
    /// Logique d'interaction pour SimHubMQTTPublisherPluginUI.xaml
    /// </summary>
    public partial class SimHubMQTTPublisherPluginUI : UserControl
    {
        public SimHubMQTTPublisherPluginUI(SimHubMQTTPublisherPlugin simHubMQTTPublisherPlugin)
        {
            InitializeComponent();
            SimHubMQTTPublisherPlugin = simHubMQTTPublisherPlugin;

            this.Model = new SimHubMQTTPublisherPluginUIModel()
            {
                Server = simHubMQTTPublisherPlugin.Settings.Server,
                Topic = simHubMQTTPublisherPlugin.Settings.Topic,
                Login = simHubMQTTPublisherPlugin.Settings.Login,
                Password = simHubMQTTPublisherPlugin.Settings.Password,
                UserId = simHubMQTTPublisherPlugin.UserSettings.UserId,

                // === CAR STATE PROPERTIES ===
                Include_SpeedKmh = simHubMQTTPublisherPlugin.Settings.Include_SpeedKmh,
                Include_Rpms = simHubMQTTPublisherPlugin.Settings.Include_Rpms,
                Include_Gear = simHubMQTTPublisherPlugin.Settings.Include_Gear,
                Include_Throttle = simHubMQTTPublisherPlugin.Settings.Include_Throttle,
                Include_Brake = simHubMQTTPublisherPlugin.Settings.Include_Brake,
                Include_Clutch = simHubMQTTPublisherPlugin.Settings.Include_Clutch,
                Include_CarCoordinates = simHubMQTTPublisherPlugin.Settings.Include_CarCoordinates,
                Include_CurrentLapTime = simHubMQTTPublisherPlugin.Settings.Include_CurrentLapTime,
                Include_CarModel = simHubMQTTPublisherPlugin.Settings.Include_CarModel,
                Include_CarClass = simHubMQTTPublisherPlugin.Settings.Include_CarClass,
                Include_EngineIgnitionOn = simHubMQTTPublisherPlugin.Settings.Include_EngineIgnitionOn,
                Include_EngineStarted = simHubMQTTPublisherPlugin.Settings.Include_EngineStarted,

                // === FLAG INFORMATION ===
                Include_Flags = simHubMQTTPublisherPlugin.Settings.Include_Flags,
                Include_FlagName = simHubMQTTPublisherPlugin.Settings.Include_FlagName,
                Include_GameName = simHubMQTTPublisherPlugin.Settings.Include_GameName,
                Include_DebugFlags = simHubMQTTPublisherPlugin.Settings.Include_DebugFlags,

                // === POSITION & TIMING DATA ===
                Include_Position = simHubMQTTPublisherPlugin.Settings.Include_Position,
                Include_PositionInClass = simHubMQTTPublisherPlugin.Settings.Include_PositionInClass,
                Include_Gap = simHubMQTTPublisherPlugin.Settings.Include_Gap,
                Include_GapToLeader = simHubMQTTPublisherPlugin.Settings.Include_GapToLeader,
                Include_GapToAhead = simHubMQTTPublisherPlugin.Settings.Include_GapToAhead,
                Include_GapToBehind = simHubMQTTPublisherPlugin.Settings.Include_GapToBehind,
                Include_LastLapTime = simHubMQTTPublisherPlugin.Settings.Include_LastLapTime,
                Include_BestLapTime = simHubMQTTPublisherPlugin.Settings.Include_BestLapTime,
                Include_PersonalBestLapTime = simHubMQTTPublisherPlugin.Settings.Include_PersonalBestLapTime,
                Include_SessionBestLapTime = simHubMQTTPublisherPlugin.Settings.Include_SessionBestLapTime,
                Include_DeltaToSessionBest = simHubMQTTPublisherPlugin.Settings.Include_DeltaToSessionBest,
                Include_DeltaToPersonalBest = simHubMQTTPublisherPlugin.Settings.Include_DeltaToPersonalBest,
                Include_DeltaToOptimal = simHubMQTTPublisherPlugin.Settings.Include_DeltaToOptimal,
                Include_Sector1Time = simHubMQTTPublisherPlugin.Settings.Include_Sector1Time,
                Include_Sector2Time = simHubMQTTPublisherPlugin.Settings.Include_Sector2Time,
                Include_Sector3Time = simHubMQTTPublisherPlugin.Settings.Include_Sector3Time,
                Include_Sector1BestTime = simHubMQTTPublisherPlugin.Settings.Include_Sector1BestTime,
                Include_Sector2BestTime = simHubMQTTPublisherPlugin.Settings.Include_Sector2BestTime,
                Include_Sector3BestTime = simHubMQTTPublisherPlugin.Settings.Include_Sector3BestTime,
                Include_CurrentSector = simHubMQTTPublisherPlugin.Settings.Include_CurrentSector,
                Include_CurrentLap = simHubMQTTPublisherPlugin.Settings.Include_CurrentLap,
                Include_TotalLaps = simHubMQTTPublisherPlugin.Settings.Include_TotalLaps,
                Include_CompletedLaps = simHubMQTTPublisherPlugin.Settings.Include_CompletedLaps,

                // === TIRE DATA ===
                Include_TireTemperatures = simHubMQTTPublisherPlugin.Settings.Include_TireTemperatures,
                Include_TirePressures = simHubMQTTPublisherPlugin.Settings.Include_TirePressures,
                Include_TireWear = simHubMQTTPublisherPlugin.Settings.Include_TireWear,
                Include_TireGrip = simHubMQTTPublisherPlugin.Settings.Include_TireGrip,
                Include_TireCompound = simHubMQTTPublisherPlugin.Settings.Include_TireCompound,
                Include_TireDirt = simHubMQTTPublisherPlugin.Settings.Include_TireDirt,

                // === FUEL & ENERGY DATA ===
                Include_Fuel = simHubMQTTPublisherPlugin.Settings.Include_Fuel,
                Include_FuelCapacity = simHubMQTTPublisherPlugin.Settings.Include_FuelCapacity,
                Include_FuelPerLap = simHubMQTTPublisherPlugin.Settings.Include_FuelPerLap,
                Include_FuelRemaining = simHubMQTTPublisherPlugin.Settings.Include_FuelRemaining,
                Include_FuelEstimatedLaps = simHubMQTTPublisherPlugin.Settings.Include_FuelEstimatedLaps,
                Include_FuelToEnd = simHubMQTTPublisherPlugin.Settings.Include_FuelToEnd,
                Include_ERS_Data = simHubMQTTPublisherPlugin.Settings.Include_ERS_Data,
                Include_DRS_Data = simHubMQTTPublisherPlugin.Settings.Include_DRS_Data,
                Include_BatteryData = simHubMQTTPublisherPlugin.Settings.Include_BatteryData,

                // === WEATHER & CONDITIONS ===
                Include_AirTemperature = simHubMQTTPublisherPlugin.Settings.Include_AirTemperature,
                Include_TrackTemperature = simHubMQTTPublisherPlugin.Settings.Include_TrackTemperature,
                Include_WeatherType = simHubMQTTPublisherPlugin.Settings.Include_WeatherType,
                Include_RainLevel = simHubMQTTPublisherPlugin.Settings.Include_RainLevel,
                Include_Humidity = simHubMQTTPublisherPlugin.Settings.Include_Humidity,
                Include_WindData = simHubMQTTPublisherPlugin.Settings.Include_WindData,
                Include_TrackGrip = simHubMQTTPublisherPlugin.Settings.Include_TrackGrip,
                Include_TimeOfDay = simHubMQTTPublisherPlugin.Settings.Include_TimeOfDay,

                // === DAMAGE & MECHANICAL ===
                Include_CarDamage = simHubMQTTPublisherPlugin.Settings.Include_CarDamage,
                Include_EngineTemperatures = simHubMQTTPublisherPlugin.Settings.Include_EngineTemperatures,
                Include_BrakeTemperatures = simHubMQTTPublisherPlugin.Settings.Include_BrakeTemperatures,
                Include_TurboData = simHubMQTTPublisherPlugin.Settings.Include_TurboData,
                Include_WearIndicators = simHubMQTTPublisherPlugin.Settings.Include_WearIndicators,

                // === DRIVER INPUT ===
                Include_SteeringInput = simHubMQTTPublisherPlugin.Settings.Include_SteeringInput,
                Include_PedalInputs = simHubMQTTPublisherPlugin.Settings.Include_PedalInputs,
                Include_DriverAssists = simHubMQTTPublisherPlugin.Settings.Include_DriverAssists,
                Include_ElectronicSystems = simHubMQTTPublisherPlugin.Settings.Include_ElectronicSystems,
                Include_InputDeviceInfo = simHubMQTTPublisherPlugin.Settings.Include_InputDeviceInfo,

                // === SAFETY & RACE CONTROL ===
                Include_SafetyCarInfo = simHubMQTTPublisherPlugin.Settings.Include_SafetyCarInfo,
                Include_FlagSectors = simHubMQTTPublisherPlugin.Settings.Include_FlagSectors,
                Include_PitInformation = simHubMQTTPublisherPlugin.Settings.Include_PitInformation,
                Include_RaceControl = simHubMQTTPublisherPlugin.Settings.Include_RaceControl,
                Include_Penalties = simHubMQTTPublisherPlugin.Settings.Include_Penalties,
                Include_FormationLap = simHubMQTTPublisherPlugin.Settings.Include_FormationLap,

                // === TRACK INFORMATION ===
                Include_TrackName = simHubMQTTPublisherPlugin.Settings.Include_TrackName,
                Include_TrackLength = simHubMQTTPublisherPlugin.Settings.Include_TrackLength,
                Include_TrackConfiguration = simHubMQTTPublisherPlugin.Settings.Include_TrackConfiguration,

                // === VEHICLE INFORMATION ===
                Include_VehicleModel = simHubMQTTPublisherPlugin.Settings.Include_VehicleModel,
                Include_VehicleClass = simHubMQTTPublisherPlugin.Settings.Include_VehicleClass,
                Include_MaxRpm = simHubMQTTPublisherPlugin.Settings.Include_MaxRpm,

                // === SESSION INFORMATION ===
                Include_SessionType = simHubMQTTPublisherPlugin.Settings.Include_SessionType,
                Include_SessionTimeLeft = simHubMQTTPublisherPlugin.Settings.Include_SessionTimeLeft,
                Include_SessionLaps = simHubMQTTPublisherPlugin.Settings.Include_SessionLaps,

                // === ADVANCED DEBUGGING ===
                EnableDebugMode = simHubMQTTPublisherPlugin.Settings.EnableDebugMode,
            };

            this.DataContext = Model;

            // Initialize password box
            PasswordBoxHidden.Password = simHubMQTTPublisherPlugin.Settings.Password;
        }

        private SimHubMQTTPublisherPluginUIModel Model { get; }

        private SimHubMQTTPublisherPlugin SimHubMQTTPublisherPlugin { get; }

        private void ShowPassword_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            PasswordBoxVisible.Text = PasswordBoxHidden.Password;
            PasswordBoxHidden.Visibility = System.Windows.Visibility.Collapsed;
            PasswordBoxVisible.Visibility = System.Windows.Visibility.Visible;
        }

        private void ShowPassword_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            PasswordBoxHidden.Password = PasswordBoxVisible.Text;
            PasswordBoxVisible.Visibility = System.Windows.Visibility.Collapsed;
            PasswordBoxHidden.Visibility = System.Windows.Visibility.Visible;
        }

        private void PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            Model.Password = PasswordBoxHidden.Password;
        }

        private void Apply_Settings(object sender, System.Windows.RoutedEventArgs e)
        {
            SimHubMQTTPublisherPlugin.Settings.Server = Model.Server;
            SimHubMQTTPublisherPlugin.Settings.Topic = Model.Topic;
            SimHubMQTTPublisherPlugin.Settings.Login = Model.Login;
            // Password is handled through the password box or visible text box
            SimHubMQTTPublisherPlugin.Settings.Password = PasswordBoxHidden.Visibility == System.Windows.Visibility.Visible
                ? PasswordBoxHidden.Password
                : PasswordBoxVisible.Text;

            // === CAR STATE PROPERTIES ===
            SimHubMQTTPublisherPlugin.Settings.Include_SpeedKmh = Model.Include_SpeedKmh;
            SimHubMQTTPublisherPlugin.Settings.Include_Rpms = Model.Include_Rpms;
            SimHubMQTTPublisherPlugin.Settings.Include_Gear = Model.Include_Gear;
            SimHubMQTTPublisherPlugin.Settings.Include_Throttle = Model.Include_Throttle;
            SimHubMQTTPublisherPlugin.Settings.Include_Brake = Model.Include_Brake;
            SimHubMQTTPublisherPlugin.Settings.Include_Clutch = Model.Include_Clutch;
            SimHubMQTTPublisherPlugin.Settings.Include_CarCoordinates = Model.Include_CarCoordinates;
            SimHubMQTTPublisherPlugin.Settings.Include_CurrentLapTime = Model.Include_CurrentLapTime;
            SimHubMQTTPublisherPlugin.Settings.Include_CarModel = Model.Include_CarModel;
            SimHubMQTTPublisherPlugin.Settings.Include_CarClass = Model.Include_CarClass;
            SimHubMQTTPublisherPlugin.Settings.Include_EngineIgnitionOn = Model.Include_EngineIgnitionOn;
            SimHubMQTTPublisherPlugin.Settings.Include_EngineStarted = Model.Include_EngineStarted;

            // === FLAG INFORMATION ===
            SimHubMQTTPublisherPlugin.Settings.Include_Flags = Model.Include_Flags;
            SimHubMQTTPublisherPlugin.Settings.Include_FlagName = Model.Include_FlagName;
            SimHubMQTTPublisherPlugin.Settings.Include_GameName = Model.Include_GameName;
            SimHubMQTTPublisherPlugin.Settings.Include_DebugFlags = Model.Include_DebugFlags;

            // === POSITION & TIMING DATA ===
            SimHubMQTTPublisherPlugin.Settings.Include_Position = Model.Include_Position;
            SimHubMQTTPublisherPlugin.Settings.Include_PositionInClass = Model.Include_PositionInClass;
            SimHubMQTTPublisherPlugin.Settings.Include_Gap = Model.Include_Gap;
            SimHubMQTTPublisherPlugin.Settings.Include_GapToLeader = Model.Include_GapToLeader;
            SimHubMQTTPublisherPlugin.Settings.Include_GapToAhead = Model.Include_GapToAhead;
            SimHubMQTTPublisherPlugin.Settings.Include_GapToBehind = Model.Include_GapToBehind;
            SimHubMQTTPublisherPlugin.Settings.Include_LastLapTime = Model.Include_LastLapTime;
            SimHubMQTTPublisherPlugin.Settings.Include_BestLapTime = Model.Include_BestLapTime;
            SimHubMQTTPublisherPlugin.Settings.Include_PersonalBestLapTime = Model.Include_PersonalBestLapTime;
            SimHubMQTTPublisherPlugin.Settings.Include_SessionBestLapTime = Model.Include_SessionBestLapTime;
            SimHubMQTTPublisherPlugin.Settings.Include_DeltaToSessionBest = Model.Include_DeltaToSessionBest;
            SimHubMQTTPublisherPlugin.Settings.Include_DeltaToPersonalBest = Model.Include_DeltaToPersonalBest;
            SimHubMQTTPublisherPlugin.Settings.Include_DeltaToOptimal = Model.Include_DeltaToOptimal;
            SimHubMQTTPublisherPlugin.Settings.Include_Sector1Time = Model.Include_Sector1Time;
            SimHubMQTTPublisherPlugin.Settings.Include_Sector2Time = Model.Include_Sector2Time;
            SimHubMQTTPublisherPlugin.Settings.Include_Sector3Time = Model.Include_Sector3Time;
            SimHubMQTTPublisherPlugin.Settings.Include_Sector1BestTime = Model.Include_Sector1BestTime;
            SimHubMQTTPublisherPlugin.Settings.Include_Sector2BestTime = Model.Include_Sector2BestTime;
            SimHubMQTTPublisherPlugin.Settings.Include_Sector3BestTime = Model.Include_Sector3BestTime;
            SimHubMQTTPublisherPlugin.Settings.Include_CurrentSector = Model.Include_CurrentSector;
            SimHubMQTTPublisherPlugin.Settings.Include_CurrentLap = Model.Include_CurrentLap;
            SimHubMQTTPublisherPlugin.Settings.Include_TotalLaps = Model.Include_TotalLaps;
            SimHubMQTTPublisherPlugin.Settings.Include_CompletedLaps = Model.Include_CompletedLaps;

            // === TIRE DATA ===
            SimHubMQTTPublisherPlugin.Settings.Include_TireTemperatures = Model.Include_TireTemperatures;
            SimHubMQTTPublisherPlugin.Settings.Include_TirePressures = Model.Include_TirePressures;
            SimHubMQTTPublisherPlugin.Settings.Include_TireWear = Model.Include_TireWear;
            SimHubMQTTPublisherPlugin.Settings.Include_TireGrip = Model.Include_TireGrip;
            SimHubMQTTPublisherPlugin.Settings.Include_TireCompound = Model.Include_TireCompound;
            SimHubMQTTPublisherPlugin.Settings.Include_TireDirt = Model.Include_TireDirt;

            // === FUEL & ENERGY DATA ===
            SimHubMQTTPublisherPlugin.Settings.Include_Fuel = Model.Include_Fuel;
            SimHubMQTTPublisherPlugin.Settings.Include_FuelCapacity = Model.Include_FuelCapacity;
            SimHubMQTTPublisherPlugin.Settings.Include_FuelPerLap = Model.Include_FuelPerLap;
            SimHubMQTTPublisherPlugin.Settings.Include_FuelRemaining = Model.Include_FuelRemaining;
            SimHubMQTTPublisherPlugin.Settings.Include_FuelEstimatedLaps = Model.Include_FuelEstimatedLaps;
            SimHubMQTTPublisherPlugin.Settings.Include_FuelToEnd = Model.Include_FuelToEnd;
            SimHubMQTTPublisherPlugin.Settings.Include_ERS_Data = Model.Include_ERS_Data;
            SimHubMQTTPublisherPlugin.Settings.Include_DRS_Data = Model.Include_DRS_Data;
            SimHubMQTTPublisherPlugin.Settings.Include_BatteryData = Model.Include_BatteryData;

            // === WEATHER & CONDITIONS ===
            SimHubMQTTPublisherPlugin.Settings.Include_AirTemperature = Model.Include_AirTemperature;
            SimHubMQTTPublisherPlugin.Settings.Include_TrackTemperature = Model.Include_TrackTemperature;
            SimHubMQTTPublisherPlugin.Settings.Include_WeatherType = Model.Include_WeatherType;
            SimHubMQTTPublisherPlugin.Settings.Include_RainLevel = Model.Include_RainLevel;
            SimHubMQTTPublisherPlugin.Settings.Include_Humidity = Model.Include_Humidity;
            SimHubMQTTPublisherPlugin.Settings.Include_WindData = Model.Include_WindData;
            SimHubMQTTPublisherPlugin.Settings.Include_TrackGrip = Model.Include_TrackGrip;
            SimHubMQTTPublisherPlugin.Settings.Include_TimeOfDay = Model.Include_TimeOfDay;

            // === DAMAGE & MECHANICAL ===
            SimHubMQTTPublisherPlugin.Settings.Include_CarDamage = Model.Include_CarDamage;
            SimHubMQTTPublisherPlugin.Settings.Include_EngineTemperatures = Model.Include_EngineTemperatures;
            SimHubMQTTPublisherPlugin.Settings.Include_BrakeTemperatures = Model.Include_BrakeTemperatures;
            SimHubMQTTPublisherPlugin.Settings.Include_TurboData = Model.Include_TurboData;
            SimHubMQTTPublisherPlugin.Settings.Include_WearIndicators = Model.Include_WearIndicators;

            // === DRIVER INPUT ===
            SimHubMQTTPublisherPlugin.Settings.Include_SteeringInput = Model.Include_SteeringInput;
            SimHubMQTTPublisherPlugin.Settings.Include_PedalInputs = Model.Include_PedalInputs;
            SimHubMQTTPublisherPlugin.Settings.Include_DriverAssists = Model.Include_DriverAssists;
            SimHubMQTTPublisherPlugin.Settings.Include_ElectronicSystems = Model.Include_ElectronicSystems;
            SimHubMQTTPublisherPlugin.Settings.Include_InputDeviceInfo = Model.Include_InputDeviceInfo;

            // === SAFETY & RACE CONTROL ===
            SimHubMQTTPublisherPlugin.Settings.Include_SafetyCarInfo = Model.Include_SafetyCarInfo;
            SimHubMQTTPublisherPlugin.Settings.Include_FlagSectors = Model.Include_FlagSectors;
            SimHubMQTTPublisherPlugin.Settings.Include_PitInformation = Model.Include_PitInformation;
            SimHubMQTTPublisherPlugin.Settings.Include_RaceControl = Model.Include_RaceControl;
            SimHubMQTTPublisherPlugin.Settings.Include_Penalties = Model.Include_Penalties;
            SimHubMQTTPublisherPlugin.Settings.Include_FormationLap = Model.Include_FormationLap;

            // === TRACK INFORMATION ===
            SimHubMQTTPublisherPlugin.Settings.Include_TrackName = Model.Include_TrackName;
            SimHubMQTTPublisherPlugin.Settings.Include_TrackLength = Model.Include_TrackLength;
            SimHubMQTTPublisherPlugin.Settings.Include_TrackConfiguration = Model.Include_TrackConfiguration;

            // === VEHICLE INFORMATION ===
            SimHubMQTTPublisherPlugin.Settings.Include_VehicleModel = Model.Include_VehicleModel;
            SimHubMQTTPublisherPlugin.Settings.Include_VehicleClass = Model.Include_VehicleClass;
            SimHubMQTTPublisherPlugin.Settings.Include_MaxRpm = Model.Include_MaxRpm;

            // === SESSION INFORMATION ===
            SimHubMQTTPublisherPlugin.Settings.Include_SessionType = Model.Include_SessionType;
            SimHubMQTTPublisherPlugin.Settings.Include_SessionTimeLeft = Model.Include_SessionTimeLeft;
            SimHubMQTTPublisherPlugin.Settings.Include_SessionLaps = Model.Include_SessionLaps;

            // === ADVANCED DEBUGGING ===
            SimHubMQTTPublisherPlugin.Settings.EnableDebugMode = Model.EnableDebugMode;

            SimHubMQTTPublisherPlugin.CreateMQTTClient();
        }

        private void EnableAll_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Root Level Properties
            Model.Include_Time = true;
            Model.Include_UserId = true;
            Model.Include_GameName = true;

            // Car State
            Model.Include_SpeedKmh = true;
            Model.Include_Rpms = true;
            Model.Include_Gear = true;
            Model.Include_Throttle = true;
            Model.Include_Brake = true;
            Model.Include_Clutch = true;
            Model.Include_CarCoordinates = true;
            Model.Include_CurrentLapTime = true;
            Model.Include_CarModel = true;
            Model.Include_CarClass = true;
            Model.Include_EngineIgnitionOn = true;
            Model.Include_EngineStarted = true;

            // Flag Information
            Model.Include_Flags = true;
            Model.Include_FlagName = true;
            // Note: DebugFlags excluded from "Enable All" - it's an advanced debugging option

            // Position & Timing Data
            Model.Include_Position = true;
            Model.Include_PositionInClass = true;
            Model.Include_Gap = true;
            Model.Include_GapToLeader = true;
            Model.Include_GapToAhead = true;
            Model.Include_GapToBehind = true;
            Model.Include_LastLapTime = true;
            Model.Include_BestLapTime = true;
            Model.Include_PersonalBestLapTime = true;
            Model.Include_SessionBestLapTime = true;
            Model.Include_DeltaToSessionBest = true;
            Model.Include_DeltaToPersonalBest = true;
            Model.Include_DeltaToOptimal = true;
            Model.Include_Sector1Time = true;
            Model.Include_Sector2Time = true;
            Model.Include_Sector3Time = true;
            Model.Include_Sector1BestTime = true;
            Model.Include_Sector2BestTime = true;
            Model.Include_Sector3BestTime = true;
            Model.Include_CurrentSector = true;
            Model.Include_CurrentLap = true;
            Model.Include_TotalLaps = true;
            Model.Include_CompletedLaps = true;

            // Tire Data
            Model.Include_TireTemperatures = true;
            Model.Include_TirePressures = true;
            Model.Include_TireWear = true;
            Model.Include_TireGrip = true;
            Model.Include_TireCompound = true;
            Model.Include_TireDirt = true;

            // Fuel & Energy Data
            Model.Include_Fuel = true;
            Model.Include_FuelCapacity = true;
            Model.Include_FuelPerLap = true;
            Model.Include_FuelRemaining = true;
            Model.Include_FuelEstimatedLaps = true;
            Model.Include_FuelToEnd = true;
            Model.Include_ERS_Data = true;
            Model.Include_DRS_Data = true;
            Model.Include_BatteryData = true;

            // Weather & Conditions
            Model.Include_AirTemperature = true;
            Model.Include_TrackTemperature = true;
            Model.Include_WeatherType = true;
            Model.Include_RainLevel = true;
            Model.Include_Humidity = true;
            Model.Include_WindData = true;
            Model.Include_TrackGrip = true;
            Model.Include_TimeOfDay = true;

            // Damage & Mechanical
            Model.Include_CarDamage = true;
            Model.Include_EngineTemperatures = true;
            Model.Include_BrakeTemperatures = true;
            Model.Include_TurboData = true;
            Model.Include_WearIndicators = true;

            // Driver Input
            Model.Include_SteeringInput = true;
            Model.Include_PedalInputs = true;
            Model.Include_DriverAssists = true;
            Model.Include_ElectronicSystems = true;
            Model.Include_InputDeviceInfo = true;

            // Safety & Race Control
            Model.Include_SafetyCarInfo = true;
            Model.Include_FlagSectors = true;
            Model.Include_PitInformation = true;
            Model.Include_RaceControl = true;
            Model.Include_Penalties = true;
            Model.Include_FormationLap = true;

            // Track Information
            Model.Include_TrackName = true;
            Model.Include_TrackLength = true;
            Model.Include_TrackConfiguration = true;

            // Vehicle Information
            Model.Include_VehicleModel = true;
            Model.Include_VehicleClass = true;
            Model.Include_MaxRpm = true;

            // Session Information
            Model.Include_SessionType = true;
            Model.Include_SessionTimeLeft = true;
            Model.Include_SessionLaps = true;
        }

        private void DisableAll_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Car State
            Model.Include_SpeedKmh = false;
            Model.Include_Rpms = false;
            Model.Include_Gear = false;
            Model.Include_Throttle = false;
            Model.Include_Brake = false;
            Model.Include_Clutch = false;
            Model.Include_CarCoordinates = false;
            Model.Include_CurrentLapTime = false;
            Model.Include_CarModel = false;
            Model.Include_CarClass = false;
            Model.Include_EngineIgnitionOn = false;
            Model.Include_EngineStarted = false;

            // Flag Information
            Model.Include_Flags = false;
            Model.Include_FlagName = false;
            Model.Include_GameName = false;
            Model.Include_DebugFlags = false;

            // Position & Timing Data
            Model.Include_Position = false;
            Model.Include_PositionInClass = false;
            Model.Include_Gap = false;
            Model.Include_GapToLeader = false;
            Model.Include_GapToAhead = false;
            Model.Include_GapToBehind = false;
            Model.Include_LastLapTime = false;
            Model.Include_BestLapTime = false;
            Model.Include_PersonalBestLapTime = false;
            Model.Include_SessionBestLapTime = false;
            Model.Include_DeltaToSessionBest = false;
            Model.Include_DeltaToPersonalBest = false;
            Model.Include_DeltaToOptimal = false;
            Model.Include_Sector1Time = false;
            Model.Include_Sector2Time = false;
            Model.Include_Sector3Time = false;
            Model.Include_Sector1BestTime = false;
            Model.Include_Sector2BestTime = false;
            Model.Include_Sector3BestTime = false;
            Model.Include_CurrentSector = false;
            Model.Include_CurrentLap = false;
            Model.Include_TotalLaps = false;
            Model.Include_CompletedLaps = false;

            // Tire Data
            Model.Include_TireTemperatures = false;
            Model.Include_TirePressures = false;
            Model.Include_TireWear = false;
            Model.Include_TireGrip = false;
            Model.Include_TireCompound = false;
            Model.Include_TireDirt = false;

            // Fuel & Energy Data
            Model.Include_Fuel = false;
            Model.Include_FuelCapacity = false;
            Model.Include_FuelPerLap = false;
            Model.Include_FuelRemaining = false;
            Model.Include_FuelEstimatedLaps = false;
            Model.Include_FuelToEnd = false;
            Model.Include_ERS_Data = false;
            Model.Include_DRS_Data = false;
            Model.Include_BatteryData = false;

            // Weather & Conditions
            Model.Include_AirTemperature = false;
            Model.Include_TrackTemperature = false;
            Model.Include_WeatherType = false;
            Model.Include_RainLevel = false;
            Model.Include_Humidity = false;
            Model.Include_WindData = false;
            Model.Include_TrackGrip = false;
            Model.Include_TimeOfDay = false;

            // Damage & Mechanical
            Model.Include_CarDamage = false;
            Model.Include_EngineTemperatures = false;
            Model.Include_BrakeTemperatures = false;
            Model.Include_TurboData = false;
            Model.Include_WearIndicators = false;

            // Driver Input
            Model.Include_SteeringInput = false;
            Model.Include_PedalInputs = false;
            Model.Include_DriverAssists = false;
            Model.Include_ElectronicSystems = false;
            Model.Include_InputDeviceInfo = false;

            // Safety & Race Control
            Model.Include_SafetyCarInfo = false;
            Model.Include_FlagSectors = false;
            Model.Include_PitInformation = false;
            Model.Include_RaceControl = false;
            Model.Include_Penalties = false;
            Model.Include_FormationLap = false;

            // Track Information
            Model.Include_TrackName = false;
            Model.Include_TrackLength = false;
            Model.Include_TrackConfiguration = false;

            // Vehicle Information
            Model.Include_VehicleModel = false;
            Model.Include_VehicleClass = false;
            Model.Include_MaxRpm = false;

            // Session Information
            Model.Include_SessionType = false;
            Model.Include_SessionTimeLeft = false;
            Model.Include_SessionLaps = false;
        }

        private void EnableBasic_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Disable all first
            DisableAll_Click(sender, e);

            // Enable only basic telemetry
            Model.Include_SpeedKmh = true;
            Model.Include_Rpms = true;
            Model.Include_Gear = true;
            Model.Include_Throttle = true;
            Model.Include_Brake = true;
            Model.Include_CurrentLapTime = true;
            Model.Include_Flags = true;
        }

        private void EnableRacing_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Disable all first
            DisableAll_Click(sender, e);

            // Enable racing-focused telemetry
            Model.Include_SpeedKmh = true;
            Model.Include_Rpms = true;
            Model.Include_Gear = true;
            Model.Include_Throttle = true;
            Model.Include_Brake = true;
            Model.Include_CurrentLapTime = true;
            Model.Include_Flags = true;
            Model.Include_Position = true;
            Model.Include_PositionInClass = true;
            Model.Include_GapToLeader = true;
            Model.Include_GapToAhead = true;
            Model.Include_LastLapTime = true;
            Model.Include_BestLapTime = true;
            Model.Include_PersonalBestLapTime = true;
            Model.Include_TireTemperatures = true;
            Model.Include_TirePressures = true;
            Model.Include_SafetyCarInfo = true;
            Model.Include_PitInformation = true;
        }

        private void EnableStrategy_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Disable all first
            DisableAll_Click(sender, e);

            // Enable strategy-focused telemetry
            Model.Include_SpeedKmh = true;
            Model.Include_Gear = true;
            Model.Include_CurrentLapTime = true;
            Model.Include_Flags = true;
            Model.Include_Position = true;
            Model.Include_LastLapTime = true;
            Model.Include_BestLapTime = true;
            Model.Include_CurrentLap = true;
            Model.Include_TotalLaps = true;
            Model.Include_TireTemperatures = true;
            Model.Include_TireWear = true;
            Model.Include_TireCompound = true;
            Model.Include_Fuel = true;
            Model.Include_FuelPerLap = true;
            Model.Include_FuelEstimatedLaps = true;
            Model.Include_FuelToEnd = true;
            Model.Include_SafetyCarInfo = true;
            Model.Include_PitInformation = true;
        }

        private void EnableAnalysis_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Enable all first
            EnableAll_Click(sender, e);

            // Analysis preset includes everything, so no changes needed
        }

        private async void TestConnection_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ConnectionStatusText.Text = "Testing...";
            ConnectionStatusText.Foreground = new SolidColorBrush(Colors.Orange);

            try
            {
                var factory = new MqttFactory();
                var mqttClient = factory.CreateMqttClient();

                var options = new MqttClientOptionsBuilder()
                    .WithTcpServer(Model.Server)
                    .WithCredentials(Model.Login, PasswordBoxHidden.Visibility == System.Windows.Visibility.Visible
                        ? PasswordBoxHidden.Password
                        : PasswordBoxVisible.Text)
                    .Build();

                // Try to connect with a timeout
                var connectTask = mqttClient.ConnectAsync(options, CancellationToken.None);
                var timeoutTask = Task.Delay(5000);

                var completedTask = await Task.WhenAny(connectTask, timeoutTask);

                if (completedTask == timeoutTask)
                {
                    ConnectionStatusText.Text = "Connection timeout";
                    ConnectionStatusText.Foreground = new SolidColorBrush(Colors.Red);
                }
                else if (mqttClient.IsConnected)
                {
                    ConnectionStatusText.Text = "Connected successfully!";
                    ConnectionStatusText.Foreground = new SolidColorBrush(Colors.Green);
                    await mqttClient.DisconnectAsync();
                }
                else
                {
                    ConnectionStatusText.Text = "Connection failed";
                    ConnectionStatusText.Foreground = new SolidColorBrush(Colors.Red);
                }

                mqttClient.Dispose();
            }
            catch (System.Exception ex)
            {
                ConnectionStatusText.Text = $"Error: {ex.Message}";
                ConnectionStatusText.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        private void ExportSettings_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                var saveDialog = new SaveFileDialog
                {
                    Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
                    DefaultExt = "json",
                    FileName = "SimHub-MQTT-Config.json",
                    Title = "Export MQTT Publisher Settings"
                };

                if (saveDialog.ShowDialog() == true)
                {
                    // Create a settings object with all current values
                    var exportSettings = new
                    {
                        // Connection settings (excluding sensitive data by default - user can manually add if needed)
                        Server = Model.Server,
                        Topic = Model.Topic,
                        Login = Model.Login,
                        // Password intentionally excluded for security

                        // All telemetry toggles
                        Include_SpeedKmh = Model.Include_SpeedKmh,
                        Include_Rpms = Model.Include_Rpms,
                        Include_Gear = Model.Include_Gear,
                        Include_Throttle = Model.Include_Throttle,
                        Include_Brake = Model.Include_Brake,
                        Include_Clutch = Model.Include_Clutch,
                        Include_CarCoordinates = Model.Include_CarCoordinates,
                        Include_CurrentLapTime = Model.Include_CurrentLapTime,
                        Include_CarModel = Model.Include_CarModel,
                        Include_CarClass = Model.Include_CarClass,
                        Include_EngineIgnitionOn = Model.Include_EngineIgnitionOn,
                        Include_EngineStarted = Model.Include_EngineStarted,
                        Include_Flags = Model.Include_Flags,
                        Include_FlagName = Model.Include_FlagName,
                        Include_GameName = Model.Include_GameName,
                        Include_DebugFlags = Model.Include_DebugFlags,
                        Include_Position = Model.Include_Position,
                        Include_PositionInClass = Model.Include_PositionInClass,
                        Include_Gap = Model.Include_Gap,
                        Include_GapToLeader = Model.Include_GapToLeader,
                        Include_GapToAhead = Model.Include_GapToAhead,
                        Include_GapToBehind = Model.Include_GapToBehind,
                        Include_LastLapTime = Model.Include_LastLapTime,
                        Include_BestLapTime = Model.Include_BestLapTime,
                        Include_PersonalBestLapTime = Model.Include_PersonalBestLapTime,
                        Include_SessionBestLapTime = Model.Include_SessionBestLapTime,
                        Include_DeltaToSessionBest = Model.Include_DeltaToSessionBest,
                        Include_DeltaToPersonalBest = Model.Include_DeltaToPersonalBest,
                        Include_DeltaToOptimal = Model.Include_DeltaToOptimal,
                        Include_Sector1Time = Model.Include_Sector1Time,
                        Include_Sector2Time = Model.Include_Sector2Time,
                        Include_Sector3Time = Model.Include_Sector3Time,
                        Include_Sector1BestTime = Model.Include_Sector1BestTime,
                        Include_Sector2BestTime = Model.Include_Sector2BestTime,
                        Include_Sector3BestTime = Model.Include_Sector3BestTime,
                        Include_CurrentSector = Model.Include_CurrentSector,
                        Include_CurrentLap = Model.Include_CurrentLap,
                        Include_TotalLaps = Model.Include_TotalLaps,
                        Include_CompletedLaps = Model.Include_CompletedLaps,
                        Include_TireTemperatures = Model.Include_TireTemperatures,
                        Include_TirePressures = Model.Include_TirePressures,
                        Include_TireWear = Model.Include_TireWear,
                        Include_TireGrip = Model.Include_TireGrip,
                        Include_TireCompound = Model.Include_TireCompound,
                        Include_TireDirt = Model.Include_TireDirt,
                        Include_Fuel = Model.Include_Fuel,
                        Include_FuelCapacity = Model.Include_FuelCapacity,
                        Include_FuelPerLap = Model.Include_FuelPerLap,
                        Include_FuelRemaining = Model.Include_FuelRemaining,
                        Include_FuelEstimatedLaps = Model.Include_FuelEstimatedLaps,
                        Include_FuelToEnd = Model.Include_FuelToEnd,
                        Include_ERS_Data = Model.Include_ERS_Data,
                        Include_DRS_Data = Model.Include_DRS_Data,
                        Include_BatteryData = Model.Include_BatteryData,
                        Include_AirTemperature = Model.Include_AirTemperature,
                        Include_TrackTemperature = Model.Include_TrackTemperature,
                        Include_WeatherType = Model.Include_WeatherType,
                        Include_RainLevel = Model.Include_RainLevel,
                        Include_Humidity = Model.Include_Humidity,
                        Include_WindData = Model.Include_WindData,
                        Include_TrackGrip = Model.Include_TrackGrip,
                        Include_TimeOfDay = Model.Include_TimeOfDay,
                        Include_CarDamage = Model.Include_CarDamage,
                        Include_EngineTemperatures = Model.Include_EngineTemperatures,
                        Include_BrakeTemperatures = Model.Include_BrakeTemperatures,
                        Include_TurboData = Model.Include_TurboData,
                        Include_WearIndicators = Model.Include_WearIndicators,
                        Include_SteeringInput = Model.Include_SteeringInput,
                        Include_PedalInputs = Model.Include_PedalInputs,
                        Include_DriverAssists = Model.Include_DriverAssists,
                        Include_ElectronicSystems = Model.Include_ElectronicSystems,
                        Include_InputDeviceInfo = Model.Include_InputDeviceInfo,
                        Include_SafetyCarInfo = Model.Include_SafetyCarInfo,
                        Include_FlagSectors = Model.Include_FlagSectors,
                        Include_PitInformation = Model.Include_PitInformation,
                        Include_RaceControl = Model.Include_RaceControl,
                        Include_Penalties = Model.Include_Penalties,
                        Include_FormationLap = Model.Include_FormationLap,
                        Include_TrackName = Model.Include_TrackName,
                        Include_TrackLength = Model.Include_TrackLength,
                        Include_TrackConfiguration = Model.Include_TrackConfiguration,
                        Include_VehicleModel = Model.Include_VehicleModel,
                        Include_VehicleClass = Model.Include_VehicleClass,
                        Include_MaxRpm = Model.Include_MaxRpm,
                        Include_SessionType = Model.Include_SessionType,
                        Include_SessionTimeLeft = Model.Include_SessionTimeLeft,
                        Include_SessionLaps = Model.Include_SessionLaps,
                        EnableDebugMode = Model.EnableDebugMode
                    };

                    var json = JsonConvert.SerializeObject(exportSettings, Formatting.Indented);
                    File.WriteAllText(saveDialog.FileName, json);

                    System.Windows.MessageBox.Show(
                        $"Settings exported successfully to:\n{saveDialog.FileName}\n\nNote: Password was excluded for security.",
                        "Export Successful",
                        System.Windows.MessageBoxButton.OK,
                        System.Windows.MessageBoxImage.Information);
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Failed to export settings:\n{ex.Message}",
                    "Export Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
        }

        private void ImportSettings_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                var openDialog = new OpenFileDialog
                {
                    Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
                    DefaultExt = "json",
                    Title = "Import MQTT Publisher Settings"
                };

                if (openDialog.ShowDialog() == true)
                {
                    var json = File.ReadAllText(openDialog.FileName);
                    var importedSettings = JsonConvert.DeserializeObject<SimHubMQTTPublisherPluginSettings>(json);

                    if (importedSettings != null)
                    {
                        // Apply connection settings if present
                        if (!string.IsNullOrEmpty(importedSettings.Server))
                            Model.Server = importedSettings.Server;
                        if (!string.IsNullOrEmpty(importedSettings.Topic))
                            Model.Topic = importedSettings.Topic;
                        if (!string.IsNullOrEmpty(importedSettings.Login))
                            Model.Login = importedSettings.Login;
                        // Password not imported for security

                        // Apply all telemetry toggles
                        Model.Include_SpeedKmh = importedSettings.Include_SpeedKmh;
                        Model.Include_Rpms = importedSettings.Include_Rpms;
                        Model.Include_Gear = importedSettings.Include_Gear;
                        Model.Include_Throttle = importedSettings.Include_Throttle;
                        Model.Include_Brake = importedSettings.Include_Brake;
                        Model.Include_Clutch = importedSettings.Include_Clutch;
                        Model.Include_CarCoordinates = importedSettings.Include_CarCoordinates;
                        Model.Include_CurrentLapTime = importedSettings.Include_CurrentLapTime;
                        Model.Include_CarModel = importedSettings.Include_CarModel;
                        Model.Include_CarClass = importedSettings.Include_CarClass;
                        Model.Include_EngineIgnitionOn = importedSettings.Include_EngineIgnitionOn;
                        Model.Include_EngineStarted = importedSettings.Include_EngineStarted;
                        Model.Include_Flags = importedSettings.Include_Flags;
                        Model.Include_FlagName = importedSettings.Include_FlagName;
                        Model.Include_GameName = importedSettings.Include_GameName;
                        Model.Include_DebugFlags = importedSettings.Include_DebugFlags;
                        Model.Include_Position = importedSettings.Include_Position;
                        Model.Include_PositionInClass = importedSettings.Include_PositionInClass;
                        Model.Include_Gap = importedSettings.Include_Gap;
                        Model.Include_GapToLeader = importedSettings.Include_GapToLeader;
                        Model.Include_GapToAhead = importedSettings.Include_GapToAhead;
                        Model.Include_GapToBehind = importedSettings.Include_GapToBehind;
                        Model.Include_LastLapTime = importedSettings.Include_LastLapTime;
                        Model.Include_BestLapTime = importedSettings.Include_BestLapTime;
                        Model.Include_PersonalBestLapTime = importedSettings.Include_PersonalBestLapTime;
                        Model.Include_SessionBestLapTime = importedSettings.Include_SessionBestLapTime;
                        Model.Include_DeltaToSessionBest = importedSettings.Include_DeltaToSessionBest;
                        Model.Include_DeltaToPersonalBest = importedSettings.Include_DeltaToPersonalBest;
                        Model.Include_DeltaToOptimal = importedSettings.Include_DeltaToOptimal;
                        Model.Include_Sector1Time = importedSettings.Include_Sector1Time;
                        Model.Include_Sector2Time = importedSettings.Include_Sector2Time;
                        Model.Include_Sector3Time = importedSettings.Include_Sector3Time;
                        Model.Include_Sector1BestTime = importedSettings.Include_Sector1BestTime;
                        Model.Include_Sector2BestTime = importedSettings.Include_Sector2BestTime;
                        Model.Include_Sector3BestTime = importedSettings.Include_Sector3BestTime;
                        Model.Include_CurrentSector = importedSettings.Include_CurrentSector;
                        Model.Include_CurrentLap = importedSettings.Include_CurrentLap;
                        Model.Include_TotalLaps = importedSettings.Include_TotalLaps;
                        Model.Include_CompletedLaps = importedSettings.Include_CompletedLaps;
                        Model.Include_TireTemperatures = importedSettings.Include_TireTemperatures;
                        Model.Include_TirePressures = importedSettings.Include_TirePressures;
                        Model.Include_TireWear = importedSettings.Include_TireWear;
                        Model.Include_TireGrip = importedSettings.Include_TireGrip;
                        Model.Include_TireCompound = importedSettings.Include_TireCompound;
                        Model.Include_TireDirt = importedSettings.Include_TireDirt;
                        Model.Include_Fuel = importedSettings.Include_Fuel;
                        Model.Include_FuelCapacity = importedSettings.Include_FuelCapacity;
                        Model.Include_FuelPerLap = importedSettings.Include_FuelPerLap;
                        Model.Include_FuelRemaining = importedSettings.Include_FuelRemaining;
                        Model.Include_FuelEstimatedLaps = importedSettings.Include_FuelEstimatedLaps;
                        Model.Include_FuelToEnd = importedSettings.Include_FuelToEnd;
                        Model.Include_ERS_Data = importedSettings.Include_ERS_Data;
                        Model.Include_DRS_Data = importedSettings.Include_DRS_Data;
                        Model.Include_BatteryData = importedSettings.Include_BatteryData;
                        Model.Include_AirTemperature = importedSettings.Include_AirTemperature;
                        Model.Include_TrackTemperature = importedSettings.Include_TrackTemperature;
                        Model.Include_WeatherType = importedSettings.Include_WeatherType;
                        Model.Include_RainLevel = importedSettings.Include_RainLevel;
                        Model.Include_Humidity = importedSettings.Include_Humidity;
                        Model.Include_WindData = importedSettings.Include_WindData;
                        Model.Include_TrackGrip = importedSettings.Include_TrackGrip;
                        Model.Include_TimeOfDay = importedSettings.Include_TimeOfDay;
                        Model.Include_CarDamage = importedSettings.Include_CarDamage;
                        Model.Include_EngineTemperatures = importedSettings.Include_EngineTemperatures;
                        Model.Include_BrakeTemperatures = importedSettings.Include_BrakeTemperatures;
                        Model.Include_TurboData = importedSettings.Include_TurboData;
                        Model.Include_WearIndicators = importedSettings.Include_WearIndicators;
                        Model.Include_SteeringInput = importedSettings.Include_SteeringInput;
                        Model.Include_PedalInputs = importedSettings.Include_PedalInputs;
                        Model.Include_DriverAssists = importedSettings.Include_DriverAssists;
                        Model.Include_ElectronicSystems = importedSettings.Include_ElectronicSystems;
                        Model.Include_InputDeviceInfo = importedSettings.Include_InputDeviceInfo;
                        Model.Include_SafetyCarInfo = importedSettings.Include_SafetyCarInfo;
                        Model.Include_FlagSectors = importedSettings.Include_FlagSectors;
                        Model.Include_PitInformation = importedSettings.Include_PitInformation;
                        Model.Include_RaceControl = importedSettings.Include_RaceControl;
                        Model.Include_Penalties = importedSettings.Include_Penalties;
                        Model.Include_FormationLap = importedSettings.Include_FormationLap;
                        Model.Include_TrackName = importedSettings.Include_TrackName;
                        Model.Include_TrackLength = importedSettings.Include_TrackLength;
                        Model.Include_TrackConfiguration = importedSettings.Include_TrackConfiguration;
                        Model.Include_VehicleModel = importedSettings.Include_VehicleModel;
                        Model.Include_VehicleClass = importedSettings.Include_VehicleClass;
                        Model.Include_MaxRpm = importedSettings.Include_MaxRpm;
                        Model.Include_SessionType = importedSettings.Include_SessionType;
                        Model.Include_SessionTimeLeft = importedSettings.Include_SessionTimeLeft;
                        Model.Include_SessionLaps = importedSettings.Include_SessionLaps;
                        Model.EnableDebugMode = importedSettings.EnableDebugMode;

                        System.Windows.MessageBox.Show(
                            $"Settings imported successfully from:\n{openDialog.FileName}\n\nDon't forget to click 'Apply Settings' to save changes.",
                            "Import Successful",
                            System.Windows.MessageBoxButton.OK,
                            System.Windows.MessageBoxImage.Information);
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Failed to import settings:\n{ex.Message}",
                    "Import Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = e.Uri.AbsoluteUri,
                UseShellExecute = true
            });
            e.Handled = true;
        }
    }
}
