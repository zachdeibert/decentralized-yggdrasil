#!/bin/bash
set -e

cd "$(dirname "$0")/../.."
./gradlew jar
nuget restore -PackagesDirectory packages packages.config
ln -sf ../packages ssl-endpoint/packages
msbuild /p:Configuration=Release decentralized-yggdrasil.sln
cd installers/Mac
cp ../../bin/Release/{{batch-launcher,ssl-endpoint}.exe,{BouncyCastle.Crypto,Newtonsoft.Json}.dll,decentralized-yggdrasil{,.exe,.jar}} "dmg/Decentralized Yggdrasil.app/Contents/Resources"
genisoimage -V "Decentralized Yggdrasil" -D -R -apple -no-pad -o "Decentralized Yggdrasil.dmg" dmg 
