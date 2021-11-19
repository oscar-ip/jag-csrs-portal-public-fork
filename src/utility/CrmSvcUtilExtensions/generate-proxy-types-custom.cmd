SETLOCAL

SET CORETOOLS=%USERPROFILE%\.nuget\packages\microsoft.crmsdk.coretools\9.1.0.92\content\bin\coretools

PATH=%PATH%;%CORETOOLS%
copy/Y %~dp0bin\Debug\net48\CrmSvcUtilExtensions.dll %CORETOOLS%

CrmSvcUtil.exe /url:https://jsb-fams.dev.jag.gov.bc.ca/JSB-FAMS/XRMServices/2011/Organization.svc ^
 /out:%~dp0..\..\backend\Csrs.Api\Models\Dynamics\Models.generated.cs ^
 /il ^
 /namespace:Csrs.Api.Models.Dynamics ^
 /namingservice:CrmSvcUtilExtensions.NamingService,CrmSvcUtilExtensions ^
 /codegenerationservice:CrmSvcUtilExtensions.CodeGenerationService,CrmSvcUtilExtensions ^
 /codewriterfilter:CrmSvcUtilExtensions.CodeWriterFilterService,CrmSvcUtilExtensions


REM PAUSE
