#!/bin/bash
set -euo pipefail
dotnet add "Tests/Tests.csproj" package coverlet.msbuild
