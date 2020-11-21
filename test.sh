#!/bin/bash
set -euo pipefail
dotnet add "Tests/Tests.csproj" package coverlet.msbuild
# Do a release build for 'optimized' define
dotnet build -c Release
# Coverage is based on logging/debug mode
dotnet test -c Debug -p:CollectCoverage=true \
                     -p:CoverletOutputFormat=opencover
# Install reportgen if needed
dotnet tool update -g dotnet-reportgenerator-globaltool
# Now run it
reportgenerator -reports:Tests/coverage.opencover.xml \
                -targetdir:CoverageReport
open CoverageReport/index.htm
