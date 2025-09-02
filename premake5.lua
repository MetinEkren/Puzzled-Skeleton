------------------------------------------------------------------------------
-- Flags
------------------------------------------------------------------------------
MacOSVersion = "14.5"
OutputDir = "%{cfg.buildcfg}-%{cfg.system}"

------------------------------------------------------------------------------
-- Solution
------------------------------------------------------------------------------
workspace "Game-Interaction"
	architecture "x86_64"
	startproject "Game-Interaction"

	configurations
	{
		"Debug",
		"Release",
		"Dist"
	}

	flags
	{
		"MultiProcessorCompile"
	}

include "Game-Interaction"
------------------------------------------------------------------------------