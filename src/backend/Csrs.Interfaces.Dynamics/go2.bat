@echo off
setlocal
set NODE_OPTIONS=--max_old_space_size=8192

echo Updating client

autorest ^
 --legacy ^
 --verbose ^
 --input-file=dynamics-swagger.json ^
 --output-folder=.  ^
 --csharp ^
 --use-datetimeoffset ^
 --generate-empty-classes ^
 --override-client-name=DynamicsClient ^
 --namespace=Csrs.Interfaces.Dynamics ^
 --preview  ^
 --add-credentials ^
 --debug
