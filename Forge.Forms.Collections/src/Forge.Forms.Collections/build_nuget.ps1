$date = Get-Date -Format "yyyyMMdd.HHmm";
dotnet pack -c Release -p:PackageVersion=2.$date --version-suffix manual -o nupkgs --include-symbols --include-source