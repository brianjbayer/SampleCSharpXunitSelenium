# Base Image is the builder stage since it is an SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder

# Dev Environment Stage
FROM builder AS devenv
# At least install vim (git is already present)
RUN apt-get update && apt-get --no-install-recommends -y install vim
# ASSUME project source is volume mounted into the container at path /app
WORKDIR /app
CMD bash

# Deploy Stage
FROM builder AS deploy
# TODO is this needed still - must run as root
RUN sed -i 's/SECLEVEL=2/SECLEVEL=1/g' /etc/ssl/openssl.cnf

# Add a non-root user to run the app
RUN adduser --disabled-password --gecos '' deployer
USER deployer

# Copy the source to /app
COPY --chown=deployer . /app/
# Change to the tests subproject directory
WORKDIR /app/SampleCSharpXunitSelenium
# Overridable: Run the tests
CMD ./script/runtests
