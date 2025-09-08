------------------------------------------------------------------------------
-- WPF Compatibility
------------------------------------------------------------------------------
local p = premake
require("vstudio")
local vstudio = p.vstudio
local dotnetbase = vstudio.dotnetbase

-- Override the function that writes the PropertyGroup for C# projects
premake.override(dotnetbase, "targetFrameworkVersion", function(base, cfg)
    -- Call the original behavior (writes <TargetFrameworkVersion>v4.8</TargetFrameworkVersion>)
    base(cfg)

    -- Now inject our custom WPF property automatically
    p.w('<UseWPF>true</UseWPF>')
	--p.w('<TargetFramework>net48</TargetFramework>')
end)

--premake.override(premake.vstudio.cs2005.elements, "csproj", function(base, prj)
--    local xml = base(prj) -- Runs base call
--
--    -- Inject additional system references after base XML is generated
--    -- Only works for old-style csproj
--    p.w('<ItemGroup>')
--    p.w('<Reference Include="PresentationCore" />')
--	p.w('<Reference Include="PresentationFramework" />')
--	p.w('<Reference Include="WindowsBase" />')
--	p.w('<Reference Include="System" />')
--	p.w('<Reference Include="System.Data" />')
--	p.w('<Reference Include="System.CodeDom" />')
--	p.w('<Reference Include="System.Xaml" />')
--    p.w('</ItemGroup>')
--
--    return xml
--end)
------------------------------------------------------------------------------

------------------------------------------------------------------------------
-- Flags
------------------------------------------------------------------------------
OutputDir = "%{cfg.buildcfg}-%{cfg.system}"

------------------------------------------------------------------------------
-- Solution
------------------------------------------------------------------------------
workspace "Puzzled-Skeleton"
	architecture "x64"
	startproject "Puzzled"
	
	configurations
	{
		"Debug",
		"Release",
		"Dist"
	}
	platforms { "x64" } -- Force x64 explicitly

	flags
	{
		"MultiProcessorCompile"
	}

include "Puzzled"
------------------------------------------------------------------------------