#!/bin/bash

case $1 in
  -r|--rebuild)
    docker-compose -f docker-compose.yml down -v
    docker-compose -f docker-compose.yml up --build --force-recreate --abort-on-container-exit --exit-code-from
    ;;
  -u|--up)
    docker-compose -f docker-compose.yml up --abort-on-container-exit --exit-code-from
    ;;
  -d|--down)
    docker-compose -f docker-compose.yml down -v
    ;;
  -D|--dependencies)
    docker-compose -f docker-compose.yml down -v
    docker-compose -f docker-compose.yml up --build --force-recreate --abort-on-container-exit db
    ;;
esac