#!/bin/bash

dotnet test --collect "XPlat Code Coverage" -r ./testresults

DATE_WITH_TIME=`date "+%Y%m%d%H%M%S"`
OUTPUT_FOLDER="/testresults/${DATE_WITH_TIME}"

mkdir $OUTPUT_FOLDER

reportgenerator -reports:"./testresults/*/coverage.cobertura.xml" -targetdir:"${OUTPUT_FOLDER}" -reporttypes:Html