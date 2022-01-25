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

   Credit for the base template files go to jag-CDDS repo contributors/Suganth
