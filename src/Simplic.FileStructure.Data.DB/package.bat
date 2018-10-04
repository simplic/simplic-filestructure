del *.nupkg
nuget pack Simplic.FileStructure.Data.DB.csproj -properties Configuration=Debug
nuget push *.nupkg -Source http://simplic.biz:10380/nuget