# https://developers.home-assistant.io/docs/add-ons/configuration#add-on-dockerfile
ARG BUILD_FROM
FROM $BUILD_FROM

RUN chmod a+x rootfs/usr/bin/Temzit.Console

# Copy root filesystem
COPY rootfs /
