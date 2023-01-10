﻿namespace DalApi;
using Do;
using System.Reflection;
using static DalApi.DalConfig;

public static class Factory
{
    public static IDal Get()
    {
        string dalType = s_dalName
            ?? throw new DalConfigException($"DAL name is not extracted from the configuration");
        string dal = s_dalPackages[dalType]
           ?? throw new DalConfigException($"Package for {dalType} is not found in packages list");
        string namespaceDal = s_dalNamespaces[dalType]
           ?? throw new DalConfigException($"Package for {dalType} is not found in packages list");
        string classDal = s_class[dalType]
          ?? throw new DalConfigException($"Package for {dalType} is not found in packages list");
        try
        {
            Assembly.Load(dal ?? throw new DalConfigException($"Package {dal} is null"));
        }
        catch (Exception)
        {
            throw new DalConfigException("Failed to load {dal}.dll package");
        }

        Type? type = Type.GetType($"{namespaceDal}.{dal}, {dal}")
            ?? throw new DalConfigException($"{classDal} Dal.{dal} was not found in {dal}.dll");

        return type.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static)?
                   .GetValue(null) as IDal
            ?? throw new DalConfigException($"{classDal} {dal} is not singleton or Instance property not found");
    }
}