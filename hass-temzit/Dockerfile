# https://developers.home-assistant.io/docs/add-ons/configuration#add-on-dockerfile
FROM ghcr.io/home-assistant/aarch64-base:latest

# Copy root filesystem
COPY hass-temzit/rootfs /

RUN /bin/chmod a+x /usr/bin/Temzit.Console
