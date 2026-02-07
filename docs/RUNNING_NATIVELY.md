## Running the Automated Tests Natively and Environment Variables

Assuming that you have a DOTNET SDK development environment,
the tests either can be run directly by the
[dotnet test runner](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-test)
or by the supplied `run` script.

### Prerequisites

* .NET 10 SDK
* To run the tests using a specific browser requires that browser
  be installed (e.g. to run the tests in the Chrome Browser requires
  Chrome be installed)

1. Restore the project:

   ```bash
   dotnet restore
   ```

### Environment Variables

#### Required Environment Variables and Secrets

For the required secrets and other environment variables,
see the [PREREQUISITES.md](PREREQUISITES.md)

#### Specify Browser

`BROWSER=`...

**Example:**
`BROWSER=chrome`

> If the `BROWSER` environment variable is not provided (i.e. set),
> then the Chrome browser is used from its setting as the default in
> `SampleCSharpXunitSelenium/appsettings.json`

The following browsers are supported and were working on Mac at the time
of this commit:

* `chrome` - Google Chrome (requires Chrome)
* `edge` - Microsoft Edge (requires Edge)
* `firefox` - Mozilla Firefox (requires Firefox)
* `safari` - Apple Safari (local only, requires Safari)

#### Specify Headless

`HEADLESS=`...

> **The `HEADLESS` environment variable is ignored if the `Browser`
> environment variable is not provided (i.e. set)**

**Example:**

`HEADLESS=true`

The `HEADLESS` environment variable expects a boolean string value
(i.e. `true` or `false`).  If not set, it is assumed false.

#### Specify Remote (Container) URL

`REMOTE_URL=`...

Specifying a Remote URL creates a remote browser of type
specified by `Browser` at the specified remote URL

 **Example:**
`REMOTE_URL='http://localhost:4444/wd/hub'`

### Examples of Running the Tests

#### Defaults

```bash
dotnet test
```

```bash
./SampleCSharpXunitSelenium/script/run tests
```

#### Local Browsers

```bash
BROWSER=firefox HEADLESS=true ./SampleCSharpXunitSelenium/script/run tests
```

#### Using the Selenium Standalone Containers

Like the docker compose framework, these tests can be run natively
using the Selenium Standalone containers and the VNC Server
if you want.

For specifics, see the Selenium Standalone Image
[documentation](https://github.com/SeleniumHQ/docker-selenium).

1. Run the Selenium Standalone image with standard port and volume mapping...

   ```bash
   docker run -d -p 4444:4444 -p 5900:5900 -p 7900:7900 -v /dev/shm:/dev/shm selenium/standalone-chromium
   ```

2. If you want, launch the VNC client in app or browser

3. Run the tests specifying the remote Selenium container...

   ```bash
   REMOTE_URL='http://localhost:4444/wd/hub' BROWSER=chrome ./SampleCSharpXunitSelenium/script/run tests
   ```
