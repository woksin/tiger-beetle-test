#! /bin/bash
docker rm $(docker stop $(docker ps -a -q --filter ancestor=ghcr.io/tigerbeetle/tigerbeetle --format="{{.ID}}"))
rm *.tigerbeetle