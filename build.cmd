@ECHO OFF

docker build ^
 -f build.Dockerfile ^
 --tag proxykit-httpoverrides-build .

if errorlevel 1 (
   echo Docker build failed: Exit code is %errorlevel%
   exit /b %errorlevel%
)

docker run --rm -it --name proxykit-httpoverrides-build ^
  -v %cd%:/repo ^
  -w /repo ^
  -e FEEDZ_PROXYKIT_API_KEY=$FEEDZ_PROXYKIT_API_KEY ^
  proxykit-httpoverrides-build ^
  dotnet run -p build/build.csproj -c Release -- %*

if errorlevel 1 (
   echo Docker build failed: Exit code is %errorlevel%
   exit /b %errorlevel%
)