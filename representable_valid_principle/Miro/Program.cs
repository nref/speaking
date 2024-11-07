var thermostat = new BetterThermostat(
    new HeatState(new Setpoint(new Celsius(25))),
    new Reading(new Fahrenheit(70)),
    new FetchInProgress());

static class ThermostatValidator
{
    public static bool IsValid(BuggyThermostat t) =>
       ((t.IsOn && (t.IsHeatOn || t.IsCoolOn)) || (!t.IsOn && !(t.IsHeatOn || t.IsCoolOn)))
    && !(t.IsHeatOn && t.IsCoolOn)
    && ((t.IsOn && t.Setpoint >= 0) || (!t.IsOn && t.Setpoint < 0));

    public static bool IsValid(BetterThermostat _) => true;
}

// Thermostat abstract state:
// - Whether the heat is on
// - Whether the cool is on
// - The cool setpoint
// - The heat setpoint
// - The inside temperature
// - The outside temperature

record BuggyThermostat(
    // Redundant state: IsOn is false, but IsHeatOn and IsCoolOn are true
    bool IsOn, 
    // Illegal state: IsHeatOn and IsCoolOn are both true
    bool IsHeatOn, bool IsCoolOn, 
    
    // Missing state: Need two setpoints, one for heat and one for cool
    // Illegal state: IsOn = false but we have a setpoint
    int Setpoint, 

    int InsideTemp, 
    
    // Overloaded state:
    //   OutsideTemp is null
    //     - because we haven't fetched it from the server
    //     - because it is currently being fetched from the server
    //     - because the server returned an error
    int? OutsideTemp = null)
{
    public bool IsStateValid => IsOn && (IsHeatOn ^ IsCoolOn); // ^ is XOR
}

//new BuggyThermostat(IsOn: false, IsHeatOn: true, IsCoolOn: true, ...);
// Valid states:
// true true false
// true false true
// false false false

record BuggyThermostat2(
    bool IsHeatOn, bool IsCoolOn, 
    int HeatSetpoint, int CoolSetpoint, 
    int InsideTemp, 
    int? OutsideTemp = null)
{
    public bool IsStateValid => IsHeatOn ^ IsCoolOn; // ^ is XOR
}

abstract record Unit(int Value);
record Fahrenheit(int Value) : Unit(Value);
record Celsius(int Value) : Unit(Value);

abstract record Temperature(Unit Value);
record Setpoint(Unit Value) : Temperature(Value);
record Reading(Unit Value) : Temperature(Value);

record BuggyThermostat3(
    bool IsHeatOn, bool IsCoolOn, 
    // Illegal state: HeatSetpoint is less than CoolSetpoint
    Setpoint HeatSetpoint, Setpoint CoolSetpoint, 
    Reading InsideTemp, Reading? OutsideTemp = null)
{
    public bool IsStateValid => IsHeatOn ^ IsCoolOn 
        && HeatSetpoint.Value.Value >= CoolSetpoint.Value.Value;
}

abstract record RunState;
record OffState : RunState;
record HeatState(Setpoint Setpoint) : RunState;
record CoolState(Setpoint Setpoint) : RunState;

record BuggyThermostat4(
    RunState State,
    Reading InsideTemp, 
    Reading? OutsideTemp = null);

abstract record FetchState;
record NotFetched : FetchState;
record FetchInProgress : FetchState;
record FetchError(string Message) : FetchState;
record Fetched(Reading Temperature) : FetchState;

record BetterThermostat(RunState RunState, Reading InsideTemp, FetchState OutsideTemp);
