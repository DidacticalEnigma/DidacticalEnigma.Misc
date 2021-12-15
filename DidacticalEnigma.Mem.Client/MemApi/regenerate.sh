#!/bin/bash
npx autorest --csharp --legacy --input-file=swagger.json --namespace=DidacticalEnigma.Mem.Client.MemApi --output-folder=.
