# https://developers.home-assistant.io/docs/add-ons/configuration#add-on-dockerfile
FROM ghcr.io/home-assistant/aarch64-base:latest

RUN chmod a+x rootfs/usr/bin/Temzit.Console

# Copy root filesystem
COPY hass-temzit/rootfs /
