#!/usr/bin/env bash

docker build \
 -f build.dockerfile \
 --tag proxykit-httpoverrides-build .

docker run --rm --name proxykit-httpoverrides-build \
 -v $PWD:/repo \
 -w /repo \
 -e FEEDZ_API_KEY=$FEEDZ_API_KEY \
 proxykit-httpoverrides-build \
 dotnet run -p build/build.csproj -c Release -- "$@"