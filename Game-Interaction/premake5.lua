local MacOSVersion = MacOSVersion or "14.5"
local OutputDir = OutputDir or "%{cfg.buildcfg}-%{cfg.system}"

project "Game-Interaction"
	kind "ConsoleApp"
	language "C#"

	dotnetframework "4.8"
	dotnetsdk "WindowsDesktop" -- Note: This is the WPF flag

	architecture "x86_64"

	warnings "Extra"

	targetdir ("%{wks.location}/bin/" .. OutputDir .. "/%{prj.name}")
	objdir ("%{wks.location}/bin-int/" .. OutputDir .. "/%{prj.name}")

	files
	{
		"src/Game-Interaction/**.cs",
	}

	-- Deprecated and replaced by dotnetsdk ^
	--flags
	--{
	--	"WPF"
	--}

	filter "system:windows"
		systemversion "latest"
		staticruntime "on"

	filter "system:linux"
		systemversion "latest"
		staticruntime "on"

    filter "system:macosx"
		systemversion(MacOSVersion)
		staticruntime "on"

	filter "configurations:Debug"
		runtime "Debug"
		symbols "on"
		
	filter "configurations:Release"
		runtime "Release"
		optimize "on"

	filter "configurations:Dist"
		runtime "Release"
		optimize "Full"
		linktimeoptimization "on"
