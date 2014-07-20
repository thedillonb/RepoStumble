# RepositoryStumble Private Policy

The following describes the private policy for mobile application RepositoryStumble. For any questions or concerns, please contact me at thedillonb@gmail.com or via twitter @thedillonb. In addition, for any questions concerning operating use, you may search through the source code of Repository Stumble found [here](https://github.com/thedillonb/RepoStumble).

## General Information

Much of the information used within the application does not leave the mobile device. The only information that is reported to an offsite medium is anonymous analytical stats, mentioned below, and error reporting, which is used to build a better application. The GitHub oAuth token the user signs in with never leaves the device.


## Information Gathering and Usage

* When you open RepositoryStumble you will be asked to login to your GitHub account. The login token and any information regarding the user's GitHub account is **not** used in any analytical processing. That data remains on the device at all times.
* When the user opens the app, an analytical indicator will be sent to Parse.com so that number of app uses a day can be calculated.
* When a user enters an Interest an analytical indicator will be sent to Parse.com so that a list of popular interests can be built.
* When the application crashes a stack trace of the applications current state will be sent to sentry.dillonbuchanan.com so that it can be analyzed and corrected.
