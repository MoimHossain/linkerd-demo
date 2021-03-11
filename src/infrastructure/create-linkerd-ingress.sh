#!/bin/bash

echo "Waiting for 2 mins to give ingress-nginx time to get things worked out..."
sleep 2m
kubectl get all -n ingress-nginx


echo "Creating ingress for Linkerd viz"
kubectl apply -f ./dashboard.yml