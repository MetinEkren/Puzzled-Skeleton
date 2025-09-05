local OutputDir = OutputDir or "%{cfg.buildcfg}-%{cfg.system}"

project "GameInteraction"
	kind "WindowedApp" -- TODO: WindowedApp
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
		"src/Game-Interaction/**.xaml",
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

    clr "Unsafe"
	framework "4.8"

	filter "files:**.xaml"
		buildaction "Page" -- WPF XAML pages
		--dependentupon(function(f) return f .. ".cs" end)

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
		runtime "Release"
		optimize "Full"
		linktimeoptimization "on"
