## Prerequisites

In order to run this project...

1. You must have Docker installed and running on your local machine

2. You must specify the login credentials (i.e. secrets) used in the
   test with the `LOGIN_USERNAME` and `LOGIN_PASSWORD` environment
   variables...

   ```bash
   LOGIN_USERNAME=tomsmith LOGIN_PASSWORD=SuperSecretPassword!
   ```

> These are publicly available values but demonstrate basic secret management

### Optional: Create a `.env` File

You can create a file named `.env` in the project root directory
that contains the required environment variables that will
be used by default by Docker Compose instead of setting them on
the command line...

```bash
LOGIN_USERNAME=tomsmith
LOGIN_PASSWORD=SuperSecretPassword!
```
