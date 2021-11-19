SETLOCAL

SET CORETOOLS=%USERPROFILE%\.nuget\packages\microsoft.crmsdk.coretools\9.1.0.92\content\bin\coretools

PATH=%PATH%;%CORETOOLS%
copy/Y %CORETOOLS%\Debug\net48\Csrs.Dynamics.Tools.dll %CORETOOLS%

CrmSvcUtil.exe /url:https://jsb-fams.dev.jag.gov.bc.ca/JSB-FAMS/XRMServices/2011/Organization.svc ^
 /out:%~dp0..\Csrs.Api\Dynamics\GeneratedCode.cs ^
 /il ^
 /namespace:Csrs.Api.Dynamics ^
 /codewriterfilter:Csrs.Tools.CsrsCodeWriterFilterService,Csrs.Dynamics.Tools ^
 /namingservice:Csrs.Tools.CsrsNamingService,Csrs.Dynamics.Tools


PAUSE
