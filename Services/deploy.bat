@echo off

docker-compose build

set services=gate hotelcommand hotelquery login offercommand offerquery payment transportcommand transportquery trip

set username=kkucht

for %%i in (%services%) do (
    docker tag %%i %username%/rsww_184543_%%i:latest
    docker push %username%/rsww_184543_%%i:latest
)