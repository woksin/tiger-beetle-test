#! /bin/bash
./clean.sh

if [[ $# -eq 0 ]] ; then
    echo "No argument given"
    exit 0
fi

if [ $1 = "single" ]; then
    ./../infra/tigerbeetle format --cluster=0 --replica=0 --replica-count=1 ../environment/0_0.tigerbeetle
    ./../infra/tigerbeetle start --addresses=0.0.0.0:3001 ../environment/0_0.tigerbeetle
elif [ $1 = "cluster" ]; then
    ./../infra/tigerbeetle format --cluster=0 --replica=0 --replica-count=3 ../environment/0_0.tigerbeetle
    ./../infra/tigerbeetle format --cluster=0 --replica=1 --replica-count=3 ../environment/0_1.tigerbeetle
    ./../infra/tigerbeetle format --cluster=0 --replica=2 --replica-count=3 ../environment/0_2.tigerbeetle


    ./../infra/tigerbeetle start --addresses=0.0.0.0:3001,0.0.0.0:3002,0.0.0.0:3003 ../environment/0_0.tigerbeetle &
    p1_pid=$!
    ./../infra/tigerbeetle start --addresses=0.0.0.0:3001,0.0.0.0:3002,0.0.0.0:3003 ../environment/0_1.tigerbeetle &
    p2_pid=$!

    trap onexit INT
    function onexit() {
        kill -9 $p1_pid
        kill -9 $p2_pid
    }
    ./../infra/tigerbeetle start --addresses=0.0.0.0:3001,0.0.0.0:3002,0.0.0.0:3003 ../environment/0_2.tigerbeetle
else
    echo "Wrong input. Must be 'single' or 'cluster"
    exit 1
fi
