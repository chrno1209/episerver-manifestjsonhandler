$outputDir = ".\package\"
$build = "Release"
$version = "11.0.0"

.\nuget.exe pack ".\src\ManifestJsonHandler\ManifestJsonHandler.v11.csproj" -IncludeReferencedProjects -properties Configuration=$build -Version $version -OutputDirectory $outputDir
