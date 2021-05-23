#!/bin/bash
npx autorest --csharp --legacy --input-file=swagger.json --namespace=MagicTranslatorProjectMemImporter.MemApi --output-folder=.
