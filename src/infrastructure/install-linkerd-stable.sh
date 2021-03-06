#!/bin/bash

curl -sL https://run.linkerd.io/install | sh
export PATH=$PATH:$HOME/.linkerd2/bin
linkerd check --pre
linkerd install | kubectl apply -f -
linkerd check

echo "Installing linkerd viz"
linkerd viz install | kubectl apply -f -
linkerd viz check
linkerd check