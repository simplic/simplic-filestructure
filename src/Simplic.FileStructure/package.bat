del *.nupkg
nuget pack Simplic.FileStructure.csproj -properties Configuration=Debug
nuget push *.nupkg -Source http://simplic.biz:10380/nuget