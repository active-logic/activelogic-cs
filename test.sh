#!/bin/bash
set -euo pipefail

test () {
    printf "\n==========\nTest config: $1\n==========\n\n"
    dotnet test -c Debug -p:DefineConstants=$1
    dotnet test -c Release -p:DefineConstants=$1
}

dotnet test -c Debug
dotnet test -c Release

test AL_BEST_PERF
test AL_OPTIMIZE
test AL_STRICT
test AL_THREAD_SAFE
