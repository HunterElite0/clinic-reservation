#!/bin/bash

cd ../src
dotnet ef database update
cd ../app/publish
dotnet clinic-reservation.dll --environment="development"