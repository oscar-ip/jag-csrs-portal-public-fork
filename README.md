[![Maintainability](https://api.codeclimate.com/v1/badges/1efca555bd2b4852e9b1/maintainability)](https://codeclimate.com/github/bcgov/jag-csrs-portal-public/maintainability) [![Test Coverage](https://api.codeclimate.com/v1/badges/1efca555bd2b4852e9b1/test_coverage)](https://codeclimate.com/github/bcgov/jag-csrs-portal-public/test_coverage)

[![img](https://img.shields.io/badge/Lifecycle-Experimental-339999)](https://github.com/bcgov/repomountie/blob/master/doc/lifecycle-badges.md)  

# jag-csrs-portal-public

Welcome to CSRS Portal Public

## Project Structure

    ├── src                                     # application source files
    |   ├── backend                             # backend apis
    │   └── frontend                            # frontend applications
    │       └── csrs-portal                     # csrs portal
    ├── CONTRIBUTING.md                         #
    ├── LICENSE                                 # Apache License
    └── README.md                               # This file.

## Apps

| Name                | Description                                  | Doc                             |
| ------------------- | -------------------------------------------- | --------------------------------|
| frontend            | all client side applications                 | [README](src/frontend/README.md)|


# Splunk Docker Examples

https://splunk.github.io/docker-splunk/EXAMPLES.html#create-standalone-with-hec

#### Docker

[Download](https://www.docker.com/products/docker-desktop) and install Docker

# Run docker-compose

Copy the `.env.template` to `.env` and then run docker-compose up.
Add the configuration for token and password for splunk.
Default user is `admin`. Password is what is configured in `.env`

```
docker-compose up
```

The frontend app csrs-portal will be accessible in the browser at http://localhost:8080 

To remove services run (all services and networking)
```
docker-compose down
```
