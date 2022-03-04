# File Manager Service #
------------

The purpose of the file manager service is to act as an interface to SharePoint.  The required configuration parameters for SharePoint are:

* APIGATEWAY_HOST
* APIGATEWAY_POLICY
* RELYING_PARTY_IDENTIFIER
* AUTHORIZATION_URI - must be a valid url
* RESOURCE - must be a valid url
* SHAREPOINT_USERNAME
* SHAREPOINT_PASSWORD

## Development ##

File manager is a .NET 6.0 application.  As such you can use an IDE such as Visual Studio or VS Code to edit the files.  

## Testing

Postman like testing for gRPC Kreya https://kreya.app/

## Installation ##

Templates for OpenShift deployment are in the `openshift` directory 

