# Build image
FROM node:17.6.0 as build-deps

# set working directory
ENV NODE_ROOT /usr/src/app

RUN mkdir -p /usr/src/app
WORKDIR /usr/src/app

COPY package.json yarn.lock ./

RUN yarn install

COPY . .

RUN yarn run build-prod

# Runtime image
#FROM registry.access.redhat.com/ubi8/nginx-120
FROM nginx:latest

USER 0

#RUN yum -y remove vim-filesystem \
#&& yum -y remove vim-minimal \
#&& yum -y remove bind-libs

RUN apt-get update && apt-get upgrade -y && apt-get clean

USER 1001

COPY --from=build-deps /usr/src/app/src/favicon.ico /usr/share/nginx/html
COPY --from=build-deps /usr/src/app/dist/csrs-portal /usr/share/nginx/html
COPY --from=build-deps /usr/src/app/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 8080

CMD ["sh", "-c", "nginx -g 'daemon off;'"]
