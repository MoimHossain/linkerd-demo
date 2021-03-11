#!/bin/bash

helm repo add linkerd https://helm.linkerd.io/stable
helm install linkerd/linkerd-viz
linkerd viz install | kubectl apply -f -

linkerd viz check
kubectl apply -f ./dashboard.yml