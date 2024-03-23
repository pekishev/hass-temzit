# https://developers.home-assistant.io/docs/add-ons/configuration#add-on-dockerfile
FROM ghcr.io/home-assistant/aarch64-base:latest

RUN /bin/chmod -R 0777 /etc/services.d/temzit && /bin/chmod -R 0777 /usr/bin

# Copy root filesystem
COPY hass-temzit/rootfs /

