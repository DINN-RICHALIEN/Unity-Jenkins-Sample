using System;
using System.Linq;
using Anonymous.Jenkins;
using UnityEditor;
using UnityEngine;

public class Batch
{
	public static void Build()
	{
		var args = new BatchArguments
		{
			BuildEnviroment = Enum.Parse<EnviromentType>(GetArgument("")),
			BuildPlatform = Enum.Parse<BuildTarget>(GetArgument("")),
			BuildVersion = GetArgument(""),
			BuildVersionCode = Convert.ToInt32(GetArgument("")),
			BuildNumber = Convert.ToInt32(GetArgument("")),

			canABB = Convert.ToBoolean(GetArgument("")),
			useKeystore = Convert.ToBoolean(GetArgument("")),
			KeystorePath = GetArgument(""),
			KeystoreAlias = GetArgument(""),
			KeystorePassword = GetArgument("")
		};

		Application.logMessageReceived += LogMessageReceived;

		BatchBuild.Build(args);

		Application.logMessageReceived -= LogMessageReceived;
	}

	public static void Bundle()
	{
		var args = new BatchArguments
		{
			BuildEnviroment = Enum.Parse<EnviromentType>(GetArgument("")),
			BuildPlatform = Enum.Parse<BuildTarget>(GetArgument("")),
			BuildVersion = GetArgument("")
		};

		Application.logMessageReceived += LogMessageReceived;

		BatchBundle.Build(args);

		Application.logMessageReceived -= LogMessageReceived;
	}

	private static string GetArgument(string name)
	{
		name += "=";

		var args = Environment.GetCommandLineArgs();
		return (from arg in args where arg.Contains(name) select arg.Split("=")[1]).FirstOrDefault();
	}

	private static void LogMessageReceived(string condition, string stackTrace, LogType type)
	{
		switch (type)
		{
			case LogType.Exception:
				Debug.Log($"Build Exception : {stackTrace}\n{condition}");
				break;

			case LogType.Error:
				Debug.Log($"Build Error : {stackTrace}\n{condition}");
				break;

			case LogType.Assert:
			case LogType.Warning:
			case LogType.Log:
			default:
				break;
		}
	}
}