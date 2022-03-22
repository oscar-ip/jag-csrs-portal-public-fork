## Templates to create openshift components related to CSRS frontend deployment

### Command to execute template
1) Fill out the environment variables (as appropriate) in jag-csrs.env files
2) Login to OC using login command
3) Run below command in each env. namespaces like dev/test/prod/tools
 For front end
   ``oc process -f jag-csrs.yml --param-file=jag-csrs.env --ignore-unknown-parameters=true| oc apply -f -``

   For DRY-RUN

   ``oc process -f jag-csrs.yml --param-file=.jag-csrs.env --ignore-unknown-parameters=true| oc apply --dry-run -f -``

 For API
   ``oc process -f jag-csrs-api.yml --param-file=jag-csrs.env --ignore-unknown-parameters=true| oc apply -f -``

  For enabling image pulling from tools space to test space
  (Please make sure that in the image-pull-enabler-config.env file OC_ENV is referring to test )

  ``oc process -f image-pull-enabler.yml --param-file=image-pull-enabler-config.env -n projectlicenseplate-test --ignore-unknown-parameters=true| oc apply -f - ``

 Additional Instructions

  ``cd openshift ``

  ``oc process -f network-policies/standard-policies-to-allow-basic-communication.yml | oc apply -f -  ``

  ``oc process -f csrs-file-manager-ca-configmap.yml | oc apply -f - ``

  ``oc process -f dcUpdaterRole.yml --param-file=.dcUpdaterRole.env --ignore-unknown-parameters=true| oc apply -f -  ``

  ``oc process -f dcUpdaterRoleBinding.yml --param-file=.dcUpdaterRoleBinding.env --ignore-unknown-parameters=true| oc apply -f -  ``

  ``oc process -f  image-pull-enabler.yml --param-file=.image-pull-enabler-config.env --ignore-unknown-parameters=true| oc apply -f -  ``

  ``oc process -f  image-push-enabler.yml --param-file=.image-pusher-config.env --ignore-unknown-parameters=true| oc apply -f -  ``

  ``oc process -f automated-service-cert-generator.yml --param-file=.service-cert-generator.env  --ignore-unknown-parameters=true| oc apply -f -  ``

  ``oc process -f jag-csrs-deploy-filemanager-from-tagged-candidate.yml --param-file=.jag-csrs-file-manager-prod.env  --ignore-unknown-parameters=true| oc apply -f -  ``

   Credit for the base template files go to jag-CDDS repo contributors/Suganth
