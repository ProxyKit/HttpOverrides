#!/usr/bin/env bash

set -euo pipefail

docker run --rm --name proxykit-httpoverrides-build \
 -v $PWD:/repo \
 -w /repo \
 -e FEEDZ_PROXYKIT_API_KEY=$FEEDZ_PROXYKIT_API_KEY \
 -e BUILD_NUMBER=$GITHUB_RUN_NUMBER \
 damianh/dotnet-core-lts-sdks:3 \
 dotnet run -p build/build.csproj -c Release -- "$@"