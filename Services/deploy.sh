#!/bin/bash

docker-compose build

services=("gate" "hotelcommand" "hotelquery" "login" "offercommand" "offerquery" "payment" "transportcommand" "transportquery" "trip" "front")

username="kkucht"

for service in "${services[@]}"
do
  docker tag $service $username/rsww_184543_$service:latest
  docker push $username/rsww_184543_$service:latest
  docker rmi $service
  docker rmi $username/rsww_184543_$service:latest
done
