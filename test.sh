#!/bin/bash
set -euo pipefail
dotnet add "Tests/Tests.csproj" package coverlet.msbuild
dotnet test -c Debug -p:CollectCoverage=true \
                     -p:CoverletOutputFormat=opencover
reportgenerator -reports:Tests/coverage.opencover.xml \
                -targetdir:CoverageReport
open CoverageReport/index.htm
