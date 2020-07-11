#!/bin/bash
set -euo pipefail
dotnet add "Tests/Tests.csproj" package coverlet.msbuild
dotnet test -c Debug -p:CollectCoverage=true \
                     -p:CoverletOutputFormat=opencover
# Install reportgen if needed
dotnet tool update -g dotnet-reportgenerator-globaltool
# Now run it
reportgenerator -reports:Tests/coverage.opencover.xml \
                -targetdir:CoverageReport
open CoverageReport/index.htm
