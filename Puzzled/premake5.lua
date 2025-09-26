local OutputDir = OutputDir or "%{cfg.buildcfg}-%{cfg.system}"

project "Puzzled"
	kind "ConsoleApp" 
	language "C#"

	dotnetframework "4.8"
	--dotnetframework "net8.0-windows"
	dotnetsdk "WindowsDesktop" -- Note: This is the WPF flag

	architecture "x64"

	warnings "Extra"

	targetdir ("%{wks.location}/bin/" .. OutputDir .. "/%{prj.name}")
	objdir ("%{wks.location}/bin-int/" .. OutputDir .. "/%{prj.name}")

	-- debugdir("%{prj.location}")

	files
	{
		"src/**.cs",
		"src/**.xaml",
	}

	-- Deprecated and replaced by dotnetsdk ^
	--flags
	--{
	--	"WPF"
	--}

	links 
	{
		"PresentationCore",
		"PresentationFramework",
		"WindowsBase",
		"System",
		"System.Data",
		"System.Xaml",
		"System.CodeDom"
	}

	-- Dependencies
	nuget 
	{ 
		"System.Text.Json:9.0.9" 
	}

    clr "Unsafe"
	framework "4.8"

	filter "files:**.xaml"
		buildaction "Page" -- WPF XAML pages

	filter "system:windows"
		systemversion "latest"
		staticruntime "on"

	filter "configurations:Debug"
		runtime "Debug"
		symbols "on"
		
	filter "configurations:Release"
		runtime "Release"
		optimize "on"

	filter "configurations:Dist"
		kind "WindowedApp" 
		runtime "Release"
		optimize "Full"
		linktimeoptimization "on"
