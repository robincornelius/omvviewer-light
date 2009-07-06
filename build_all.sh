#!/bin/sh

which nant

if [ $? -ne 0 ]  
then 
    echo "nant not found, please ensure you have the nant built tool available"
    exit -1
fi


which csc

if [ $? -ne 0 ]  
then 
    echo "csc (c sharp compiler) not found, please ensure you have the csc tool available"
    exit -1
fi

which resgen

if [ $? -ne 0 ]  
then 
    echo "resgen (resource compiler) not found, please ensure you have the resgen tool available"
    exit -1
fi


cd libomv
./runprebuild.sh
nant
cd ..
./copy_libs.sh
nant
echo "Output files are in omvviewer-light/bin/"
