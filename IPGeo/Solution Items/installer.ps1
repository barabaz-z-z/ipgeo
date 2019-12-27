$path = Split-Path $PSScriptRoot -Parent
$ipGeoAPIPath = Join-Path $path IPGeo.API
$ipGeoAPIExePath = Join-Path $ipGeoAPIPath "bin\Release\netcoreapp2.1\publish\IPGeo.API.dll"
$ipGeoUpdaterPath = Join-Path $path IPGeo.DatabaseUpdater
$ipGeoUpdaterExePath = Join-Path $ipGeoUpdaterPath "bin\Release\netcoreapp2.1\win7-x64\publish\IPGeo.DatabaseUpdater.exe"

dotnet publish $ipGeoAPIPath --configuration Release 
dotnet publish $ipGeoUpdaterPath --configuration Release 

New-Service -Name IPGeoDatabaseUpdater -BinaryPathName $ipGeoUpdaterExePath -DisplayName "IPGeo Database Updater"
Start-Service -Name IPGeoDatabaseUpdater

$service = Get-Service -Name IPGeoDatabaseUpdater

Write-Host $service.Status

dotnet $ipGeoAPIExePath