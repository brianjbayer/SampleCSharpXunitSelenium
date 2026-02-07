## Running Using Other Selenium Standalone Containers

You can also run the tests using other Selenium Standalone
containers (such as Firefox and Edge) with the docker compose
framework.

### Prerequisites

Before being able to run this project, you must follow the requirements
in the [PREREQUISITES.md](PREREQUISITES.md)

### To Run Using the Firefox Standalone Container

1. Ensure Docker is running
2. From the project root directory, run the `dockercomposerun`
   script setting the `Browser` and `SELENIUM_IMAGE`
   environment variables to specify Firefox...

   ```bash
   BROWSER=firefox SELENIUM_IMAGE=selenium/standalone-firefox ./SampleCSharpXunitSelenium/script/dockercomposerun
   ```

### To Run Using the Edge Standalone Container (Intel only)

1. Ensure Docker is running
2. From the project root directory, run the `dockercomposerun`
   script setting the `BROWSER` and `SELENIUM_IMAGE`
   environment variables to specify Edge...

   ```bash
   BROWSER=edge SELENIUM_IMAGE=selenium/standalone-edge ./SampleCSharpXunitSelenium/script/dockercomposerun
   ```

   There is no `arm64` Selenium Edge (or Chrome) image. On this machine architecture
   the `chromium` image is used.  Both Chrome and Edge are chromium-based browsers.
