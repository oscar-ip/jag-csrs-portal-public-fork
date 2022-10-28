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

FROM nginx:1.23.1

USER 0

RUN mkdir -p /var/cache/nginx/client_temp && \
mkdir -p /var/cache/nginx/proxy_temp && \
mkdir -p /var/cache/nginx/scgi_temp && \
mkdir -p /var/cache/nginx/uwsgi_temp && \
mkdir -p /var/cache/nginx/fastcgi_temp && \
chmod -R a+rwx /var/run && \
chmod -R a+rw- /var/cache/nginx/client_temp && \
chown -R 1001:0 /var/cache/nginx/client_temp && \
chown -R 1001:0 /var/run

USER 1001

COPY --from=build-deps /usr/src/app/src/favicon.ico /usr/share/nginx/html
COPY --from=build-deps /usr/src/app/dist/csrs-portal /usr/share/nginx/html
COPY --from=build-deps /usr/src/app/nginx.conf /opt/app-root/etc/nginx.default.d/default.conf

EXPOSE 8080

CMD ["sh", "-c", "nginx -g 'daemon off;'"]
