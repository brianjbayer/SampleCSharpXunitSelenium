# SampleCSharpXunitSelenium

This is an example of Acceptance Test Driven Development (ATDD) using
[C#](https://docs.microsoft.com/en-us/dotnet/csharp/), [xUnit](https://xunit.net/),
[Selenium](https://www.selenium.dev/).

**However, it also provides a somewhat extensible framework that can be reused
by replacing the existing tests.**

These tests show how to use C#-Selenium to verify...
* The ability to login as a user
* That critical elements are on a page

It also demonstrates the basic features of the
C#-Selenium framework and how they can be extended.

## Run Locally or in Containers
This project can be run...
* Locally containerized in two separate Docker containers:
  one containing the tests, the other the browser
* Locally natively running the tests against a local browser
  or a containerized browser

## Contents of this Framework
This framework contains support for...
* Using Selenium Standalone containers eliminating the need
  for locally installed browsers or drivers
* Multiple local browsers with automatic driver management
* Headless execution for those browsers that support it
* Single-command docker-compose framework to run
  the tests or a supplied command
* Native through fully-containerized execution
* Containerized development environment
* Continuous Integration with GitHub Actions vetting
  linting, static security scanning, and functional
  tests
* Basic secrets management using environment variables and
  [GitHub Secrets](https://docs.github.com/en/actions/security-guides/encrypted-secrets)

## To Run the Automated Tests in Docker
The easiest way to run the tests is with the docker-compose
framework using the `dockercomposerun` script.

This will pull the latest docker image of this project and run
the tests against a
[Selenium Standalone](https://github.com/SeleniumHQ/docker-selenium)
container.

You can view the running tests, using the included
Virtual Network Computing (VNC) server.

### Prerequisites
1. You must have Docker installed and running on your local machine.
2. You must specify the login credentials (i.e. secrets) used in the
   test with the `ValidLoginUser` and `ValidLoginPass` environment
   variables...
   ```
   ValidLoginUser=tomsmith ValidLoginPass=SuperSecretPassword!
   ```

#### Optional: Create a `.env` File
You can create a file named `.env` in the project root directory
that contains the required environment variables that will
be used by default by Docker Compose instead of setting them on
the command line...
```
ValidLoginUser=tomsmith
ValidLoginPass=SuperSecretPassword!
```

### To See the Tests Run Using the VNC Server
> Browsers in the containers are not visible in the VNC server
> when running headless

The Selenium Standalone containers used in the docker-compose
framework have an included VNC server for viewing and
debugging the tests.

You can use either a VNC client or a web browser to view the tests.

1. Ensure that you are running the Selenium Standalone containers
   (e.g. in the docker-compose framework)
2. To view the tests... using a web browser, navigate to
   http://localhost:7900/; or to use a VNC server, use
   `vnc://localhost:5900` (On Mac you can simply enter
   this address into a web browser)
3. When prompted for the (default) password, enter `secret`

For more information, see the Selenium Standalone Image
[VNC documentation](https://github.com/SeleniumHQ/docker-selenium#debugging)

### To Run Using the Default Chrome Standalone Container
1. Ensure Docker is running
2. From the project root directory, run the `dockercomposerun`
   script with the defaults...

   ```
   ValidLoginUser=tomsmith ValidLoginPass=SuperSecretPassword! ./SampleCSharpXunitSelenium/script/dockercomposerun
   ```

### To Run Using the Firefox Standalone Container
1. Ensure Docker is running
2. From the project root directory, run the `dockercomposerun`
   script setting the `Browser` and `SELENIUM_IMAGE`
   environment variables to specify Firefox...
   ```
   Browser=firefox SELENIUM_IMAGE=selenium/standalone-firefox ValidLoginUser=tomsmith ValidLoginPass=SuperSecretPassword! ./SampleCSharpXunitSelenium/script/dockercomposerun
   ```

### To Run Using the Edge Standalone Container
1. Ensure Docker is running
2. From the project root directory, run the `dockercomposerun`
   script setting the `Browser` and `SELENIUM_IMAGE`
   environment variables to specify Edge...
   ```
   Browser=edge SELENIUM_IMAGE=selenium/standalone-edge ValidLoginUser=tomsmith ValidLoginPass=SuperSecretPassword! ./SampleCSharpXunitSelenium/script/dockercomposerun
   ```

### To Run the Test Container Interactively (i.e. "Shell In")
1. Ensure Docker is running
2. From the project root directory, run the `dockercomposerun`
   script and supply the bash shell command `bash`...
   ```
   ValidLoginUser=tomsmith ValidLoginPass=SuperSecretPassword! ./SampleCSharpXunitSelenium/script/dockercomposerun bash
   ```
3. Run desired commands in the container
   (e.g. `./script/runtests`)
4. Run the exit command to exit the Test container
   ```
   exit
   ```

## To Run the Automated Tests Natively
Assuming that you have a DOTNET SDK development environment,
the tests either can be run directly by the 
[dotnet test runner](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-test)
or by the supplied `runtests` script.

### Prerequisites
* .NET 6 SDK
* To run the tests using a specific browser requires that browser
  be installed (e.g. to run the tests in the Chrome Browser requires
  Chrome be installed).

1. Restore the project:
   ```
   dotnet restore
   ```

### Environment Variables
#### Required Secrets
`ValidLoginUser=tomsmith`

`ValidLoginPass=SuperSecretPassword!`

**These must be set for the login test to pass.**

> These are publicly available values but demonstrate
> basic secret management

#### Specify Browser
`Browser=`...

**Example:**
`Browser=chrome`

> If the `Browser` environment variable is not provided (i.e. set),
> then the Chrome browser is used from its setting as the default in 
> `SampleCSharpXunitSelenium/appsettings.json`

The following browsers are supported and were working on Mac at the time
of this commit:
* `chrome` - Google Chrome (requires Chrome)
* `edge` - Microsoft Edge (requires Edge)
* `firefox` - Mozilla Firefox (requires Firefox)
* `safari` - Apple Safari (local only, requires Safari)

> This project uses the
> [WebDriverManager.Net](https://github.com/rosolko/WebDriverManager.Net)
> package to automatically download and manage chromedriver, edgedriver, and
> geckodriver (Firefox)

#### Specify Headless
`Headless=`...

> **The `Headless` environment variable is ignored if the `Browser`
> environment variable is not provided (i.e. set)**

**Example:**
`Headless=true`

The `Headless` environment variable expects a boolean string value
(i.e. `true` or `false`).  If not set, it is assumed false.

#### Specify Remote (Container) URL
`Remote_Url=`...

Specifying a Remote URL creates a remote browser of type
specified by `Browser` at the specified remote URL

 **Example:**
`Remote_Url='http://localhost:4444/wd/hub'`

### Examples of Running the Tests
#### Defaults
```
ValidLoginUser=tomsmith ValidLoginPass=SuperSecretPassword! dotnet test
```

```
ValidLoginUser=tomsmith ValidLoginPass=SuperSecretPassword! ./SampleCSharpXunitSelenium/script/runtests
```

#### Local Browsers
```
Browser=firefox Headless=true ValidLoginUser=tomsmith ValidLoginPass=SuperSecretPassword! ./SampleCSharpXunitSelenium/script/runtests
```

#### Using the Selenium Standalone Containers
Like the docker-compose framework, these tests can be run natively
using the Selenium Standalone containers and the VNC Server
if you want.

For specifics, see the Selenium Standalone Image
[documentation](https://github.com/SeleniumHQ/docker-selenium).

1. Run the Selenium Standalone image with standard port and volume mapping...
   ```
   docker run -d -p 4444:4444 -p 5900:5900 -p 7900:7900 -v /dev/shm:/dev/shm selenium/standalone-chrome
   ```
2. If you want, launch the VNC client in app or browser
3. Run the tests specifying the remote Selenium container...
   ```
   Remote_Url='http://localhost:4444/wd/hub' REMOTE_STATUS=${Remote_Url}/status Browser=chrome ValidLoginUser=tomsmith ValidLoginPass=SuperSecretPassword! ./SampleCSharpXunitSelenium/script/runtests
   ```

## Development
This project can be developed using the supplied basic, container-based
development environment which includes `vim` and `git`.

The development environment container volume mounts your local source
code to recognize and persist any changes.

By default the development environment container executes the Debian
`bash` shell providing a command line interface.

> Unlike the deploy container, the current directory in the development
> environment is (assumed to be) the project root directory

### To Develop Using the Container-Based Development Environment
The easiest way to run the containerized development environment is with
the docker-compose framework using the `dockercomposerun` script with the
`-d` (development environment) option...
```
ValidLoginUser=tomsmith ValidLoginPass=SuperSecretPassword! ./SampleCSharpXunitSelenium/script/dockercomposerun -d
```

This will pull and run the latest development environment image of this
project along with the Chrome [Selenium Standalone](https://github.com/SeleniumHQ/docker-selenium)
container.

#### Running Just the Development Environment
To run the development environment on its own in the docker-compose
environment **without a Selenium browser**, use the `-n` option for
no Selenium and the `-d` option for the development environment...
```
ValidLoginUser=tomsmith ValidLoginPass=SuperSecretPassword! ./SampleCSharpXunitSelenium/script/dockercomposerun -n -d
```

#### Building Your Own Development Environment Image
You can also build and run your own development environment image.

1. Build your development environment image specifying the `devenv` build
   stage as the target and supplying a name (tag) for the image.
   ```
   docker build --no-cache --target devenv -t browsertests-dev .
   ```

2. Run your development environment image in the docker-compose
   environment either on its own or with the Selenium Chrome
   (or other browser containers) and specify your development
   environment image with `BROWSERTESTS_IMAGE`
   ```
   BROWSERTESTS_IMAGE=browsertests-dev ValidLoginUser=tomsmith ValidLoginPass=SuperSecretPassword! ./SampleCSharpXunitSelenium/script/dockercomposerun -n -d
   ```

#### Specifying the Source Code Location
To use another directory as the source code for the development
environment, set the `BROWSERTESTS_SRC` environment variable.
For example...
```
BROWSERTESTS_SRC=${PWD} BROWSERTESTS_IMAGE=browsertests-dev ValidLoginUser=tomsmith ValidLoginPass=SuperSecretPassword! ./SampleCSharpXunitSelenium/script/dockercomposerun -d
```

#### Running the Tests, Linting, and Security Scanning
To run the tests, linting, and security scanning in the development
environment like CI, use the appropriate wrapper scripts.

If you are running interactively (command line) in the development
environment...

* To run the **tests**...
  ```
  ./SampleCSharpXunitSelenium/script/runtests
  ```

* To run the **linting**...
  ```
  ./SampleCSharpXunitSelenium/script/runlint
  ```

* To run the dependency **security scan**...
  ```
  ./SampleCSharpXunitSelenium/script/runsecscan
  ```

## Sources and Additional Information
* The [WebDriverManager.Net](https://github.com/rosolko/WebDriverManager.Net)
* The [Selenium Docker Images](https://github.com/SeleniumHQ/docker-selenium)
