#!/bin/sh

mono Prebuild.exe /target nant
#mono Prebuild.exe /target monodev
mono Prebuild.exe /target vs2005

