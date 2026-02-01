# Base Image is the builder stage since it is an SDK
ARG BASE_IMAGE=mcr.microsoft.com/dotnet/sdk:10.0
FROM ${BASE_IMAGE} AS sdk

# Dev Environment Stage
FROM sdk AS devenv

# At least install vim (git is already present)
ARG DEVENV_PACKAGES='vim'
RUN apt-get update \
    && apt-get install -y --no-install-recommends ${DEVENV_PACKAGES} \
    && rm -rf /var/lib/apt/lists/*

# ASSUME project source is volume mounted into the container at path /app
WORKDIR /app

# Start devenv in (command line) shell
CMD ["bash"]

# Deploy Stage
FROM sdk AS deploy

# Add a non-root user to run the app
RUN useradd -m -s /bin/bash -c '' deployer && usermod -L deployer
USER deployer

# Copy the source to /app
COPY --chown=deployer . /app/

# Change to the tests subproject directory
WORKDIR /app/SampleCSharpXunitSelenium

# Overridable: Run the tests
CMD ["./script/run", "tests"]
