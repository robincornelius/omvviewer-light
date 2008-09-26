#!/bin/sh

mono Prebuild.exe /target nant
#mono Prebuild.exe /target monodev
mono Prebuild.exe /target vs2005

echo "For fucks sake, now i have to run sed to insert the correct package references"
echo "for nant/gmcs on linux as it prebuild does not do it correctly"

cat omvviewer-light/omvviewer-light.exe.build | sed "s\</references>\</references><pkg-references> <package name=\"gtk-sharp-2.0\"/> </pkg-references>\g" > omvviewer-light/omvviewer-light.exe.build

echo "Done.... good luck!"
